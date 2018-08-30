using UnityEngine;
using System.Collections;


public class steg_cs : MonoBehaviour
{
	Transform Spine0,Spine1,Spine2,Neck0, Neck1,Neck2,Neck3,Head,Jaw, Tail0,Tail1,Tail2,Tail3,Tail4,Tail5,Tail6,Tail7,Tail8;
	float turn,pitch,open,balance,temp,velocity,animcount,Scale = 0.0F;
	bool reset,soundplayed,isdead =false;
	int lodselect=0, skinselect =0;
	string infos;
	Animator anim;
	AudioSource source;
	LODGroup lods;
	SkinnedMeshRenderer[] rend;
	public Texture[] skin;
	public AudioClip Medstep,Idleherb,Steg_Roar1,Steg_Roar2,Steg_Roar3,Sniff2,Chew,Largestep;


	void Awake ()
	{
		Tail0 = this.transform.Find ("Steg/root/pelvis/tail0");
		Tail1 = this.transform.Find ("Steg/root/pelvis/tail0/tail1");
		Tail2 = this.transform.Find ("Steg/root/pelvis/tail0/tail1/tail2");
		Tail3 = this.transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = this.transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = this.transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = this.transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = this.transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = this.transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Spine0 = this.transform.Find ("Steg/root/spine0");
		Spine1 = this.transform.Find ("Steg/root/spine0/spine1");
		Spine2 = this.transform.Find ("Steg/root/spine0/spine1/spine2");
		Neck0 = this.transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = this.transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = this.transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Neck3 = this.transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3");
		Head = this.transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/head");
		Jaw = this.transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/head/jaw0");

		source = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		lods = GetComponent<LODGroup>();
		rend = GetComponentsInChildren <SkinnedMeshRenderer>();
	}



	void OnGUI ()
	{
		switch (skinselect)
		{
		case 0:
			if (GUI.Button (new Rect (5,40,80,20), "Skin A"))
			{
				rend[0].material.mainTexture = skin[1];
				rend[1].material.mainTexture = skin[1];
				rend[2].material.mainTexture = skin[1];
				skinselect=1;
			}
			break;
		case 1:
			if (GUI.Button (new Rect (5,40,80,20), "Skin B"))
			{
				rend[0].material.mainTexture = skin[2];
				rend[1].material.mainTexture = skin[2];
				rend[2].material.mainTexture = skin[2];
				skinselect=2;
			}
			break;
		case 2:
			if (GUI.Button (new Rect (5,40,80,20), "Skin C"))
			{
				rend[0].material.mainTexture = skin[0];
				rend[1].material.mainTexture = skin[0];
				rend[2].material.mainTexture = skin[0];
				skinselect=0;
			}
			break;
		}
		
		if(rend[0].isVisible)infos = rend[0].sharedMesh.triangles.Length/3+" triangles";
		else if (rend[1].isVisible)infos = rend[1].sharedMesh.triangles.Length/3+" triangles";
		else if (rend[2].isVisible)infos = rend[2].sharedMesh.triangles.Length/3+" triangles";
		
		switch (lodselect)
		{
		case 0:
			if (GUI.Button (new Rect (5,100,190,30), "LOD_Auto -> " + infos))
			{
				lods.ForceLOD(0);
				lodselect=1;
			}
			break;
		case 1:
			if (GUI.Button (new Rect (5,100,190,30), "LOD_0 -> " + infos))
			{
				lods.ForceLOD(1);
				lodselect=2;
			}
			
			break;
		case 2:
			if (GUI.Button (new Rect (5,100,190,30), "LOD_1 -> " + infos))
			{
				lods.ForceLOD(2);
				lodselect=3;
			}
			break;
		case 3:
			if (GUI.Button (new Rect (5,100,190,30), "LOD_2 -> " + infos))
			{
				lods.ForceLOD(-1);
				lodselect=0;
			}
			break;
		}

		GUI.Box (new Rect (0, 170, 200, 380), "Help");
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
		GUI.Label(new Rect(5,460,Screen.width,Screen.height),"num 6 = Die");
	}



