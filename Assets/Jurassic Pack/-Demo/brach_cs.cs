using UnityEngine;
using System.Collections;

public class brach_cs : MonoBehaviour
{
	Transform Spine0,Spine1,Spine2,Head,Jaw,Tail0,Tail1,Tail2,Tail3,Tail4,Tail5,Tail6,Tail7,Tail8,
	Neck0, Neck1,Neck2,Neck3,Neck4,Neck5,Neck6,Neck7,Neck8,Neck9,Neck10,Neck11,Neck12,Neck13,Neck14,Neck15,Neck16;
	float turn,pitch,open,balance,velocity,animcount,Scale = 0.0F;
	bool reset,soundplayed,isdead =false;
	int lodselect=0, skinselect =0;
	string infos;
	Animator anim;
	AudioSource source;
	LODGroup lods;
	SkinnedMeshRenderer[] rend;
	public Texture[] skin;
	public AudioClip Largestep,Idleherb,Brach_Roar1,Brach_Roar2,Brach_Call1,Brach_Call2,Chew;


	void Awake ()
	{
		Tail0 = this.transform.Find ("Brach/root/pelvis/tail0");
		Tail1 = this.transform.Find ("Brach/root/pelvis/tail0/tail1");
		Tail2 = this.transform.Find ("Brach/root/pelvis/tail0/tail1/tail2");
		Tail3 = this.transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = this.transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = this.transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = this.transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = this.transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = this.transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Spine0 = this.transform.Find ("Brach/root/spine0");
		Spine1 = this.transform.Find ("Brach/root/spine0/spine1");
		Spine2 = this.transform.Find ("Brach/root/spine0/spine1/spine2");
		Neck0 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Neck3 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3");
		Neck4 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4");
		Neck5 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5");
		Neck6 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6");
		Neck7 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7");
		Neck8 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8");
		Neck9 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9");
		Neck10 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10");
		Neck11 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11");
		Neck12 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12");
		Neck13 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13");
		Neck14 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14");
		Neck15 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15");
		Neck16 = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15/neck16");
		Head = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15/neck16/head");
		Jaw = this.transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15/neck16/head/jaw0");

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
		GUI.Label(new Rect(5,240,Screen.width,Screen.height),"Left Mouse = Open Jaw");
		GUI.Label(new Rect(5,260,Screen.width,Screen.height),"W,A,S,D = Moves");
		GUI.Label(new Rect(5,280,Screen.width,Screen.height),"LeftShift = Run");
		GUI.Label(new Rect(5,300,Screen.width,Screen.height),"Space = Rise");
		GUI.Label(new Rect(5,320,Screen.width,Screen.height),"E = Growl");
		GUI.Label(new Rect(5,340,Screen.width,Screen.height),"num 1 = Eat");
		GUI.Label(new Rect(5,360,Screen.width,Screen.height),"num 2 = Drink");
		GUI.Label(new Rect(5,380,Screen.width,Screen.height),"num 3 = Sit/Sleep");
		GUI.Label(new Rect(5,400,Screen.width,Screen.height),"num 4 = Die");
	}
	
	
	void Update ()
	{
		//***************************************************************************************
		//Moves animation controller
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) anim.SetInteger("State", 3); //Run animation controller
		else if (Input.GetKey(KeyCode.W)) anim.SetInteger("State", 1); //Walk animation controller
		else if (Input.GetKey(KeyCode.S)) anim.SetInteger("State", -1); //Walk backward animation controller
		else if (Input.GetKey(KeyCode.A))anim.SetInteger("State", 10); // Strafe+ animation controller
		else if (Input.GetKey(KeyCode.D))anim.SetInteger("State", -10); // Strafe- animation controller}
		else anim.SetInteger("State", 0); //Idle

		//Growl animation controller
		if (Input.GetKey (KeyCode.E)) anim.SetBool("Growl", true);
		else anim.SetBool("Growl", false);

