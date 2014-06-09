using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameGUI : MonoBehaviour {
	
	public PlayerScript playerScript;
	public SpawnScript spawnScript;

	public static EnemyBaseScript Boss;
	public static bool BossActive;

	//font
	private Font			font;
	private string			fontPath					= "Fonts/FreePixel";

	//lives
	private Texture2D 	heartTexture;
	private Texture2D 	greyHeartTexture;
	private string 		heartTexturePath				= "Textures/Hearts/Heart";
	private string 		greyHeartTexturePath			= "Textures/Hearts/GreyHeart";

	//scroll bars
	private GUIStyle	progressBarStyle;
	private float		progressBarTextOffset;
	private ProgressBar	healthBar;
	private ProgressBar	staminaBar;
	private ProgressBar	auraBar;
	private ProgressBar	skillShotBar;
	private ProgressBar attackDamageBar;
	private ProgressBar skillAvailableBar;
	private ProgressBar bossHealthBar;

	private ProgressBar waveInfoBar;

	private string acceptUpgradeCoverPath = "Textures/ProgressBar/GreenCover";
	private Texture2D acceptUpgradeCover;
	private string denyUpgradeCoverPath = "Textures/ProgressBar/RedCover";
	private Texture2D denyUpgradeCover;

	//skill upgrades
	private static int skillsToDisplay = 5;
	private static int selectedSkill = 0;
	private float skillSelectCooldownTimer = 0f;
	public static float SkillSelectCooldown = 0.2f;
	private float spendSkillTimer = 0f;
	public static float SpendSkillCooldown = 0.2f;

	//skill tooltip display
	private bool displayTooltip;
	private GUIStyle tooltipStyle;
	private float skillShowTooltipTimer = 0f;
	public static float SkillShowTooltipCooldown = 3f;
	public string tooltipLine1;
	public string tooltipLine2;
	public string tooltipLine3;

	public int				MaxPowerupsToDisplay				= 10;

	private string 	progressBarBackPath 				= "Textures/ProgressBar/Back";
	private string 	progressBarYellowPath 			= "Textures/ProgressBar/YellowBar";
	private string 	progressBarRedPath 				= "Textures/ProgressBar/RedBar";
	private string		progressBarBluePath				= "Textures/ProgressBar/BlueBar";
	private string		progressBarGreenPath				= "Textures/ProgressBar/GreenBar";
	private string		progressBarProgressGreyPath	= "Textures/ProgressBar/GreyBar";
	private string		progressBarSkillUpPath			= "Textures/ProgressBar/SkillUpBar";

	private string 	progressBarCoverPath 			= "Textures/ProgressBar/Cover";
	private string		progressBarDarkGreyPath			= "Textures/ProgressBar/DarkGreyBar";

	private string progressBarCoverBigPath = "Textures/ProgressBar/CoverBig";
	private string progressBarBackBigPath = "Textures/ProgressBar/BackBig";

	private string progressBarCoverBossPath = "Textures/ProgressBar/CoverBoss";
	private string progressBarBackBossPath = "Textures/ProgressBar/BackBoss";
	private string progressBarRedBossPath = "Textures/ProgressBar/RedBarBoss";

	private string skillDisplayBackPath			= "Textures/SkillDisplays/Background";
	private string skillDisplayCoverPath		= "Textures/SkillDisplays/Cover";
	private string skillDisplaySelectedPath		= "Textures/SkillDisplays/Selected";
	private string skillHealthIconPath			= "Textures/SkillDisplays/HealthDisplay";
	private string skillStaminaIconPath			= "Textures/SkillDisplays/StaminaDisplay";
	private string skillSpeedIconPath			= "Textures/SkillDisplays/SpeedDisplay";
	private string skillAttackIconPath			= "Textures/SkillDisplays/DamageDisplay";
	private string skillSkillShotIconPath		= "Textures/SkillDisplays/SkillShotDisplay";
	private string skillAuraIconPath				= "Textures/SkillDisplays/AuraDisplay";

	private string skillPointAvailablePath = "Textures/SkillDisplays/SkillAvailable";
	private Texture2D skillPointAvailable;

	private string tooltipBackgroundPath = "Textures/SkillDisplays/TooltipBackground";

	private GUIStyle waveInfoStyle;
	private GUIStyle bossInfoStyle;

	//load resources for the gui
	void Start()
	{
		//load our font
		font = Resources.Load<Font>(fontPath);

		//set our text offset
		progressBarTextOffset = 6;

		//create the style for the title text
		progressBarStyle = new GUIStyle();
		progressBarStyle.fontSize = 14;
		progressBarStyle.font = font;
		progressBarStyle.normal.textColor = Color.black;

		tooltipStyle = new GUIStyle();
		tooltipStyle.fontSize = 14;
		tooltipStyle.font = font;
		tooltipStyle.normal.textColor = Color.black;
		tooltipStyle.normal.background = Resources.Load<Texture2D>(tooltipBackgroundPath);

		waveInfoStyle = new GUIStyle();
		waveInfoStyle.font = font;
		waveInfoStyle.fontSize = 30;
		waveInfoStyle.normal.textColor = Color.black;

		bossInfoStyle = new GUIStyle();
		bossInfoStyle.font = font;
		bossInfoStyle.fontSize = 30;
		bossInfoStyle.normal.textColor = Color.black;

		heartTexture = Resources.Load<Texture2D>(heartTexturePath);
		greyHeartTexture = Resources.Load<Texture2D>(greyHeartTexturePath);

		acceptUpgradeCover = Resources.Load<Texture2D>(acceptUpgradeCoverPath);
		denyUpgradeCover = Resources.Load<Texture2D>(denyUpgradeCoverPath);

		healthBar = new ProgressBar(new Vector2(10, 60), new Vector2(200, 30),
			Resources.Load<Texture2D>(progressBarBackPath),
			Resources.Load<Texture2D>(progressBarRedPath),
			Resources.Load<Texture2D>(progressBarProgressGreyPath),
			Resources.Load<Texture2D>(progressBarCoverPath),
			progressBarStyle,
			progressBarTextOffset);

		staminaBar = new ProgressBar(new Vector2(10, 100), new Vector2(200, 30),
			Resources.Load<Texture2D>(progressBarBackPath),
			Resources.Load<Texture2D>(progressBarYellowPath),
			Resources.Load<Texture2D>(progressBarProgressGreyPath),
			Resources.Load<Texture2D>(progressBarCoverPath),
			progressBarStyle,
			progressBarTextOffset);

		auraBar = new ProgressBar(new Vector2(10, 140), new Vector2(200, 30),
		   Resources.Load<Texture2D>(progressBarBackPath),
		   Resources.Load<Texture2D>(progressBarBluePath),
		   Resources.Load<Texture2D>(progressBarProgressGreyPath),
		   Resources.Load<Texture2D>(progressBarCoverPath),
			progressBarStyle,
			progressBarTextOffset);
		
		skillShotBar = new ProgressBar(new Vector2(10, 180), new Vector2(200, 30),
		   Resources.Load<Texture2D>(progressBarBackPath),
		   Resources.Load<Texture2D>(progressBarGreenPath),
		   Resources.Load<Texture2D>(progressBarProgressGreyPath),
		   Resources.Load<Texture2D>(progressBarCoverPath),
			progressBarStyle,
			progressBarTextOffset);

		attackDamageBar = new ProgressBar(new Vector2(10, 220), new Vector2(200, 30),
			Resources.Load<Texture2D>(progressBarProgressGreyPath),
		   Resources.Load<Texture2D>(progressBarDarkGreyPath),
		   Resources.Load<Texture2D>(progressBarProgressGreyPath),
		   Resources.Load<Texture2D>(progressBarCoverPath),
			progressBarStyle,
			progressBarTextOffset);
		
		skillAvailableBar = new ProgressBar(new Vector2(10, 260), new Vector2(200, 30),
			Resources.Load<Texture2D>(progressBarSkillUpPath),
			Resources.Load<Texture2D>(progressBarDarkGreyPath),
			Resources.Load<Texture2D>(progressBarSkillUpPath),
			Resources.Load<Texture2D>(progressBarCoverPath),
			progressBarStyle,
			progressBarTextOffset);

		waveInfoBar = new ProgressBar(new Vector2((Screen.width / 2) - 200, 15), new Vector2(400, 100),
			Resources.Load<Texture2D>(progressBarBackBigPath),
			Resources.Load<Texture2D>(progressBarBackBigPath),
			Resources.Load<Texture2D>(progressBarBackBigPath),
			Resources.Load<Texture2D>(progressBarCoverBigPath),
			progressBarStyle,
			progressBarTextOffset);

		bossHealthBar = new ProgressBar(new Vector2((Screen.width / 2) - 300, 15), new Vector2(600, 50),
			Resources.Load<Texture2D>(progressBarBackBossPath),
			Resources.Load<Texture2D>(progressBarRedBossPath),
			Resources.Load<Texture2D>(progressBarBackBossPath),
			Resources.Load<Texture2D>(progressBarCoverBossPath),
			bossInfoStyle,
			progressBarTextOffset);

		spawnScript = GameObject.Find ("Spawner").gameObject.GetComponent<SpawnScript> ();

		skillPointAvailable = Resources.Load<Texture2D>(skillPointAvailablePath);

	}

	void DisplaySpawnInfo()
	{
		GUI.Label (new Rect ((Screen.width / 2) - 150, 25, 200, 20), "Round " + spawnScript.waveSystem.RoundNumber + "      Wave " + spawnScript.waveSystem.WaveNumber, waveInfoStyle);

		GUI.Label(new Rect((Screen.width / 2) - 75, 52, 200, 20), "Score: " + PlayerScript.Score.ToString("F0"), waveInfoStyle);

		if (MapSystemScript.instance.GetCurrentLevelType() != LevelType.Home)
		{
			if (WaveSystem.WaveCountdownOccuring)
			{
				GUI.Label(new Rect((Screen.width / 2) - 120, 78, 200, 20), "Next Wave In: " + WaveSystem.spawnWaveTimer.ToString("F0"), waveInfoStyle);
			}
			else if (EnemyContainerScript.instance.GetEnemyCount() == 0)
			{
				GUI.Label(new Rect((Screen.width / 2) - 100, 78, 200, 20), "Wave Finished!", waveInfoStyle);
			}
			else
			{
				GUI.Label(new Rect((Screen.width / 2) - 160, 78, 200, 20), "Enemies Remaining: " + EnemyContainerScript.instance.GetEnemyCount(), waveInfoStyle);
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (!playerScript.CanSprint) staminaBar.GreyOut(true);
		else staminaBar.GreyOut(false);

		skillSelectCooldownTimer -= Time.deltaTime;
		skillShowTooltipTimer -= Time.deltaTime;
		if (InputHandler.WantToChangeSkillUp && skillSelectCooldownTimer < 0)
		{
			selectedSkill = Mathf.Clamp(selectedSkill - 1, 0, skillsToDisplay - 1);
			skillSelectCooldownTimer = SkillSelectCooldown;
			skillShowTooltipTimer = SkillShowTooltipCooldown;
		}
		if (InputHandler.WantToChangeSkillDown && skillSelectCooldownTimer < 0)
		{
			selectedSkill = Mathf.Clamp(selectedSkill + 1, 0, skillsToDisplay - 1);
			skillSelectCooldownTimer = SkillSelectCooldown;
			skillShowTooltipTimer = SkillShowTooltipCooldown;
		}

		spendSkillTimer -= Time.deltaTime;
		if (InputHandler.WantToSpendSkillPoint && spendSkillTimer <= 0)
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
					playerScript.Skills.UpdgradeSkill(SkillType.Aura);
					break;
				case 3:
					playerScript.Skills.UpdgradeSkill(SkillType.SkillShot);
					break;
				case 4:
					playerScript.Skills.UpdgradeSkill(SkillType.Attack);
					break;
				default:
					break;
			}
			spendSkillTimer = SpendSkillCooldown;
		}

		Debug.LogError("Boss Active: " + BossActive);
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
			(playerScript.Skills.GetPlayerHealth() / playerScript.Skills.GetPlayerHealthMax()),
			"Health",
			" " + playerScript.Skills.GetPlayerHealth().ToString("F0") + "/" + playerScript.Skills.GetPlayerHealthMax().ToString("F0"),
			""
		);

		//display stamina bar
		staminaBar.OnGUI(
			(playerScript.Skills.GetPlayerStamina() / playerScript.Skills.GetPlayerStaminaMax()),
			"Stamina",
			"  " + playerScript.Skills.GetPlayerStamina().ToString("F0") + "/" + playerScript.Skills.GetPlayerStaminaMax().ToString("F0"),
			""
		);
		
		//display aura bar
		if (PlayerScript.IsAuraActive || PlayerScript.IsAuraReady) 
		{
			float percent = playerScript.GetAuraDurationPercentage();
			if (PlayerScript.IsAuraReady) percent = 1;
			auraBar.OnGUI(
				percent,
				"Aura Level " + (playerScript.Skills.AuraSkill.Level + 1),
				"",
				""
			);
		}
		else
		{
			auraBar.OnGUI(
				playerScript.GetAuraCooldownPercentage(),
				"Aura Level " + (playerScript.Skills.AuraSkill.Level + 1),
				"",
				""
			);
		}

		//display skill shot bar
		skillShotBar.OnGUI(
			playerScript.GetSkillShotCooldownPercentage(),
			"Skill Shot Level " + (playerScript.Skills.SkillShotSkill.Level + 1),
			"",
			""
		);

		attackDamageBar.OnGUI(
			0f,
			"Attack Damage: " + playerScript.Skills.GetPlayerDamage(),
			"",
			""
		);

		if (BossActive)
		{
			float bossHealthPercentage = (Boss.Health / Boss.MaxHealth);
			bossHealthBar.OnGUI(
				bossHealthPercentage,
				"",
				"         Boss at " + (100.0f * bossHealthPercentage).ToString("F0") + "%",
				""
			);
		}
		else
		{
			waveInfoBar.OnGUI(0f, "", "", "");
			DisplaySpawnInfo();
		}

		if (playerScript.Skills.PointsToSpend > 0)
		{
			//highlight selected skill
			switch (selectedSkill)
			{
				case 0: /* Health */
					GUI.Label(healthBar.GetRect(), acceptUpgradeCover, progressBarStyle);
					break;
				case 1: /* Stamina */
					GUI.Label(staminaBar.GetRect(), acceptUpgradeCover, progressBarStyle);
					break;
				case 2: /* Aura */
					if (playerScript.Skills.AuraSkill.IsFullyUpgraded())
						GUI.Label(auraBar.GetRect(), denyUpgradeCover, progressBarStyle);
					else
						GUI.Label(auraBar.GetRect(), acceptUpgradeCover, progressBarStyle);
					break;
				case 3: /* Skill Shot */
					if (playerScript.Skills.SkillShotSkill.IsFullyUpgraded())
						GUI.Label(skillShotBar.GetRect(), denyUpgradeCover, progressBarStyle);
					else
						GUI.Label(skillShotBar.GetRect(), acceptUpgradeCover, progressBarStyle);
					break;
				case 4: /* Attack */
					if (playerScript.Skills.AttackSkill.IsFullyUpgraded())
						GUI.Label(attackDamageBar.GetRect(), denyUpgradeCover, progressBarStyle);
					else
						GUI.Label(attackDamageBar.GetRect(), acceptUpgradeCover, progressBarStyle);
					break;
			}

			//skill bar
			skillAvailableBar.OnGUI(
				0f,
				"     Skill Points: " + playerScript.Skills.PointsToSpend,
				"",
				""
			);


		}

	}
}
