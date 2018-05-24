using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalControl : MonoBehaviour {

	public static GlobalControl Instance;

	public GameStatistics savedGameData = new GameStatistics();
	
	public bool Showed;
	private bool _startGame;
	private static bool _canTouch;
	private static bool _showMainMenu;
	public GameObject OpeningScreen;
	public GameObject Sound;

	void Awake ()   
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () 
	{
		//plays opening credits once 
		transform.GetChild(1).gameObject.SetActive(true);
		OpeningScreen.SetActive(true);
		Invoke( "EnableTouch", 2.2f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		//closes the opening credits and readies the game to be played
		if ( _canTouch && _startGame )
		{
			OpeningScreen.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(false);
			Showed = true;
		}
	}

	private void EnableTouch()
	{
		_canTouch = true;
		OpeningScreen.transform.GetChild(0).gameObject.SetActive(true);
	}

	public void StartGame()
	{
		_startGame = true;
		transform.GetChild(1).GetChild(1).transform.rotation = Quaternion.Euler(Vector3.zero);
	}

	public void MainMenu()
	{
		GameController.instance.restart();
		_startGame = false;
		Showed = false;
		OpeningScreen.SetActive(true);
		transform.GetChild(1).gameObject.SetActive(true);
		OpeningScreen.transform.GetChild(0).gameObject.SetActive(true);
	}
}
