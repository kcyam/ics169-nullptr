using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBridge : MonoBehaviour {
	private GameObject boxCol;
	private Animator anim;
	private bool playGrowingSound;
	public float maxLength = 1;
	public GameObject vineGrowSound;
	
	void Start()
	{
		anim = transform.parent.GetComponent<Animator>();
		anim.SetFloat("GrowSpeed", 0);
	}
	
	void Update()
	{
		if (playGrowingSound == true)
        {
            vineGrowSound.SetActive(true);
        }
        else if (playGrowingSound == false)
        {
            vineGrowSound.SetActive(false);
        }
	}
	
	void OnTriggerStay( Collider other )
	{
		if( other.gameObject.tag == "FX_Rain" )
		{
			if( other.gameObject.GetComponent<RainProperties>().IsRaining() && transform.localScale.y < maxLength )
			{
				transform.localScale += new Vector3(0, Time.deltaTime*0.4f*8f, 0);
				//transform.position += new Vector3(0, 0, Time.deltaTime*0.4f*4f);
				playGrowingSound = true;
				anim.SetFloat("GrowSpeed", 0.184f);
			}
			
			else
			{
				playGrowingSound = false;
				anim.SetFloat("GrowSpeed", 0);
			}
		}
	}
	
	void OnTriggerExit( Collider other )
	{
		if( other.gameObject.tag == "FX_Rain" )
		{
			playGrowingSound = false;
			anim.SetFloat("GrowSpeed", 0);
		}
	}
}
