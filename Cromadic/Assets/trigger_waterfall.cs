using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_waterfall : MonoBehaviour {
    
    
    public GameObject lake1;
    public GameObject lake2_cover;
    public GameObject waterfall;//prefab
    public GameObject waterflowposition2;
    private WaterProperties w1;
    
    //constrain for water gate 2
    public GameObject watergate2;
    public GameObject Boat2;
    public GameObject lake2;
    private bool canPlayWaterfallSound = false;
    public GameObject waterFallSound;
    //public GameObject destroyCoverSound;
	
	private float waterfallProgress = 0.0f;
	private bool waterFade = false;
	private float waterFadeProgress = 1.0f;
    private Vector3 begin;
    private Vector3 target;
    private float timer;
   
    void Start () {
		w1 = lake1.GetComponent<WaterProperties>();
        begin = this.transform.localPosition ;
        target = new Vector3(-170f,72.37f,-113.8f);
	}
	
	// Update is called once per frame
	void Update () {
        //open water gate automatically
        if(this.transform.localPosition.y > 72.30476f){
            timer+=0.01f+0.005f*Time.deltaTime;
            this.transform.localPosition = Vector3.Lerp(begin, target, timer);
        }
        
        // constrain lake2
        if(watergate2.transform.localPosition.y > -2.2){
            lake2.GetComponent<WaterProperties>().currentVolume=0;
        }
        
        else{
            
            Boat2.GetComponent<BoatFloatOnWater>().wp = lake2.GetComponent<WaterProperties>();
            //Boat2.GetComponent<BoatFloatOnWater>().offset=4f;
        }
        
        //waterfall sha
		if(this.transform.localPosition.x<-159f && w1.currentVolume >0.0f){
			waterfallProgress = Mathf.Clamp(waterfallProgress + 0.007f, 0, 1);
			float waterfallX = Mathf.Lerp(-32, -37, waterfallProgress);
			float waterfallScale = Mathf.Lerp(0,500, waterfallProgress);
			waterfall.transform.position = new Vector3(waterfallX, waterfall.transform.position.y, waterfall.transform.position.z);
			waterfall.transform.localScale = new Vector3(waterfallScale, waterfall.transform.localScale.y, waterfall.transform.localScale.z);
            lake2_cover.SetActive(false);
            //destroyCoverSound.SetActive(true);
            waterFallSound.SetActive(true);
            //w2.Fill(2);
            w1.Drain(2);
            //Instantiate(waterfall,waterflowposition2.transform.position,waterflowposition2.transform.rotation);
            Invoke("InsWater",1.5f);
			Invoke("WaterFade", 4.0f);
            //waterFallSound.SetActive(false);
        }
		
		if(waterFade){
			waterFadeProgress = Mathf.Clamp(waterFadeProgress - 0.01f, 0, 1);
			float waterfallScale = Mathf.Lerp(0,500, waterFadeProgress);
			waterfall.transform.localScale = new Vector3(waterfallScale, waterfall.transform.localScale.y, waterfall.transform.localScale.z);
		}
    }
    private void InsWater(){
        canPlayWaterfallSound = true;
    }
	
	private void WaterFade(){
		waterFade = true;
	}
  
}
