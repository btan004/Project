using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class SkillDisplay
	{
		private Texture2D backgroundTexture;
		private Texture2D coverTexture;
		private Texture2D iconTexture;
		private Texture2D selectedTexture;

		private Vector2 position;
		private Vector2 size;

		int level;

		private GUIStyle style;

		public SkillDisplay(Vector2 position, Vector2 size, Texture2D background, Texture2D cover, Texture2D icon, Texture2D selected, GUIStyle guiStyle)
		{
			this.position = position;
			this.size = size;
			backgroundTexture = background;
			coverTexture = cover;
			iconTexture = icon;
			selectedTexture = selected;
			style = guiStyle;
		}

		public void Update(int level)
		{
			this.level = level;
		}

		public void OnGUI(bool isSelected)
		{
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), backgroundTexture);
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), iconTexture);
			GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), coverTexture);
			GUI.Label(new Rect(position.x + 30, position.y + 30, 20, 20), level.ToString(), style);
			if (isSelected)
				GUI.DrawTexture(new Rect(position.x, position.y, size.x, size.y), selectedTexture);
		}
	}
}
