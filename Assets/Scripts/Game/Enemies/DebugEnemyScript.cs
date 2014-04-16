using UnityEngine;
using System.Collections;

public class DebugEnemyScript : MonoBehaviour {

	//Lifetime in seconds
	public float Lifetime = 60f;

	// Use this for initialization
	void Start () 
	{
		foreach (MeshRenderer mesh in this.GetComponentsInChildren<MeshRenderer>())
		{
			if (mesh.name == "Body")
				mesh.material.color = new Color(126f / 255f, 143f / 255f, 156f / 255f);
			else if (mesh.name == "Head")
				mesh.material.color = new Color(186f / 255f, 93f / 255f, 104f / 255f);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		Lifetime -= Time.deltaTime;
		if( Lifetime <= 0 )
		{
			Destroy(this.gameObject);
		}
	}
}
