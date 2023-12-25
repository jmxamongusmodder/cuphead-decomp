using System;
using System.Collections.Generic;
using Rewired.Integration.UnityUI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C07 RID: 3079
	[AddComponentMenu("")]
	public class CalibrationWindow : Window
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06004972 RID: 18802 RVA: 0x0026667C File Offset: 0x00264A7C
		private bool axisSelected
		{
			get
			{
				return this.joystick != null && this.selectedAxis >= 0 && this.selectedAxis < this.joystick.calibrationMap.axisCount;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06004973 RID: 18803 RVA: 0x002666B5 File Offset: 0x00264AB5
		private AxisCalibration axisCalibration
		{
			get
			{
				if (!this.axisSelected)
				{
					return null;
				}
				return this.joystick.calibrationMap.GetAxis(this.selectedAxis);
			}
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x002666DC File Offset: 0x00264ADC
		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this.rightContentContainer == null || this.valueDisplayGroup == null || this.calibratedValueMarker == null || this.rawValueMarker == null || this.calibratedZeroMarker == null || this.deadzoneArea == null || this.deadzoneSlider == null || this.sensitivitySlider == null || this.zeroSlider == null || this.invertToggle == null || this.axisScrollAreaContent == null || this.doneButton == null || this.calibrateButton == null || this.axisButtonPrefab == null || this.doneButtonLabel == null || this.cancelButtonLabel == null || this.defaultButtonLabel == null || this.deadzoneSliderLabel == null || this.zeroSliderLabel == null || this.sensitivitySliderLabel == null || this.invertToggleLabel == null || this.calibrateButtonLabel == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			this.axisButtons = new List<Button>();
			this.buttonCallbacks = new Dictionary<int, Action<int>>();
			this.doneButtonLabel.text = ControlMapper.GetLanguage().done;
			this.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			this.defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			this.deadzoneSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_deadZoneSliderLabel;
			this.zeroSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_zeroSliderLabel;
			this.sensitivitySliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_sensitivitySliderLabel;
			this.invertToggleLabel.text = ControlMapper.GetLanguage().calibrateWindow_invertToggleLabel;
			this.calibrateButtonLabel.text = ControlMapper.GetLanguage().calibrateWindow_calibrateButtonLabel;
			base.Initialize(id, isFocusedCallback);
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x00266930 File Offset: 0x00264D30
		public void SetJoystick(int playerId, Joystick joystick)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			this.joystick = joystick;
			if (joystick == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: Joystick cannot be null!");
				return;
			}
			float num = 0f;
			for (int i = 0; i < joystick.axisCount; i++)
			{
				int index = i;
				GameObject gameObject = UITools.InstantiateGUIObject<Button>(this.axisButtonPrefab, this.axisScrollAreaContent, "Axis" + i);
				Button button = gameObject.GetComponent<Button>();
				button.onClick.AddListener(delegate()
				{
					this.OnAxisSelected(index, button);
				});
				Text componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				if (componentInSelfOrChildren != null)
				{
					componentInSelfOrChildren.text = joystick.AxisElementIdentifiers[i].name;
				}
				if (num == 0f)
				{
					num = UnityTools.GetComponentInSelfOrChildren<LayoutElement>(gameObject).minHeight;
				}
				this.axisButtons.Add(button);
			}
			float spacing = this.axisScrollAreaContent.GetComponent<VerticalLayoutGroup>().spacing;
			this.axisScrollAreaContent.sizeDelta = new Vector2(this.axisScrollAreaContent.sizeDelta.x, Mathf.Max((float)joystick.axisCount * (num + spacing) - spacing, this.axisScrollAreaContent.sizeDelta.y));
			this.origCalibrationData = joystick.calibrationMap.ToXmlString();
			this.displayAreaWidth = this.rightContentContainer.sizeDelta.x;
			this.rewiredStandaloneInputModule = base.gameObject.transform.root.GetComponentInChildren<RewiredStandaloneInputModule>();
			if (this.rewiredStandaloneInputModule != null)
			{
				this.menuHorizActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.horizontalAxis);
				this.menuVertActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.verticalAxis);
			}
			if (joystick.axisCount > 0)
			{
				this.SelectAxis(0);
			}
			base.defaultUIElement = this.doneButton.gameObject;
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x00266B54 File Offset: 0x00264F54
		public void SetButtonCallback(CalibrationWindow.ButtonIdentifier buttonIdentifier, Action<int> callback)
		{
			if (!base.initialized)
			{
				return;
			}
			if (callback == null)
			{
				return;
			}
			if (this.buttonCallbacks.ContainsKey((int)buttonIdentifier))
			{
				this.buttonCallbacks[(int)buttonIdentifier] = callback;
			}
			else
			{
				this.buttonCallbacks.Add((int)buttonIdentifier, callback);
			}
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00266BA4 File Offset: 0x00264FA4
		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick != null)
			{
				this.joystick.ImportCalibrationMapFromXmlString(this.origCalibrationData);
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(1, out action))
			{
				if (this.cancelCallback != null)
				{
					this.cancelCallback();
				}
				return;
			}
			action(base.id);
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x00266C10 File Offset: 0x00265010
		protected override void Update()
		{
			if (!base.initialized)
			{
				return;
			}
			base.Update();
			this.UpdateDisplay();
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x00266C2C File Offset: 0x0026502C
		public void OnDone()
		{
			if (!base.initialized)
			{
				return;
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(0, out action))
			{
				return;
			}
			action(base.id);
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00266C65 File Offset: 0x00265065
		public void OnCancel()
		{
			this.Cancel();
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00266C6D File Offset: 0x0026506D
		public void OnRestoreDefault()
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick == null)
			{
				return;
			}
			this.joystick.calibrationMap.Reset();
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00266CA4 File Offset: 0x002650A4
		public void OnCalibrate()
		{
			if (!base.initialized)
			{
				return;
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(3, out action))
			{
				return;
			}
			action(this.selectedAxis);
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00266CDD File Offset: 0x002650DD
		public void OnInvert(bool state)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.invert = state;
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00266D03 File Offset: 0x00265103
		public void OnZeroValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.calibratedZero = value;
			this.RedrawCalibratedZero();
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00266D2F File Offset: 0x0026512F
		public void OnZeroCancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.calibratedZero = this.origSelectedAxisCalibrationData.zero;
			this.RedrawCalibratedZero();
			this.RefreshControls();
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x00266D6C File Offset: 0x0026516C
		public void OnDeadzoneValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.deadZone = Mathf.Clamp(value, 0f, 0.8f);
			if (value > 0.8f)
			{
				this.deadzoneSlider.value = 0.8f;
			}
			this.RedrawDeadzone();
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x00266DCD File Offset: 0x002651CD
		public void OnDeadzoneCancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.deadZone = this.origSelectedAxisCalibrationData.deadZone;
			this.RedrawDeadzone();
			this.RefreshControls();
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x00266E0C File Offset: 0x0026520C
		public void OnSensitivityValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.sensitivity = Mathf.Clamp(value, this.minSensitivity, float.PositiveInfinity);
			if (value < this.minSensitivity)
			{
				this.sensitivitySlider.value = this.minSensitivity;
			}
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00266E6A File Offset: 0x0026526A
		public void OnSensitivityCancel(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.sensitivity = this.origSelectedAxisCalibrationData.sensitivity;
			this.RefreshControls();
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00266EA0 File Offset: 0x002652A0
		public void OnAxisScrollRectScroll(Vector2 pos)
		{
			if (!base.initialized)
			{
				return;
			}
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00266EAE File Offset: 0x002652AE
		private void OnAxisSelected(int axisIndex, Button button)
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick == null)
			{
				return;
			}
			this.SelectAxis(axisIndex);
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00266EDB File Offset: 0x002652DB
		private void UpdateDisplay()
		{
			this.RedrawValueMarkers();
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00266EE3 File Offset: 0x002652E3
		private void Redraw()
		{
			this.RedrawCalibratedZero();
			this.RedrawValueMarkers();
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x00266EF4 File Offset: 0x002652F4
		private void RefreshControls()
		{
			if (!this.axisSelected)
			{
				this.deadzoneSlider.value = 0f;
				this.zeroSlider.value = 0f;
				this.sensitivitySlider.value = 0f;
				this.invertToggle.isOn = false;
			}
			else
			{
				this.deadzoneSlider.value = this.axisCalibration.deadZone;
				this.zeroSlider.value = this.axisCalibration.calibratedZero;
				this.sensitivitySlider.value = this.axisCalibration.sensitivity;
				this.invertToggle.isOn = this.axisCalibration.invert;
			}
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x00266FA8 File Offset: 0x002653A8
		private void RedrawDeadzone()
		{
			if (!this.axisSelected)
			{
				return;
			}
			float x = this.displayAreaWidth * this.axisCalibration.deadZone;
			this.deadzoneArea.sizeDelta = new Vector2(x, this.deadzoneArea.sizeDelta.y);
			this.deadzoneArea.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.deadzoneArea.anchoredPosition.y);
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x00267040 File Offset: 0x00265440
		private void RedrawCalibratedZero()
		{
			if (!this.axisSelected)
			{
				return;
			}
			this.calibratedZeroMarker.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.calibratedZeroMarker.anchoredPosition.y);
			this.RedrawDeadzone();
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x002670A8 File Offset: 0x002654A8
		private void RedrawValueMarkers()
		{
			if (!this.axisSelected)
			{
				this.calibratedValueMarker.anchoredPosition = new Vector2(0f, this.calibratedValueMarker.anchoredPosition.y);
				this.rawValueMarker.anchoredPosition = new Vector2(0f, this.rawValueMarker.anchoredPosition.y);
				return;
			}
			float axis = this.joystick.GetAxis(this.selectedAxis);
			float num = Mathf.Clamp(this.joystick.GetAxisRaw(this.selectedAxis), -1f, 1f);
			this.calibratedValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * axis, this.calibratedValueMarker.anchoredPosition.y);
			this.rawValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * num, this.rawValueMarker.anchoredPosition.y);
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x002671A8 File Offset: 0x002655A8
		private void SelectAxis(int index)
		{
			if (index < 0 || index >= this.axisButtons.Count)
			{
				return;
			}
			if (this.axisButtons[index] == null)
			{
				return;
			}
			this.axisButtons[index].interactable = false;
			this.axisButtons[index].Select();
			for (int i = 0; i < this.axisButtons.Count; i++)
			{
				if (i != index)
				{
					this.axisButtons[i].interactable = true;
				}
			}
			this.selectedAxis = index;
			this.origSelectedAxisCalibrationData = this.axisCalibration.GetData();
			this.SetMinSensitivity();
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00267261 File Offset: 0x00265661
		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
			if (this.selectedAxis >= 0)
			{
				this.SelectAxis(this.selectedAxis);
			}
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x00267290 File Offset: 0x00265690
		private void SetMinSensitivity()
		{
			if (!this.axisSelected)
			{
				return;
			}
			this.minSensitivity = 0.1f;
			if (this.rewiredStandaloneInputModule != null)
			{
				if (this.IsMenuAxis(this.menuHorizActionId, this.selectedAxis))
				{
					this.GetAxisButtonDeadZone(this.playerId, this.menuHorizActionId, ref this.minSensitivity);
				}
				else if (this.IsMenuAxis(this.menuVertActionId, this.selectedAxis))
				{
					this.GetAxisButtonDeadZone(this.playerId, this.menuVertActionId, ref this.minSensitivity);
				}
			}
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x00267328 File Offset: 0x00265728
		private bool IsMenuAxis(int actionId, int axisIndex)
		{
			if (this.rewiredStandaloneInputModule == null)
			{
				return false;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			int count = allPlayers.Count;
			for (int i = 0; i < count; i++)
			{
				IList<JoystickMap> maps = allPlayers[i].controllers.maps.GetMaps<JoystickMap>(this.joystick.id);
				if (maps != null)
				{
					int count2 = maps.Count;
					for (int j = 0; j < count2; j++)
					{
						IList<ActionElementMap> axisMaps = maps[j].AxisMaps;
						if (axisMaps != null)
						{
							int count3 = axisMaps.Count;
							for (int k = 0; k < count3; k++)
							{
								ActionElementMap actionElementMap = axisMaps[k];
								if (actionElementMap.actionId == actionId && actionElementMap.elementIndex == axisIndex)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0026741C File Offset: 0x0026581C
		private void GetAxisButtonDeadZone(int playerId, int actionId, ref float value)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				return;
			}
			int behaviorId = action.behaviorId;
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			value = inputBehavior.buttonDeadZone + 0.1f;
		}

		// Token: 0x04004F83 RID: 20355
		private const float minSensitivityOtherAxes = 0.1f;

		// Token: 0x04004F84 RID: 20356
		private const float maxDeadzone = 0.8f;

		// Token: 0x04004F85 RID: 20357
		[SerializeField]
		private RectTransform rightContentContainer;

		// Token: 0x04004F86 RID: 20358
		[SerializeField]
		private RectTransform valueDisplayGroup;

		// Token: 0x04004F87 RID: 20359
		[SerializeField]
		private RectTransform calibratedValueMarker;

		// Token: 0x04004F88 RID: 20360
		[SerializeField]
		private RectTransform rawValueMarker;

		// Token: 0x04004F89 RID: 20361
		[SerializeField]
		private RectTransform calibratedZeroMarker;

		// Token: 0x04004F8A RID: 20362
		[SerializeField]
		private RectTransform deadzoneArea;

		// Token: 0x04004F8B RID: 20363
		[SerializeField]
		private Slider deadzoneSlider;

		// Token: 0x04004F8C RID: 20364
		[SerializeField]
		private Slider zeroSlider;

		// Token: 0x04004F8D RID: 20365
		[SerializeField]
		private Slider sensitivitySlider;

		// Token: 0x04004F8E RID: 20366
		[SerializeField]
		private Toggle invertToggle;

		// Token: 0x04004F8F RID: 20367
		[SerializeField]
		private RectTransform axisScrollAreaContent;

		// Token: 0x04004F90 RID: 20368
		[SerializeField]
		private Button doneButton;

		// Token: 0x04004F91 RID: 20369
		[SerializeField]
		private Button calibrateButton;

		// Token: 0x04004F92 RID: 20370
		[SerializeField]
		private Text doneButtonLabel;

		// Token: 0x04004F93 RID: 20371
		[SerializeField]
		private Text cancelButtonLabel;

		// Token: 0x04004F94 RID: 20372
		[SerializeField]
		private Text defaultButtonLabel;

		// Token: 0x04004F95 RID: 20373
		[SerializeField]
		private Text deadzoneSliderLabel;

		// Token: 0x04004F96 RID: 20374
		[SerializeField]
		private Text zeroSliderLabel;

		// Token: 0x04004F97 RID: 20375
		[SerializeField]
		private Text sensitivitySliderLabel;

		// Token: 0x04004F98 RID: 20376
		[SerializeField]
		private Text invertToggleLabel;

		// Token: 0x04004F99 RID: 20377
		[SerializeField]
		private Text calibrateButtonLabel;

		// Token: 0x04004F9A RID: 20378
		[SerializeField]
		private GameObject axisButtonPrefab;

		// Token: 0x04004F9B RID: 20379
		private Joystick joystick;

		// Token: 0x04004F9C RID: 20380
		private string origCalibrationData;

		// Token: 0x04004F9D RID: 20381
		private int selectedAxis = -1;

		// Token: 0x04004F9E RID: 20382
		private AxisCalibrationData origSelectedAxisCalibrationData;

		// Token: 0x04004F9F RID: 20383
		private float displayAreaWidth;

		// Token: 0x04004FA0 RID: 20384
		private List<Button> axisButtons;

		// Token: 0x04004FA1 RID: 20385
		private Dictionary<int, Action<int>> buttonCallbacks;

		// Token: 0x04004FA2 RID: 20386
		private int playerId;

		// Token: 0x04004FA3 RID: 20387
		private RewiredStandaloneInputModule rewiredStandaloneInputModule;

		// Token: 0x04004FA4 RID: 20388
		private int menuHorizActionId = -1;

		// Token: 0x04004FA5 RID: 20389
		private int menuVertActionId = -1;

		// Token: 0x04004FA6 RID: 20390
		private float minSensitivity;

		// Token: 0x02000C08 RID: 3080
		public enum ButtonIdentifier
		{
			// Token: 0x04004FA8 RID: 20392
			Done,
			// Token: 0x04004FA9 RID: 20393
			Cancel,
			// Token: 0x04004FAA RID: 20394
			Default,
			// Token: 0x04004FAB RID: 20395
			Calibrate
		}
	}
}
