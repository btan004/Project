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

				// Move enemy using navmesh 
				e.GetComponent<NavMeshAgent>().SetDestination(playerLocation);

				// Make sure the enemy stays on the ground plane
				//e.transform.SetPositionY(1);
			}
			else if( e.IsWithinAttackRange() )
			{
				e.GetComponent<NavMeshAgent>().Stop();
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