using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 视觉：锥形范围内视野，要求在Head位置下有一个SpotLight
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class PawnSensor : MonoBehaviour {
    public GameObject m_Head = null;

    //请确保SpotLight的方向x朝向龙体右侧，y朝向龙体正上方
    public Light m_Light = null;

    //感受区，恐龙只能在该感受区中感受物体
    public SphereCollider m_SenseArea = null;
    //视觉距离
    public float m_VisionRange = 10.0f;
    //视觉角度（从视野左边缘到视野右边缘的张角）
    public float m_VisionAngle = 80.0f;

    public List<GameObject> PawnsSensed = new List<GameObject>();

    public MeshCollider m_MeshCollider = null;
    public AnimalAI m_AnimalAI = null;

	// Use this for initialization
	void Start () {
        m_Light = m_Head.GetComponentInChildren<Light>();
        BasicTools.Assert(m_Light != null && m_Light.type == LightType.Spot);
        m_VisionRange = m_Light.range;
        m_VisionAngle = m_Light.spotAngle;
        m_SenseArea = GetComponent<SphereCollider>();
        m_MeshCollider = GetComponent<MeshCollider>();
        m_AnimalAI = GetComponent<AnimalAI>();


        EventManager.Register(EMessageID.Msg_Noise, OnSenseNoise);

	}
	
	// Update is called once per frame
	void Update () {

        Watch();
	}

    public void Watch()
    {
        List<GameObject> PawnsSensed = new List<GameObject>();

        var senseAreaRaius = m_SenseArea.radius * transform.localScale.x;
        Collider[] cols = Physics.OverlapSphere(transform.position, senseAreaRaius);
        foreach(var c in cols)
        {
            //在视线范围内的加入
            Vector3 _localPosition = m_Light.transform.worldToLocalMatrix * c.transform.position;

            //将自己排除
            if (c == m_SenseArea) continue;
            if (c == m_MeshCollider) continue;

            //将地形碰撞排除
            if (c.gameObject == GameManager.Instance.Ground && GameManager.Instance.Ground!=null) continue;
            if (c.gameObject.transform.parent != null && c.gameObject.transform.parent.gameObject == GameManager.Instance.Ground) continue;
            
            //如果在之前已经察觉了该物体，则继续认定为已察觉
            if (this.PawnsSensed.Contains( c.gameObject) && !PawnsSensed.Contains(c.gameObject))
            {
                PawnsSensed.Add(c.gameObject);
                continue;
            }
            
            if (_localPosition.z>0 
                && _localPosition.z<m_VisionRange 
                && Mathf.Tan(m_VisionAngle/2.0f * Mathf.Deg2Rad)>Mathf.Abs(_localPosition.x/_localPosition.z))
            {
                if(!PawnsSensed.Contains(c.gameObject))
                {
                    PawnsSensed.Add(c.gameObject);
                    Debug.Log("看见你了");
                    continue;
                }
            }
        }

        this.PawnsSensed = PawnsSensed;
        
        string s = "sensedpawns:";
        foreach(var v in this.PawnsSensed)
        {
            s += v.name + ",";
        }
        Logger.Instance.LogInfoToScreen(s, (int)EHandles.EHandle_SensedPawns, 3);
    }

    /// <summary>
    /// 碰撞时产生
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            Debug.Log("有人打我");

            //产生噪音事件（广播性质）
            EventManager.Send(new Msg(EMessageID.Msg_Noise, collision.gameObject.transform.position, ENoiseType.ENoise_Sound, 10));

            //本地被攻击响应
            m_AnimalAI.IsAlert = true;

            //警戒位置
            var alertPosition = (collision.transform.position - transform.position).normalized * 3.0f + transform.position;
            Instantiate(Logger.Instance.DebugShowPoint, alertPosition, Quaternion.identity);
            m_AnimalAI.m_PositionToAlert = alertPosition;
        }
    }

    public void OnSenseNoise(Msg msg)
    {

    }
}
