using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicTools
{
    public static void Assert(bool b)
    {
        if (!b) Debug.LogError("error happens");
    }

    /// <summary>
    /// 格式必须是(1,2,3)
    /// </summary>
    public static Vector3 ConvertToVector3(string v)
    {
        v = v.Substring(1, v.Length - 2);
        var t = v.Split(',');
        Assert(t.Length == 3);

        var x = (float)System.Convert.ToDouble(t[0]);
        var y = (float)System.Convert.ToDouble(t[1]);
        var z = (float)System.Convert.ToDouble(t[2]);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 
    /// C#的模是考虑负数的模如 -10 % 3 = -1
    /// </summary>
    /// <param name="a"></param>
    public static float NormalizeAngleBetween_n180top180(float a)
    {
        if (a <= 180 && a >= -180) return a;
        else return a >= 0 ? (a + 180) % 360 - 180 : (a - 180) % 360 + 180;
    }

    public static  float GetHorizontalDistance(Vector3 a, Vector3 b)
    {
        var c = a - b;
        c.y = 0;
        return c.magnitude;
    }

}

public class GameManager : Singleton<GameManager> {
    public GameObject Player = null;
    public GameObject Ground = null;
    public GameObject[] GOs = new GameObject[10];
    public Text HPLabel = null;
    public Animator BloodAnim = null;

    public override void Start()
    {
        base.Start();

        Player = GameObject.Find("Player");
        Ground = GameObject.Find("Ground");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
