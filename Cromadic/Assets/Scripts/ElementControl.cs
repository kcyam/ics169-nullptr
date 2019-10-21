using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ElementControl : MonoBehaviour
{
    public GameObject GUICanvas;
    public GameObject firePrefab;
	private Animator anim;
	private Transform headObject;
	private Transform backWindPos;
	private List<ParticleSystem> groundDust;
	private Transform windCollider;
    public DoorTorchLight L; 

    //public AudioClip windSound;
    //public GameObject windSound;
    public GameObject fireSound;
    public GameObject waterSound;

    public AudioSource switchSound;
    public AudioSource windSound;
    public AudioSource rainSound;
    private enum elementType
    {
        water,
        wind,
        fire
    };
    public GameObject spraywaterParticles;
    private elementType elementIndex;       //Which element is currently selected   
    private Vector3 windDirection;          //Which way the wind is blowing
	private bool windBlowing;				//Is wind blowing?
	private Transform forwardWindParticles;//Wind particle effect
	private Transform backwardWindParticles;
    private GameObject fireParticles;
	private float windOffset = 60f;				//How far away from the player the wind particle effect starts
	private int windPushOrPull = 0;
	private float groundDustSpeed = 70f;        //speed multiplier for ground dust particles
	private GameObject model;

    private bool playGrowingSound = false;

    public bool leftClicking = false, rightClicking = false;
	public bool absorbingWater = false, shootingWater = false;
	public float waterStored = 0; 				//How much water you can currently shoot
	private float waterCapacity = 100f;			//How much water you can store total
	
	private GameObject[] pushableObjects;	//List of objects affected by wind
	public GameObject waterIn;
    private GameObject vineBridge;
    private Scene scene;
	
	private bool eyesClosed = false;
	public Material openEyesMaterial;
	public Material closedEyesMaterial;

	public bool isPaused = false; //used when game is paused

    // Use this for initialization
    void Start()
    {
        scene = SceneManager.GetActiveScene ();
		
		//playGrowingSound = false;
        try
		{
			L = GameObject.FindGameObjectWithTag("fire base1").transform.gameObject.GetComponent<DoorTorchLight>();
		}
		catch(NullReferenceException e)
		{
			Debug.Log("No fire base found");
		}
        elementIndex = elementType.wind;
        GUICanvas.GetComponent<GUIControl>().SelectElement((int)elementIndex);
		
		windBlowing = false;
        windDirection = Vector3.zero;
		waterStored = 0;
		model = gameObject.GetComponent<actorcontroller> ().model;
		anim = model.GetComponent<Animator> ();
		
		pushableObjects = GameObject.FindGameObjectsWithTag( "Pushable" );
        //vineBridge = GameObject.FindGameObjectWithTag("original vine");
        
		headObject = transform.Find( "CameraHandle" );
		if(headObject == null)
		{
			headObject = transform.Find( "CameraHandle2" ).Find( "CameraHandle" );
		}
		//Debug.Log(headObject);
		windCollider = headObject.Find( "WindCollision" );
		//Debug.Log(windCollider);
		
		forwardWindParticles = headObject.Find( "forward wind" );
		backwardWindParticles = headObject.Find( "backwards wind" );
		backWindPos = headObject.Find( "backwards wind position" );
        
		
		groundDust = new List<ParticleSystem>();
		
		waterIn = null;
        
        
    }

    // Update is called once per frame
    void Update()
    {
		if (isPaused) {
			return;
		}
        
        if(scene.name != "Tutorial"){
            //Cycling through elements
            if(scene.name != "Level 1"){
            if ( Input.GetKeyDown(KeyCode.Q) )
            {
                if((int)elementIndex ==1){
                ShiftElementLeft();
                }
                else if((int)elementIndex ==0){
                    ShiftElementRight();
                }
                switchSound.Play();
                GUICanvas.GetComponent<GUIControl>().SelectElement( (int)elementIndex );
            }
//            else if ( Input.GetKeyDown(KeyCode.E) )
//            {
//                ShiftElementRight();
//                switchSound.Play();
//                GUICanvas.GetComponent<GUIControl>().SelectElement( (int)elementIndex );
//            }
            }
            //For left clicking
            if ( Input.GetMouseButtonDown(0) )
            {
                leftClicking = true;
                switch ( elementIndex )
                {
                    //BLOW WIND
                    case elementType.wind:
                        windPushOrPull = 1;
                        windBlowing = true;
                        windCollider.GetComponent<WindProperties>().SetBlowing(true);
                        forwardWindParticles.GetComponent<ParticleSystem>().Play();
                        backwardWindParticles.GetComponent<ParticleSystem>().Stop();
                        windSound.Play();
                        //AudioSource.PlayClipAtPoint(windSound, transform.position);
                        //fireSound.SetActive(false);
                        //windSound.SetActive(true);
                        //waterSound.SetActive(false);
                        break;

                    //SPRAY WATER
                    case elementType.water:
                        shootingWater = true;
                        absorbingWater = false;
                        //fireSound.SetActive(false);
                        rainSound.Play();
                        //windSound.SetActive(false);
                        //waterSound.SetActive(true);
                        break;

                    case elementType.fire:
                        //INSERT WHAT TO DO FOR FIRE
                        fireSound.SetActive(true);
                        //windSound.SetActive(false);
                        waterSound.SetActive(false);

                        break;
                }
            }
        
            //For right clicking
            else if ( Input.GetMouseButtonDown(1) )
            {
                rightClicking = true;
                switch ( elementIndex )
                {
                    //PULL WIND
                    case elementType.wind:
                        windPushOrPull = -1;
                        windBlowing = true;
                        windCollider.GetComponent<WindProperties>().SetBlowing(true);
                        forwardWindParticles.GetComponent<ParticleSystem>().Stop();
                        backwardWindParticles.GetComponent<ParticleSystem>().Play();
                        windSound.Play();
                        //AudioSource.PlayClipAtPoint(windSound, transform.position);
                        fireSound.SetActive(false);
                        //windSound.SetActive(true);
                        waterSound.SetActive(false);
                        break;

                    //ABSORB WATER
                    case elementType.water:
                        absorbingWater = true;
                        shootingWater = false;
                        break;

                    case elementType.fire:
                        //INSERT WHAT TO DO FOR FIRE
                        break;
                }
            }

            //RELEASE LEFT CLICK
            if( Input.GetMouseButtonUp(0) )
            {
                leftClicking = false;
                switch( elementIndex )
                {
                    case elementType.wind:
                        forwardWindParticles.GetComponent<ParticleSystem>().Stop();
                        windSound.Stop();
                        if ( rightClicking )
                        {
                            windPushOrPull = -1;
                            backwardWindParticles.GetComponent<ParticleSystem>().Play();
                            windSound.Play();
                        }
                        else
                        {
                            windBlowing = false;                                	
                            windCollider.GetComponent<WindProperties>().SetBlowing(false);
                        }
                        break;

                    case elementType.water:
                        spraywaterParticles.GetComponent<ParticleSystem>().Stop();
                        spraywaterParticles.GetComponent<RainProperties>().SetRaining(false);
                        rainSound.Stop();
                        playGrowingSound = false;
                        shootingWater = false;
                        if( rightClicking )
                        {
                            absorbingWater = true;
                        }
                        break;
                }
            }

            //RELEASE RIGHT CLICK
            if( Input.GetMouseButtonUp(1) )
            {
                rightClicking = false;
                switch( elementIndex )
                {
                    case elementType.wind:
                        backwardWindParticles.GetComponent<ParticleSystem>().Stop();
                        windSound.Stop();
                        if ( leftClicking )
                        {
                            windPushOrPull = 1;
                            forwardWindParticles.GetComponent<ParticleSystem>().Play();
                        }
                        else
                        {
                            windBlowing = false;                                	
                            windCollider.GetComponent<WindProperties>().SetBlowing(false);
                        }
                        break;

                    case elementType.water:
                        absorbingWater = false;
                        if( leftClicking )
                        {
                            shootingWater = true;
                        }
                        break;
                }
            }
        }
		
		//Handle wind
		if( windBlowing )
		{
			if( windPushOrPull == -1 )
			{
				RaycastHit hit;
				Vector3 desiredDir = backwardWindParticles.position - headObject.position;
				if( Physics.Raycast( headObject.position, desiredDir, out hit, 13.17f, LayerMask.GetMask("Ground") ) ){
					backwardWindParticles.position = hit.point;
				}
				else
				{
					backwardWindParticles.position = backWindPos.position;
				}
			}
			Vector3 lookDir = headObject.forward;
			//model.transform.eulerAngles = new Vector3(0f, gameObject.transform.eulerAngles.y, 0f);
			windDirection = windPushOrPull * lookDir;
			windCollider.GetComponent<WindProperties>().SetWindDir( windDirection );
		}
		
		//Handle water
		if( shootingWater )
		{
            spraywaterParticles.GetComponent<ParticleSystem>().Play();
			spraywaterParticles.GetComponent<RainProperties>().SetRaining(true);
            if ( waterIn != null )
			{
				waterStored -= waterIn.GetComponent<WaterProperties>().Fill( waterStored );
			}
			
			else
			{
				RaycastHit hit;
				if ( Physics.Raycast( headObject.position, headObject.forward, out hit, 50 ) )
				{
					if( hit.transform.gameObject.layer == LayerMask.NameToLayer( "Water" ) )
					{      
                    
						waterStored -= hit.transform.GetComponent<WaterProperties>().Fill( waterStored );
					}
                    /*
					if( hit.transform.gameObject.tag =="original vine" )
                    {   
                        vineBridge.transform.localScale += new Vector3(0, Time.deltaTime*0.4f*8f, 0);
						vineBridge.transform.position += new Vector3(0, 0, Time.deltaTime*0.4f*4f);
                        playGrowingSound = true;
                    }
					*/
                    if ( hit.transform.gameObject.tag =="fire base1" )
                    {  Debug.Log("222");
                        L.waterputsFireOut();
                    }
                    
				}
                
			}
		}
		
		else if( absorbingWater )
		{
			if( waterIn != null )
			{
				waterStored += waterIn.GetComponent<WaterProperties>().Drain( waterCapacity - waterStored );
			}
			
			else
			{
				RaycastHit hit;
				if ( Physics.Raycast( headObject.position, headObject.forward, out hit, 50 ) )
				{
					if( hit.transform.gameObject.layer == LayerMask.NameToLayer( "Water" ) )
					{
						waterStored += hit.transform.GetComponent<WaterProperties>().Drain( waterCapacity - waterStored );
					}
				}
			}
		}
		
		if( leftClicking || rightClicking )
		{
			anim.SetLayerWeight(1, ( anim.GetLayerWeight(1)*3 + 0.75f ) / 4);
			if( !eyesClosed )
			{
				eyesClosed = true;
				Renderer r = model.transform.Find("Plane").GetComponent<Renderer>();
				r.material = closedEyesMaterial;
			}
		}
		
		else
		{
			anim.SetLayerWeight(1, anim.GetLayerWeight(1) * 0.75f);
			if( eyesClosed )
			{
				eyesClosed = false;
				Renderer r = model.transform.Find("Plane").GetComponent<Renderer>();
				r.material = openEyesMaterial;
			}
		}
    }

    //Change selected element
    void ShiftElementLeft()
    {
        switch ( elementIndex )
        {
            case elementType.wind:
                elementIndex = elementType.water;

				forwardWindParticles.GetComponent<ParticleSystem>().Stop();
				backwardWindParticles.GetComponent<ParticleSystem>().Stop();
				windCollider.GetComponent<WindProperties>().SetBlowing(false);
				windBlowing = false;
                break;

//            case elementType.water:
//                spraywaterParticles.GetComponent<ParticleSystem>().Stop();
//                rainSound.Stop();
//				spraywaterParticles.GetComponent<RainProperties>().SetRaining(false);
//				elementIndex = elementType.fire;
//				absorbingWater = false;
//				shootingWater = false;
//                break;
//
//            case elementType.fire:
//                elementIndex = elementType.wind;
//                break;
        }
    }

    void ShiftElementRight()
    {
        switch ( elementIndex )
        {
//            case elementType.wind:
//                elementIndex = elementType.fire;
//				forwardWindParticles.GetComponent<ParticleSystem>().Stop();
//				backwardWindParticles.GetComponent<ParticleSystem>().Stop();
//				windCollider.GetComponent<WindProperties>().SetBlowing(false);
//				windBlowing = false;
//                break;

            case elementType.water:
                spraywaterParticles.GetComponent<ParticleSystem>().Stop();
                rainSound.Stop();
				spraywaterParticles.GetComponent<RainProperties>().SetRaining(false);
                elementIndex = elementType.wind;
				absorbingWater = false;
				shootingWater = false;
                break;

//            case elementType.fire:
//                elementIndex = elementType.water;
//                break;
        }
    }
	
	
	//For ground dust particle systems
	public void AddGroundDust(ParticleSystem p)
	{
		groundDust.Add(p);
	}
	
	void OnTriggerEnter( Collider other )
	{
		if( other.transform.gameObject.layer == LayerMask.NameToLayer( "Water" ) )
		{
			waterIn = other.transform.gameObject;
			Debug.Log(waterIn);
		}
	}
	
	void OnTriggerExit( Collider other )
	{
		if( other.transform.gameObject == waterIn )
		{
			waterIn = null;
			Debug.Log(waterIn);
		}
	}
}

