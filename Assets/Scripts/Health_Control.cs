using UnityEngine;
using System.Collections;

public class Health_Control : MonoBehaviour
{

	private Sprite[] healthSprites;
	private SpriteRenderer healthRenderer;
	public static Health_Control healthControl;

	// Use this for initialization
	void Start ()
	{
		healthControl = GetComponent<Health_Control> ();
		healthRenderer = GetComponent<SpriteRenderer> ();
		healthSprites = Resources.LoadAll<Sprite> ("Sprites/HP_Sprites");
	}

	public void updateHealth ()
	{
		//update health UI when HP is in allowed range
		if (Player_Control.playerHP <= 3 && Player_Control.playerHP >= 0) {
			healthRenderer.sprite = healthSprites [Player_Control.playerHP];
		}
	}



}
