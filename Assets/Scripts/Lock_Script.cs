using UnityEngine;
using System.Collections;

public class Lock_Script : MonoBehaviour
{
	
	private Transform lockBar;
	public static Lock_Script lockScript;

	// Use this for initialization
	void Start ()
	{
		lockBar = GetComponent<Transform> ();
		lockScript = GetComponent<Lock_Script> ();
	}

	public void updateLockBar ()
	{
		//scale lock bar's width proportional to number of keys collected
		lockBar.localScale = new Vector2 ((float)Player_Control.keyCount / Player_Control.maxKeys, 1);
		if (lockBar.localScale.x == 1f) {
			Spawn_Control.spawner.spawnBoss ();
		}
	}
}
