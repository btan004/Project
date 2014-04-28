using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Difficulty
{
	Easy = 0,
	Normal = 1,
	Hard = 2
};

public class WaveSystem
{
	//misc
	public SpawnScript spawnScript;
	public int WaveNumber;
	public static int EnemiesRemaining;

	//wave information
	public const float TimeBetweenWaves = 3f;
	public float TimeBetweenWavesTimer;
	private bool hasFinishedWave;

	//wave types: 3 repeating waves of steadily increasing numbers of enemies
	public static int WaveTypeCount = 3;
	public static List<int> ChasersPerWave = new List<int>() { 10, 15, 18 };
	public static List<int> SnipersPerWave = new List<int> () { 7, 12, 16 };
	public static List<int> BigChasersPerWave = new List<int>() { 3, 5, 8 };
	public static List<int> HealersPerWave = new List<int>() { 0, 3, 5 };
	public static List<int> SpawnersPerWave = new List<int>() { 0, 0, 3 };

	//upgrades per difficulty
	public static Difficulty GameDifficulty;
	public static List<int> LivesPerDifficulty = new List<int>() { 5, 3, 1 };
	
	//chaser upgrades
	public static EnemyUpgrade ChaserUpgrade;
	public static List<float> ChaserHealthUpgrade = new List<float>() { 5, 10, 20 };
	public static List<float> ChaserVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> ChaserDamageUpgrade = new List<float>() { 3, 5, 7 };
	public static List<float> ChaserAttackRateUpgrade = new List<float>() { 0.95f, 0.9f, 0.8f };
	public static List<float> ChaserExperienceUpgrade = new List<float>() { 10, 20, 30 };
	public const float ChaserHealthInitial = 20f;
	public const float ChaserVelocityInitial = 5f;
	public const float ChaserDamageInitial = 5f;
	public const float ChaserAttackRateInitial = 2f;
	public const float ChaserExperienceInitial = 10f;
	public static float ChaserHealth;
	public static float ChaserVelocity;
	public static float ChaserDamage;
	public static float ChaserAttackRate;
	public static float ChaserExperience;

	//BigChaser upgrades
	public static EnemyUpgrade BigChaserUpgrade;
	public static List<float> BigChaserHealthUpgrade = new List<float>() { 5, 10, 20 };
	public static List<float> BigChaserVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> BigChaserDamageUpgrade = new List<float>() { 3, 5, 7 };
	public static List<float> BigChaserAttackRateUpgrade = new List<float>() { 0.95f, 0.9f, 0.8f };
	public static List<float> BigChaserExperienceUpgrade = new List<float>() { 10, 20, 30 };
	public const float BigChaserHealthInitial = 20f;
	public const float BigChaserVelocityInitial = 5f;
	public const float BigChaserDamageInitial = 5f;
	public const float BigChaserAttackRateInitial = 2f;
	public const float BigChaserExperienceInitial = 10f;
	public static float BigChaserHealth;
	public static float BigChaserVelocity;
	public static float BigChaserDamage;
	public static float BigChaserAttackRate;
	public static float BigChaserExperience;

	//sniper upgrades
	public static EnemyUpgrade SniperUpgrade;
	public static List<float> SniperHealthUpgrade = new List<float>() { 2, 5, 10 };
	public static List<float> SniperVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> SniperDamageUpgrade = new List<float>() { 6, 10, 15 };
	public static List<float> SniperAttackRateUpgrade = new List<float>() { 0.95f, 0.87f, 0.7f };
	public static List<float> SniperExperienceUpgrade = new List<float>() { 10, 20, 30 };
	public static float SniperHealthInitial = 15f;
	public static float SniperVelocityInitial = 5f;
	public static float SniperDamageInitial = 5f;
	public static float SniperAttackRateInitial = 2f;
	public static float SniperExperienceInitial = 15f;
	public static float SniperHealth;
	public static float SniperVelocity;
	public static float SniperDamage;
	public static float SniperAttackRate;
	public static float SniperExperience;

