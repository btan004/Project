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
				if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Home)
				{
					if (other.GetComponent<PortalScript>().IsActive)
					{
						MapSystemScript.instance.TransitionToLevel(other.GetComponent<PortalScript>().Destination);
						other.GetComponent<PortalScript>().IsActive = false;

						if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Level)
						{
							SpawnScript.SpawnWave = true;
						}
					}
				}
				else if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Level)
				{
					if (other.GetComponent<PortalScript>().IsActive && WaveSystem.WaveFinished)
					{
						MapSystemScript.instance.TransitionToLevel(other.GetComponent<PortalScript>().Destination);
					}
				}
				else if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Boss)
				{
					if (other.GetComponent<PortalScript>().IsActive)
					{
						MapSystemScript.instance.GetHomeLevel().GetComponent<HomeScript>().ResetHome();
						MapSystemScript.instance.TransitionToLevel(other.GetComponent<PortalScript>().Destination);
					}
				}
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
