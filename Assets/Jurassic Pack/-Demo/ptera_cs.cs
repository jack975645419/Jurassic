using UnityEngine;
using System.Collections;

public class ptera_cs : MonoBehaviour
{
	
	Transform Root,WingR,WingL,Neck0,Neck1,Neck2,Neck3,Neck4,Neck5,Neck6,Head,Jaw,
	Tail0,Tail1,Tail2,Tail3,Tail4,Tail5,Tail6,Tail7,Tail8,Tail9,Tail10,Tail11,Arm1,Arm2;
	float turn,pitch,open,balance,roll,pitch2,velocity,FlyX,FlyY,FlyZ,animcount,Scale = 0.0F;
	bool reset,soundplayed,isdead =false;
	int lodselect=0, skinselect =0;
	string infos;
	Animator anim;
	Rigidbody rg;
	AudioSource source;
	LODGroup lods;
	SkinnedMeshRenderer[] rend;
	public Texture[] skin;
	public AudioClip Smallstep,Idlecarn,Ptera_Roar1,Ptera_Roar2,Bite,Sniff2,Wind,Bigstep;
	
	
	void Awake ()
	{
		Root = this.transform.Find ("Ptera/root");
		WingR = this.transform.Find ("Ptera/root/spine0/spine1/spine2/right wing0");
		WingL = this.transform.Find ("Ptera/root/spine0/spine1/spine2/left wing0");
		Neck0 = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0");
		Neck1 = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1");
		Neck2 = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2");
		Neck3 = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3");
		Neck4  = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4");
		Neck5  = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5");
		Neck6  = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5/neck6");
		Head   = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5/neck6/head");
		Jaw    = this.transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5/neck6/head/jaw0");
		
		rg = GetComponent<Rigidbody>();
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
		GUI.Label(new Rect(5,280,Screen.width,Screen.height),"LeftShift = Run/Landing/Fly Down");
		GUI.Label(new Rect(5,300,Screen.width,Screen.height),"Space = Takeoff/Fly Up");
		GUI.Label(new Rect(5,320,Screen.width,Screen.height),"E = Growl");
		GUI.Label(new Rect(5,340,Screen.width,Screen.height),"num 1 = IdleA");
		GUI.Label(new Rect(5,360,Screen.width,Screen.height),"num 2 = IdleB");
		GUI.Label(new Rect(5,380,Screen.width,Screen.height),"num 3 = Eat");
		GUI.Label(new Rect(5,400,Screen.width,Screen.height),"num 4 = Drink");
		GUI.Label(new Rect(5,420,Screen.width,Screen.height),"num 5 = Sleep");
		GUI.Label(new Rect(5,440,Screen.width,Screen.height),"num 6 = Die");
	}
	
	
	
	void OnCollisionEnter(Collision collision )
	{
		if(collision.gameObject.name == "Ground") anim.SetBool("Onground", true);
	}
	void OnCollisionExit(Collision collision )
	{
		if(collision.gameObject.name == "Ground") anim.SetBool("Onground", false);
	}
	
	
	
	void Update ()
	{
		//***************************************************************************************
		//Moves animation controller
		
		//Attack
		if (Input.GetKey(KeyCode.Mouse0)) anim.SetBool ("Attack", true);
		else anim.SetBool ("Attack", false);
		
		//Moves animation controller
		if (Input.GetKey (KeyCode.Space)) anim.SetBool ("Fly", true);
		else anim.SetBool ("Fly", false);
		
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) anim.SetInteger("State", 3); //Run animation controller
		else if (Input.GetKey(KeyCode.LeftShift))anim.SetInteger("State", -3);
		else if (Input.GetKey(KeyCode.W)) anim.SetInteger("State", 1); //Walk animation controller
		else if (Input.GetKey(KeyCode.S)) anim.SetInteger("State", -1); //Walk backward animation controller
		else if (Input.GetKey(KeyCode.A))anim.SetInteger("State", 10); // Strafe+ animation controller
		else if (Input.GetKey(KeyCode.D))anim.SetInteger("State", -10); // Strafe- animation controller}
		else anim.SetInteger("State", 0); //Idle
		
