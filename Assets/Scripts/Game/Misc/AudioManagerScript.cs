using UnityEngine;
using System.Collections;

public enum MusicType
{
	Home, Level, Boss
}

public class AudioManagerScript : MonoBehaviour {

	public static AudioManagerScript instance;

	/* Background Music */
	public MusicType MusicType;
	public AudioClip BackgroundHomeStart;
	public AudioClip BackgroundHomeLoop;
	public AudioClip BackgroundLevelStart;
	public AudioClip BackgroundLevelLoop;
	public AudioClip BackgroundBossStart;
	public AudioClip BackgroundBossLoop;

	private bool startFinished;

	// Use this for initialization
	void Start () {
		instance = this;
		
		StartHomeMusic();
	}
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying)
		{
			switch (MusicType)
			{
				case MusicType.Home:
					audio.clip = BackgroundHomeLoop;
					break;
				case MusicType.Level:
					audio.clip = BackgroundLevelLoop;
					break;
				case MusicType.Boss:
					audio.clip = BackgroundBossLoop;
					break;
			}
			audio.Play();
		}
	}

	public void StartHomeMusic()
	{
		audio.clip = BackgroundHomeStart;
		audio.Play();
	}

	public void StartLevelMusic()
	{
		audio.clip = BackgroundLevelStart;
		audio.Play();
	}

	public void StartBossMusic()
	{
		audio.clip = BackgroundBossStart;
		audio.Play();
	}
}
