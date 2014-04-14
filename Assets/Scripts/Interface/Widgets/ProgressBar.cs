using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class ProgressBar
	{
		//stamina scroll bar
		private Texture2D 	progressBarBack;
		private Texture2D 	progressBarProgress;
		private Texture2D 	progressBarGrey;
		private Texture2D 	progressBarCover;
		private Texture2D 	progressBarText;

		private Vector2 	position;
		private Vector2 	size;
		private float		textOffset;
		private float		percent;
		private bool		greyedOut;
		private GUIStyle  style;

		public ProgressBar(Vector2 position, Vector2 size, Texture2D back, Texture2D progressBar, Texture2D greyBar, Texture2D cover, GUIStyle style, float textOffset)
		{
			this.position = position;
			this.size = size;
			this.textOffset = textOffset;
			this.style = style;

			progressBarBack = back;
			progressBarProgress = progressBar;
			progressBarGrey = greyBar;
			progressBarCover = cover;
		}

		public void OnGUI(float percent, string textLeft, string textMiddle, string textRight)
		{
			//set our percentage
			this.percent = percent;

			//draw our background texture
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), progressBarBack);
		
			//draw our bar
			GUI.BeginGroup(new Rect(position.x, position.y, size.x * percent, size.y));
			if (!greyedOut) GUI.DrawTexture(new Rect(0, 0, size.x, size.y), progressBarProgress);
			else 			GUI.DrawTexture(new Rect(0, 0, size.x, size.y), progressBarGrey);
			GUI.EndGroup();
		
			//draw our cover
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), progressBarCover);

			//draw our left text
			//Rect textLeftRect = GUILayoutUtility.GetRect(new GUIContent(textLeft), style);
			//textLeftRect.x = position.x + textOffset;
			//textLeftRect.y = position.y + ((size.y - textLeftRect.height)/ 2.0f);
			//GUI.Label(textLeftRect, textLeft, style);
			GUI.Label(new Rect(position.x + 7, position.y + 8, size.x, size.y), textLeft, style);

			//draw our middle text
			//Rect textMiddleRect = GUILayoutUtility.GetRect(new GUIContent(textMiddle), style);
			//textMiddleRect.x = position.x + ((size.x - textMiddleRect.width) / 2.0f) + 10;
			//textMiddleRect.y = position.y + ((size.y - textMiddleRect.height) / 2.0f);
			//GUI.Label(textMiddleRect, textMiddle, style);
			GUI.Label(new Rect(position.x + 80, position.y + 8, size.x, size.y), textMiddle, style);

			//draw our right text
			//Rect textRightRect = GUILayoutUtility.GetRect(new GUIContent(textRight), style);
			//textRightRect.x = position.x + size.x - textOffset - 32;
			//textRightRect.y = position.y + ((size.y - textRightRect.height) / 2.0f);
			//GUI.Label(textRightRect, textRight, style);
			GUI.Label(new Rect(position.x + 166, position.y + 8, size.x, size.y), textRight, style);
		}

		public void GreyOut(bool status)
		{
			greyedOut = status;
		}
	}
}




