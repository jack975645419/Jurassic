using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPComponent : MonoBehaviour {

    private Text HPLabel = null;
    public float m_HP = 100;
    private Animator BloodFX = null;
    private PlayerController PC = null;
    public float HP
    {
        set
        {
            if(m_HP>value && BloodFX!=null)
            {
                BloodFX.gameObject.SetActive(true);
                BloodFX.Play("BloodAnim", 0, 0);
            }

            m_HP = value;

            if(HPLabel!=null)
            {
                HPLabel.color = new Color( (1 - m_HP / 100), m_HP / 100,0, 1);
                HPLabel.text = "HP " + value;
            }

            //死亡事件
            if (value <= 0)
            {
                HPLabel.color = Color.red;
                HPLabel.text = "DEAD!";
            }
        }
        get
        {
            return m_HP;
        }
    }

    // Use this for initialization
    void Start () {
        PC = GetComponent<PlayerController>();
        if ( PC!=null )
        {
            HPLabel = GameManager.Instance.HPLabel;
            BloodFX = GameManager.Instance.BloodAnim;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
