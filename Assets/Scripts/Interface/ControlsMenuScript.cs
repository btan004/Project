using UnityEngine;
using System.Collections;

public class ControlsMenuScript : MonoBehaviour {

	public Texture back;
	public Texture black;

	InputHandler inputHandler;

	// Use this for initialization
	void Start () {
		inputHandler = new InputHandler ();
	}
	
	// Update is called once per frame
	void Update () {
		inputHandler.Update();
		if (InputHandler.WantToViewControls){
			Application.LoadLevel("StartMenuScene");
			InputHandler.DisableInput();
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
	}
}
