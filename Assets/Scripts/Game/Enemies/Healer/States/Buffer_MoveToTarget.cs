using UnityEngine;
using System.Collections;
using System;

public class Buffer_MoveToTarget : State<EnemyBufferScript>
{
	static readonly Buffer_MoveToTarget instance = new Buffer_MoveToTarget();
	
	public static Buffer_MoveToTarget Instance
	{
		get { return instance; }
	}
	static Buffer_MoveToTarget()
	{
	}

	public float countdownTime = 5;
	public float countdownValue;
	public bool counting = false;

	public override void BeforeEnter( EnemyBufferScript e )
	{

	}

	public override void Action( EnemyBufferScript e)
	{	
		if( e.targetLocation )
		{
			if( e.IsWithinRange( e.targetLocation, 3f ) )
			{
				e.anim.SetFloat ("Speed", 0f);
				e.GetComponent<NavMeshAgent>().Stop ();
				e.StartCoroutine( e.SelectOtherTarget( countdownTime ) );
			}
			else
			{
				e.anim.SetFloat ("Speed", 1f);
				e.GetComponent<NavMeshAgent>().SetDestination( e.targetLocation.transform.position );
			}
		}
		else
		{
			e.GetComponent<NavMeshAgent>().Stop ();
			e.ChangeState( Buffer_SelectTarget.Instance );
		}

	}
	
	public override void BeforeExit( EnemyBufferScript e )
	{

	}
}