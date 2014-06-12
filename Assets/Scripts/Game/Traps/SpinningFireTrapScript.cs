using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpinningFireTrapScript : MonoBehaviour {

	public float RotateSpeed = 1f;
	public float Damage = 10f;
	public float TimeOff = 5f;
	public float TimeOn = 5f;

	private List<FireTrapScript> fireTraps = new List<FireTrapScript>();

	// Use this for initialization
	void Start () {

		//get a random delay to start from
		float timeDelay = Random.Range(0.0f, 6.0f);

		//get references to all of our fire traps
		foreach(FireTrapScript s in this.GetComponentsInChildren<FireTrapScript>())
		{
			fireTraps.Add(s);
			s.Damage = Damage;
			s.TimeOff = 5f;
			s.TimeOn = 5f;
			s.timer = timeDelay;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//spin the trap
		transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);

		//update our fire traps in case of any changes
		foreach(FireTrapScript s in fireTraps)
		{
			s.Damage = Damage;
			s.TimeOff = 5f;
			s.TimeOn = 5f;
		}
	}

	public void EnableSpinningFireTrap()
	{
		foreach (FireTrapScript trap in fireTraps)
		{
			trap.TurnTrapOn();
		}
	}

	public void DisableSpinningFireTrap()
	{
		foreach (FireTrapScript trap in fireTraps)
		{
			trap.TurnTrapOff();
		}
	}
}
