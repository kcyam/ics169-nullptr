using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainProperties : MonoBehaviour {
	private bool raining = false;
	public GameObject cloud;
	
	void Start()
	{
		cloud.transform.localScale = Vector3.zero;
	}
	
	void Update()
	{
		if( raining && cloud.transform.localScale.x < 149f )
		{
			cloud.transform.localScale = ( cloud.transform.localScale + new Vector3(150f, 150f, 100f) ) / 2;
		}
		else if( !raining && cloud.transform.localScale.x > 1f )
		{
			cloud.transform.localScale = ( cloud.transform.localScale + new Vector3(0f, 0f, 0f) ) / 2;
		}
		else if( !raining )
		{
			cloud.transform.localScale = Vector3.zero;
		}
	}
	
	public void SetRaining( bool r )
	{
		raining = r;
	}
	
	public bool IsRaining()
	{
		return raining;
	}
}
