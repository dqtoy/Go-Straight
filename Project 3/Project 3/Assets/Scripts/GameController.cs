using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour {

	public static GameController instance;
	public static int messageNo;

	private const int MESSAGE_GAP = 15;
	private const int MESSAGE_DURATION = 2;
	private const float BACKGROUND_HEIGHT = 25.9f;
	private const int BACKGROUND_COUNT = 3;
	private const int WALL_COUNT = 8;
	private const int WALL_DISTANCE = 5;
	private const int DELETION_GAP = 2;
	private const int DELETION_DISTANCE = DELETION_GAP * WALL_DISTANCE;

	public GameStatistics localGameData = new GameStatistics ();

	public GameObject obstacle;
	public List<GameObject> walls;
	public GameObject background;
	public List<GameObject> backgrounds;
	public GameObject loseScreen;
	public GameObject pauseScreen;
	public GameObject gameScreen;
	public GameObject settingsMenu;
	public GameObject messages;

	private Player player;
	private bool gameOver;
	private bool paused;
	private bool shown;
	private int score;
	private string[] messageTexts = new string[]{ "IS THAT THE BEST YOU GOT?", "NOW WE\'RE TALKING", "GOOD JOB!","KEEP GOING!","WELL DONE!", "YOU GO GIRL",};
	private string MotivationMessage;

	//to avoid multiple game controllers
	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () 
	{
		LoadGameData ();

		player = FindObjectOfType<Player>();
		walls = new List<GameObject> ();

		loseScreen.SetActive (false);
		pauseScreen.SetActive (false);
		settingsMenu.SetActive (false);
		messages.SetActive (false);
		gameScreen.SetActive(false);

		BuildLevel ();
		
		score = 0;
		messageNo = 0;

		gameOver = false;
		paused = false;
		shown = false;

		UpdatehighScore ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GlobalControl.Instance.Showed)
		{
			gameScreen.SetActive(true);
			transform.GetChild(0).gameObject.SetActive(true);

			if (player.Started())
			{
				gameScreen.transform.GetChild(3).gameObject.SetActive(false);
				transform.GetChild(0).gameObject.SetActive(false);
				if (!paused)
				{
					gameScreen.transform.GetChild(2).gameObject.SetActive(true);
				}
			}

			if (gameOver)
			{
				loseScreen.transform.GetChild(1).GetComponent<Text>().text = score + "";
				loseScreen.SetActive(true);
				gameScreen.transform.GetChild(2).gameObject.SetActive(false);
				SaveGameData();
				UpdatehighScore();
			}

			UpdateScore();

			if (!gameOver && score != 0 && score % MESSAGE_GAP == 0)
			{
				if (!shown)
				{
					ShowMessage();
				}
			}

			if (Input.GetKeyDown(KeyCode.Escape) && !gameOver && player.Started())
			{
				PauseGame();
			}

		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	//Creates the obstacles
	void BuildLevel()
	{
		for ( int i = 0; i < WALL_COUNT; i++ )
		{
			walls.Add( Instantiate (obstacle, new Vector3 (0, WALL_DISTANCE * ( i + 1 ), 1), transform.rotation) );
		}
		for ( int i = 0; i < BACKGROUND_COUNT; i++ )
		{
			backgrounds.Add ( Instantiate( background, new Vector3( 0, (BACKGROUND_HEIGHT * i) + WALL_DISTANCE,1 ), transform.rotation) );
		}
	}

	//updates the best score
	void UpdatehighScore()
	{
		Text highScoreText = gameScreen.transform.GetChild (1).gameObject.GetComponent<Text> ();
		if (score > localGameData.highScore)
		{
			localGameData.highScore = score;
		}
		highScoreText.text = "High Score: " + localGameData.highScore;
	}

	//restarts game
	public void restart()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		//Start();
	}

	//goes to the mainMenu
	public void mainMenu()
	{
		GlobalControl.Instance.MainMenu();
	}

	public void PauseGame()
	{
		paused = true;
		settingsMenu.SetActive (false);
		player.OnPauseGame ();
		gameScreen.transform.GetChild (2).gameObject.SetActive (false);
		pauseScreen.SetActive (true);

		foreach (GameObject obs in walls)
		{
			obs.GetComponent<Obstacle>().OnPauseGame ();
		}
	}

	public void resumeGame()
	{
		paused = false;
		player.OnResumeGame ();
		gameScreen.transform.GetChild (2).gameObject.SetActive (true);
		pauseScreen.SetActive (false);

		foreach (GameObject obs in walls)
		{
			obs.GetComponent<Obstacle>().OnResumeGame ();
		}
	}

	public void SettingsMenu()
	{
		settingsMenu.SetActive (true);
	}

	public void DisableSettingsMenu()
	{
		settingsMenu.SetActive(false);
	}

	//ends the game
	public void EndGame()
	{
		foreach (GameObject obs in walls)
		{
			obs.GetComponent<Obstacle>().EndGame ();
		}
		gameOver = true;
	}

	//increments the score accordngly
	void UpdateScore ()
	{
		//score = (int)player.transform.position.y / WALL_DISTANCE;
		Text scoreText = gameScreen.transform.GetChild (0).gameObject.GetComponent<Text> ();
		scoreText.text = "Score: " + score;
	}

	public void incrementScore()
	{
		score++;
	}

	//returns deletion distance
	public float getDeletionDistance()
	{
		return DELETION_DISTANCE;
	}

	//returns wall distance
	public float getWallDistance()
	{
		return WALL_DISTANCE;
	}

	public void SaveGameData()
	{
		GlobalControl.Instance.savedGameData = localGameData;
		if (PlayerPrefs.HasKey ("highScore"))
		{
			if (localGameData.highScore > PlayerPrefs.GetInt ("highScore"))
			{
				PlayerPrefs.SetInt ("highScore", localGameData.highScore);
				PlayerPrefs.Save ();
			}
		}
		else
		{
			PlayerPrefs.SetInt ("highScore",localGameData.highScore);
			PlayerPrefs.Save ();
		}
	}

	public void LoadGameData()
	{
		GlobalControl.Instance.savedGameData.highScore = PlayerPrefs.GetInt ("highScore");
		localGameData = GlobalControl.Instance.savedGameData;
	}

	public void ShowAd()
	{
		AdController.ShowRewardedAd ();
		loseScreen.transform.GetChild (3).gameObject.SetActive (false);
	}
		
	public void SecondChance()
	{
		if (gameOver)
		{
			gameOver = false;
			Vector3 newPosition = player.transform.transform.position;
			newPosition.x = 0;
			player.transform.position = newPosition;

			loseScreen.SetActive (false);
			gameScreen.transform.GetChild (3).gameObject.SetActive (true);

			player.SecondChance ();

			player.getCrashedWall ().GetComponentInParent<Obstacle> ().MoveForward ();

			foreach (GameObject obs in walls)
			{
				obs.GetComponent<Obstacle>().SecondChance ();
			}
			
			gameScreen.transform.GetChild (2).gameObject.SetActive (true);
		}
	}

	public int getScore()
	{
		return score;
	}

	public float getBackgroundHeight()
	{
		return BACKGROUND_HEIGHT;
	}

	void ShowMessage ()
	{
		if ( messageTexts.Length > messageNo )
		{
			MotivationMessage = messageTexts[ messageNo ];
			messages.SetActive (true);
			GameObject message = messages.transform.GetChild (0).gameObject;
			message.GetComponent<Text> ().text = MotivationMessage;
			shown = true;
			messageNo++;
			Invoke ("DisableMessage", MESSAGE_DURATION);
		}
	}

	void DisableMessage()
	{
		messages.SetActive (false);
		shown = false;
	}
}
