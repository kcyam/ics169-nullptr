using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickItems : MonoBehaviour {

    public GameObject TorchInHand;
    public GameObject TorchFireInHand;
    public GameObject torchToPick;
    public GameObject fireBaseFire;
    public GameObject fireBaseFire2;
    public GameObject LidToPick;
    public GameObject LidInHand;
    public GameObject door1;
    public GameObject door2;
    public bool gotTorch = false;
    public bool gotFireInHand = false;
    public bool canPickTorch1 = false;
    public bool canPickLid = false;
    public bool canPickFire = false;
    public bool canLightUp1 = false;
    public bool canLightUp2 = false;
    public bool gotLid = false;
    public bool check1door1 = false;
    public bool check2door1 = false;
    public bool door1opened = false;
    
   
    public string PickKey;
    // trigger once signal
    public bool pick;
    private bool newpick;
    private bool lastpick;

    public AudioSource pickItemSound;
    public AudioSource putItemSound;
    public AudioSource lightUpTorchSound;

	public GameObject textbox;
	private Text message;
	private string msgPickup = "(F) Pickup";
	private string msgFireSource = "(F) Light";
	private string msgFireBase = "(F) Light";
	private string msgPlaceLid = "(F) Place";
	private string msgGrabLid = "(F) Grab";
    
    private enum door
    {
        open,
        close
    };
    private door doorstate;  
    private bool torch;
    // Use this for initialization
    void Start () {
        doorstate = door.open;
		torch = SceneTransporter.hold_torch;
		if (torch == true){
			torchToPick.SetActive (false);
			TorchInHand.SetActive (true);
			gotTorch = true;
			canPickTorch1 = false;
			textbox.SetActive (false);
		}
		message = textbox.GetComponentInChildren<Text> ();
    }
	
	// Update is called once per frame
	void Update () {
        
//        newpick = Input.GetKey (PickKey);
//        //Debug.Log(newpick);
//        if (newpick != lastpick && newpick == true){
//            pick=true;
//            
//        }
//        else{
//            pick = false;
//        }
//        lastpick = newpick;
        /*
        
        
        switch ( doorstate )
            {
                //BLOW WIND
				case door.open:
                    door2.transform.Translate(Vector3.up * 10f * Time.deltaTime, Space.World);
                    if(door2.transform.position.y>13f){
                        door2.transform.position = new Vector3(0,13f,0); 
                    }
                
                    break;
                
                case door.close:
                    door2.transform.Translate(Vector3.down * 10f * Time.deltaTime, Space.World);
                    if(door2.transform.position.y<5f){
                        door2.transform.position= new Vector3(0,5f,0); 
                    }
                    
                    
                    break;
            }
        */

		if (Input.GetKeyDown (KeyCode.F)) {
			// pick torch
			//if (Input.GetKeyDown(KeyCode.F) && canPickTorch1 )
			if (canPickTorch1) {
				//Debug.Log ("picking torch");
				pickItemSound.Play ();
				torchToPick.SetActive (false);
				TorchInHand.SetActive (true);
				gotTorch = true;
				canPickTorch1 = false;

				textbox.SetActive (false);
			}
	        
	       
			// light up torch
			//if (Input.GetKeyDown(KeyCode.F) && canPickFire && gotTorch)
			if (canPickFire && gotTorch) {
				//Debug.Log ("picking fire");
				lightUpTorchSound.Play ();
				TorchFireInHand.SetActive (true);
				gotFireInHand = true;

				textbox.SetActive (false);
			}

			if (message.text == msgFireBase) {
				textbox.SetActive (false);
			}
		}
        /*
        if (Input.GetKeyDown(KeyCode.F) && canLightUp1 && gotFireInHand)
        {
            Debug.Log("lighting up fire base1");
            fireBaseFire.SetActive(true);
            check1door1 = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && canLightUp2 && gotFireInHand)
        {
            Debug.Log("lighting up fire base2");
            fireBaseFire2.SetActive(true);
            check2door1 = true;
        }
        
        check_opendoor(check1door1,check2door1);
        */
    }
    
    private void check_opendoor(bool left,bool right){
        if ((left == true) && (right == true)&&(door1opened==false))
        {
            //Debug.Log("door opening");
            door1.transform.Translate(Vector3.up * 10f * Time.deltaTime, Space.World);
            if (door1.transform.position.y > 13)
            {
                door1opened = true;
            }
         
            
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ontrigger Enter");
        if (other.tag == "toPick1")
        {
            canPickTorch1 = true;
            
			textbox.SetActive (true);
			message.text = msgPickup;
        }

        if (other.tag == "fire source")
        {
            canPickFire = true;

			if (gotTorch && !gotFireInHand) {
				textbox.SetActive (true);
				message.text = msgFireSource;
			}
        }

        if (other.tag == "fire base1")
        {
			canLightUp1 = true;
			
			if (gotFireInHand && !other.GetComponent<DoorTorchLight>().isLit) {
				textbox.SetActive (true);
				message.text = msgFireBase;
			}
        }

        if (other.tag == "fire base2")
        {
			canLightUp2 = true;
			
			if (gotFireInHand && !other.GetComponent<DoorTorchLight>().isLit) {
				textbox.SetActive (true);
				message.text = msgFireBase;
			}
        }
        
        
       
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("ontrigger Exit");
            
        if (other.tag == "toPick1")
        {
			canPickTorch1 = false;

			if (message.text == msgPickup) {
				textbox.SetActive (false);
			}
        }

        if (other.tag == "fire source")
        {
            canPickFire = false;

			if (message.text == msgFireSource) {
				textbox.SetActive (false);
			}
        }

        if (other.tag == "fire base1")
        {
			canLightUp1 = false;

			if (message.text == msgFireBase) {
				textbox.SetActive (false);
			}
        }

        if (other.tag == "fire base2")
        {
			canLightUp2 = false;

			if (message.text == msgFireBase) {
				textbox.SetActive (false);
			}
        }
        
		if (other.tag == "Lid")
		{
			if (message.text == msgGrabLid || message.text == msgPlaceLid) {
				textbox.SetActive (false);
			}
		}
    }
    
    
    // added for level2 torch machanic
     public void OnTriggerStay(Collider other)
    {
            
        if (other.tag == "Lid")
        {   
            //place lid back to its position
            if( Input.GetKeyDown(KeyCode.F) && gotLid){
                other.gameObject.GetComponent<Renderer>().enabled = true;
                putItemSound.Play();
                LidInHand.SetActive(false);
                LidToPick.SetActive(true);
                canPickLid = false;
                gotLid = false; 
                //doorstate=door.open;//open door
                
				textbox.SetActive (false);
            } 
            //grab lid
            else if (Input.GetKeyDown(KeyCode.F) && canPickLid==false){
                //other.gameObject.SetActive(false);
                pickItemSound.Play();
	            other.gameObject.GetComponent<Renderer>().enabled = false;

	            LidInHand.SetActive(true);
	            gotLid = true;
	            canPickLid =true;
	            //doorstate=door.close; //close door
            
				textbox.SetActive (false);
            }
            
			if (!textbox.activeSelf) {
				if (gotLid && !other.GetComponent<MeshRenderer> ().isVisible) {
					textbox.SetActive (true);
					message.text = msgPlaceLid;
				} 
				else if (!gotLid && other.GetComponent<MeshRenderer> ().isVisible) {
					textbox.SetActive (true);
					message.text = msgGrabLid;
				}
			}
        }
    }
}
