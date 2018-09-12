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

    public float m_SensitiveColdingDownTime = 5.0f;
    private GameObject m_SensitiveObjectPrivate = null;
    private float m_LastSetSensitiveObjectTime = -210001;
    public GameObject m_SensitiveObjectWithAutoClear
    {
        get
        {
            if (Time.time - m_LastSetSensitiveObjectTime > m_SensitiveColdingDownTime)
                m_SensitiveObjectPrivate = null;
            return m_SensitiveObjectPrivate;
        }
        set
        {
            m_SensitiveObjectPrivate = value;
            m_LastSetSensitiveObjectTime = Time.time;
        }
    }
    //敏感最大距离：当存在敏感对象的时候，保持察觉得到的距离
    public float m_SensitiveMaxDistance = 50.0f;


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

        TimerManager.Instance.SetNewTimer(0.25f, UpdateSensedPawns, true, true);
    }

    public void UpdateSensedPawns(params object [] Params)
    {
        Watch();

        if (m_SensitiveObjectWithAutoClear != null && Vector3.Distance(m_SensitiveObjectWithAutoClear.transform.position, transform.position) < m_SensitiveMaxDistance)
        {
            if (!PawnsSensed.Contains(m_SensitiveObjectWithAutoClear))
            {
                PawnsSensed.Add(m_SensitiveObjectWithAutoClear);
            }
        }
        
    }

	// Update is called once per frame
	void Update ()
    {
    }

    public void Watch()
    {
        List<GameObject> PawnsSensed = new List<GameObject>();

        var senseAreaRaius = m_SenseArea.radius * transform.localScale.x;
        Collider[] cols = Physics.OverlapSphere(transform.position, senseAreaRaius);
        foreach(var c in cols)
        {
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

            //在视线范围内的加入
            Vector3 _localPosition = m_Light.transform.InverseTransformPoint(c.transform.position) * m_Light.transform.lossyScale.x;

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

    private GameObject m_CacheSensedHuman = null;
    public GameObject SensedHuman
    {
        get
        {
            if (m_CacheSensedHuman != null && PawnsSensed.Contains(m_CacheSensedHuman)) return m_CacheSensedHuman;
            for (var i = 0; i < PawnsSensed.Count; i++)
            {
                if (PawnsSensed[i].CompareTag("Player"))
                { m_CacheSensedHuman = PawnsSensed[i]; return m_CacheSensedHuman; }
            }
            m_CacheSensedHuman = null;
            return null;
        }
    }

    private GameObject m_CacheMainSensedDinosaur = null;
    public GameObject MainSensedDinosaur
    {
        get
        {
            if (m_CacheMainSensedDinosaur != null && PawnsSensed.Contains(m_CacheMainSensedDinosaur)) return m_CacheMainSensedDinosaur;
            for (var i = 0; i < PawnsSensed.Count; i++)
            {
                if (!PawnsSensed[i].CompareTag("Player") && PawnsSensed[i].GetComponent<AnimalAI>() != null)
                {
                    m_CacheMainSensedDinosaur = PawnsSensed[i];
                    return m_CacheMainSensedDinosaur;
                }
            }
            m_CacheMainSensedDinosaur = null;
            return m_CacheMainSensedDinosaur;
        }
    }
}
