using UnityEngine;
using System.Collections;

public class EnemyHealerScript : EnemyBaseScript {
	
	//Enemy Movement
	public float TurnVelocity;
	public float MovementRadius;
	public float MinDistanceAwayFromPlayer;
	public float MaxDistanceAwayFromPlayer;
	
	//Enemy Attack

	//Healing properties
	//Total healing is calculated by HealPerSec * HealingInterv
	public float HealPerSec = 25;			//Power of the healing
	public float HealingRadius = 6;			//Area of effect for the aura
	public float HealingInterval = 3;		//How long will the healing aura be active
	public float HealingActiveTime = 0;		//How long has the healing aura been active
	public float HealingCooldown = 15;		//Time before healing aura is active again
	public float HealingCurrentCooldown = 0;

	public Animator anim;

	public FSM<EnemyHealerScript> StateMachine;

	public void ChangeState( State<EnemyHealerScript> s )
	{
		StateMachine.ChangeState ( s );
	}

	// Use this for initialization
	public override void Start () {
		if (!player) AssignPlayer();
		StateMachine = new FSM<EnemyHealerScript> ( this, Healer_MoveToPlayer.Instance );
		anim = GetComponent<Animator>();	

		WaveSystem.EnemiesRemaining++;
		
		// Set stats
		//MaxHealth = 100;
		//Health = 100;
		//Experience = 10;
		ScoreValue = 300;
		
		// Movement
		IsMoving = true;
		Velocity = 2f;
		TurnVelocity = 3f;
		MinDistanceAwayFromPlayer = 6f;
		MaxDistanceAwayFromPlayer = 8f;

		//Setting Healing properties
		HealPerSec = 25;
		HealingRadius = 6;
		HealingInterval = 3;
		HealingActiveTime = 0;
		HealingCooldown = 5;
		HealingCurrentCooldown = 5;

		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			s.enableEmission = false;
		}

		if( renderers.Length <= 0 )
		{
			Debug.LogWarning("[EnemyHealerScript]: No renderers are set in order to flash this enemy when they are hit. If this is intentional, ignore");
		}

		mass = 20;
	}
	
	// Update is called once per frame
	public override void Update () {
		// Check enemy health, if <=0 die
		CheckHealth ();
		
		// Move Enemy
		//MoveEnemy ();
		
		// Rotate enemy towards player
		RotateEnemy ();

		if( Health > 0 )
		{
			StateMachine.Update ();
		}
	}

	public void Heal()
	{
		if( HealingCurrentCooldown <= 0 )
		{
			Collider[] nearObjects = Physics.OverlapSphere (this.transform.position, HealingRadius);
			foreach( Collider obj in nearObjects )
			{
				if( obj.tag == "Enemy" )
				{
					EnemyBaseScript enemy = obj.GetComponent<EnemyBaseScript>();
					if( enemy != null )
					{
						enemy.Health += Time.deltaTime*HealPerSec;
						if( enemy.Health > enemy.MaxHealth )
						{
							enemy.Health = enemy.MaxHealth;
						}
					}
				}
			}
			HealingActiveTime += Time.deltaTime;
			if( HealingActiveTime > HealingInterval )
			{
				HealingCurrentCooldown = HealingCooldown;
				HealingActiveTime = 0;
			}

			//Debug.Log ("Healing for: " + Time.deltaTime*HealPerSec );
			//
		}
		else
		{
			HealingCurrentCooldown -= Time.deltaTime;
			//Debug.Log ("Cooling down: " + HealingCurrentCooldown);
		}
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			s.enableEmission = (bool)( HealingActiveTime > 0 );
		}
	}

	public bool IsTooFarFromPlayer()
	{
		if (player)
		{
			// Get player location
			Vector3 playerLocation = player.transform.position;
			
			// Get distance between player and enemy
			float distance = Vector3.Distance (playerLocation, this.transform.position);
			
			// Return if outside of maximum radius of player
			return (distance > MaxDistanceAwayFromPlayer);
		}
		else
		{
			return false;
		}
	}

	public bool IsTooCloseToPlayer()
	{
		if (player)
		{
			// Get player location
			Vector3 playerLocation = player.transform.position;
			
			// Get distance between player and enemy
			float distance = Vector3.Distance (playerLocation, this.transform.position);
			
			// Return if outside of maximum radius of player
			return (distance < MinDistanceAwayFromPlayer);
		}
		else
		{
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
			Vector3 playerDir = Vector3.RotateTowards( this.transform.forward, playerLocation-this.transform.position, rotationStep, 0.0f);

			//Set y vector to 0 since we don't want to do anything with the y axis
			playerDir.y = 0;

			this.transform.rotation = Quaternion.LookRotation(playerDir);
		}
	}
	
	public void MoveEnemy() {
		// Find player in game
		if (IsTooFarFromPlayer() || IsTooCloseToPlayer())
			IsMoving = true;

		// Get player location
		Vector3 playerLocation = player.transform.position;
		
		//Set y vector to 0 since we don't want to do anything with the y axis
		playerLocation.y = 0;
		
		// Set movement step
		float moveStep = Velocity*Time.deltaTime;

		if (player && IsMoving && IsTooFarFromPlayer() )
		{
			Vector3 MovingVector = Vector3.MoveTowards(this.transform.position,playerLocation, moveStep);
			MovingVector.y = this.transform.position.y;
			
			// Move towards player
			this.transform.position = MovingVector;

		}
		else if (player && IsMoving && IsTooCloseToPlayer() )
		{
			Vector3 MovingVector = Vector3.MoveTowards(this.transform.position,playerLocation, -moveStep);
			MovingVector.y = this.transform.position.y;

			// Move towards player
			this.transform.position = MovingVector;
		}
		
		// Set rotation step
		float rotationStep = TurnVelocity*Time.deltaTime;


		// Rotate enemy towards player
		Vector3 playerDir = Vector3.RotateTowards(this.transform.forward,playerLocation-this.transform.position,rotationStep,0.0f);
		playerDir = new Vector3(playerDir.x,0,playerDir.z);
		this.transform.rotation = Quaternion.LookRotation(playerDir);
		EnemyAnimation.transform.rotation = Quaternion.LookRotation(playerDir);
	}
}
