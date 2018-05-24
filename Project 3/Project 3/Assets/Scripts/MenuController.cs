using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public GameObject settingsMenu;

	// Use this for initialization
	public void newGame () 
	{
		SceneManager.LoadScene (1);
	}
	
	// Update is called once per frame
	public void quitGame () 
	{
		Application.Quit ();
	}

	public void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Application.Quit (); 
		}
	}

	public void Settings()
	{
		settingsMenu.SetActive (true);
	}

	public void mainMenu()
	{
		settingsMenu.SetActive (false);
	}

	public void Start()
	{
		settingsMenu.SetActive (false);
	}
}
