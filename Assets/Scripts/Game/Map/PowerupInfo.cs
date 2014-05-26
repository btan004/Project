using UnityEngine;

public class PowerupInfo : MonoBehaviour 
{
	/* Powerup Lifetime */
	public static float Lifetime = 60f;

	/* Health Powerup */
	public static Color HealthColor = new Color(125f / 255f, 34f / 255f, 37f / 255f, 1);
	public static float HealthMinAmount = 10f;
	public static float HealthMaxAmount = 100f;

	/* Health Regen Powerup */
	private static Texture2D healthRegenIcon;
	private static string healthRegenIconPath = "Textures/Icons/RedPlusPowerup";
	public static Color HealthRegenParticleColor = Color.white;
	public static float HealthRegenMinAmount = 1f;
	public static float HealthRegenMaxAmount = 10f;
	public static float HealthRegenMinDuration = 3f;
	public static float HealthRegenMaxDuration = 10f;

	/* Stamina Powerup */
	public static Color StaminaColor = new Color(216f / 255f, 163f / 255f, 84f / 255f, 1);
	public static float StaminaMinAmount = 1f;
	public static float StaminaMaxAmount = 5f;

	/* Stamina Regen Powerup */
	private static Texture2D staminaRegenIcon;
	private static string staminaRegenIconPath = "Textures/Icons/YellowPlusPowerup";
	public static Color StaminaRegenParticleColor = Color.white;
	public static float StaminaRegenMinAmount = .5f;
	public static float StaminaRegenMaxAmount = 2f;
	public static float StaminaRegenMinDuration = 3f;
	public static float StaminaRegenMaxDuration = 10f;

	/* Experience Powerup */
	public static Color ExperienceColor = Color.gray;
	public static float ExperienceMinAmount = 20f;
	public static float ExperienceMaxAmount = 30f;

	/* Movespeed Powerup */
	private static Texture2D movespeedIcon;
	private static string movementSpeedIconPath = "Textures/Icons/PurplePlusPowerup";
	public static Color MovementSpeedColor = new Color(92f / 255f, 33f / 255f, 169f / 255f);
	public static float MovementSpeedMinAmount = 1.2f;
	public static float MovementSpeedMaxAmount = 2.0f;
	public static float MovementSpeedMinDuration = 3f;
	public static float MovementSpeedMaxDuration = 10f;
	public static float MovementSpeedCap = 2.0f;

	public static Color GetColor(PowerupType type)
	{
		//if our color is magenta, that means we didnt find the color
		Color c = Color.magenta;
		switch (type)
		{
			case (PowerupType.Health):
				c = HealthColor;
				break;
			case (PowerupType.HealthRegen):
				c = HealthColor;
				break;
			case (PowerupType.Stamina):
				c = StaminaColor;
				break;
			case (PowerupType.StaminaRegen):
				c = StaminaColor;
				break;
			case (PowerupType.Experience):
				c = ExperienceColor;
				break;
			case (PowerupType.MovementSpeed):
				c = MovementSpeedColor;
				break;
		}
		return c;
	}

	public static Color GetParticleColor(PowerupType type)
	{
		//if our color is magenta, that means we didnt find the color
		Color c = Color.magenta;
		switch (type)
		{
			case (PowerupType.Health):
				c = HealthColor;
				break;
			case (PowerupType.HealthRegen):
				c = HealthRegenParticleColor;
				break;
			case (PowerupType.Stamina):
				c = StaminaColor;
				break;
			case (PowerupType.StaminaRegen):
				c = StaminaRegenParticleColor;
				break;
			case (PowerupType.Experience):
				c = ExperienceColor;
				break;
			case (PowerupType.MovementSpeed):
				c = MovementSpeedColor;
				break;
		}
		return c;
	}

	public static Texture2D GetHealthRegenIcon()
	{
		if (healthRegenIcon == null)
			healthRegenIcon = Resources.Load<Texture2D>(healthRegenIconPath);
		return healthRegenIcon;
	}

	public static Texture2D GetStaminaRegenIcon()
	{
		if (staminaRegenIcon == null)
			staminaRegenIcon = Resources.Load<Texture2D>(staminaRegenIconPath);
		return staminaRegenIcon;
	}

	public static Texture2D GetMovespeedIcon()
	{
		if (movespeedIcon == null)
			movespeedIcon = Resources.Load<Texture2D>(movementSpeedIconPath);
		return movespeedIcon;
	}

	public static Texture2D GetIcon(PowerupType type)
	{
		switch (type)
		{
			case (PowerupType.HealthRegen):
				return GetHealthRegenIcon();
			case (PowerupType.StaminaRegen):
				return GetStaminaRegenIcon();
			case (PowerupType.MovementSpeed):
				return GetMovespeedIcon();
		}

		return null;
	}
}