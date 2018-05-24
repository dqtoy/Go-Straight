using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour {

	private const float MOVE_SPEED = 3.5f;
	private const float TILT_ANGLE = 45f;
	private const float X_LIMIT = 5f;
	private const float ROTATION_SPEED = 20f;

	public AudioClip ScoreScound;

	private Rigidbody2D rb;

	private bool dir;
	private bool gameOver;
	private bool paused;
	private bool start;
	private GameObject crashedWall;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
		gameOver = false;
		start = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GlobalControl.Instance.Showed)
		{

			if (gameOver || paused)
			{
				rb.velocity = Vector3.zero;
			}
			else
			{
				if (start)
				{
					UpdateMovement();
				}
			}

			if (Input.GetMouseButtonDown(0) && !start)
			{
				StartMovement();
			}
		}
	}

	public void OnPauseGame()
	{
		paused = true;
	}

	public void OnResumeGame()
	{
		paused = false;
	}

	public void Death()
	{
		gameOver = true;
		start = false;
		GameController.instance.EndGame();
	}

	void UpdateMovement()
	{
		if ( dir )
		{
			rb.velocity = new Vector2( MOVE_SPEED, 0);// MOVE_SPEED );
			transform.rotation = Quaternion.RotateTowards( transform.rotation, Quaternion.Euler(0, 0, -TILT_ANGLE), ROTATION_SPEED);
		}
		else
		{
			rb.velocity = new Vector2 (-MOVE_SPEED, 0);// MOVE_SPEED );
			transform.rotation = Quaternion.RotateTowards( transform.rotation, Quaternion.Euler(0, 0, TILT_ANGLE), ROTATION_SPEED);
		}

		if ( transform.position.x > X_LIMIT || transform.position.x < -X_LIMIT )
		{
			Death();
		}
		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

		if(Input.GetMouseButtonDown(0))
		{
			if(Input.mousePosition.y < Screen.height/2)
			{
				dir = !dir; //lower part of the screen
				//Rotate ();
			}
		}

		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		int TouchID = -1;

		if(Input.touchCount > 0)
		{
			foreach(Touch touch in Input.touches)
			{
				if(touch.phase == TouchPhase.Began)
				{
					TouchID = touch.fingerId;
				}
				if(touch.phase != TouchPhase.Ended && touch.fingerId == TouchID)
				{
					if(touch.position.y < Screen.height/2)
					{
						dir = !dir; //lower part of the screen
					}
				}
				if(touch.phase == TouchPhase.Ended)
				{
					TouchID = -1;
				}
			}
		}

		#endif 
	}

	void OnCollisionEnter2D(Collision2D other) 
	{
		if (other.gameObject.CompareTag ("Wall"))
		{
			crashedWall = other.gameObject;
			Death();
		}
	}
		
	void OnTriggerEnter2D(Collider2D col)
	{
		if ( col.gameObject.CompareTag("TriggerLine") )
		{
			GameController.instance.incrementScore ();
			//SoundManager.Instance.RandomizeSfx(ScoreScound);
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

	public GameObject getCrashedWall()
	{
		return crashedWall;
	}

	public float getMoveSpeed()
	{
		return MOVE_SPEED;
	}
}
