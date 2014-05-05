using UnityEngine;
using System.Collections;

public class HitboxScript : MonoBehaviour {

	public PlayerScript player;

	// Use this for initialization
	void Start () {
		transform.position = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position;
	}

	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "Landmine":
				other.gameObject.GetComponent<LandmineScript>().ActivateTrap(player);
				break;
			case "SlimeTrap":
				other.gameObject.GetComponent<SlimeTrapScript>().ActivateTrap(player);
				break;
			case "Portal":
				if (WaveSystem.WaveFinished) WaveSystem.ForceSpawnWave = true;
				break;
			default:
				break;
		}
	}

	void OnTriggerStay(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "FireTrap":
				other.gameObject.GetComponent<FireTrapScript>().ActivateTrap(player);
				break;
			default:
				break;
		}
	}
}
