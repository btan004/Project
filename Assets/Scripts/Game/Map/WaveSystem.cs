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
	public PlayerScript playerScript;
	public SpawnScript spawnScript;
	public int RoundNumber;
	public int WaveNumber;
	public static int EnemiesRemaining;

	//wave information
	public static bool ForceSpawnWave;
	public static bool WaveFinished;

	//wave types: 3 repeating waves of steadily increasing numbers of enemies
	public static int WaveTypeCount = 6;
	public static List<int> ChasersPerWave = new List<int>()		{ 10,  10,  10,  10,  10,  0 };
	public static List<int> SnipersPerWave = new List<int> ()	{ 10,  10,  10,  10,  10,  0 };
	public static List<int> BouncersPerWave = new List<int>()	{  0,   3,   4,   5,   5,  0 };
	public static List<int> ChargersPerWave = new List<int>()	{  0,   0,   3,   4,   5,  0 };
	public static List<int> HealersPerWave = new List<int>()		{  0,   2,   3,   3,   5,  0 };
	public static List<int> SpawnersPerWave = new List<int>()	{  0,   0,   0,   3,   5,  0 };
	public static List<int> BossesPerWave = new List<int>()		{  0,   0,   0,   0,   0,  1 };

	//lives per difficulty
	public static Difficulty GameDifficulty;
	public static List<int> LivesPerDifficulty = new List<int>() { 5, 3, 1 };
	
	//chaser upgrades
	public static EnemyUpgrade ChaserUpgrade;
	public static List<float> ChaserHealthUpgrade = new List<float>() { 1, 2, 3 };
	public static List<float> ChaserVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> ChaserDamageUpgrade = new List<float>() { 3, 5, 7 };
	public static List<float> ChaserAttackRateUpgrade = new List<float>() { 0.95f, 0.9f, 0.8f };
	public const float ChaserHealthInitial = 20f;
	public const float ChaserVelocityInitial = 5f;
	public const float ChaserDamageInitial = 5f;
	public const float ChaserAttackRateInitial = 2f;
	public static float ChaserHealth;
	public static float ChaserVelocity;
	public static float ChaserDamage;
	public static float ChaserAttackRate;

	//Bouncer upgrades
	public static EnemyUpgrade BouncerUpgrade;
	public static List<float> BouncerHealthUpgrade = new List<float>() { 1, 2, 3 };
	public static List<float> BouncerVelocityUpgrade = new List<float>() { 0.125f, 0.25f, .5f };
	public static List<float> BouncerDamageUpgrade = new List<float>() { 6, 10, 14 };
	public static List<float> BouncerAttackRateUpgrade = new List<float>() { 0.95f, 0.9f, 0.8f };
	public const float BouncerHealthInitial = 20f;
	public const float BouncerVelocityInitial = 5f;
	public const float BouncerDamageInitial = 5f;
	public const float BouncerAttackRateInitial = 2f;
	public static float BouncerHealth;
	public static float BouncerVelocity;
	public static float BouncerDamage;
	public static float BouncerAttackRate;

	//Charger upgrades
	public static EnemyUpgrade ChargerUpgrade;
	public static List<float> ChargerHealthUpgrade = new List<float>() { 100, 200, 300 };
	public static List<float> ChargerVelocityUpgrade = new List<float>() { 0.125f, 0.25f, .5f };
	public static List<float> ChargerDamageUpgrade = new List<float>() { 6, 10, 14 };
	public static List<float> ChargerAttackRateUpgrade = new List<float>() { 0.95f, 0.9f, 0.8f };
	public const float ChargerHealthInitial = 20f;
	public const float ChargerVelocityInitial = 5f;
	public const float ChargerDamageInitial = 5f;
	public const float ChargerAttackRateInitial = 2f;
	public static float ChargerHealth;
	public static float ChargerVelocity;
	public static float ChargerDamage;
	public static float ChargerAttackRate;

	//sniper upgrades
	public static EnemyUpgrade SniperUpgrade;
	public static List<float> SniperHealthUpgrade = new List<float>() { 100, 200, 300 };
	public static List<float> SniperVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> SniperDamageUpgrade = new List<float>() { 6, 10, 15 };
	public static List<float> SniperAttackRateUpgrade = new List<float>() { 0.95f, 0.87f, 0.7f };
	public static float SniperHealthInitial = 15f;
	public static float SniperVelocityInitial = 5f;
	public static float SniperDamageInitial = 5f;
	public static float SniperAttackRateInitial = 2f;
	public static float SniperHealth;
	public static float SniperVelocity;
	public static float SniperDamage;
	public static float SniperAttackRate;

	//healer upgrades
	public static EnemyUpgrade HealerUpgrade;
	public static List<float> HealerHealthUpgrade = new List<float>() { 100, 200, 300 };
	public static List<float> HealerVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> HealerDamageUpgrade = new List<float>() { 6, 10, 15 };
	public static List<float> HealerAttackRateUpgrade = new List<float>() { 0.95f, 0.87f, 0.7f };
	public static float HealerHealthInitial = 15f;
	public static float HealerVelocityInitial = 5f;
	public static float HealerDamageInitial = 5f;
	public static float HealerAttackRateInitial = 2f;
	public static float HealerHealth;
	public static float HealerVelocity;
	public static float HealerDamage;
	public static float HealerAttackRate;

	//Spawner upgrades
	public static EnemyUpgrade SpawnerUpgrade;
	public static List<float> SpawnerHealthUpgrade = new List<float>() { 100, 200, 300 };
	public static List<float> SpawnerVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> SpawnerDamageUpgrade = new List<float>() { 3, 5, 7 };
	public static List<float> SpawnerAttackRateUpgrade = new List<float>() { 0.95f, 0.9f, 0.8f };
	public const float SpawnerHealthInitial = 20f;
	public const float SpawnerVelocityInitial = 5f;
	public const float SpawnerDamageInitial = 5f;
	public const float SpawnerAttackRateInitial = 2f;
	public const float SpawnerExperienceInitial = 10f;
	public static float SpawnerHealth;
	public static float SpawnerVelocity;
	public static float SpawnerDamage;
	public static float SpawnerAttackRate;

	//Boss upgrades
	public static EnemyUpgrade BossUpgrade;
	public static List<float> BossHealthUpgrade = new List<float>() { 100, 200, 300 };
	public static List<float> BossVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> BossDamageUpgrade = new List<float>() { 3, 5, 7 };
	public static List<float> BossAttackRateUpgrade = new List<float>() { 0.95f, 0.9f, 0.8f };
	public const float BossHealthInitial = 20f;
	public const float BossVelocityInitial = 5f;
	public const float BossDamageInitial = 5f;
	public const float BossAttackRateInitial = 2f;
	public static float BossHealth;
	public static float BossVelocity;
	public static float BossDamage;
	public static float BossAttackRate;

	public WaveSystem (SpawnScript spawnScript, PlayerScript playerScript)
	{
		//misc wave system init
		RoundNumber = 1;
		WaveNumber = 0;
		this.spawnScript = spawnScript;
		this.playerScript = playerScript;
		EnemiesRemaining = 0;
		ForceSpawnWave = false;
		WaveFinished = true;

		//initialize enemy data

		ChaserUpgrade = new EnemyUpgrade(ChaserHealthInitial, ChaserVelocityInitial, ChaserDamageInitial, ChaserAttackRateInitial);
		BouncerUpgrade = new EnemyUpgrade(BouncerHealthInitial, BouncerVelocityInitial, BouncerDamageInitial, BouncerAttackRateInitial);
		ChargerUpgrade = new EnemyUpgrade(ChargerHealthInitial, ChargerVelocityInitial, ChargerDamageInitial, ChargerAttackRateInitial);
		SniperUpgrade = new EnemyUpgrade(SniperHealthInitial, SniperVelocityInitial, SniperDamageInitial, SniperAttackRateInitial);
		HealerUpgrade = new EnemyUpgrade(HealerHealthInitial, HealerVelocityInitial, HealerDamageInitial, HealerAttackRateInitial);
		SpawnerUpgrade = new EnemyUpgrade(SpawnerHealthInitial, SpawnerVelocityInitial, SpawnerDamageInitial, SpawnerAttackRateInitial);
		BossUpgrade = new EnemyUpgrade(BossHealthInitial, BossVelocityInitial, BossDamageInitial, BossAttackRateInitial);

	}

	public void update()
	{
		//if enemies are remaining, then the wave is not finished
		WaveFinished = !(EnemiesRemaining > 0);

		//if the wave is finished
		if (WaveFinished)
		{
			//set the portal to active
			foreach (Component c in MapSystemScript.instance.GetCurrentLevel().GetComponents<Component>())
			{
				if (c.name == "Portal")
				{
					c.GetComponent<PortalScript>().IsActive = true;
				}
				else continue;
			}
		}

		if (EnemiesRemaining <= 0 && !WaveFinished)
		{
			//the wave is finished
			WaveFinished = true;

			//


		}

		if (ForceSpawnWave)
		{
			//increment wave and round numbers
			if (WaveNumber == WaveTypeCount)
			{
				WaveNumber = 1;
				RoundNumber++;
			}
			else
			{
				WaveNumber++;
			}

			SpawnWave();
		}
	}

	public void SpawnWave()
	{
		//create our enemies
		spawnScript.SpawnEnemy(SpawnScript.EnemyTypes.Chaser,		ChaserUpgrade,		ChasersPerWave		[WaveNumber - 1]);
		spawnScript.SpawnEnemy(SpawnScript.EnemyTypes.Bouncer,	BouncerUpgrade,	BouncersPerWave	[WaveNumber - 1]);
		spawnScript.SpawnEnemy(SpawnScript.EnemyTypes.Charger,	ChargerUpgrade,	ChargersPerWave	[WaveNumber - 1]);
		spawnScript.SpawnEnemy(SpawnScript.EnemyTypes.Sniper,		SniperUpgrade,		SnipersPerWave		[WaveNumber - 1]);
		spawnScript.SpawnEnemy(SpawnScript.EnemyTypes.Healer,		HealerUpgrade,		HealersPerWave		[WaveNumber - 1]);
		spawnScript.SpawnEnemy(SpawnScript.EnemyTypes.Spawner,	SpawnerUpgrade,	SpawnersPerWave	[WaveNumber - 1]);
		spawnScript.SpawnEnemy(SpawnScript.EnemyTypes.Boss,		BossUpgrade,		BossesPerWave		[WaveNumber - 1]);

		//after we spawn our enemies for the wave, update the upgrade for the next level
		if (WaveNumber == WaveTypeCount)
		{
			ChaserUpgrade.Health += ChaserHealthUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Velocity += ChaserVelocityUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Damage += ChaserDamageUpgrade[(int)GameDifficulty];
			ChaserUpgrade.AttackRate *= ChaserAttackRateUpgrade[(int)GameDifficulty];

			BouncerUpgrade.Health += BouncerHealthUpgrade[(int)GameDifficulty];
			BouncerUpgrade.Velocity += BouncerVelocityUpgrade[(int)GameDifficulty];
			BouncerUpgrade.Damage += BouncerDamageUpgrade[(int)GameDifficulty];
			BouncerUpgrade.AttackRate *= BouncerAttackRateUpgrade[(int)GameDifficulty];

			ChargerUpgrade.Health += ChargerHealthUpgrade[(int)GameDifficulty];
			ChargerUpgrade.Velocity += ChargerVelocityUpgrade[(int)GameDifficulty];
			ChargerUpgrade.Damage += ChargerDamageUpgrade[(int)GameDifficulty];
			ChargerUpgrade.AttackRate *= ChargerAttackRateUpgrade[(int)GameDifficulty];

			SniperUpgrade.Health += SniperHealthUpgrade[(int)GameDifficulty];
			SniperUpgrade.Velocity += SniperVelocityUpgrade[(int)GameDifficulty];
			SniperUpgrade.Damage += SniperDamageUpgrade[(int)GameDifficulty];
			SniperUpgrade.AttackRate *= SniperAttackRateUpgrade[(int)GameDifficulty];

			HealerUpgrade.Health += HealerHealthUpgrade[(int)GameDifficulty];
			HealerUpgrade.Velocity += HealerVelocityUpgrade[(int)GameDifficulty];
			HealerUpgrade.Damage += HealerDamageUpgrade[(int)GameDifficulty];
			HealerUpgrade.AttackRate *= HealerAttackRateUpgrade[(int)GameDifficulty];

			SpawnerUpgrade.Health += SpawnerHealthUpgrade[(int)GameDifficulty];
			SpawnerUpgrade.Velocity += SpawnerVelocityUpgrade[(int)GameDifficulty];
			SpawnerUpgrade.Damage += SpawnerDamageUpgrade[(int)GameDifficulty];
			SpawnerUpgrade.AttackRate *= SpawnerAttackRateUpgrade[(int)GameDifficulty];

			BossUpgrade.Health += BossHealthUpgrade[(int)GameDifficulty];
			BossUpgrade.Velocity += BossVelocityUpgrade[(int)GameDifficulty];
			BossUpgrade.Damage += BossDamageUpgrade[(int)GameDifficulty];
			BossUpgrade.AttackRate *= BossAttackRateUpgrade[(int)GameDifficulty];
		}

		//reset our force wave
		ForceSpawnWave = false;

		WaveFinished = false;
	}
}

