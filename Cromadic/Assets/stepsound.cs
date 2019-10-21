using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepsound : MonoBehaviour {
    private AudioSource _as;
    
    public AudioClip []audioClipArray;
    
    
    void Awake(){
        _as = GetComponent<AudioSource>();
    }
    
    
	//events in the animation curve
    public void LeftStep(string name){
        _as.clip=audioClipArray[Random.Range(0,audioClipArray.Length)];
        _as.PlayOneShot(_as.clip);
        //Debug.Log("left sound!");
    }
    
     public void RightStep(string name){
        _as.clip=audioClipArray[Random.Range(0,audioClipArray.Length)];
        _as.PlayOneShot(_as.clip);
        
    }
}
