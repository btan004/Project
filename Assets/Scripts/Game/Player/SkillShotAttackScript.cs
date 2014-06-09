using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillShotAttackScript : MonoBehaviour {

	public PlayerScript player;
	public CursorScript cursor;
	private Vector3 directionToOffset;
	public float Force = 3f;
	public float ParticleDuration = 0.5f;
	private float particleDurationTimer;
	public ParticleSystem particleSystem;

	ParticleSystem[] particleSystems;

	public AudioClip skillShotSound;
	public AudioClip enemyHitSound;
	public AudioSource[] enemiesHitSounds;

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
			audio.clip = skillShotSound;
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
			//SkillShot Damage = Skill Shot Skill Multiplier x Player Attack Damage
			enemy.ApplyDamage(player.Skills.GetPlayerSkillShotDamage() * player.Skills.GetPlayerDamage());
			enemy.AddKnockback(enemy.transform.position - player.transform.position, Force);
			UnityEngine.Debug.LogWarning("Skill Shot Damage: " + (player.Skills.GetPlayerSkillShotDamage() * player.Skills.GetPlayerDamage()));
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

						Debug.LogError("Looking for open audio source!");
						for (int i = 0; i < enemiesHitSounds.Length; i++)
						{
							if (!enemiesHitSounds[i].audio.isPlaying || enemiesHitSounds[i].audio.time > 1.0f)
							{
								enemiesHitSounds[i].audio.Play();
								break;
								Debug.LogError("Found open audio source!");
							}
						}

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
