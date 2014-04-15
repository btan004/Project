using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuraAttackScript : MonoBehaviour {

	public PlayerScript player;
	public float RotateSpeed;

	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Use this for initialization
	void Start () {
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			particleSystems.Add(s);
			s.enableEmission = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = player.transform.position;

		this.renderer.enabled = PlayerScript.IsAuraActive;
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			s.enableEmission = PlayerScript.IsAuraActive;
		}

		transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
	}
}
