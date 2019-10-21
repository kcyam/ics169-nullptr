using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSounds : MonoBehaviour
{

	private AudioSource collisionAudio; //general sounds when boulder collides with any object

	//speed of impact required to trigger sound and volume of collision
	public float minSpeed = 1f; //0.04
	public float maxSpeed = 10f; //15
	private float speed;

    void Start()
    {
		collisionAudio = gameObject.GetComponent<AudioSource> ();
    }

	void OnCollisionEnter(Collision other){
		speed = other.relativeVelocity.magnitude;
		//Debug.Log ("boulder collision: " + other.gameObject.name + " " + speed);

		if (speed > minSpeed) {
			if (speed > maxSpeed) {
				collisionAudio.volume = 1f;
			} else {
				collisionAudio.volume = speed / maxSpeed;
			}
			//Debug.Log ("Play boulder audio");
			collisionAudio.Play ();
		} 
	}
}
