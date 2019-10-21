using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectControl : MonoBehaviour {
    public Button level02Button, level03Button;
    public bool LockSceneOn;
    int levelPassed;
    
    
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
		levelPassed = PlayerPrefs.GetInt("LevelPassed");
        if(LockSceneOn){
        level02Button.interactable = false;
        level03Button.interactable = false;
        }
        
        switch(levelPassed){
        case 1:
            level02Button.interactable=true;
            break;
            
        case 2:
            level02Button.interactable=true;
            level03Button.interactable=true;
            break;
        }
	}
	
    public void levelToLoad(int level){
        SceneManager.LoadScene(level);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