		if (Input.GetKey (KeyCode.Alpha1))
			anim.SetInteger ("Idle", 1); //Eat 1
		else if (Input.GetKey (KeyCode.Alpha2))
			anim.SetInteger ("Idle", 2); //Drink 2
		else if (Input.GetKey (KeyCode.Space))
			anim.SetInteger ("Idle", 3); //Rise
		else if (Input.GetKey (KeyCode.Alpha3))
			anim.SetInteger ("Idle", 4); //Sit/sleep
		else if (Input.GetKey (KeyCode.Alpha4))
			anim.SetInteger ("Idle", 5); //Die
		else
			anim.SetInteger ("Idle", 0);


		//***************************************************************************************
		//Neck control
		if (Input.GetKey(KeyCode.Mouse1) && reset == false)
		{
			turn += Input.GetAxis ("Mouse X") * 0.5F;
			pitch += Input.GetAxis ("Mouse Y") * 0.5F;
		}
		else if(turn != 0.0F || pitch !=0.0F) // Reset neck
		{
			if(turn <0.0F)
			{
				if(turn <-0.1F) turn += 0.1F; else turn = 0.0F; 
			}
			else if(turn >0.0F)
			{
				if(turn >0.1F) turn -= 0.1F; else turn = 0.0F; 
			}
			if(pitch <0.0F)
			{
				if(pitch <-0.1F) pitch += 0.1F; else { pitch = 0.0F; reset =false; }
			}
			else if(pitch >0.0F)
			{
				if(pitch >0.1F) pitch -= 0.1F; else { pitch = 0.0F; reset =false; }
			}
		}
		
		
		//Jaw control
		if (
			(Input.GetKey(KeyCode.Mouse0)) && (reset == false))
			open-=4.0F; else open+=1.0F;

		
		//Reset spine position while any specific animation
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|EatB"))
			reset = true; else reset = false;
		

		//Reset tail and spine position
		if((anim.GetInteger("State") !=1) || (anim.GetInteger("State") !=3) && (balance != 0.0F))
		{
			if(balance <0.0F)
			{
				if(balance <-0.1F) balance += 0.1F; else balance = 0.0F; 
			}
			else if(balance >0.0F)
			{
				if(balance >0.1F) balance -= 0.1F; else balance = 0.0F; 
			}
		}


		//***************************************************************************************
		//Sound Fx code
		
		//Get current animation point
		animcount = (anim.GetCurrentAnimatorStateInfo (0).normalizedTime) % 1.0F;
		if(anim.GetAnimatorTransitionInfo(0).normalizedTime!=0.0F) animcount=0.0F;
		animcount = Mathf.Round(animcount * 30);

