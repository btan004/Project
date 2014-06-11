using UnityEngine;
using System.Collections;
using System;

public class Buffer_SelectTarget : State<EnemyBufferScript>
{
	static readonly Buffer_SelectTarget instance = new Buffer_SelectTarget();
	
	public static Buffer_SelectTarget Instance
	{
		get { return instance; }
	}
	static Buffer_SelectTarget()
	{
	}

	public float checkRadius = 10f;

	public override void BeforeEnter( EnemyBufferScript e )
	{

	}

	/// <summary>
	/// State that handles selecting a target to go to
	/// </summary>
	/// <description>
	/// If there are enemies within a radius around player, select one of them
	/// Else:
	/// 	If there are other enemies other than this 1, select one of them
	/// 	Else spawn a random enemy
	/// </description>
	public override void Action( EnemyBufferScript e)
	{	
		int layerMask = 1 << Globals.ENEMY_LAYER;
		Collider[] enemies = Physics.OverlapSphere (EnemyBaseScript.player.transform.position, checkRadius, layerMask);
		Debug.Log ("Selecting...");
		if( enemies.Length > 1 )
		{
			int randomEnemy = UnityEngine.Random.Range(0, enemies.Length - 1);

			Debug.Log ("Selecting enemy num: " + randomEnemy );
			Debug.Log ("Random enemy is: " + enemies[randomEnemy].gameObject.name);

			if( enemies[randomEnemy].gameObject.CompareTag("Enemy") )
			{
				e.targetLocation = enemies[randomEnemy].gameObject;
				e.ChangeState( Buffer_MoveToTarget.Instance );
			}
		}
		else
		{
			GameObject enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");
			EnemyContainerScript containerScript = enemyContainer.GetComponent<EnemyContainerScript>();
			//If there are enemies other than me on the map
			if( containerScript.GetEnemyCount() > 1 )
			{
				int randomEnemy = UnityEngine.Random.Range(0, containerScript.GetEnemyCount() - 1 );

				Debug.Log ("Selecting enemy num: " + randomEnemy );

				EnemyBaseScript[] scripts = enemyContainer.GetComponentsInChildren<EnemyBaseScript>();

				Debug.Log ("Random enemy is: " + scripts[randomEnemy].gameObject.name);


				e.targetLocation = scripts[randomEnemy].gameObject;
				e.ChangeState( Buffer_MoveToTarget.Instance );
			}
			//Else select player

		}
		e.anim.SetFloat ("Speed", 0f);
	}

	public override void BeforeExit( EnemyBufferScript e )
	{

	}
}