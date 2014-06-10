using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBaseScript : MonoBehaviour {
	
	// Spawner 
	public bool IsSpawned;
	public GameObject ParentSpawner;

	//Boss
	public bool isBoss;

	//Upgradable Enemy Stats
	public bool HasBeenUpgraded = false;
	public float baseHealth = 100;
	public float baseMaxHealth = 100;
	public float baseVelocity = 10;
	public float baseDamage = 10;
	public float baseAttackRate = 2;
	public float ScoreValue;

	//Stats that will be used
	//Will contain base stat + any modifiers to the stat.
	public float Health;
	public float MaxHealth;
	public float Velocity;
	public float Damage;
	public float AttackRate;
	public bool velocityBuffed;
	public bool damageBuffed;
	public bool healthBuffed;
	public bool maxHealthBuffed;
	public bool attackRateBuffed;

	//List of buffs
	public List<EnemyBuff> buffs = new List<EnemyBuff> ();

	//Non-upgradable Enemy Stats
	protected float mass = 10;
	protected Vector3 knockback;

	// Animations
	public Animation EnemyAnimation;
	public bool IsMoving;
	public bool IsAttacking;
	public bool IsHit;
	public bool IsDead;

	// GameObjects with Renderers
	// Used for flashing an enemy when they are hit.
	// THIS MUST BE SET IN THE INSPECTOR IN ORDER FOR IT TO WORK.
	public Renderer[] renderers;

	//Dictionaries to store colors/shaders and return them to normal when flashing
	Dictionary<Material, Color> colorDefs = new Dictionary<Material, Color>();
	Dictionary<Material, Shader> shaderDefs = new Dictionary<Material, Shader>();
	public static Color FlashColor;
	public static Color SpawnedColor;
	public static float FlashDuration = 0.2f;
	private float flashTimer;
	private bool isFlashing = false;

	private GameObject mapSystem;


	public bool applyABuff = false;


	public void ClearAnimationInfo()
	{
		IsMoving = false;
		IsAttacking = false;
		IsHit = false;
	}

	public void AnimateSkeleton(bool isHit, bool isAttacking, bool isMoving)
	{
		if (IsHit)
		{
			EnemyAnimation.Play("gethit");
		}
		else if (IsAttacking && !EnemyAnimation.IsPlaying("attack"))
		{
			EnemyAnimation.Play("attack");
		}
		else if (IsMoving && !EnemyAnimation.IsPlaying("run") && !EnemyAnimation.IsPlaying("attack"))
		{
			EnemyAnimation.Play("run");
		}
		else if (!EnemyAnimation.isPlaying)
		{
			EnemyAnimation.Play("idle");
		}
	}

	public static PlayerScript player;

	// Use this for initialization
	public virtual void Start () {
		IsSpawned = false;
		//print ("This is the base class");
		knockback = new Vector3();
		if (!player) AssignPlayer();

		mapSystem = GameObject.FindGameObjectWithTag("MapContainer");

		Health = baseHealth;
		MaxHealth = baseMaxHealth;
		Velocity = baseVelocity;
		Damage = baseDamage;
		AttackRate = baseAttackRate;

		RefreshRendererInfo();

		if (isBoss)
		{
			GameGUI.Boss = this;
			GameGUI.BossActive = true;
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		//Check enemy health
		CheckHealth();

		//if (isBoss)
		//{
		//	GameGUI.Boss = this;
		//	GameGUI.BossActive = true;
		//}

		flashTimer -= Time.deltaTime;
		if (isFlashing && flashTimer <= 0)
		{
			if(!IsSpawned){
				FlashReturnToNormal();
			}
			else{
				FlashSpawned();
			}
			isFlashing = false;
		}

		//Check if enemy is outside of map
		outOfBounds ();

		UpdateBuffs ();

		if( applyABuff )
		{
			applyABuff = false;
			buffs.Add( new EnemyBuff( BuffType.Velocity, 4, 3, 3 ) );
		}
	}

	// Health checker
	public virtual void CheckHealth () {
		if (Health <= 0 || transform.position.y < 0) {
			// Call death of enemy
			Death();
		}
	}

	// Death of enemy
	public virtual void Death() {
		// Check if spawner
		if(IsSpawned){
			ReportToSpawner();
		}
		//Give exp to player
		player.ApplyExperience(ScoreValue);

		//decrement the enemy count
		WaveSystem.EnemiesRemaining--;

		//
		if (isBoss)
		{
			GameGUI.BossActive = false;
			Debug.LogError("Boss Dead:" + GameGUI.BossActive);
		}
		//Destroy object
		DestroyObject (this.gameObject);
	}

	// Check if enemy out of bounds
	public void outOfBounds(){
		/*
		if(mapSystem!=null){	
			Rect level = mapSystem.GetComponent<MapSystemScript>().GetLevelBounds();
			Vector2 currentLocation = new Vector2 (this.transform.position.x, this.transform.position.z);
			if(!level.Contains(currentLocation)){
				Debug.Log("Out of bounds");
				this.Death();
			}
			else{
				Debug.Log("Not out of bounds");
			}
		}
		else{
			Debug.Log("Map is null");
		}
		 * */
	}

	// Spawner
	public virtual void ReportToSpawner(){
		if(ParentSpawner!=null){
			ParentSpawner.GetComponent<EnemySpawnerScript>().ReduceSpawns ();
		}
		else{
			print ("Parent Dead");
		}
	}

	public virtual void SetSpawner(GameObject Spawner){
		IsSpawned = true;
		ParentSpawner = Spawner;
	}

	public virtual void ApplyDamage(float damage)
	{
		Health -= damage;
		FlashTurnRed();
		flashTimer = FlashDuration;
		isFlashing = true;
	}

	public virtual void AddKnockback(Vector3 direction, float force)
	{
		direction = new Vector3 (direction.x,0,direction.z);
		knockback = direction * (force / mass);
	}
   
	protected virtual void ApplyKnockback()
	{
		transform.position = transform.position + knockback;
		
		knockback = Vector3.Lerp(knockback, Vector3.zero, 5 * Time.deltaTime);
	}

	protected void AssignPlayer()
	{
		player =  GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
	}

	public void ApplyUpgrade(EnemyUpgrade upgrade)
	{
		this.baseHealth = upgrade.Health;
		this.baseVelocity = upgrade.Velocity;
		this.baseDamage = upgrade.Damage;
		this.baseAttackRate = upgrade.AttackRate;
		this.HasBeenUpgraded = true;
		Health = baseHealth;
		MaxHealth = baseMaxHealth;
		Velocity = baseVelocity;
		Damage = baseDamage;
		AttackRate = baseAttackRate;
	}

	void OnTriggerStay(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "FireTrap":
				other.gameObject.GetComponent<FireTrapScript>().ActivateTrap(this);
				break;
			default:
				break;
		}
	}

	public void RefreshRendererInfo()
	{
		//Debug.LogError("Refreshing Renderer Info...");

		if (colorDefs == null) colorDefs = new Dictionary<Material, Color>();
		if (shaderDefs == null) shaderDefs = new Dictionary<Material, Shader>();

		FlashColor = Color.red;

		//store all of our renderer info
		foreach (Renderer r in renderers)
		{
			foreach (Material m in r.materials)
			{
				if (m.HasProperty("_Color"))
				{
					colorDefs.Add(m, m.color);
				}
				if (m.shader != Shader.Find("Transparent/Diffuse"))
				{
					shaderDefs.Add(m, m.shader);
				}
			}
		}
	}

	public void FlashTurnRed()
	{
		//Debug.LogWarning("Flashing red...");

		foreach (Renderer r in renderers)
		{
			foreach (Material m in r.materials)
			{
				if (m.HasProperty("_Color"))
				{
					m.color = FlashColor;
				}
				if (m.shader != Shader.Find("Transparent/Diffuse"))
				{
					m.shader = Shader.Find("Transparent/Diffuse");
				}
			}
		}
	}
	public void FlashSpawned()
	{	
		foreach (Renderer r in renderers)
		{
			foreach (Material m in r.materials)
			{
				if (m.HasProperty("_Color"))
				{
					m.color = SpawnedColor;
				}
				if (m.shader != Shader.Find("Transparent/Diffuse"))
				{
					m.shader = Shader.Find("Transparent/Diffuse");
				}
			}
		}
	}

	public void FlashReturnToNormal()
	{
		Debug.Log("Returning to normal...");

		//go through our dictionaries and return our renderer data to normal
		foreach (KeyValuePair<Material, Color> entry in colorDefs)
		{
			entry.Key.color = entry.Value;
		}
		foreach (KeyValuePair<Material, Shader> entry in shaderDefs)
		{
			entry.Key.shader = entry.Value;
		}
	}

	public void ApplyBuff( EnemyBuff b )
	{
		if( b.Type == BuffType.AttackRate && !attackRateBuffed )
		{
			attackRateBuffed = true;
			buffs.Add(b);
		}
		if( b.Type == BuffType.Damage && !damageBuffed )
		{
			damageBuffed = true;
			buffs.Add(b);
		}
		if( b.Type == BuffType.Velocity && !velocityBuffed )
		{
			velocityBuffed = true;
			buffs.Add(b);
		}
	}

	private void UpdateBuffs()
	{
		float damageBuffValue = baseDamage;
		float attackRateBuffValue = baseAttackRate;
		float velocityBuffValue = baseVelocity;
		foreach( EnemyBuff buff in buffs )
		{
			switch( buff.Type )
			{
				case BuffType.Damage:
					damageBuffValue = baseDamage + buff.Value;
					break;
				case BuffType.AttackRate:
					attackRateBuffValue = baseAttackRate / buff.Value;
					break;
				case BuffType.Velocity:
					velocityBuffValue = baseVelocity * buff.Value;
					break;
			}
			buff.Duration -= Time.deltaTime;
		}
		Damage = damageBuffValue;
		AttackRate = attackRateBuffValue;
		Velocity = velocityBuffValue;
		GetComponent<NavMeshAgent> ().speed = Velocity;
		buffs.RemoveAll (IsBuffAlive);
	}

	private bool IsBuffAlive(EnemyBuff b)
	{
		if( b.Duration <= 0 )
		{
			switch( b.Type )
			{
				case BuffType.Damage:
					damageBuffed = false;
					break;
				case BuffType.AttackRate:
					attackRateBuffed = false;
					break;
				case BuffType.Velocity:
					velocityBuffed = false;
					GetComponent<NavMeshAgent>().speed = baseVelocity;
					break;
			}
		}
		return (b.Duration <= 0);
	}
}
