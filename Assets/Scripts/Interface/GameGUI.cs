using UnityEngine;
using System.Collections;
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

		powerupBar = new ProgressBar(new Vector2(Screen.width - 250, 10), new Vector2(200, 30),
			(Texture2D)Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarDarkGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D)Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			progressBarStyle,
			progressBarTextOffset);

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerScript.RanOutOfStamina) staminaBar.GreyOut(true);
		else staminaBar.GreyOut(false);

		/*
		if (PlayerScript.IsAuraActive || PlayerScript.IsAuraReady) 
		{
			auraBar.GreyOut(false);
		}
		else
		{
			auraBar.GreyOut(true);
		}
		 */
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
			(playerScript.Experience / playerScript.ExperienceToNextLevel),
			"Experience",
			"  " + playerScript.Experience + "/" + playerScript.ExperienceToNextLevel,
			(100 * playerScript.Experience / playerScript.ExperienceToNextLevel).ToString("F0") + "%"
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
			GUI.Label(powerupTextDisplayRect, "+" + powerup.Amount.ToString("F1") + " for " + powerup.Amount.ToString("F2") + " seconds");
			powerupDisplayRect.y += 68;
			powerupTextDisplayRect.y += 68;
		}
	}
}
