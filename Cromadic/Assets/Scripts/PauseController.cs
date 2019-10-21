using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {
	public string mainMenuScene;
	public GameObject pauseMenu;
	public bool isPaused;
	public GameObject pausePanel;
	public GameObject controlPanel;
    public AudioSource escSound;

    //Additional scripts that need to be paused
    private GameObject player;
	private CamLook cl;
	private ElementControl ec;
	public BoatRailMovement brm;

	private GameObject textbox;
	private bool tbActive = false;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("PlayerHandle");
		textbox = GameObject.Find ("PlayerHandle/GUI/Textbox");
		if (player != null) {
			ec = player.GetComponent<ElementControl> ();
			cl = player.GetComponentInChildren<CamLook> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//isCursorVisible = Cursor.visible;
		if (Input.GetKeyDown (KeyCode.Escape)) {
            escSound.Play();
            if (isPaused) {
				ResumeGame ();
			} 
			else {
				Pause ();
            }

		}

		//restarting scene (in case of bugs)
		if (isPaused && Input.GetKeyDown (KeyCode.P)) {
			RestartLevel ();
		}

		if (!isPaused && Cursor.visible == true) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	void Pause(){
		//Debug.Log ("paused");
		isPaused = true;
		pauseMenu.SetActive (true);
        //AudioListener.pause = true;
        //comment this in case UI sounds can be played, will disable all other sounds later
		ec.isPaused = true;
		cl.isPaused = true;
		if (brm != null)
			brm.isPaused = true;
		if (textbox.activeSelf) {
			tbActive = true;
			textbox.SetActive (false);
		}
		Time.timeScale = 0f;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void ResumeGame(){
		//Debug.Log ("unpaused");
		//if leaving pause screen while looking at controls screen, reset to default
		pausePanel.SetActive (true);
		controlPanel.SetActive (false);

		isPaused = false;
		pauseMenu.SetActive (false);
		//AudioListener.pause = false;
		ec.isPaused = false;
		cl.isPaused = false;
		if (brm != null)
			brm.isPaused = false;
		if (tbActive) {
			tbActive = false;
			textbox.SetActive (true);
		}
		Time.timeScale = 1f;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void RestartLevel(){
		ResumeGame ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void ReturnToMainMenu(){
		ResumeGame ();
		SceneManager.LoadScene ("start page");
	}
}
