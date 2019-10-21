using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandle : MonoBehaviour {
    public playerinput pi;
    
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerx;
    private GameObject model;
    private GameObject camera;
    
    private Vector3 cameraDampVelociy;
	// Use this for initialization
	void Awake () {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerx = 20;
        model = playerHandle.GetComponent<actorcontroller> ().model;
        camera = Camera.main.gameObject;
        
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tempModelEuler = model.transform.eulerAngles;
        
        //camera horizontal rotate
        playerHandle.transform.Rotate(Vector3.up, pi.Jright *100.0f*Time.deltaTime);
        
        
        //cameraHandle.transform.Rotate(Vector3.right, pi.Jup * -80.0f*Time.deltaTime);
        //tempEulerx = cameraHandle.transform.eulerAngles.x;
        
        //limit vertical rotate angle
        tempEulerx -= pi.Jup* -80 * Time.deltaTime;
        tempEulerx = Mathf.Clamp(tempEulerx, -40,30);
        cameraHandle.transform.localEulerAngles = new Vector3(
            tempEulerx,0,0);
        
        model.transform.eulerAngles = tempModelEuler;
        
        //camera chase the character
        //camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position,0.2f);
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position,transform.position,ref cameraDampVelociy,0.05f);

        //camera.transform.eulerAngles = transform.eulerAngles;
        camera.transform.LookAt(cameraHandle.transform);
	}
}
