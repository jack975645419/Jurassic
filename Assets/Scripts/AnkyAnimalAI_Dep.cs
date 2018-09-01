using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkyAnimalAI_Dep : AnimalAI {
/*

    protected AINode AINode_DrivenByPlayer = null;
    protected AINode AINode_AttackedByPlayer = null;
    protected AINode AINode_Alert = null;
    protected AINode AINode_FaceToAlertPosition = null;
    protected AINode AINode_Waiting = null;
    public const float m_AlertHoldingTime = 20.0f;
    protected AINode AINode_MoveToTargetLocation = null;

    private void Awake()
    {
        / *Root = new AINode();

        //第一树：有人骑行
        AINode_DrivenByPlayer = new AINode(AITaskCallback_DrivenByPlayer, AIPreCallback_IsBeingDrivenByPlayer, AINodeType.AINode_DoUntilWrong);
        Root.AddSubNode(AINode_DrivenByPlayer);

        //第二树：有攻击者信息
        AINode_AttackedByPlayer = new AINode(AITaskCallback_AttackedByPlayer, AIPreCallback_IsBeingAttackedByPlayer, AINodeType.AINode_DoUntilWrong);
        Root.AddSubNode(AINode_AttackedByPlayer);

        //第三树：有警惕（针对石头的事件）信息
        AINode_Alert = new AINode(AITaskCallback_Alert, AIPreCallback_Alert, AINodeType.AINode_DoUntilWrong);
        AINode_FaceToAlertPosition = new AINode(AITaskCallback_FaceToPosition);
        AINode_Alert.AddSubNode(AINode_FaceToAlertPosition);
        AINode_Waiting = new AINode(AITask_Wait);
        AINode_Alert.AddSubNode(AINode_Waiting);
        AINode_MoveToTargetLocation = new AINode(AITaskCallback_MoveToTargetLocation);
        AINode_Alert.AddSubNode(AINode_MoveToTargetLocation);

        Root.AddSubNode(AINode_Alert);

        //第四树：PawnSensed中的信息有人时


        //第五树：PawnSensed中的信息有龙时


        //第六树：孤独的时候
* /

    }

    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();

    }*/

    //AITask————————————————————————
/*
    public AINodeResultType AITaskCallback_DrivenByPlayer()
    {
        Debug.Log("被骑行中");
        //AI树本身不做任何事情，只需要将控制权接口交由PlayerController即可
        return AINodeResultType.AIResult_Doing;
    }

    public bool AIPreCallback_IsBeingDrivenByPlayer()
    {
        return false;// GetValueAsGO(Names.DrivingPlayer) != null;
    }

    public AINodeResultType AITaskCallback_AttackedByPlayer()
    {
        return AINodeResultType.AIResult_Doing;
    }
    public bool AIPreCallback_IsBeingAttackedByPlayer()
    {
        var attacker = GetValueAsGO(Names.Attacker);
        return attacker != null && attacker == GameManager.Instance.Player;
    }*/
    
/*
        //————————————————————————待拷贝
    public AINodeResultType AITaskCallback_Alert()
    {
        var LastAlertTime = GetValueAsFloat(Names.LastAlertTimeAsFloat);
        if(Time.time - LastAlertTime>m_AlertHoldingTime)
        {
            SetValue(Names.AlertAsBool, false);
            
            return AINodeResultType.AIResult_Finish;
        }
        
        //在父节点中为子节点准备数据
        SetValue(Names.PositionToFaceTo, GetValueAsVector3(Names.PositionToAlert));

        //如果Waiting节点还没有开始，则布置等候时间为2s
        if (AINode_Waiting.m_AINodeState == AINodeState.AINS_NotStarted) 
        {
            SetValue(Names.TerminalTimeAsFloat, Time.time + 2.0f);
        }

        if(AINode_MoveToTargetLocation.m_AINodeState==AINodeState.AINS_NotStarted)
        {
            SetValue(Names.TargetLocation, transform.position + transform.forward * 3.0f);
        }

        return AINodeResultType.AIResult_Doing;
    }*/

/*
    public AINodeResultType AITask_Wait()
    {
        var TerminalTime = GetValueAsFloat(Names.TerminalTimeAsFloat);
        if(Time.time>TerminalTime)
        {
            SetValue(Names.Waiting, false);
            return AINodeResultType.AIResult_Finish;
        }
        SetValue(Names.Waiting, true);
        return AINodeResultType.AIResult_Doing;
    }

    public bool AIPreCallback_Alert()
    {
        var a = GetValueAsObject(Names.AlertAsBool);
        return GetValueAsGO(Names.Attacker) == null && a!=null && (bool)a;
    }

    public AINodeResultType AITaskCallback_FaceToPosition()
    {
        var positionToFaceTo = GetValueAsVector3(Names.PositionToFaceTo);
        return AITask_FaceToPosition(positionToFaceTo);
    }
    */

/*

    //来自AnimalAI的方法重载————————————————————————
    public override void OnMove(Vector2 leftPadInput)
    {
        base.OnMove(leftPadInput);
        

        
    }*/

/*
    public void MoveTo(Vector3 pos)
    {
        if( AITask_FaceToPosition(pos) == AINodeResultType.AIResult_Finish )
        {
            //目标位置减去当前位置
            m_Movement.MoveDirectionWithLimit(pos - transform.position);
        }
    }*/

/*
    public AINodeResultType AITaskCallback_MoveToTargetLocation()
    {
        var targetLocation = GetValueAsVector3(Names.TargetLocation);
        if ((targetLocation - transform.position).magnitude < 0.1f) return AINode_MoveToTargetLocation.SafelyEnd(AINodeResultType.AIResult_Finish);
        MoveTo(targetLocation);
        return AINode_MoveToTargetLocation.SafelyEnd(AINodeResultType.AIResult_Doing);
    }*/
}
