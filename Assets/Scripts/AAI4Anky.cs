using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAI4Anky : AnimalAI {

    public override bool AITreeNode_BeDriven()
    {
        base.AITreeNode_BeDriven();

/*
 * if(m_HuntingMode)
        {
            AITask_Hunt(m_PawnSensor.m_SensitiveObjectWithAutoClear);
        }*/
        return true;
    }

    public override bool AITreeNode_Alert()
    {
        base.AITreeNode_Alert();

        //3.0. 朝向警惕方向
        if (m_AINodeStates[30] != AINodeState.AINS_Finish)
        {
            m_AINodeStates[30] = AITask_FaceTo(m_PositionToAlert);
        }
        //注：3.0已完成
        //3.1. 静候两秒
        else if (m_AINodeStates[31] != AINodeState.AINS_Finish)
        {
            if (m_AINodeStates[31] == AINodeState.AINS_NotStarted)
            {
                m_TimeToWait = Time.time + 2.0f;
            }
            m_AINodeStates[31] = AITask_WaitUntil(m_TimeToWait);
        }
        //注：3.1已完成
        //3.2. 前进一段距离
        else if (m_AINodeStates[32] != AINodeState.AINS_Finish)
        {
            if (m_AINodeStates[32] == AINodeState.AINS_NotStarted)
            {
                m_TargetLocation = transform.position + transform.forward * 5.0f;
            }
            m_AINodeStates[32] = AITask_MoveHorizontallyTo(m_TargetLocation);
        }
        //注：3.2已完成，即所有任务完成
        else
        {
            m_AINodeStates[3] = AINodeState.AINS_Finish;
        }

        return false;
    }


    public bool m_IsOffensiveType_ElseDefensiveType = true;
    //占用41，42
    public override bool AITreeNode_SenseHumans()
    {
        base.AITreeNode_SenseHumans();

        m_PawnSensor.m_SensitiveObjectWithAutoClear = m_SensedHumanPlayer;
        if (m_SensedHumanPlayer!=null)
        {
            if(m_IsOffensiveType_ElseDefensiveType)
            {
                m_Attacker = m_SensedHumanPlayer;
                m_AINodeStates[41] = AINodeState.AINS_Finish;
            }
            else
            {
                m_AINodeStates[41] = AITask_EscapeFrom(m_SensedHumanPlayer, ref m_AINodeStates[41], ref m_AINodeStates[42]);
            }   
        }
        return true;
    }


    //占用65状态
    //闲暇时可以做的事有E、1、2、3、4、5、MoveToSomePlace、waitfor
    public override bool AITreeNode_Alone()
    {
        base.AITreeNode_Alone();


        if(Time.time>m_NextDesideTimeForAloneState && m_AINodeStates[65] != AINodeState.AINS_Doing )
        {
            int ran = Random.Range(0, 100);
            if (ran < 10)
            {
                m_AnimCtrl.m_PseudoInput.EForGrowl = true;
                Debug.Log("ran = e");
                m_NextDesideTimeForAloneState = Time.time + Random.Range(3, 6);
            }
            else if (ran < 20)
            {
                m_ContinuouslyInputAlphaX = 1;
                Debug.Log("ran = 1");
                m_NextDesideTimeForAloneState = Time.time + Random.Range(2, 4);
            }
            else if (ran < 30)
            {
                m_ContinuouslyInputAlphaX = 2;
                Debug.Log("ran =2 ");
                m_NextDesideTimeForAloneState = Time.time + Random.Range(3, 6);
            }
            else if (ran < 40)
            {
                m_ContinuouslyInputAlphaX = 3;//Eat
                Debug.Log("ran = 3");
                m_NextDesideTimeForAloneState = Time.time + Random.Range(8, 13);
            }
            else if (ran < 50)
            {
                m_ContinuouslyInputAlphaX = 4;//Drink
                Debug.Log("ran = 4");
                m_NextDesideTimeForAloneState = Time.time + Random.Range(6, 13);
            }
            else if (ran < 60)
            {
                m_ContinuouslyInputAlphaX = 5;//Sit|Sleep
                m_NextDesideTimeForAloneState = Time.time + Random.Range(14, 34);
                Debug.Log("ran = 5");
            }
            else //if (ran < 70)
            {
                m_ContinuouslyInputAlphaX = 0;
                m_RandomDestinationForAlone = transform.position + new Vector3(Random.Range(0, 15), 0, Random.Range(0, 15));
                m_AINodeStates[65] = AINodeState.AINS_Doing;        
                Debug.Log("ran = " + m_RandomDestinationForAlone);
            }
        }
        m_AnimCtrl.m_PseudoInput.AlphaX = m_ContinuouslyInputAlphaX;
        if(m_AINodeStates[65] == AINodeState.AINS_Doing)
        m_AINodeStates[65] = AITask_MoveHorizontallyTo(m_RandomDestinationForAlone);

        return false;
    }


}
