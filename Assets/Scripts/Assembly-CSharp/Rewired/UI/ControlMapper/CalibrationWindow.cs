using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	public class CalibrationWindow : Window
	{
		[SerializeField]
		private RectTransform rightContentContainer;
		[SerializeField]
		private RectTransform valueDisplayGroup;
		[SerializeField]
		private RectTransform calibratedValueMarker;
		[SerializeField]
		private RectTransform rawValueMarker;
		[SerializeField]
		private RectTransform calibratedZeroMarker;
		[SerializeField]
		private RectTransform deadzoneArea;
		[SerializeField]
		private Slider deadzoneSlider;
		[SerializeField]
		private Slider zeroSlider;
		[SerializeField]
		private Slider sensitivitySlider;
		[SerializeField]
		private Toggle invertToggle;
		[SerializeField]
		private RectTransform axisScrollAreaContent;
		[SerializeField]
		private Button doneButton;
		[SerializeField]
		private Button calibrateButton;
		[SerializeField]
		private Text doneButtonLabel;
		[SerializeField]
		private Text cancelButtonLabel;
		[SerializeField]
		private Text defaultButtonLabel;
		[SerializeField]
		private Text deadzoneSliderLabel;
		[SerializeField]
		private Text zeroSliderLabel;
		[SerializeField]
		private Text sensitivitySliderLabel;
		[SerializeField]
		private Text invertToggleLabel;
		[SerializeField]
		private Text calibrateButtonLabel;
		[SerializeField]
		private GameObject axisButtonPrefab;
	}
}