		//Growl animation controller
		if (Input.GetKey (KeyCode.E)) anim.SetBool("Growl", true);
		else anim.SetBool("Growl", false);
		
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
		//Neck control
		if (Input.GetKey(KeyCode.Mouse1) && reset == false)
		{
			turn += Input.GetAxis ("Mouse X") * 1.0F;
			pitch += Input.GetAxis ("Mouse Y") * 1.0F;
		}
		else if(turn != 0.0F || pitch !=0.0F) // Reset neck
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
				if(pitch <-0.25F) pitch += 0.25F; else { pitch = 0.0F; reset =false; }
			}
			else if(pitch >0.0F)
			{
				if(pitch >0.25F) pitch -= 0.25F; else { pitch = 0.0F; reset =false; }
			}
		}
		
		//Root control
		if (anim.GetBool("Onground")==false && (
			Input.GetKey (KeyCode.A) ||
			Input.GetKey (KeyCode.D) ||
			Input.GetKey (KeyCode.LeftShift) ||
			Input.GetKey (KeyCode.Space)))
		{
			if(Input.GetKey (KeyCode.A)) { roll -= 0.5F;  balance += 2.0F;  }
			if(Input.GetKey (KeyCode.D)) { roll += 0.5F;  balance -= 2.0F;  }
			if(Input.GetKey (KeyCode.LeftShift)) pitch2 += 0.5F;
			if(Input.GetKey (KeyCode.Space)) pitch2 -= 0.5F;
		}
		else if(roll != 0.0F || pitch2 !=0.0F || balance !=0.0F) // Reset
		{
			if(roll <0.0F)
			{
				if(roll <-0.25F) roll += 0.25F; else roll = 0.0F; 
			}
			else if(roll >0.0F)
			{
				if(roll >0.25F) roll -= 0.25F; else roll = 0.0F; 
			}
			if(pitch2 <0.0F)
			{
				if(pitch2 <-0.25F) pitch2 += 0.25F; else pitch2 = 0.0F;
			}
			else if(pitch2 >0.0F)
			{
				if(pitch2 >0.25F) pitch2 -= 0.25F; else pitch2 = 0.0F;
			}
			if(balance <0.0F)
			{
				if(balance <-0.5F) balance += 0.5F; else balance = 0.0F;
			}
			else if(balance >0.0F)
			{
				if(balance >0.5F) balance -= 0.5F; else balance = 0.0F;
			}
		}
		
		
		//Jaw control
		if ((anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StandA") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Run") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Walk") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Walk-") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Strafe+") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Strafe-")) &&
		    (Input.GetKey(KeyCode.Mouse0)) && (reset == false))
			open-=2.0F; else open+=1.0F;
		
		
		//Reset Neck
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StandB"))
			reset = true; else reset = false;
		
		
		
		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if(anim.GetAnimatorTransitionInfo(0).normalizedTime!=0.0F) animcount=0.0F;
		animcount = Mathf.Round(animcount * 30);
		
		if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Stationary") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Stationary") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|StationaryUp") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StationaryUp") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|StationaryGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StationaryGrowl") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Landing") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Landing") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Takeoff") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Takeoff")
		    )
		{
			
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Landing") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Landing"))
			{
				if(soundplayed==false &&(animcount==9))
				{
					source.pitch=Random.Range(0.9F, 1.0F);
					source.PlayOneShot(Smallstep,Random.Range(0.4F, 0.6F));
					soundplayed=true;
				}
				
			}
			else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|StationaryGrowl") ||
			         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StationaryGrowl"))
			{
				if(soundplayed==false &&(animcount==5))
				{
					source.pitch=Random.Range(1.0F, 1.1F);
					source.PlayOneShot(Ptera_Roar2,Random.Range(0.75F, 1.0F));
					soundplayed=true;
				}
			}
			
			if(soundplayed==false &&(animcount==10))
			{
				source.pitch=Random.Range(1.0F, 1.1F);
				source.PlayOneShot(Sniff2,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=5 && animcount!=9 && animcount!=10) soundplayed=false;
		}
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Flight") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Flight") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Glide") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Glide") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|GlideGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|GlideGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|FlightGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|FlightGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Dive") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Dive"))
		{
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Flight") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Flight"))
			{
				if(soundplayed==false &&(animcount==10))
				{
					source.pitch=Random.Range(1.0F, 1.1F);
					source.PlayOneShot(Sniff2,Random.Range(0.4F, 0.6F));
					soundplayed=true;
				}
			}
			
			else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|GlideGrowl") ||
			         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|GlideGrowl") ||
			         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|FlightGrowl") ||
			         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|FlightGrowl"))
			{
				if(soundplayed==false &&(animcount==5))
				{
					source.pitch=Random.Range(0.9F, 1.1F);
					source.PlayOneShot(Ptera_Roar2,Random.Range(0.75F, 1.0F));
					soundplayed=true;
				}
			}
			
			if(soundplayed==false &&(animcount==1))
			{
				source.pitch=Random.Range(1.0F, 1.1F);
				source.PlayOneShot(Wind,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=1 && animcount!=5 && animcount!=10) soundplayed=false;
		}
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Walk") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Walk") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Strafe+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Strafe+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Strafe-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Strafe-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Walk-"))
		{
			if(soundplayed==false &&(animcount==10))
			{
				source.pitch=Random.Range(1.1F, 1.25F);
				source.PlayOneShot(Smallstep,0.1F);
				soundplayed=true;
			}
			else if(soundplayed==false &&(animcount==25))
			{
				source.pitch=Random.Range(1.1F, 1.25F);
				source.PlayOneShot(Smallstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=10 && animcount!=25) soundplayed=false;
		}
		
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|RunToFlight") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|RunToFlight") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|FlightToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|FlightToRun")
		         
		         
		         )
		{
			if(soundplayed==false &&(animcount==20))
			{
				source.pitch=Random.Range(1.1F, 1.25F);
				source.PlayOneShot(Smallstep,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(soundplayed==false &&(animcount==25))
			{
				source.pitch=Random.Range(1.1F, 1.25F);
				source.PlayOneShot(Sniff2,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(animcount!=20 && animcount!=25) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StandGrowl"))
		{
			if(soundplayed==false &&(animcount==3))
			{
				source.pitch=Random.Range(0.9F, 1.25F);
				source.PlayOneShot(Ptera_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			
			if(animcount!=3) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|AttackFly"))
		{
			if(soundplayed==false &&(animcount==10))
			{
				source.pitch=Random.Range(1.5F, 1.8F);
				source.PlayOneShot(Bite,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			if(animcount !=10) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StandC"))
		{
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(0.9F, 1.25F);
				source.PlayOneShot(Ptera_Roar1,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			else if(soundplayed==false &&(animcount==8))
			{
				source.pitch=Random.Range(1.0F, 1.1F);
				source.PlayOneShot(Sniff2,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			if(animcount!=5 && animcount!=8) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|SleepLoop"))
		{
			if(soundplayed==false &&(animcount==15))
			{
				source.pitch=Random.Range(1.5F, 1.6F);
				source.PlayOneShot(Idlecarn,Random.Range(0.4F, 0.6F));
				soundplayed=true;
			}
			
			if(animcount!=15) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Fall"))
		{
			
			if(soundplayed==false &&(animcount==3))
			{
				source.pitch=Random.Range(1.0F, 1.6F);
				source.PlayOneShot(Ptera_Roar2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			
			if(animcount!=3) soundplayed=false;
		}
		
		
		else if (!isdead && anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Die1"))
		{
			
			if(soundplayed==false &&(animcount==3))
			{
				source.pitch=Random.Range(1.5F, 1.6F);
				source.PlayOneShot(Ptera_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			
			if(soundplayed==false &&(animcount==25))
			{
				source.pitch=Random.Range(1.5F, 1.6F);
				source.PlayOneShot(Bigstep,Random.Range(0.75F, 1.0F));
				soundplayed=true;
				isdead = true;
			}
			
			if(animcount!=3 && animcount!=25) soundplayed=false;
		}
		
		
		else if (!isdead && anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Die2"))
		{
			if(soundplayed==false &&(animcount==15))
			{
				source.pitch=Random.Range(1.5F, 1.6F);
				source.PlayOneShot(Bigstep,Random.Range(0.75F, 1.0F));
				soundplayed=true;
				isdead = true;
			}
			
			if(animcount!=15) soundplayed=false;
		}
		
		
		else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Rise"))
		{
			isdead = false;
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(1.0F, 1.1F);
				source.PlayOneShot(Ptera_Roar1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			
			if(animcount!=5) soundplayed=false;
		}
		
	}
	
	
	
	//***************************************************************************************
	//Clamp and set bone rotations
	void LateUpdate()
	{
		open = Mathf.Clamp(open, -40.0F, 0.0F);
		turn = Mathf.Clamp (turn, -15.0F, 15.0F);
		pitch = Mathf.Clamp(pitch, -12.0F, 12.0F);
		
		roll = Mathf.Clamp(roll, -30.0F, 30.0F);
		pitch2 = Mathf.Clamp(pitch2, -15.0F, 20.0F);
		balance = Mathf.Clamp(balance, -10.0F, 10.0F);
		
		//Root
		Root.transform.localRotation *= Quaternion.Euler (new Vector3 (roll, pitch2, 0));
		
		//Wings
		WingR.transform.localRotation *= Quaternion.Euler (new Vector3 (0, balance, 0));
		WingL.transform.localRotation *= Quaternion.Euler (new Vector3 (0, -balance, 0));
		
		//Neck and head
		Neck0.transform.localRotation *= Quaternion.Euler (new Vector3 (0, pitch, -turn));
		Neck1.transform.localRotation *= Quaternion.Euler (new Vector3 (0, pitch, -turn));
		Neck2.transform.localRotation *= Quaternion.Euler (new Vector3 (0, pitch, -turn));
		Neck3.transform.localRotation *= Quaternion.Euler (new Vector3 (0, pitch, -turn));
		Neck4.transform.localRotation *= Quaternion.Euler (new Vector3 (0, pitch, -turn));
		Neck5.transform.localRotation *= Quaternion.Euler (new Vector3 (0, pitch, -turn));
		Neck6.transform.localRotation *= Quaternion.Euler (new Vector3 (0, pitch, -turn));
		Head.transform.localRotation *= Quaternion.Euler (new Vector3 (0, pitch, -turn));
		
		//Jaw
		Jaw.transform.localRotation *= Quaternion.AngleAxis (open, new Vector3 (0, -1, 0));
	}
	
	
	
	void FixedUpdate ()
	{
		//***************************************************************************************
		//Model translations and rotations
		//Walking
		
		//adjust speed to the model's scale
		Scale = this.transform.localScale.x;
		//adjust gravity to the model's scale
		Physics.gravity = new Vector3(0, -Scale*40.0f, 0);
		
		if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Walk"))
		{
			FlyZ=0.0F;
			
			if (Input.GetKey (KeyCode.A))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, -1, 0));
			}
			else if (Input.GetKey (KeyCode.D))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, 1, 0));
			}
			
			if (velocity < 0.1F)
			{
				velocity = velocity + (Time.deltaTime * 1.0F); //acceleration
			}
			else if (velocity > 0.1F) //deceleration
			{
				velocity = velocity - (Time.deltaTime * 1.0F);
			}
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		//Backward
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Walk-"))
		{
			if (Input.GetKey (KeyCode.A))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, -1, 0));
			}
			else if (Input.GetKey (KeyCode.D))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, 1, 0));
			}
			
			if (velocity > -0.075F)
			{
				velocity = velocity - (Time.deltaTime * 1.0F); //acceleration
			}
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		
		//Strafe+
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Strafe+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Strafe+"))
		{
			
			if (Input.GetKey (KeyCode.Mouse1)) //turning
			{
				this.transform.localRotation *= Quaternion.AngleAxis (turn /8, new Vector3 (0, 1, 0));
			}
			
			if (velocity > -0.05F)
			{
				velocity = velocity - (Time.deltaTime * 1.0F); //acceleration
			}
			
			this.transform.Translate (-velocity*Scale, 0, 0);
		}
		
		
		//Strafe-
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Strafe-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Strafe-"))
		{
			
			if (Input.GetKey (KeyCode.Mouse1)) //turning
			{
				this.transform.localRotation *= Quaternion.AngleAxis (turn /8, new Vector3 (0, 1, 0));
			}
			
			if (velocity > -0.05F)
			{
				velocity = velocity - (Time.deltaTime * 1.0F); //acceleration
			}
			
			this.transform.Translate (velocity*Scale, 0, 0);
		}
		
		
		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Run"))
		{
			if (Input.GetKey (KeyCode.A))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, -1, 0));
			}
			else if (Input.GetKey (KeyCode.D))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (1.0F, new Vector3 (0, 1, 0));
			}
			
			if (velocity < 0.5F)
			{
				velocity = velocity + (Time.deltaTime * 1.0F); //acceleration
			}
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		
		//Running to Fly
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|RunToFlight") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|RunToFlight"))
		{
			rg.useGravity = true;
			
			if (velocity < 0.75F)
			{
				velocity = velocity + (Time.deltaTime * 2.0F); //acceleration
				FlyZ = velocity;
			}
			
			this.transform.Translate (0, 0, velocity*Scale);
		}
		
		
		//Fly to Run
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|FlightToRun") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|FlightToRun"))
		{
			rg.useGravity = true;
			
			if (FlyZ > 0.5F)
			{
				FlyZ = FlyZ - (Time.deltaTime * 1.0F); //acceleration
				velocity =FlyZ;
			}
			FlyX = FlyY = 0.0F;
			this.transform.Translate (0, 0, FlyZ*Scale);
		}
		
		
		//Stand
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|StandA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StandA") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Landing") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Landing"))
		{
			rg.useGravity = true;
			velocity = 0.0F;
			
			this.transform.Translate (0, 0, velocity);
		}
		
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Takeoff") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Takeoff"))
		{
			rg.useGravity = false;
		}
		
		
		//Fly - Stationary
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Stationary") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Stationary") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|StationaryUp") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StationaryUp") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|StationaryGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|StationaryGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|AttackFly") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|AttackFly"))
		{
			rg.useGravity = false;
			
			if (Input.GetKey (KeyCode.Space))
			{ 
				if (FlyY < 1.0F)
				{
					FlyY = FlyY + (Time.deltaTime * 0.5F); //Up
				}
			}
			else if (Input.GetKey (KeyCode.LeftShift) && anim.GetBool("Onground")==false)
			{ 
				if (FlyY > -1.0F)
				{
					FlyY = FlyY - (Time.deltaTime * 0.5F); //Down
				}
			}
			else if (FlyY > -0.25F && anim.GetBool("Onground")==false) FlyY = FlyY - (Time.deltaTime * 0.25F); //Gravity
			else if (anim.GetBool("Onground")==true) FlyY =0.0F; // On ground ? Stop
			
			
			
			if (Input.GetKey (KeyCode.W))
			{
				if (FlyY < 1.0F)
				{
					FlyY = FlyY + (Time.deltaTime * 0.5F); //Up
				}
				
				if (FlyZ < 1.0F)
				{
					FlyZ = FlyZ + (Time.deltaTime * 1.0F); //Forward
				}
			}
			else if (Input.GetKey (KeyCode.S))
			{
				if (FlyZ > -0.5F)
				{
					FlyZ = FlyZ - (Time.deltaTime * 1.0F); //Forward
				}
			}
			else if (FlyZ > 0.0F) FlyZ = FlyZ - (Time.deltaTime * 0.25F); //Forward Deceleration
			else if (FlyZ < 0.0F) FlyZ = FlyZ + (Time.deltaTime * 0.25F); //Backward Deceleration
			else FlyZ =0.0F;
			
			if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.Mouse1))
			{
				if(FlyX > -0.5F) FlyX = FlyX - (Time.deltaTime * 0.5F); //Strafe
				
			}
			else if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.Mouse1))
			{
				if(FlyX < 0.5F) FlyX = FlyX + (Time.deltaTime * 0.5F); //Strafe
			}
			else if (FlyX > 0.0F) FlyX = FlyX - (Time.deltaTime * 0.5F);
			else if (FlyX < 0.0F) FlyX = FlyX + (Time.deltaTime * 0.5F);
			
			this.transform.localRotation *= Quaternion.AngleAxis (1.5F, new Vector3 (0, roll, 0)); //turn
			this.transform.Translate (FlyX*Scale, FlyY*Scale, FlyZ*Scale);
		}
		
		
		//Fly
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Dive") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Dive") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Flight") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Flight") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|FlightGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|FlightGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Glide") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Glide") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|GlideGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|GlideGrowl"))
		{
			rg.useGravity = false;
			
			if (Input.GetKey (KeyCode.LeftShift) && anim.GetBool("Onground") == false)
			{
				if (FlyY > -1.0F)
				{
					FlyY = FlyY - (Time.deltaTime * 1.0F); //down
				}
				
				if (FlyZ < 1.0F)
				{
					FlyZ = FlyZ + (Time.deltaTime * 0.25F); //forward
				}
			}
			else if (Input.GetKey (KeyCode.Space))
			{
				if (FlyY < 1.0F)
				{
					if(anim.GetBool("Onground") == true && anim.GetBool("Fly") == true)
						anim.SetBool("Onground", false);
					FlyY = FlyY + (Time.deltaTime * 0.5F); //up
				}
				
				if (FlyZ < 1.0F)
				{
					FlyZ = FlyZ + (Time.deltaTime * 0.5F); //forward
				}
				
			}
			
			else
			{
				if (FlyY > 0.0F && anim.GetBool("Onground")==false) FlyY = FlyY - (Time.deltaTime * 0.5F); //Up Deceleration
				else if (FlyY < 0.1F && anim.GetBool("Onground")==false) FlyY = FlyY + (Time.deltaTime * 0.5F); //Down Deceleration
				if (FlyZ > 0.5F) FlyZ = FlyZ - (Time.deltaTime * 0.1F); //forward Deceleration
			}
			
			
			if (FlyZ < 0.75F) FlyZ = FlyZ + (Time.deltaTime * 2.0F); //forward glide min speed
			
			this.transform.localRotation *= Quaternion.AngleAxis (0.5F, new Vector3 (0, roll, 0)); //turn 
			
			if (anim.GetBool("Onground")==true) FlyY =0.0F; // On ground ? stop
			
			this.transform.Translate (0, FlyY*Scale, FlyZ*Scale);
		}
		
		
		//Die
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Ptera|Fall") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Ptera|Fall"))
		{
			
			if (FlyX <-0.1F) FlyX += 0.01F; 
			else if (FlyX > 0.1F) FlyX -= 0.01F; 
			else FlyX = 0.0F;
			
			if (FlyY <-0.1F) FlyY += 0.01F; 
			else if (FlyY > 0.1F) FlyY -= 0.01F; 
			else FlyY = 0.0F;
			
			if (FlyZ <-0.1F) FlyZ += 0.01F; 
			else if (FlyZ > 0.1F) FlyZ -= 0.01F; 
			else FlyZ = 0.0F;
			
			rg.useGravity = true;
			this.transform.Translate (FlyX*Scale, FlyY*Scale, FlyZ*Scale);
		}
		
		
		
		
	}
}










