using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsProperties : MonoBehaviour {
	
	//Wind properties
	public bool pushable = false; //Does wind push this object?
	public float pushability = 0; //The higher this is, the faster the object will move in wind
	public bool receiveYForce = true; // Whether or not to be pushed on the Y axis (maybe disable for floating platforms)
	
	//Water properties
	public bool floatsInWater = false; //Does this object float in water?
	public bool snapToWaterHeight = false; //Should this object stay perfectly at the water's height (use for platforms)
	public float buoyantForce = 1f; //How fast does this float to the top of the water?
	
	//Fire properties
	public bool flammable = false;
	
	private GameObject player;
	private static Vector3 windOffset;

	void Start () 
	{
		player = GameObject.Find( "PlayerHandle" );
		windOffset = new Vector3( 0, 1.93f, 0 );
	}
	
	void Update () 
	{
		
	}
	
	void OnTriggerStay( Collider other )
	{
		//WIND PUSHING
		if( pushable && other.gameObject.tag == "Wind" )
		{
			Vector3 playerPos = player.transform.position + windOffset;
			Vector3 dirFromPlayer = transform.position - (player.transform.position + windOffset);
			bool canSeePlayer = !Physics.Raycast( playerPos, Vector3.Normalize(dirFromPlayer), dirFromPlayer.magnitude, LayerMask.GetMask("Ground") );

            if ( other.gameObject.GetComponent<WindProperties>().IsBlowing() && canSeePlayer )
			{
				Vector3 pushDir = other.gameObject.GetComponent<WindProperties>().GetWindDir();
				if( !receiveYForce )
				{
					pushDir.y = 0;
				}
				
				//Debug.Log(pushDir.x);
				
				GetComponent<Rigidbody>().AddForce( pushDir * pushability );
			}
		}
		
		//WATER FLOATING
		if( floatsInWater && other.gameObject.layer == LayerMask.NameToLayer("Water") )
		{
			
		}
	}
}
