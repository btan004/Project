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
	public const float TimeBetweenWaves = 99f;
	public float TimeBetweenWavesTimer;
	private bool hasFinishedWave;

	//wave types: 3 repeating waves of steadily increasing numbers of enemies
	public static int WaveTypeCount = 3;
	public static List<int> ChasersPerWave = new List<int>() { 10, 15, 18 };
	public static List<int> SnipersPerWave = new List<int> () { 7, 12, 16 };

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

		SniperHealth = SniperHealthInitial;
		SniperVelocity = SniperVelocityInitial;
		SniperDamage = SniperDamageInitial;
		SniperAttackRate = SniperAttackRateInitial;
		SniperExperience = SniperExperienceInitial;
		SniperUpgrade = new EnemyUpgrade(SniperHealth, SniperVelocity, SniperDamage, SniperAttackRate, SniperExperience);
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

		//create our snipers
		spawnScript.SpawnEnemy (SnipersPerWave [waveType], SpawnScript.EnemyTypes.Sniper, SniperUpgrade);

		//increase buffs at the end of spawning wave 3
		if (waveType == 2)
		{
			ChaserUpgrade.Health += ChaserHealthUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Velocity += ChaserVelocityUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Damage += ChaserDamageUpgrade[(int)GameDifficulty];
			ChaserUpgrade.AttackRate *= ChaserAttackRateUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Experience += ChaserExperienceUpgrade[(int)GameDifficulty];

			SniperUpgrade.Health += SniperHealthUpgrade[(int)GameDifficulty];
			SniperUpgrade.Velocity += SniperVelocityUpgrade[(int)GameDifficulty];
			SniperUpgrade.Damage += SniperDamageUpgrade[(int)GameDifficulty];
			SniperUpgrade.AttackRate *= SniperAttackRateUpgrade[(int)GameDifficulty];
			SniperUpgrade.Experience += SniperExperienceUpgrade[(int)GameDifficulty];
		}
	}
}

