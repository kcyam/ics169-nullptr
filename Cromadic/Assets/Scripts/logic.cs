using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logic : MonoBehaviour {
    
    public static logic sharedInstance=null;
    public bool wind;
    public bool fire;
    public bool water;
    public GameObject weapon = null;
    //public weaponItem current_item = null;
    public GameObject elementpos;
    private GameObject newbackground =null;
    
    [System.Serializable]
    public class weaponItem{
        public GameObject prefab = null;
        public string name = null;
        public Sprite icon = null;
        public virtual void trigger(GameObject obj){
            
        }
    }
    
    public weaponItem[] weaponList = null;
    
    public GameObject grid = null;
    
    public void changeBackground(string name)
    {   //TODO 
        for(int i =0;i<3; i++){
            if (weaponList[i].name==name){
                if (newbackground!=null){
                    Destroy (newbackground,0.01f);
                }
                wind =false;
                fire= false;
                water= false;
                if(name == "wind"){
                    wind =true;
                }
                newbackground = GameObject.Instantiate(weaponList[i].prefab);
                newbackground.transform.position = elementpos.transform.position;
                newbackground.transform.parent = elementpos.transform;
                Debug.Log(weaponList[i].prefab.name);
            }
        }
        
        //GameObject newbackground = GameObject.Instantiate(item.prefab);
        //newbackground.transform.position = Vector3.zero;
        //weapon = newbackground;
        //current_item = item;
        
    }
   
    

    void Awake(){
        sharedInstance=this;
    }
    void Start(){
        //grid.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = weaponList[0].icon;
       // grid.transform.GetChild(0).gameObject.SetActive(true);
       // grid.transform.GetChild(0).GetComponent<dragme>().item = weaponList[0];
    }
	// Update is called once per frame
	void Update () {
		
	}
}
