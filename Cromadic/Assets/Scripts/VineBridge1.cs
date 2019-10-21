using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBridge1 : MonoBehaviour {
	private GameObject boxCol;
	private Animator anim;
	public float maxLength = 1;
	
	void Start()
	{
		anim = transform.parent.GetComponent<Animator>();
		anim.SetFloat("GrowSpeed", 1);
        transform.localScale += new Vector3(0, 22, 0);
    }
	
	void Update()
	{

    }
	

}
