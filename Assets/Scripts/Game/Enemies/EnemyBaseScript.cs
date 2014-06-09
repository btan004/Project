using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBaseScript : MonoBehaviour {
	
	// Spawner 
	public bool IsSpawned;
	public GameObject ParentSpawner;
	
	//Upgradable Enemy Stats
	public bool HasBeenUpgraded = false;
	public float Health = 100;
	public float MaxHealth = 100;
	public float Velocity = 10;
	public float Damage = 10;
	public float AttackRate = 2;
	public float ScoreValue;

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

		RefreshRendererInfo();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		//Check enemy health
		CheckHealth();


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

		//Destroy object
		DestroyObject (this.gameObject);
	}

	// Check if enemy out of bounds
	public void outOfBounds(){
		GameObject mapSystem = GameObject.FindGameObjectWithTag ("MapContainer");
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
		//StartCoroutine(Flash(0.2f, Color.red));
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
		this.Health = upgrade.Health;
		this.Velocity = upgrade.Velocity;
		this.Damage = upgrade.Damage;
		this.AttackRate = upgrade.AttackRate;
		this.HasBeenUpgraded = true;
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

	/// <summary>
	/// Flashes this enemy when they are hit with a color
	/// </summary>
	/// <param name="time">How long the given color persists on the enemy</param>
	/// <param name="color">Color to flash</param>
	/// <returns>
	/// IEnumerator back to the function for yield
	/// </returns>
	/// <remarks>
	/// This function should be called within scripts that are applying damage to the enemy.
	/// E.g, when this enemy is damaged by a melee attack, this function is called within MeleeAttackBoxScript.cs
	/// </remarks>
	public IEnumerator Flash( float time, Color color )
	{
		Dictionary<Material, Color> colorDefs = new Dictionary<Material, Color> ();
		Dictionary<Material, Shader> shaderDefs = new Dictionary<Material, Shader> ();
		foreach ( Renderer r in renderers )
		{
			foreach ( Material m in r.materials )
			{
				Debug.Log ( "Material is: " + m.name );
				if( m.HasProperty("_Color") && m.color != color )
				{
					colorDefs.Add( m, m.color );
					m.color = color;
				}
				if( m.shader != Shader.Find ("Transparent/Diffuse") )
				{
					shaderDefs.Add ( m, m.shader );
					m.shader = Shader.Find("Transparent/Diffuse");
				}
			}
		}
		yield return new WaitForSeconds (time);
		
		foreach ( KeyValuePair<Material, Color> entry in colorDefs )
		{
			entry.Key.color = entry.Value;
		}
		foreach ( KeyValuePair<Material, Shader> entry in shaderDefs )
		{
			entry.Key.shader = entry.Value;
		}
	}
}
