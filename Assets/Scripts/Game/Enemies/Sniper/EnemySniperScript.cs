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
	public StateID CurrentState;

	//Function to handle statemachine
	public void ChangeState( State<EnemySniperScript> s )
	{
		if(s.GetType().Name == "Sniper_AttackPlayer"){
			CurrentState = StateID.attacking;
		}
		if(s.GetType().Name == "Sniper_MoveToPlayer"){
			CurrentState = StateID.moving;
		}
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
			baseHealth = 1;
			baseMaxHealth = 1;
			baseVelocity = 1;
			baseDamage = 1;
			baseAttackRate = 5;
			
			Health = baseHealth;
			MaxHealth = baseMaxHealth;
			Velocity = baseVelocity;
			Damage = baseDamage;
			AttackRate = baseAttackRate;
		}
		ScoreValue = 100;
		mass = 20;

		// Movement
		IsMoving = true;
		TurnVelocity = 5f;

		// Attack
		IsAttacking = false;
		NextAttack = AttackRate;
		AttackDistance = 10;

		RefreshRendererInfo();

		if (isBoss)
		{
			GameGUI.Boss = this;
			GameGUI.BossActive = true;
		}

		//Checking if there are any renderers to flash when this enemy is hit
		if( renderers.Length <= 0 )
		{
			Debug.LogWarning("[EnemySniperScript]: No renderers are set in order to flash this enemy when they are hit. If this is intentional, ignore");
		}

		//misc
		renderer.material.color = new Color(1f, 165f / 255f, 0f);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		// Check enemy health, if <=0 die
		CheckHealth ();
		
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

	public void RotateEnemy() {
		if (player)
		{
			switch(CurrentState){
				case StateID.moving:
					// Get velocity path
					Vector3 rotateDir = this.GetComponent<NavMeshAgent>().velocity;
					if( this.GetComponent<NavMeshAgent>().velocity == Vector3.zero )
					{
						rotateDir = this.transform.forward;
					}
				
					// Set rotation step
					float rotationStep = 5f*Time.deltaTime;
				
					// Rotate enemy towards player
					rotateDir = new Vector3(rotateDir.x,0,rotateDir.z);
					this.transform.rotation = Quaternion.LookRotation(rotateDir);
					break;

				case StateID.attacking:
					// Get player location
					Vector3 playerLocation = player.transform.position;
				
					// Set rotation step
					float rStep = TurnVelocity*Time.deltaTime;
				
					// Rotate enemy towards player
					Vector3 playerDir = Vector3.RotateTowards(this.transform.forward,playerLocation-this.transform.position,rStep,0.0f);
					playerDir = new Vector3(playerDir.x,0,playerDir.z);
					this.transform.rotation = Quaternion.LookRotation(playerDir);
					break;

				default:
					break;
			}
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
