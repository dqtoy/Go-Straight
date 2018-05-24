using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Fishes : MonoBehaviour {

	private const float FISH_SPEED = 2.0f;

	private List<GameObject> fishes = new List<GameObject> ();

	private Random random = new Random();

	// Use this for initialization
	void Start () 
	{
		
		Vector3 temp = transform.position;
		temp.x = 0;
		temp.z = +15;
		transform.position = temp;

		GetComponent<Rigidbody2D> ().velocity = Vector2.down * FISH_SPEED;

		for ( int i = 0; i < 4; i++ )
		{
			fishes.Add(gameObject.transform.GetChild(i).gameObject);
		}
		
		ShuffleFishes();
	}
	
	// Update is called once per frame
	//void Update () 
	//{
	//	
	//}
	
	public void ShuffleFishes()
	{
		foreach ( GameObject obj in fishes  )
		{
			obj.transform.localPosition = new Vector2(  random.Next(-5, 5), random.Next(-12, 12) );
		}
	}
}
