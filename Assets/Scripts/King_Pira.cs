using UnityEngine;
using System.Collections;

public class King_Pira : Boss
{
	private int bubbleCount, maxBubbles = 50;
	private GameObject fireball;
	private Vector2 offset = new Vector2 (0, -1.5f);
	private Animator kingPiraAnimation;

	// Use this for initialization
	void Awake ()
	{
		health = 120;
		bossSpeed = new Vector2 (3f, -1f);

		boss = GetComponent<Rigidbody2D> ();
		bossAnimation = GetComponent<Animator> ();
		bossSprite = GetComponent<SpriteRenderer> ();
		fireball = Resources.Load ("Prefabs/fireball") as GameObject;
		StartCoroutine (enterBoss ());

	}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update ();
		if (bossIdle) {
			randVal = Random.value; 
			if (randVal < 0.3f) {
				StartCoroutine (tackle ());
			} else if (randVal < 0.5f) {
				StartCoroutine (enemyWave ());
			} else {
				StartCoroutine (shootFire ());
			}
		} else if (!bossInvinc && !bossInvinc) {
			//boss is no longer stunned, speeds up
			//health is decreased by 1 to avoid infinite loop
			if (health == 100 || health == 60) {
				health--;
				bossSpeed *= 1.25f;
				boss.velocity = bossSpeed;
				bossAnimation.Play ("king_pira_idle");
				bossSemiInvinc = true;
				bossInvinc = true;

				StartCoroutine (cooldown ());
			} 
		}
	}

	IEnumerator enterBoss ()
	{
		boss.velocity = new Vector2 (0f, -0.7f);
		yield return new WaitForSeconds (6f);
		boss.velocity = Vector2.zero;
		yield return new WaitForSeconds (1f);
		boss.velocity = bossSpeed;
		keepBossIn = true;
		bossIdle = true;

	}

	IEnumerator tackle ()
	{
		float startTime;

		//boss is no longer idle
		bossIdle = false;
		yield return StartCoroutine (moveToCentre ());

		//move boss back
		boss.velocity = new Vector2 (0, 1f);
		yield return new WaitForSeconds (3f);
		boss.velocity = Vector2.zero;
		yield return new WaitForSeconds (0.5f);

		//follow player for 3 seconds
		startTime = Time.time;
		while (Time.time < startTime+3f) {
			yield return new WaitForEndOfFrame ();
			if (boss.position.x > Player_Control.player.position.x + 0.5f) {
				boss.velocity = new Vector2 (-Player_Control.playerSpeed, 0);
			} else if (boss.position.x < Player_Control.player.position.x - 0.5f) {
				boss.velocity = new Vector2 (Player_Control.playerSpeed, 0);
			} else {
				boss.velocity = Vector2.zero;
			}
		}

		//charge forward
		keepBossIn = false;
		boss.velocity = new Vector2 (0f, -bossSpeed.magnitude - 1f);

		//warp boss back
		while (boss.position.y > -7.8f) {
			yield return new WaitForEndOfFrame ();
		}
		boss.position = new Vector2 (0f, 7.4f);

		//return to position and reset speed
		boss.velocity = new Vector2 (0f, -0.7f);
		yield return new WaitForSeconds (6f);
		boss.velocity = Vector2.zero;
		yield return new WaitForSeconds (1f);
		boss.velocity = bossSpeed;
		keepBossIn = true;

		StartCoroutine (cooldown ());

	}

	IEnumerator enemyWave ()
	{
		//boss is no longer idle
		bossIdle = false;
		yield return StartCoroutine (moveToCentre ());

		//move boss back
		boss.velocity = new Vector2 (0, 1f);
		yield return new WaitForSeconds (3f);
		boss.velocity = Vector2.zero;
		yield return new WaitForSeconds (0.5f);

		enemyWaveActive = true;
		
		//spawn enemies for 7 seconds
		StartCoroutine (Spawn_Control.spawner.spawnEnemy ());
		yield return new WaitForSeconds (7);
		
		//stop spawning and start moving boss
		enemyWaveActive = false;
		boss.velocity = bossSpeed;
		
		StartCoroutine (cooldown ());

	}

	IEnumerator shootFire ()
	{
		//boss becomes vulnerable in this state
		bossIdle = false;
		bossInvinc = false;
		bossSemiInvinc = true;

		bossAnimation.Play ("king_pira_open");
		yield return new WaitForSeconds (1);

		bubbleCount = 0;
		while (bubbleCount < maxBubbles && !bossInvinc && bossSemiInvinc) {
			Instantiate (fireball, boss.position + offset, Quaternion.identity);
			yield return new WaitForSeconds (Random.Range (1f, 2f));
		}
	}

	IEnumerator cooldown ()
	{
		yield return new WaitForSeconds (Random.Range (3f, 5f));
		bossIdle = true;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!bossInvinc && bossSemiInvinc && other.tag == "Bullet") {
			//bubbles build up in mouth
			bubbleCount++;
			Destroy (other.gameObject);

			//play different animations depending on how "full" mouth is
			if (bubbleCount == 10) {
				bossAnimation.Play ("king_pira_open_A");
			} else if (bubbleCount == 25) {
				bossAnimation.Play ("king_pira_open_B");
			} else if (bubbleCount == 40) {
				bossAnimation.Play ("king_pira_open_C");
			} else if (bubbleCount == maxBubbles) {
				bossAnimation.Play ("king_pira_stun");
				boss.velocity /= 1.25f;
				bossSemiInvinc = false;
				bubbleCount = 0;
			}
		}
	} 
}
