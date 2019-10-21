using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushLightTrigger : MonoBehaviour
{
	public GameObject mushrooms; //some object with all the mushrooms as children
	public Light[] mushLights;
	private Renderer[] renderers;
	private Color[] originalColors;
	public float intensity = 0f;
	public bool lightOn = false;
	public bool lightEnabled = false;
	private int size = 0;

    void Start()
    {
		mushLights = mushrooms.GetComponentsInChildren<Light> ();
		renderers = mushrooms.GetComponentsInChildren<Renderer> ();
		originalColors = new Color[mushLights.Length];
		size = mushLights.Length;

		for (int i = 0; i < size; ++i) {
			originalColors[i] = renderers[i].material.GetColor ("_Color");
			renderers[i].material.SetColor("_Color", Color.black);
			mushLights[i].intensity = intensity;
			mushLights [i].enabled = false;
		}
    }


    void Update()
    {
		if(lightOn) //if light should be on
		{
			if( intensity < 1 ) //and if not at max intensity
			{
				if (!lightEnabled) {
					for (int i = 0; i < size; ++i) {
						mushLights [i].enabled = true;
					}
					lightEnabled = true;
				}

				intensity += Time.deltaTime;
				if (intensity > 1.0f)
					intensity = 1.0f;

				for (int i = 0; i < size; ++i) {
					mushLights[i].intensity = intensity;
					renderers[i].material.SetColor("_Color", originalColors[i] * intensity);
				}
			}
		}
		else
		{
			if( intensity > 0 )
			{
				intensity -= Time.deltaTime;
				if (intensity < 0.0f)
					intensity = 0.0f;
				
				for (int i = 0; i < size; ++i) {
					mushLights[i].intensity = intensity;
					renderers[i].material.SetColor("_Color", originalColors[i] * intensity);
				}
					
				if (lightEnabled && intensity == 0.0f) {
					for (int i = 0; i < size; ++i) {
						mushLights [i].enabled = false;
					}
					lightEnabled = false;
				}
			}
		}
    }

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			lightOn = true;
		}
	}
	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			lightOn = false;
		}
	}
}
