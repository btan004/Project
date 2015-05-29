using UnityEngine;
using System.Collections;

public class EnemyHitboxCloneScript : MonoBehaviour {

	public EnemyBaseCloneScript enemy;

	// Use this for initialization
	void Start () {
		transform.position = enemy.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = enemy.transform.position;
	}

	/*
	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "PlayerMeleeAttackHitbox":
				//Debug.Log("Within player attack range: " + this.name);
				//other.gameObject.GetComponent<MeleeAttackBoxScript>().ApplyMeleeAttack(enemy);
				break;
			case "PlayerSkillShotAttackHitbox":
				//Debug.Log("Within player skill shot range: " + this.name);
				//other.gameObject.GetComponent<SkillShotAttackScript>().ApplySkillShotAttack(enemy);
				break;
			default:
				break;
		}
	}

	void OnTriggerStay(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "PlayerMeleeAttackHitbox":
				Debug.Log("Within player attack range: " + this.name);
				//other.gameObject.GetComponent<MeleeAttackBoxScript>().ApplyMeleeAttack(enemy);
				break;
			case "PlayerSkillShotAttackHitbox":
				Debug.Log("Within player skill shot range: " + this.name);
				//other.gameObject.GetComponent<SkillShotAttackScript>().ApplySkillShotAttack(enemy);
				break;
			case "PlayerAuraAttackHitbox":
				Debug.Log("Within aura range: " + this.name);
				//other.gameObject.GetComponent<AuraAttackScript>().ApplyAuraAttack(enemy);
				break;
			default:
				break;
		}
	}
	*/
}
