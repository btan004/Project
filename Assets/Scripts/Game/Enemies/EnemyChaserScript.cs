using UnityEngine;
using System.Collections;

public class EnemyChaserScript : EnemyBaseScript {

	//Enemy Movement
	public bool IsMoving;
	public float Velocity;
	public float TurnVelocity;
	
	//Enemy Attack
	public bool IsAttacking;
	public float AttackPower;
	public float Force;
	public float AttackRate;
	public float NextAttack;
	public float AttackDistance;
	public GameObject EnemyAttackSphere;

	// Use this for initialization
	public override void Start () {
		if (!player) AssignPlayer();

		// Set stats
		Health = 200;
		ExperienceToGive = 10;

		// Movement
		IsMoving = true;
		Velocity = 3f;
		TurnVelocity = 5f;

		// Attack
		IsAttacking = false;
		AttackPower = 10;
		Force = 10f;
		AttackRate = 3;
		AttackDistance = 2;
		NextAttack = AttackRate;

		renderer.material.color = Color.green;
		mass = 20;
	}
	
	// Update is called once per frame
	public override void Update () {

		// Check enemy health, if <=0 die
		CheckHealth ();

		// Move Enemy
		MoveEnemy ();

		//apply knockback
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
			playerDir = new Vector3(playerDir.x,0,playerDir.z);
			this.transform.rotation = Quaternion.LookRotation(playerDir);
		}
	}

	public void MoveEnemy() {
		// Find player in game
		if (player && IsMoving && !IsWithinAttackRange()) {
			// Get player location
			Vector3 playerLocation = player.transform.position;

			// Set movement step
			float moveStep = Velocity*Time.deltaTime;

			// Move towards player
			this.transform.position = Vector3.MoveTowards(this.transform.position,playerLocation,moveStep);
		}
	}

	public void StopAndAttack () {
		NextAttack = NextAttack - Time.deltaTime;
		if (IsWithinAttackRange ()) {
			if(NextAttack <= 0){
				NextAttack = AttackRate;
				Vector3 createPosition = transform.position + transform.forward;
				GameObject attack = Instantiate(EnemyAttackSphere) as GameObject;
				attack.transform.position = createPosition;
				attack.GetComponent<EnemyAttackSphereScript>().SetDamage(AttackPower);
				attack.GetComponent<EnemyAttackSphereScript>().SetForce(Force);
			}
		}
	}

}
