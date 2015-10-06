using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
	
	public int health;
	public Rigidbody2D boss;
	public Vector2 bossSpeed;
	public Vector2 screenSize;

	//keepBossIn forces boss to stay within boundaries, disabled for certain attacks
	//bossInvinc will mean the boss is not damaged by player attacks
	//bossSemiInvinc will allow boss to react to enemy attacks, but not necessarily take damage 

	public bool keepBossIn = false, bossInvinc = true, bossSemiInvinc = true;
	public bool bossIdle = false;

	public float randVal;

	public static bool enemyWaveActive = false;

	public Animator bossAnimation;

	public SpriteRenderer bossSprite;

	private Game_Over_Script gameOver;


	// Use this for initialization
	void Start ()
	{
		screenSize = Player_Control.screenSize;	
		gameOver = GameObject.Find ("Canvas").GetComponent<Game_Over_Script> ();

	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		if (keepBossIn) {
			boundaryCheck ();
		}
	}

	void boundaryCheck ()
	{
		//Keep within boundaries
		if (boss.position.y > screenSize.y) {
			boss.velocity = new Vector2 (boss.velocity.x, -boss.velocity.y);
			boss.position = new Vector2 (boss.position.x, screenSize.y);
		}
		if (boss.position.y < -screenSize.y) {
			boss.velocity = new Vector2 (boss.velocity.x, -boss.velocity.y);
			boss.position = new Vector2 (boss.position.x, -screenSize.y);
			
		}
		if (boss.position.x > screenSize.x) {
			boss.velocity = new Vector2 (-boss.velocity.x, boss.velocity.y);
			boss.position = new Vector2 (screenSize.x, boss.position.y);
		}
		if (boss.position.x < -screenSize.x) {
			boss.velocity = new Vector2 (-boss.velocity.x, boss.velocity.y);
			boss.position = new Vector2 (-screenSize.x, boss.position.y);
		}
	}


	protected void decreaseBossHealth ()
	{
		if (!bossSemiInvinc) {
			health--;
			StartCoroutine (flashBossRed ());
			checkBossAlive ();
		}
	}


	protected void checkBossAlive ()
	{
		if (health <= 0) {			
			//die when health hits zero
			gameOver.displayYouWin ();
			Destroy (gameObject);
		}
	}

	protected IEnumerator moveToCentre ()
	{
		Vector2 direction;

		//stop
		boss.velocity = Vector2.zero;
		yield return new WaitForSeconds (1f);

		//determine unit vector direction
		direction = -boss.position.normalized;
		boss.velocity = direction * bossSpeed.magnitude; 

		//move in that direction until you reach centre
		while (boss.position.x * boss.velocity.x < 0 || boss.position.y * boss.velocity.y <0) {
			yield return new WaitForEndOfFrame ();
		}

		//stop
		boss.velocity = Vector2.zero;
		boss.position = Vector2.zero;

	}

	IEnumerator flashBossRed ()
	{
		bossSprite.material.color = new Vector4 (1f, 0.5f, 0.5f, 1f);
		yield return new WaitForSeconds (0.2f);
		bossSprite.material.color = new Vector4 (1f, 1f, 1f, 1f);

	}

}
