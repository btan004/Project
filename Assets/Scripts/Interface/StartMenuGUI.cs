using UnityEngine;
using System.Collections;

public class StartMenuGUI : MonoBehaviour {

	private string GameTitle = "The Arena";
	private string StartGameButtonText = "Start Game";
	private string QuitGameButtonText = "Quit Game";

	void OnGUI()
	{
		GUI.Label(new Rect((Screen.width / 2) + 15, 150, 200, 30), GameTitle);

		if (GUI.Button(new Rect((Screen.width / 2) - 50, 200, 200, 30), StartGameButtonText))
		{
			StartGame();
		}

		if (GUI.Button(new Rect((Screen.width / 2) - 50, 250, 200, 30), QuitGameButtonText))
		{
			QuitGame();
		}
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
