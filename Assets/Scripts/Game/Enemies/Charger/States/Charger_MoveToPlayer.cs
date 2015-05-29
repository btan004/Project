using UnityEngine;
using System.Collections;

public class Charger_MoveToPlayer : State<EnemyChargerScript>
{
	static readonly Charger_MoveToPlayer instance = new Charger_MoveToPlayer();
	
	public static Charger_MoveToPlayer Instance
	{
		get { return instance; }
	}
	static Charger_MoveToPlayer()
	{
	}


	//State Machine variables go here
	bool IsChargingUp = false;

	public override void BeforeEnter( EnemyChargerScript e )
	{
		IsChargingUp = false;
	}

	public override void Action( EnemyChargerScript e)
	{
		if( EnemyBaseScript.player )
		{
			if ( e.ChargeReady && !e.IsWithinAttackRange() )
			{
				int layerMask = 1 << Globals.DEFAULT_LAYER;
				bool lineOfSight = e.clearLineOfSight( EnemyBaseScript.player.transform.position, layerMask );
				if( lineOfSight )
				{
					float ChanceToCharge = Random.Range(0.0f, 100.0f);
					if( ChanceToCharge <= 2.0 )
					{
						IsChargingUp = true;
						e.GetComponent<NavMeshAgent>().Stop();
						e.ChangeState(Charger_ChargingUp.Instance);
					}
				}
			}
			else if (e.IsMoving && !e.IsWithinAttackRange() && !e.ChargeReady) {
				// Get player location
				Vector3 playerLocation = EnemyBaseScript.player.transform.position;

				//Before moving, check if enemy still alive.
				//There's an issue where an enemy still tries to move, but they are
				//dead right before they perform the navmesh move
				if( e.Health > 0 )
				{
					// Move enemy using navmesh 
					e.GetComponent<NavMeshAgent>().SetDestination(playerLocation);
				}
				
				// Make sure the enemy stays on the ground plane
				//e.transform.SetPositionY(1);
			}
			else if( e.IsWithinAttackRange() )
			{
				e.GetComponent<NavMeshAgent>().Stop ();
				e.ChangeState(Charger_AttackPlayer.Instance);
			}
			e.anim.SetFloat ("Speed", System.Convert.ToSingle(!e.IsWithinAttackRange() && !IsChargingUp ));
		}
		else
		{
			Debug.Log ("[Charger_MoveToPlayer] WARNING: player entity null does not exist");
		}

		//Update anything that needs a cooldown
		e.ChargeCooldownCounter += Time.deltaTime;
		
		if( e.ChargeCooldownCounter >= e.ChargeCooldown )
		{
			e.ChargeCooldownCounter = e.ChargeCooldown;
			e.ChargeReady = true;
		}
	}

	public override void BeforeExit( EnemyChargerScript e )
	{

	}
}