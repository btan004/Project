using UnityEngine;
using System.Collections;

public class MapSystemScript : MonoBehaviour
{

	public static MapSystemScript instance;

	public GameObject[] Levels;
	public GameObject Player;

	public int HomeLevel;
	public int BossLevel;
	public int WaveLevelStart;
	public int WaveLevelEnd;
	public int StartLevel;
	public int CurrentLevel;

	// Use this for initialization
	void Start()
	{
		if (instance == null)
		{
			instance = this;

			//disable all levels
			foreach (GameObject level in Levels)
				level.SetActive(false);

			//start the player in the home level
			TransitionToLevel(StartLevel);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public GameObject GetLevel(int level)
	{
		return Levels[level];
	}

	public GameObject GetHomeLevel()
	{
		return Levels[HomeLevel];
	}

	public GameObject GetBossLevel()
	{
		return Levels[BossLevel];
	}

	public GameObject GetCurrentLevel()
	{
		return Levels[CurrentLevel];
	}

	public LevelType GetCurrentLevelType()
	{
		return GetCurrentLevel().GetComponent<LevelScript>().CurrentLevelType;
	}

	public GameObject GetRandomWaveLevel()
	{
		return Levels[Random.Range(WaveLevelStart, WaveLevelEnd)];
	}

	public bool TrapsEnabled()
	{
		return Levels[CurrentLevel].GetComponent<LevelScript>().TrapsEnabled;
	}

	public bool PowerupsEnabled()
	{
		return Levels[CurrentLevel].GetComponent<LevelScript>().PowerupsEnabled;
	}

	public bool EnemiesEnabled()
	{
		return Levels[CurrentLevel].GetComponent<LevelScript>().EnemiesEnabled;
	}

	public Rect GetLevelBounds()
	{
		return GetCurrentLevel().GetComponent<LevelScript>().LevelBounds;
	}

	//responsible for camera fade out, moving player to the new level, then camera fade in
	public void TransitionToLevel(int level)
	{
		//fade out

		//disable the current level
		GetCurrentLevel().SetActive(false);

		//set our new level and activate it
		CurrentLevel = level;
		GetCurrentLevel().SetActive(true);

		//move the player
		Player.transform.position = GetCurrentLevel().GetComponent<LevelScript>().PlayerSpawnPoint.transform.position;

		//if the player moved home, give him points
		if (GetCurrentLevelType() == LevelType.Home)
		{
			Debug.Log("Player moved home");
			if (Player.GetComponent<PlayerScript>().Skills != null)
			{
				Debug.Log("Giving player skill points");
				Player.GetComponent<PlayerScript>().Skills.AddSkillPoints(WaveSystem.GameDifficulty);
			}
		}
		//else if the player moved to an arena zone, spawn the next wave.
		else
		{
			Debug.Log("Player moved to arena zone - spawning next wave");
			//WaveSystem.ForceSpawnWave = true;
			WaveSystem.instance.StartWaveCountdown();
			Debug.Log("<MapSystemScript> WaveSystem.ForceSpawnWave: " + WaveSystem.instance.ForceSpawnWave);
		}

		//fade in


		Debug.Log("Transitioned to Level: " + GetCurrentLevel().name);
	}


}
