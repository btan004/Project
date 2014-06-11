using UnityEngine;
using System.Collections;
using System;

public class Charger_ChargingUp : State<EnemyChargerScript>
{
	static readonly Charger_ChargingUp instance = new Charger_ChargingUp();

	public static Charger_ChargingUp Instance
	{
		get { return instance; }
	}
	static Charger_ChargingUp()
	{

	}

	public override void BeforeEnter( EnemyChargerScript e )
	{
		foreach (ParticleSystem s in e.GetComponentsInChildren<ParticleSystem>())
		{
			if( s.name == "ChargeupParticles" )
			{
				s.enableEmission = true;
			}
		}
	}

	public override void Action( EnemyChargerScript e)
	{
		e.TimeUntilChargeCounter += Time.deltaTime;
		if( e.TimeUntilChargeCounter >= e.TimeUntilCharge )
		{
			int layerMask = 1 << Globals.DEFAULT_LAYER;
			bool lineOfSight = e.clearLineOfSight (EnemyBaseScript.player.transform.position, layerMask);

			if( lineOfSight )
			{
				e.ChargeTarget.x = EnemyBaseScript.player.transform.position.x;
				e.ChargeTarget.y = e.transform.position.y;
				e.ChargeTarget.z = EnemyBaseScript.player.transform.position.z;
				
				//Set state to Charging
				e.ChangeState(Charger_Charging.Instance);
			}
			else
			{
				e.ChangeState(Charger_MoveToPlayer.Instance);
				e.ChargeCooldownCounter = 0f;
				e.ChargeReady = false;
			}
			e.TimeUntilChargeCounter = 0;

		}
	}
	public override void BeforeExit( EnemyChargerScript e )
	{
		foreach (ParticleSystem s in e.GetComponentsInChildren<ParticleSystem>())
		{
			if( s.name == "ChargeupParticles" )
			{
				s.enableEmission = false;
			}
		}
	}
}