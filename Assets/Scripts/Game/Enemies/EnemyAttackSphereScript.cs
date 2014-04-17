using UnityEngine;
using System.Collections;

public class EnemyAttackSphereScript : MonoBehaviour {

	// Stats
	public float Damage;

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

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "PlayerHitbox"){
			// Apply damage to player and destroy self
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			PlayerScript playerScript = player.GetComponent<PlayerScript>();
			playerScript.ApplyDamage(Damage);
			Destroy(gameObject);
		}
	}
}
