using UnityEngine;
using System.Collections;

public class LandmineScript : MonoBehaviour {

	public float Damage = 30;
	public float ExplosionDuration = 2f;
	private float explosionTimer;
	private bool exploding;

	// Use this for initialization
	void Start () 
	{
		foreach (MeshRenderer mesh in this.GetComponentsInChildren<MeshRenderer>())
		{
			if (mesh.name == "Base")
				mesh.material.color = new Color(126f / 255f, 143f / 255f, 156f / 255f);
			else if (mesh.name == "Trigger")
				mesh.material.color = new Color(186f / 255f, 93f / 255f, 104f / 255f);
		}
		print ("I'm attached to " + transform.name);
		exploding = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (exploding)
		{
			explosionTimer -= Time.deltaTime;

			if (explosionTimer <= 0)
				Destroy(this.gameObject);
		}
	}

	public void ActivateTrap(PlayerScript player)
	{
		//apply the damage to the character
		player.ApplyDamage(Damage);

		//turn on the explosion
		exploding = true;
		explosionTimer = ExplosionDuration;
		this.GetComponentInChildren<ParticleSystem>().enableEmission = true;
		
	}
}
