using UnityEngine;
using System.Collections;

public class Key_Script : MonoBehaviour
{

	private Rigidbody2D key;
	private float speed;

	// Use this for initialization
	void Start ()
	{
		key = GetComponent<Rigidbody2D> ();
		speed = Random.Range (1.5f, 2.0f);
		key.velocity = new Vector2 (0, -speed);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gameObject.transform.position.y < -10f) {
			Destroy (gameObject);			
		}
	}
}
