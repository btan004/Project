using UnityEngine;
using System.Collections;

public class EnemyChaserScript : EnemyBaseScript {

	//Enemy Movement
	public float TurnVelocity;
	
	//Enemy Attack
	public float Force;
	public float NextAttack;
	public float AttackDistance;

	public bool waitingForAnimationDelay;
	public const float AttackAnimationDelay = 0.3f;
	public float attackAnimationDelayTimer;

	public Animator anim;

	public GameObject EnemyAttackSphere;

	//Our statemachine
	public FSM<EnemyChaserScript> StateMachine;
	public enum StateID{ moving, attacking };
	
	public void ChangeState( State<EnemyChaserScript> s )
	{
		StateMachine.ChangeState ( s );
	}

	// Use this for initialization
	public override void Start () {
		StateMachine = new FSM<EnemyChaserScript> ( this, Chaser_MoveToPlayer.Instance );
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
		}
		ScoreValue = 100;

		// Movement
		IsMoving = true;
		TurnVelocity = 5f;

		// Attack
		IsAttacking = false;
		AttackDistance = 2;
		NextAttack = AttackRate;
		waitingForAnimationDelay = false;
		attackAnimationDelayTimer = AttackAnimationDelay;

		//Knockback
		Force = 100f;
		mass = 20;

		//Checking if there are any renderers to flash when this enemy is hit
		if( renderers.Length <= 0 )
		{
			//Debug.LogWarning("[EnemyChaserScript]: No renderers are set in order to flash this enemy when they are hit. If this is intentional, ignore");
		}
		
		//misc
		renderer.material.color = Color.green;

		RefreshRendererInfo();

		if(IsSpawned){
			SpawnedColor = new Color(255f/255f, 118f/255f, 0f/255f);
			FlashSpawned();
			//Debug.Log("Spawned");
		}
		else{
			//Debug.Log("Is not spawned");
		}
	}
	
	// Update is called once per frame
	public override void Update () {
		// Reset animation info
		//ClearAnimationInfo();

		// Check enemy health, if <=0 die
		base.Update();
		//CheckHealth ();

		// Move Enemy
		//MoveEnemy ();

		//apply knockback
		ApplyKnockback();

		// Rotate enemy towards player
		RotateEnemy ();

		//Update anything that needs a cooldown
		NextAttack = NextAttack - Time.deltaTime;
		
		//Run through this.StateMachine
		if( Health > 0 )
		{
			StateMachine.Update ();
		}
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

	public void RotateEnemy(){
		if (player) {
			// Get player location
			Vector3 playerLocation = player.transform.position;
			
			// Set rotation step
			float rotationStep = TurnVelocity*Time.deltaTime;
			
			// Rotate enemy towards player
			Vector3 playerDir = Vector3.RotateTowards( this.transform.forward, playerLocation-this.transform.position, rotationStep, 0.0f);
			
			//Set y vector to 0 since we don't want to do anything with the y axis
			playerDir.y = 0;
			
			this.transform.rotation = Quaternion.LookRotation(playerDir);
		}
		/*
		if (player) {
			// Get velocity path
			Vector3 rotateDir = this.GetComponent<NavMeshAgent>().velocity;
			Debug.Log ( this.GetComponent<NavMeshAgent>().velocity );
			
			// Set rotation step
			float rotationStep = 5f*Time.deltaTime;
			
			// Rotate enemy towards player
			rotateDir = new Vector3(rotateDir.x,0,rotateDir.z);
			this.transform.rotation = Quaternion.LookRotation(rotateDir);
		}*/
	}

	/*
	public void MoveEnemy() {
		// Find player in game
		if (!IsWithinAttackRange ()){
			IsMoving = true;
		}

		if (player && IsMoving && !IsWithinAttackRange()) {
			// Get player location
			Vector3 playerLocation = player.transform.position;

			// Set movement step
			float moveStep = Velocity*Time.deltaTime;

			// Move towards player
			this.transform.position = Vector3.MoveTowards(this.transform.position,playerLocation,moveStep);

			//make sure the enemy stays on the ground plane
			//this.transform.SetPositionY(1);
		}
	}

	public void StopAndAttack () {
		NextAttack = NextAttack - Time.deltaTime;
		if (IsWithinAttackRange () && !waitingForAnimationDelay) {
			if(NextAttack <= 0){
				NextAttack = AttackRate;
				waitingForAnimationDelay = true;
				attackAnimationDelayTimer = AttackAnimationDelay;
				IsAttacking = true;
			}

			//Todo: Add stopping animation here when enemy is in range.
			//This'll fix the problem where the enemy plays the moving animation,
			//even if it is next to the player
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
