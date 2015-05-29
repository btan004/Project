using UnityEngine;
using System.Collections;

public class EnemyBufferScript : EnemyBaseScript {
	
	//Enemy Movement
	public float TurnVelocity;
	public float MovementRadius;
	public float MinDistanceAwayFromPlayer;
	public float MaxDistanceAwayFromPlayer;
	
	//Enemy Attack

	//Buffing properties
	//Total healing is calculated by BuffPerSec * BuffingInterv
	public float BuffPerSec = 25;			//Power of the healing
	public float BuffingRadius = 6;			//Area of effect for the aura
	public float BuffingInterval = 3;		//How long will the healing aura be active
	public float BuffingActiveTime = 0;		//How long has the healing aura been active
	public float BuffingCooldown = 15;		//Time before healing aura is active again
	public float BuffingCurrentCooldown = 0;

	//Preset buffs
	public EnemyBuff damageBuff = new EnemyBuff (BuffType.Damage, 1f, 5, 5);
	public EnemyBuff attackRateBuff = new EnemyBuff (BuffType.AttackRate, 2f, 5, 5);
	public EnemyBuff velocityBuff = new EnemyBuff (BuffType.Velocity, 3f, 5, 5);

	public Animator anim;

	public FSM<EnemyBufferScript> StateMachine;
	public GameObject targetLocation;

	public void ChangeState( State<EnemyBufferScript> s )
	{
		StateMachine.ChangeState ( s );
	}

	// Use this for initialization
	public override void Start () {
		if (!player) AssignPlayer();
		StateMachine = new FSM<EnemyBufferScript> ( this, Buffer_SelectTarget.Instance );
		anim = GetComponent<Animator>();	

		WaveSystem.EnemiesRemaining++;
		
		if (!HasBeenUpgraded)
		{
			baseHealth = 1;
			baseMaxHealth = 1;
			baseVelocity = 4;
			baseDamage = 1;
			baseAttackRate = 5;
			
			Health = baseHealth;
			MaxHealth = baseMaxHealth;
			Velocity = baseVelocity;
			Damage = baseDamage;
			AttackRate = baseAttackRate;
		}
		ScoreValue = 300;
		
		// Movement
		IsMoving = true;
		Velocity = 2f;
		TurnVelocity = 3f;
		MinDistanceAwayFromPlayer = 6f;
		MaxDistanceAwayFromPlayer = 8f;

		//Setting Buffing properties
		BuffPerSec = 25;
		BuffingRadius = 6;
		BuffingInterval = 3;
		BuffingActiveTime = 0;
		BuffingCooldown = 5;
		BuffingCurrentCooldown = 5;

		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			s.enableEmission = false;
		}

		if( renderers.Length <= 0 )
		{
			Debug.LogWarning("[EnemyBufferScript]: No renderers are set in order to flash this enemy when they are hit. If this is intentional, ignore");
		}

		mass = 20;
	}
	
	// Update is called once per frame
	public override void Update () {
		// Check enemy health, if <=0 die
		CheckHealth ();
		
		// Move Enemy
		//MoveEnemy ();
		
		if( targetLocation )
		{
			RotateTowards( targetLocation );
		}

		if( Health > 0 )
		{
			StateMachine.Update ();
		}
		Buff ();
	}

	public void Buff()
	{
		if( BuffingCurrentCooldown <= 0 )
		{
			Collider[] nearObjects = Physics.OverlapSphere (this.transform.position, BuffingRadius);
			foreach( Collider obj in nearObjects )
			{
				if( obj.tag == "Enemy" )
				{
					EnemyBaseScript enemy = obj.GetComponent<EnemyBaseScript>();
					if( enemy != null )
					{
						enemy.ApplyBuff( velocityBuff );
						enemy.ApplyBuff( attackRateBuff );
					}
				}
			}
			BuffingActiveTime += Time.deltaTime;
			if( BuffingActiveTime > BuffingInterval )
			{
				BuffingCurrentCooldown = BuffingCooldown;
				BuffingActiveTime = 0;
			}
		}
		else
		{
			BuffingCurrentCooldown -= Time.deltaTime;
			//Debug.Log ("Cooling down: " + BuffingCurrentCooldown);
		}
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			s.enableEmission = (bool)( BuffingActiveTime > 0 );
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

	/// <summary>
	/// Rotates towards the given GameObject
	/// </summary>
	/// <param name="obj">GameObject to rotate towards.</param>
	public void RotateTowards( GameObject obj ) {
		if (obj) 
		{
			// Get player location
			Vector3 objectLocation = obj.transform.position;
			
			// Set rotation step
			float rotationStep = TurnVelocity*Time.deltaTime;
			
			// Rotate enemy towards player
			Vector3 objectDir = Vector3.RotateTowards( this.transform.forward, objectLocation-this.transform.position, rotationStep, 0.0f);

			//Set y vector to 0 since we don't want to do anything with the y axis
			objectDir.y = 0;

			this.transform.rotation = Quaternion.LookRotation(objectDir);
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

	/// <summary>
	/// Determines whether this instance is within a certain range.
	/// </summary>
	/// <param name="obj"> obj to check within range of </para>
	/// <param name="range"> distance of check </para>
	/// <returns><c>true</c> if this instance is within range; otherwise, <c>false</c>. Also <c>false</c> if
	/// obj does not exist.
	/// </returns>
	public bool IsWithinRange( GameObject obj, float range )
	{
		// Find object in game
		if ( obj )
		{
			// Get player location
			Vector3 objectLocation = obj.transform.position;
			
			// Get distance between this instance and obj
			float distance = Vector3.Distance (objectLocation, this.transform.position);
			
			// Return if within range
			return (distance <= range);
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Changes state to SelectTarget after a given amount of seconds
	/// </summary>
	/// <param name="e">BufferScript.</param>
	/// <param name="seconds">Seconds.</param>
	public IEnumerator SelectOtherTarget( float seconds )
	{
		if( !Buffer_MoveToTarget.Instance.counting )
		{
			Buffer_MoveToTarget.Instance.counting = true;
			float countdownValue = seconds;
			while( countdownValue > 0 )
			{
				yield return new WaitForSeconds(1.0f);
				--countdownValue;
				Debug.Log (countdownValue);
			}
			GetComponent<NavMeshAgent> ().Stop ();
			ChangeState (Buffer_SelectTarget.Instance);
			Buffer_MoveToTarget.Instance.counting = false;
		}
	}
}
