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

	private bool newHighScore = false;

	//Style
	private Font font;
	private string fontPath = "Fonts/FreePixel";
	private GUIStyle style;

	// Use this for initialization
	void Start ()
	{
		inputHandler = new InputHandler();

		//load our font
		font = Resources.Load<Font>(fontPath);

		//set up our normal style
		style = new GUIStyle();
		style.fontSize = 60;
		style.font = font;
		style.normal.textColor = Color.black;

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
		int x = (Screen.width / 2) - (StringWidth / 2);
		int y = 50;
		GUI.Label(new Rect(x, y, StringWidth, StringHeight), GameOverString, style);
		GUI.Label(new Rect(x, y + 200, StringWidth, StringHeight), HighScoreString, style);
		GUI.Label(new Rect(x, y + 300, StringWidth, StringHeight), ScoreString, style);
		GUI.Label(new Rect(x, y + 400, StringWidth, StringHeight), ContinueString, style);
	}
}
