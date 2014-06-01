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
		if (e.IsWithinAttackRange ())
		{
			e.NextAttack = e.NextAttack - Time.deltaTime;
			if(e.NextAttack <= 0)
			{
				// Create Bullet
				Transform bulletPosition = e.transform;
				bulletPosition.SetPositionY(1);
				GameObject bullet = GameObject.Instantiate(e.EnemyBulletPrefab, bulletPosition.position,Quaternion.identity) as GameObject;
				bullet.GetComponent<EnemyBulletScript>().SetDamage(e.Damage);
				e.NextAttack = e.AttackRate;
			}
		}
		else
		{
			e.ChangeState( Sniper_MoveToPlayer.Instance );
		}
	}
	public override void BeforeExit( EnemySniperScript e )
	{
		e.anim.SetBool ("Attacking", false);	
	}
}