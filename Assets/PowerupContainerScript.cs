using UnityEngine;
using System.Collections;

public class PowerupContainerScript : MonoBehaviour
{

	public static PowerupContainerScript instance;

	// Use this for initialization
	void Start()
	{
		instance = this;

	}

	public void DeleteAllPowerups()
	{
		foreach (PowerupScript powerup in instance.GetComponentsInChildren<PowerupScript>())
		{
			Destroy(powerup.gameObject);
		}

	}
}
