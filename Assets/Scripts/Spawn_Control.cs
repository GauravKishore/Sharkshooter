using UnityEngine;
using System.Collections; 
using System.Collections.Generic; 
public class Spawn_Control : MonoBehaviour
{ 
	private GameObject currEnemy, currBoss; 
	private float currSpawnRate; 
	private Vector2 screenSize = new Vector2 (4.5f, 5.4f);
	private GameObject pira;
	private GameObject kingPira; 
	public static Spawn_Control spawner;

	// Use this for initialization 
	void	Start ()
	{ 
		spawner = gameObject.GetComponent<Spawn_Control> (); 
		pira = Resources.Load ("Prefabs/pira") as GameObject; 
		kingPira = Resources.Load ("Prefabs/king_pira") as GameObject;
		currEnemy = pira; 
		currBoss = kingPira; 
		currSpawnRate = currEnemy.GetComponent<Enemy> ().spawnRate; 
		StartCoroutine (spawnEnemy ()); 
	} 
	public IEnumerator spawnEnemy ()
	{ 
		//spawn enemies within a random range 
		yield return new WaitForSeconds (0.1f); 
		while (Player_Control.playerAlive && Player_Control.keyCount != Player_Control.maxKeys || Boss.enemyWaveActive) { 
			Instantiate (currEnemy, new Vector2 (Random.Range (-screenSize.x, screenSize.x), screenSize.y + 1.0f), Quaternion.identity);
			yield return new WaitForSeconds (Random.Range (currSpawnRate * 0.5f, currSpawnRate)); 
		} 
		//yield break; 
	} 

	public void spawnBoss ()
	{ 
		Instantiate (currBoss); 
	} 

	public void killEnemies ()
	{ 
		//remove enemies, bullets, and drop items 
		GameObject[] enemyList = GameObject.FindGameObjectsWithTag ("Enemy"); 
		foreach (GameObject enemy in enemyList) { 
			Destroy (enemy); 
		} 

		GameObject[] bulletList = GameObject.FindGameObjectsWithTag ("Bullet");
		foreach (GameObject bullet in bulletList) { 
			Destroy (bullet); 
		} 

		GameObject[] heartList = GameObject.FindGameObjectsWithTag ("Heart");
		foreach (GameObject heart in heartList) {
			Destroy (heart); 
		}

		GameObject[] keyList = GameObject.FindGameObjectsWithTag ("Key"); 
		foreach (GameObject key in keyList) { 
			Destroy (key); 
		}

		GameObject[] bossList = GameObject.FindGameObjectsWithTag ("Boss");
		foreach (GameObject boss in bossList) { 
			Destroy (boss); 
		}

		GameObject[] eBulletList = GameObject.FindGameObjectsWithTag ("EBullet");
		foreach (GameObject eBullet in eBulletList) { 
			Destroy (eBullet); 
		}

	}
} 
