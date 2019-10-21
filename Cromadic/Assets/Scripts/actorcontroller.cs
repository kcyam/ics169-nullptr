using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actorcontroller : MonoBehaviour {
    public GameObject model;
	public playerinput pi;
	public float walkSpeed = 1.4f;
    public float runMultiplier = 1.8f;
    public float jumpVelocity = 5.0f;
	public float doubleJumpVelocity = 8.0f;
	public float glideMoveSpeed;
	public float glideDropSpeed;
    public bool isgliding;
    public AudioClip jump1;
    public AudioClip jump2;
    public AudioClip landSound;
    public GameObject glidingSound;

    [SerializeField]
    private Animator anim;
	private Rigidbody rigid;
	private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool onground =true;
    private bool lockPlanar= false;
	private Vector3 windDirection;
	private bool windBlowing = false;
	private float windBoost = 10f;		//How much the wind should push the player while gliding
	private float currentWindBoost = 0.0f;  //for smoothing
	private float windBoostAcceleration = 0.075f; //how fast the player should reach maximum wind boost speed
    //remember to change boat direction
    private bool canPlayGlideSound = false;


    void Start () {
		pi=GetComponent<playerinput> (); //get the script of playerinput
		anim=model.GetComponent<Animator> ();
		rigid= GetComponent<Rigidbody> ();
		windDirection = Vector3.zero;
        
        

    }
	
	// Update is called once per frame
	void Update () {
        
		float targetRunMulti=((pi.run)?2.0f:1.0f);
		anim.SetFloat("forward",pi.Dmag*Mathf.Lerp(anim.GetFloat("forward"),targetRunMulti,0.5f));
        
		/*   //bug with onground checker
		if(pi.jumpcount < 2 && onground == true){
			Debug.Log ("reset jumpcount");
			pi.jumpcount = 2;
		}


		if(pi.jump){
			//Debug.Log ("jump hit");

			if(pi.jumpcount == 2){
				anim.SetTrigger("jump");
			}
			else if(pi.jumpcount == 1){
				anim.SetTrigger("doublejump");
			}
			pi.jumpcount --;

			//Debug.Log ("decrease jump count");

		}
		*/

		if(pi.jump){

			//if(onground){  //bug with ongroundsensor.cs
			if(pi.jumpcount == 2){
				anim.SetTrigger("jump");

                AudioSource.PlayClipAtPoint(jump1, transform.position);

                pi.jumpcount = 1;  //2 - 1 = 1
			}
			else if(pi.jumpcount == 1){
				anim.SetTrigger("doublejump");
                AudioSource.PlayClipAtPoint(jump2, transform.position);
                pi.jumpcount = 0; //1 - 1 = 0
			}

		}
        
        
      
		if(pi.Dmag>0.1f){
            model.transform.forward= Vector3.Slerp(model.transform.forward,pi.Dvec,0.3f);
		}//模型forward向量
        
        if(lockPlanar == false){
            planarVec=pi.Dmag*model.transform.forward*walkSpeed* ((pi.run)? runMultiplier:1.0f);
        }

        if (canPlayGlideSound)
        {
            glidingSound.SetActive(true);
        }
        else
        {
            glidingSound.SetActive(false);
        }
	}
    
	
	void FixedUpdate(){
	  //rigid.position +=planarVec *Time.fixedDeltaTime;
      rigid.velocity=new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
      
      thrustVec = Vector3.zero;

		//gliding
		if (pi.glide && pi.jumpcount <= 1 && rigid.velocity.y < 0f) //(holding spacebar && after double jump && falling)
		{ 
            isgliding = true;
            canPlayGlideSound = true;

            anim.SetBool("gliding",true);
			planarVec = pi.Dmag*model.transform.forward*glideMoveSpeed* ((pi.run)? runMultiplier:1.0f); //(temp) lockPlaner = false
			if(windBlowing)
			{
				if(currentWindBoost < windBoost)
				{
					currentWindBoost = Mathf.Min(currentWindBoost + windBoostAcceleration, windBoost);
				}
				planarVec += currentWindBoost * windDirection;
			}
			rigid.velocity = new Vector3(planarVec.x, glideDropSpeed, planarVec.z);
		}
		else
		{ 
            isgliding = false;
            canPlayGlideSound = false;
            anim.SetBool("gliding",false);
			currentWindBoost = 0.0f;
		}
			
	}
    //
    //Message processing block
    //
    //
    public void Doublejump(){
        pi.inputEnabled = false;
        //lockPlanar = true;

		rigid.velocity=new Vector3(planarVec.x, 0f, planarVec.z); //reset y velocity when initiating double jump

        thrustVec = new Vector3(0,doubleJumpVelocity,0);
        //print("double");
    }
    public void OnJumpEnter(){

        pi.inputEnabled = false;
        //lockPlanar = true;
		thrustVec = new Vector3(0,jumpVelocity,0);
        
        
    }
    
 //   public void OnJumpExit(){
 //       pi.inputEnabled = true;
 //       lockPlanar = false;
  //      print("down");
  //  }
    

    public void IsGround(){
        //print("on ground");
		if(pi.jumpcount == 0){
			pi.jumpcount = 2;
		}
        onground = true;
        anim.SetBool("isGround",true);
        
    }
    public void IsNotGround(){
        onground = false;
        //print("on air");
        anim.SetBool("isGround", false);

		//* //for ground->falling animation. allows player to double jump while falling in this scenario
		if (pi.jumpcount == 2) {
			pi.jumpcount = 1;
		}
		//*/
        
    }
    
    public void OnGroundEnter(){
        AudioSource.PlayClipAtPoint(landSound, transform.position);
        pi.inputEnabled = true;
        lockPlanar = false;
		pi.jumpcount = 2;
    }

    public void updateWind(Vector3 newDir, bool blowing)
	{
		windDirection = newDir;
		windBlowing = blowing;
		
		if(blowing)
		{
			currentWindBoost = 0;
		}
	}

    }
