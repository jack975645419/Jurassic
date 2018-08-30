using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class Names
{
    public const string DrivingPlayer = "DrivingPlayer";
    public const string LeftPadInput = "LeftPadInput";
    public const string RightPadInput = "RightPadInput";
    public const string Attacker = "Attacker";
    public const string AlertAsBool = "Alert";
    public const string LastAlertTimeAsFloat = "LastAlertTime";
    public const string GameObjectToAlert = "GameObjectToAlert";
    public const string PositionToAlert = "PositionToAlert";
    public const string PositionToFaceTo = "PositionToFaceTo";
    public const string TerminalTimeAsFloat = "TerminalTimeAsFloat";
    public const string Waiting = "Waiting";
    public const string TargetLocation = "TargetLocation";
}*/
/*

public enum AINodeType
{
    AINode_DoUntilWrong,
    AINode_DoUntilRight,
}

public enum AINodeResultType
{
    AIResult_Unknown,
    AIResult_Fail,
    AIResult_Doing,
    AIResult_Finish,
    AIResult_ConditionUnsatisfied,
}*/

//节点态：要求在Finish时将所有子节点的节点态设置为Finish
public enum AINodeState
{
    AINS_NotStarted,
    AINS_Doing,
    //AINS_End,//失败结尾或成功结尾都用End
    AINS_Finish,
    AINS_Fail,
}

/*

/// <summary>
/// 废弃不使用这么复杂的行为树
/// </summary>
public class AINode
{
    public delegate AINodeResultType AITask();
    public delegate bool Precondition();

    public AITask m_Task;
    public Precondition m_Precondition;
    public List<AINode> m_SubNodes = new List<AINode>();
    public AINodeType m_AINodeType;
    public AINodeState m_AINodeState = AINodeState.AINS_NotStarted;
    public AINodeResultType Exec()
    {
        m_AINodeState = AINodeState.AINS_Doing;

        //未通过条件
        if (m_Precondition != null && !m_Precondition()) return SafelyEnd(AINodeResultType.AIResult_ConditionUnsatisfied);
        //前序遍历
        if (m_Task!=null && m_Task()== AINodeResultType.AIResult_Fail) return SafelyEnd(AINodeResultType.AIResult_Fail);
        if (m_Task != null && m_Task() == AINodeResultType.AIResult_Finish) return SafelyEnd(AINodeResultType.AIResult_Finish);

        //只有本身条件通过且不是执行成功也不是执行失败，才可以执行子节点
        //全部子节点执行成功后，自身的Doing也认定为成功
        if (m_AINodeType==AINodeType.AINode_DoUntilWrong)
        {
            for (int k = 0; k < m_SubNodes.Count; k++)
            {
                if (m_SubNodes[k] != null && m_SubNodes[k].Exec() == AINodeResultType.AIResult_Doing)
                    return SafelyEnd(AINodeResultType.AIResult_Doing);
                if (m_SubNodes[k] != null && m_SubNodes[k].Exec() == AINodeResultType.AIResult_Fail)
                    return SafelyEnd(AINodeResultType.AIResult_Fail);
            }
            return SafelyEnd(AINodeResultType.AIResult_Finish);
        }
        else// if(m_AINodeType==AINodeType.AINode_DoUntilRight)
        //全部子节点失败后，自身的Doing也认定为失败
        {
            for (int k = 0; k < m_SubNodes.Count; k++)
            {
                if (m_SubNodes[k] != null && m_SubNodes[k].Exec() == AINodeResultType.AIResult_Doing)
                    return SafelyEnd(AINodeResultType.AIResult_Doing);
                if (m_SubNodes[k] != null && m_SubNodes[k].Exec() == AINodeResultType.AIResult_Finish)
                    return SafelyEnd(AINodeResultType.AIResult_Finish);
            }
            return SafelyEnd(AINodeResultType.AIResult_Fail);
        }

        //绝不会到达这里
        //return AINodeResultType.AIResult_Unknown;
    }

    public AINode(AITask aiTask = null, Precondition pre = null, AINodeType aiNodeType = AINodeType.AINode_DoUntilWrong)
    {
        m_Task = aiTask; m_Precondition = pre; m_AINodeType = aiNodeType;
    }
    public void AddSubNode(AINode node)
    {
        m_SubNodes.Add(node);
    }
    private void End()
    {
        for(int k = 0; k<m_SubNodes.Count; k++)
        {
            m_SubNodes[k].m_AINodeState = AINodeState.AINS_NotStarted;
        }
        //将自己的状态设置为已完结，将子孙节点设置为未开始
        this.m_AINodeState = AINodeState.AINS_End;
    }
    public AINodeResultType SafelyEnd(AINodeResultType r)
    {
        if (r == AINodeResultType.AIResult_Doing) return r;

        End();
        return r;
    }
}
*/

