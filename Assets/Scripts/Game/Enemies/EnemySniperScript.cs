using UnityEngine;
using System.Collections;

public class EnemySniperScript : EnemyBaseScript {

	//Enemy Movement
	public bool IsMoving;
	public float Velocity;
	public float TurnVelocity;
	
	//Enemy Attack
	public bool IsAttacking;
	public float AttackPower;
	public float AttackRate;
	public float NextAttack;
	public float AttackDistance;
	public GameObject EnemyBulletPrefab;

	// Use this for initialization
	public override void Start () {
		if (!player) AssignPlayer();

		// Set stats
		Health = 200;
		ExperienceToGive = 50;

		// Movement
		IsMoving = true;
		Velocity = 3f;
		TurnVelocity = 5f;

		// Attack
		IsAttacking = false;
		AttackPower = 1;
		AttackRate = 1;
		NextAttack = AttackRate;
		AttackDistance = 10;

		renderer.material.color = new Color(1f, 165f / 255f, 0f);
	}
	
	// Update is called once per frame
	public override void Update () {
		// Check enemy health, if <=0 die
		CheckHealth ();
		
		// Move Enemy
		MoveEnemy ();

		//
		ApplyKnockback();

		// Rotate enemy towards player
		RotateEnemy ();
		
		// If within a certain distance stop and attack player
		StopAndAttack ();
	}

	// Figure out if enemy within range of player
	public bool IsWithinAttackRange(){
		// Find player in game
		if (player) {
			// Get player location
			Vector3 playerLocation = player.transform.position;

			// Get distance between player and enemy
			float distance = Vector3.Distance (playerLocation, this.transform.position);

			// Return if within range
			return (distance <= AttackDistance);
		}
		else{
			return false;
		}
	}

	public void RotateEnemy() {
		if (player) {
			// Get player location
			Vector3 playerLocation = player.transform.position;
			
			// Set rotation step
			float rotationStep = TurnVelocity*Time.deltaTime;
			
			// Rotate enemy towards player
			Vector3 playerDir = Vector3.RotateTowards(this.transform.forward,playerLocation-this.transform.position,rotationStep,0.0f);
			this.transform.rotation = Quaternion.LookRotation(playerDir);
		}
	}

	public void MoveEnemy() {
		// Find player in game
		if (player && IsMoving && !IsWithinAttackRange() ) {
			// Get player location
			Vector3 playerLocation = player.transform.position;
			
			// Set movement step
			float moveStep = Velocity*Time.deltaTime;
			
			// Move towards player
			this.transform.position = Vector3.MoveTowards(this.transform.position,playerLocation,moveStep);
		}
	}

	public void StopAndAttack (){
		if (IsWithinAttackRange ()) {
			NextAttack = NextAttack - Time.deltaTime;
			if(NextAttack <=0){
				GameObject bullet = Instantiate(EnemyBulletPrefab,this.transform.position,Quaternion.identity) as GameObject;
				bullet.GetComponent<EnemyBulletScript>().SetDamage(AttackPower);
				NextAttack = AttackRate;
			}
		}
	}
}
