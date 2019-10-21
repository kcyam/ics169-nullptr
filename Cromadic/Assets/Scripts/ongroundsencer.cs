using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ongroundsencer : MonoBehaviour {
    public CapsuleCollider capcol;
    
    private Vector3 point1;
    private Vector3 point2;
    private float radius;
	// Use this for initialization
	void Awake () {
        radius = capcol.radius;
       
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		//point1 = transform.position +transform.up *radius;
        //point2 = transform.position + transform.up * capcol.height - transform.up*radius;
		point1 = capcol.bounds.center;
		point2 = new Vector3(capcol.bounds.center.x, capcol.bounds.min.y-0.1f, capcol.bounds.center.z);
        
		/*
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));

        if(outputCols.Length != 0){
        //    foreach(var col in outputCols){
        //        print("collision:" + col.name);
        //    }
            SendMessageUpwards("IsGround"); // send message to its parent : Playerhandler can have a function named same as the msg"IsGround"
        }
        else{
            SendMessageUpwards("IsNotGround");
        }
        //*/

		if(Physics.CheckCapsule(point1, point2, 0.1f, LayerMask.GetMask("Ground"))){ //if grounded
			SendMessageUpwards("IsGround"); // send message to its parent : Playerhandler can have a function named same as the msg"IsGround"
			//Debug.Log("ON GROUND");
		}
		else{
			SendMessageUpwards("IsNotGround");
			//Debug.Log("NOT ON GROUND");
		}
	}
}