/// <summary>
/// 一棵行为树
/// </summary>
public class AnimalAI : MonoBehaviour {

    public GameObject m_SitPlace = null;
    public Movement m_Movement = null;
    public Animator m_Animator = null;
    public AnimManager m_AnimManager = null;
    public PawnSensor m_PawnSensor = null;
    public GameObject m_DrivingPlayer = null;
    public Vector2 m_LeftPadInput = Vector2.zero;
    public Vector2 m_RightPadInput = Vector2.zero;
    public GameObject m_Attacker = null;

    //变量自动延时关闭的实现方式
    public bool IsAlert
    {
        get
        {
            return Time.time - m_LastStartAlertTime < m_AlertHoldingTime;
        }
        set
        {
            if(value)
            {
                m_LastStartAlertTime = Time.time;
            }
            else
            {
                m_LastStartAlertTime = Time.time - m_AlertHoldingTime;
            }
        }
    }
    
    public float m_LastStartAlertTime = -210001.0f;
    public const float m_AlertHoldingTime = 100;
    public Vector3 m_PositionToAlert = Vector3.zero;
    public float m_TimeToWait = 0;
    public Vector3 m_TargetLocation = Vector3.zero;

    // Use this for initialization
    public virtual void Start()
    {
        m_Movement = GetComponent<Movement>();
        m_Animator = GetComponent<Animator>();
        m_AnimManager = GetComponent<AnimManager>();
        m_PawnSensor = GetComponent<PawnSensor>();


    }

    //第几树就对应第几个，10表示第1组树里的第0个任务的状态
    public AINodeState[] m_AINodeStates = new AINodeState[80];
    public void InitNodeStatesExceptXthTree(int X)
    {
        for(int k = 0; k<m_AINodeStates.Length; k++)
        {
            if (k == X || k / 10 == X) continue;
            m_AINodeStates[k] = AINodeState.AINS_NotStarted;
        }
    }





