using UnityEngine;
using System.Collections;


public class anky_cs : AnimCtrl
{
	Transform Spine0,Spine1,Spine2,Neck0, Neck1,Neck2,Neck3,Head,Jaw, Tail0,Tail1,Tail2,Tail3,Tail4,Tail5;
	float turn,pitch,open,balance,temp,velocity,animcount,Scale = 0.0F;
	bool reset,soundplayed,isdead =false;
	int lodselect=0, skinselect =0;
	string infos;
	Animator anim;
	AudioSource source;
	LODGroup lods;
	SkinnedMeshRenderer[] rend;
	public Texture[] skin;
	public AudioClip Medstep,Idleherb,Anky_Roar1,Anky_Roar2,Anky_Call1,Anky_Call2,Sniff1,Chew,Largestep;
    public Movement m_Movement = null;

	void Awake ()
	{
		Physics.gravity = new Vector3(0, -10.0F, 0);

		Tail0 = this.transform.Find ("Anky/root/pelvis/tail0");
		Tail1 = this.transform.Find ("Anky/root/pelvis/tail0/tail1");
		Tail2 = this.transform.Find ("Anky/root/pelvis/tail0/tail1/tail2");
		Tail3 = this.transform.Find ("Anky/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = this.transform.Find ("Anky/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = this.transform.Find ("Anky/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Spine0 = this.transform.Find ("Anky/root/spine0");
		Spine1 = this.transform.Find ("Anky/root/spine0/spine1");
		Spine2 = this.transform.Find ("Anky/root/spine0/spine1/spine2");
		Neck0 = this.transform.Find ("Anky/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = this.transform.Find ("Anky/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = this.transform.Find ("Anky/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Neck3 = this.transform.Find ("Anky/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3");
		Head = this.transform.Find ("Anky/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/head");
		Jaw = this.transform.Find ("Anky/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/head/jaw0");

		source = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		lods = GetComponent<LODGroup>();
		rend = GetComponentsInChildren <SkinnedMeshRenderer>();
	}

    /*
            GUI.Label(new Rect(5,200,Screen.width,Screen.height),"Middle Mouse = Camera/Zoom");
            GUI.Label(new Rect(5,220,Screen.width,Screen.height),"Right Mouse = Spine move");
            GUI.Label(new Rect(5,240,Screen.width,Screen.height),"Left Mouse = Attack");
            GUI.Label(new Rect(5,260,Screen.width,Screen.height),"W,A,S,D = Moves");
            GUI.Label(new Rect(5,280,Screen.width,Screen.height),"LeftShift = Run");
            GUI.Label(new Rect(5,300,Screen.width,Screen.height),"LeftCtrl = Attack Pose");
            GUI.Label(new Rect(5,320,Screen.width,Screen.height),"Space = Steps");
            GUI.Label(new Rect(5,340,Screen.width,Screen.height),"E = Growl");
            GUI.Label(new Rect(5,360,Screen.width,Screen.height),"num 1 = IdleA");
            GUI.Label(new Rect(5,380,Screen.width,Screen.height),"num 2 = IdleB");
            GUI.Label(new Rect(5,400,Screen.width,Screen.height),"num 3 = Eat");
            GUI.Label(new Rect(5,420,Screen.width,Screen.height),"num 4 = Drink");
            GUI.Label(new Rect(5,440,Screen.width,Screen.height),"num 5 = Sit/Sleep");
            GUI.Label(new Rect(5,460,Screen.width,Screen.height),"num 6 = Die");*/

    public override void PreProcessPseudoInput()
    {
        base.PreProcessPseudoInput();
        m_ModifiedInput.LeftShift = m_ModifiedInput.LeftPad.y > 0 && m_ModifiedInput.LeftPad.magnitude > 0.7;

    }

    public override void Start()
    {
        m_Movement = GetComponent<Movement>();
    }


    public override void Update()
    {
        base.Update();

        m_Movement.TurnRight(m_ModifiedInput.LeftPad.x);

        //***************************************************************************************
        //Moves animation controller
        if (m_ModifiedInput.W && m_ModifiedInput.Space) anim.SetInteger("State", 2); //Steps forward
        else if ( m_ModifiedInput.LeftShift && m_ModifiedInput.W ) anim.SetInteger("State", 3); //Run
        else if ( m_ModifiedInput.W ) anim.SetInteger("State", 1); //Walk
        else if ( m_ModifiedInput.Space && m_ModifiedInput.S ) anim.SetInteger("State", -2); //Steps backward
        else if (m_ModifiedInput.S) anim.SetInteger("State", -1); //Walk backward
        else if (m_ModifiedInput.A) anim.SetInteger("State", 10); //Strafe+
        else if (m_ModifiedInput.D) anim.SetInteger("State", -10); //Strafe-
        else if (m_ModifiedInput.Space) anim.SetInteger("State", 100); //Steps
        else if (m_ModifiedInput.LeftCtrl) anim.SetInteger("State", -100); //Attack  pose
        else anim.SetInteger("State", 0); //back to loop


        //Attack animation controller
        if (m_ModifiedInput.LeftClick)
            anim.SetBool("Attack", true);
        else
            anim.SetBool("Attack", false);


        //Growl animation controller
        if ( m_ModifiedInput.EForGrowl )
            anim.SetBool("Growl", true);
        else
            anim.SetBool("Growl", false);


        //Idles animation controller
        if (m_ModifiedInput.AlphaX == 1)
            anim.SetInteger("Idle", 1); //Idle 1
        else if (m_ModifiedInput.AlphaX == 2)
            anim.SetInteger("Idle", 2); //Idle 2
        else if (m_ModifiedInput.AlphaX == 3)
            anim.SetInteger("Idle", 3); //Eat
        else if (m_ModifiedInput.AlphaX == 4)
            anim.SetInteger("Idle", 4); //Drink
        else if (m_ModifiedInput.AlphaX == 5)
            anim.SetInteger("Idle", 5); //Sleep
        else if (m_ModifiedInput.AlphaX == 6)
            anim.SetInteger("Idle", 6); //Die
        else
            anim.SetInteger("Idle", 0);


        //***************************************************************************************
        //Spine control
        turn = m_ModifiedInput.Turn;
        pitch = m_ModifiedInput.Pitch;

        //Jaw control
        if (m_ModifiedInput.LeftClick &&
            (reset == false) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|Stand1A")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|Stand2A")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|Stand1B")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|Stand2C")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|Stand1Attack")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|Stand2Attack")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|Step1Attack")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|Step2Attack")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|WalkGrowl")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|RunGrowl")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|RunAttackA")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|RunAttackB")) &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|WalkShake"))
            )
            open -= 4.0F;
        else
            open += 1.0F;


        //Reset spine position during specific animation
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Anky|EatC"))
            reset = true;
        else reset = false;


        //Reset tail and spine position
        if (((anim.GetInteger("State") != 1) || (anim.GetInteger("State") != 3)) && (balance != 0.0F))
        {
            if (balance < 0.0F)
            {
                if (balance < -0.1F)
                    balance += 0.1F;
                else
                    balance = 0.0F;
            }
            else if (balance > 0.0F)
            {
                if (balance > 0.1F)
                    balance -= 0.1F;
                else
                    balance = 0.0F;
            }
        }

        //soundfx

    }

	//***************************************************************************************
	//Clamp and set bone rotations
	void LateUpdate()
	{
		
		balance = Mathf.Clamp(balance, -8.0F, 8.0F);
		open = Mathf.Clamp(open, -20.0F, 0.0F);
		turn = Mathf.Clamp (turn, -30.0F, 30.0F);
		pitch = Mathf.Clamp(pitch, -15.0F, 10.0F);
		temp = turn;
		temp -= balance;
		temp = Mathf.Clamp(temp, -30.0F, 30.0F);
		
		//Neck and head
		Neck0.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Neck1.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Neck2.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Neck3.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Head.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		
		//Spine and tail
		Spine0.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, balance));
		Spine1.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, balance));
		Spine2.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, balance));
		Tail0.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail1.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail2.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail3.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail4.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail5.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));

		//Jaw
		Jaw.transform.localRotation *= Quaternion.Euler (new Vector3 (open, 0, 0));

	}


	void FixedUpdate ()
	{
		//***************************************************************************************
		//Model translations and rotations

		Scale = this.transform.localScale.x; //adjust speed to the model's scale

		//Walking
		if (anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step1") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step1") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step2") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step2ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step2ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Anky|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|WalkGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step2ToEat") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step2ToEat"))
		{

			if( anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step1") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step2"))
			{
				if (m_ModifiedInput.A) //turning
				{
					if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
					{
						this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, -1, 0));
					}
					balance += 0.2F;
				} 
				else if (m_ModifiedInput.D)
				{
					if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
					{
						this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, 1, 0));
					}
					balance -= 0.2F;
				} 
			}
			else
			{
				if (m_ModifiedInput.A)
				{
					this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, -1, 0));
					balance += 0.2F;
				}
				else if (m_ModifiedInput.D)
				{
					this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, 1, 0));
					balance -= 0.2F;
				}
			}

			if (velocity < 0.06F)
			{
				velocity = velocity + (Time.deltaTime * 0.5F); //acceleration
			}

			else if (velocity > 0.14F) //deceleration
			{
				velocity = velocity - (Time.deltaTime * 2.0F);
			}

		
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Stand1A") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Stand2A"))
			{
				velocity = velocity - (Time.deltaTime * 0.6F);
			}

			this.transform.Translate (0, 0, velocity*Scale);
		}


		//Backward walk
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step1-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step2-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step2-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step1ToWalk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step1ToWalk-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Walk-") ||
				 anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step2-ToSit") ||
				 anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step2-ToSit"))
		{

			if( anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step1-") ||
			   anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step2-"))
			{
				if (m_ModifiedInput.A) //turning
				{
					if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
					{
						this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, 1, 0));
					}
					balance += 0.25F;
				}
				else if (m_ModifiedInput.D)
				{
					if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
					{
						this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, -1, 0));
					}
					balance -= 0.25F;
				}
			}
			else
			{
				if (m_ModifiedInput.A) //turning
				{
					this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, 1, 0));
					balance += 0.25F;
				}
				else if (m_ModifiedInput.D)
				{
					this.transform.localRotation *= Quaternion.AngleAxis (0.2F, new Vector3 (0, -1, 0));
					balance -= 0.25F;
				}
			}
			
			if (velocity > -0.06F)
			{
				velocity = velocity - (Time.deltaTime * 0.5F); //acceleration
			}

			if (anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Stand1A") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Stand2A"))
			{
				velocity = velocity + (Time.deltaTime * 0.6F); //deceleration
			}
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
        
		//Strafe-
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Strafe1+") ||
				 anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Strafe1+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Strafe2-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Strafe2-")
		         )
		{
			if (m_ModifiedInput.Mouse1) //turning
			{
				this.transform.localRotation *= Quaternion.AngleAxis (turn /32, new Vector3 (0, 1, 0));
				if (turn < 0) balance += 0.1F;
				else balance -= 0.1F;
			}

			if (velocity < 0.025F)
			{
				velocity = velocity + (Time.deltaTime * 0.5F); //acceleration
			}

			this.transform.Translate (-velocity*Scale,0,0);
		}
        
		//Strafe+
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Strafe1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Strafe1-")||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Strafe2+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Strafe2+"))
		{
			if (m_ModifiedInput.Mouse1) //turning
			{
				this.transform.localRotation *= Quaternion.AngleAxis (turn /32, new Vector3 (0, 1, 0));
				if (turn < 0) balance += 0.1F;
				else balance -= 0.1F;
			}

			if (velocity < 0.025F)
			{
				velocity = velocity + (Time.deltaTime * 0.5F); //acceleration
			}
			
			this.transform.Translate (velocity*Scale,0,0);
		}

		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Step2ToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Step2ToRun") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|RunGrowl"))
		{
			if (m_ModifiedInput.A) //turning
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, -1, 0));
				balance += 0.25F;
			}
			else if (m_ModifiedInput.D)
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, 1, 0));
				balance -= 0.25F;
			}

			if (velocity < 0.4F)
			{
				velocity = velocity + (Time.deltaTime * 1.5F); //acceleration
			}
			
			this.transform.Translate (0, 0, velocity*Scale);
		}

		//Attack
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Stand1ToAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Stand1ToAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Stand1ToAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Stand1ToAttackB"))
		{
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
			{
			velocity = velocity + (Time.deltaTime * 0.3F);
			this.transform.localRotation *= Quaternion.AngleAxis (4.0F, new Vector3 (0, -1, 0));
			}
			else velocity = 0.0F;
			this.transform.Translate (0, 0, velocity*Scale);
		}

		//Stop
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Anky|Stand1A") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|Stand2A") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Anky|AttackLoop"))
		{
			velocity =0.0F;
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
	}
}

