using UnityEngine;
using System.Collections;

public class Player_Control : MonoBehaviour
{
	private KeyCode moveUp = KeyCode.UpArrow, 
		moveDown = KeyCode.DownArrow, 
		moveRight = KeyCode.RightArrow, 
		moveLeft = KeyCode.LeftArrow, 
		shootKey = KeyCode.Space;
	public static float playerSpeed;
	private float nextVuln, invincTime;
	public static int playerHP, maxHP;
	public GameObject bubble;
	public static Rigidbody2D player;
	private SpriteRenderer playerSprite;
	private Vector2 upVec, rightVec, upRightVec, downRightVec, offset;
	public static Vector2 screenSize; 
	private Game_Over_Script gameOver;
	public static bool playerAlive;
	public static Player_Control playerControl;
	private Animator playerAnimation;
	public static int keyCount, maxKeys;


	// Use this for initialization
	void Awake ()
	{
		playerControl = gameObject.GetComponent<Player_Control> ();

		player = GetComponent<Rigidbody2D> ();
		playerSprite = GetComponent<SpriteRenderer> ();
		playerAnimation = GetComponent<Animator> ();

		playerSpeed = 5.0f;
		playerHP = maxHP = 3;
		nextVuln = 0.0f;
		invincTime = 2.0f;
		playerAlive = true;

		upVec = new Vector2 (0, playerSpeed);
		rightVec = new Vector2 (playerSpeed, 0);
		upRightVec = new Vector2 (Mathf.Sqrt (2) / 2 * playerSpeed, Mathf.Sqrt (2) / 2 * playerSpeed);
		downRightVec = new Vector2 (Mathf.Sqrt (2) / 2 * playerSpeed, -Mathf.Sqrt (2) / 2 * playerSpeed);
		offset = new Vector2 (0, 0.8f);
		screenSize = new Vector2 (4.5f, 5.4f);

		gameOver = GameObject.Find ("Canvas").GetComponent<Game_Over_Script> ();

		keyCount = 0;
		maxKeys = 5;

	}
	
	// Update is called once per frame
	void Update ()
	{
		player.WakeUp ();

		if (playerAlive) {

			//Shoot bubble
			if (Input.GetKeyDown (shootKey)) {
				Instantiate (bubble, player.position + offset, Quaternion.identity);
			}
			movePlayer ();

			if (playerHP <= 0) {
				gameOver.displayGameOver ();
				killPlayer ();
			} 

		} else if (player.position.y < -7f) {
			//when player is killed and in free-fall, stop player movement
			player.isKinematic = true;
			player.position = new Vector2 (0f, -7f);
		}
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (playerAlive && (other.tag == "Enemy" || other.tag == "Boss" || other.tag == "EBullet") && Time.time > nextVuln) {
			//take touch damage
			playerHP--;
			Health_Control.healthControl.updateHealth ();

			//Flash player while invincible (do not flash on death)
			if (playerHP > 0) {
				nextVuln = Time.time + invincTime;
				StartCoroutine (flashPlayer ());
			}
			if (other.tag == "EBullet") {
				Destroy (other.gameObject);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (playerAlive) {
			if (other.tag == "Heart") {
				if (playerHP < maxHP) {
					//increase health if not maxed
					playerHP++;
					Health_Control.healthControl.updateHealth ();
				}
				Destroy (other.gameObject);
			} else if (other.tag == "Key") {
				if (keyCount < maxKeys) {
					//increase lock meter if not maxed
					keyCount++;
					Lock_Script.lockScript.updateLockBar ();
				}
				Destroy (other.gameObject);
			}
		}
	}

	void movePlayer ()
	{
		//Hardcode player movement
		if (Input.GetKey (moveUp) && Input.GetKey (moveRight)) {
			player.velocity = upRightVec;
		} else if (Input.GetKey (moveDown) && Input.GetKey (moveRight)) {
			player.velocity = downRightVec;
		} else if (Input.GetKey (moveDown) && Input.GetKey (moveLeft)) {
			player.velocity = -upRightVec;
		} else if (Input.GetKey (moveUp) && Input.GetKey (moveLeft)) {
			player.velocity = -downRightVec;
		} else if (Input.GetKey (moveUp)) {
			player.velocity = upVec;
		} else if (Input.GetKey (moveDown)) {
			player.velocity = -upVec;
		} else if (Input.GetKey (moveRight)) {
			player.velocity = rightVec;
		} else if (Input.GetKey (moveLeft)) {
			player.velocity = -rightVec;
		} else {
			player.velocity = Vector2.zero;
		}

		//Keep within boundaries
		if (player.position.y > screenSize.y) {
			player.velocity = new Vector2 (player.velocity.x, 0f);
			player.position = new Vector2 (player.position.x, screenSize.y);
		}
		if (player.position.y < -screenSize.y) {
			player.velocity = new Vector2 (player.velocity.x, 0f);
			player.position = new Vector2 (player.position.x, -screenSize.y);
		}
		if (player.position.x > screenSize.x) {
			player.velocity = new Vector2 (0f, player.velocity.y);
			player.position = new Vector2 (screenSize.x, player.position.y);
		}
		if (player.position.x < -screenSize.x) {
			player.velocity = new Vector2 (0f, player.velocity.y);
			player.position = new Vector2 (-screenSize.x, player.position.y);
		}

		//Remove "jerking" at edges
		if (player.position.y == screenSize.y) {
			moveUp = KeyCode.None;
		} else {
			moveUp = KeyCode.UpArrow;
		}

		if (player.position.y == -screenSize.y) {
			moveDown = KeyCode.None;
		} else {
			moveDown = KeyCode.DownArrow;
		}

		if (player.position.x == screenSize.x) {
			moveRight = KeyCode.None;
		} else {
			moveRight = KeyCode.RightArrow;
		}

		if (player.position.x == -screenSize.x) {
			moveLeft = KeyCode.None;
		} else {
			moveLeft = KeyCode.LeftArrow;
		}

	}

	IEnumerator flashPlayer ()
	{
		while (Time.time <= nextVuln) {
			playerSprite.enabled = false;
			yield return new WaitForSeconds (0.1f);
			playerSprite.enabled = true;
			yield return new WaitForSeconds (0.1f);
		}
	}

	void killPlayer ()
	{		
		player.GetComponent<Collider2D> ().isTrigger = false;
		playerAlive = false;
		playerAnimation.Play ("player_death");
		player.velocity = upVec * 1.5f;
		player.isKinematic = false;

	}

	public IEnumerator resetPlayer ()
	{
		yield return new WaitForSeconds (0.01f);
		player.isKinematic = true;
		player.velocity = Vector2.zero;
		player.position = new Vector2 (0f, -4.6f);
		playerAnimation.Play ("player_animation");
		playerHP = maxHP;
		keyCount = 0;
		Health_Control.healthControl.updateHealth ();
		Lock_Script.lockScript.updateLockBar ();
		playerAlive = true;
		player.GetComponent<Collider2D> ().isTrigger = true;
	}
}
