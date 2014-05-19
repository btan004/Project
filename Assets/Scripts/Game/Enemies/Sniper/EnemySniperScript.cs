using UnityEngine;
using System.Collections;

public class EnemySniperScript : EnemyBaseScript {

	//Enemy Movement
	public float TurnVelocity;
	
	//Enemy Attack
	public float NextAttack;
	public float AttackDistance;
	public GameObject EnemyBulletPrefab;

	//Mecanim Animator
	public Animator anim;

	//State Machine for this enemy
	public FSM<EnemySniperScript> StateMachine;
	public enum StateID{ moving, attacking };

	//Function to handle statemachine
	public void ChangeState( State<EnemySniperScript> s )
	{
		StateMachine.ChangeState ( s );
	}
	
	// Use this for initialization
	public override void Start () {
		//Initialize state machine and animator
		StateMachine = new FSM<EnemySniperScript> ( this, Sniper_MoveToPlayer.Instance );
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
		NextAttack = AttackRate;
		AttackDistance = 10;

		//misc
		renderer.material.color = new Color(1f, 165f / 255f, 0f);
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
		
		// If within a certain distance stop and attack player
		//StopAndAttack ();

		// Animate
		//AnimateSkeleton(IsHit, IsAttacking, IsMoving);
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

	public void MoveEnemy() {
		// Find player in game
		if (!IsWithinAttackRange ())
			IsMoving = true;


		if (player && IsMoving && !IsWithinAttackRange() ) {
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

	public void StopAndAttack (){
		if (IsWithinAttackRange ()) {
			NextAttack = NextAttack - Time.deltaTime;
			if(NextAttack <=0){

				// Create Bullet
				Transform bulletPosition = this.transform;
				bulletPosition.SetPositionY(1);
				GameObject bullet = Instantiate(EnemyBulletPrefab, bulletPosition.position,Quaternion.identity) as GameObject;
				bullet.GetComponent<EnemyBulletScript>().SetDamage(Damage);
				NextAttack = AttackRate;
			}
		}
	}

}
