using UnityEngine;
using System.Collections;

public class EnemyBaseScript : MonoBehaviour {

	//Enemy Stats
	public float Health = 100;
	public int ExperienceToGive = 1;

	// Use this for initialization
	public virtual void Start () {
		print ("This is the base class");
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
	                 
}
