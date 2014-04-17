using UnityEngine;
using System.Collections;

public class EnemyAttackSphereScript : MonoBehaviour {

	// Stats
	public float Damage;
	public float Force;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Set damage of bullets
	public void SetDamage(float dmg){
		Damage = dmg;
	}

	public void SetForce(float force)
	{
		Force = force;
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "PlayerHitbox"){
			// Apply damage to player and destroy self
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			PlayerScript playerScript = player.GetComponent<PlayerScript>();
			playerScript.ApplyDamage(Damage);
			playerScript.AddKnockback(playerScript.transform.position - this.transform.position, Force);
			Destroy(gameObject);
		}
	}
}
