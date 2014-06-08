using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomeScript : MonoBehaviour {

	public GameObject[] FireLines;
	public GameObject[] Portals;

	Dictionary<string, GameObject> fireLinesDictionary;

	// Use this for initialization
	void Start () {
		fireLinesDictionary = new Dictionary<string, GameObject>();

		foreach (GameObject g in FireLines)
		{
			fireLinesDictionary.Add(g.name, g);
			ActivateFireLine(g.name, false);
			//Debug.Log("Added to dictionary: " + g.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool portalActive1 = Portals[0].GetComponent<PortalScript>().IsActive;
		bool portalActive2 = Portals[1].GetComponent<PortalScript>().IsActive;
		bool portalActive3 = Portals[2].GetComponent<PortalScript>().IsActive;
		bool portalActive4 = Portals[3].GetComponent<PortalScript>().IsActive;
		bool portalActive5 = Portals[4].GetComponent<PortalScript>().IsActive;

		Portals[0].GetComponentInChildren<Component>().renderer.enabled = portalActive1;
		Portals[1].GetComponentInChildren<Component>().renderer.enabled = portalActive2;
		Portals[2].GetComponentInChildren<Component>().renderer.enabled = portalActive3;
		Portals[3].GetComponentInChildren<Component>().renderer.enabled = portalActive4;
		Portals[4].GetComponentInChildren<Component>().renderer.enabled = portalActive5;

		ActivateFireLine("FireLine12", !portalActive1 && !portalActive2);
		ActivateFireLine("FireLine13", !portalActive1 && !portalActive3);
		ActivateFireLine("FireLine14", !portalActive1 && !portalActive4);
		ActivateFireLine("FireLine15", !portalActive1 && !portalActive5);
		ActivateFireLine("FireLine23", !portalActive2 && !portalActive3);
		ActivateFireLine("FireLine24", !portalActive2 && !portalActive4);
		ActivateFireLine("FireLine25", !portalActive2 && !portalActive5);
		ActivateFireLine("FireLine34", !portalActive3 && !portalActive4);
		ActivateFireLine("FireLine35", !portalActive3 && !portalActive5);
		ActivateFireLine("FireLine45", !portalActive4 && !portalActive5);

		Portals[5].GetComponent<PortalScript>().IsActive =
			(!portalActive1 && !portalActive2 && !portalActive3 && !portalActive4 && !portalActive5);
	}

	private void ActivateFireLine(string fireline, bool active)
	{
		foreach (ParticleSystem ps in fireLinesDictionary[fireline].GetComponentsInChildren<ParticleSystem>())
			ps.enableEmission = active;
	}

	public void ResetHome()
	{
		Portals[0].GetComponent<PortalScript>().IsActive = true;
		Portals[1].GetComponent<PortalScript>().IsActive = true;
		Portals[2].GetComponent<PortalScript>().IsActive = true;
		Portals[3].GetComponent<PortalScript>().IsActive = true;
		Portals[4].GetComponent<PortalScript>().IsActive = true;
	}
}
