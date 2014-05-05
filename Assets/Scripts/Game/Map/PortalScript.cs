using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {

	public static bool IsActive;

	private ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		IsActive = true;
		particleSystem = GetComponentInChildren<ParticleSystem>();
	}

	void Update()
	{
		particleSystem.enableEmission = IsActive;
	}

}