	//healer upgrades
	public static EnemyUpgrade HealerUpgrade;
	public static List<float> HealerHealthUpgrade = new List<float>() { 2, 5, 10 };
	public static List<float> HealerVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> HealerDamageUpgrade = new List<float>() { 6, 10, 15 };
	public static List<float> HealerAttackRateUpgrade = new List<float>() { 0.95f, 0.87f, 0.7f };
	public static List<float> HealerExperienceUpgrade = new List<float>() { 10, 20, 30 };
	public static float HealerHealthInitial = 15f;
	public static float HealerVelocityInitial = 5f;
	public static float HealerDamageInitial = 5f;
	public static float HealerAttackRateInitial = 2f;
	public static float HealerExperienceInitial = 15f;
	public static float HealerHealth;
	public static float HealerVelocity;
	public static float HealerDamage;
	public static float HealerAttackRate;
	public static float HealerExperience;

	//Spawner upgrades
	public static EnemyUpgrade SpawnerUpgrade;
	public static List<float> SpawnerHealthUpgrade = new List<float>() { 5, 10, 20 };
	public static List<float> SpawnerVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> SpawnerDamageUpgrade = new List<float>() { 3, 5, 7 };
	public static List<float> SpawnerAttackRateUpgrade = new List<float>() { 0.95f, 0.9f, 0.8f };
	public static List<float> SpawnerExperienceUpgrade = new List<float>() { 10, 20, 30 };
	public const float SpawnerHealthInitial = 20f;
	public const float SpawnerVelocityInitial = 5f;
	public const float SpawnerDamageInitial = 5f;
	public const float SpawnerAttackRateInitial = 2f;
	public const float SpawnerExperienceInitial = 10f;
	public static float SpawnerHealth;
	public static float SpawnerVelocity;
	public static float SpawnerDamage;
	public static float SpawnerAttackRate;
	public static float SpawnerExperience;

	public WaveSystem (SpawnScript spawnScript)
	{
		//misc wave system init
		WaveNumber = 0;
		this.spawnScript = spawnScript;
		EnemiesRemaining = 0;
		TimeBetweenWavesTimer = 0;
		hasFinishedWave = false;

		//initialize enemy data
		ChaserHealth = ChaserHealthInitial;
		ChaserVelocity = ChaserVelocityInitial;
		ChaserDamage = ChaserDamageInitial;
		ChaserAttackRate = ChaserAttackRateInitial;
		ChaserExperience = ChaserExperienceInitial;
		ChaserUpgrade = new EnemyUpgrade(ChaserHealth, ChaserVelocity, ChaserDamage, ChaserAttackRate, ChaserExperience);

		BigChaserHealth = BigChaserHealthInitial;
		BigChaserVelocity = BigChaserVelocityInitial;
		BigChaserDamage = BigChaserDamageInitial;
		BigChaserAttackRate = BigChaserAttackRateInitial;
		BigChaserExperience = BigChaserExperienceInitial;
		BigChaserUpgrade = new EnemyUpgrade(BigChaserHealth, BigChaserVelocity, BigChaserDamage, BigChaserAttackRate, BigChaserExperience);

		SniperHealth = SniperHealthInitial;
		SniperVelocity = SniperVelocityInitial;
		SniperDamage = SniperDamageInitial;
		SniperAttackRate = SniperAttackRateInitial;
		SniperExperience = SniperExperienceInitial;
		SniperUpgrade = new EnemyUpgrade(SniperHealth, SniperVelocity, SniperDamage, SniperAttackRate, SniperExperience);

		HealerHealth = HealerHealthInitial;
		HealerVelocity = HealerVelocityInitial;
		HealerDamage = HealerDamageInitial;
		HealerAttackRate = HealerAttackRateInitial;
		HealerExperience = HealerExperienceInitial;
		HealerUpgrade = new EnemyUpgrade(HealerHealth, HealerVelocity, HealerDamage, HealerAttackRate, HealerExperience);

		SpawnerHealth = SpawnerHealthInitial;
		SpawnerVelocity = SpawnerVelocityInitial;
		SpawnerDamage = SpawnerDamageInitial;
		SpawnerAttackRate = SpawnerAttackRateInitial;
		SpawnerExperience = SpawnerExperienceInitial;
		SpawnerUpgrade = new EnemyUpgrade(SpawnerHealth, SpawnerVelocity, SpawnerDamage, SpawnerAttackRate, SpawnerExperience);

	}

