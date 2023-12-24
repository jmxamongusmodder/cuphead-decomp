using System;
using UnityEngine.UI;
using UnityEngine;

public class RestartTowerConfirmGUI : AbstractMonoBehaviour
{
	[Serializable]
	public class Button
	{
		public Text text;
	}

	[SerializeField]
	private GameObject mainObject;
	[SerializeField]
	private Button[] mainObjectButtons;
}
