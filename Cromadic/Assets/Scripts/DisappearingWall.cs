using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingWall : MonoBehaviour
{
	public GameObject wall;

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Boulder")) {
			wall.SetActive (false);
		}
	}
	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Boulder")) {
			wall.SetActive (true);
		}
	}
}
