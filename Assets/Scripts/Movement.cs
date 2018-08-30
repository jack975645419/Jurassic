using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    protected Transform m_Transform = null;
    protected Rigidbody m_Rigidbody = null;
    public float MaxSpeed = 2.0f;
    public Vector3 m_LatestVelocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v">不带速度，将会乘以MaxSpeed</param>
    public void MoveDirectionWithLimit(Vector3 velocity)
    {
        velocity *= MaxSpeed;
        if(velocity.magnitude>MaxSpeed)
        {
            velocity = velocity.normalized * MaxSpeed;
        }

        m_LatestVelocity = velocity;
        //m_Rigidbody.velocity = velocity;
        m_Transform.Translate(velocity * Time.deltaTime);
    }

    public void TurnRight(float mag)
    {
        transform.Rotate(new Vector3(0, mag, 0), Space.Self);
    }
}
