using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game_Over_Script : MonoBehaviour
{
	public Text gameOver, youWin, controls;

	private Color white = new Vector4 (1, 1, 1, 1), transparent = new Vector4 (1, 1, 1, 0);

	private KeyCode R = KeyCode.R;

	// Update is called once per frame
	void Start ()
	{
		StartCoroutine (hideControls ());
	}

	void Update ()
	{
		//"Game Over" labels are active
		if ((gameOver.color == white || youWin.color == white) && Input.GetKey (R)) {

			Spawn_Control.spawner.killEnemies ();
			StartCoroutine (Player_Control.playerControl.resetPlayer ());
			StartCoroutine (Spawn_Control.spawner.spawnEnemy ());
				
			youWin.color = transparent;
			gameOver.color = transparent;

		}
	}


	public void displayGameOver ()
	{
		gameOver.color = white;

	}

	public void displayYouWin ()
	{
		youWin.color = white;

	}

	IEnumerator hideControls ()
	{
		yield return new WaitForSeconds (5);
		controls.color = transparent;
	}
}
