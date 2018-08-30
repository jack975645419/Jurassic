using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMessageID
{
    //有效击中事件
    Msg_Hit,
    //报文参数：位置、击打向量、误差角

    //Miss事件
    Msg_Miss,
    //报文参数：位置、击打向量、误差角

    //动静事件
    Msg_Noise,
    //报文参数：传来方向（三维）、动静类型（声响、痛觉）、动静幅值（痛觉的幅值比较大 声响的幅值比较小）

}

public enum ENoiseType
{
    ENoise_Sound,
    ENoise_Hurt,
}




//消息报文
public class Msg
{
    public EMessageID MessageID;
    public object[] Params = null;
    public Msg(EMessageID MessageID, params object[] Params)
    {
        this.MessageID = MessageID;
        this.Params = Params;
    }
}


public class EventManager : Singleton<EventManager> {
    
    public delegate void OnMessageEvent(Msg msg);
    //消息寄存器
    private static Dictionary<EMessageID, List<OnMessageEvent>> m_Dict_EventRegister = new Dictionary<EMessageID, List<OnMessageEvent>>();

    public static void Register(EMessageID msg_id, OnMessageEvent e)
    {
        if (!m_Dict_EventRegister.ContainsKey(msg_id))
        {
            m_Dict_EventRegister.Add(msg_id, new List<OnMessageEvent>());
        }
        if(!m_Dict_EventRegister[msg_id].Contains(e))
        {
            m_Dict_EventRegister[msg_id].Add(e);
        }
    }

    public static void Unregister(OnMessageEvent e)
    {
        foreach (var i in m_Dict_EventRegister)
        {
            if (i.Value.Contains(e))
            {
                i.Value.Remove(e);
            }
        }
    }

    public static void Send(Msg msg)
    {
        if (m_Dict_EventRegister.ContainsKey(msg.MessageID))
        {
            List<OnMessageEvent> events = m_Dict_EventRegister[msg.MessageID];
            for (int k = 0; k < events.Count; k++)
            {
                events[k](msg);
            }
        }
    }
}
