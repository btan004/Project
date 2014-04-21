using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	public enum EnemyTypes {Debugging, Chaser, Sniper};

	//Debugging spawning option
	private InputHandler inputHandler;

	public WaveSystem waveSystem;

	//Spawn Data
	public float Wave = 1;
	public float SpawnRate = 10;
	public float WaveSpawnTime = 10;		//in seconds
	public float TimeUntilNextWave = 10;
	public int EnemiesRemaining = 0;

	//Minimum range that enemies must spawn away from player
	//E.g, if player.x = 20 and range is 5, enemy must spawn at x position > 25 or < 15
	public float minimumSpawnRange = 6;

	//Powerup Data
	public int MaxNumberOfPowerups = 100;
	public int NumberOfPowerups = 0;
	public float PowerupSpawnRateMin = 2f;
	public float PowerupSpawnRateMax = 5f;
	private float timeUntilNextPowerupSpawn;

	//Trap data
	public int MaxNumberOfTraps = 100;
	public int NumberOfTraps = 0;
	public float TrapSpawnRateMin = 2f;
	public float TrapSpawnRateMax = 5f;
	private float timeUntilNextTrapSpawn;

	// Use this for initialization
	void Start () 
	{
		inputHandler = new InputHandler ();
		waveSystem = new WaveSystem (this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		inputHandler.Update ();

		//Handle powerup spawning
		SpawnPowerups();

		//Handle trap spawning
		SpawnTraps();

		//wave system
		waveSystem.update ();

		//SpawnEnemyDebug ();

		//SpawnWave ();
	}

	private void SpawnPowerups()
	{
		//if we have room for more powerups on the map
		if (NumberOfPowerups < MaxNumberOfPowerups)
		{
			//if we are ready to spawn the next one
			if (timeUntilNextPowerupSpawn <= 0)
			{
				//decide which powerup to spawn
				float probability = Random.value;
				if (probability < 0.25)
				{
					if (probability < 0.125) ObjectFactory.CreateHealthPowerup();
					else ObjectFactory.CreateHealthRegenPowerup();
				}
				else if (probability < 0.50)
				{
					if (probability < 0.375) ObjectFactory.CreateStaminaPowerup();
					else ObjectFactory.CreateStaminaRegenPowerup();
				}
				else if (probability < 0.75)
				{
					ObjectFactory.CreateMovementSpeedPowerup();
				}
				else
				{
					ObjectFactory.CreateExperiencePowerup();
				}
				
				//pick a random amount of time before spawning the next one
				timeUntilNextPowerupSpawn = Random.Range(PowerupSpawnRateMin, PowerupSpawnRateMax);
			}
			else
			{
				//we are not ready to spawn another powerup
				//so we need to decrement the timer
				timeUntilNextPowerupSpawn -= Time.deltaTime;
			}
		}
	}

	private void SpawnTraps()
	{
		//if we are ready to spawn the next trap
		if (timeUntilNextTrapSpawn <= 0)
		{
			//spawn our trap
			TrapType type;
			float probability = Random.value;
			if (probability < .5) type = TrapType.Landmine;
			else type = TrapType.SlimeTrap;

			switch(type)
			{
				case (TrapType.SlimeTrap):
					ObjectFactory.CreateSlimeTrap(Random.Range(2, 5), MapInfo.GetRandomPointOnMap());
					break;
				case (TrapType.Landmine):
					ObjectFactory.CreateLandmine(Random.Range(10, 30), MapInfo.GetRandomPointOnMap());
					break;
				default:
					break;
			}

			//pick a random amount of time before spawning the next trap
			timeUntilNextTrapSpawn = Random.Range(TrapSpawnRateMin, TrapSpawnRateMax);
		}
		else
		{
			//we are not ready to spawn another trap
			//so we decrement the timer
			timeUntilNextTrapSpawn -= Time.deltaTime;
		}
	}

	public void SpawnEnemy( int count, EnemyTypes type )
	{
		GameObject player = GameObject.Find ("Player");
		Vector3 playerPos = player.transform.position;

		for( int i = 0; i < count; ++i )
		{
			Vector3 spawnPos = MapInfo.GetRandomPointOnMap ();

			while( Mathf.Abs(playerPos.x - spawnPos.x) <= minimumSpawnRange || Mathf.Abs(playerPos.z - spawnPos.z) <= minimumSpawnRange )
			{
				spawnPos = MapInfo.GetRandomPointOnMap();
			}
			switch(type)
			{
				case (EnemyTypes.Debugging):
					ObjectFactory.CreateDebugEnemy(spawnPos);
					break;
				case (EnemyTypes.Chaser):
					ObjectFactory.CreateEnemyChaser(spawnPos);
					break;
				case (EnemyTypes.Sniper):
					ObjectFactory.CreateEnemySniper(spawnPos);
					break;
			}
			++EnemiesRemaining;
		}
	}

	private void SpawnEnemyDebug()
	{
		//If the spawn button is pressed
		if (inputHandler.WantToSpawnEnemy)
		{
			//Function to spawn the enemy
			SpawnEnemy(1, EnemyTypes.Chaser);
		}
	}

	private void SpawnWave()
	{
		if (EnemiesRemaining == 0)
		{
			TimeUntilNextWave -= Time.deltaTime;

			if (TimeUntilNextWave <= 0)
			{
				SpawnEnemy(3, EnemyTypes.Sniper);
				SpawnEnemy(9, EnemyTypes.Chaser);
				TimeUntilNextWave = 10;
				++Wave;
			}
		}
	}
}
