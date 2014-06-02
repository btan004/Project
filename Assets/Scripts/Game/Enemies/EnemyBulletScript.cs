using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {

	// Stats
	public float Velocity;
	public float Damage;
	public Vector3 Direction;
	public float Force;

	// Use this for initialization
	void Start () {


		//Bullet color
		this.renderer.material.color = Color.cyan;

		// Find player and move in that direction
		if (GameObject.FindGameObjectWithTag ("Player")) {
			// Get player location
			Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

			// Set as direction
			Direction = playerPosition - this.transform.position;
			Direction.Normalize();
		}
		else{
			Direction = Vector3.forward;
		}

		// Other stats
		Velocity = 10f;
	}
	
	// Update is called once per frame
	void Update () {
		// Move fowards
		this.transform.Translate(Direction * Velocity * Time.deltaTime);

		CheckLifeBound ();
	}

	public void CheckLifeBound (){
		GameObject map = GameObject.FindGameObjectWithTag ("Map");
		if (map) {
			// Get size of map
			Vector3 sizeOfMap = map.renderer.bounds.size;
			Vector3 centerOfMap = map.renderer.bounds.center;

			// Check if bullet outside map
			Vector3 currentPosition = this.transform.position;

			// First the X axis
			float xpositive = sizeOfMap.x/2 + centerOfMap.x;
			float xnegative = centerOfMap.x - sizeOfMap.x/2;
			if(currentPosition.x > xpositive || currentPosition.x < xnegative){
				if (this.gameObject) DestroyImmediate(this.gameObject);
			}

			// Next the Z axis
			float zpositive = sizeOfMap.z/2 + centerOfMap.z;
			float znegative = centerOfMap.z - sizeOfMap.z/2;
			if(currentPosition.z > zpositive || currentPosition.z < znegative){
				if (this) DestroyImmediate(this.gameObject);
			}
		}
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
			playerScript.AddKnockback(playerScript.transform.position - this.transform.position, Force);
			Destroy(gameObject);
		}
	}
}