	void Update ()
	{
		//***************************************************************************************
		//Moves animation controller
		if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.W)) anim.SetInteger ("State", 2); //Steps forward
		else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.W)) anim.SetInteger ("State", 3); //Run
		else if (Input.GetKey (KeyCode.W))anim.SetInteger ("State", 1); //Walk
		else if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.S)) anim.SetInteger ("State", -2); //Steps backward
		else if (Input.GetKey (KeyCode.S)) anim.SetInteger ("State", -1); //Walk backward
		else if (Input.GetKey (KeyCode.A)) anim.SetInteger ("State", 10); //Strafe+
		else if (Input.GetKey (KeyCode.D)) anim.SetInteger ("State", -10); //Strafe-
		else if (Input.GetKey (KeyCode.Space)) anim.SetInteger ("State", 100); //Steps
		else if (Input.GetKey (KeyCode.LeftControl)) anim.SetInteger ("State", -100); //Attack  pose
		else anim.SetInteger ("State", 0); //back to loop


		//Attack animation controller
		if (Input.GetKey (KeyCode.Mouse0))
			anim.SetBool ("Attack", true);
		else
			anim.SetBool ("Attack", false);


		//Growl animation controller
		if (Input.GetKey (KeyCode.E))
			anim.SetBool ("Growl", true);
		else
			anim.SetBool ("Growl", false);


		//Idles animation controller
		if (Input.GetKey (KeyCode.Alpha1))
			anim.SetInteger ("Idle", 1); //Idle 1
		else if (Input.GetKey (KeyCode.Alpha2))
			anim.SetInteger ("Idle", 2); //Idle 2
		else if (Input.GetKey (KeyCode.Alpha3))
			anim.SetInteger ("Idle", 3); //Eat
		else if (Input.GetKey (KeyCode.Alpha4))
			anim.SetInteger ("Idle", 4); //Drink
		else if (Input.GetKey (KeyCode.Alpha5))
			anim.SetInteger ("Idle", 5); //Sleep
		else if (Input.GetKey (KeyCode.Alpha6))
			anim.SetInteger ("Idle", 6); //Die
		else
			anim.SetInteger ("Idle", 0);


		//***************************************************************************************
		//Spine control
		if (Input.GetKey (KeyCode.Mouse1) && reset == false) {
			turn += Input.GetAxis ("Mouse X") * 1.0F;
			pitch += Input.GetAxis ("Mouse Y") * 1.0F;
		} else if (turn != 0.0F || pitch != 0.0F) {
			if (turn < 0.0F) {
				if (turn < -0.25F)
					turn += 0.25F;
				else
					turn = 0.0F; 
			} else if (turn > 0.0F) {
				if (turn > 0.25F)
					turn -= 0.25F;
				else
					turn = 0.0F; 
			}
			if (pitch < 0.0F)
			{
				if (pitch < -0.5F)
					pitch += 0.5F;
				else
				{
					pitch = 0.0F;
					reset = false;
				}
			}
			else if (pitch > 0.0F)
			{
				if (pitch > 0.5F)
					pitch -= 0.5F;
				else {
					pitch = 0.0F;
					reset = false;
				}
			}
		}


		//Jaw control
		if ((Input.GetKey (KeyCode.Mouse0)) &&
			(reset == false) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1A")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2A")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1B")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2C")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1Attack")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2Attack")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1Attack")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2Attack")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|WalkGrowl")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunGrowl")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunAttackA")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunAttackB")) &&
			(!anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|WalkShake"))
			)
			open -= 4.0F;
		else
			open += 1.0F;


		//Reset spine position during specific animation
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|EatC"))
			reset = true; else reset = false;
		
		
		//Reset tail and spine position
		if (((anim.GetInteger ("State") != 1) || (anim.GetInteger ("State") != 3)) && (balance != 0.0F))
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
	

		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if(anim.GetAnimatorTransitionInfo(0).normalizedTime!=0.0F) animcount=0.0F;
		animcount = Mathf.Round(animcount * 30);


		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Walk-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Walk-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1ToWalk-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1ToWalk-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe1+") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe1+") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe1-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe1-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe2+") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe2+") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe2-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe2-"))
		{
			
			if(anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
			   anim.GetNextAnimatorStateInfo (0).IsName ("Steg|WalkGrowl"))
			{
				if(animcount==4 && soundplayed==false)
				{
					source.pitch=Random.Range(1.0F, 1.25F);
					source.PlayOneShot(Steg_Roar2,Random.Range(0.75F, 1.0F));
					soundplayed=true;
				}
			}

			if(soundplayed==false &&(animcount==8 || animcount==22))
			{
				source.pitch=Random.Range(0.8F, 1.0F);
				source.PlayOneShot(Medstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=4 && animcount!=8 && animcount!=22) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToAttackB") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToAttackB") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit"))
		{
			if(animcount==10 && soundplayed==false)
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Medstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			else if( animcount!=10) soundplayed=false;
			
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|RunGrowl"))
		{
			if(anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunGrowl") ||
			   anim.GetNextAnimatorStateInfo (0).IsName ("Steg|RunGrowl"))
			{
				if(animcount==4 && soundplayed==false)
				{
					source.pitch=Random.Range(0.8F, 1.25F);
					source.PlayOneShot(Steg_Roar3,Random.Range(0.75F, 1.0F));
					soundplayed=true;
				}
			}
			else if(animcount==3 && soundplayed==false)
			{
				source.pitch=Random.Range(0.8F, 1.0F);
				source.PlayOneShot(Sniff2,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			if(soundplayed==false &&(animcount==8 || animcount==22))
			{
				source.pitch=Random.Range(0.8F, 1.0F);
				source.PlayOneShot(Medstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=3 && animcount!=4 && animcount!=8 && animcount!=22) soundplayed=false;
	
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1A") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2A") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SitLoop") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SleepLoop") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|EatA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|AttackLoop"))
		{

			if(anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|EatA") &&
			   animcount==20 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Chew,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}

			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Idleherb,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=15 && animcount!=20) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|EatB"))
		{
			
			if(soundplayed==false &&
			   (animcount==0 || animcount==10 || animcount==20) )
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Chew,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=0 && animcount!=10 && animcount!=20) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1B") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2B"))
		{
			
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Steg_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1C"))
		{
			
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Steg_Roar3,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2C"))
		{
			
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}

	
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1Call") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2Call") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SitCall"))
		{
			
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Steg_Roar2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}


		else if ( anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|SitGrowl"))
		{
			
			if(animcount==1 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Steg_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=1) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|AttackC"))
		{
			if(animcount==3 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Steg_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=3 && animcount!=5) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackA")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand2ToAttack"))
		{
			
			if(animcount==4 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Steg_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=4) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackB"))
		{
			if(animcount==3 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Steg_Roar2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=3 && animcount!=5) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|AttackLoopGrowl"))
		{
			
			if(animcount==3 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Steg_Roar3,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount==10 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Largestep,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=3 && animcount!=10) soundplayed=false;
		}


		else if (!isdead && (
				 anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Die1") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Die2")))
		{
			
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(0.5F, 0.75F);
				source.PlayOneShot(Steg_Roar3,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount==25 && soundplayed==false)
			{
				source.pitch=Random.Range(0.5F, 0.75F);
				source.PlayOneShot(Largestep,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=5 && animcount!=25) soundplayed=false;

			if(animcount>25) isdead=true;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Rise1") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Rise2"))
		{
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(0.75F,1.25F);
				source.PlayOneShot(Steg_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}

			else if(animcount!=5) soundplayed=false;
			isdead=false;
		}




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
		Tail6.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail7.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail8.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		
		//Jaw
		Jaw.transform.localRotation *= Quaternion.Euler (new Vector3 (open, 0, 0));

	}


	void FixedUpdate ()
	{
		//***************************************************************************************
		//Model translations and rotations

		//adjust speed to the model's scale
		Scale = this.transform.localScale.x;
		//adjust gravity to the model's scale
		Physics.gravity = new Vector3(0, -Scale*40.0f, 0);

		//Walking
		if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|WalkGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToEat") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToEat"))
		{

			if( anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2"))
			{
				if (Input.GetKey (KeyCode.A)) //turning
				{
					if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
					{
						this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, -1, 0));
					}
					balance += 0.2F;
				} 
				else if (Input.GetKey (KeyCode.D))
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
				if (Input.GetKey (KeyCode.A))
				{
					this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, -1, 0));
					balance += 0.2F;
				}
				else if (Input.GetKey (KeyCode.D))
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

		
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand1A") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand2A"))
			{
				velocity = velocity - (Time.deltaTime * 0.6F);
			}

			this.transform.Translate (0, 0, velocity*Scale);
		}


		//Backward walk
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step1ToWalk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1ToWalk-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Walk-") ||
				 anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit") ||
				 anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-ToSit"))
		{

			if( anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step1-") ||
			   anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2-"))
			{
				if (Input.GetKey (KeyCode.A)) //turning
				{
					if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.8)
					{
						this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, 1, 0));
					}
					balance += 0.25F;
				}
				else if (Input.GetKey (KeyCode.D))
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
				if (Input.GetKey (KeyCode.A)) //turning
				{
					this.transform.localRotation *= Quaternion.AngleAxis (0.4F, new Vector3 (0, 1, 0));
					balance += 0.25F;
				}
				else if (Input.GetKey (KeyCode.D))
				{
					this.transform.localRotation *= Quaternion.AngleAxis (0.2F, new Vector3 (0, -1, 0));
					balance -= 0.25F;
				}
			}
			
			if (velocity > -0.06F)
			{
				velocity = velocity - (Time.deltaTime * 0.5F); //acceleration
			}

			if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand1A") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand2A"))
			{
				velocity = velocity + (Time.deltaTime * 0.6F); //deceleration
			}
			
			this.transform.Translate (0, 0, velocity*Scale);
		}


		//Strafe-
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe1+") ||
				 anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe1+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe2-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe2-")
		         )
		{
			if (Input.GetKey (KeyCode.Mouse1)) //turning
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
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe1-")||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Strafe2+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Strafe2+"))
		{
			if (Input.GetKey (KeyCode.Mouse1)) //turning
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
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Step2ToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Step2ToRun") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|RunGrowl"))
		{
			if (Input.GetKey (KeyCode.A)) //turning
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, -1, 0));
				balance += 0.25F;
			}
			else if (Input.GetKey (KeyCode.D))
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
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand1ToAttackB"))
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
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Steg|Stand1A") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|Stand2A") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Steg|AttackLoop"))
		{
			velocity =0.0F;
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
	
	}

}




