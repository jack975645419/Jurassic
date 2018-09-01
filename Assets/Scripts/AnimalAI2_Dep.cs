using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAI2_Dep : MonoBehaviour {

    public PawnSensor m_PawnSensor = null;
    public Movement m_Movement = null;
    public GameObject m_SitPlace = null;
    public Animator m_Animator = null;
    public AnimManager_Dep m_AnimManager = null;

    // Use this for initialization
    void Start () {
		
	}
	


	// Update is called once per frame
	void Update () {
		
        //第一树：有人骑行
        //if( GetValueAsGO(Names.DrivingPlayer) )



	}

    //public void StartDrivenBy()











    //黑板模块，要求提供接口GetValueAs(ValueString, typeof(Vector3))，表示从黑板中获取字串对应的值，如果没有找到就返回默认值，SetValueAs(ValueString, typeof(int))，表示写入某值到黑板，如果没有就创建，如果有就覆盖，黑板字典的类型只有object，但是可以根据类型写入或传出
    public Dictionary<string, object> BlackBoard = new Dictionary<string, object>();
    public void SetValue(string name, object value)
    {
        if (BlackBoard.ContainsKey(name))
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
    }
}
