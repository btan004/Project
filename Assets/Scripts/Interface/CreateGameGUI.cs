using UnityEngine;
using System.Collections;

public class CreateGameGUI : MonoBehaviour {

	private enum Section
	{
		ClassSelection,
		AuraSelection,
		SkillShotSelection
	};

	private Font font;
	private string fontPath = "Assets/Resources/Fonts/FreePixel.ttf";

	private GUIStyle titleStyle;
	private string titleText = "Create Your Character";
	private Rect titleRect;

	private GUIStyle textStyle;

	void Start()
	{
		//load our font
		font = Resources.LoadAssetAtPath(fontPath, typeof(Font)) as Font;

		//create the style for the title text
		titleStyle = new GUIStyle();
		titleStyle.fontSize = 36;
		titleStyle.font = font;

		//create the style for the other text
		textStyle = new GUIStyle();
		titleStyle.fontSize = 28;
		titleStyle.font = font;
		
	}

	void OnGUI()
	{
		//Title Text
		titleRect = GUILayoutUtility.GetRect(new GUIContent(titleText), titleStyle);
		titleRect.x += ((Screen.width - titleRect.width) / 2f);
		titleRect.y += 20;
		GUI.Label(titleRect, titleText, titleStyle);

		//Class Selection

	}
}
