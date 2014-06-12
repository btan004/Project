using UnityEngine;
using System.Collections;

public class TrapContainerScript : MonoBehaviour {

	public static TrapContainerScript instance;

	// Use this for initialization
	void Start()
	{
		instance = this;

	}

	void Update()
	{
		if (WaveSystem.WaveFinished)
		{
			DeleteAllTraps();
		}
	}

	public void DeleteAllTraps()
	{
		foreach (SlimeTrapScript slimeTrap in instance.GetComponentsInChildren<SlimeTrapScript>())
		{
			Destroy(slimeTrap.gameObject);
		}
		foreach (LandmineScript landmineTrap in instance.GetComponentsInChildren<LandmineScript>())
		{
			Destroy(landmineTrap.gameObject);
		}
	}
}
