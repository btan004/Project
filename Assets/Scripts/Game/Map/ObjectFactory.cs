using UnityEngine;
using System.Collections;

public class ObjectFactory : MonoBehaviour {

	protected static ObjectFactory instance;

	//Our objects that we can create

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

	// Use this for initialization
	void Start () {
		instance = this;

		
	}
	
	public static PowerupScript CreatePowerup(PowerupType type, float amount, float duration, float lifetime, Vector3 position)
	{
		//Instanciate our powerup
		PowerupScript powerup = (Object.Instantiate(instance.PowerupPrefab, position, Quaternion.identity) as GameObject).GetComponent<PowerupScript>();

		//Set the powerup's stats
		powerup.Type = type;
		powerup.Amount = amount;
		powerup.Duration = duration;
		powerup.Lifetime = lifetime;

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
			Random.Range(PowerupInfo.MovementSpeedMinAmount, PowerupInfo.MovementSpeedMaxAmount),
			Random.Range(PowerupInfo.MovementSpeedMinDuration, PowerupInfo.MovementSpeedMaxDuration),
			PowerupInfo.Lifetime,
			MapInfo.GetRandomPointOnMap()
			);
	}	

	public static SlimeTrapScript CreateSlimeTrap(float duration, Vector3 position)
	{
		//Instanciate our slime trap
		SlimeTrapScript slimeTrap = (Object.Instantiate(instance.SlimeTrapPrefab, position, Quaternion.identity) as GameObject).GetComponent<SlimeTrapScript>();

		//Set the slime traps duration
		slimeTrap.Duration = duration;

		//return the slime trap
		return slimeTrap;
	}

	public static LandmineScript CreateLandmine(float Damage, Vector3 position)
	{
		//Instanciate our landmine
		LandmineScript landmine = (Object.Instantiate(instance.LandminePrefab, position, Quaternion.identity) as GameObject).GetComponent<LandmineScript>();

		//Set the landmines damage
		landmine.Damage = Damage;

		//return the landmine
		return landmine;
	}

	public static DebugEnemyScript CreateDebugEnemy(Vector3 position)
	{
		//Instantiate enemy
		DebugEnemyScript enemy = (Object.Instantiate (instance.DebugEnemyPrefab, position, Quaternion.identity) as GameObject).GetComponent<DebugEnemyScript> ();

		return enemy;
	}

	public static EnemyChaserScript CreateEnemyChaser(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemyChaserPrefab, position, Quaternion.identity) as GameObject;
		EnemyChaserScript enemy = enemyObject.GetComponent<EnemyChaserScript>();

		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);

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

		//return the enemy
		return enemy;
	}

	public static EnemyHealerScript CreateEnemyHealer(Vector3 position, EnemyUpgrade upgrade)
	{
		//Instanciate our enemy
		GameObject enemyObject = Instantiate(instance.EnemyHealerPrefab, position, Quaternion.identity) as GameObject;
		EnemyHealerScript enemy = enemyObject.GetComponent<EnemyHealerScript>();
		
		//Set the stats for the enemy
		enemy.ApplyUpgrade(upgrade);
		
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
		
		//return the enemy
		return enemy;
	}

}
