using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	//Spawn Data
	public float Wave = 1;
	public float SpawnRate = 10;
	public float TimeUntilNextWave;

	public float EnemiesRemaining = 10;

	// Use this for initialization
	void Start () {
		TimeUntilNextWave = SpawnRate;
	}
	
	// Update is called once per frame
	void Update () {
		//update our time until next wave
		TimeUntilNextWave -= Time.deltaTime;

		//check if its time to spawn yet
		if (TimeUntilNextWave < 0)
		{
			//spawn the next wave

			//set our time back to the spawn rate
			TimeUntilNextWave = SpawnRate;

			//update our wave count
			Wave++;
		}
	}
}
