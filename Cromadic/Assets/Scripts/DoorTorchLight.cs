using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTorchLight : MonoBehaviour {
    public GameObject waterDestroyFire; //for audio
    public bool isLit = false;

	// Use this for initialization
	void Start () {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        waterDestroyFire.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
    }
    public void waterputsFireOut(){
        isLit = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

        waterDestroyFire.SetActive(true);//for audio
    }
    public void OnTriggerStay(Collider Object) {
        //Add the water putting it out later
        if (Object.tag == "Player" && Input.GetKeyDown(KeyCode.F) && Object.GetComponent<pickItems>().gotFireInHand)
        {
            isLit = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            waterDestroyFire.SetActive(false);//for audio
            //Debug.Log("On");
        }
        if (Object.tag =="FX_Rain" && Object.GetComponent<RainProperties>().IsRaining() && isLit){
            waterputsFireOut();
        }



    }
     


}
