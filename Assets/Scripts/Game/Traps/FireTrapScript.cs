using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireTrapScript : MonoBehaviour {

	public float Damage = 10f;
	public float TimeOff = 5f;
	public float TimeOn = 5f;
	public float timer = 0f;
	public bool IsActive = false;

	public bool partOfSpinningTrap = false;

	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Use this for initialization
	void Start () {
		//get references to our particle systems
		foreach (ParticleSystem s in this.GetComponentsInChildren<ParticleSystem>())
		{
			particleSystems.Add(s);
			s.enableEmission = false;
		}

		//get a random delay to start from
		if (!partOfSpinningTrap)
			timer = Random.Range(0.0f, 6.0f);
	}
	
	// Update is called once per frame
	void Update () {
		//reduce our timer
		timer -= Time.deltaTime;

		//if the fire trap is active
		if (IsActive)
		{
			//check if its time to turn the fire trap off
			if (timer <= 0)
			{
				//turn the fire trap off
				TurnTrapOff();

				//reset our timer
				timer = TimeOff;
			}
		}
		//if the fire trap is inactive
		else
		{
			//check if its time to turn the fire trap on
			if (timer <= 0)
			{
				//turn the fire trap on
				TurnTrapOn();

				//reset our timer
				timer = TimeOn;
			}
		}
	}

	void TurnTrapOn()
	{
		//enable particle systems
		foreach (ParticleSystem s in particleSystems)
			s.enableEmission = true;

		//set active
		IsActive = true;
	}

	void TurnTrapOff()
	{
		//enable particle systems
		foreach (ParticleSystem s in particleSystems)
			s.enableEmission = false;

		//set active
		IsActive = false;
	}

	public void ActivateTrap(PlayerScript player)
	{
		//if the trap is active
		if (IsActive)
		{
			//apply the damage to the player
			player.ApplyDamage(0.1f * player.Skills.GetPlayerHealthMax() * Time.deltaTime);
		}
	}

	public void ActivateTrap(EnemyBaseScript enemy)
	{
		//if the trap is active
		if (IsActive)
		{
			//apply the damage to the player
			enemy.ApplyDamage(0.1f * enemy.MaxHealth * Time.deltaTime);
		}		
	}

}