	public void update()
	{
		if (EnemiesRemaining == 0 && !hasFinishedWave) {
			hasFinishedWave = true;
			TimeBetweenWavesTimer = TimeBetweenWaves;
		}

		if (hasFinishedWave)
			TimeBetweenWavesTimer -= Time.deltaTime;
			
		if (hasFinishedWave && TimeBetweenWavesTimer < 0) 
		{
			SpawnWave();
			WaveNumber++;
			hasFinishedWave = false;
		}

	}

	public void SpawnWave()
	{
		//determine the wave type so we know how many of each unit to spawn
		int waveType = WaveNumber % WaveTypeCount;

		//create our chasers
		spawnScript.SpawnEnemy (ChasersPerWave [waveType], SpawnScript.EnemyTypes.Chaser, ChaserUpgrade);

		//create our big chasers
		spawnScript.SpawnEnemy(BigChasersPerWave[waveType], SpawnScript.EnemyTypes.BigChaser, BigChaserUpgrade);

		//create our snipers
		spawnScript.SpawnEnemy (SnipersPerWave [waveType], SpawnScript.EnemyTypes.Sniper, SniperUpgrade);

		//create our healers
		spawnScript.SpawnEnemy(HealersPerWave[waveType], SpawnScript.EnemyTypes.Healer, HealerUpgrade);

		//create our spawners
		spawnScript.SpawnEnemy(SpawnersPerWave[waveType], SpawnScript.EnemyTypes.Spawner, SpawnerUpgrade);

		//increase buffs at the end of spawning wave 3
		if (waveType == 2)
		{
			ChaserUpgrade.Health += ChaserHealthUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Velocity += ChaserVelocityUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Damage += ChaserDamageUpgrade[(int)GameDifficulty];
			ChaserUpgrade.AttackRate *= ChaserAttackRateUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Experience += ChaserExperienceUpgrade[(int)GameDifficulty];

			BigChaserUpgrade.Health += BigChaserHealthUpgrade[(int)GameDifficulty];
			BigChaserUpgrade.Velocity += BigChaserVelocityUpgrade[(int)GameDifficulty];
			BigChaserUpgrade.Damage += BigChaserDamageUpgrade[(int)GameDifficulty];
			BigChaserUpgrade.AttackRate *= BigChaserAttackRateUpgrade[(int)GameDifficulty];
			BigChaserUpgrade.Experience += BigChaserExperienceUpgrade[(int)GameDifficulty];

			SniperUpgrade.Health += SniperHealthUpgrade[(int)GameDifficulty];
			SniperUpgrade.Velocity += SniperVelocityUpgrade[(int)GameDifficulty];
			SniperUpgrade.Damage += SniperDamageUpgrade[(int)GameDifficulty];
			SniperUpgrade.AttackRate *= SniperAttackRateUpgrade[(int)GameDifficulty];
			SniperUpgrade.Experience += SniperExperienceUpgrade[(int)GameDifficulty];

			HealerUpgrade.Health += HealerHealthUpgrade[(int)GameDifficulty];
			HealerUpgrade.Velocity += HealerVelocityUpgrade[(int)GameDifficulty];
			HealerUpgrade.Damage += HealerDamageUpgrade[(int)GameDifficulty];
			HealerUpgrade.AttackRate *= HealerAttackRateUpgrade[(int)GameDifficulty];
			HealerUpgrade.Experience += HealerExperienceUpgrade[(int)GameDifficulty];

			SpawnerUpgrade.Health += SpawnerHealthUpgrade[(int)GameDifficulty];
			SpawnerUpgrade.Velocity += SpawnerVelocityUpgrade[(int)GameDifficulty];
			SpawnerUpgrade.Damage += SpawnerDamageUpgrade[(int)GameDifficulty];
			SpawnerUpgrade.AttackRate *= SpawnerAttackRateUpgrade[(int)GameDifficulty];
			SpawnerUpgrade.Experience += SpawnerExperienceUpgrade[(int)GameDifficulty];
		}
	}
}

