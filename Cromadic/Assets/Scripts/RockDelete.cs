using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDelete : MonoBehaviour
{
	private float delay = 2f;

    // Update is called once per frame
    void Update()
    {
		
		if( delay > 0f )
		{
			delay -= Time.deltaTime;
		}
		
		else
		{
			Destroy(gameObject);
		}
    }
}
