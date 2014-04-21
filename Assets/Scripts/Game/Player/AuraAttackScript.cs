using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuraAttackScript : MonoBehaviour {

	public PlayerScript player;
	public float RotateSpeed;
	public bool Debug;

	public float Force = 1f;

	private List<GameObject> enemiesInRange = new List<GameObject>();

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

		this.renderer.enabled = Debug;
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			s.enableEmission = PlayerScript.IsAuraActive;
		}

		transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
	}

	void LateUpdate()
	{
		if (PlayerScript.IsAuraActive)
		{
			foreach (GameObject other in enemiesInRange)
			{
				EnemyBaseScript enemy = other.GetComponent<EnemyBaseScript>();
				enemy.ApplyDamage(PlayerScript.AuraDamage);
				enemy.AddKnockback(enemy.transform.position - player.transform.position, PlayerScript.AuraForce);
			}
		}

		enemiesInRange.Clear();
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			enemiesInRange.Add(other.gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			enemiesInRange.Add(other.gameObject);
		}
	}

}
