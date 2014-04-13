using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PowerupType
{
	Health, HealthRegen,
	Stamina, StaminaRegen
};

public class PowerupScript : MonoBehaviour {

	public static float RotateSpeed = 50f;
	public static float FloatSpeed = .5f;
	public static float Transparency = 0.8f;
	public static float InitialHeight = 1f;
	public static float MaxHeight = 1.5f;
	private static bool floatUp = true;

	public PowerupType Type;
	public float Amount;
	public float Duration;
	public float Lifetime;

	public static Dictionary<PowerupType, Color> PowerupColors;

	// Use this for initialization
	void Start () {
		//set up the powerup color dictionary
		PowerupColors = new Dictionary<PowerupType, Color>()
		{
			{ PowerupType.Health, Color.red },
			{ PowerupType.HealthRegen, Color.red },
			{ PowerupType.Stamina, Color.yellow },
			{ PowerupType.StaminaRegen, Color.yellow }
		};

		//set our initial height
		transform.SetPositionY(InitialHeight);

		//get this powerups color and fix its transparency
		Color thisColor = PowerupColors[Type];
		thisColor.a = Transparency;

		//for all of the particle systems
		foreach (ParticleSystem t in this.GetComponentsInChildren<ParticleSystem>())
		{
			//set its color
			t.startColor = thisColor;
		}

		//for all of the mesh renderers (exclude particle renderers)
		foreach (MeshRenderer t in this.GetComponentsInChildren<MeshRenderer>())
		{
			//set the material to a transparent/diffuse material
			t.material = new Material(Shader.Find("Transparent/Diffuse"));
			t.material.color = thisColor;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		//rotate the powerup
		transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);

		//make the transform float
		if (transform.position.y <= InitialHeight) floatUp = true;
		if (transform.position.y >= MaxHeight) floatUp = false;
		Vector3 movement = Vector3.up;
		if (!floatUp) movement.y *= -1;
		this.transform.position = (this.transform.position + (movement * FloatSpeed * Time.deltaTime));

		//decrease the lifetime of the powerup
		Lifetime -= Time.deltaTime;

		//check if the powerup has died
		if (Lifetime <= 0)
			Destroy(this.gameObject);
	}
}
