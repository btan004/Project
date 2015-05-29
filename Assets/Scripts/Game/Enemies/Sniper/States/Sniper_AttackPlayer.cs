using UnityEngine;
using System.Collections;
using System;

public class Sniper_AttackPlayer : State<EnemySniperScript>
{
	static readonly Sniper_AttackPlayer instance = new Sniper_AttackPlayer();

	public static Sniper_AttackPlayer Instance
	{
		get { return instance; }
	}
	static Sniper_AttackPlayer()
	{
	}

	public override void BeforeEnter( EnemySniperScript e )
	{

	}

	public override void Action( EnemySniperScript e)
	{
		int layerMask = 1 << Globals.DEFAULT_LAYER;
		bool lineOfSight = e.clearLineOfSight (EnemyBaseScript.player.transform.position, layerMask);
		if (e.IsWithinAttackRange () && lineOfSight )
		{
			e.NextAttack = e.NextAttack - Time.deltaTime;
			if(e.NextAttack <= 0)
			{
				// Create Bullet
				if(!e.isBoss){
					Transform bulletPosition = e.transform;
					bulletPosition.SetPositionY(1);
					GameObject bullet = GameObject.Instantiate(e.EnemyBulletPrefab, bulletPosition.position,Quaternion.identity) as GameObject;
					bullet.GetComponent<EnemyBulletScript>().SetDamage(e.Damage);
					e.NextAttack = e.AttackRate;
				}
				else{
					Transform bulletPosition = e.transform;
					bulletPosition.SetPositionY(1);
					float deg = 0f; 
					for(int i = 5; i>0; i=i-1){
						//GameObject enemy = Instantiate(EnemySpawn) as GameObject;
						deg = deg + (360f/5);
						float enemyx = e.transform.position.x + 5*Mathf.Cos(deg*Mathf.Deg2Rad);
						float enemyz = e.transform.position.z + 5*Mathf.Sin(deg*Mathf.Deg2Rad);
						Vector3 bulletStart = new Vector3(enemyx,1,enemyz);
						Vector3 startDirection = bulletStart - e.transform.position;
						startDirection.Normalize();

						// Boss settings
						GameObject bulletDirect = GameObject.Instantiate(e.EnemyBulletPrefab, bulletPosition.position,Quaternion.identity) as GameObject;
						bulletDirect.GetComponent<EnemyBulletScript>().SetDamage(e.Damage);
						bulletDirect.GetComponent<EnemyBulletScript>().SetBossBullet(true);
						bulletDirect.GetComponent<EnemyBulletScript>().SetInitialDirection(startDirection);
					}
					e.NextAttack = e.AttackRate;
				}
			}
		}
		else
		{
			e.ChangeState( Sniper_MoveToPlayer.Instance );
		}
	}
	public override void BeforeExit( EnemySniperScript e )
	{
		//e.anim.SetBool ("Attacking", false);	
	}
}