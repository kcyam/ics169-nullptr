using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class storyscene: MonoBehaviour
{
 private int a;
    public GameObject canvas;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canvas = GameObject.Find("Canvas");
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        
    }
    void Update()
    { 
    }


   public void nextlevel(){
        if(SceneManager.GetActiveScene().buildIndex == 7)
        {
            SceneManager.LoadScene(0);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("Tutorial");
    }

}
