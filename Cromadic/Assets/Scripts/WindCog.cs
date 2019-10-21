using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCog : MonoBehaviour {

	public bool translateObject;  //t = translate, f = rotate
	public bool remoteRotation;  //if rotate true -> t = remote rotation, f = attatched to bridge
	public char cogRotationAxis;
	public bool negativeRotation; //reverse rotation velocity for opposite movement for movable object
	public GameObject movableObject;
	public float cogRotation; //debugging
	private float rotationVelocity; //private
	//public float cogRotationQuat; //for debugging purposes
	private bool lockRotation;
	private Rigidbody rb;
	//public Vector3 angleVelocity;
	public AudioSource audio;

	[Header(" ------ rotation (connected) -----")]
	public bool setMinMax;
	public float minRotation;
	public float maxRotation;
	public float rotationOffset; //will lock in place once within a certain range of the offset

	[Header(" ------ rotation (remote) -----")]
	public char objectRotationAxis; //X, Y, Z
	public float rotateSpeed; //rotation speed of the object

	[Header(" ------ translation -----")]
	public float moveSpeed; //the speed at which the platform will move
	private Vector3 startPoint;
	public Vector3 endPoint;
	public float rotationSpeedDeadZone; //buffer so very small rotations do not cause platform to move
	//private bool horizontalOrientation; //check if cog is laid down horizontally
	private AudioSource movableObjectAudio;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		lockRotation = false;

		if (movableObject != null) {
			startPoint = movableObject.transform.position;

			/*
			if (transform.parent.rotation.eulerAngles.z == 90f) {
				horizontalOrientation = true;
			}
			*/

			movableObjectAudio = movableObject.GetComponent<AudioSource> ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (lockRotation) {
			return;
		}
		cogRotation = gameObject.transform.rotation.eulerAngles.x;

		//cogRotationQuat = gameObject.transform.rotation.x;
		if (!translateObject) { //rotate object
			if (setMinMax) {
				if (cogRotation < minRotation) {
					//cogRotation = minRotation;
					//Debug.Log ("Below min rotation");

					gameObject.transform.eulerAngles = new Vector3 (
						minRotation + 1f,
						gameObject.transform.eulerAngles.y,
						gameObject.transform.eulerAngles.z
					);
					rb.angularVelocity = Vector3.zero;
				} else if (cogRotation > maxRotation - rotationOffset) {
					//cogRotation = maxRotation;
					//Debug.Log ("Above max rotation");

					gameObject.transform.eulerAngles = new Vector3 (
						maxRotation,
						gameObject.transform.eulerAngles.y,
						gameObject.transform.eulerAngles.z
					);
					lockRotation = true;

					rb.constraints = 
						RigidbodyConstraints.FreezePositionX |
						RigidbodyConstraints.FreezePositionY |
						RigidbodyConstraints.FreezePositionZ |
						RigidbodyConstraints.FreezeRotationX |
						RigidbodyConstraints.FreezeRotationY |
						RigidbodyConstraints.FreezeRotationZ;
				}

				if (cogRotationAxis == 'X' || cogRotationAxis == 'x') {
					rotationVelocity = rb.angularVelocity.x;
				}
				else if (cogRotationAxis == 'Y' || cogRotationAxis == 'y') {
					rotationVelocity = rb.angularVelocity.y;
				}
				else { //if (cogRotationAxis == 'Z' || cogRotationAxis == 'z') {
					rotationVelocity = rb.angularVelocity.z;
				}

				if (audio != null) {
					if (Mathf.Abs (rotationVelocity) > 0.1) {
						if (!audio.isPlaying) {
							audio.Play ();
						}
					} 
					else if (audio.isPlaying) {
						audio.Stop ();
					}
				}
			}
			//angleVelocity = rb.angularVelocity;
			//rb.angularVelocity = new Vector3(angleVelocity.x,angleVelocity.y,angleVelocity.z);
			//rotationVelocity = rb.angularVelocity.x;

			if (remoteRotation) {
				/*
				if (!horizontalOrientation) {
					rotationVelocity = rb.angularVelocity.z;
				} 
				else {
					rotationVelocity = rb.angularVelocity.y;
				}
				*/
				if (cogRotationAxis == 'X' || cogRotationAxis == 'x') {
					rotationVelocity = rb.angularVelocity.x;
				}
				else if (cogRotationAxis == 'Y' || cogRotationAxis == 'y') {
					rotationVelocity = rb.angularVelocity.y;
				}
				else { //if (cogRotationAxis == 'Z' || cogRotationAxis == 'z') {
					rotationVelocity = rb.angularVelocity.z;
				}

				if (negativeRotation)
					rotationVelocity *= -1;

				if (objectRotationAxis == 'X' || objectRotationAxis == 'x') {
					movableObject.transform.Rotate (rotationVelocity * rotateSpeed * Vector3.right * Time.deltaTime);
				}
				else if (objectRotationAxis == 'Y' || objectRotationAxis == 'y') {
					movableObject.transform.Rotate (rotationVelocity * rotateSpeed * Vector3.up * Time.deltaTime);
				}
				else { //if (objectRotationAxis == 'Z' || objectRotationAxis == 'z') {
					movableObject.transform.Rotate (rotationVelocity * rotateSpeed * Vector3.forward * Time.deltaTime);
				}
			}
		} 
		else { //translate object
			/*
			if (!horizontalOrientation) {
				rotationVelocity = rb.angularVelocity.z;
			} 
			else {
				rotationVelocity = rb.angularVelocity.y;
			}
			*/

			if (cogRotationAxis == 'X' || cogRotationAxis == 'x') {
				rotationVelocity = rb.angularVelocity.x;
			}
			else if (cogRotationAxis == 'Y' || cogRotationAxis == 'y') {
				rotationVelocity = rb.angularVelocity.y;
			}
			else { //if (cogRotationAxis == 'Z' || cogRotationAxis == 'z') {
				rotationVelocity = rb.angularVelocity.z;
			}

			if (negativeRotation)
				rotationVelocity *= -1;

			if (rotationVelocity > rotationSpeedDeadZone) {
				TranslateTo (endPoint);
			} 
			else if (rotationVelocity < -rotationSpeedDeadZone) {
				TranslateTo (startPoint);
			} 
			else if (movableObjectAudio != null) {
				movableObjectAudio.Stop ();
			}
		}

		if (audio != null) {
			if (Mathf.Abs (rotationVelocity) > 0.1) {
				if (!audio.isPlaying) {
					audio.Play ();
				}
			} 
			else if (audio.isPlaying) {
				audio.Stop ();
			}
		}
	}

	private void TranslateTo(Vector3 stoppingPoint){
		float step = moveSpeed * Time.deltaTime;
		movableObject.transform.position = Vector3.MoveTowards (movableObject.transform.position, stoppingPoint, step);

		if (movableObjectAudio != null) {
			if (!movableObjectAudio.isPlaying) {
				movableObjectAudio.Play ();
			}
			if (movableObject.transform.position.Equals (stoppingPoint)) {
				movableObjectAudio.Stop ();
			}
		}
	}
}
