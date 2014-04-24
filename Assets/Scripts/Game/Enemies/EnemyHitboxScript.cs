using UnityEngine;
using System.Collections;

public class EnemyHitboxScript : MonoBehaviour {

	public EnemyBaseScript enemy;

	// Use this for initialization
	void Start () {
		transform.position = enemy.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = enemy.transform.position;
	}

	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "PlayerMeleeAttackHitbox":
				//other.gameObject.GetComponent<MeleeAttackBoxScript>().ApplyMeleeAttack(enemy);
				break;
			case "PlayerSkillShotAttackHitbox":
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
			case "PlayerAuraAttackHitbox":
				other.gameObject.GetComponent<AuraAttackScript>().ApplyAuraAttack(enemy);
				break;
			default:
				break;
		}
	}
}
