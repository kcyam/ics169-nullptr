using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterset : MonoBehaviour {
    
    public static characterset instance = null;  //globle only have one. all script share one instance
    
    void Awake(){
        instance = this;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
