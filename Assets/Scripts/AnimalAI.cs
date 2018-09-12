using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    AINS_Finish,
    AINS_Fail,
}

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
    public GameObject m_SensedHumanPlayer = null;
    public float m_SafeFeelingAreaRadius = 50.0f;
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
    public const float THRESHOLD_TURNUPSIDEDOWN_UP_Y = 0.3f;
    public HPComponent m_HPComponent = null;
    public float m_Hurt = 10;

/*
    private float m_WAccumulatingTime = 0;
    public bool m_HuntingMode
    {
        get
        {
            return m_WAccumulatingTime > 5.0f && m_PawnSensor.m_SensitiveObjectWithAutoClear!=null;
        }
        set
        {
            if(value)
            {
                //重新计时
                if(m_PawnSensor.m_SensitiveObjectWithAutoClear!=null && 
                    !m_PawnSensor.m_SensitiveObjectWithAutoClear.CompareTag("Player"))
                {
                    m_PawnSensor.m_SensitiveObjectWithAutoClear = m_PawnSensor.m_SensitiveObjectWithAutoClear;
                }
                else
                {
                    m_PawnSensor.m_SensitiveObjectWithAutoClear = m_PawnSensor.MainSensedDinosaur;
                }
            }
            else
            {
                m_PawnSensor.m_SensitiveObjectWithAutoClear = null;
                m_WAccumulatingTime = 0;
            }
        }
    }*/
    public bool IsDriven
    {
        get
        {
            return m_DrivingPlayer != null;
        }
    }

    // Use this for initialization
    public virtual void Start()
    {
        m_Movement = GetComponent<Movement>();
        m_Animator = GetComponent<Animator>();
        m_AnimManager = GetComponent<AnimManager_Dep>();
        m_PawnSensor = GetComponent<PawnSensor>();
        m_AnimCtrl = GetComponent<AnimCtrl>();
        m_SitPlaceColliderListener = m_SitPlace.GetComponent<SitPlaceColliderListener>();
        m_HPComponent = gameObject.AddComponent<HPComponent>();

        //倒地检测
        TimerManager.Instance.SetNewTimer(2.0f, (o) => {
            if(transform.up.y<THRESHOLD_TURNUPSIDEDOWN_UP_Y)
            {
                this.transform.SetPositionAndRotation(this.transform.position + new Vector3(0, 1.8f, 0), Quaternion.identity);
            }
        }, true);
    }

    //第几树就对应第几个，10表示第1组树里的第0个任务的状态，100以后作保留用
    public AINodeState[] m_AINodeStates = new AINodeState[120];
    public void InitNodeStatesWhenExecXthTree(int X)
    {
        if (m_AINodeStates[X] == AINodeState.AINS_NotStarted)
        {
            m_AINodeStates[X] = AINodeState.AINS_Doing;
        }
        for (int k = 0; k < m_AINodeStates.Length; k++)
        {
            if (k == X || k / 10 == X) continue;
            m_AINodeStates[k] = AINodeState.AINS_NotStarted;
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        UpdatePadInput();

        //驾驶状态不要清空输入，其它状态要清空输入
        if (m_AINodeStates[1] != AINodeState.AINS_Doing)
        {
            m_AnimCtrl.m_PseudoInput.Init();
        }

        //此处仅为了测试
/*
        var TargetLocation_0 = GameObject.Find("TargetLocation_0");
        bool DebugJ = true;
        if (DebugJ)
        {

        }*/

        if (m_HPComponent.HP <= 0) return;

        var bShouldReturn = true;

        //第一树：有骑行
        if (m_DrivingPlayer != null)
        {
            bShouldReturn = AITreeNode_BeDriven();
            if (bShouldReturn) return;
        }
        //第二树：受攻击 处于已知攻击者的状态
        if (m_Attacker != null)
        {
            bShouldReturn = AITreeNode_Attacked();
            if (bShouldReturn) return;
        }
        //第三树：警惕
        if (IsAlert)
        {
            bShouldReturn = AITreeNode_Alert();
            if (bShouldReturn) return;
        }

        m_SensedHumanPlayer = m_PawnSensor.SensedHuman;
        //第四树：探知有人
        if (m_SensedHumanPlayer != null)
        {
            bShouldReturn = AITreeNode_SenseHumans();
            if (bShouldReturn) return;
        }
        //第五树：探知有龙
        if (m_PawnSensor.MainSensedDinosaur)
        {
            bShouldReturn = AITreeNode_SenseDinosaurs();
            if (bShouldReturn) return;
        }
        //第六树：孤独状态
        if(m_PawnSensor.PawnsSensed.Count==0)
        {
            bShouldReturn = AITreeNode_Alone();
            if (bShouldReturn) return;
        }

    }
    
    public virtual bool AITreeNode_BeDriven()
    {
        InitNodeStatesWhenExecXthTree(1);
        m_AINodeStates[1] = AITaskCallback_DrivenByPlayer();
        return true;
    }

    //攻击态：通用逻辑 走到攻击者面前播放攻击动画
    public float m_CancelAttackDistance = 60.0f;
    HPComponent m_AttackerHP = null;
    public float m_AttackAllowedDistance = 5.5f;
    public virtual bool AITreeNode_Attacked()
    {
        InitNodeStatesWhenExecXthTree(2);

        if (m_Attacker != null)
        {
            if (m_AttackerHP == null)
            {
                m_AttackerHP = m_Attacker.GetComponent<HPComponent>();
            }
            else
            {
                var d = BasicTools.GetHorizontalDistance(this.transform.position, m_Attacker.transform.position);

                //因为攻击者太远或者是攻击者死亡而取消攻击者
                if (m_AttackerHP.HP <= 0 || d >= m_CancelAttackDistance)
                {
                    AITask_MoveHorizontallyTo(m_Attacker.transform.forward * 50.0f);
                    //m_Attacker = null;
                    //m_AttackerHP = null;
                }
                else if (d < m_AttackAllowedDistance)
                {
                    m_AnimCtrl.m_PseudoInput.LeftClick = true;
                }
                else
                {
                    AITask_MoveHorizontallyTo(m_Attacker.transform.position);
                }

            }
        }
        return true;
    }

    public virtual bool AITreeNode_Alert()
    {
        InitNodeStatesWhenExecXthTree(3);
        return true;
    }

    public virtual bool AITreeNode_SenseHumans()
    {
        InitNodeStatesWhenExecXthTree(4);
        return true;
    }

    public virtual bool AITreeNode_SenseDinosaurs()
    {
        InitNodeStatesWhenExecXthTree(5);
        return true;
    }


    protected float m_NextDesideTimeForAloneState = 2;
    protected Vector3 m_RandomDestinationForAlone;
    protected int m_ContinuouslyInputAlphaX = 0;
    public virtual bool AITreeNode_Alone()
    {
        InitNodeStatesWhenExecXthTree(6);
        return true;
    }

    //状态编号 1
    public AINodeState AITaskCallback_DrivenByPlayer()
    {
        //Debug.Log("被骑行中");
        //AI树本身不做任何事情，只需要将控制权接口交由PlayerController即可

        return AINodeState.AINS_Doing;
    }

    //状态编号 30
    public AINodeState AITask_FaceTo(Vector3 worldPositionToFaceTo)
    {
        var localPosition = gameObject.transform.InverseTransformPoint(worldPositionToFaceTo) * transform.localScale.x;
        //Debug.Log("" + localPosition.ToString());
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

    //状态编号 31
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
        m_Attacker = null;
        m_AttackerHP = null;

        m_DrivenBeginningTime = Time.time;
        if(m_WildState == EWildState.EWild)
        {
            Logger.Instance.LogInfoToScreen("请尽可能走直线(维持15s)", 210, 15);
            m_WildState = EWildState.ERebellous;
        }
        m_PawnSensor.m_SensitiveObjectWithAutoClear = null;
        m_SitPlaceColliderListener.enabled = false;
        m_AnimCtrl.m_PhysicalPower = m_AnimCtrl.m_MaxPhysicalPower;
    }

    public void EndBeingDriven()
    {
        m_DrivingPlayer = null;
        m_SitPlaceColliderListener.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Undriven");
        m_SitPlace.layer = LayerMask.NameToLayer("Undriven");
        TimerManager.Instance.SetNewTimer(1f, (o)=> {
            gameObject.layer = LayerMask.NameToLayer("Default");
            m_SitPlace.layer = LayerMask.NameToLayer("Default");
            m_SitPlaceColliderListener.enabled = true;
        });

        if (m_WildState == EWildState.ERebellous) m_WildState = EWildState.EWild;
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
        m_AnimCtrl.m_PseudoInput.LeftPad = m_LeftPadInput;
        
        //驯服玩法
        if (m_WildState == EWildState.ERebellous)
        {
            if(m_RebellionCurveX.length == 0 || Time.time - m_DrivenBeginningTime >= m_RebellionCurveX.keys[m_RebellionCurveX.length - 1].time)
            {
                m_WildState = EWildState.EOwned;
            }

            if( m_RebellionCurveX.length>0 && Time.time - m_DrivenBeginningTime < m_RebellionCurveX.keys[m_RebellionCurveX.length-1].time )
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
                    GetRidOfDriver(result.x);
                }
                Logger.Instance.LogInfoToScreen("Offset:" + _offset, 211);

                m_AnimCtrl.m_PseudoInput.LeftPad = result;
            }
        }

