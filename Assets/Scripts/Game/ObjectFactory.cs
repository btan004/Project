using UnityEngine;
using System.Collections;

public class ObjectFactory : MonoBehaviour {

	protected static ObjectFactory instance;

	//Our objects that we can create
	public GameObject PowerupPrefab;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	public static PowerupScript CreatePowerup(PowerupType type, int amount, int duration, int lifetime, Vector3 position)
	{
		PowerupScript powerup = (Object.Instantiate(instance.PowerupPrefab, position, Quaternion.identity) as GameObject).GetComponent<PowerupScript>();
		powerup.Type = type;
		powerup.Amount = amount;
		powerup.Duration = duration;
		powerup.Lifetime = lifetime;

		return powerup;
	}
}
