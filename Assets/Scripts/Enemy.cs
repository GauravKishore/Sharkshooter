using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	public int health;
	public float speed, spawnRate;
	public Rigidbody2D enemy;
	private GameObject heart, key;
	private float randVal;

	// Use this for initialization
	void Start ()
	{
		//initialize drops
		heart = Resources.Load ("Prefabs/heart") as GameObject;
		key = Resources.Load ("Prefabs/key") as GameObject;

		enemy.velocity = new Vector2 (0, -speed);

	}

	// Update is called once per frame
	void Update ()
	{
		if (gameObject.transform.position.y < -10f) {
			Destroy (gameObject);
		}
	}


	protected void decreaseEnemyHealth ()
	{
		health--;
		checkEnemyAlive ();
	}


	protected void checkEnemyAlive ()
	{
		if (health <= 0) {

			//die when health hits zero
			Destroy (gameObject);

			//determine if item will drop
			randVal = Random.value;

			//only drop heart when player not at full HP
			if (Player_Control.playerHP < Player_Control.maxHP && randVal < 0.5f) {
				Instantiate (heart, gameObject.transform.position, Quaternion.identity);
			} 

			//only drop key when lock meter is not full
			else if (Player_Control.keyCount < Player_Control.maxKeys && randVal > 0.7f) {
				Instantiate (key, gameObject.transform.position, Quaternion.identity);
			}
		}
	}
	

}
