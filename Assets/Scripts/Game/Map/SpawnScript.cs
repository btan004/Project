using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	public enum EnemyTypes {Debugging, Chaser, Bouncer, Charger, Sniper, Healer, Spawner, Boss};

	//Debugging spawning option
	private InputHandler inputHandler;

	public PlayerScript player;

	//enemy spawning wave system
	public WaveSystem waveSystem;

	//the portal
	public GameObject Portal;

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

	//spawn data
	public static bool SpawnWave;
	public float timeUntilSpawnWave = 5f;
	private float timeUntilSpawnWaveTimer;

	// Use this for initialization
	void Start () 
	{
		inputHandler = new InputHandler ();
		waveSystem = new WaveSystem (this, player);

		SpawnWave = false;
		timeUntilSpawnWaveTimer = timeUntilSpawnWave;
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
		if (SpawnWave)
		{
			timeUntilSpawnWaveTimer -= Time.deltaTime;
			if (timeUntilSpawnWaveTimer <= 0)
			{
				WaveSystem.ForceSpawnWave = true;
				SpawnWave = false;
				timeUntilSpawnWaveTimer = timeUntilSpawnWave;
			}
		}

		waveSystem.update ();

		//update the level portal
		if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Level && WaveSystem.WaveFinished)
			MapSystemScript.instance.GetCurrentLevel().GetComponentInChildren<PortalScript>().IsActive = WaveSystem.WaveFinished;
	}

	private void SpawnPowerups()
	{
		if (MapSystemScript.instance.PowerupsEnabled())
		{
			//if we have room for more powerups on the map
			if (NumberOfPowerups < MaxNumberOfPowerups)
			{
				//if we are ready to spawn the next one
				if (timeUntilNextPowerupSpawn <= 0)
				{
					//decide which powerup to spawn
					float probability = Random.value;
					if (probability < 0.4)
					{
						ObjectFactory.CreateHealthPowerup();
					}
					else if (probability < 0.8)
					{
						ObjectFactory.CreateStaminaPowerup();
					}
					else
					{
						ObjectFactory.CreateMovementSpeedPowerup();
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
	}

	private void SpawnTraps()
	{
		if (MapSystemScript.instance.TrapsEnabled())
		{
			//if we are ready to spawn the next trap
			if (timeUntilNextTrapSpawn <= 0)
			{
				//spawn our trap
				TrapType type;
				float probability = Random.value;
				if (probability < .5) type = TrapType.Landmine;
				else type = TrapType.SlimeTrap;

				switch (type)
				{
					case (TrapType.SlimeTrap):
						ObjectFactory.CreateSlimeTrap(1, MapInfo.GetRandomPointOnMap());
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
	}

	public void SpawnEnemy( int count, EnemyTypes type, EnemyUpgrade upgrade)
	{
		if (MapSystemScript.instance.EnemiesEnabled())
		{
			GameObject player = GameObject.Find("Player");
			Vector3 playerPos = player.transform.position;

			for (int i = 0; i < count; ++i)
			{
				Vector3 spawnPos = MapInfo.GetRandomPointOnMap();

				while (Mathf.Abs(playerPos.x - spawnPos.x) <= minimumSpawnRange || Mathf.Abs(playerPos.z - spawnPos.z) <= minimumSpawnRange)
				{
					spawnPos = MapInfo.GetRandomPointOnMap();
				}
				switch (type)
				{
					case (EnemyTypes.Debugging):
						ObjectFactory.CreateDebugEnemy(spawnPos);
						break;
					case (EnemyTypes.Chaser):
						ObjectFactory.CreateEnemyChaser(spawnPos, upgrade);
						break;
					case (EnemyTypes.Bouncer):
						ObjectFactory.CreateEnemyBouncer(spawnPos, upgrade);
						break;
					case (EnemyTypes.Charger):
						ObjectFactory.CreateEnemyCharger(spawnPos, upgrade);
						break;
					case (EnemyTypes.Sniper):
						ObjectFactory.CreateEnemySniper(spawnPos, upgrade);
						break;
					case (EnemyTypes.Healer):
						ObjectFactory.CreateEnemyHealer(spawnPos, upgrade);
						break;
					case (EnemyTypes.Spawner):
						ObjectFactory.CreateEnemySpawner(spawnPos, upgrade);
						break;
					case (EnemyTypes.Boss):
						ObjectFactory.CreateEnemySniperBoss(spawnPos, upgrade);
						break;
					default:
						break;
				}

			}
		}
	}

	private void SpawnEnemyDebug()
	{
		//If the spawn button is pressed
		if (inputHandler.WantToSpawnEnemy)
		{
			//Function to spawn the enemy
			SpawnEnemy(1, EnemyTypes.Chaser, null);
		}
	}

}
