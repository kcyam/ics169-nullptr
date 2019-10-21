using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnLevel2 : MonoBehaviour
{
	public string respawnObjectTag;
	public Transform respawnPoint;

	void OnTriggerEnter(Collider other){
		if (other.CompareTag (respawnObjectTag)) {
			other.transform.position = new Vector3(respawnPoint.transform.position.x,respawnPoint.transform.position.y,respawnPoint.transform.position.z);
		}
	}
}
