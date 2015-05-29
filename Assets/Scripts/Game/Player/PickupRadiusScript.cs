using UnityEngine;
using System.Collections;

public class PickupRadiusScript : MonoBehaviour {

	public PlayerScript player;

	// Use this for initialization
	void Start () {
		transform.position = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position;
		//this.gameObject.GetComponent<SphereCollider>().radius = player.Radius;
	}

	void OnTriggerEnter(Collider other)
	{
		//if the player picked up a powerup
		if (other.gameObject.tag == "Powerup")
		{
			//get the powerup instance
			PowerupScript powerup = other.gameObject.GetComponent<PowerupScript>();

			//apply the powerup to our player
			player.ApplyPowerup(new Powerup(powerup));

			//then destroy the powerup
			Destroy(other.gameObject);
		}
	}
}
