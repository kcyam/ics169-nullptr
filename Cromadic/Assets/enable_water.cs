using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enable_water : MonoBehaviour {
    public trigger_waterfall w;
    public GameObject waterplane1;
    public GameObject waterplane2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(w.waterfall==true){
            waterplane1.SetActive(true);
            waterplane2.SetActive(true);
        }
	}
}
