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
	ParticleSystem particleSystem;

	private List<GameObject> enemiesInRange = new List<GameObject>();

	// Use this for initialization
	void Start () {
		renderer.material.color = Color.green;
		
		directionToOffset = new Vector3();

		particleSystem = this.GetComponentInChildren<ParticleSystem>();
		particleSystem.enableEmission = false;
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
			particleSystem.enableEmission = true;
			particleDurationTimer = ParticleDuration;
			audio.Play();
		}

		if (particleSystem.enableEmission)
		{
			particleDurationTimer -= Time.deltaTime;
			if (particleDurationTimer <= 0)
			{
				particleSystem.enableEmission = false;
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
