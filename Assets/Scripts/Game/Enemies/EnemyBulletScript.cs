using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {

	// Stats
	public float Velocity;
	public float Damage;
	public Vector3 Direction;
	public float Force;
	public bool isBoss;
	public Vector3 initialDirection;
	public float timer;

	// Use this for initialization
	void Start () {
		//Bullet color
		this.renderer.material.color = Color.cyan;

		// Set initial bullet direction
		MoveBullet();

		// Other stats
		Velocity = 10f;

		timer = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		// Move fowards
		this.transform.Translate(Direction * Velocity * Time.deltaTime);

		CheckLifeBound ();

		//Update bullet direction
		UpdateBullet();
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

	public void MoveBullet(){
		if(!isBoss){
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
		}
		else{
			if(GameObject.FindGameObjectWithTag("Player")){
				// Move in the initial direction first
				Direction = initialDirection;
			}
		}
	}

	public void UpdateBullet(){
		if(isBoss){
			timer = timer - Time.deltaTime;
			if (timer <= 0){
				if (GameObject.FindGameObjectWithTag ("Player")) {
					// Get player location
					Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
				
					// Set as direction
					Direction = playerPosition - this.transform.position;
					Direction.Normalize();

					//Turn off boss
					isBoss = false;
				}
			}
		}
	}

	public void SetBossBullet(bool boss){
		isBoss = boss;
	}

	public void SetInitialDirection(Vector3 dir){
		initialDirection = dir;
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
