using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerinput : MonoBehaviour {
[Header(" ------ key settings -----")]
public string keyup="w";
public string keyDown="s";
public string keyLeft="a";
public string keyRight="d";

public string keyA;
public string keyB;
public string keyC;
public string keyD;

public string keyJRight;
public string keyJUp;
public string KeyJLeft;
public string KeyJDown;

[Header("----- Output signals -----")]
public float Dup;
public float Dright;
public float Dmag;
public Vector3 Dvec;
public float Jup;
public float Jright;

// pressing signal
public bool run;
//trigger once signal  == jump signal
public bool jump;
public bool newJump;
public bool lastJump;
public int jumpcount=0;
public bool glide;


//trigger type signal
//3. double trigger

[Header("-----others-----")]
public bool inputEnabled=true;
private float targetDup;
private float targetDright;
private float velocityDup;
private float velocityDright;
private CamLook c;







	void Start () {
        c = Camera.main.GetComponent<CamLook>();
		//LOCK CURSOR
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        //camera rotate
        Jup = (Input.GetKey(keyJUp)? 1.0f:0) - (Input.GetKey(KeyJDown)? 1.0f:0);
        Jright = (Input.GetKey(keyJRight)? 1.0f:0) - (Input.GetKey(KeyJLeft)? 1.0f:0);
        
        
        
        
		if(inputEnabled==false){
			targetDup=0;
			targetDright=0;
		}
        // walking
        if(c.canmove){
		targetDup=(Input.GetKey(keyup)? 1.0f:0) -(Input.GetKey(keyDown)? 1.0f:0);
		targetDright=(Input.GetKey(keyRight)? 1.0f:0) -(Input.GetKey(keyLeft)? 1.0f:0);
		
		Dup=Mathf.SmoothDamp(Dup,targetDup, ref velocityDup,0.1f);
		Dright=Mathf.SmoothDamp(Dright,targetDright,ref velocityDright,0.1f);
		
		Vector2 tempDAxis=SquareToCircle(new Vector2(Dright,Dup));
		float Dright2=tempDAxis.x;
		float Dup2=tempDAxis.y;

		Dmag=Mathf.Sqrt((Dup2*Dup2) +(Dright2*Dright2));
		Dvec=Dright2*transform.right+Dup2*transform.forward;
        }

		run = Input.GetKey (keyA);
        
        
        newJump = Input.GetKey(keyB);
        if (newJump != lastJump && newJump == true){
            jump=true;
            
        }
        else{
            jump = false;
        }
        lastJump = newJump;
		glide = newJump; //= Input.GetKey(keyB); //activate glide when jump button is held down (after double jump)

	}

	private Vector2 SquareToCircle(Vector2 input){
		Vector2 output=Vector2.zero;
//x''=x* sqrt(1-y^2/2)
		output.x=input.x* Mathf.Sqrt(1-(input.y*input.y)/2.0f);
		output.y=input.y* Mathf.Sqrt(1-(input.x*input.x)/2.0f);

		return output;
	}
    
    
}
