using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GUIScript : MonoBehaviour {
	
	public PlayerScript playerScript;
	public SpawnScript spawnScript;

	//lives
	private Texture2D 	heartTexture;
	private Texture2D 	greyHeartTexture;
	private string 		heartTexturePath			= "Assets/Resources/Textures/Hearts/Heart.png";
	private string 		greyHeartTexturePath		= "Assets/Resources/Textures/Hearts/GreyHeart.png";

	//scroll bars
	private ProgressBar healthBar;
	private ProgressBar staminaBar;
	private ProgressBar auraBar;
	private ProgressBar skillShotBar;
	private string 		progressBarBackPath 		= "Assets/Resources/Textures/ProgressBar/Back1.png";
	private string 		progressBarYellowPath 		= "Assets/Resources/Textures/ProgressBar/YellowFadebar.png";
	private string 		progressBarRedPath 			= "Assets/Resources/Textures/ProgressBar/RedFadebar.png";
	private string		progressBarBluePath			= "Assets/Resources/Textures/ProgressBar/BlueFadebar.png";
	private string		progressBarGreenPath		= "Assets/Resources/Textures/ProgressBar/GreenFadebar.png";
	private string		progressBarProgressGreyPath = "Assets/Resources/Textures/ProgressBar/GreyFadebar.png";
	private string 		progressBarCoverPath 		= "Assets/Resources/Textures/ProgressBar/Cover1.png";
	private string		healthTextPath				= "Assets/Resources/Textures/ProgressBar/HealthText.png";
	private string		staminaTextPath				= "Assets/Resources/Textures/ProgressBar/StaminaText.png";
	private string		auraTextPath				= "Assets/Resources/Textures/ProgressBar/AuraText.png";
	private string		skillShotTextPath			= "Assets/Resources/Textures/ProgressBar/SkillShotText.png";



	//load resources for the gui
	void Start()
	{
		heartTexture = (Texture2D) Resources.LoadAssetAtPath(heartTexturePath, typeof(Texture2D));
		greyHeartTexture = (Texture2D) Resources.LoadAssetAtPath(greyHeartTexturePath, typeof(Texture2D));

		healthBar = new ProgressBar(new Vector2(10, 60), new Vector2(200, 50),
            (Texture2D) Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
            (Texture2D) Resources.LoadAssetAtPath(progressBarRedPath, typeof(Texture2D)),
            (Texture2D) Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
            (Texture2D) Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
            (Texture2D) Resources.LoadAssetAtPath(healthTextPath, typeof(Texture2D)));

		staminaBar = new ProgressBar(new Vector2(10, 110), new Vector2(200, 50),
			(Texture2D) Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
			(Texture2D) Resources.LoadAssetAtPath(progressBarYellowPath, typeof(Texture2D)),
			(Texture2D) Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
			(Texture2D) Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
			(Texture2D) Resources.LoadAssetAtPath(staminaTextPath, typeof(Texture2D)));

		auraBar = new ProgressBar(new Vector2(10, 160), new Vector2(200, 50),
		    (Texture2D) Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
		    (Texture2D) Resources.LoadAssetAtPath(progressBarBluePath, typeof(Texture2D)),
		    (Texture2D) Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
		    (Texture2D) Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
		    (Texture2D) Resources.LoadAssetAtPath(auraTextPath, typeof(Texture2D)));
		
		skillShotBar = new ProgressBar(new Vector2(10, 210), new Vector2(200, 50),
		     (Texture2D) Resources.LoadAssetAtPath(progressBarBackPath, typeof(Texture2D)),
		     (Texture2D) Resources.LoadAssetAtPath(progressBarGreenPath, typeof(Texture2D)),
		     (Texture2D) Resources.LoadAssetAtPath(progressBarProgressGreyPath, typeof(Texture2D)),
		     (Texture2D) Resources.LoadAssetAtPath(progressBarCoverPath, typeof(Texture2D)),
		     (Texture2D) Resources.LoadAssetAtPath(skillShotTextPath, typeof(Texture2D)));

	}
	
	// Update is called once per frame
	void Update () 
	{
		healthBar.SetPercentage(playerScript.Health / playerScript.TotalHealth);
		
		staminaBar.SetPercentage(playerScript.Stamina / playerScript.TotalStamina);
		if (playerScript.RanOutOfStamina) staminaBar.GreyOut(true);
		else staminaBar.GreyOut(false);

		if (PlayerScript.IsAuraActive || PlayerScript.IsAuraReady) 
		{
			auraBar.GreyOut(false);
			if (PlayerScript.IsAuraReady) auraBar.SetPercentage(1);
			else auraBar.SetPercentage((PlayerScript.auraDurationTimer) / PlayerScript.auraDuration);
		}
		else
		{
			auraBar.GreyOut(true);
			auraBar.SetPercentage((PlayerScript.auraCooldown - PlayerScript.auraCooldownTimer)/ PlayerScript.auraCooldown);
		}

		skillShotBar.SetPercentage(1);
	}

	//Our GUI 
	void OnGUI()
	{
		//Display the current Wave
		GUI.Label(new Rect((Screen.width / 2) - 50, 10, 100, 20), "Current Wave: " + spawnScript.Wave);

		//Dispaly the time until the next wave
		GUI.Label(new Rect((Screen.width / 2) - 78, 35, 200, 20), "Time Until Next Wave: " + (int) spawnScript.TimeUntilNextWave);

		//Display the number of enemies remaining
		GUI.Label(new Rect((Screen.width / 2) - 50, 70, 100, 20), "Enemies Remaining: " + (int) spawnScript.EnemiesRemaining);

		//Display the players Score
		GUI.Label(new Rect(Screen.width - 110, 60, 100, 20), "Score: " + playerScript.Score);

		//display lives
		for (int i = 0; i < PlayerScript.MaxLives; i++)
		{
			if (i < PlayerScript.Lives)
				GUI.DrawTexture(new Rect(10 + (50 * i), 10, 50, 50), heartTexture);
			else
				GUI.DrawTexture(new Rect(10 + (50 * i), 10, 50, 50), greyHeartTexture);
		}

		//display health bar
		healthBar.OnGUI();

		//display stamina bar
		staminaBar.OnGUI();

		//display aura bar
		auraBar.OnGUI();

		//display skill shot bar
		skillShotBar.OnGUI();


		//Display the players Lives
		GUI.Label(new Rect(10, 300, 100, 20), "Lives: " + PlayerScript.Lives);
		
		//Display the players Health
		GUI.Label(new Rect(10, 330, 100, 20), "Health: " + playerScript.Health);
		
		//Display the players Stamina
		GUI.Label (new Rect (10, 360, 100, 20), "Stamina: " + (int) playerScript.Stamina);


	}
}
