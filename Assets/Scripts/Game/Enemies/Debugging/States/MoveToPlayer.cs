using UnityEngine;
using System.Collections;
using System;

public class MoveToPlayer : State<EnemyChaserCloneScript>
{
	static readonly MoveToPlayer instance = new MoveToPlayer();
	
	public static MoveToPlayer Instance
	{
		get { return instance; }
	}
	static MoveToPlayer()
	{
	}


	public override void BeforeEnter( EnemyChaserCloneScript e )
	{

	}

	public override void Action( EnemyChaserCloneScript e)
	{
		if (EnemyBaseCloneScript.player && e.IsMoving && !e.IsWithinAttackRange()) {
			// Get player location
			Vector3 playerLocation = EnemyBaseCloneScript.player.transform.position;
			
			// Set movement step
			float moveStep = e.Velocity*Time.deltaTime;
			Debug.Log (moveStep);
			
			// Move towards player
			e.transform.position = Vector3.MoveTowards(e.transform.position,playerLocation,moveStep);
			
			//make sure the enemy stays on the ground plane
			//this.transform.SetPositionY(1);
		}
		else if( e.IsWithinAttackRange() )
		{
			e.ChangeState(AttackPlayer.Instance);
		}
		e.anim.SetFloat ("Speed", Convert.ToSingle(!e.IsWithinAttackRange()));
	}

	public override void BeforeExit( EnemyChaserCloneScript e )
	{

	}
}