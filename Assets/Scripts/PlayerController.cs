using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Movement m_Movement = null;
    //public Camera m_Camera = null;
    public float m_ViewChangeRate = 2.0f;
    public float m_VIEW_LIMIT = 60.0f;
    private float m_ViewRotationX = 0.0f;
    private Rigidbody m_Rigidbody = null;
    private Collider[] m_BoxColliders = new Collider[2];

    public GameObject m_DrivenAnimal = null;
    public AnimalAI m_DrivenAnimalAI = null;


    public GameObject m_Stone = null;
    public float m_Strength = 500.0f;
    public Camera m_MainCamera = null;
    public Camera m_BackCamera = null;
    public GameObject m_VisualBody = null;
    public bool m_OnGround = true;
    SingleTimer m_TimerToStandUp;
    [Tooltip("普通下马的操作时长，轻点下马键一下即大约是0.1s，当点击时长小于该值时能够触发正常下马")]
    public float m_Threshold_NormallyGetDownOperationTime = 0.2f;
    public float m_MaxJumpMagnitude = 12;
    public HPComponent m_HPComponent = null;
    public SingleTimer TIMER_ShakeMainCamera = null;


    // Use this for initialization
    void Start () {
        m_Movement = GetComponent<Movement>();
        //m_Camera = GetComponentInChildren<Camera>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_BoxColliders = GetComponents<Collider>();
        m_TimerToStandUp = new SingleTimer(1.5f, StandUpImmediately);
        m_HPComponent = gameObject.AddComponent<HPComponent>();
    }

    // Update is called once per frame
    void Update () {

        if (m_HPComponent.HP <= 0) return;

        //位移和角度稳定
		if(m_DrivenAnimalAI!=null)
        {
            this.transform.localPosition = Vector3.zero;
            var e = this.transform.localEulerAngles;
            e.x = 0;
            e.z = 0;
            e.y = BasicTools.NormalizeAngleBetween_n180top180(e.y);
            e.y = Mathf.Clamp(e.y, -m_VIEW_LIMIT, m_VIEW_LIMIT);

            this.transform.localEulerAngles = e;
        }

        UpdateMainCameraShake();
        UpdateBackCameraShake();
    }

    public void OnMoveStart()
    {
        if (m_HPComponent.HP <= 0) return;
    }

    public void OnMove(Vector2 padInput)
    {
        if (m_HPComponent.HP <= 0) return;

        if (m_DrivenAnimal!=null)
        {
            m_DrivenAnimalAI.InputLeftPad(padInput);
        }
        else
        {
            m_Movement.MoveDirectionWithLimit(new Vector3(padInput.x, 0, padInput.y));
        }
    }

    public void OnMoveEnd()
    {
        if (m_HPComponent.HP <= 0) return;
        //结束时要清空玩家的移动以及恐龙的移动
        if (m_DrivenAnimal != null)
        {
            m_DrivenAnimalAI.InputLeftPad(Vector2.zero);
        }
        else
        {
            m_Movement.MoveDirectionWithLimit(Vector3.zero);
        }
        
    }

    public void OnViewChange(Vector2 padInput)
    {
        if (m_HPComponent.HP <= 0) return;
        //m_ViewRotationX = Mathf.Clamp(m_ViewRotationX - padInput.y, -VIEW_LIMIT, VIEW_LIMIT);


        //相机本地X角即Pitch角//上下允许
        var eulerX = BasicTools.NormalizeAngleBetween_n180top180(m_MainCamera.gameObject.transform.localEulerAngles.x);
        if (m_MainCamera.enabled && eulerX - padInput.y < m_VIEW_LIMIT && eulerX - padInput.y>-m_VIEW_LIMIT)
        {
            m_MainCamera.gameObject.transform.Rotate(new Vector3(-padInput.y, 0, 0), Space.Self);
        }
        
        //Player本地Y角即Yaw角
        var eulerY = BasicTools.NormalizeAngleBetween_n180top180(gameObject.transform.localEulerAngles.y);
        if (m_DrivenAnimal == null || eulerY + padInput.x < m_VIEW_LIMIT && eulerY + padInput.x > -m_VIEW_LIMIT)
        {
            transform.Rotate(new Vector3(0, padInput.x, 0), Space.Self);
        }

        //如果有骑行
        if (m_DrivenAnimal != null)
        {
            m_DrivenAnimalAI.InputRightPad(padInput);
        }
    }

    protected float m_LastB1StartTime = 0.0f;
    public void OnB1Down()
    {
        if (m_HPComponent.HP <= 0) return;
        m_LastB1StartTime = Time.time;
        if(m_DrivenAnimal)
        {
            m_DrivenAnimalAI.m_AnimCtrl.m_PseudoInput.Space = true;
        }
    }
    public void OnB1Pressed()
    {

    }
    public void OnB1Up()
    {
        if (m_HPComponent.HP <= 0) return;

        var deltaT = Time.time - m_LastB1StartTime;
        deltaT = Mathf.Clamp01(deltaT);
        if(deltaT<m_Threshold_NormallyGetDownOperationTime && m_DrivenAnimalAI!=null )//正常下马逻辑
        {
            var offset = m_DrivenAnimalAI.m_AnimCtrl.m_GetDownOffset;
            EndDrive();
            this.transform.Translate(offset);
            var e = this.transform.localEulerAngles;
            e.x = 0;
            e.z = 0;
            this.transform.localEulerAngles = e;
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        }
        else
        {
            Jump(deltaT * m_MaxJumpMagnitude);
        }
    }

    public void ThrowStone(float strength)
    {
        var stone = Instantiate(m_Stone, transform.position + transform.forward * 1.0f, transform.rotation);
        var rigidbody0 = stone.GetComponent<Rigidbody>();
        rigidbody0.AddRelativeForce(new Vector3(0, 1, 1) * strength);
    }

    public void DriveAnimal(GameObject drivenAnimal)
    {
        this.m_DrivenAnimal = drivenAnimal;
        m_DrivenAnimalAI = drivenAnimal.GetComponent<AnimalAI>();
        m_DrivenAnimalAI.StartDrivenBy(gameObject);

        //Transform关系改动
        GameObject sitPlace = m_DrivenAnimalAI.m_SitPlace;
        
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        BasicTools.Assert(m_BoxColliders[0].isTrigger == false);
        m_BoxColliders[0].enabled = false;
        //m_BoxColliders[1].enabled = false;

        this.transform.parent = sitPlace.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;

        m_OnGround = false;
        ChangeToBackCamera();
    }

    public void Jump(float force)
    {
        EndDrive();
        m_Rigidbody.AddRelativeForce(new Vector3(0, force * 1.5f, force), ForceMode.VelocityChange);
    }

    public void FellDownFromAnimal(float xOffset)
    {
        EndDrive();
        m_Rigidbody.AddRelativeForce(new Vector3(xOffset, 0, 0) * -10.0f, ForceMode.VelocityChange);
        m_Rigidbody.AddRelativeTorque(new Vector3(0, 0, -xOffset)*10);
    }

    public void EndDrive()
    {
        if(m_DrivenAnimalAI!=null)
        {
            m_DrivenAnimalAI.EndBeingDriven();

            m_Rigidbody.isKinematic = false;
            transform.parent = null;

            m_BoxColliders[0].enabled = true;
            m_BoxColliders[1].enabled = true;

            m_DrivenAnimalAI = null;
            m_DrivenAnimal = null;

            ChangeToMainCamera();
        }
    }

    private const float m_ShakeFrequency = 9.1f;
    private const float m_ShakeMagnitude = 0.2f;
    private bool m_IsMainCameraShaking = false;
    public void UpdateMainCameraShake()
    {
        if(m_MainCamera.enabled && m_IsMainCameraShaking)
        {
            m_MainCamera.transform.localPosition = new UnityEngine.Vector3(Mathf.Sin(2 * 3.14f * m_ShakeFrequency * Time.time),0 , 0) * m_ShakeMagnitude;
        }
    }
    public void SetMainCameraShake(float duration)
    {
        m_IsMainCameraShaking = true;
        if(TIMER_ShakeMainCamera==null)
        {
            TIMER_ShakeMainCamera = new SingleTimer(duration, (o) => {
                m_IsMainCameraShaking = false;
                m_MainCamera.transform.localPosition = Vector3.zero;
            });
        }
        TIMER_ShakeMainCamera.Interval = duration;
        TimerManager.Instance.StartTimer(TIMER_ShakeMainCamera);
    }



    public void UpdateBackCameraShake()
    {
        //后视处理
        if (m_BackCamera.enabled && m_DrivenAnimalAI !=null)
        {
            var backCameraTargetPosition = m_DrivenAnimalAI.m_AnimCtrl.m_BackCameraTargetPosition;
            if (Vector3.Distance(m_BackCamera.transform.localPosition, backCameraTargetPosition) < 0.1f)
            {
                m_BackCamera.transform.localPosition = backCameraTargetPosition;
            }
            else
            {
                m_BackCamera.transform.localPosition = Vector3.Lerp(m_BackCamera.transform.localPosition, backCameraTargetPosition, 0.5f);
            }

            //视角平滑化
            var targetVisualBodyLocEuler = m_DrivenAnimalAI.m_AnimCtrl.m_VisualBodyInertiaTargetLocEuler;
            if(Vector3.Distance(m_VisualBody.transform.localEulerAngles, targetVisualBodyLocEuler)<1.0f)
            {
                m_VisualBody.transform.localEulerAngles = targetVisualBodyLocEuler;
            }
            else
            {
                var originLocEuler = m_VisualBody.transform.localEulerAngles;
                originLocEuler.x = BasicTools.NormalizeAngleBetween_n180top180(originLocEuler.x);
                originLocEuler.y = BasicTools.NormalizeAngleBetween_n180top180(originLocEuler.y);
                originLocEuler.z = BasicTools.NormalizeAngleBetween_n180top180(originLocEuler.z);
                m_VisualBody.transform.localEulerAngles = Vector3.Lerp(originLocEuler, targetVisualBodyLocEuler, 0.5f);
            }
        }
    }

    public void ChangeToMainCamera()
    {
        m_VisualBody.SetActive(false);
        m_BackCamera.enabled = false;
        m_MainCamera.enabled = true;

        m_BackCamera.transform.localEulerAngles = Vector3.zero;
        m_BackCamera.transform.localPosition = Vector3.zero;
    }
    public void ChangeToBackCamera()
    {
        m_VisualBody.SetActive(true);
        
        m_BackCamera.enabled = true;
        m_MainCamera.enabled = false;

        m_BackCamera.transform.localEulerAngles = Vector3.zero;
        m_BackCamera.transform.localPosition = Vector3.zero;
    }

    public void StandUpImmediately(params object [] Params)
    {
        if (m_OnGround) return;

        m_OnGround = true;
        transform.position = transform.position+new Vector3(0,1.5f,0);
        transform.localEulerAngles = Vector3.zero;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.name == "Ground" )
        {
            if(m_Rigidbody.constraints == RigidbodyConstraints.None)
            {
                TimerManager.Instance.StartTimer(m_TimerToStandUp);
            }
        }
    }
}
