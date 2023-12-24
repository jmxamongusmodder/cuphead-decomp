using System;
using UnityEngine.UI;
using UnityEngine;

public class LevelSelectList : AbstractMonoBehaviour
{
	[Serializable]
	public class SceneGroup
	{
		public bool included;
		public Scenes scene;
	}

	public SceneGroup[] scenes;
	public Button button;
	public RectTransform contentPanel;
}
