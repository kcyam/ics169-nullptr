using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreak : MonoBehaviour {
	
	public GameObject rocks;
	public AudioSource audio;
	
	// Use this for initialization
	void Start () {
		//audio = gameObject.GetComponent<AudioSource> ();
	}
    
    public void OnCollisionEnter(Collision Object)
    {
        print("Boom");
        if (Object.gameObject.CompareTag("Boulder"))
		{
            foreach (Transform child in rocks.transform)
			{
				SphereCollider s = child.gameObject.AddComponent<SphereCollider>();
				s.radius /= 1.8f;
				child.gameObject.AddComponent<Rigidbody>();
				Vector3 pushVector = new Vector3(1/(child.position.x - Object.transform.position.x), 1/(child.position.y - Object.transform.position.y), 1/(child.position.z - Object.transform.position.z));
				float pushForce = 100f;
				child.GetComponent<Rigidbody>().AddForce(Vector3.Scale(pushVector, pushVector) * pushForce);
				
				child.gameObject.AddComponent<RockDelete>();
			}

			if (audio != null)
				audio.Play ();
			
			rocks.transform.DetachChildren();
			gameObject.SetActive(false);
		}

    }
}
