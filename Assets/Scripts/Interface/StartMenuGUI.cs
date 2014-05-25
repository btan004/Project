using UnityEngine;
using System.Collections;

public class StartMenuGUI : MonoBehaviour {

	private string GameTitle = "The Arena";
	private static int GameTitleWidth = 200;
	private static int GameTitleHeight = 50;

	private string DifficultyString = "Easy";
	private static int DifficultyWidth = 200;
	private static int DifficultyHeight = 50;
	private float difficultyToggleTimer;
	private static float difficultyToggleCooldown = 0.1f;

	//Start Menu Style
	private Font font;
	private string fontPath = "Assets/Resources/Fonts/FreePixel.ttf";
	private GUIStyle style;

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
		font = Resources.LoadAssetAtPath(fontPath, typeof(Font)) as Font;

		//set up our normal style
		style = new GUIStyle();
		style.fontSize = 60;
		style.font = font;
		style.normal.textColor = Color.black;

		//set up the easy difficulty and the difficulty style
		difficultyStyle = new GUIStyle();
		difficultyStyle.fontSize = 60;
		difficultyStyle.font = font;
		difficultyStyle.normal.textColor = Color.green;
		difficulty = Difficulty.Easy;
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
		//Display the Game Title
		GUI.Label(new Rect((Screen.width / 2) - (GameTitleWidth / 2), 50, GameTitleWidth, GameTitleHeight), GameTitle, style);

		//Display the Difficulty
		GUI.Label(new Rect((Screen.width / 2) - (DifficultyWidth / 2), 250, DifficultyWidth, DifficultyHeight), DifficultyString, difficultyStyle); 

		//Display Controls
		GUI.Label(new Rect(Screen.width - 600, 500, controlStringWidth, controlStringHeight), controlStringStartGame, style);
		GUI.Label(new Rect(Screen.width - 600, 500 + controlStringHeight, controlStringWidth, controlStringHeight), controlStringChangeDifficulty, style);
		GUI.Label(new Rect(Screen.width - 600, 500 + controlStringHeight * 2, controlStringWidth, controlStringHeight), controlStringControls, style);
		GUI.Label(new Rect(Screen.width - 600, 500 + controlStringHeight * 3, controlStringWidth, controlStringHeight), controlStringQuitGame, style);

	}

	private void StartGame()
	{
		Application.LoadLevel("GameScene");
	}

	private void QuitGame()
	{
		Application.Quit();
	}
	

}

