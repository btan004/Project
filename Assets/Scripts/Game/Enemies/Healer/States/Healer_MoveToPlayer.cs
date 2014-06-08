using UnityEngine;
using System.Collections;
using System;

public class Healer_MoveToPlayer : State<EnemyHealerScript>
{
	static readonly Healer_MoveToPlayer instance = new Healer_MoveToPlayer();
	
	public static Healer_MoveToPlayer Instance
	{
		get { return instance; }
	}
	static Healer_MoveToPlayer()
	{
	}


	public override void BeforeEnter( EnemyHealerScript e )
	{

	}

	public override void Action( EnemyHealerScript e)
	{	
		// Get player location
		Vector3 playerLocation = EnemyBaseScript.player.transform.position;
		
		//Set y vector to 0 since we don't want to do anything with the y axis
		
		// Set movement step
		//float moveStep = e.Velocity*Time.deltaTime;
		if( e.IsTooFarFromPlayer() )
		{
			e.GetComponent<NavMeshAgent>().SetDestination(playerLocation);
		}
		else
		{
			e.GetComponent<NavMeshAgent>().Stop();
		}

		if( e.IsTooCloseToPlayer() )
		{
			NavMeshHit hit;
			NavMesh.Raycast( e.transform.position, -e.transform.forward*4, out hit, -1);
			e.GetComponent<NavMeshAgent>().SetDestination( hit.position + e.transform.position );
			e.ChangeState( Healer_RunFromPlayer.Instance );
			//TODO: Keep performing raycasts every certain period of time.
			//TODO: Check if raycast hits something. If it does, perform another raycast, some degrees to left or right
			//		and see if the enemy can retreat towards that direction
		}


		//e.anim.SetFloat ("Speed", Convert.ToSingle(e.IsTooFarFromPlayer() || e.IsTooCloseToPlayer()));
	}

	public override void BeforeExit( EnemyHealerScript e )
	{

	}
}