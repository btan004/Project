using UnityEngine;
using System.Collections;

public class ObjectFactory : MonoBehaviour {

	protected static ObjectFactory instance;

	//Storage
	public static GameObject EnemyContainer;
	public static GameObject PowerupContainer;
	public static GameObject TrapContainer;

	/* Powerups */
	public GameObject PowerupPrefab;

	/* Traps */
	public GameObject SlimeTrapPrefab;
	public GameObject LandminePrefab;

	/* Enemies */

	//A testing enemy to debug spawning
	public GameObject DebugEnemyPrefab;

	//Enemies to be included in real game
	public GameObject EnemyChaserPrefab;
	public GameObject EnemySniperPrefab;
	public GameObject EnemyHealerPrefab;
	public GameObject EnemySpawnerPrefab;
	public GameObject EnemyBouncerPrefab;
	public GameObject EnemyChargerPrefab;

	public GameObject EnemySniperBossPrefab;
	public GameObject EnemyChaserBossPrefab;
	public GameObject EnemySpawnerBossPrefab;

	// Use this for initialization
	void Start () {
		instance = this;

		EnemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");
		PowerupContainer = GameObject.FindGameObjectWithTag("PowerupContainer");
		TrapContainer = GameObject.FindGameObjectWithTag("TrapContainer");
	}
	
	public static PowerupScript CreatePowerup(PowerupType type, float amount, float duration, float lifetime, Vector3 position)
	{
		//Instanciate our powerup
		GameObject powerupObject = (Object.Instantiate(instance.PowerupPrefab, position, Quaternion.identity) as GameObject);
		PowerupScript powerup = powerupObject.GetComponent<PowerupScript>();

		//Set the powerup's stats
		powerup.Type = type;
		powerup.Amount = amount;
		powerup.Duration = duration;
		powerup.Lifetime = lifetime;

		//add the powerup to the container
		powerupObject.transform.parent = PowerupContainer.transform;

		//return the powerup
		return powerup;
	}

	public static PowerupScript CreateHealthPowerup()
	{
		return CreatePowerup(
			PowerupType.Health,
			Random.Range(PowerupInfo.HealthMinAmount, PowerupInfo.HealthMaxAmount),
			0,
			PowerupInfo.Lifetime,
			MapInfo.GetRandomPointOnMap()
			);
	}

	public static PowerupScript CreateHealthRegenPowerup()
	{
		return CreatePowerup(
			PowerupType.HealthRegen,
			Random.Range(PowerupInfo.HealthRegenMinAmount, PowerupInfo.HealthRegenMaxAmount),
			Random.Range(PowerupInfo.HealthRegenMinAmount, PowerupInfo.HealthRegenMaxDuration),
			PowerupInfo.Lifetime,
			MapInfo.GetRandomPointOnMap()
			);		
	}

	public static PowerupScript CreateStaminaPowerup()
	{
		return CreatePowerup(
			PowerupType.Stamina,
			Random.Range(PowerupInfo.StaminaMinAmount, PowerupInfo.StaminaMaxAmount),
			0,
			PowerupInfo.Lifetime,
			MapInfo.GetRandomPointOnMap()
			);
	}

	public static PowerupScript CreateStaminaRegenPowerup()
	{
		return CreatePowerup(
			PowerupType.StaminaRegen,
			Random.Range(PowerupInfo.StaminaRegenMinAmount, PowerupInfo.StaminaRegenMaxAmount),
			Random.Range(PowerupInfo.StaminaRegenMinAmount, PowerupInfo.StaminaRegenMaxDuration),
			PowerupInfo.Lifetime,
			MapInfo.GetRandomPointOnMap()
			);
	}

	public static PowerupScript CreateExperiencePowerup()
	{
		return CreatePowerup(
			PowerupType.Experience,
			Random.Range(PowerupInfo.ExperienceMinAmount, PowerupInfo.ExperienceMaxAmount),
			0,
			PowerupInfo.Lifetime,
			MapInfo.GetRandomPointOnMap()
			);
	}

	public static PowerupScript CreateMovementSpeedPowerup()
	{
		return CreatePowerup(
			PowerupType.MovementSpeed,
			PowerupInfo.MovementSpeedAmount,
			PowerupInfo.MovementSpeedDuration,
			PowerupInfo.Lifetime,
			MapInfo.GetRandomPointOnMap()
			);
	}	

