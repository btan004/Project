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
		private float		percent;
		private bool		greyedOut;

		public ProgressBar(Vector2 position, Vector2 size, Texture2D back, Texture2D progressBar, Texture2D greyBar, Texture2D cover, Texture2D text)
		{

			this.position = position;
			this.size = size;
			
			progressBarBack = back;
			progressBarProgress = progressBar;
			progressBarGrey = greyBar;
			progressBarCover = cover;
			progressBarText = text;
		}

		public void OnGUI()
		{
			//display stamina bar
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), progressBarBack);
		
			GUI.BeginGroup(new Rect(position.x, position.y, size.x * percent, size.y));
			if (!greyedOut) GUI.DrawTexture(new Rect(0, 0, size.x, size.y), progressBarProgress);
			else 			GUI.DrawTexture(new Rect(0, 0, size.x, size.y), progressBarGrey);
			GUI.EndGroup();
		
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), progressBarCover);

			GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), progressBarText);
		}

		public void SetPercentage(float p)
		{
			percent = p;
		}

		public void GreyOut(bool status)
		{
			greyedOut = status;
		}
	}
}




