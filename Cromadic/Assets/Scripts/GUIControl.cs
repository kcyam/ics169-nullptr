using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIControl : MonoBehaviour {
	private Scene scene;
	private Transform feather;
	private Transform[] elementCircles;
    public Image[] img;
	private float[] elementCircleScales;
	private int elementSelected;
    public actorcontroller act;
    public ElementControl E;
    private bool gliding;
    private float desiredRotation;
    float count;
    float count2;
    private bool move;
    private GameObject player;
    public Vector2 range = new Vector2(20f, 20f);/// range:UI移动的范围
    private Vector2 mRot = Vector2.zero;/// mRot:移动中平滑运动的差值
    private float moveSpeed = 5;//差值的速度,即到达旋转角度的速度
    private float halfWidth = Screen.width * .5f;
    private float halfHeight = Screen.height * .5f;

	// Use this for initialization
	void Start () {
        scene = SceneManager.GetActiveScene ();

		elementCircles = new Transform[3];
		elementCircles[0] = transform.Find("Water");
		elementCircles[1] = transform.Find("Wind");
        player= GameObject.Find("crowtagonist");
        img[0] = GameObject.Find("Water").GetComponent<Image>();
		img[1] = GameObject.Find("Wind").GetComponent<Image>();
		
		elementCircleScales = new float[3];
		elementCircleScales[0] = 1;
		elementCircleScales[1] = 1;
		elementCircleScales[2] = 1;
	}
	
	// Update is called once per frame
	void Update () {
         float x = Mathf.Clamp((Input.mousePosition.x - halfWidth)/halfWidth, -1, 1);
        float y = Mathf.Clamp((Input.mousePosition.y - halfHeight)/halfHeight, -1, 1);
        mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * moveSpeed);
        elementCircles[0].transform.localRotation = Quaternion.Euler(-mRot.y * range.y, mRot.x * range.x, 0f);

//		desiredRotation = (feather.eulerAngles.z + 60 - (elementSelected*25))/2f;
//		feather.Rotate(0, 0, desiredRotation - feather.eulerAngles.z);
	    
       
        
        // Q for water  , E for wind
//		for(int i = 0; i < 2; ++i)
//		{
//			if(elementSelected == i)
//			{
//				elementCircleScales[i] = (elementCircleScales[i] + 1.5f)/2f;
//			}
//			else
//			{
//				elementCircleScales[i] = (elementCircleScales[i] + 1f)/2f;
//			}
//			elementCircles[i].localScale = Vector3.one * elementCircleScales[i];
//		}
//        
        if(scene.name == "Tutorial")
        {
            img[0].color = new Color(0.4716981f,0.44f,0.44f,1);
            elementCircles[0].GetChild(0).gameObject.SetActive(true);
            img[1].color = new Color(0.4716981f,0.44f,0.44f,1);
            elementCircles[1].GetChild(0).gameObject.SetActive(true);
        }
        if(scene.name == "Level 1")
        {
            
            if ( E.leftClicking  ){
                elementCircleScales[1] = (elementCircleScales[1] + 1.5f)/2f;
                elementCircles[1].localScale = Vector3.one * elementCircleScales[1];
                
            }
            if  (E.rightClicking){
                //player.transform.localRotation=Quaternion.Euler(transform.localRotation.x,transform.localRotation.y+180f,transform.localRotation.z);
                elementCircleScales[1] = (elementCircleScales[1] + 1.5f)/2f;
                elementCircles[1].localScale = Vector3.one * elementCircleScales[1];
                
            }
            else if (!E.leftClicking && !E.rightClicking) {
				elementCircleScales[1] = (elementCircleScales[1] + 1f)/2f;
                elementCircles[1].localScale = Vector3.one * elementCircleScales[1];
            }
            
            
            elementCircles[0].gameObject.SetActive(false);
            img[0].color = new Color(0.4716981f,0.44f,0.44f,1);
            //wind true
            img[1].color = new Color(1f,1f,1f,1);
            elementCircles[1].GetChild(0).gameObject.SetActive(false);
        }
        if(scene.name == "level 2")
        {
            if(Input.GetKeyDown("q")||Input.GetKeyDown("e")){
            if(elementSelected ==1)
                {
                if(count2==0){
                Animator a1 = img[0].GetComponent<Animator>();
                Animator a2 = img[1].GetComponent<Animator>();
                a1.Play("water");
                a2.Play("wind");
                Sprite temp = img[0].sprite;
                img[0].sprite=img[1].sprite;
                img[1].sprite = temp;
                    count2+=1;
                                   count=0;}
                }
            else{
                if (count==0){
                Animator a1 = img[0].GetComponent<Animator>();
                Animator a2 = img[1].GetComponent<Animator>();
                a1.Play("water");
                a2.Play("wind");
                Sprite temp = img[0].sprite;
                img[0].sprite=img[1].sprite;
                img[1].sprite = temp;
                count+=1;
                    count2=0;
                }
                }
            }
            if(E.leftClicking )
            {
                elementCircleScales[1] = (elementCircleScales[1] + 1.5f)/2f;
                elementCircles[1].localScale = Vector3.one * elementCircleScales[1];
                
            }
            if (E.rightClicking){
                elementCircleScales[1] = (elementCircleScales[1] + 1.5f)/2f;
                elementCircles[1].localScale = Vector3.one * elementCircleScales[1];
                
                }
            else if (!E.leftClicking && !E.rightClicking) {
				elementCircleScales[1] = (elementCircleScales[1] + 1f)/2f;
                elementCircles[1].localScale = Vector3.one * elementCircleScales[1];
            }
        }
	}
	
	public void SelectElement(int element)
	{
		elementSelected = element;
	}
}
