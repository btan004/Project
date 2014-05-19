using UnityEngine;
using System.Collections;
using System;

public class Chaser_MoveToPlayer : State<EnemyChaserScript>
{
	static readonly Chaser_MoveToPlayer instance = new Chaser_MoveToPlayer();
	
	public static Chaser_MoveToPlayer Instance
	{
		get { return instance; }
	}
	static Chaser_MoveToPlayer()
	{
	}


	public override void BeforeEnter( EnemyChaserScript e )
	{

	}

	public override void Action( EnemyChaserScript e)
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
				e.ChangeState(Chaser_AttackPlayer.Instance);
			}
			e.anim.SetFloat ("Speed", Convert.ToSingle(!e.IsWithinAttackRange()));
		}
		else
		{
			Debug.Log ("[Chaser_MoveToPlayer] WARNING: player entity null does not exist");
		}
	}

	public override void BeforeExit( EnemyChaserScript e )
	{

	}
}