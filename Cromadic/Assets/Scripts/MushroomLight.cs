using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomLight : MonoBehaviour {
	private Light light;
	private Transform player;
	private GameObject mushroom;
	private Color originalColor;
	private float activationDistance = 20;
	
	// Use this for initialization
	void Start () {
		light = GetComponent<Light>();
		player = GameObject.FindWithTag("Player").transform;
		mushroom = transform.parent.Find("Cube").gameObject;
		originalColor = mushroom.GetComponent<Renderer>().material.GetColor("_Color");
	}
	
	// Update is called once per frame
	void Update () {
		if( Vector3.Distance( player.position, transform.position ) > activationDistance )
		{
			if( light.intensity > 0 )
			{
				light.intensity -= Time.deltaTime;
				mushroom.GetComponent<Renderer>().material.SetColor("_Color", originalColor * light.intensity);
				if( light.intensity <= 0 )
				{
					light.enabled = false;
					light.intensity = 0;
				}
			}
		}
		else
		{
			if( light.intensity < 1 )
			{
				light.enabled = true;
				light.intensity += Time.deltaTime;
				mushroom.GetComponent<Renderer>().material.SetColor("_Color", originalColor * light.intensity);
				if( light.intensity > 1 )
				{
					light.intensity = 1;
				}
			}
		}
	}
}
