using UnityEngine;
using System.Collections;
using System;

public class EnemyChaserCloneScript : EnemyBaseCloneScript {

	//Enemy Movement
	public float TurnVelocity;
	
	//Enemy Attack
	public float Force;
	public float NextAttack;
	public float AttackDistance;
	
	public bool waitingForAnimationDelay;
	public const float AttackAnimationDelay = 0.5f;
	public float attackAnimationDelayTimer;

	public Animator anim;
	
	public GameObject EnemyAttackSphere;

	//Our statemachine
	public FSM<EnemyChaserCloneScript> StateMachine;
	public enum StateID{ moving, attacking };

	public void ChangeState( State<EnemyChaserCloneScript> s )
	{
		StateMachine.ChangeState ( s );
	}

	// Use this for initialization
	public override void Start () {
		StateMachine = new FSM<EnemyChaserCloneScript> ( this, MoveToPlayer.Instance );
		anim = GetComponent<Animator>();					  

		if (!player) AssignPlayer();
		WaveSystem.EnemiesRemaining++;
		
		// Set initial stats (should overide by applying an upgrade)
		if (!HasBeenUpgraded)
		{
			Health = 1;
			Velocity = 1;
			Damage = 1;
			AttackRate = 5;
			Experience = 1;
		}
		
		// Movement
		IsMoving = true;
		TurnVelocity = 5f;
		
		// Attack
		IsAttacking = false;
		AttackDistance = 2;
		NextAttack = AttackRate;
		
		//Knockback
		Force = 50f;
		mass = 20;
		
		//misc
		renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	public override void Update () {
		
		// Reset animation info
		//ClearAnimationInfo();
		
		// Check enemy health, if <=0 die
		CheckHealth ();
		
		// Move Enemy
		//MoveEnemy ();
		
		//apply knockback
		ApplyKnockback();
		
		// Rotate enemy towards player
		RotateEnemy ();

		//Update anything that needs a cooldown
		NextAttack = NextAttack - Time.deltaTime;

		//Run through this.StateMachine
		StateMachine.Update ();
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
			//EnemyAnimation.transform.rotation = Quaternion.LookRotation(playerDir);
		}
	}

	/*
	public void StopAndAttack () {
		NextAttack = NextAttack - Time.deltaTime;
		if (IsWithinAttackRange () && !waitingForAnimationDelay) {
			if(NextAttack <= 0){
				NextAttack = AttackRate;
				waitingForAnimationDelay = true;
				attackAnimationDelayTimer = AttackAnimationDelay;
				IsAttacking = true;
			}
		}

		if (waitingForAnimationDelay)
		{
			attackAnimationDelayTimer -= Time.deltaTime;
			if (attackAnimationDelayTimer <= 0)
			{
				// Create sphere attack

				Vector3 createPosition = transform.position + transform.forward;
				GameObject attack = Instantiate(EnemyAttackSphere) as GameObject;
				attack.transform.position = createPosition;
				attack.GetComponent<EnemyAttackSphereScript>().SetDamage(Damage);
				attack.GetComponent<EnemyAttackSphereScript>().SetForce(Force);
				
				waitingForAnimationDelay = false;
			}
		}
	}
	
	*/
}