		if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|StandA") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|StandA") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Brach|EatB") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|EatB") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Brach|RiseStand") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|RiseStand") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Brach|SitA") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|SitA") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Sleep") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Sleep"))
		{
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(0.5F, 1.0F);
				source.PlayOneShot(Idleherb,Random.Range(0.5F, 0.75F));
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|StandB") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|StandB") ||
		    	 anim.GetNextAnimatorStateInfo (0).IsName ("Brach|RiseGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|RiseGrowl"))
		{
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Brach_Call2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|EatA") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|EatA"))
		{
			if(soundplayed==false &&(animcount==11))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Chew,Random.Range(0.5F, 0.75F));
				soundplayed=true;
			}
			else if(animcount!=11) soundplayed=false;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|EatC") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|EatC"))
		{
			if(soundplayed==false &&(animcount==5 || animcount==20))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Chew,Random.Range(0.1F, 0.25F));
				soundplayed=true;
			}
			else if(animcount!=5 && animcount!=20) soundplayed=false;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Sitting") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Sitting")||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Rising") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Rising"))
		{
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Brach_Call2,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|SitB") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|SitB"))
		{
			if(soundplayed==false &&(animcount==5))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Brach_Call1,Random.Range(0.75F, 1.0F));
				soundplayed=true;
			}
			else if(animcount!=5) soundplayed=false;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Rise") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Rise"))
		{
			if(soundplayed==false &&(animcount==9))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Brach_Roar1,Random.Range(1.0F, 1.5F));
				soundplayed=true;
			}
			else if(soundplayed==false &&(animcount==15))
			{
				source.PlayOneShot(Brach_Call2,Random.Range(1.0F, 1.5F));
				soundplayed=true;
			}
			else if(animcount!=9 && animcount!=15) soundplayed=false;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Rise-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Rise-"))
		{
			if(soundplayed==false &&(animcount==3))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Brach_Call2,Random.Range(1.0F, 1.5F));
				soundplayed=true;
			}
			if(soundplayed==false &&(animcount==10))
			{
				source.PlayOneShot(Largestep,Random.Range(2.0F, 2.5F));
				soundplayed=true;
			}
		
			else if(animcount!=3 && animcount!=10) soundplayed=false;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Walk") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Walk") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Walk-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Walk-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|WalkGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|WalkGrowl") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|WalkGrowl-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|WalkGrowl-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Strafe-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Strafe-") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Strafe+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Strafe+") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|RunGrowl"))
		{
			
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|WalkGrowl") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|WalkGrowl") ||
			    anim.GetNextAnimatorStateInfo (0).IsName ("Brach|WalkGrowl-") ||
			    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|WalkGrowl-"))
			{
				if(soundplayed==false &&(animcount==5))
				{
					source.pitch=Random.Range(0.8F, 1.2F);
					source.PlayOneShot(Brach_Roar2,Random.Range(1.0F, 1.5F));
					soundplayed=true;
				}
				else if(animcount!=5) soundplayed=false;
			}
			else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|RunGrowl") ||
			         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|RunGrowl"))
			{
				if(soundplayed==false &&(animcount==5))
				{
					source.pitch=Random.Range(0.8F, 1.2F);
					source.PlayOneShot(Brach_Roar1,Random.Range(1.0F, 1.5F));
					soundplayed=true;
				}
				else if(animcount!=5) soundplayed=false;
			}
			else if(soundplayed==false &&(
				animcount==10 || animcount==25))
			{
				source.pitch=Random.Range(0.8F, 1.0F);
				source.PlayOneShot(Largestep,Random.Range(0.25F, 0.4F));
				soundplayed=true;
			}
			else if(animcount!=10 && animcount!=25 && animcount!=30) soundplayed=false;
		}


		else if (!isdead && (
				 anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Die") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Die")))
		{
			if(soundplayed==false &&(animcount==4))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Brach_Call2,Random.Range(1.0F, 1.5F));
				soundplayed=true;
			}
			if(soundplayed==false &&(animcount==20))
			{
				source.pitch=Random.Range(0.75F, 0.75F);
				source.PlayOneShot(Largestep,Random.Range(2.0F, 2.5F));
				soundplayed=true;
			}
			
			else if(animcount!=4 && animcount!=20) soundplayed=false;

			if(animcount>20) isdead=true;
		}


		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Die-") ||
			     anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Die-"))
		{
			isdead=false;
			if(soundplayed==false &&(animcount==2))
			{
				source.pitch=Random.Range(0.75F, 1.0F);
				source.PlayOneShot(Brach_Roar1,Random.Range(1.0F, 1.5F));
				soundplayed=true;
			}

			
			else if(animcount!=2) soundplayed=false;
		}

	}


	//***************************************************************************************
	//Clamp and set bone rotations
	void LateUpdate()
	{
		
		balance = Mathf.Clamp(balance, -10.0F, 10.0F);
		open = Mathf.Clamp(open, -30.0F, 0.0F);
		turn = Mathf.Clamp (turn, -10.0F, 10.0F);
		pitch = Mathf.Clamp(pitch, -5.0F, 15.0F);

		//Neck and head
		Neck0.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck1.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck2.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck3.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck4.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck5.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck6.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck7.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck8.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck9.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck10.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck11.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck12.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck13.transform.localRotation *= Quaternion.Euler (new Vector3 (-pitch, turn, -turn));
		Neck14.transform.localRotation *= Quaternion.Euler (new Vector3 (pitch, 0, balance));
		Neck15.transform.localRotation *= Quaternion.Euler (new Vector3 (pitch, 0, balance));
		Neck16.transform.localRotation *= Quaternion.Euler (new Vector3 (pitch, 0, balance));
		Head.transform.localRotation *= Quaternion.Euler (new Vector3 (pitch, 0, balance));

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
		Jaw.transform.localRotation *= Quaternion.AngleAxis (open, new Vector3 (1, 0, 0));
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

		if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Walk") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Walk") ||
		    anim.GetNextAnimatorStateInfo (0).IsName ("Brach|WalkGrowl") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|WalkGrowl")
		    )
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
			
			if (velocity < 0.11F)
			{
				velocity = velocity + (Time.deltaTime * 0.1F); //acceleration
			}
			
			else if (velocity > 0.11F) //deceleration
			{
				velocity = velocity - (Time.deltaTime * 0.5F);
			}
			

			if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|StandA"))
			{
				velocity = velocity - (Time.deltaTime * 0.25F);
			}

			this.transform.Translate (0, 0, velocity*Scale);
		}

		//Backward
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Walk-") ||
		   		 anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Walk-") ||
		   		 anim.GetNextAnimatorStateInfo (0).IsName ("Brach|WalkGrowl-") ||
		   		 anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|WalkGrowl-")
		    )
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
			
			if (velocity > -0.11F)
			{
				velocity = velocity - (Time.deltaTime * 0.1F); //acceleration
			}
			
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|StandA"))
			{
				velocity = velocity + (Time.deltaTime * 0.25F);
			}
			
			this.transform.Translate (0, 0, velocity*Scale);
		}

		//Strafe+
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Strafe+") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Strafe+"))
		{

			if (Input.GetKey (KeyCode.Mouse1)) //turning
			{
				this.transform.localRotation *= Quaternion.AngleAxis (turn /16, new Vector3 (0, 1, 0));
				if (turn < 0) balance += 0.2F;
				else balance -= 0.2F;
			}

			if (velocity > -0.05F)
			{
				velocity = velocity - (Time.deltaTime * 0.1F); //acceleration
			}
			
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|StandA"))
			{
				velocity = velocity + (Time.deltaTime * 0.25F);
			}
			
			this.transform.Translate (velocity*Scale, 0, 0);
		}


		//Strafe-
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Strafe-") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Strafe-"))
		{

			if (Input.GetKey (KeyCode.Mouse1)) //turning
			{
				this.transform.localRotation *= Quaternion.AngleAxis (turn /16, new Vector3 (0, 1, 0));
				if (turn < 0) balance += 0.2F;
				else balance -= 0.2F;
			}

			if (velocity > -0.05F)
			{
				velocity = velocity - (Time.deltaTime * 0.1F); //acceleration
			}
			
			if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|StandA"))
			{
				velocity = velocity + (Time.deltaTime * 0.25F);
			}
			
			this.transform.Translate (-velocity*Scale, 0, 0);
		}


		//Running
		else if (anim.GetNextAnimatorStateInfo (0).IsName ("Brach|Run") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|Run") ||
		         anim.GetNextAnimatorStateInfo (0).IsName ("Brach|RunGrowl") ||
		         anim.GetCurrentAnimatorStateInfo (0).IsName ("Brach|RunGrowl")
		    )
		{
			if (Input.GetKey (KeyCode.A))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (0.33F, new Vector3 (0, -1, 0));
				balance += 0.2F;
			}
			else if (Input.GetKey (KeyCode.D))
			{
				this.transform.localRotation *= Quaternion.AngleAxis (0.33F, new Vector3 (0, 1, 0));
				balance -= 0.2F;
			}
			
			if (velocity < 0.22F)
			{
				velocity = velocity + (Time.deltaTime * 0.25F); //acceleration
			}

			this.transform.Translate (0, 0, velocity*Scale);
		}

	}
}