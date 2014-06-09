using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {

	public bool IsActive;

	public int Destination;

	public Color ActiveColor;
	public Color InactiveColor;

	// Use this for initialization
	void Start () {
		IsActive = true;

		ActiveColor = new Color(0, 1, 0, 1);
		InactiveColor = new Color(1, 0, 0, 1);

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