#region 取消Hunting玩法
/*
                if(m_DrivingPlayer!=null && m_WildState== EWildState.EOwned)
                {
                    if (m_AnimCtrl.m_PseudoInput.W)
                    {
                        m_WAccumulatingTime += Time.deltaTime;
                    }

                    //切换为狩猎模式
                    if(m_WAccumulatingTime>=5.0f)
                    {
                        m_HuntingMode = true;
                        Debug.Log("开始狩猎，猎物为：" + m_PawnSensor.m_SensitiveObjectWithAutoClear);
                    }
                    if (m_AnimCtrl.m_PseudoInput.S)
                    {
                        m_HuntingMode = false;
                    }
                }*/
                #endregion
        
    }

    public virtual void GetRidOfDriver(float xOffset)
    {
        m_DrivingPlayer.GetComponent<PlayerController>().FellDownFromAnimal(xOffset);
    }
    
    public AINodeState AITask_MoveHorizontallyTo(Vector3 pos)
    {
        var delHor = BasicTools.GetHorizontalDistance(pos, gameObject.transform.position);

        if (delHor<1.0f)
        {
            return AINodeState.AINS_Finish;
        }

        if (AITask_FaceTo(pos)== AINodeState.AINS_Finish)
        {
            m_AnimCtrl.m_PseudoInput.W = true;
        }

        return AINodeState.AINS_Doing;
    }

    //占用状态编号 a（自身）/b（占用） 用变量体现编号
    Vector3 lastDeltaPos;
    Vector3 directionToEscape;
    public AINodeState AITask_EscapeFrom(GameObject hunter, ref AINodeState nodeState_a, ref AINodeState nodeState_b)
    {
        if (hunter == null) return AINodeState.AINS_Fail;

        var curDeltaPos = transform.position - hunter.transform.position;
        if (curDeltaPos.magnitude > m_SafeFeelingAreaRadius)
        {
            return AINodeState.AINS_Finish;
        }

        //刚开始逃跑定一个方向，范围级别变化也重新调整逃跑方向
        if (nodeState_a == AINodeState.AINS_NotStarted || (int)(lastDeltaPos.magnitude/10) != (int)(curDeltaPos.magnitude/10) )
        {
            directionToEscape.x = Random.Range(0, curDeltaPos.x);
            directionToEscape.y = Random.Range(0, curDeltaPos.y);
            directionToEscape.z = Random.Range(0, curDeltaPos.z);
            nodeState_b = AINodeState.AINS_NotStarted;

            Debug.Log("重新定逃跑方向" + directionToEscape.ToString());
        }

        if(nodeState_b != AINodeState.AINS_Finish) nodeState_b = AITask_FaceTo(directionToEscape + transform.position);

        m_AnimCtrl.m_PseudoInput.W = true;

        lastDeltaPos = curDeltaPos;
        return AINodeState.AINS_Doing;
    }

/*取消猎杀玩法
    //状态编号 11自动猎杀
    public AINodeState AITask_Hunt(GameObject prey)
    {
        if (prey == null) return AINodeState.AINS_Fail;
        
        AITask_MoveHorizontallyTo(transform.position + prey.transform.forward * 2.0f/ * * prey.GetComponent<AnimCtrl>().velocity* / );
        return AINodeState.AINS_Doing;
    }

 */


    public void SingleAttack(float multiplier = 1.0f)
    {
        if(m_AttackerHP!=null)
        {
            m_AttackerHP.HP -= m_Hurt * multiplier;
            var PC = m_Attacker.GetComponent<PlayerController>();
            if(PC!=null)
            {
                PC.SetMainCameraShake(0.5f);
            }
        }
    }
}
