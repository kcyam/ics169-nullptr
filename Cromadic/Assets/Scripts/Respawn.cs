using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
	public string respawnObjectTag;
	[Header(" Can use either transform or coords")]
	public Transform respawnPoint;
	public Vector3 respawnPos;
	private bool hasTransform = false;
	public float respawnDelay = 0f;
	public float timestep = 0.1f; //mostly affects smoothness of screen fade

	[Header(" ------ Screen fade -----")]
	public bool fadeout = false; //set true to have screen fade out when respawning
	public Image fadeScreen;
	private Color screenColor;
	public float fadeinTime = 0f; //time it takes to return back to the normal screen

	void Start(){
		if (respawnPoint != null) {
			hasTransform = true;
		}

		if (fadeout) {
			screenColor = fadeScreen.color;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag (respawnObjectTag)) {
			//other.transform.position = respawnPoint;
			//other.gameObject.SetActive(false);
			StartCoroutine("RespawnDelay", other);
		}
	}

	IEnumerator RespawnDelay(Collider other){
		/*
		yield return new WaitForSeconds(respawnDelay);
		other.transform.position = respawnPoint;
		//other.gameObject.SetActive(true);

		Rigidbody rb = other.gameObject.GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
		*/

		for (float f = timestep; f <= respawnDelay; f += timestep) {
			if (fadeout) {
				screenColor.a = f / respawnDelay;
				fadeScreen.color = screenColor;
				Debug.Log ("fadeout: " + f + " alpha: " + screenColor.a);
			}

			yield return new WaitForSeconds (timestep);
		}

		if (hasTransform) {
			respawnPos = respawnPoint.position;
		}
		other.transform.position = respawnPos;

		Rigidbody rb = other.gameObject.GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		for (float f = 0f; f <= fadeinTime; f += timestep) {
			if (fadeout) {
				screenColor.a = (1f - (f / fadeinTime));
				fadeScreen.color = screenColor;
				Debug.Log ("fadein: " + f + " alpha: " + screenColor.a);
			}

			yield return new WaitForSeconds (timestep);
		}
	}
}