//OLD WIND CODE
/*
					case elementType.wind:
					Vector3 lookDir = headObject.forward;								//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
					windDirection = new Vector3(lookDir.x, 0, lookDir.z);				//Update info for pushable objects
					windBlowing = true;                                               	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                    //AudioSource.PlayClipAtPoint(windSound, transform.position);
                    fireSound.SetActive(false);
                    windSound.SetActive(true);
                    waterSound.SetActive(false);
                    //GetComponent<actorcontroller>().updateWind(windDirection, true);	//Enable boosted gliding
					
					windParticles.transform.position = transform.position - (windOffset * windDirection);	//Control particle system
					windParticles.GetComponent<ParticleSystem>().Play();
					Vector3 lookAngles = headObject.eulerAngles;
					windParticles.transform.rotation = Quaternion.Euler(0, lookAngles.y, 0);
					
					foreach (ParticleSystem p in groundDust)
					{
						ParticleSystem.ForceOverLifetimeModule fol = p.forceOverLifetime;
						fol.x = windDirection.x * groundDustSpeed;
						fol.z = windDirection.z * groundDustSpeed;
						
						p.Play();
					}
					
					
					//STOP WIND
					case elementType.wind:
					windBlowing = false;												//Stop moving pushable objects
					GetComponent<actorcontroller>().updateWind(Vector3.zero, false);	//Disable glide boosting
					windParticles.GetComponent<ParticleSystem>().Stop();				//Stop particle effect
					
					foreach ( ParticleSystem p in groundDust )							//Stop ground dust
					{
						p.Stop();
					}
                    break;
					
					
					//Make the particle system follow the player at a distance of 10
					if(windBlowing)
					{
						windParticles.transform.position = transform.position - (windOffset * windDirection);
					}
					
	
	//For physics related interactions
	void FixedUpdate()
	{
		if(windBlowing)
		{
			foreach (GameObject g in pushableObjects)
			{
				float pushability = g.GetComponent<PhysicsProperties>().pushability;
				g.GetComponent<Rigidbody>().AddForce(windDirection * pushability);
			}
		}
	}
*/
