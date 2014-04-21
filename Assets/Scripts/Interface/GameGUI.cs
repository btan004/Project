﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameGUI : MonoBehaviour {
	
	public PlayerScript playerScript;
	public SpawnScript spawnScript;

	//font
	private Font			font;
	private string			fontPath								= "Assets/Resources/Fonts/FreePixel.ttf";

	//lives
	private Texture2D 	heartTexture;
	private Texture2D 	greyHeartTexture;
	private string 		heartTexturePath					= "Assets/Resources/Textures/Hearts/Heart.png";
	private string 		greyHeartTexturePath				= "Assets/Resources/Textures/Hearts/GreyHeart.png";

	//scroll bars
	private GUIStyle		progressBarStyle;
	private float			progressBarTextOffset;
	private ProgressBar	healthBar;
	private ProgressBar	staminaBar;
	private ProgressBar	auraBar;
	private ProgressBar	skillShotBar;

	private ProgressBar	experienceBar;
	private ProgressBar	movementSpeedBar;
	private ProgressBar	pickupRadiusBar;
	private ProgressBar	powerupBar;
	private ProgressBar	skillPointsBar;

	//skill displays
	private GUIStyle skillDisplayStyle;
	private List<SkillDisplay> skillDisplays = new List<SkillDisplay>();
	private static int skillsToDisplay = 3;
	private static int selectedSkill = 1;
	private float skillSelectCooldownTimer = 0f;
	public static float SkillSelectCooldown = 0.2f;
	private SkillDisplay healthSkillDisplay;
	private SkillDisplay staminaSkillDisplay;
	private SkillDisplay speedSkillDisplay;

	//skill tooltip display
	private bool displayTooltip;
	private GUIStyle tooltipStyle;
	private float skillShowTooltipTimer = 0f;
	public static float SkillShowTooltipCooldown = 3f;
	public string tooltipLine1;
	public string tooltipLine2;
	public string tooltipLine3;

	public int				MaxPowerupsToDisplay				= 10;

	private string 		progressBarBackPath 				= "Assets/Resources/Textures/ProgressBar/Back.png";
	private string 		progressBarYellowPath 			= "Assets/Resources/Textures/ProgressBar/YellowBar.png";
	private string 		progressBarRedPath 				= "Assets/Resources/Textures/ProgressBar/RedBar.png";
	private string			progressBarBluePath				= "Assets/Resources/Textures/ProgressBar/BlueBar.png";
	private string			progressBarGreenPath				= "Assets/Resources/Textures/ProgressBar/GreenBar.png";
	private string			progressBarProgressGreyPath	= "Assets/Resources/Textures/ProgressBar/GreyBar.png";
	private string 		progressBarCoverPath 			= "Assets/Resources/Textures/ProgressBar/Cover.png";
	private string			progressBarDarkGreyPath			= "Assets/Resources/Textures/ProgressBar/DarkGreyBar.png";
	private string			progressBarPurplePath			= "Assets/Resources/Textures/ProgressBar/PurpleBar.png";

	private string skillDisplayBackPath = "Assets/Resources/Textures/SkillDisplays/Background.png";
	private string skillDisplayCoverPath = "Assets/Resources/Textures/SkillDisplays/Cover.png";
	private string skillDisplaySelectedPath = "Assets/Resources/Textures/SkillDisplays/Selected.png";
	private string skillHealthIconPath = "Assets/Resources/Textures/SkillDisplays/HealthDisplay.png";
	private string skillStaminaIconPath = "Assets/Resources/Textures/SkillDisplays/StaminaDisplay.png";
	private string skillSpeedIconPath = "Assets/Resources/Textures/SkillDisplays/SpeedDisplay.png";

	private string tooltipBackgroundPath = "Assets/Resources/Textures/SkillDisplays/TooltipBackground.png";

	//load resources for the gui
	void Start()
	{
		//load our font
		font = Resources.LoadAssetAtPath(fontPath, typeof(Font)) as Font; ;

		//set our text offset
		progressBarTextOffset = 6;

		//create the style for the title text
		progressBarStyle = new GUIStyle();
		progressBarStyle.fontSize = 14;
		progressBarStyle.font = font;
		progressBarStyle.normal.textColor = Color.black;//new Color(74f / 255f, 74f / 255f, 74f / 255f);

		//create the style for the skill displays
		skillDisplayStyle = new GUIStyle();
		skillDisplayStyle.fontSize = 14;
		skillDisplayStyle.font = font;
		skillDisplayStyle.normal.textColor = Color.black;

		tooltipStyle = new GUIStyle();
		tooltipStyle.fontSize = 14;
		tooltipStyle.font = font;
		tooltipStyle.normal.textColor = Color.black;
		tooltipStyle.normal.background = (Texture2D) Resources.LoadAssetAtPath(tooltipBackgroundPath, typeof(Texture2D));

		heartTexture = (Texture2D) Resources.LoadAssetAtPath(heartTexturePath, typeof(Texture2D));
		greyHeartTexture = (Texture2D) Resources.LoadAssetAtPath(greyHeartTexturePath, typeof(Texture2D));

		healthBar = new ProgressBar(new Vector2(10, 60), new Vector2(200, 30),
			(Texture2D)Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarRedPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

		staminaBar = new ProgressBar(new Vector2(10, 100), new Vector2(200, 30),
			(Texture2D) Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D) Resources.LoadAssetAtPath(progressBarYellowPath, typeof(Texture2D)),
			(Texture2D) Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D) Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

		auraBar = new ProgressBar(new Vector2(10, 140), new Vector2(200, 30),
		   (Texture2D) Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
		   (Texture2D) Resources.LoadAssetAtPath(progressBarBluePath, typeof(Texture2D)),
		   (Texture2D) Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
		   (Texture2D) Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);
		
		skillShotBar = new ProgressBar(new Vector2(10, 180), new Vector2(200, 30),
		   (Texture2D) Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
		   (Texture2D) Resources.LoadAssetAtPath(progressBarGreenPath, typeof(Texture2D)),
		   (Texture2D) Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
		   (Texture2D) Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

		experienceBar = new ProgressBar(new Vector2(10, 220), new Vector2(200, 30),
			(Texture2D)Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarDarkGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

		movementSpeedBar = new ProgressBar(new Vector2(10, 260), new Vector2(200, 30),
			(Texture2D)Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarDarkGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

		pickupRadiusBar = new ProgressBar(new Vector2(10, 300), new Vector2(200, 30),
			(Texture2D)Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarDarkGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

		skillPointsBar = new ProgressBar(new Vector2(10, 340), new Vector2(200, 30),
			(Texture2D)Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarDarkGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

		powerupBar = new ProgressBar(new Vector2(Screen.width - 250, 10), new Vector2(200, 30),
			(Texture2D)Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarDarkGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

		healthSkillDisplay = new SkillDisplay(new Vector2(100, Screen.height - 100), new Vector2(50, 50),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplayBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillHealthIconPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplayCoverPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplaySelectedPath, typeof(Texture2D)),
			skillDisplayStyle);

		staminaSkillDisplay = new SkillDisplay(new Vector2(160, Screen.height - 100), new Vector2(50, 50),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplayBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillStaminaIconPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplayCoverPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplaySelectedPath, typeof(Texture2D)),
			skillDisplayStyle);

		speedSkillDisplay = new SkillDisplay(new Vector2(220, Screen.height - 100), new Vector2(50, 50),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplayBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillSpeedIconPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplayCoverPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(skillDisplaySelectedPath, typeof(Texture2D)),
			skillDisplayStyle);

		skillDisplays.Add(healthSkillDisplay);
		skillDisplays.Add(staminaSkillDisplay);
		skillDisplays.Add(speedSkillDisplay);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerScript.RanOutOfStamina) staminaBar.GreyOut(true);
		else staminaBar.GreyOut(false);

		healthSkillDisplay.Update(1);
		staminaSkillDisplay.Update(1);
		speedSkillDisplay.Update(1);

		skillSelectCooldownTimer -= Time.deltaTime;
		skillShowTooltipTimer -= Time.deltaTime;
		if (InputHandler.WantToChangeSkillLeft && skillSelectCooldownTimer < 0)
		{
			selectedSkill = Mathf.Clamp(selectedSkill - 1, 0, skillsToDisplay - 1);
			skillSelectCooldownTimer = SkillSelectCooldown;
			skillShowTooltipTimer = SkillShowTooltipCooldown;
		}
		if (InputHandler.WantToChangeSkillRight && skillSelectCooldownTimer < 0)
		{
			selectedSkill = Mathf.Clamp(selectedSkill + 1, 0, skillsToDisplay - 1);
			skillSelectCooldownTimer = SkillSelectCooldown;
			skillShowTooltipTimer = SkillShowTooltipCooldown;
		}
		
		if (InputHandler.WantToSpendSkillPoint)
		{
			switch (selectedSkill)
			{
				case 0:
					playerScript.Skills.UpdgradeSkill(SkillType.Health);
					break;
				case 1:
					playerScript.Skills.UpdgradeSkill(SkillType.Stamina);
					break;
				case 2:
					playerScript.Skills.UpdgradeSkill(SkillType.Speed);
					break;
				default:
					break;
			}
		}

		//update tooltip
		switch (selectedSkill)
		{
			case 0:
				tooltipLine1 = playerScript.Skills.HealthSkill.Name;
				tooltipLine2 = playerScript.Skills.HealthSkill.Description;
				tooltipLine3 = "Current Amount: " + playerScript.Skills.HealthSkill.CurrentAmount + ", Next Amount: " + playerScript.Skills.HealthSkill.NextAmount;
				break;
			case 1:
				tooltipLine1 = playerScript.Skills.StaminaSkill.Name;
				tooltipLine2 = playerScript.Skills.StaminaSkill.Description;
				tooltipLine3 = "Current Amount: " + playerScript.Skills.StaminaSkill.CurrentAmount + ", Next Amount: " + playerScript.Skills.StaminaSkill.NextAmount;
				break;
			case 2:
				tooltipLine1 = playerScript.Skills.SpeedSkill.Name;
				tooltipLine2 = playerScript.Skills.SpeedSkill.Description;
				tooltipLine3 = "Current Amount: " + playerScript.Skills.SpeedSkill.CurrentAmount + ", Next Amount: " + playerScript.Skills.SpeedSkill.NextAmount;
				break;
			default:
				break;
		}
	}

	//Our GUI 
	void OnGUI()
	{
		//display lives
		for (int i = 0; i < PlayerScript.MaxLives; i++)
		{
			if (i < PlayerScript.Lives)
				GUI.DrawTexture(new Rect(10 + (50 * i), 10, 50, 50), heartTexture);
			else
				GUI.DrawTexture(new Rect(10 + (50 * i), 10, 50, 50), greyHeartTexture);
		}

		//display health bar
		healthBar.OnGUI(
			(playerScript.Health / playerScript.TotalHealth),
			"Health",
			" " + playerScript.Health.ToString("F0") + "/" + playerScript.TotalHealth.ToString("F0"),
			(100 * (playerScript.Health / playerScript.TotalHealth)).ToString("F0") + "%"
		);

		//display stamina bar
		
		staminaBar.OnGUI(
			(playerScript.Stamina / playerScript.TotalStamina),
			"Stamina",
			"  " + playerScript.Stamina.ToString("F0") + "/" + playerScript.TotalStamina.ToString("F0"),
			(100 * (playerScript.Stamina / playerScript.TotalStamina)).ToString("F0") + "%"
		);
		
		//display aura bar
		if (PlayerScript.IsAuraActive || PlayerScript.IsAuraReady) 
		{
			float percent = playerScript.GetAuraDurationPercentage();
			if (PlayerScript.IsAuraReady) percent = 1;
			auraBar.OnGUI(
				percent,
				"Aura",
				"",
				PlayerScript.IsAuraReady ? "100%" : (100 * playerScript.GetAuraDurationPercentage()).ToString("F0") + "%"
			);
		}
		else
		{
			auraBar.OnGUI(
				playerScript.GetAuraCooldownPercentage(),
				"Aura",
				"",
				(100 * playerScript.GetAuraCooldownPercentage()).ToString("F0") + "%"
			);
		}

		//display skill shot bar
		skillShotBar.OnGUI(
			playerScript.GetSkillShotCooldownPercentage(),
			"Skill Shot",
			"",
			(100 * playerScript.GetSkillShotCooldownPercentage()).ToString("F0") + "%"
		);
		
		//display the experience bar
		experienceBar.OnGUI(
			(playerScript.LevelSystem.CurrentExperience / playerScript.LevelSystem.ExperienceToNextLevel),
			"Experience",
			"  " + playerScript.LevelSystem.CurrentExperience.ToString("F0") + "/" + playerScript.LevelSystem.ExperienceToNextLevel,
			playerScript.LevelSystem.CurrentLevel.ToString()
		);

		movementSpeedBar.OnGUI(
			0f,
			"Movement Speed: " + playerScript.FinalMoveSpeed.ToString("F0"),
			"",
			""
		);

		pickupRadiusBar.OnGUI(
			0f,
			"Pickup Radius: " + playerScript.Radius.ToString("F0"),
			"",
			""
		);

		skillPointsBar.OnGUI(
			0f,
			"Skill Points: " + playerScript.Skills.PointsToSpend.ToString("F0"),
			"",
			""
		);

		powerupBar.OnGUI(
			0f,
			"Powerups:",
			"",
			""
		);

		//display powerups
		int powerupsToDisplay = Mathf.Min(playerScript.ActivePowerups.Count, MaxPowerupsToDisplay);
		Rect powerupDisplayRect = new Rect(Screen.width - 250, 50, 60, 60);
		Rect powerupTextDisplayRect = new Rect(powerupDisplayRect.x + 70, powerupDisplayRect.y, 200, 60);
		for (int i = 0; i < powerupsToDisplay; i++)
		{
			Powerup powerup = playerScript.ActivePowerups[i];
			
			GUI.DrawTexture(powerupDisplayRect, PowerupInfo.GetIcon(powerup.Type));
			GUI.Label(powerupTextDisplayRect, "+" + powerup.Amount.ToString("F1") + " for " + powerup.Duration.ToString("F2") + " seconds");
			powerupDisplayRect.y += 68;
			powerupTextDisplayRect.y += 68;
		}

		//display skills
		for (int i = 0; i < skillsToDisplay; i++)
		{
			skillDisplays[i].OnGUI(selectedSkill == i);
		}

		//display tooltip for skills
		if (skillShowTooltipTimer > 0)
		{
			Rect tooltipRect = new Rect((Screen.width / 2) - 200, Screen.height - 200, 400, 50);
			GUI.Box(tooltipRect, "", tooltipStyle);
			GUI.Label(new Rect(tooltipRect.left + 5, tooltipRect.top + 3, tooltipRect.width - 10, 10), tooltipLine1, skillDisplayStyle);
			GUI.Label(new Rect(tooltipRect.left + 5, tooltipRect.top + 17, tooltipRect.width - 10, 10), tooltipLine2, skillDisplayStyle);
			GUI.Label(new Rect(tooltipRect.left + 5, tooltipRect.top + 32, tooltipRect.width - 10, 10), tooltipLine3, skillDisplayStyle);
		}
	}
}
