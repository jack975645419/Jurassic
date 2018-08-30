using UnityEngine;
using System.Collections;


public class spino_cs : MonoBehaviour
{
	Transform Spine0,Spine1,Spine2,Neck0,Neck1,Neck2,Head,Jaw,Tail1,Tail2,Tail3,Tail4,Tail5,Tail6;
	float turn,pitch,open,balance,temp,velocity,animcount,Scale = 0.0F;
	bool reset,soundplayed,isdead =false;
	int lodselect=0, skinselect =0;
	string infos;
	Animator anim;
	AudioSource source;
	LODGroup lods;
	SkinnedMeshRenderer[] rend;
	public Texture[] skin;
	public AudioClip Bigstep,Idlecarn,Spino_Roar1,Spino_Roar2,Carngrowl1,Carngrowl2,Bite,Swallow,Sniff1;

	void Awake ()
	{
		Tail1 = this.transform.Find ("Spino/root/tail0/tail1");
		Tail2 = this.transform.Find ("Spino/root/tail0/tail1/tail2");
		Tail3 = this.transform.Find ("Spino/root/tail0/tail1/tail2/tail3");
		Tail4 = this.transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4");
		Tail5 = this.transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = this.transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Spine0 = this.transform.Find ("Spino/root/spine0");
		Spine1 = this.transform.Find ("Spino/root/spine0/spine1");
		Spine2 = this.transform.Find ("Spino/root/spine0/spine1/spine2");
		Neck0 = this.transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0");
		Neck1 = this.transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1");
		Neck2 = this.transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2");
		Head = this.transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2/head");
		Jaw = this.transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2/head/jaw0");

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
		GUI.Label(new Rect(5,300,Screen.width,Screen.height),"Space = Steps");
		GUI.Label(new Rect(5,320,Screen.width,Screen.height),"E = Growl");
		GUI.Label(new Rect(5,340,Screen.width,Screen.height),"num 1 = IdleA");
		GUI.Label(new Rect(5,360,Screen.width,Screen.height),"num 2 = IdleB");
		GUI.Label(new Rect(5,380,Screen.width,Screen.height),"num 3 = IdleC");
		GUI.Label(new Rect(5,400,Screen.width,Screen.height),"num 4 = Eat");
		GUI.Label(new Rect(5,420,Screen.width,Screen.height),"num 5 = Drink");
		GUI.Label(new Rect(5,440,Screen.width,Screen.height),"num 6 = Sleep");
		GUI.Label(new Rect(5,460,Screen.width,Screen.height),"num 7 = Die");
	}


	
	void Update ()
	{
		//***************************************************************************************
		//Moves animation controller
		if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W)) anim.SetInteger("State", 2); //Steps forward
		else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) anim.SetInteger("State", 3); //Run
		else if (Input.GetKey(KeyCode.W)) anim.SetInteger("State", 1); //Walk
		else if (Input.GetKey(KeyCode.S)) anim.SetInteger("State", -2); //Steps backward
		else if (Input.GetKey(KeyCode.A)) anim.SetInteger("State", 10); //Steps Strafe+
		else if (Input.GetKey(KeyCode.D)) anim.SetInteger("State", -10); //Steps Strafe-
		else if (Input.GetKey(KeyCode.Space)) anim.SetInteger("State", 100); //Space pressed
		else anim.SetInteger("State", 0); //back to loop
		
		
		//Attak animation controller
		if (Input.GetKey(KeyCode.Mouse0))
		{
			anim.SetInteger("Attack", 1); //Attak
		} else anim.SetInteger("Attack", 0);
		
		
		//Growl animation controller
		if (Input.GetKey (KeyCode.E))
		{
			anim.SetBool("Growl", true);
			
		} else anim.SetBool("Growl", false);
		
		
		//Idles animation controller
			if (Input.GetKey (KeyCode.Alpha1))
				anim.SetInteger ("Idle", 1); //Idle 1
			else if (Input.GetKey (KeyCode.Alpha2))
				anim.SetInteger ("Idle", 2); //Idle 2
			else if (Input.GetKey (KeyCode.Alpha3))
				anim.SetInteger ("Idle", 3); //Idle 3
			else if (Input.GetKey (KeyCode.Alpha4))
				anim.SetInteger ("Idle", 4); //Eat
			else if (Input.GetKey (KeyCode.Alpha5))
				anim.SetInteger ("Idle", 5); //Drink
			else if (Input.GetKey (KeyCode.Alpha6))
				anim.SetInteger ("Idle", 6); //Sleep
			else if (Input.GetKey (KeyCode.Alpha7))
				anim.SetInteger ("Idle", 7); //Die
			else
				anim.SetInteger ("Idle", 0);

	

		//***************************************************************************************
		//Spine control
		if (Input.GetKey(KeyCode.Mouse1) && reset == false)
		{
			turn += Input.GetAxis ("Mouse X") * 1.0F;
			pitch += -Input.GetAxis ("Mouse Y") * 1.0F;
		}
		else if(turn != 0.0F || pitch !=0.0F)
		{
			if(turn <0.0F)
			{
				if(turn <-0.25F) turn += 0.25F; else turn = 0.0F; 
			}
			else if(turn >0.0F)
			{
				if(turn >0.25F) turn -= 0.25F; else turn = 0.0F; 
			}
			if(pitch <0.0F)
			{
				if(pitch <-0.5F) pitch += 0.5F; else { pitch = 0.0F; reset =false; }
			}
			else if(pitch >0.0F)
			{
				if(pitch >0.5F) pitch -= 0.5F; else { pitch = 0.0F; reset =false; }
			}
		}
		

		//Reset spine position during specific animation
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|EatA") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|EatB") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|EatC") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Die1") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Die2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|SleepLoop") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2B") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2D") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkShake"))
			reset = true; else reset = false;
		

		//Reset tail and spine position
		if((anim.GetInteger("State") !=1) || (anim.GetInteger("State") !=3) && (balance != 0.0F))
		{
			if(balance <0.0F)
			{
				if(balance <-0.25F) balance += 0.25F; else balance = 0.0F; 
			}
			else if(balance >0.0F)
			{
				if(balance >0.25F) balance -= 0.25F; else balance = 0.0F; 
			}
		}


		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if(anim.GetAnimatorTransitionInfo(0).normalizedTime!=0.0F) animcount=0.0F;
		animcount = Mathf.Round(animcount * 30);
		
		
		if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand1B"))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Spino_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=0) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand1C"))
		{
			if(animcount==10 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff1,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff1,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			if(animcount==20 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Sniff1,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=10 && animcount!=15 && animcount!=20) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2B"))
		{
			if(animcount==5 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bite,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			if(animcount==10 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bite,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bite,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=5 && animcount!=10 && animcount!=15 ) soundplayed=false;
		}
		
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand2C"))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Spino_Roar2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=0) soundplayed=false;
		}
		
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand2D"))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Carngrowl1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=0) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand1A")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2A")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|SleepLoop") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|EatA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|EatA")
		         )
		{
			
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|EatA"))
			{
				if(animcount==5 && soundplayed==false)
				{
					source.pitch=Random.Range(1.0F, 1.25F);
					source.PlayOneShot(Carngrowl2,Random.Range(0.4F, 0.6F));
					soundplayed=true;
				}
			}
			
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Idlecarn,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=5 && animcount!=15) soundplayed=false;
			
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Strafe1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Strafe1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Strafe2+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Strafe2-"))
		{
			if(animcount==13 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount==25 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=13 && animcount!=25) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand1Attack") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2Attack"))
		{
			
			
			if(animcount==13 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bite,Random.Range(1.0F, 1.5F));
				soundplayed=true;
			}
			else if(animcount==21 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=13 && animcount!=15 && animcount!=21) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1+")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2+")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2ToStand1C") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2ToEatA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2ToEatC") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1Attack")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2Attack"))
		{
			
			if (animcount==1 && soundplayed==false && (
				anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1Attack")||
				anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2Attack")))
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Carngrowl1,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.25F, 0.5F));
				
				if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1Attack")||		  
				    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2Attack"))
				{
					source.pitch=Random.Range(1.0F, 1.25F);
					source.PlayOneShot(Bite,Random.Range(1.0F, 1.5F));
				}
				soundplayed=true;
			}
			else if(animcount!=1 && animcount!=15 ) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1-")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2-"))
		{
			if(animcount==25 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=25) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand1ToWalk"))
		{
			
			
			
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			else if(animcount==27 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			else if(animcount!=15 && animcount!=27) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2ToWalk"))
		{
			
			if(animcount==20 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			if(animcount!=20) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkToStand2"))
		{
			if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			else if(animcount!=15) soundplayed=false;
		}

		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand1ToRun"))
		{
			if(animcount==13 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount==25 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			
			else if(animcount!=13&&animcount!=25) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Walk")||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Walk")||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkShake") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkShake") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkAttackB") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunAttackB"))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.PlayOneShot(Bigstep,Random.Range(0.4F, 0.6F));
				
				if(anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkGrowl"))
				{
					source.PlayOneShot(Spino_Roar1,Random.Range(0.75F, 1.0F));
				}
				
				else if(anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunGrowl"))
				{
					source.PlayOneShot(Spino_Roar2,Random.Range(0.75F, 1.0F));
				}
				else if(anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkAttackA") ||
				        anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkAttackB") ||
				        anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunAttackA") ||
				        anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunAttackB"))
				{
					source.PlayOneShot(Carngrowl1,Random.Range(0.4F, 0.6F));
				}
				soundplayed=true;
			}
			
			else if(animcount==15 && soundplayed==false)
			{
				source.pitch=Random.Range (1.0F,1.25F);
				source.PlayOneShot(Bigstep,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount==20 && soundplayed==false && (
				anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkAttackA") ||
				anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkAttackB") ||
				anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunAttackA") ||
				anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunAttackB")))
			{
				source.pitch=Random.Range (1.0F,1.25F);
				source.PlayOneShot(Bite,Random.Range(1.0F, 1.5F));
				soundplayed=true;
			}
			
			else if(animcount!=0 && animcount!=15 && animcount!=20) soundplayed=false;
		}


		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|EatB"))
		{
			if(animcount==1 && soundplayed==false)
			{
				source.pitch=Random.Range(1.0F, 1.25F);
				source.PlayOneShot(Swallow,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=1) soundplayed=false;
		}
		
		
		else if (!isdead && (
			anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Die1") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Die2")))
		{
			if(animcount==0 && soundplayed==false)
			{
				source.pitch=Random.Range(0.5F, 0.5F);
				source.PlayOneShot(Carngrowl1,Random.Range(0.5F, 0.75F));
				soundplayed=true;
			}
			else if(animcount==25 && soundplayed==false)
			{
				source.pitch=Random.Range(0.9F, 0.9F);
				source.PlayOneShot(Bigstep,Random.Range(1.5F, 2.0F));
				soundplayed=true;
			}
			else if(animcount!=0 && animcount!=25 ) soundplayed=false;
			if (animcount>25) isdead=true;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Rise1") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Rise2")
		         )
		{
			isdead=false;
			if(animcount==2 && soundplayed==false)
			{
				
				source.pitch=Random.Range(0.75F, 0.75F);
				source.PlayOneShot(Carngrowl2,Random.Range(1.0F, 1.25F));
				soundplayed=true;
			}
			else if(animcount!=2) soundplayed=false;
			
		}

	}
	

	//***************************************************************************************
	//Clamp and set bone rotations
	void LateUpdate()
	{
		
		balance = Mathf.Clamp(balance, -11.0F, 11.0F);
		turn = Mathf.Clamp(turn, -25.0F, 25.0F);
		pitch = Mathf.Clamp(pitch, -10.0F, 13.0F);
		open = Mathf.Clamp(open, -40.0F, 0.0F);
		temp = turn-balance;
		temp += turn;
		temp = Mathf.Clamp(temp, -25.0F, 25.0F);
		
		//Spine and neck
		Spine0.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Spine1.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Spine2.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Neck0.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Neck1.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Neck2.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		Head.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, 0, -temp));
		
		//Tail
		Tail1.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail2.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail3.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail4.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail5.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		Tail6.transform.localRotation *= Quaternion.Euler (new Vector3 (0, 0, -balance));
		
		//Jaw
		Jaw.transform.localRotation *= Quaternion.AngleAxis (open, new Vector3 (-1, 0, 0));
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
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkToStand2") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkToStand2") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand1ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand1ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2ToWalk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand2ToWalk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkShake") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkShake"))
		{
			
			if(!anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand1A") &&
			   !anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand2A"))
			{
				
				if(Input.GetKey(KeyCode.A))
				{
					this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, -1, 0));
					balance += 0.5F;
				}
				else if(Input.GetKey(KeyCode.D))
				{
					this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, 1, 0));
					balance -= 0.5F;
				}
			}
			
			
			if (velocity < 0.3F)
			{
				velocity = velocity + (Time.deltaTime * 0.5F); //acceleration
			}
			else if (velocity > 0.3F)
			{
				velocity = velocity - (Time.deltaTime * 0.5F); //deceleration
			}
			
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkToStand2") &&
			    anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.6)
				velocity =0.0F;
		

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunToStand1") &&
			         anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.7)
				velocity =0.0F;
			else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunToStand1") && (velocity > 0.0F))
			{
				velocity = velocity - (Time.deltaTime * 0.25F); //deceleration
			}


			if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|EatA"))
				velocity =0.0F;
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		
		//Backward steps
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step2-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1ToStand2B") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step1ToStand2B") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1ToStand2D") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step1ToStand2D") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1ToSleep") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step1ToSleep"))
		{
			
			if(Input.GetKey(KeyCode.A))
			{
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.3 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.9)
				{ this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, 1, 0)); }
				balance += 0.5F;
			}
			else if(Input.GetKey(KeyCode.D))
			{
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.3 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.9 )
				{ this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, -1, 0)); }
				balance -= 0.5F;
			}
			
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.4 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.9)
			{
				if(velocity > -0.35F) velocity = velocity - (Time.deltaTime * 0.5F);
			}  else velocity =0.0F;
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		
		//Forward steps
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step2+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2ToStand1C") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step2ToStand1C") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2ToEatA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step2ToEatA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2ToEatC") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step2ToEatC"))
		{
			if(Input.GetKey(KeyCode.A))
			{
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.6)
				{ this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, -1, 0)); }
				balance += 0.5F;
			}
			else if(Input.GetKey(KeyCode.D))
			{
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.6)
				{ this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, 1, 0)); }
				balance -= 0.5F;
			} 
			
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.55)
			{
				if(velocity < 0.35F) velocity = velocity + (Time.deltaTime * 0.5F);
			}  else velocity =0.0F;
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		
		//Attack Steps 
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step1Attack") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step1Attack") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Step2Attack") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Step2Attack"))
		{
			if (velocity < 0.5F)
			{
				velocity = velocity + (Time.deltaTime * 1.5F); //acceleration
			}
			
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.6) velocity = 0.0F;
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		
		//Strafe+ Speed translation
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Strafe1+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Strafe1+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Strafe2-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Strafe2-"))
		{
			if (Input.GetKey(KeyCode.Mouse1))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (turn/32, new Vector3 (0, 1, 0));
			}
			
			if (velocity < 0.1F)
			{
				velocity = velocity + (Time.deltaTime * 0.5F); //acceleration
			}
			
			this.transform.Translate (-velocity*Scale,0,0);
		}
		
		
		//Strafe- Speed translation
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Strafe1-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Strafe1-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Strafe2+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Strafe2+"))
		{
			if (Input.GetKey(KeyCode.Mouse1))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (turn/32, new Vector3 (0, 1, 0));
			}
			
			if (velocity < 0.1F)
			{
				velocity = velocity + (Time.deltaTime * 0.5F); //acceleration
			}
			
			this.transform.Translate (velocity*Scale,0,0);
		}
		
		
		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunAttackB") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkAttackA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkAttackA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|WalkAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|WalkAttackB") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand1ToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand1ToRun") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Spino|Stand2ToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2ToRun"))
		{
			if(Input.GetKey(KeyCode.A))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, -1, 0));
				balance += 0.5F;
			}
			else if(Input.GetKey(KeyCode.D))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, 1, 0));
				balance -= 0.5F;
			}
			


			if (anim.GetNextAnimatorStateInfo (0).IsName ("Spino|RunGrowl") ||
				anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|RunGrowl"))
			{
				if (velocity > 0.6F)
				{
					velocity = velocity - (Time.deltaTime * 0.5F);
				}

			}
			if (velocity < 0.75F)
			{
				velocity = velocity + (Time.deltaTime * 1.5F);
			}
		

			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		
		//Stop
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand1A") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Spino|Stand2A"))
		{
			velocity =0.0F;
			this.transform.Translate (0, 0, velocity*Scale);
		}

	}

}
