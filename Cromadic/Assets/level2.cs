using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level2 : MonoBehaviour {
    public GameObject lake1_cover;
	public GameObject lake1_cover_smoke;
    public GameObject lake2_cover;
    public GameObject WaterGate;
    public GameObject wellLid;
    public GameObject wellflow;
    public GameObject wellwater;
    public GameObject lake1;
    public GameObject sail;
    public GameObject button;
    public GameObject lake2;
    public GameObject waterflowposition;
    public GameObject waterflowposition2;
    public GameObject firebase2;
    public GameObject waterflow;
    public GameObject waterfall;
    private pickItems p;
    private bool raise_firebase2;
    private DoorTorchLight L;
    private DoorTorchLight L2;
    private WaterProperties well;
    private WaterProperties w1;
    private WaterProperties w2;
    private ElementControl e;
    private float waterLevel;
    
    public bool cover_burned;
    public bool lake1filled;
    public bool lake2filled;
    private bool playRaiseSound;
    private bool playDropSound;
    public GameObject firebase2RaiseSound;
    public GameObject firebase2DropSound;

    public AudioSource burnCoverSound;

    // Use this for initialization
    void Start () {
		p = transform.gameObject.GetComponent<pickItems>();
        e = transform.gameObject.GetComponent<ElementControl>();
        well = wellwater.GetComponent<WaterProperties>();
        w1 = lake1.GetComponent<WaterProperties>();
        w2 = lake2.GetComponent<WaterProperties>();
        L = GameObject.FindGameObjectWithTag("fire base1").transform.gameObject.GetComponent<DoorTorchLight>();
        L2 = firebase2.GetComponent<DoorTorchLight>();
	}
	
    
	// Update is called once per frame
	void Update () {
        e.waterStored = 2f;
        waterLevel = well.GetActualHeight();
        if (playRaiseSound)
        {
            firebase2RaiseSound.SetActive(true);
        }
        if (playRaiseSound == false)
        {
            firebase2RaiseSound.SetActive(false);
        }
        if (playDropSound)
        {
            firebase2DropSound.SetActive(true);
        }
        if (playDropSound == false)
        {
            firebase2DropSound.SetActive(false);
        }
        if (firebase2.transform.position.y< waterLevel){
            L2.waterputsFireOut();
        }
        if(raise_firebase2){
            if(firebase2.transform.localPosition.y< 12.0f){
                    
                    firebase2.transform.localPosition += new Vector3(0,-17f*Time.deltaTime*(-2.0f),0);
                }
                if(button.transform.position.y>11.64f){
                //need to wait
                button.transform.position -= new Vector3(0,12.55f*Time.deltaTime*(0.5f),0);
                firebase2DropSound.SetActive(true);
                }
        }
        else{
            if(button.transform.position.y<12.51f){
                button.transform.position += new Vector3(0,11.64f*Time.deltaTime*(0.5f),0);
                }
                if(firebase2.transform.localPosition.y> -0.1f){
                    firebase2.transform.localPosition += new Vector3(0,-5*Time.deltaTime*(2.0f),0);
                }
        }
        if(w1.currentVolume == 1){
            L.waterputsFireOut();
        }
        
        //lake1
        
        // if watergate opened and lake1 filled: lake1's water will fall down to lake2
		if(WaterGate.transform.localPosition.y<=23.5f && w1.currentVolume >0.0f){
            lake2_cover.SetActive(false); 
            lake2.layer =  4 ;
            w2.Fill(2);
            w1.Drain(2);
             Instantiate(waterfall,waterflowposition2.transform.position,waterflowposition2.transform.rotation);
        }
        
        //lake 2 
        
            
            //if open Lid losing water . waterflow press button:firebase 2 raise
            if((p.gotLid || wellLid.gameObject.GetComponent<Renderer>().enabled == true)  && w2.currentVolume>0.0f){
                
                
                w2.drainSpeed = 0.0005f; //change lake2 drain speed here
                w2.Drain(0.1f);
                Instantiate(waterflow,waterflowposition.transform.position,waterflowposition.transform.rotation);
                raise_firebase2=true;
                playRaiseSound = true;
                playDropSound = false;


        }
            else
            {
                raise_firebase2=false;
                playRaiseSound = false;
                //playDropSound = true;


            }
        
        //well 3
        //sail.transform.localPosition =new Vector3(sail.transform.localPosition.x, 1.41f*well.currentVolume*6.5f,sail.transform.localPosition.z);
            //close LID:water raise 
            if(wellLid.gameObject.GetComponent<Renderer>().enabled == true){
                
                
            }
            //open LID:  water can not above the hole 
            if(well.currentVolume>0.0f && wellLid.gameObject.GetComponent<Renderer>().enabled == false){
                
                Instantiate(wellflow,wellLid.transform.position,wellLid.transform.rotation);
                if(well.currentVolume <0.08f){
                well.currentVolume += 0.0001f*Time.deltaTime;}
                else{
                    well.currentVolume=0.08f;
                }
                
            }
            
            
        
	}
    
    public void OnCollisionStay(Collision Object) {
        
        
        if (Object.gameObject == lake1_cover  && p.gotFireInHand)
            
        {
            if(Input.GetKeyDown(KeyCode.F)){
            lake1_cover.SetActive(false);
            burnCoverSound.Play();
			lake1_cover_smoke.GetComponent<ParticleSystem>().Play();
            cover_burned =true;}
            
        }
         if (Object.gameObject.name == "button"){
             raise_firebase2=true;
             playRaiseSound = true;
             playDropSound = false;
        }



    }
    public void OnCollisionEnter(Collision Object) {
        
    
        


    }
    public void OnCollisionExit(Collision Object) {
         if (Object.gameObject.name == "button"){
             raise_firebase2=false;
             playRaiseSound = false;
             //playDropSound = true;
        }



    }
}
