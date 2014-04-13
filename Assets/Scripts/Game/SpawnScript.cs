using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	//Spawn Data
	public float Wave = 1;
	public float SpawnRate = 10;
	public float TimeUntilNextWave;
	public float EnemiesRemaining = 10;

	//Powerup Data
	public PowerupScript Powerup;
	public int MaxNumberOfPowerups = 100;
	public int NumberOfPowerups = 0;
	public float SpawnRateMin = .1f;
	public float SpawnRateMax = .2f;
	private float timeUntilNextSpawn;

	// Use this for initialization
	void Start () 
	{
		TimeUntilNextWave = SpawnRate;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Handle powerup spawning
		SpawnPowerups();
	}

	private void SpawnPowerups()
	{
		//if we have room for more powerups on the map
		if (NumberOfPowerups < MaxNumberOfPowerups)
		{
			//if we are ready to spawn the next one
			if (timeUntilNextSpawn <= 0)
			{
				//spawn our powerup
				ObjectFactory.CreatePowerup(PowerupType.Stamina, 3, 0, 100, MapInfo.GetRandomPointOnMap());

				//pick a random amount of time before spawning the next one
				timeUntilNextSpawn = Random.Range(SpawnRateMin, SpawnRateMax);
			}
			else
			{
				//we are not ready to spawn another powerup
				//so we need to decrement the timer
				timeUntilNextSpawn -= Time.deltaTime;
			}



		}
	}
}
