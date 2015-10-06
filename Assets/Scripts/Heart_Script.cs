using UnityEngine;
using System.Collections;

public class Heart_Script : MonoBehaviour
{

	private Rigidbody2D heart;
	private float speed;

	// Use this for initialization
	void Start ()
	{
		heart = GetComponent<Rigidbody2D> ();
		speed = Random.Range (1.0f, 1.5f);
		heart.velocity = new Vector2 (0, -speed);
	}

	// Update is called once per frame
	void Update ()
	{
		if (gameObject.transform.position.y < -10f) {
			Destroy (gameObject);
			
		}
	}
}
