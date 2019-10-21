using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneControl : MonoBehaviour {
    public logic l;
    public GameObject model;
    public GameObject boat;
    private Vector3 velocity = new Vector3(-0.5f,0.0f,0.0f);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if( l.wind == true){
            if (boat.transform.position.x > -23){
                boat.transform.position += velocity *Time.deltaTime;}
                model.transform.parent= boat.transform;
        }
        else{
            model.transform.parent=null;
            if(boat.transform.position.x < -10)
            boat.transform.position -= velocity *Time.deltaTime;}
        
        }
}