	public static SlimeTrapScript CreateSlimeTrap(float duration, Vector3 position)
	{
		//Instanciate our slime trap
		GameObject slimeTrapObject = (Object.Instantiate(instance.SlimeTrapPrefab, position, Quaternion.identity) as GameObject);
		SlimeTrapScript slimeTrap = slimeTrapObject.GetComponent<SlimeTrapScript>();

		//Set the slime traps duration
		slimeTrap.Duration = duration;

		//add the slime trap to the trap container
		slimeTrapObject.transform.parent = TrapContainer.transform;

		//return the slime trap
		return slimeTrap;
	}

	public static LandmineScript CreateLandmine(float Damage, Vector3 position)
	{
		//Instanciate our landmine
		GameObject landmineObject = (Object.Instantiate(instance.LandminePrefab, position, Quaternion.identity) as GameObject);
		LandmineScript landmine = landmineObject.GetComponent<LandmineScript>();

		//Set the landmines damage
		landmine.Damage = Damage;

		//add the land mine trap to the trap container
		landmineObject.transform.parent = TrapContainer.transform;

		//return the landmine
		return landmine;
	}

	public static DebugEnemyScript CreateDebugEnemy(Vector3 position)
	{
		//Instantiate enemy
		DebugEnemyScript enemy = (Object.Instantiate (instance.DebugEnemyPrefab, position, Quaternion.identity) as GameObject).GetComponent<DebugEnemyScript> ();

		return enemy;
	}

	public static EnemyBaseScript CreateRandomEnemy(Vector3 position)
	{
		int enemyType = Random.Range (0, 6);
		Debug.Log ("Type: "+enemyType);
		GameObject enemyPrefab;
		EnemyUpgrade enemyUpgrade;
		if(enemyType==0){ return CreateEnemyChaser(position, WaveSystem.ChaserUpgrade);}
		else if(enemyType==1){ return CreateEnemySniper(position, WaveSystem.SniperUpgrade);}
		else if(enemyType==2){ return CreateEnemyHealer(position, WaveSystem.HealerUpgrade);}
		else if(enemyType==3){ return CreateEnemySpawner(position, WaveSystem.SpawnerUpgrade);}
		else if(enemyType==4){ return CreateEnemyBouncer(position, WaveSystem.BouncerUpgrade);}
		else if(enemyType==5){ return CreateEnemyCharger(position, WaveSystem.ChargerUpgrade);}
		else{ return CreateEnemyChaser(position, WaveSystem.ChaserUpgrade);}
	}

	public static EnemyChaserScript CreateEnemyChaser(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemyChaserPrefab, position, Quaternion.identity) as GameObject;
		EnemyChaserScript enemy = enemyObject.GetComponent<EnemyChaserScript>();

		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;
	}

	public static EnemyChaserScript CreateEnemyBouncer(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemyBouncerPrefab, position, Quaternion.identity) as GameObject;
		EnemyChaserScript enemy = enemyObject.GetComponent<EnemyChaserScript>();
		
		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;
	}

	public static EnemyChargerScript CreateEnemyCharger(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemyChargerPrefab, position, Quaternion.identity) as GameObject;
		EnemyChargerScript enemy = enemyObject.GetComponent<EnemyChargerScript>();
		
		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;
	}

	public static EnemySniperScript CreateEnemySniper(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemySniperPrefab, position, Quaternion.identity) as GameObject;
		EnemySniperScript enemy = enemyObject.GetComponent<EnemySniperScript>();

		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;
	}

	public static EnemySniperScript CreateEnemySniperBoss(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemySniperBossPrefab, position, Quaternion.identity) as GameObject;
		EnemySniperScript enemy = enemyObject.GetComponent<EnemySniperScript>();

		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;
	}

	public static EnemyChaserScript CreateEnemyChaserBoss(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemyChaserBossPrefab, position, Quaternion.identity) as GameObject;
		EnemyChaserScript enemy = enemyObject.GetComponent<EnemyChaserScript>();

		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;		
	}

	public static EnemySpawnerScript CreateEnemySpawnerBoss(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemySpawnerBossPrefab, position, Quaternion.identity) as GameObject;
		EnemySpawnerScript enemy = enemyObject.GetComponent<EnemySpawnerScript>();

		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;
	}

	public static EnemyBufferScript CreateEnemyHealer(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemyHealerPrefab, position, Quaternion.identity) as GameObject;
		EnemyBufferScript enemy = enemyObject.GetComponent<EnemyBufferScript>();
		
		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;
	}

	public static EnemySpawnerScript CreateEnemySpawner(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemySpawnerPrefab, position, Quaternion.identity) as GameObject;
		EnemySpawnerScript enemy = enemyObject.GetComponent<EnemySpawnerScript>();
		
		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

		//add the enemy object to the enemy container
		enemyObject.transform.parent = EnemyContainer.transform;

		//return the enemy
		return enemy;
	}

}
