using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragme : MonoBehaviour {
    
    bool found = false;
    Vector3 offset = Vector3.zero; //差值
    public logic.weaponItem item = null;




    // Use this for initialization
    void Awake () {

    }



    void Update(){

       
        GameObject Tailsman = characterset.instance.gameObject;
        RectTransform main = Tailsman.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        RectTransform right = Tailsman.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        RectTransform left = Tailsman.transform.GetChild(2).gameObject.GetComponent<RectTransform>();

        if (Input.GetKeyDown(KeyCode.E))
        {
            
            
            Sprite a = main.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite;

            main.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = right.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
                right.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = a;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
          
            Sprite a = main.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite;

            main.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = left.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            left.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = a;
        }
        string name = main.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().sprite.name;
        Debug.Log(name);
        if(name != null){
            logic.sharedInstance.changeBackground(name);
         }
               

        
       
    }
}
