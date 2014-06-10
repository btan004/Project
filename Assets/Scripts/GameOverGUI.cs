using UnityEngine;
using System.Collections;

public class GameOverGUI : MonoBehaviour {

	InputHandler inputHandler;

	private static string GameOverString = "Game Over!";
	private static string ContinueString = "Press A to continue.";
	private static string ScoreString;
	private static string HighScoreString;
	private static int StringWidth = 200;
	private static int StringHeight = 50;
	public Texture black;
	public Texture back;
	private bool newHighScore = false;

	//Style
	private Font font;
	private Font fontUnifrak;
	private string fontPath = "Fonts/FreePixel";
	private string fontPathUnifrak = "Fonts/UnifrakturCook";
	private GUIStyle style;
	private GUIStyle styleUnifrak;

	// Use this for initialization
	void Start ()
	{
		inputHandler = new InputHandler();

		//load our font
		font = Resources.Load<Font>(fontPath);
		fontUnifrak = Resources.Load<Font> (fontPathUnifrak);

		//set up our normal style
		style = new GUIStyle();
		style.fontSize = 60;
		style.font = font;
		style.normal.textColor = Color.white;

		//set up our unifrak style
		styleUnifrak = new GUIStyle();
		styleUnifrak.fontSize = 80;
		styleUnifrak.font = fontUnifrak;
		styleUnifrak.normal.textColor = Color.white;

		//figure out our score and high score
		if (PlayerPrefs.GetFloat("Score") > PlayerPrefs.GetFloat("HighScore"))
		{
			newHighScore = true;
			PlayerPrefs.SetFloat("HighScore", PlayerPrefs.GetFloat("Score"));
		}

		if (newHighScore)
		{
			ScoreString = "New High Score: " + PlayerPrefs.GetFloat("Score");
		}
		else
		{
			ScoreString = "Score: " + PlayerPrefs.GetFloat("Score");
		}
		HighScoreString = "High Score: " + PlayerPrefs.GetFloat("HighScore");
	}
	
	// Update is called once per frame
	void Update () 
	{
		inputHandler.Update();

		if (InputHandler.WantToStartGame)
			Application.LoadLevel("StartMenuScene");
	}

	void OnGUI()
	{
		// Background
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), black);
		float locx = 0.0f;
		float locy = 0.0f;
		if(Screen.width > 1271){
			float spacex = (Screen.width - 1271)/2.0f;
			locx = spacex;
			float spacey = (Screen.height-725)/2.0f;
			locy = spacey;
			GUI.DrawTexture (new Rect (locx, locy, 1271, 725), back, ScaleMode.ScaleAndCrop);
		}
		else{
			GUI.DrawTexture (new Rect (0, 0, 1271, 725), back, ScaleMode.ScaleAndCrop);
		} 

		// Score
		int x = (Screen.width / 2) - (StringWidth / 2);
		int y = 50;
		GUI.Label(new Rect(locx+(Screen.width/2.0f)-175, y, StringWidth, StringHeight), GameOverString, styleUnifrak);
		GUI.Label(new Rect(locx+(Screen.width/2.0f)-235, y + 200, StringWidth, StringHeight), HighScoreString, style);
		GUI.Label(new Rect(locx+(Screen.width/2.0f)-155, y + 300, StringWidth, StringHeight), ScoreString, style);
		GUI.Label(new Rect(locx+(Screen.width/2.0f)-275, y + 500, StringWidth, StringHeight), ContinueString, style);
	}
}
