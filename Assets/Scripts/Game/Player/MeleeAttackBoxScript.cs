﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttackBoxScript : MonoBehaviour {

	public CursorScript cursor;
	public PlayerScript player;
	private Vector3 directionToOffset;
	public float Force = 3f;

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

		this.renderer.enabled = cursor.player.IsAttacking;
	}

	void LateUpdate()
	{
		if (player.IsAttacking)
		{
			foreach (GameObject other in enemiesInRange)
			{
				EnemyBaseScript enemy = other.GetComponent<EnemyBaseScript>();
				enemy.ApplyDamage(player.AttackDamage);
				enemy.AddKnockback(enemy.transform.position - player.transform.position, Force);
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
}