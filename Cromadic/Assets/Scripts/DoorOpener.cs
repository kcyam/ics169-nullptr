using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpener : MonoBehaviour {
    public GameObject doorOpenSound;
    private bool playDoorOpenSound = false;
    private int max_torches;
    private int num_torches;
    public GameObject[] torch_list;
    public CamLook c;
    public Animation anim;
    private bool open;
    public bool lerpmiddle;
    public GameObject cameracut;
    

    // Use this for initialization
    void Start ()
    {
        c = Camera.main.GetComponent<CamLook>();
        //anim = GetComponentInChildren<Animation>();

    }
	
	// Update is called once per frame
	void Update ()
    {   
        //if(playDoorOpenSound == true){
           // doorOpenSound.SetActive(true);
       // }
        
      //  if(playDoorOpenSound == false){
        //    doorOpenSound.SetActive(false);
       // }
        
        //print("UPDATE");
        if (CheckTorches())
        {
            //playDoorOpenSound =true;
            
            doorOpenSound.SetActive(true);
            //Remove below line and change it to animator.
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false); //index 0 should be door1
            // camera look at the door open
            if(lerpmiddle){
                c.middlelerp = true;
                c.middleposition=gameObject.transform.GetChild(9).gameObject;
            }
            c.DoorPosition = cameracut;
            if(c.reached){
            //Debug.Log("dooor"+c.camera_rolling);
            c.camera_rolling = false;
            this.enabled=false;
            return;}
            c.camera_rolling = true;
            //Debug.Log("dooor2"+c.camera_rolling);
            

        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
	}

    bool CheckTorches()
    {
        num_torches = 0;
        foreach (GameObject torch in torch_list)
        {
            //print(torch.gameObject.GetComponent<DoorTorchLight>().isLit);
            if (!torch.gameObject.GetComponent<DoorTorchLight>().isLit)
                break;
            num_torches++;
        }
        return num_torches == torch_list.Length;
    }

    //Instructions: (X means done)
    /*
     * X    Door has a list of torches 
     * X    Check if player holds a torch (in DoorTorchLight.cs)
     * X    Let torch be lit if player is holding a torch
     *      Spawn a lit block if torch is lit (just to show. may take this off after Kevin finishes torch)
     * X    If all torches are lit, open door
     *          Remove collider that blocks player
     */
}
