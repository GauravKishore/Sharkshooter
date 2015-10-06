using UnityEngine;
using System.Collections;

public class Pira : Enemy
{
	// Use this for initialization
	void Awake ()
	{
		enemy = GetComponent<Rigidbody2D> ();
		health = 2;
		speed = Random.Range (1.0f, 3.0f);
		spawnRate = 2.0f;

	}

}
