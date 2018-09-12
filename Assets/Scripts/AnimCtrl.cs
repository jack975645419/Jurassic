using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl : MonoBehaviour {
    public PseudoInput m_PseudoInput = new PseudoInput();
    public PseudoInput m_ModifiedInput = null;
    public AnimalAI m_AnimalAI = null;
    public float velocity;
    [Tooltip("最大体力值")]
    public float m_MaxPhysicalPower = 100;
    public float m_PhysicalPower = 100;
    [Tooltip("跑步的体力消耗率")]
    public float m_RunConsumePhysicalPowerRate = 1.0f;
    [Tooltip("不跑步时的体力恢复率")]
    public float m_RecoverPhysicalPowerRate = 0.5f;
    [Tooltip("体力充沛临界值。当体力超过该值时，能够以全速奔跑，当体力小于该值时，跑步速度是（体力值/体力充沛临界值）*最大速度")]
    public float m_MaxSpeedPhysicalPowerThreshold = 50f;
    [Tooltip("强制休息时间。当恐龙累了时，需要休息的时长，休息完毕后将会获得该时长*消耗率的体力")]
    public float m_TakeRestTime = 20.0f;
    public Vector3 m_GetDownOffset = new Vector3(-2.0f, 0.0f, 0.0f);
    public float m_RunSpeedScale
    {
        get
        {
            return Mathf.Max(0.4f, m_PhysicalPower > m_MaxSpeedPhysicalPowerThreshold ? 1 : (m_PhysicalPower / m_MaxSpeedPhysicalPowerThreshold));
        }
    }
    public const string PARANAME_RunSpeedScale = "RunSpeedScale";
    public float m_WalkThresholdPhysicalPower
    {
        get
        {
            return m_MaxSpeedPhysicalPowerThreshold * 0.4f;
        }
    }


    // Use this for initialization
    public virtual void Start () {
        m_AnimalAI = GetComponent<AnimalAI>();
        m_PhysicalPower = m_MaxPhysicalPower;
	}

    // Update is called once per frame
    public virtual void Update () {
        PreProcessPseudoInput();
        UpdateBackCameraShake();
    }

    public virtual void PreProcessPseudoInput()
    {
        m_ModifiedInput = m_PseudoInput.GetCopy();
    }



    public Vector3 m_BackCameraTargetPosition;
    public Vector3 m_VisualBodyInertiaTargetLocEuler;
    public virtual void UpdateBackCameraShake()
    {
        //if (m_AnimalAI.m_HuntingMode) return;

        m_BackCameraTargetPosition = new Vector3(m_PseudoInput.LeftPad.x, 0, -m_PseudoInput.LeftPad.y) * 2;
        m_VisualBodyInertiaTargetLocEuler = new Vector3(-m_PseudoInput.LeftPad.y, 0, m_PseudoInput.LeftPad.x) * 20;
        
    }
}
