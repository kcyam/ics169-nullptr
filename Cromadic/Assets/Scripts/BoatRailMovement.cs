using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRailMovement : MonoBehaviour {

	//public GameObject boat;
	private bool diagMovement = true; //Set true to allow boat to move diagonally
	public Vector3 startPoint;  //point where automatic moving starts
	public Vector3 endPoint;    //point where automatic moving stops
	private Vector3 curPos; //current position
	private Vector3 prePos; //previous position
	private char axis;
	private int dir = 1; //positive or negative
	private float dist;  //change in distance
	public float acceleration; //accel value used in distance formula. Returning to end point
	public float spdMod = 1f; //adjust move speed to start point
	public float stopOffset;
	private float yDif;

	public bool isPaused = false; //used when game is paused

	public AudioSource hitWallAudio;
	private bool atEndpoint = false;

	// Use this for initialization
	void Start () {
		if (Mathf.Abs (startPoint.x - endPoint.x) > Mathf.Abs (startPoint.z - endPoint.z)) {
			axis = 'x';
			if (endPoint.x - startPoint.x < 0.0) {
				dir = -1;
			}
		} else {
			axis = 'z';
			if (endPoint.z - startPoint.z < 0.0) {
				dir = -1;
			} else { //if there is no change from in (x,z) from start to end
				dir = 0;
			}
		}
		if (startPoint.y - endPoint.y == 0.0) {
			diagMovement = false;
		}

		curPos = gameObject.transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (dir == 0 || isPaused)
			return;

		prePos = curPos;
		curPos = gameObject.transform.position;

		if (axis == 'x') {
			dist = (curPos.x - prePos.x) * dir;
		}
		else { //if (axis == 'z')
			dist = (curPos.z - prePos.z) * dir;
		}

		if (dist >= -stopOffset) { //if moving toward endpoint or not moving
			if (curPos == endPoint) {
				if (hitWallAudio != null) {
					if (!atEndpoint) {
						hitWallAudio.Play ();
					}
				}
				atEndpoint = true;
				return;
			}
			if (diagMovement) {
				yDif = prePos.y - curPos.y; //should be cur - pre, but does not matter because it will be squared
				dist = Mathf.Sqrt(dist*dist + yDif*yDif); //update dist
			}
			float dTime = Time.deltaTime;
			//float step = (dist + acceleration * dTime * dTime/ 2f) * spdMod;
			float step = dist + acceleration * dTime * dTime/ 2f;
			gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, endPoint, step);
		} 
		else if (diagMovement) { //if moving toward startpoint and has diagonal movement
			
			float step = dist * -1 * spdMod;
			//rigid (potentially jittery)
			gameObject.transform.position = Vector3.MoveTowards (prePos, startPoint, step);
			curPos = gameObject.transform.position;
			//flexible (possiblly not aligned properly)
			//gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, startPoint, step);

			if (atEndpoint) {
				atEndpoint = false;
			}
		}
	}
}
