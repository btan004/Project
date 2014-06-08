using UnityEngine;
using System.Collections;
using System;

public class Healer_RunFromPlayer : State<EnemyHealerScript>
{
	static readonly Healer_RunFromPlayer instance = new Healer_RunFromPlayer();
	
	public static Healer_RunFromPlayer Instance
	{
		get { return instance; }
	}
	static Healer_RunFromPlayer()
	{
	}


	public override void BeforeEnter( EnemyHealerScript e )
	{

	}

	public override void Action( EnemyHealerScript e)
	{	
	
	}

	public override void BeforeExit( EnemyHealerScript e )
	{

	}
}