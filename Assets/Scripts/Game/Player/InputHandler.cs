﻿using UnityEngine;
using System.Collections;

public class InputHandler {

	//Input
	public static Vector3 MovementVector;
	public static Vector3 DirectionVector;
	public static bool		WantToSprint;
	public static bool		WantToAttack;
	public static bool		WantToAura;
	public static bool		WantToSkillShot;
	public static bool		WantToQuit;
	public static bool		WantToSpendSkillPoint;
	public static bool		WantToChangeSkillLeft;
	public static bool		WantToChangeSkillRight;

	// Use this for initialization
	public InputHandler () {
		MovementVector = Vector3.zero;
		DirectionVector = Vector3.zero;
		WantToSprint = false;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		//Check for user input for the player actions
		CheckMovement();
		CheckDirection();
		CheckSprint();
		CheckMeleeAttack();
		CheckAura();
		CheckSkillShot();
		CheckSpendSkillPoint();
		CheckSkillChange();
	}

	//supports: xbox controller and keyboard
	private void CheckMovement()
	{		
		//check for changes in our movement
		MovementVector = Vector3.zero;
		if (Input.GetAxis("Horizontal Movement") < -.5 || Input.GetAxis("Horizontal Movement") > .5)
			MovementVector.x = Input.GetAxis("Horizontal Movement");
		if (Input.GetAxis("Vertical Movement") < -.5 || Input.GetAxis("Vertical Movement") > .5)
			MovementVector.z = -1 * Input.GetAxis ("Vertical Movement");
		MovementVector.Normalize();
	}

	//supports: xbox controller
	private void CheckDirection()
	{
		//check for changes in our direction
		DirectionVector = Vector3.zero;
		if (Mathf.Abs(Input.GetAxis("Horizontal Direction")) > .3)
			DirectionVector.x = Input.GetAxis ("Horizontal Direction");
		if (Mathf.Abs(Input.GetAxis("Vertical Direction")) > .3)
			DirectionVector.z = -1 * Input.GetAxis ("Vertical Direction");
		DirectionVector.Normalize();
	}

	//supports xbox controller and keyboard
	private void CheckSprint()
	{
		if ((Input.GetAxis("Sprint") > 0.5f))
			WantToSprint = true;
		else
			WantToSprint = false;
	}

	//supports: xbox controller
	private void CheckMeleeAttack()
	{
		WantToAttack = (Input.GetAxis("Attack") < -0.5f); 
	}

	//supports: xbox controller
	private void CheckAura()
	{
		WantToAura = Input.GetButton("Aura");
	}

	//supports: xbox controller
	private void CheckSkillShot()
	{
		WantToSkillShot = Input.GetButton("Skill Shot");
	}

	//supports: keyboard
	private void CheckForQuit()
	{
		WantToQuit = Input.GetKey("escape");
	}
	
	private void CheckSpendSkillPoint()
	{
		WantToSpendSkillPoint = Input.GetButton("SpendSkillPoint");
	}

	private void CheckSkillChange()
	{
		WantToChangeSkillLeft = false;
		WantToChangeSkillRight = false;

		if (Input.GetAxis("SkillSelect") < -.5)
			WantToChangeSkillLeft = true;
		if (Input.GetAxis("SkillSelect") > .5)
			WantToChangeSkillRight = true;

	}

}
