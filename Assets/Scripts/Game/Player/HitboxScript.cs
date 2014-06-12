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
				//if we are in the home level
				if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Home)
				{
					//and the portal we entered is active
					if (other.GetComponent<PortalScript>().IsActive)
					{
						//transition to the destination
						MapSystemScript.instance.TransitionToLevel(other.GetComponent<PortalScript>().Destination);
						other.GetComponent<PortalScript>().IsActive = false;

						//if the destination was a level
						if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Level)
						{
							//for each game object in the level
							foreach (Component c in MapSystemScript.instance.GetCurrentLevel().GetComponentsInChildren<Component>())
							{
								//if it is a portal, set it false
								if (c.name == "Portal")
								{
									c.GetComponent<PortalScript>().IsActive = false;	
								}
							}
							//and start the level music
							AudioManagerScript.instance.StartLevelMusic();
						}
						else
						{
							//boss level
							AudioManagerScript.instance.StartBossMusic();
						}
					}
				}
				//else if we are in a level
				else if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Level)
				{
					//and the portal is active with 0 enemies remaining
					if (other.GetComponent<PortalScript>().IsActive && EnemyContainerScript.instance.GetEnemyCount() == 0)
					{
						//transition to the home level
						MapSystemScript.instance.TransitionToLevel(other.GetComponent<PortalScript>().Destination);
						//and start the home music
						AudioManagerScript.instance.StartHomeMusic();
					}
				}
					//else if we are in the boss level
				else if (MapSystemScript.instance.GetCurrentLevelType() == LevelType.Boss)
				{
					if (other.GetComponent<PortalScript>().IsActive)
					{
						MapSystemScript.instance.GetHomeLevel().GetComponent<HomeScript>().ResetHome();
						MapSystemScript.instance.TransitionToLevel(other.GetComponent<PortalScript>().Destination);
						AudioManagerScript.instance.StartHomeMusic();
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
