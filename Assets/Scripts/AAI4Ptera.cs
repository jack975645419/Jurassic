using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAI4Ptera : AnimalAI {

    ptera_cs m_AnimCtrlAsPtera = null;

    public override void Start()
    {
        base.Start();
        m_AnimCtrlAsPtera = m_AnimCtrl as ptera_cs;
    }

    //占用 65 66 67
    public override bool AITreeNode_Alone()
    {
        base.AITreeNode_Alone();
        if(Time.time>m_NextDesideTimeForAloneState)
        {
            int ran = Random.Range(0, 100);
            if(ran<10)
            {
                m_AnimCtrl.m_PseudoInput.EForGrowl = true;
                Debug.Log("ran = e");
                m_NextDesideTimeForAloneState = Time.time + Random.Range(3, 6);
            }
            else if (ran < 20)
            {
                m_ContinuouslyInputAlphaX = 1;//idleA
                Debug.Log("ran = 1");
                m_NextDesideTimeForAloneState = Time.time + Random.Range(2, 4);
            }
            else if (ran < 30)
            {
                m_ContinuouslyInputAlphaX = 2;//idleB
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
            else
            {
                m_ContinuouslyInputAlphaX = 0;
                m_RandomDestinationForAlone = transform.position + new Vector3(Random.Range(40, 70), 0, Random.Range(40, 70));
                m_AINodeStates[65] = AINodeState.AINS_Doing;
                Debug.Log("飞往" + m_RandomDestinationForAlone);
            }
        }
        m_AnimCtrl.m_PseudoInput.AlphaX = m_ContinuouslyInputAlphaX;
        if (m_AINodeStates[65] == AINodeState.AINS_Doing)
            m_AINodeStates[65] = AITask_FlyTo(m_RandomDestinationForAlone, ref m_AINodeStates[66], ref m_AINodeStates[67]);

        return true;
    }

    //占用 飞行阶段/
    public AINodeState AITask_FlyTo(Vector3 des, ref AINodeState flyStage, ref AINodeState landStage)
    {
        var dist = BasicTools.GetHorizontalDistance(transform.position, des);

        if (flyStage == AINodeState.AINS_Finish && landStage == AINodeState.AINS_Finish) return AINodeState.AINS_Finish;

        //当距离较远时飞行
        if( flyStage!= AINodeState.AINS_Finish && dist>10f)
        {
            flyStage = AINodeState.AINS_Doing;
            AITask_FaceTo(des);
            m_AnimCtrl.m_PseudoInput.W = true;
        }
        if( landStage== AINodeState.AINS_Doing || dist<=10f)
        {
            flyStage = AINodeState.AINS_Finish;
            landStage = AINodeState.AINS_Doing;
            m_AnimCtrl.m_PseudoInput.S = true;

            if(m_AnimCtrlAsPtera.IsGround)
            {
                landStage = AINodeState.AINS_Finish;
                return AINodeState.AINS_Finish;
            }
        }
        return AINodeState.AINS_Doing;
    } 
}
