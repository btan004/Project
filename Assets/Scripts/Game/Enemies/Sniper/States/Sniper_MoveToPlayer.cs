using UnityEngine;
using System.Collections;
using System;

public class Sniper_MoveToPlayer : State<EnemySniperScript>
{
	static readonly Sniper_MoveToPlayer instance = new Sniper_MoveToPlayer();
	
	public static Sniper_MoveToPlayer Instance
	{
		get { return instance; }
	}
	static Sniper_MoveToPlayer()
	{
	}


	public override void BeforeEnter( EnemySniperScript e )
	{

	}

	public override void Action( EnemySniperScript e)
	{
		if( EnemyBaseScript.player )
		{
			if (e.IsMoving && !e.IsWithinAttackRange()) {
				// Get player location
				Vector3 playerLocation = EnemyBaseScript.player.transform.position;
				
				// Set movement step
				float moveStep = e.Velocity*Time.deltaTime;
				
				// Move towards player
				e.transform.position = Vector3.MoveTowards(e.transform.position,playerLocation,moveStep);
				
				//make sure the enemy stays on the ground plane
				//this.transform.SetPositionY(1);
			}
			else if( e.IsWithinAttackRange() )
			{
				e.ChangeState(Sniper_AttackPlayer.Instance);
			}
			e.anim.SetFloat ("Speed", Convert.ToSingle(!e.IsWithinAttackRange()));
		}
		else
		{
			Debug.Log ("[Sniper_MoveToPlayer] WARNING: player entity null does not exist");
		}
	}

	public override void BeforeExit( EnemySniperScript e )
	{

	}
}