using UnityEngine;
using System.Collections;

public class EnemyChargerScript : EnemyBaseScript {

	//Enemy Movement
	public bool MovingEnabled;
	public float TurnVelocity;
	
	//Enemy Attack
	public bool EnemyIsAttacking;
	public float Force;
	public float NextAttack;
	public float AttackDistance;
	public GameObject EnemyAttackSphere;
	
	public bool ChargeReady;				//Ready to charge?
	public bool IsGettingReadyToCharge;		//Preparing to charge
	public bool IsCharging;					//Currently charging towards target
	public bool IsResting;					//Is it resting after a charge?
	public float ChargeCooldown;			//Time before this can charge a target again
	public float ChargeCooldownCounter;		//Counter for above
	public float TimeUntilCharge;			//Time to get ready for a charge
	public float TimeUntilChargeCounter;	//Counter for above
	public float RestingTime;				//Time to stay inactive after a charge
	public float RestingTimeCounter;		//Counter for above
	public float ChargeVelocity;			//Charging speed
	public float MinDistanceToCharge;		//Minimum distance between target to initiate a charge
	public Vector3 ChargeTarget;			//Target to charge at

	// Use this for initialization
	public override void Start () {
		if (!player) AssignPlayer();
		WaveSystem.EnemiesRemaining++;

		// Set initial stats (should ovaeride by applying an upgrade)
		if (!HasBeenUpgraded)
		{
			Health = 1;
			Velocity = 1;
			Damage = 1;
			AttackRate = 5;
			Experience = 1;
		}

		// Movement
		MovingEnabled = true;
		TurnVelocity = 5f;

		// Attack
		EnemyIsAttacking = false;
		AttackDistance = 2;
		NextAttack = AttackRate;

		ChargeReady = false;
		IsGettingReadyToCharge = false;
		IsCharging = false;
		IsResting = false;
		ChargeCooldown = 5;
		ChargeCooldownCounter = 0;
		TimeUntilCharge = 3;
		TimeUntilChargeCounter = 0;
		RestingTime = 1;
		RestingTimeCounter = 0;
		ChargeVelocity = 75;
		MinDistanceToCharge = 10;

		//Knockback
		Force = 5f;
		mass = 20;

		//misc
		renderer.material.color = Color.green;
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			s.enableEmission = false;
		}
	}
	
	// Update is called once per frame
	public override void Update () {
		// Reset animation info
		ClearAnimationInfo();

		// Check enemy health, if <=0 die
		CheckHealth ();

		AIDecision ();

		// Animate
		AnimateSkeleton(IsHit, IsAttacking, IsMoving);
	}

	public void AIDecision()
	{
		if (player) {
			// Get player location
			Vector3 playerLocation = player.transform.position;
			
			// Get distance between player and enemy
			float distance = Vector3.Distance (playerLocation, this.transform.position);

			if( ChargeReady && distance >= MinDistanceToCharge && !IsGettingReadyToCharge && !IsCharging && !IsResting )
			{
				float ChanceToCharge = Random.Range(0.0f, 100.0f);
				if( ChanceToCharge <= 5.0 )
				{
					IsGettingReadyToCharge = true;
					ChargeAttack();
					RotateEnemy();
					ChargeReady = false;
					IsMoving = true;
				}
			}
			else if( IsGettingReadyToCharge || IsCharging )
			{
				ChargeAttack ();
				RotateEnemy();
			}
			else if( IsResting )
			{
				RestingTimeCounter += Time.deltaTime;
				if( RestingTimeCounter >= RestingTime )
				{
					IsResting = false;
					RestingTimeCounter = 0;
					MovingEnabled = true;
				}
			}
			else
			{
				MoveEnemy();
				ApplyKnockback();
				RotateEnemy();
				//StopAndAttack();

				if( ChargeCooldownCounter >= ChargeCooldown )
				{
					ChargeReady = true;
					ChargeCooldownCounter = ChargeCooldown;
				}
				else
				{
					ChargeCooldownCounter += Time.deltaTime;
				}
			}
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
		if (player) {
			// Get player location
			Vector3 playerLocation = player.transform.position;

			// Set rotation step
			float rotationStep = TurnVelocity*Time.deltaTime;
		
			// Rotate enemy towards player
			Vector3 playerDir = Vector3.RotateTowards(this.transform.forward,playerLocation-this.transform.position,rotationStep,0.0f);
			playerDir = new Vector3(playerDir.x,0,playerDir.z);
			this.transform.rotation = Quaternion.LookRotation(playerDir);
			EnemyAnimation.transform.rotation = Quaternion.LookRotation(playerDir);
		}
	}

	public void MoveEnemy() {
		// Find player in game
		if (player && MovingEnabled && !IsWithinAttackRange()) {
			// Get player location
			Vector3 playerLocation = player.transform.position;

			// Set movement step
			float moveStep = Velocity*Time.deltaTime;

			float y = this.transform.position.y;

			// Move towards player
			this.transform.position = Vector3.MoveTowards(this.transform.position,playerLocation,moveStep);

			IsMoving = true;

			//make sure the enemy stays on the ground plane
			this.transform.SetPositionY(y);
		}
	}

	public void StopAndAttack () {
		NextAttack = NextAttack - Time.deltaTime;
		if (IsWithinAttackRange ()) {
			if(NextAttack <= 0){
				NextAttack = AttackRate;

				// Create sphere attack
				Vector3 createPosition = transform.position + transform.forward;
				GameObject attack = Instantiate(EnemyAttackSphere) as GameObject;
				attack.transform.position = createPosition;
				attack.GetComponent<EnemyAttackSphereScript>().SetDamage(Damage);
				attack.GetComponent<EnemyAttackSphereScript>().SetForce(Force);
			}
		}
	}

	public void ChargeAttack()
	{
		//Getting ready to charge at player..
		if( IsGettingReadyToCharge )
		{
			TimeUntilChargeCounter += Time.deltaTime;
			if( TimeUntilChargeCounter >= TimeUntilCharge )
			{
				IsCharging = true;
				IsGettingReadyToCharge = false;
				TimeUntilChargeCounter = 0;
				ChargeTarget.x = player.transform.position.x;
				ChargeTarget.y = this.transform.position.y;
				ChargeTarget.z = player.transform.position.z;
				Debug.Log("Charging to position: " + ChargeTarget.x + "," + ChargeTarget.y + "," + ChargeTarget.z);
			}

			foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
			{
				if( s.name == "ChargeupParticles" )
				{
					s.enableEmission = IsGettingReadyToCharge;
				}
			}
		}

		else
		{
			float ChargeStep = ChargeVelocity*Time.deltaTime;
			this.transform.position = Vector3.MoveTowards( this.transform.position, ChargeTarget, ChargeStep );

			//Charge has reached it's target position
			if( this.transform.position == ChargeTarget )
			{
				IsCharging = false;
				IsResting = true;
				MovingEnabled = false;
				ChargeCooldownCounter = 0;
			}

			foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
			{
				if( s.name == "TrailParticles" )
				{
					s.enableEmission = true;
				}
			}
		}
	}

	public void ApplyChargeHit()
	{
		Vector3 createPosition = transform.position + transform.forward;
		GameObject attack = Instantiate(EnemyAttackSphere) as GameObject;
		attack.transform.position = createPosition;
		attack.GetComponent<EnemyAttackSphereScript>().SetDamage(40);
		attack.GetComponent<EnemyAttackSphereScript>().SetForce(20);
	}



	public void OnCollisionEnter( Collision collision )
	{
		if( collision.gameObject.layer == Globals.PLAYER_LAYER )
		{
			if( IsCharging )
			{
				ApplyChargeHit();
			}
		}
	}
}