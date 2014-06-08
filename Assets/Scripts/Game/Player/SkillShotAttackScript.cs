using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillShotAttackScript : MonoBehaviour {

	public PlayerScript player;
	public CursorScript cursor;
	private Vector3 directionToOffset;
	public float Force = 10f;
	public float ParticleDuration = 0.5f;
	private float particleDurationTimer;
	public ParticleSystem particleSystem;

	ParticleSystem[] particleSystems;

	private List<GameObject> enemiesInRange = new List<GameObject>();

	// Use this for initialization
	void Start () {
		renderer.material.color = Color.green;
		
		directionToOffset = new Vector3();

		particleSystems = this.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps in particleSystems)
			ps.enableEmission = false;
		particleDurationTimer = ParticleDuration;
	}
	
	// Update is called once per frame
	void Update()
	{
		directionToOffset = cursor.transform.position - player.transform.position;
		this.transform.position = cursor.transform.position + (.5f * directionToOffset);

		Vector3 direction = (cursor.player.transform.position - cursor.transform.position);
		Quaternion rotation = Quaternion.LookRotation(direction);
		rotation *= Quaternion.Euler(0, 90, 0);
		this.transform.rotation = rotation;

		//this.renderer.enabled = cursor.player.IsSkillShotActive;

		if (cursor.player.IsSkillShotActive)
		{
			foreach (ParticleSystem ps in particleSystems)
				ps.enableEmission = true;

			particleDurationTimer = ParticleDuration;
			audio.Play();
		}

		foreach (ParticleSystem ps in particleSystems)
		{
			if (ps.enableEmission)
			{
				particleDurationTimer -= (Time.deltaTime / particleSystems.Length);
				if (particleDurationTimer <= 0)
				{
					ps.enableEmission = false;
				}
			}
		}
	}
	
	public void ApplySkillShotAttack(EnemyBaseScript enemy)
	{
		if (player.IsSkillShotActive)
		{
			enemy.ApplyDamage(player.Skills.GetPlayerSkillShotDamage());
			enemy.AddKnockback(enemy.transform.position - player.transform.position, Force);			
		}
	}	
	
	void LateUpdate()
	{
		if (player.IsSkillShotActive)
		{
			foreach (GameObject other in enemiesInRange)
			{
				if( other )
				{
					EnemyBaseScript enemy = other.GetComponent<EnemyBaseScript>();
					if (enemy)
					{
						enemy.ApplyDamage(player.Skills.GetPlayerDamage());
						enemy.AddKnockback(enemy.transform.position - player.transform.position, Force);
						StartCoroutine(enemy.Flash(0.2f, Color.red));
					}
				}
			}
		}

		enemiesInRange.Clear();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			enemiesInRange.Add(other.gameObject);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (other.tag == "Enemy")
		{
			enemiesInRange.Add(other.gameObject);
		}
	}
}
