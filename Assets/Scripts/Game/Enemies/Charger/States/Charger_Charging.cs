using UnityEngine;
using System.Collections;
using System;

public class Charger_Charging : State<EnemyChargerScript>
{
	static readonly Charger_Charging instance = new Charger_Charging();

	public static Charger_Charging Instance
	{
		get { return instance; }
	}
	static Charger_Charging()
	{

	}

	public override void BeforeEnter( EnemyChargerScript e )
	{

	}

	public override void Action( EnemyChargerScript e)
	{
		float ChargeStep = e.ChargeVelocity*Time.deltaTime;
		e.transform.position = Vector3.MoveTowards( e.transform.position, e.ChargeTarget, ChargeStep );

		//Charge has reached it's target position
		if( e.transform.position == e.ChargeTarget )
		{
			e.ChargeCooldownCounter = 0;
			e.ChangeState(Charger_Resting.Instance);
		}
	}
	public override void BeforeExit( EnemyChargerScript e )
	{
		foreach (ParticleSystem s in e.GetComponentsInChildren<ParticleSystem>())
		{
			if( s.name == "TrailParticles" )
			{
				s.enableEmission = true;
			}
		}
	}
}