using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour {

	public GameObject Boundaries;
	public GameObject Lighting;
	public GameObject PlayerSpawnPoint;
	public GameObject Portal;
	public GameObject Traps;
	public GameObject Walls;
	
	public Rect LevelBounds;

	public bool TrapsEnabled;
	public bool EnemiesEnabled;

	// Use this for initialization
	void Start () {
		//get our level bounds
		foreach (Component c in Boundaries.GetComponentsInChildren<Component>())
		{
			//Debug.LogError("Found: " + c.name);
			if (c.name == "Ground Plane")
			{
				LevelBounds.Set(
					c.transform.position.x,
					c.transform.position.z,
					c.transform.localScale.x,
					c.transform.localScale.z);
			}
			else continue;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
