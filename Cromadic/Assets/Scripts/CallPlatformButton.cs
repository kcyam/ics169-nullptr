using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallPlatformButton : MonoBehaviour {

	public GameObject movableObject;
	public Vector3 endPoint;
	public float moveSpeed;

	void OnTriggerStay(Collider other){
		if(other.CompareTag("Player")){
			float step = moveSpeed * Time.deltaTime;
			movableObject.transform.position = Vector3.MoveTowards (movableObject.transform.position, endPoint, step);
		}
	}
}
