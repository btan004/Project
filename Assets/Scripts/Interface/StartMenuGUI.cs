using UnityEngine;
using System.Collections;

public class StartMenuGUI : MonoBehaviour
{

	private string GameTitleThe = "The";
	private string GameTitleArena = "Arena";
	private static int GameTitleWidth = 200;
	private static int GameTitleHeight = 50;

	private string DifficultyString = "Easy";
	private static int DifficultyWidth = 200;
	private static int DifficultyHeight = 50;
	private float difficultyToggleTimer;
	private static float difficultyToggleCooldown = 0.1f;

	//Start Menu Style
	private Font font;
	private Font fontFree;
	private string fontPath = "Fonts/UnifrakturCook";
	private string fontPathFree = "Fonts/FreePixel";
	private GUIStyle style;
	private GUIStyle styleThe;
	private GUIStyle styleArena;
	public Texture back;
	public Texture black;

	private GUIStyle difficultyStyle;
	private Difficulty difficulty;

	private GUIStyle controlStyle;
	private string controlStringStartGame = "A: Start Game";
	private string controlStringChangeDifficulty = "B: Change Difficulty";
	private string controlStringControls = "X: View Controls";
	private string controlStringQuitGame = "Y: Quit Game";
	private static int controlStringWidth = 200;
	private static int controlStringHeight = 50;


	InputHandler inputHandler;


	void Start()
	{
		//load our font
		font = Resources.Load<Font>(fontPath);
		fontFree = Resources.Load<Font>(fontPathFree);

		//set up our normal style
		style = new GUIStyle();
		style.fontSize = 40;
		style.font = fontFree;
		style.normal.textColor = Color.white;

		//set up our normal style
		styleThe = new GUIStyle();
		styleThe.fontSize = 50;
		styleThe.font = font;
		styleThe.normal.textColor = Color.white;

		//set up our normal style
		styleArena = new GUIStyle();
		styleArena.fontSize = 85;
		styleArena.font = font;
		styleArena.normal.textColor = Color.white;

		//set up the easy difficulty and the difficulty style
		difficultyStyle = new GUIStyle();
		difficultyStyle.fontSize = 60;
		difficultyStyle.font = font;
		difficultyStyle.alignment = TextAnchor.MiddleCenter;
		difficultyStyle.normal.textColor = Color.green;
		difficulty = Difficulty.Normal;
		difficultyToggleTimer = difficultyToggleCooldown;

		inputHandler = new InputHandler();

		PlayerPrefs.SetString("Difficulty", "Easy");
	}

	void Update()
	{
		inputHandler.Update();

		if (InputHandler.WantToStartGame) StartGame();
		if (InputHandler.WantToChangeDifficulty && difficultyToggleTimer < 0)
		{
			toggleDifficulty();
			difficultyToggleTimer = difficultyToggleCooldown;
		}
		if (InputHandler.WantToViewControls) {
			Application.LoadLevel("ControlsScene");
			InputHandler.DisableInput();
		}
		if (InputHandler.WantToQuit) QuitGame();

		difficultyToggleTimer -= Time.deltaTime;
	}



	private void toggleDifficulty()
	{
		Debug.Log("Toggling difficulty");

		if (difficulty == Difficulty.Easy)
		{
			difficulty = Difficulty.Normal;
			difficultyStyle.normal.textColor = Color.green;
			DifficultyString = "Easy";
			PlayerPrefs.SetString("Difficulty", "Easy");
		}
		else if (difficulty == Difficulty.Normal)
		{
			difficulty = Difficulty.Hard;
			difficultyStyle.normal.textColor = Color.yellow;
			DifficultyString = "Normal";
			PlayerPrefs.SetString("Difficulty", "Normal");
		}
		else
		{
			difficulty = Difficulty.Easy;
			difficultyStyle.normal.textColor = Color.red;
			DifficultyString = "Hard";
			PlayerPrefs.SetString("Difficulty", "Hard");
		}
	}

	void OnGUI()
	{
		// Background
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), black);
		float locx = 0.0f;
		float locy = 0.0f;
		if(Screen.width > 1212){
			float spacex = (Screen.width - 1212)/2.0f;
			locx = spacex;
			float spacey = (Screen.height-726)/2.0f;
			locy = spacey;
			GUI.DrawTexture (new Rect (locx, locy, 1212, 726), back, ScaleMode.ScaleAndCrop);
		}
		else{
			GUI.DrawTexture (new Rect (0, 0, 1212, 726), back, ScaleMode.ScaleAndCrop);
		} 

		//Display the Game Title
		GUI.Label(new Rect(locx+195, 50+locy, GameTitleWidth, GameTitleHeight), GameTitleThe, styleThe);
		GUI.Label(new Rect(locx+130, 70+locy, GameTitleWidth, GameTitleHeight), GameTitleArena, styleArena);

		//Display the Difficulty
		GUI.Label(new Rect(locx+90, 250+locy, 285, DifficultyHeight), DifficultyString, difficultyStyle);

		//Display Controls
		GUI.Label(new Rect(locx+1212 - 410, locy + 540, controlStringWidth, controlStringHeight-10), controlStringStartGame, style);
		GUI.Label(new Rect(locx+1212 - 410, locy + 540 + controlStringHeight-10, controlStringWidth, controlStringHeight), controlStringChangeDifficulty, style);
		GUI.Label(new Rect(locx+1212 - 410, locy + 540 + controlStringHeight * 2 -20, controlStringWidth, controlStringHeight), controlStringControls, style);
		GUI.Label(new Rect(locx+1212 - 410, locy + 540 + controlStringHeight * 3 -30, controlStringWidth, controlStringHeight), controlStringQuitGame, style);

	}

	private void StartGame()
	{
		Application.LoadLevel("GameScene");
		InputHandler.DisableInput();
	}

	private void QuitGame()
	{
		Debug.Log("Quitting Game...");
		Application.Quit();
	}


}

