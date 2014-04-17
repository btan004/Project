using UnityEngine;
using System.Collections;

public class EnemyBaseScript : MonoBehaviour {

	//Enemy Stats
	public float Health = 100;
	protected float mass = 10;
	public int ExperienceToGive = 1;
	protected Vector3 knockback;

	// Use this for initialization
	public virtual void Start () {
		//print ("This is the base class");
		knockback = new Vector3();
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
}
