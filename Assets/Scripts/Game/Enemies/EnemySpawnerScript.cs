using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : EnemyBaseScript {

	// Spawning
	public int CurrentSpawns;
	public int MaxSpawns;
	public GameObject EnemySpawn;
	public float SpawnRadius;
	public float CurrentBuffer;
	public float MaxBuffer;
	public GameObject[] EnemyList;
	public int EnemyStartAmount;
	public int EnemyMaxAmount;

<<<<<<< HEAD
	//Boss
	//public bool isBoss;

=======
>>>>>>> 5269c658c5d931b2c138444d9f8ebf35e4ba3970
	// Use this for initialization
	public override void Start () {
		// Set stats
		Health = 5;
		ScoreValue = 100;

		// Spawning
		CurrentSpawns = 0;
		MaxSpawns = 5;
		MaxBuffer = 3f;
		CurrentBuffer = MaxBuffer;
		SpawnRadius = 7f;

		// Color
		renderer.material.color = Color.cyan;

		if (isBoss)
		{
			GameGUI.Boss = this;
			GameGUI.BossActive = true;
		}

		RefreshRendererInfo();

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		// Check spawns
		CheckCurrentSpawns ();
	}

	// How many of your spawns are in play
	public void CheckCurrentSpawns() {
		if(CurrentSpawns <= 0){
			CreateSpawns();
		}
	}

	// New batch of enemies
	public void CreateSpawns() {
		// Time buffer before next batch appears
		CurrentBuffer = CurrentBuffer - Time.deltaTime;
		if(CurrentBuffer<=0){
			// Spawn enemies in circle
			float deg = 0f; 
			for(int i = MaxSpawns; i>0; i=i-1){
				//GameObject enemy = Instantiate(EnemySpawn) as GameObject;
				deg = deg + (360f/MaxSpawns);
				float enemyx = transform.position.x + SpawnRadius*Mathf.Cos(deg*Mathf.Deg2Rad);
				float enemyz = transform.position.z + SpawnRadius*Mathf.Sin(deg*Mathf.Deg2Rad);
				EnemyBaseScript enemy;
				if(!isBoss){
					enemy = ObjectFactory.CreateEnemyChaser(new Vector3(enemyx, 1, enemyz), WaveSystem.ChaserUpgrade);
				}
				else{
					enemy = ObjectFactory.CreateRandomEnemy(new Vector3(enemyx, 1, enemyz));
				}

				enemy.renderer.material.color = Color.red;
				enemy.SetSpawner(this.gameObject);
				CurrentSpawns = CurrentSpawns+1;
			}

			// Reset buffer
			CurrentBuffer = MaxBuffer; 
		}
	}

	// Reduce Spawns
	public void ReduceSpawns(){
		CurrentSpawns = CurrentSpawns - 1;
	}
}
