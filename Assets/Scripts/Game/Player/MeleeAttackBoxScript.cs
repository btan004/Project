using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttackBoxScript : MonoBehaviour {

	public CursorScript cursor;
	public PlayerScript player;
	private Vector3 directionToOffset;
	public float Force = 30f;
	private bool attacking;
	private bool really;

	public AudioClip[] swordSounds;
	public AudioClip[] enemyBeingHitSounds;

	private List<GameObject> enemiesInRange = new List<GameObject>();

	// Use this for initialization
	void Start () {
		renderer.material.color = Color.green;
		directionToOffset = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
		directionToOffset = cursor.transform.position - player.transform.position;

		this.transform.position = cursor.transform.position - (.25f * directionToOffset);

		Vector3 direction = (cursor.player.transform.position - cursor.transform.position);
		Quaternion rotation = Quaternion.LookRotation (direction);
		rotation *= Quaternion.Euler(0, 90, 0);
		this.transform.rotation = rotation;

		//this.renderer.enabled = PlayerScript.IsAttacking;
	}

	void LateUpdate()
	{
		if (PlayerScript.IsAttacking)
		{
			audio.clip = swordSounds[Random.Range(0, swordSounds.Length)];
			audio.pitch = 1.76f;
			audio.volume = 1.0f;

			foreach (GameObject other in enemiesInRange)
			{
				if (other)
				{
					EnemyBaseScript enemy = other.GetComponent<EnemyBaseScript>();

					/*REMOVE LATER
					 **/
					EnemyBaseCloneScript enemy2 = other.GetComponent<EnemyBaseCloneScript>();

					audio.clip = enemyBeingHitSounds[Random.Range(0, enemyBeingHitSounds.Length)];
					audio.pitch = 0.6f;
					audio.volume = 0.188f;

					if (enemy)
					{
						enemy.ApplyDamage(player.Skills.GetPlayerDamage());
						enemy.AddKnockback(enemy.transform.position - player.transform.position, Force);
					}
					if (enemy2)
					{
						enemy2.ApplyDamage(player.Skills.GetPlayerDamage());
						enemy2.AddKnockback(enemy2.transform.position - player.transform.position, Force);
					}
				}
			}

			audio.Play();
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