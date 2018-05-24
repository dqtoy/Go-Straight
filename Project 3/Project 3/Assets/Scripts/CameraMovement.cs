using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	private Player player;

	// Use this for initialization
	void Start () 
	{
		//finds player and sets the camera tp -10 to render
		player = FindObjectOfType<Player>();
		transform.position = new Vector3(0,0, -10);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//follows the player
		transform.position = new Vector3(0, player.transform.position.y + 5, -10);
	}
}
