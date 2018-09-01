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

public enum EWildState
{
    EWild,
    ERebellous,
    EOwned
}

/// <summary>
/// 伪输入
/// </summary>
public class PseudoInput
{
    public void Init()
    {
        Space = false;
        LeftPad = Vector2.zero;
        RightPad = Vector2.zero;
        LeftShift = false;
        LeftCtrl = false;
        LeftClick = false;
        EForGrowl = false;
        AlphaX = 0;
        
    }
    public PseudoInput GetCopy()
    {
        PseudoInput a = new PseudoInput();
        a.Space = Space;
        a.LeftPad = LeftPad;
        a.RightPad = RightPad;
        a.LeftShift = LeftShift;
        a.LeftCtrl = LeftCtrl;
        a.LeftClick = LeftClick;
        a.EForGrowl = EForGrowl;
        a.AlphaX = AlphaX;
        return a;
    }


    public bool Space = false;
    public Vector2 LeftPad;
    public Vector2 RightPad;
    public bool LeftShift = false;

    public float Turn
    {
        get
        {
            return RightPad.x;
        }
    }
    public float Pitch
    {
        get
        {
            return RightPad.y;
        }
    }
    public bool Mouse1
    {
        get
        {
            return !(Turn == 0 && Pitch == 0);
        }
    }

    public bool LeftCtrl = false;
    public bool LeftClick = false;//攻击
    public bool EForGrowl = false;
    public int AlphaX = 0;//数字键输入

