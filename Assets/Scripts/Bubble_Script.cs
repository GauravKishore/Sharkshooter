using UnityEngine;
using System.Collections;

public class Bubble_Script : MonoBehaviour
{	
	private Rigidbody2D bubble;
	private float shootSpeed;
	private Animator bubbleAnimation;

	// Use this for initialization
	void Start ()
	{
		bubble = GetComponent<Rigidbody2D> ();
		bubbleAnimation = GetComponent<Animator> ();

		shootSpeed = 7f;
		bubble.velocity = new Vector2 (0, shootSpeed);
		
	}
	
	void OnBecameInvisible ()
	{
		Destroy (gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Enemy" || other.tag == "Boss") {
			hitOther (other);
		}
	}

	void hitOther (Collider2D other)
	{
		//damage enemy and check if the shot killed
		if (other.tag == "Enemy") {
			other.gameObject.SendMessage ("decreaseEnemyHealth");
		} 


		if (other.tag == "Boss") {

			//bubble can't hit anything else
			GetComponent<Collider2D> ().enabled = false;

			if (other.GetComponent<Boss> ().bossInvinc) {
				bounceBubble (other);
			} else if (other.GetComponent<Boss> ().bossSemiInvinc) {
				GetComponent<Collider2D> ().enabled = true;
			} else {
				other.gameObject.SendMessage ("decreaseBossHealth");
				StartCoroutine (popBubble ());
			}
		} else {
			StartCoroutine (popBubble ());
		}

	}

	IEnumerator popBubble ()
	{
		bubble.velocity = Vector2.zero;
		bubbleAnimation.Play ("bubble_pop");
		yield return new WaitForSeconds (0.2f);
		Destroy (gameObject);
	}

	void bounceBubble (Collider2D other)
	{
		float angle;
		int sign = 1;

		//bubble bounces left if tot he left of boss, bounces right if to the righ of boss
		if (other.GetComponent<Rigidbody2D> ().position.x > bubble.position.x) { 
			sign = -1;
		} else {
			sign = 1;
		}

		//angle the buble will bounce at
		angle = Random.Range (Mathf.PI / 8, Mathf.PI * 2 / 5);
		bubble.velocity = new Vector2 (sign * Mathf.Cos (angle), Mathf.Sin (angle)) * shootSpeed;
	}
}

