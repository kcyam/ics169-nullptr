using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	//public GameObject sail;
	public bool onPlatform;
	public GameObject player;

	//void Update(){
	//	transform.parent.position = transform.position - transform.localPosition;
	//}

	//*
	void Update(){
		if (player == null) {
			return;
		}

		if (onPlatform && player.transform.parent == null) {
			player.transform.parent = transform.parent;
		}

		if (!onPlatform && player.transform.parent != null) {
			player.transform.parent = null;
			player = null;
		}
	}
	//*/

	void OnCollisionEnter(Collision other){
		//Debug.Log (other.gameObject.tag);
		if(other.gameObject.CompareTag("Player")){
			//Debug.Log ("player landed on platform");
			//other.gameObject.transform.parent = transform.parent.transform;

			player = other.gameObject;
			onPlatform = true;
		}
	}
	void OnCollisionExit(Collision other){
		if(other.gameObject.CompareTag("Player")){
			//Debug.Log ("player landed on platform");
			//other.gameObject.transform.parent = null;

			onPlatform = false;
		}
	}
}
