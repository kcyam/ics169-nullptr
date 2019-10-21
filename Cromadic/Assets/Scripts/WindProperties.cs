using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindProperties : MonoBehaviour {
	private Vector3 windDir;
	private bool isBlowing = false;
	
	public void SetWindDir( Vector3 v )
	{
		windDir = v;
	}
	
	public Vector3 GetWindDir()
	{
		return windDir;
	}
	
	public void SetBlowing( bool b )
	{
		isBlowing = b;
	}
	
	public bool IsBlowing()
	{
		return isBlowing;
	}
}
