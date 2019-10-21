﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class start_new_game : MonoBehaviour {
    //Scene to change
	public int SceneDestination = 0;


	//Fade out on start
	public bool FadeOnStart = true;

	//Fade Animator
	private Animator FadeAnimator = null;

	//Target position for player when they arrive at destination
	public Vector3 TargetPosition = Vector3.zero;

	//Last position to set player
	public static Vector3 LastTarget = Vector3.zero;

    
	//Trigger Name
	private int FadeInTrigger = Animator.StringToHash("FadeIn");
	private int FadeOutTrigger = Animator.StringToHash("FadeOut");
	//--------------------------------
	void Start()
	{

        FadeAnimator = GetComponent<Animator>();

		if(FadeOnStart && FadeAnimator!=null)
			FadeAnimator.SetTrigger(FadeInTrigger);
	}
	//--------------------------------
	public void SceneChange()
	{
		start_new_game.LastTarget = TargetPosition;
        //Application.LoadLevel(SceneDestination);
        
        SceneManager.LoadScene("Story1");
	}
	//--------------------------------
	void OnTriggerEnter(Collider other)
	{
		//If not player entered, then exit
		if(!other.gameObject.CompareTag("Player")) return;

        //Lock player controls
        //PlayerControl.PlayerInstance.CanControl = false;
        
        SceneChange();
        Debug.Log("change");
		FadeAnimator.SetTrigger(FadeOutTrigger);
	}
	//--------------------------------
}
//--------------------------------
	

