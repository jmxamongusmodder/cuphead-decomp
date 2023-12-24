using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	public class InputBehaviorWindow : Window
	{
		[SerializeField]
		private RectTransform spawnTransform;
		[SerializeField]
		private Button doneButton;
		[SerializeField]
		private Button cancelButton;
		[SerializeField]
		private Button defaultButton;
		[SerializeField]
		private Text doneButtonLabel;
		[SerializeField]
		private Text cancelButtonLabel;
		[SerializeField]
		private Text defaultButtonLabel;
		[SerializeField]
		private GameObject uiControlSetPrefab;
		[SerializeField]
		private GameObject uiSliderControlPrefab;
	}
}
