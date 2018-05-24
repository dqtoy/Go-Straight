using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	private const float MIN_GAP_SIZE = 5.85f;
	private const float MAX_GAP_SIZE = 7.2f;
	private const int TOTAL_POS_CHANGE = 8;
	private const int RANDOM_SCALE = 100;
	private const int THRESHOLD_TILT = 27;
	private const int THRESHOLD_LOW = 10;
	private const int THRESHOLD_MID = 33;
	private const int THRESHOLD_HIGH = 50;
	private const int TILT_ANGLE = 30;
	private const int TILT_START = 10;

	private GameObject leftWall;
	private GameObject rightWall;
	private Player player;

	private bool gameOver;
	private bool paused;
	private bool start;

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<Player> ();

		gameOver = false;
		start = false;

		//assigns the walls for calculations
		rightWall = gameObject.transform.GetChild (0).gameObject;
		leftWall =  gameObject.transform.GetChild (1).gameObject;


		Setup ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GlobalControl.Instance.Showed)
		{
			//if the obstacle is behind enough the player it will move forward
			if (player.transform.position.y - transform.position.y > GameController.instance.getDeletionDistance())
			{
				MoveForward();
			}

			if (gameOver || paused)
			{
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			else
			{
				if (start)
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(0, -player.getMoveSpeed());
					GetComponent<Rigidbody2D>().velocity = new Vector2(0, -player.getMoveSpeed());
				}
			}

			if (Input.GetMouseButtonDown(0) && !start && player.Started())
			{
				StartMovement();
			}
		}
	}

	//sets up the obstacle
	void Setup()
	{
		//generates a random Gap size and a random scale to gap the 
		//walls in a semi random fashion
		float randomNumber = Random.Range( 0, RANDOM_SCALE );
		float randomGapSize;
		if (randomNumber < THRESHOLD_LOW)
		{
			randomGapSize = MAX_GAP_SIZE;
		}
		else if (randomNumber < THRESHOLD_MID)
		{
			randomGapSize = Random.Range (MIN_GAP_SIZE, MAX_GAP_SIZE);
		}
		else if (randomNumber < THRESHOLD_HIGH)
		{
			randomGapSize = MIN_GAP_SIZE +1;
		}
		else
		{
			randomGapSize = MIN_GAP_SIZE;
		}
		setGap ( randomGapSize );

		//changes the position of the obstacle
		float maxPosChange = TOTAL_POS_CHANGE - randomGapSize;
		float obstaclePositionX = Random.Range ( -maxPosChange, maxPosChange );

		Vector3 temp = transform.position;
		temp.x = obstaclePositionX;
		transform.position = temp;

		//randomly tilts some platforms
		randomNumber = Random.Range(0, RANDOM_SCALE );

		if ( GameController.instance.getScore() > TILT_START)
		{
			if (randomNumber < THRESHOLD_TILT )
			{
				Tilt ();
			}
		}
	}

	//sets up the gap between walls
	void setGap( float size )
	{
		Vector3 temp = rightWall.transform.position;
		temp.x = size;
		rightWall.transform.position = temp;

		temp = leftWall.transform.position;
		temp.x = -size;
		leftWall.transform.position = temp;
	}

	//moves the obstacle forward
	public void MoveForward()
	{
		GameController.instance.walls.Remove (gameObject);
		Vector3 temp = transform.position;
		temp.x = 0;
		temp.y = GameController.instance.walls [GameController.instance.walls.Count - 1].transform.position.y;
		temp.y += GameController.instance.getWallDistance();
		transform.position = temp;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		GameController.instance.walls.Add(gameObject);

		Setup ();
	}

	void Tilt()
	{
		int pos = Random.Range (0, 2);
		if (pos == 1)
		{
			transform.rotation = Quaternion.Euler (0, 0, -TILT_ANGLE);
		}
		else
		{
			transform.rotation = Quaternion.Euler (0, 0, TILT_ANGLE);
		}
	}
		
	void StartMovement()
	{
		start = true;
	}

	public bool Started()
	{
		return start;
	}

	public void SecondChance()
	{
		gameOver = false;
		start = false;
	}

	public void OnPauseGame()
	{
		paused = true;
	}

	public void OnResumeGame()
	{
		paused = false;
	}

	public void EndGame()
	{
		gameOver = true;
	}
}
