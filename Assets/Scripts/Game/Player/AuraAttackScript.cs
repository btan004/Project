using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuraAttackScript : MonoBehaviour {

	public PlayerScript player;
	public float RotateSpeed;
	public bool Debug;

	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Use this for initialization
	void Start () {
		//get references to all particle systems, and disable their emission
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			particleSystems.Add(s);
			s.enableEmission = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = player.transform.position;

		this.renderer.enabled = Debug;
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			
			s.enableEmission = PlayerScript.IsAuraActive;
			if (!PlayerScript.IsAuraActive) s.Clear();
		}

		transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
	}

	
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			//UnityEngine.Debug.LogWarning("Aura Enter: " + other.name);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			//UnityEngine.Debug.LogWarning("Aura Stay: " + other.name);
			if (PlayerScript.IsAuraActive)
			{
				EnemyBaseScript enemy = other.GetComponent<EnemyBaseScript>();
				enemy.ApplyDamage(player.Skills.GetAuraDamage() * Time.deltaTime);
				enemy.AddKnockback(enemy.transform.position - player.transform.position, PlayerScript.AuraForce);
			}
		}
	}
}
