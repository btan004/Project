using UnityEngine;

public class PowerupInfo : MonoBehaviour 
{
	/* Powerup Lifetime */
	public static float Lifetime = 60f;

	/* Health Powerup */
	public static Color HealthColor = Color.red;
	public static float HealthMinAmount = 10f;
	public static float HealthMaxAmount = 100f;

	/* Health Regen Powerup */
	public static Color HealthRegenParticleColor = Color.white;
	public static float HealthRegenMinAmount = 1f;
	public static float HealthRegenMaxAmount = 10f;
	public static float HealthRegenMinDuration = 3f;
	public static float HealthRegenMaxDuration = 10f;

	/* Stamina Powerup */
	public static Color StaminaColor = Color.yellow;
	public static float StaminaMinAmount = 1f;
	public static float StaminaMaxAmount = 5f;

	/* Stamina Regen Powerup */
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
	public static Color MovementSpeedColor = new Color(92f / 255f, 33f / 255f, 169f / 255f);
	public static float MovementSpeedMinAmount = 20f;
	public static float MovementSpeedMaxAmount = 50f;
	public static float MovementSpeedMinDuration = 3f;
	public static float MovementSpeedMaxDuration = 10f;

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

}