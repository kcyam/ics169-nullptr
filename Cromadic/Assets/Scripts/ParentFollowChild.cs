using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentFollowChild : MonoBehaviour {

	//Nova840

	[SerializeField]
	private Transform follow = null;
	//public Vector3 followPosition; //debugging

	private Vector3 originalLocalPosition;
	private Quaternion originalLocalRotation;

	//private Transform previousTransform;
	private Vector3 previousTransPos;
	private bool usePreviousTransform = false;

	public bool limitX;
	public float xMin;
	public float xMax;
	public bool limitY;
	public float yMin;
	public float yMax;
	public bool limitZ;
	public float zMin;
	public float zMax;
	private float currentPosition;
	private float previousPosition; //only used to detect playing audio

	public AudioSource hitWallAudio;
	private bool atEndpoint = false;
	private float audioOffset = 0.5f;
	public bool onWater = false; //set true if boat is on water. (will only affect the audio)
	public AudioSource waterAudio;
	public float minSpeedAudio; //minimum boat speed neccessary to activate audio
	private float speed; //debugging

	private void Awake() {
		originalLocalPosition = follow.localPosition;
		originalLocalRotation = follow.localRotation;
	}

	private void Update() {
		//followPosition = follow.position;
		//*
		if (usePreviousTransform) {
			//follow.position = previousTransform.position;
			//usePreviousTransform = false;

			transform.position = previousTransPos;
			follow.localPosition = originalLocalPosition;
			usePreviousTransform = false;
			return;
		}

		previousTransPos = transform.position;
		//*/

		if (limitX) {
			previousPosition = currentPosition;
			currentPosition = follow.position.x - originalLocalPosition.x;
			if(currentPosition < xMin || currentPosition > xMax){
				follow.localPosition = originalLocalPosition;

				if (hitWallAudio != null) {
					if (!hitWallAudio.isPlaying && !atEndpoint) {
						hitWallAudio.Play ();
					}
				}
				atEndpoint = true;
				return;
			}
		}

		if (limitY) {
			previousPosition = currentPosition;
			currentPosition = follow.position.y - originalLocalPosition.y;
			if(currentPosition < yMin || currentPosition > yMax){
				follow.localPosition = originalLocalPosition;
				return;
			}
		}

		if (limitZ) {
			previousPosition = currentPosition;
			currentPosition = follow.position.z - originalLocalPosition.z;
			if(currentPosition < zMin || currentPosition > zMax){
				follow.localPosition = originalLocalPosition;

				if (hitWallAudio != null) {
					if (!hitWallAudio.isPlaying && !atEndpoint) {
						hitWallAudio.Play ();
					}
				}
				atEndpoint = true;
				return;
			}
		}

		if (atEndpoint) {
			if (limitX) {
				//currentPosition = follow.position.x - originalLocalPosition.x;
				if (xMin+audioOffset < currentPosition && currentPosition < xMax-audioOffset) {
					Debug.Log ((xMin + audioOffset).ToString() + " < cp < " + (xMax - audioOffset).ToString());
					atEndpoint = false;
				}
			}
			if (limitZ) {
				//currentPosition = follow.position.z - originalLocalPosition.z;
				if (zMin + audioOffset < currentPosition && currentPosition < zMax - audioOffset) {
					atEndpoint = false;
				} else { //in case using both limit x and z
					atEndpoint = true;
				}
			}
		}

		//move the parent to child's position
		transform.position = follow.position;

		/*
		//"reverse" the quaternion so that the local rotation is 0 if it is equal to the original local rotation
		follow.RotateAround(follow.position, follow.forward, -originalLocalRotation.eulerAngles.z);
		follow.RotateAround(follow.position, follow.right, -originalLocalRotation.eulerAngles.x);
		follow.RotateAround(follow.position, follow.up, -originalLocalRotation.eulerAngles.y);

		//rotate the parent
		transform.rotation = follow.rotation;
		//*/

		//moves the parent by the child's original offset from the parent
		transform.position += -transform.right * originalLocalPosition.x;
		transform.position += -transform.up * originalLocalPosition.y;
		transform.position += -transform.forward * originalLocalPosition.z;

		//resets local rotation, undoing step 2
		//follow.localRotation = originalLocalPosition;

		//reset local position
		follow.localPosition = originalLocalPosition;

		if (onWater) {
			if (waterAudio != null) {
				speed = Vector3.Distance (previousTransPos, transform.position) / Time.deltaTime;
				if (speed > minSpeedAudio) {
					if (!waterAudio.isPlaying) {
						waterAudio.Play ();
						//Debug.Log ("play water audio");
					}
				} else if (waterAudio.isPlaying) {
					waterAudio.Stop ();
				}
			}
		}
	}

	/*
	void OnTriggerEnter(Collider other){
		Debug.Log ("Boat triggered: " + other.gameObject.name);
		//if (!other.CompareTag ("Player") && !other.CompareTag ("Boat") && !other.CompareTag ("Wind")) {
		if (!other.CompareTag ("Player") && !other.CompareTag ("Wind")) {
			usePreviousTransform = true;
			//Debug.Log (other.tag);
			Debug.Log("boat trigger");
		}
	}
	//*/

	/*
	void OnCollisionEnter(Collision other){
		usePreviousTransform = true;
		Debug.Log ("boat collision");
	}
	//*/
}
