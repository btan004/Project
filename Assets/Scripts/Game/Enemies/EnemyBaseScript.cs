using UnityEngine;
using System.Collections;

public class EnemyBaseScript : MonoBehaviour {

	//Upgradable Enemy Stats
	public bool HasBeenUpgraded = false;
	public float Health = 100;
	public float Velocity = 10;
	public float Damage = 10;
	public float AttackRate = 2;
	public float Experience = 1;

	//Non-upgradable Enemy Stats
	protected float mass = 10;
	protected Vector3 knockback;

	public static PlayerScript player;

	// Use this for initialization
	public virtual void Start () {
		//print ("This is the base class");
		knockback = new Vector3();
		if (!player) AssignPlayer();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		//Check enemy health
		CheckHealth();
	}

	// Health checker
	public virtual void CheckHealth () {
		if (Health <= 0) {
			// Call death of enemy
			Death();
		}
	}

	// Death of enemy
	public virtual void Death() {
		//Give exp to player
		player.ApplyExperience(Experience);

		//decrement the enemy count
		WaveSystem.EnemiesRemaining--;

		//Destroy object
		DestroyObject (this.gameObject);


	}

	public virtual void ApplyDamage(float damage)
	{
		Health -= damage;
	}

	public virtual void AddKnockback(Vector3 direction, float force)
	{
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
		this.Experience = upgrade.Experience;
		this.HasBeenUpgraded = true;
	}
}
