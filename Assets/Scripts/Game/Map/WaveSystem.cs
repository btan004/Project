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
	public static WaveSystem instance;

	//misc
	public PlayerScript playerScript;
	public SpawnScript spawnScript;
	public int RoundNumber;
	public int WaveNumber;
	public static int EnemiesRemaining;

	//wave information
	public bool ForceSpawnWave;
	public static bool WaveCountdownOccuring;
	public static bool WaveFinished;
	
	//timer for spawning when player enters an arena zone
	public static float TimeUntilWaveSpawn = 5.0f;
	public static float spawnWaveTimer;

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
	public static List<float> ChaserHealthUpgrade = new List<float>() { 0.25f, 0.5f, .75f };
	public static List<float> ChaserVelocityUpgrade = new List<float>() { 0.25f, 0.5f, 1f };
	public static List<float> ChaserDamageUpgrade = new List<float>() { 0.25f, 0.5f, .75f };
	public static List<float> ChaserAttackRateUpgrade = new List<float>() { 0.97f, 0.95f, 0.8f };
	public const float ChaserHealthInitial = 2f;
	public const float ChaserVelocityInitial = 4f;
	public const float ChaserDamageInitial = 1f;
	public const float ChaserAttackRateInitial = 2f;
	public static float ChaserHealth;
	public static float ChaserVelocity;
	public static float ChaserDamage;
	public static float ChaserAttackRate;

	//Bouncer upgrades (upgrades based on chaser)
	public static EnemyUpgrade BouncerUpgrade;
	public static List<float> BouncerHealthUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> BouncerVelocityUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> BouncerDamageUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> BouncerAttackRateUpgrade = new List<float>() { 0, 0, 0 };
	public const float BouncerHealthInitial = ChaserHealthInitial * 2.0f;
	public const float BouncerVelocityInitial = ChaserVelocityInitial * 0.5f;
	public const float BouncerDamageInitial = ChaserDamageInitial * 2.0f;
	public const float BouncerAttackRateInitial = ChaserAttackRateInitial * 2.0f;
	public static float BouncerHealth;
	public static float BouncerVelocity;
	public static float BouncerDamage;
	public static float BouncerAttackRate;

	//Charger upgrades (upgrades based on chaser)
	public static EnemyUpgrade ChargerUpgrade;
	public static List<float> ChargerHealthUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> ChargerVelocityUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> ChargerDamageUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> ChargerAttackRateUpgrade = new List<float>() { 0, 0, 0 };
	public const float ChargerHealthInitial = ChaserHealthInitial * 1.5f;
	public const float ChargerVelocityInitial = ChaserVelocityInitial;
	public const float ChargerDamageInitial = ChaserDamageInitial * 4.0f;
	public const float ChargerAttackRateInitial = ChaserAttackRateInitial;
	public static float ChargerHealth;
	public static float ChargerVelocity;
	public static float ChargerDamage;
	public static float ChargerAttackRate;

	//sniper upgrades (upgrades based on chaser)
	public static EnemyUpgrade SniperUpgrade;
	public static List<float> SniperHealthUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> SniperVelocityUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> SniperDamageUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> SniperAttackRateUpgrade = new List<float>() { 0, 0, 0 };
	public static float SniperHealthInitial = ChaserHealthInitial * 0.5f;
	public static float SniperVelocityInitial = ChaserVelocityInitial;
	public static float SniperDamageInitial = ChaserDamageInitial * 1.25f;
	public static float SniperAttackRateInitial = ChaserAttackRateInitial * (1f / 1.25f);
	public static float SniperHealth;
	public static float SniperVelocity;
	public static float SniperDamage;
	public static float SniperAttackRate;

	//healer upgrades (upgrades based on chaser)
	public static EnemyUpgrade HealerUpgrade;
	public static List<float> HealerHealthUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> HealerVelocityUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> HealerDamageUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> HealerAttackRateUpgrade = new List<float>() { 0, 0, 0 };
	public static float HealerHealthInitial = ChaserHealthInitial * 0.75f;
	public static float HealerVelocityInitial = ChaserVelocityInitial;
	public static float HealerDamageInitial = 0;
	public static float HealerAttackRateInitial = 10000;
	public static float HealerHealth;
	public static float HealerVelocity;
	public static float HealerDamage;
	public static float HealerAttackRate;

	//Spawner upgrades (upgrades based on chaser)
	public static EnemyUpgrade SpawnerUpgrade;
	public static List<float> SpawnerHealthUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> SpawnerVelocityUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> SpawnerDamageUpgrade = new List<float>() { 0, 0, 0 };
	public static List<float> SpawnerAttackRateUpgrade = new List<float>() { 0, 0, 0 };
	public const float SpawnerHealthInitial = ChaserHealthInitial * 10.0f;
	public const float SpawnerVelocityInitial = 0;
	public const float SpawnerDamageInitial = 0;
	public const float SpawnerAttackRateInitial = 0;
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
	public const float BossHealthInitial = ChaserHealthInitial * 50.0f;
	public const float BossVelocityInitial = ChaserVelocityInitial * 1.5f;
	public const float BossDamageInitial = 2.0f;
	public const float BossAttackRateInitial = 1.0f;
	public static float BossHealth;
	public static float BossVelocity;
	public static float BossDamage;
	public static float BossAttackRate;

	public WaveSystem (SpawnScript spawnScript, PlayerScript playerScript)
	{
		if (instance == null)
		{
			instance = this;

			//misc wave system init
			RoundNumber = 1;
			WaveNumber = 0;
			this.spawnScript = spawnScript;
			this.playerScript = playerScript;
			EnemiesRemaining = 0;
			ForceSpawnWave = false;
			WaveFinished = true;
			WaveCountdownOccuring = false;

			spawnWaveTimer = TimeUntilWaveSpawn;

			//initialize enemy data
			ChaserUpgrade = new EnemyUpgrade(ChaserHealthInitial, ChaserVelocityInitial, ChaserDamageInitial, ChaserAttackRateInitial);
			BouncerUpgrade = new EnemyUpgrade(BouncerHealthInitial, BouncerVelocityInitial, BouncerDamageInitial, BouncerAttackRateInitial);
			ChargerUpgrade = new EnemyUpgrade(ChargerHealthInitial, ChargerVelocityInitial, ChargerDamageInitial, ChargerAttackRateInitial);
			SniperUpgrade = new EnemyUpgrade(SniperHealthInitial, SniperVelocityInitial, SniperDamageInitial, SniperAttackRateInitial);
			HealerUpgrade = new EnemyUpgrade(HealerHealthInitial, HealerVelocityInitial, HealerDamageInitial, HealerAttackRateInitial);
			SpawnerUpgrade = new EnemyUpgrade(SpawnerHealthInitial, SpawnerVelocityInitial, SpawnerDamageInitial, SpawnerAttackRateInitial);
			BossUpgrade = new EnemyUpgrade(BossHealthInitial, BossVelocityInitial, BossDamageInitial, BossAttackRateInitial);
		}



	}

	public void StartWaveCountdown()
	{
		Debug.LogWarning("Singleton WaveSystem told to StartWaveCountdown");
		ForceSpawnWave = true;
	}

	public void update()
	{
		//Debug.Log("Wave Finished: " + WaveFinished + ", Force Spawn Wave: " + ForceSpawnWave + ", Wave Countdown Occuring: " + WaveCountdownOccuring + ", Time Until Next Wave: " + spawnWaveTimer); 

		//if enemies are remaining, then the wave is not finished
		WaveFinished = (EnemyContainerScript.instance.GetEnemyCount() <= 0);

		if (ForceSpawnWave && !WaveCountdownOccuring)
		{
			//reset our timer
			spawnWaveTimer = TimeUntilWaveSpawn;
			WaveCountdownOccuring = true;

			Debug.Log("Starting wave spawn countdown!");
		}

		if (WaveCountdownOccuring)
		{
			//if we are ready to spawn the next wave
			if (spawnWaveTimer <= 0)
			{
				Debug.Log("Spawning wave!");

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

				ForceSpawnWave = false;
				WaveCountdownOccuring = false;
			}
			else
			{
				//continue counting down
				spawnWaveTimer -= Time.deltaTime;
			}
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
			ChaserUpgrade.Health			+= ChaserHealthUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Velocity		+= ChaserVelocityUpgrade[(int)GameDifficulty];
			ChaserUpgrade.Damage			+= ChaserDamageUpgrade[(int)GameDifficulty];
			ChaserUpgrade.AttackRate	*= ChaserAttackRateUpgrade[(int)GameDifficulty];

			BouncerUpgrade.Health		= 2.0f * ChaserUpgrade.Health;
			BouncerUpgrade.Velocity		= 0.5f * ChaserUpgrade.Velocity;
			BouncerUpgrade.Damage		= 2.0f * ChaserUpgrade.Damage;
			BouncerUpgrade.AttackRate	= 2.0f * ChaserUpgrade.AttackRate;

			ChargerUpgrade.Health		= 1.5f * ChaserUpgrade.Health;
			ChargerUpgrade.Velocity		= 1.0f * ChaserUpgrade.Velocity;
			ChargerUpgrade.Damage		= 4.0f * ChaserUpgrade.Damage;
			ChargerUpgrade.AttackRate	= 0f;

			SniperUpgrade.Health			= 0.5f * ChaserUpgrade.Health;
			SniperUpgrade.Velocity		= 1.0f * ChaserUpgrade.Velocity;
			SniperUpgrade.Damage			= 1.25f * ChaserUpgrade.Damage;
			SniperUpgrade.AttackRate	= (1f / 1.25f) * ChaserUpgrade.AttackRate;

			HealerUpgrade.Health			= 0.75f * ChaserUpgrade.Health;
			HealerUpgrade.Velocity		= 1.0f * ChaserUpgrade.Velocity;
			HealerUpgrade.Damage			= 0;
			HealerUpgrade.AttackRate	= 0;

			SpawnerUpgrade.Health		= 10.0f * ChaserUpgrade.Health;
			SpawnerUpgrade.Velocity		= 0;
			SpawnerUpgrade.Damage		= 0;
			SpawnerUpgrade.AttackRate	= 0;

			BossUpgrade.Health			= 50.0f * ChaserUpgrade.Health;
			BossUpgrade.Velocity			= 1.5f * ChaserUpgrade.Velocity;
			BossUpgrade.Damage			= 2.0f * ChaserUpgrade.Damage;
			BossUpgrade.AttackRate		= 0.5f * ChaserUpgrade.AttackRate;
		}
	}
}

