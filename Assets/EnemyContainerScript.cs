using UnityEngine;
using System.Collections;

public class EnemyContainerScript : MonoBehaviour
{

	public static EnemyContainerScript instance;

	// Use this for initialization
	void Start()
	{
		instance = this;

	}

	public int GetEnemyCount()
	{
		return instance.GetComponentsInChildren<EnemyBaseScript>().Length;
	}
}
