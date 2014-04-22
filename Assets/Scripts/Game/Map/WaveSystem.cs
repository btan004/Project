using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSystem
{
	public SpawnScript spawnScript;

	public int WaveNumber;
	public static int EnemiesRemaining;

	public float TimeBetweenWaves = 3f;
	public float TimeBetweenWavesTimer;
	private bool hasFinishedWave;

	public static int waveTypeCount = 3;
	public static List<int> chasersPerWave = new List<int>() { 10, 15, 18 };
	public static List<int> snipersPerWave = new List<int> () { 7, 12, 16 };

	public WaveSystem (SpawnScript spawnScript)
	{
		WaveNumber = 0;
		this.spawnScript = spawnScript;
		EnemiesRemaining = 0;
		TimeBetweenWavesTimer = 0;
		TimeBetweenWaves = 5;
		hasFinishedWave = false;
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
		int waveType = WaveNumber % waveTypeCount;
		spawnScript.SpawnEnemy (chasersPerWave [waveType], SpawnScript.EnemyTypes.Chaser);
		spawnScript.SpawnEnemy (snipersPerWave [waveType], SpawnScript.EnemyTypes.Sniper);

		//increase buffs

	}
}

