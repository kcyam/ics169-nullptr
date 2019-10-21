using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProperties : MonoBehaviour {
	public float drainSpeed = 0.005f;
	public float fillSpeed = 0.005f;
	public float maxVolume = 1f;
	public float currentVolume = 0.3f;


	private Renderer rend;
	private float minHeight;
	private float maxHeight;
	private float currentHeight;	//for Shader
	public float actualHeight;      //for other calculations, Y coordinate of top of water
    private DoorTorchLight DoortorchLight;
    public bool extinguishFire = false;


    // Use this for initialization
    void Start () 
	{
		maxVolume = Mathf.Max( 0, maxVolume );
		currentVolume = Mathf.Clamp( currentVolume, 0, maxVolume );
		
		//For the water shader
		rend = GetComponent<Renderer>();
		Collider col = GetComponent<Collider>();
		minHeight = transform.position.y - col.bounds.extents.y;
		maxHeight = transform.position.y + col.bounds.extents.y;
		UpdateHeight();
	}

    public void Update()
    {
        if (actualHeight >= 26 && extinguishFire)
        {
            GameObject.Find("fire base 1").GetComponent<DoorTorchLight>().isLit = false;
            if (GameObject.Find("CubeToDes"))
            {
                GameObject.Find("CubeToDes").SetActive(false);
            }
        }
    }
    //Returns the amount successfully drained
    //float capacity is how much MORE water the player can carry
    public float Drain( float capacity )
	{
		float drainAmount = Mathf.Min( drainSpeed, currentVolume );
		drainAmount = Mathf.Min( drainAmount, capacity );
		currentVolume -= drainAmount;
		UpdateHeight();
		return drainAmount;
	}
	
	//Returns the amount successfully filled
	//float stored is the amount of water the player is currently carrying
	public float Fill( float stored )
	{
		float fillAmount = Mathf.Min( fillSpeed, maxVolume - currentVolume );
		fillAmount = Mathf.Min( fillAmount, stored );
		currentVolume += fillAmount;
		UpdateHeight();
		return fillAmount;
	}
	
	void UpdateHeight()
	{
		currentHeight = ( ( currentVolume / maxVolume ) - 0.5f );
		actualHeight = minHeight + ( ( maxHeight - minHeight ) * ( currentVolume / maxVolume) );
		rend.material.SetFloat("_AbsoluteHeight", currentHeight);
	}

	public float GetActualHeight(){
		return actualHeight;
	}
}
