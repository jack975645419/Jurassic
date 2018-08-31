using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Movement m_Movement = null;
    public Camera m_Camera = null;
    public float m_ViewChangeRate = 2.0f;
    public float m_VIEW_LIMIT = 60.0f;
    private float m_ViewRotationX = 0.0f;
    private Rigidbody m_Rigidbody = null;
    private BoxCollider[] m_BoxColliders = new BoxCollider[2];

    public GameObject m_DrivenAnimal = null;
    public AnimalAI m_DrivenAnimalAI = null;
    public AnimalAI2 m_DrivenAnimalAI2 = null;


    public GameObject m_Stone = null;
    public float m_Strength = 500.0f;

	// Use this for initialization
	void Start () {
        m_Movement = GetComponent<Movement>();
        m_Camera = GetComponentInChildren<Camera>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_BoxColliders = GetComponents<BoxCollider>();

	}
	
	// Update is called once per frame
	void Update () {
		
        

	}

    public void OnMoveStart()
    {
        
    }

    public void OnMove(Vector2 padInput)
    {
        if(m_DrivenAnimal!=null)
        {
            m_DrivenAnimalAI.OnMove(padInput);
            
            this.transform.localPosition = Vector3.zero;

        }
        else
        {
            m_Movement.MoveDirectionWithLimit(new Vector3(padInput.x, 0, padInput.y));
        }
    }

    public void OnMoveEnd()
    {
        //结束时要清空玩家的移动以及恐龙的移动
        if (m_DrivenAnimal != null)
        {
            m_DrivenAnimalAI.OnMove(Vector2.zero);

            var e = this.transform.localEulerAngles;
            e.x = 0;
            e.z = 0;
            this.transform.localEulerAngles = e;
        }
        else
        {
            m_Movement.MoveDirectionWithLimit(Vector3.zero);
        }
        
    }

    public void OnViewChange(Vector2 padInput)
    {
        //m_ViewRotationX = Mathf.Clamp(m_ViewRotationX - padInput.y, -VIEW_LIMIT, VIEW_LIMIT);

        
        //相机本地X角即Pitch角
        var eulerX = BasicTools.NormalizeAngleBetween_n180top180(m_Camera.gameObject.transform.localEulerAngles.x);
        //上下允许
        if (eulerX-padInput.y < m_VIEW_LIMIT && eulerX - padInput.y>-m_VIEW_LIMIT)
        {
            m_Camera.gameObject.transform.Rotate(new Vector3(-padInput.y, 0, 0), Space.Self);
        }


        Debug.Log(gameObject.transform.localEulerAngles.ToString());
        //Player本地Y角即Yaw角
        var eulerY = BasicTools.NormalizeAngleBetween_n180top180(gameObject.transform.localEulerAngles.y);
        //Debug.Log("eY" + eulerY);
        
        if (m_DrivenAnimal == null || eulerY + padInput.x < m_VIEW_LIMIT && eulerY + padInput.x > -m_VIEW_LIMIT)
        {

            transform.Rotate(new Vector3(0, padInput.x, 0), Space.Self);
        }

        //如果有骑行
        if (m_DrivenAnimal != null)
        {
            m_DrivenAnimalAI.OnViewChange(padInput);
        }
    }

    protected float m_LastB1StartTime = 0.0f;
    public void OnB1Down()
    {
        m_LastB1StartTime = Time.time;
    }
    public void OnB1Pressed()
    { }
    public void OnB1Up()
    {
        var deltaT = Time.time - m_LastB1StartTime;
        deltaT = Mathf.Clamp01(deltaT);

        var stone = Instantiate(m_Stone, transform.position + transform.forward*1.0f, transform.rotation);
        var rigidbody0 = stone.GetComponent<Rigidbody>();
        rigidbody0.AddRelativeForce(new Vector3(0, 1, 1) * m_Strength * deltaT);
        
    }




    public void DriveAnimal(GameObject drivenAnimal)
    {
        this.m_DrivenAnimal = drivenAnimal;
        //m_DrivenAnimalAI2 = drivenAnimal.GetComponent<AnimalAI2>();
        m_DrivenAnimalAI = drivenAnimal.GetComponent<AnimalAI>();
        //m_DrivenAnimalAI2.
        m_DrivenAnimalAI.StartDrivenBy(gameObject);

        //Transform关系改动
        GameObject sitPlace = m_DrivenAnimalAI.m_SitPlace;
        
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        BasicTools.Assert(m_BoxColliders[0].isTrigger == false);
        m_BoxColliders[0].enabled = false;
        m_BoxColliders[1].enabled = false;

        this.transform.parent = sitPlace.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
    }
}