    // Update is called once per frame
    public virtual void Update()
    {
        Logger.Instance.LogInfoToScreen(IsAlert ? "alerting" : "", 200, 0.2f);

        //第一树：有骑行
        if (m_DrivingPlayer != null)
        {
            if(m_AINodeStates[1]== AINodeState.AINS_NotStarted)//首次进入某树某态
            {
                m_AnimManager.ChangeState(EAnimState.EAS_Run, 0);
            }

            InitNodeStatesExceptXthTree(1);

            m_AINodeStates[1] = AITaskCallback_DrivenByPlayer();
            return;
        }
        //第二树：受攻击 处于已知攻击者的状态
        else if (m_Attacker != null)
        {
            InitNodeStatesExceptXthTree(2);

        }
        //第三树：警惕
        else if (IsAlert)
        {
            InitNodeStatesExceptXthTree(3);

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
                m_AINodeStates[32] = AITask_MoveTo(m_TargetLocation);
            }
            //注：3.2已完成，即所有任务完成
        }
        //第四树：探知有人
        /*else if ()
        {
            InitNodeStatesExceptXthTree(4);

        }*/
        //第五树：探知有龙
        //else if()
        //第六树：孤独状态

    }



    public AINodeState AITaskCallback_DrivenByPlayer()
    {
        Debug.Log("被骑行中");
        //AI树本身不做任何事情，只需要将控制权接口交由PlayerController即可
        return AINodeState.AINS_Doing;
    }

    public AINodeState AITask_FaceTo(Vector3 worldPositionToFaceTo)
    {
        var localPosition = transform.worldToLocalMatrix * worldPositionToFaceTo * transform.localScale.x;
        localPosition.y = 0.0f;
        
        //已经在正前方了
        if (localPosition.x == 0 && localPosition.y >= 0)
        {
            return AINodeState.AINS_Finish;
        }
        else if (localPosition.z > 0 && localPosition.z / localPosition.x < 0.2f)
        {
            return AINodeState.AINS_Finish;
        }
        //在右侧
        else if (Vector3.Cross(Vector3.forward, localPosition).y > 0)
        {
            m_Movement.TurnRight(1);
        }
        else
        {
            m_Movement.TurnRight(-1);
        }
        return AINodeState.AINS_Doing;
    }

    public AINodeState AITask_WaitUntil(float TerminalTime)
    {
        if (Time.time < TerminalTime) return AINodeState.AINS_Doing;
        return AINodeState.AINS_Finish;
    }


    //黑板模块，要求提供接口GetValueAs(ValueString, typeof(Vector3))，表示从黑板中获取字串对应的值，如果没有找到就返回默认值，SetValueAs(ValueString, typeof(int))，表示写入某值到黑板，如果没有就创建，如果有就覆盖，黑板字典的类型只有object，但是可以根据类型写入或传出
    /*public Dictionary<string, object> BlackBoard = new Dictionary<string, object>();
    public void SetValue(string name, object value)
    {
        if(BlackBoard.ContainsKey(name))
        {
            BlackBoard[name] = value;
        }
        else
        {
            BlackBoard.Add(name, value);
        }
    }
    public object GetValueAsObject(string name)
    {
        return BlackBoard.ContainsKey(name) ? BlackBoard[name] : null;
    }
    public int GetValueAsInt(string name)
    {
        object a = GetValueAsObject(name);
        if (a != null) return (int)a;
        return 0;
    }
    public float GetValueAsFloat(string name)
    {
        object a = GetValueAsObject(name);
        if (a != null) return (float)a;
        return 0;
    }
    public Vector3 GetValueAsVector3(string name)
    {
        object a = GetValueAsObject(name);
        if (a != null) return (Vector3)a;
        return Vector3.zero;
    }
    public GameObject GetValueAsGO(string name)
    {
        object a = GetValueAsObject(name);
        if (a != null) return (GameObject)a;
        return null;
    }*/

/*

    public AINode Root = null;
*/


    //————————————————————————动物类公用方法
    public void StartDrivenBy(GameObject Player)
    {
        m_DrivingPlayer = Player;
    }

    
    public virtual void OnMove(Vector2 leftPadInput)
    {
        m_LeftPadInput = leftPadInput;

        //恐龙没有平移走，只有前后走
        m_Movement.MoveDirectionWithLimit(new Vector3(0, 0, leftPadInput.y));
        m_Movement.TurnRight(leftPadInput.x);
    }

    
    public virtual void OnViewChange(Vector2 rightPadInput)
    {
        m_RightPadInput = rightPadInput;
    }

    public AINodeState AITask_MoveTo(Vector3 pos)
    {
        if((pos-transform.position).magnitude<0.3f)
        {
            return AINodeState.AINS_Finish;
        }

        if (AITask_FaceTo(pos)== AINodeState.AINS_Finish)
        {
            //目标位置减去当前位置
            m_Movement.MoveDirectionWithLimit(pos - transform.position);
        }

        return AINodeState.AINS_Doing;
    }



}