    public bool W
    {
        set
        {
            if (value)
            {
                LeftPad = new Vector2(0, 1);
            }
            else
            {
                if (LeftPad.y > 0) LeftPad.y = 0;
            }
        }
        get
        {
            return LeftPad.y > 0 && Mathf.Abs(LeftPad.y) > Mathf.Abs(LeftPad.x);
        }
    }
    public bool S
    {
        set
        {
            if (value)
            {
                LeftPad = new Vector2(0, -1);
            }
            else
            {
                if (LeftPad.y < 0) LeftPad.y = 0;

            }
        }
        get
        {
            return LeftPad.y < 0 && Mathf.Abs(LeftPad.y) > Mathf.Abs(LeftPad.x);
        }
    }
    public bool A
    {
        set
        {
            if (value)
            {
                LeftPad = new Vector2(-1, 0);
            }
            else
            {
                if (LeftPad.x < 0) LeftPad.x = 0;

            }
        }
        get
        {
            return LeftPad.x < 0 && Mathf.Abs(LeftPad.y) < Mathf.Abs(LeftPad.x);
        }
    }
    public bool D
    {
        set
        {
            if(value)
            {
                LeftPad = new Vector2(1, 0);
            }
            else
            {
                if (LeftPad.x> 0) LeftPad.x = 0;

            }
        }
        get
        {
            return LeftPad.x > 0 && Mathf.Abs(LeftPad.y) < Mathf.Abs(LeftPad.x);
        }

    }

}


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
    public AnimManager_Dep m_AnimManager = null;
    public PawnSensor m_PawnSensor = null;
    public GameObject m_DrivingPlayer = null;
    public Vector2 m_LeftPadInput = Vector2.zero;
    public Vector2 m_RightPadInput = Vector2.zero;
    public GameObject m_Attacker = null;
    public SitPlaceColliderListener m_SitPlaceColliderListener = null;
    public AnimCtrl m_AnimCtrl = null;
    public EWildState m_WildState = EWildState.EWild;
    public float m_DrivenBeginningTime = 0;
    public AnimationCurve m_RebellionCurveX;
    public float m_OffsetAccumulation = 0;
    //由策划调好
    //public Vector2[] m_RebellousOffset = 


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
    public float m_TurnRate = 40.0f;

    // Use this for initialization
    public virtual void Start()
    {
        m_Movement = GetComponent<Movement>();
        m_Animator = GetComponent<Animator>();
        m_AnimManager = GetComponent<AnimManager_Dep>();
        m_PawnSensor = GetComponent<PawnSensor>();
        m_AnimCtrl = GetComponent<AnimCtrl>();
        m_SitPlaceColliderListener = m_SitPlace.GetComponent<SitPlaceColliderListener>();

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

        UpdatePadInput();

        //驾驶状态不要清空
        if(m_AINodeStates[1] != AINodeState.AINS_Doing)
        {
            m_AnimCtrl.m_PseudoInput.Init();
        }


        var TargetLocation_0 = GameObject.Find("TargetLocation_0");
        bool DebugJ = true;
        if(DebugJ)
        {

        }



        //第一树：有骑行
        if (m_DrivingPlayer != null)
        {
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
                m_AINodeStates[32] = AITask_MoveHorizontallyTo(m_TargetLocation);
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
        //Debug.Log("被骑行中");
        //AI树本身不做任何事情，只需要将控制权接口交由PlayerController即可

        return AINodeState.AINS_Doing;
    }

    public AINodeState AITask_FaceTo(Vector3 worldPositionToFaceTo)
    {
        
        var localPosition = gameObject.transform.InverseTransformPoint(worldPositionToFaceTo) * transform.localScale.x;
        Debug.Log("" + localPosition.ToString());
        Logger.Instance.LogInfoToScreen("W" + worldPositionToFaceTo.ToString(), 202, 0.6f);
        Logger.Instance.LogInfoToScreen("L"+localPosition.ToString(), 201, 0.6f);


        localPosition.y = 0.0f;
        
        //已经在正前方了
        if (Mathf.Abs( localPosition.x ) <0.2f && localPosition.z >= 0)
        {
            return AINodeState.AINS_Finish;
        }
        else if (localPosition.z > 0 && Mathf.Abs( localPosition.x / localPosition.z) < 0.1f)
        {
            return AINodeState.AINS_Finish;
        }
        //在右侧
        else if (Vector3.Cross(Vector3.forward, localPosition).y > 0)
        {
            m_AnimCtrl.m_PseudoInput.D = true;
            m_Movement.TurnRight(Time.deltaTime * m_TurnRate);
        }
        else
        {
            m_AnimCtrl.m_PseudoInput.A = true;
            m_Movement.TurnRight(-Time.deltaTime * m_TurnRate);
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

        m_DrivenBeginningTime = Time.time;
        if(m_WildState == EWildState.EWild)
        {
            Logger.Instance.LogInfoToScreen("请尽可能走直线(维持15s)", 210, 15);
            m_WildState = EWildState.ERebellous;
        }
    }

    public void EndBeingDriven()
    {
        m_DrivingPlayer = null;
        m_SitPlaceColliderListener.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Undriven");
        TimerManager.Instance.SetNewTimer(0.5f, RestartToBeDrivable);

        if (m_WildState == EWildState.ERebellous) m_WildState = EWildState.EWild;
    }
    public void RestartToBeDrivable(params object [] p)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        m_SitPlaceColliderListener.enabled = true;
    }

    public virtual void InputLeftPad(Vector2 leftPadInput)
    {
        m_LeftPadInput = leftPadInput;
    }

    
    public virtual void InputRightPad(Vector2 rightPadInput)
    {
        m_RightPadInput = rightPadInput;
    }

    public virtual void UpdatePadInput()
    {
        //不要轻易修改源数据
        m_AnimCtrl.m_PseudoInput.LeftPad = m_LeftPadInput;
        
        if (m_WildState == EWildState.ERebellous)
        {
            if( Time.time - m_DrivenBeginningTime < m_RebellionCurveX.keys[m_RebellionCurveX.length-1].time )
            {
                //读源数据且修改后赋值到正式数据中
                var result = m_LeftPadInput;
                result.x += m_RebellionCurveX.Evaluate(Time.time - m_DrivenBeginningTime);
                result.y = 1.0f;//全速奔跑~

                Debug.Log("m_LeftPadInput"+ m_LeftPadInput.ToString());
                Debug.Log("result" + result.ToString());
                var _offset = Mathf.Abs(result.x);
                if (_offset > 1.0f)
                {
                    m_DrivingPlayer.GetComponent<PlayerController>().Jump(10);
                }
                Logger.Instance.LogInfoToScreen("Offset:" + _offset, 211);

                m_AnimCtrl.m_PseudoInput.LeftPad = result;
            }
        }

    }

    public AINodeState AITask_MoveHorizontallyTo(Vector3 pos)
    {
        var delHor = pos - gameObject.transform.position;
        delHor.y = 0;
        Debug.Log("pos" + pos + " transform.pos:" + transform.position );


        if (delHor.magnitude<1.0f)
        {
            return AINodeState.AINS_Finish;
        }

        if (AITask_FaceTo(pos)== AINodeState.AINS_Finish)
        {
            m_AnimCtrl.m_PseudoInput.W = true;
        }

        return AINodeState.AINS_Doing;
    }
}
