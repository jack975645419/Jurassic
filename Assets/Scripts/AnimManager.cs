using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAnimState
{
    EAS_Stands = 0,
    EAS_Eats = 1,
    EAS_Sits = 2,
    EAS_Run = 3,
    EAS_Die = 4,
    EAS_Attack = 5,
    EAS_MAX
}


public class AnimManager : MonoBehaviour {

    public int AnimSubStateForAnky = 0;
    private SingleTimer Timer = null;
    public Animator AnimCtrl = null;
    public Rigidbody m_Rigidbody = null;
    public Movement m_Movement = null;
	// Use this for initialization
	void Start () {
        Timer = new SingleTimer(100, ChangeState, false, false);
        AnimCtrl = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Movement = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
        //奔跑速度更新，速度不是用Rigidbody来计算的，而是使用
        //取出LatestVelocity的x和z即可分析速度，以x速度为准
        AnimCtrl.SetFloat("RunSpeed", m_Movement.m_LatestVelocity.z);
	}

    public void ChangeState(params object[] Params)
    {
        if (Params.Length < 2) return;
        AnimCtrl.SetInteger("EAS", (int)Params[0]);
        AnimCtrl.SetInteger("SUB", (int)Params[1]);
    }

    /// <summary>
    /// 只移动x和z
    /// </summary>
    /// <param name="v"></param>
    public void Move(Vector3 v)
    {
        v.y = 0;
        transform.Translate(v);
    }



}
