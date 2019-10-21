using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRailGear : MonoBehaviour
{
	public GameObject boat;
	public float yRotation = 0f;
	public float zDir = 1f;
	private Vector3 offset;
    private bool canPlaySound=false;
    public GameObject rotatingRailSound;
	
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - boat.transform.position;
        rotatingRailSound.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x > 38 && transform.position.x < 66)
        {
            rotatingRailSound.SetActive(true);
        }
        else
        {
            rotatingRailSound.SetActive(false);
        }

        transform.position = boat.transform.position + offset;
		transform.rotation = Quaternion.EulerAngles( 0, yRotation, -transform.position.x * zDir);
    }
}
