using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	
	public PlayerScript playerScript;
	public SpawnScript spawnScript;

	//Our GUI 
	void OnGUI()
	{
		//Display the players Lives
		GUI.Label(new Rect(10, 10, 100, 20), "Lives: " + playerScript.Lives);

		//Display the players Health
		GUI.Label(new Rect(10, 35, 100, 20), "Health: " + playerScript.Health);

		//Display the players Stamina
		GUI.Label (new Rect (10, 60, 100, 20), "Stamina: " + (int) playerScript.Stamina);

		//Display the current Wave
		GUI.Label(new Rect((Screen.width / 2) - 50, 10, 100, 20), "Current Wave: " + spawnScript.Wave);

		//Dispaly the time until the next wave
		GUI.Label(new Rect((Screen.width / 2) - 78, 35, 200, 20), "Time Until Next Wave: " + (int) spawnScript.TimeUntilNextWave);

		//Display the number of enemies remaining
		GUI.Label(new Rect((Screen.width / 2) - 50, 70, 100, 20), "Enemies Remaining: " + (int) spawnScript.EnemiesRemaining);

		//Display the players Score
		GUI.Label(new Rect(Screen.width - 110, 60, 100, 20), "Score: " + playerScript.Score);

		//debug stuff
		GUI.Label(new Rect(10, 100, 100, 20), "Player Position: (" + (int) playerScript.transform.position.x +
		          ", " + (int) playerScript.transform.position.y + ", " + (int) playerScript.transform.position.z + ")");
	}
}
