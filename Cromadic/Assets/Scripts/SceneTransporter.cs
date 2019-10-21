using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransporter : MonoBehaviour {
    public string scene_name;
    private pickItems p;

    
    public static bool hold_torch;
    
	// Use this for initialization
	void Start () {
       
		p = GameObject.FindGameObjectWithTag("Player").transform.gameObject.GetComponent<pickItems>();
	}
	
	// Update is called once per frame
	void Update () {
		if(p.gotTorch == true){
            hold_torch = true;
        }
	}

    public void OnTriggerEnter(Collider Object)
    {
       // Debug.Log("Hello");
        //Debug.Log(Object.tag);
        if(Object.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(scene_name);
    }
}
   
