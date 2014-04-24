using UnityEngine;
using System.Collections;

public class EnemyBaseScript : MonoBehaviour {

	// Spawner 
	public bool IsSpawned;
	public GameObject ParentSpawner;

	//Enemy Stats
	public float Health = 100;
	protected float mass = 10;
	public int ExperienceToGive = 1;
	protected Vector3 knockback;

	public static PlayerScript player;

	// Use this for initialization
	public virtual void Start () {
		IsSpawned = false;
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
		// Check if spawner
		if(IsSpawned){
			ReportToSpawner();
		}

		//Destroy object
		DestroyObject (this.gameObject);

		//Give exp to player
		player.ApplyExperience(ExperienceToGive);
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
}
