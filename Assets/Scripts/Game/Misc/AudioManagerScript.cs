using UnityEngine;
using System.Collections;

public class AudioManagerScript : MonoBehaviour {

	public static AudioManagerScript instance;

	/* Background Music */
	public AudioClip BackgroundHome;
	public AudioClip BackgroundLevel;
	public AudioClip BackgroundBoss;

	// Use this for initialization
	void Start () {
		instance = this;

		StartHomeMusic();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartHomeMusic()
	{
		audio.clip = BackgroundHome;
		audio.Play();
	}

	public void StartLevelMusic()
	{
		audio.clip = BackgroundLevel;
		audio.Play();
	}

	public void StartBossMusic()
	{
		audio.clip = BackgroundBoss;
		audio.Play();
	}
}
