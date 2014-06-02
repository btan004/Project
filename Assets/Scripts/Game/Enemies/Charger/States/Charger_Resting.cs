using UnityEngine;
using System.Collections;
using System;

public class Charger_Resting : State<EnemyChargerScript>
{
	static readonly Charger_Resting instance = new Charger_Resting();

	public static Charger_Resting Instance
	{
		get { return instance; }
	}
	static Charger_Resting()
	{

	}

	public override void BeforeEnter( EnemyChargerScript e )
	{

	}

	public override void Action( EnemyChargerScript e)
	{
		e.RestingTimeCounter += Time.deltaTime;
		if( e.RestingTimeCounter >= e.RestingTime )
		{
			e.RestingTimeCounter = 0;
			e.ChangeState(Charger_MoveToPlayer.Instance);
		}
	}
	public override void BeforeExit( EnemyChargerScript e )
	{

	}
}