using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnkyStandsAnimBehaviour : StateMachineBehaviour {
    private Animator m_CachedAnimator = null;
    private const string m_Str_DetailClipID = "DetailClipID";
    private const string m_Str_ConstMaxOfDetailClips = "ConstMaxOfDetailClips";
    private const string m_Str_MinTimeOfDetailClipsInteval = "MinTimeOfDetailClipsInteval";
    private const string m_Str_MaxTimeOfDetailClipsInteval = "MaxTimeOfDetailClipsInteval";
    //消息号
    private enum EMessageID
    {
        Msg_AnimShouldTransform
    }

    //消息报文
    private class Msg
    {
        public EMessageID MessageID;
        public object[] Params = null;
        public Msg(EMessageID MessageID, params object[] Params)
        {
            this.MessageID = MessageID;
            this.Params = Params;
        }
    }

    private delegate void OnMessageEvent(Msg msg);
    //消息寄存器
    private Dictionary<EMessageID, List<OnMessageEvent>> m_Dict_EventRegister;
    private void Register(EMessageID msg_id, OnMessageEvent e)
    {
        if(!m_Dict_EventRegister.ContainsKey(msg_id))
        {
            m_Dict_EventRegister.Add(msg_id, new List<OnMessageEvent>());
        }
        m_Dict_EventRegister[msg_id].Add(e);
    }
    private void Unregister(OnMessageEvent e)
    {
        foreach (var i in m_Dict_EventRegister)
        {
            if (i.Value.Contains(e))
            {
                i.Value.Remove(e);
            }
        }
    }
    private void Send(Msg msg)
    {
        if(m_Dict_EventRegister.ContainsKey(msg.MessageID))
        {
            List<OnMessageEvent> events = m_Dict_EventRegister[msg.MessageID];
            for(int k = 0; k<events.Count; k++)
            {
                events[k](msg);
            }
        }
    }


    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(stateInfo.IsName("StandBase"))
        {
            animator.SetInteger(m_Str_DetailClipID, 1);

            int r = Random.Range(1, animator.GetInteger(m_Str_ConstMaxOfDetailClips));
            float time = Random.Range(animator.GetInteger(m_Str_MinTimeOfDetailClipsInteval), animator.GetInteger(m_Str_MaxTimeOfDetailClipsInteval));
            Msg msg = new Msg(EMessageID.Msg_AnimShouldTransform, r);
            SendMsgAfter(time, msg);
        }
	}

    IEnumerator SendMsgAfter(float time, Msg msg)
    {
        yield return new WaitForSeconds(time);
        Send(msg);
    }
    

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMachineEnter is called when entering a statemachine via its Entry Node
	override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
        m_CachedAnimator = animator;
        Register(EMessageID.Msg_AnimShouldTransform, OnShouldAnimTransfer);
	}

    void OnShouldAnimTransfer(Msg msg)
    {
        m_CachedAnimator.SetInteger(m_Str_DetailClipID, (int)msg.Params[0]);
    }

	// OnStateMachineExit is called when exiting a statemachine via its Exit Node
	override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
        Unregister(OnShouldAnimTransfer);
	}


}
