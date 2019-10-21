using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFloatOnWater : MonoBehaviour {
    
    public bool changelimit;
    public bool boat_limit_both_x_and_z;
    public float offset = 0;
	public GameObject water;
	public WaterProperties wp;
	public float waterLevel;
    private ElementControl E;
    public float fillspeed;
    private bool change;
    private bool change2;
    private float radian = 0;
    private float deltaRadian =0.15f;
    private float radius = 0.05f;
    private ParentFollowChild P;
    //public Camera cam;
    private float currentFOV;
    public float minFOV;
    public float maxFOV;
    public float zoomRate;
    private int a;
    //public Transform headObject;
    float time=0f;
    private  Quaternion targetrotation;
    private  Quaternion targetrotation2;
    public GameObject rotate_boat_mesh;
    public GameObject sail;
	// Use this for initialization
	void Start () {
        targetrotation = Quaternion.Euler(-89.999f, 180f, 0f);
        targetrotation2 = Quaternion.Euler(-89.999f,180f, -100f);
        //cam = headObject.Find("Camera").GetComponent<Camera>();
		if (water != null) {
			wp = water.GetComponent<WaterProperties> ();
		}
        E = GameObject.FindGameObjectWithTag("Player").transform.gameObject.GetComponent<ElementControl>();
        P = transform.gameObject.GetComponent<ParentFollowChild>();
//        waterLevel = wp.GetActualHeight();
//        transform.position = new Vector3 (
//			transform.position.x,
//			waterLevel+21f, //wp.GetActualHeight (),
//			transform.position.z
//		);
	}
	IEnumerator floating_effect(){
        
        
      
        if((Time.time-time)<2f)
            
        {
        radian += deltaRadian;
            float dy = Mathf.Cos(radian) * radius;
            transform.position = this.transform.position + new Vector3(0, dy, 0);
        
        }
        yield return null;
            
    }
	// Update is called once per frame
	void Update () {
        
        
        
        
        
//        if(P.limitX){
//            if(this.transform.position.x > P.xMax){
//                this.transform.Translate(-0.01f,0f,0f);
//            }
//            else if(this.transform.position.x < P.xMin){
//                this.transform.Translate(0.01f,0f,0f);
//            }
//        }
        if(P.limitZ){
            if(this.transform.position.z >= P.zMax){
                Debug.Log("limit");
                this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z-1f);
            }
            else if(this.transform.position.z <= P.zMin){
                this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z+1f);
            }
        }
       // zoomCamera();
		if (water == null) {
			return;
		}
        waterLevel = wp.GetActualHeight();
        
        if(boat_limit_both_x_and_z){
            
            // float on right level
//            if(transform.position.y > 13f){
//                transform.position = new Vector3 (
//                    transform.position.x,
//                    waterLevel, //wp.GetActualHeight (),
//                    transform.position.z);
//            }
            if(transform.position.y > 0f){
                    
                    transform.position = new Vector3 (
                    transform.position.x,
                    waterLevel*0.9f, //wp.GetActualHeight (),
                    transform.position.z);
            }
            if(transform.position.y < -0.2f){
                    transform.position = new Vector3 (
                    transform.position.x,
                    waterLevel+offset, //wp.GetActualHeight (),
                    transform.position.z);
            }
                
                
            if(a==0){
            if(transform.position.z < -60){
                P.xMin = -50f;
            }
            }
            if(transform.position.z > -60) {
                a+=1;P.xMin = -66.65f;
                rotate_boat_mesh.transform.localRotation = Quaternion.Slerp(rotate_boat_mesh.transform.rotation, targetrotation,  Time.deltaTime * 5f);
    }                      }
            
            if(transform.position.x < -60){
                P.xMax = -60;
                rotate_boat_mesh.transform.localRotation= Quaternion.Slerp(rotate_boat_mesh.transform.rotation, targetrotation2,  Time.deltaTime * 5f);
                
            }
            if(transform.position.x < -60 &&transform.position.z<-71.36f){

                P.zMin = -71.36f;
                P.zMax = -70.36f;
                
            }
            
        
        
        if (changelimit){
            transform.position = new Vector3 (
                transform.position.x,
                waterLevel+offset, //wp.GetActualHeight (),
                transform.position.z
            );
        
            if(waterLevel>=27.1f){
                transform.position = new Vector3 (
                transform.position.x,
                waterLevel+1.8f, //wp.GetActualHeight (),
                transform.position.z
            );
            }
		
            if(wp.currentVolume >= 0.5f){
                P.xMax = -17;
            }
        }
                if(0.0f<wp.currentVolume&&wp.currentVolume<0.99f){
                if(E.shootingWater){

                //transform.Translate(Vector3.up*waterLevel*fillspeed);
                change=E.shootingWater;
                time= Time.time;
                }
                if(E.shootingWater!=change){
                    StartCoroutine("floating_effect");
                }


                if(E.absorbingWater){

                  //  transform.Translate(Vector3.down*waterLevel*fillspeed);
                    change2=E.absorbingWater;
                    time= Time.time;
                }


                }
        
        
	}
    public void OnTriggerStay(Collider Object) {
        
        if(Object.tag=="Pushable"){
            sail.SetActive(false);
        }
    }
    
//    public void zoomCamera(){
//        currentFOV = cam.fieldOfView;
//        float scroll = Input.GetAxis("Mouse ScrollWheel");
//        currentFOV += scroll*zoomRate;
//        currentFOV = Mathf.Clamp(currentFOV, minFOV,maxFOV);
//        cam.fieldOfView = currentFOV;
//    }
}
