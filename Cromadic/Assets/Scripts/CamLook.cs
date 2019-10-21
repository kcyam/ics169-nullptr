using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLook : MonoBehaviour {

	public float sensitivity = 5f;
	
	private GameObject cameraHandle;
	private GameObject cameraHandle2;
	private Vector3 destDirection;
	private bool moving = false;
	private float smoothTurn = 0f;
	public GameObject desiredPosition;
	public GameObject player;
	public float viewDistance = 2.5f;
	Vector2 ml;
    public GameObject middleposition;
    
	public bool isPaused = false; //used when game is paused
	
    public GameObject DoorPosition;//used when door open, cut scene to it
    public bool canmove = true;
    public bool reached;
    public bool camera_rolling = false;
    public bool middlelerp;
    public bool finish_middlelerp;
	// Use this for initialization
	void Start () {
        canmove = true;
		ml = new Vector2(transform.rotation.eulerAngles.y, 0);
		cameraHandle = transform.parent.gameObject;
		cameraHandle2 = new GameObject("CameraHandle2");
		cameraHandle2.transform.parent = cameraHandle.transform;
		cameraHandle2.transform.localPosition = Vector3.zero;
		cameraHandle2.transform.SetParent(player.transform);
		cameraHandle2.transform.forward = player.transform.forward;
		cameraHandle.transform.SetParent(cameraHandle2.transform);
		desiredPosition.transform.localPosition = new Vector3( 0, 0, -viewDistance );
		destDirection = cameraHandle2.transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPaused) 
		{
			return;
		}
        if(true) //this is a bug hiding cuz lerping is weird right now
		{
			reached = false;
			Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
			md *= sensitivity;
			
			ml += md;
			ml.y = Mathf.Clamp(ml.y, -85f, 85f);
			
			cameraHandle.transform.localRotation = Quaternion.AngleAxis(-ml.y, Vector3.right);
			Quaternion horView = Quaternion.AngleAxis(ml.x, Vector3.up);
			cameraHandle2.transform.rotation = horView;
			//player.transform.localRotation = Quaternion.AngleAxis(ml.x, player.transform.up);
			//if( player.GetComponent<playerinput>().Dmag > 0.95f )
			//{
			//	player.transform.localRotation = Quaternion.AngleAxis(ml.x, Vector3.up);
			//	cameraHandle2.transform.localRotation = Quaternion.EulerAngles( Vector3.zero );
			//}
			
			if( player.GetComponent<playerinput>().Dmag > 0.1f )
			{
				if(!moving)
				{
					smoothTurn = 0;
					moving = true;
				}
				//player.transform.localRotation = Quaternion.AngleAxis(ml.x, Vector3.up);
				destDirection = cameraHandle2.transform.forward;
			}
			else
			{
				moving = false;
			}
			
			smoothTurn += 0.075f;
			smoothTurn = Mathf.Clamp(smoothTurn, 0, 1);
			player.transform.forward = Vector3.Slerp(player.transform.forward, destDirection ,smoothTurn);
			cameraHandle2.transform.rotation = horView;
			
			//if(player.transform.forward != cameraHandle2.transform.forward)
				//Debug.Log( "PLAYER:" + player.transform.forward.x + "," + player.transform.forward.y + "," + player.transform.forward.z + "\nCAMERA" +cameraHandle2.transform.forward.x + "," + cameraHandle2.transform.forward.y + "," + cameraHandle2.transform.forward.z);
			
			RaycastHit hit;
			Vector3 desiredDir = desiredPosition.transform.position - cameraHandle.transform.position;
			if( Physics.Raycast(cameraHandle.transform.position, desiredDir, out hit, desiredDir.magnitude) ){
				transform.position = hit.point;
			}
			else{
				transform.position = desiredPosition.transform.position;
			}
			
			transform.LookAt(cameraHandle.transform);
        }
       
        if(false) //this too
		{
			MoveToPostion();
        }
	}
    
   
    void MoveToPostion(){
        float t=0f;
        t+=Time.deltaTime;
    
        if(!reached){
            if(t >=3f){
                reached = true;
                canmove = true;
                //transform.position = desiredPosition.transform.position;
                camera_rolling=false;
            }
            if(Vector3.Distance(transform.position,DoorPosition.transform.position)<=3f){
                Debug.Log("reached");
                reached = true;
                canmove = true;
                camera_rolling=false;
                finish_middlelerp=false;
            }
        }
        if(!reached){  
            if(middlelerp){
                lerpCam(middleposition);
                
            }
            
            lerpCam(DoorPosition);
        }
        
    }
    private void lerpCam(GameObject DoorPosition){
        
        float xLerp = Mathf.LerpAngle(transform.rotation.x, DoorPosition.transform.rotation.x, 1f*Time.deltaTime);
         float yLerp = Mathf.LerpAngle(transform.rotation.y, DoorPosition.transform.rotation.y, 1f*Time.deltaTime);
         float zLerp = Mathf.LerpAngle(transform.rotation.z, DoorPosition.transform.rotation.z, 1f*Time.deltaTime);
         
        transform.rotation = Quaternion.Euler(xLerp, yLerp, zLerp);
        float xLerp2 = Mathf.Lerp(transform.position.x, DoorPosition.transform.position.x, 1f*Time.deltaTime);
         float yLerp2 = Mathf.Lerp(transform.position.y, DoorPosition.transform.position.y, 1f*Time.deltaTime);
         float zLerp2 = Mathf.Lerp(transform.position.z, DoorPosition.transform.position.z, 1f*Time.deltaTime);
        transform.position=new Vector3(xLerp2, yLerp2, zLerp2);
        if(Vector3.Distance(transform.position,DoorPosition.transform.position)<=6f){
            finish_middlelerp=true;
        }
    }

}
