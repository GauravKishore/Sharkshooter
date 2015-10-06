using UnityEngine; 
using System.Collections; 
public class BGScrolling : MonoBehaviour
{ 

	private float spriteHeight;
	private Vector2 scrollSpeed; 
	private Rigidbody2D background; 

	void Start ()
	{ 

		scrollSpeed = new Vector2 (0, -0.5f); 
		background = GetComponent<Rigidbody2D> (); 
		spriteHeight = GetComponent< SpriteRenderer> ().sprite.bounds.size.y; 

		//scroll background down 
		background.velocity = scrollSpeed; 
	}

	// Update is called once per frame 

	void Update ()
	{
		//loop background at end of image

		if (background.position.y + spriteHeight <= 5.4f) { 
			background.position = new Vector2 (0, -5.4f); 
		}
	}
} 
	 
