using UnityEngine;
using System.Collections;

public class Fireball_Script : MonoBehaviour
{

	private Rigidbody2D fireball;
	private Vector2 fireballSpeed;

	// Use this for initialization
	void Start ()
	{
		fireball = GetComponent<Rigidbody2D> ();
		fireballSpeed = new Vector2 (0f, -6f);
		fireball.velocity = fireballSpeed;
	}
	
	void OnBecameInvisible ()
	{
		Destroy (gameObject);
	}
}
