using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossLevelScript : MonoBehaviour {

	public GameObject MiddleFireTrapObject;
	public GameObject[] CornerFireTrapObjects;

	public SpinningFireTrapScript MiddleFireTrap;
	public SpinningFireTrapScript[] CornerFireTraps;

	public float Rotation;
	public float RotationIncrease;

	private float fireTrapTimer;
	public float TrapTimer;

	// Use this for initialization
	void Start () {
		//reset our fire trap timer
		fireTrapTimer = TrapTimer;

		//get all of our fire traps and enable them
		MiddleFireTrap = MiddleFireTrapObject.GetComponent<SpinningFireTrapScript>();
		MiddleFireTrap.EnableSpinningFireTrap();

		CornerFireTraps = new SpinningFireTrapScript[CornerFireTrapObjects.Length];
		for (int i = 0; i < CornerFireTrapObjects.Length; i++)
		{
			CornerFireTraps[i] = CornerFireTrapObjects[i].GetComponent<SpinningFireTrapScript>();
			CornerFireTraps[i].EnableSpinningFireTrap();
		}
	}

	// Update is called once per frame
	void Update()
	{
		//increment the timer
		fireTrapTimer -= Time.deltaTime;

		//check if its time to change the state
		if (fireTrapTimer <= 0)
		{
			//reset our timer
			fireTrapTimer = TrapTimer;

			//disable all corner traps
			foreach (SpinningFireTrapScript trap in CornerFireTraps)
				trap.DisableSpinningFireTrap();

			//reverse the direction of the middle fire trap
			MiddleFireTrap.RotateSpeed = -1 * MiddleFireTrap.RotateSpeed;

			//increase turn rate of all traps
			Rotation += RotationIncrease;
			if (MiddleFireTrap.RotateSpeed > 0)
				MiddleFireTrap.RotateSpeed = Rotation;
			else
				MiddleFireTrap.RotateSpeed = -1 * Rotation;
			foreach (SpinningFireTrapScript trap in CornerFireTraps)
				trap.RotateSpeed = Rotation;

			//enable 2 random corner traps
			int firstTrapEnabled = Random.Range(0, 3);
			int secondTrapEnabled = Random.Range(0, 3);
			while (firstTrapEnabled == secondTrapEnabled)
				secondTrapEnabled = Random.Range(0, 3);
			CornerFireTraps[firstTrapEnabled].EnableSpinningFireTrap();
			CornerFireTraps[secondTrapEnabled].EnableSpinningFireTrap();
			MiddleFireTrap.EnableSpinningFireTrap();
		}
	}}
