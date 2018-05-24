using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	private const float WATER_SPEED = 2.5f;

	private Player player;

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<Player>();

		Vector3 temp = transform.position;
		temp.x = 0;
		temp.z = 25;
		transform.position = temp;

		GetComponent<Rigidbody2D> ().velocity = Vector2.down * WATER_SPEED;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//transform.position =  Vector3.MoveTowards(transform.position, transform.position * 2, 1);
		if ( player.transform.position.y - transform.position.y > GameController.instance.getBackgroundHeight() )
		{
			MoveForward ();
		}
	}

	public void MoveForward()
	{
		GameController.instance.backgrounds.Remove (gameObject);
		Vector3 temp = transform.position;
		temp.x = 0;
		temp.z = 25;
		temp.y = GameController.instance.backgrounds[GameController.instance.backgrounds.Count - 1].transform.position.y;
		temp.y += GameController.instance.getBackgroundHeight();
		transform.position = temp;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		GameController.instance.backgrounds.Add(gameObject);
		
		gameObject.transform.GetChild (0).GetComponent<Fishes>().ShuffleFishes();
	}
}
