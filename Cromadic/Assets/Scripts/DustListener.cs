using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustListener : MonoBehaviour {
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("PlayerHandle");
	}
	
	
	void Update()
	{
		player.GetComponent<ElementControl>().AddGroundDust(transform.GetComponent<ParticleSystem>());
		Destroy(this);
	}
}
