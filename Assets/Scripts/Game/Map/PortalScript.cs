using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {

	public bool IsActive;

	//private ParticleSystem particleSystem;

	public int Destination;

	public Color ActiveColor;
	public Color InactiveColor;

	// Use this for initialization
	void Start () {
		IsActive = true;

		ActiveColor = new Color(159f / 255f, 89f / 255f, 5f / 255f, 1);
		InactiveColor = new Color(255f / 255f, 0, 0, 1);

		if (IsActive)
			particleSystem.startColor = ActiveColor;
		else
			particleSystem.startColor = InactiveColor;		
	}

	void Update()
	{
		if (IsActive)
			particleSystem.startColor = ActiveColor;
		else
			particleSystem.startColor = InactiveColor;		
	}

}
