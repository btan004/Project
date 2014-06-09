using UnityEngine;
using System.Collections;

public enum LevelType
{
	Home,
	Boss,
	Level
};


public class LevelScript : MonoBehaviour {


	public LevelType CurrentLevelType;

	public GameObject Boundaries;
	public GameObject Lighting;
	public GameObject PlayerSpawnPoint;
	public GameObject Portal;
	public GameObject Traps;
	public GameObject Walls;
	
	public Rect LevelBounds;

	public bool TrapsEnabled;
	public bool EnemiesEnabled;
	public bool PowerupsEnabled;

	// Use this for initialization
	void Start () {
		//get our level bounds
		foreach (Component c in Boundaries.GetComponentsInChildren<Component>())
		{
			//Debug.LogError("Found: " + c.name);
			if (c.name == "Ground Plane")
			{
				LevelBounds.Set(
					c.transform.position.x - 0.5f * c.transform.localScale.x,
					c.transform.position.z - 0.5f * c.transform.localScale.z,
					c.transform.localScale.x,
					c.transform.localScale.z);
			}
			else continue;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentLevelType != LevelType.Home)
		{
			//if the wave is finished
			if (WaveSystem.WaveFinished)
			{
				//set the portal to active
				Portal.GetComponent<PortalScript>().IsActive = true;
				//Debug.LogError("Wave Finished and portal set to active");
			}
			else
			{
				//reset the portal to inactive
				Portal.GetComponent<PortalScript>().IsActive = false;
				//Debug.LogError("Wave Finished and portal set to inactive");
			}
		}
	}
}
