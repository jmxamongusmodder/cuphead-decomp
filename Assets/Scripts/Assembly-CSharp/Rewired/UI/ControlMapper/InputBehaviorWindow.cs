using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C2E RID: 3118
	[AddComponentMenu("")]
	public class InputBehaviorWindow : Window
	{
		// Token: 0x06004C73 RID: 19571 RVA: 0x002732AC File Offset: 0x002716AC
		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this.spawnTransform == null || this.doneButton == null || this.cancelButton == null || this.defaultButton == null || this.uiControlSetPrefab == null || this.uiSliderControlPrefab == null || this.doneButtonLabel == null || this.cancelButtonLabel == null || this.defaultButtonLabel == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			this.inputBehaviorInfo = new List<InputBehaviorWindow.InputBehaviorInfo>();
			this.buttonCallbacks = new Dictionary<int, Action<int>>();
			this.doneButtonLabel.text = ControlMapper.GetLanguage().done;
			this.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			this.defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			base.Initialize(id, isFocusedCallback);
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x002733BC File Offset: 0x002717BC
		public void SetData(int playerId, ControlMapper.InputBehaviorSettings[] data)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			foreach (ControlMapper.InputBehaviorSettings inputBehaviorSettings in data)
			{
				if (inputBehaviorSettings != null && inputBehaviorSettings.isValid)
				{
					InputBehavior inputBehavior = this.GetInputBehavior(inputBehaviorSettings.inputBehaviorId);
					if (inputBehavior != null)
					{
						UIControlSet uicontrolSet = this.CreateControlSet();
						Dictionary<int, InputBehaviorWindow.PropertyType> dictionary = new Dictionary<int, InputBehaviorWindow.PropertyType>();
						string customEntry = ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.labelLanguageKey);
						if (!string.IsNullOrEmpty(customEntry))
						{
							uicontrolSet.SetTitle(customEntry);
						}
						else
						{
							uicontrolSet.SetTitle(inputBehavior.name);
						}
						if (inputBehaviorSettings.showJoystickAxisSensitivity)
						{
							UISliderControl uisliderControl = this.CreateSlider(uicontrolSet, inputBehavior.id, null, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.joystickAxisSensitivityLabelLanguageKey), inputBehaviorSettings.joystickAxisSensitivityIcon, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax, new Action<int, int, float>(this.JoystickAxisSensitivityValueChanged), new Action<int, int>(this.JoystickAxisSensitivityCanceled));
							uisliderControl.slider.value = Mathf.Clamp(inputBehavior.joystickAxisSensitivity, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax);
							dictionary.Add(uisliderControl.id, InputBehaviorWindow.PropertyType.JoystickAxisSensitivity);
						}
						if (inputBehaviorSettings.showMouseXYAxisSensitivity)
						{
							UISliderControl uisliderControl2 = this.CreateSlider(uicontrolSet, inputBehavior.id, null, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.mouseXYAxisSensitivityLabelLanguageKey), inputBehaviorSettings.mouseXYAxisSensitivityIcon, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax, new Action<int, int, float>(this.MouseXYAxisSensitivityValueChanged), new Action<int, int>(this.MouseXYAxisSensitivityCanceled));
							uisliderControl2.slider.value = Mathf.Clamp(inputBehavior.mouseXYAxisSensitivity, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax);
							dictionary.Add(uisliderControl2.id, InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity);
						}
						this.inputBehaviorInfo.Add(new InputBehaviorWindow.InputBehaviorInfo(inputBehavior, uicontrolSet, dictionary));
					}
				}
			}
			base.defaultUIElement = this.doneButton.gameObject;
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x0027359C File Offset: 0x0027199C
		public void SetButtonCallback(InputBehaviorWindow.ButtonIdentifier buttonIdentifier, Action<int> callback)
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

		// Token: 0x06004C76 RID: 19574 RVA: 0x002735EC File Offset: 0x002719EC
		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
			{
				inputBehaviorInfo.RestorePreviousData();
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

		// Token: 0x06004C77 RID: 19575 RVA: 0x0027368C File Offset: 0x00271A8C
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

		// Token: 0x06004C78 RID: 19576 RVA: 0x002736C5 File Offset: 0x00271AC5
		public void OnCancel()
		{
			this.Cancel();
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x002736D0 File Offset: 0x00271AD0
		public void OnRestoreDefault()
		{
			if (!base.initialized)
			{
				return;
			}
			foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
			{
				inputBehaviorInfo.RestoreDefaultData();
			}
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x00273738 File Offset: 0x00271B38
		private void JoystickAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
		{
			this.GetInputBehavior(inputBehaviorId).joystickAxisSensitivity = value;
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x00273747 File Offset: 0x00271B47
		private void MouseXYAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
		{
			this.GetInputBehavior(inputBehaviorId).mouseXYAxisSensitivity = value;
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x00273758 File Offset: 0x00271B58
		private void JoystickAxisSensitivityCanceled(int inputBehaviorId, int controlId)
		{
			InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo = this.GetInputBehaviorInfo(inputBehaviorId);
			if (inputBehaviorInfo == null)
			{
				return;
			}
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.JoystickAxisSensitivity, controlId);
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x0027377C File Offset: 0x00271B7C
		private void MouseXYAxisSensitivityCanceled(int inputBehaviorId, int controlId)
		{
			InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo = this.GetInputBehaviorInfo(inputBehaviorId);
			if (inputBehaviorInfo == null)
			{
				return;
			}
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity, controlId);
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x002737A0 File Offset: 0x00271BA0
		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x002737A8 File Offset: 0x00271BA8
		private UIControlSet CreateControlSet()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.uiControlSetPrefab);
			gameObject.transform.SetParent(this.spawnTransform, false);
			return gameObject.GetComponent<UIControlSet>();
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x002737DC File Offset: 0x00271BDC
		private UISliderControl CreateSlider(UIControlSet set, int inputBehaviorId, string defaultTitle, string overrideTitle, Sprite icon, float minValue, float maxValue, Action<int, int, float> valueChangedCallback, Action<int, int> cancelCallback)
		{
			UISliderControl uisliderControl = set.CreateSlider(this.uiSliderControlPrefab, icon, minValue, maxValue, delegate(int cId, float value)
			{
				valueChangedCallback(inputBehaviorId, cId, value);
			}, delegate(int cId)
			{
				cancelCallback(inputBehaviorId, cId);
			});
			string text = (!string.IsNullOrEmpty(overrideTitle)) ? overrideTitle : defaultTitle;
			if (!string.IsNullOrEmpty(text))
			{
				uisliderControl.showTitle = true;
				uisliderControl.title.text = text;
			}
			else
			{
				uisliderControl.showTitle = false;
			}
			uisliderControl.showIcon = (icon != null);
			return uisliderControl;
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x0027387F File Offset: 0x00271C7F
		private InputBehavior GetInputBehavior(int id)
		{
			return ReInput.mapping.GetInputBehavior(this.playerId, id);
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x00273894 File Offset: 0x00271C94
		private InputBehaviorWindow.InputBehaviorInfo GetInputBehaviorInfo(int inputBehaviorId)
		{
			int count = this.inputBehaviorInfo.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.inputBehaviorInfo[i].inputBehavior.id == inputBehaviorId)
				{
					return this.inputBehaviorInfo[i];
				}
			}
			return null;
		}

		// Token: 0x040050D3 RID: 20691
		private const float minSensitivity = 0.1f;

		// Token: 0x040050D4 RID: 20692
		[SerializeField]
		private RectTransform spawnTransform;

		// Token: 0x040050D5 RID: 20693
		[SerializeField]
		private Button doneButton;

		// Token: 0x040050D6 RID: 20694
		[SerializeField]
		private Button cancelButton;

		// Token: 0x040050D7 RID: 20695
		[SerializeField]
		private Button defaultButton;

		// Token: 0x040050D8 RID: 20696
		[SerializeField]
		private Text doneButtonLabel;

		// Token: 0x040050D9 RID: 20697
		[SerializeField]
		private Text cancelButtonLabel;

		// Token: 0x040050DA RID: 20698
		[SerializeField]
		private Text defaultButtonLabel;

		// Token: 0x040050DB RID: 20699
		[SerializeField]
		private GameObject uiControlSetPrefab;

		// Token: 0x040050DC RID: 20700
		[SerializeField]
		private GameObject uiSliderControlPrefab;

		// Token: 0x040050DD RID: 20701
		private List<InputBehaviorWindow.InputBehaviorInfo> inputBehaviorInfo;

		// Token: 0x040050DE RID: 20702
		private Dictionary<int, Action<int>> buttonCallbacks;

		// Token: 0x040050DF RID: 20703
		private int playerId;

		// Token: 0x02000C2F RID: 3119
		private class InputBehaviorInfo
		{
			// Token: 0x06004C83 RID: 19587 RVA: 0x002738EE File Offset: 0x00271CEE
			public InputBehaviorInfo(InputBehavior inputBehavior, UIControlSet controlSet, Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty)
			{
				this._inputBehavior = inputBehavior;
				this._controlSet = controlSet;
				this.idToProperty = idToProperty;
				this.copyOfOriginal = new InputBehavior(inputBehavior);
			}

			// Token: 0x1700075E RID: 1886
			// (get) Token: 0x06004C84 RID: 19588 RVA: 0x00273917 File Offset: 0x00271D17
			public InputBehavior inputBehavior
			{
				get
				{
					return this._inputBehavior;
				}
			}

			// Token: 0x1700075F RID: 1887
			// (get) Token: 0x06004C85 RID: 19589 RVA: 0x0027391F File Offset: 0x00271D1F
			public UIControlSet controlSet
			{
				get
				{
					return this._controlSet;
				}
			}

			// Token: 0x06004C86 RID: 19590 RVA: 0x00273927 File Offset: 0x00271D27
			public void RestorePreviousData()
			{
				this._inputBehavior.ImportData(this.copyOfOriginal);
			}

			// Token: 0x06004C87 RID: 19591 RVA: 0x0027393B File Offset: 0x00271D3B
			public void RestoreDefaultData()
			{
				this._inputBehavior.Reset();
				this.RefreshControls();
			}

			// Token: 0x06004C88 RID: 19592 RVA: 0x00273950 File Offset: 0x00271D50
			public void RestoreData(InputBehaviorWindow.PropertyType propertyType, int controlId)
			{
				if (propertyType != InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
				{
					if (propertyType == InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
					{
						float mouseXYAxisSensitivity = this.copyOfOriginal.mouseXYAxisSensitivity;
						this._inputBehavior.mouseXYAxisSensitivity = mouseXYAxisSensitivity;
						UISliderControl control = this._controlSet.GetControl<UISliderControl>(controlId);
						if (control != null)
						{
							control.slider.value = mouseXYAxisSensitivity;
						}
					}
				}
				else
				{
					float joystickAxisSensitivity = this.copyOfOriginal.joystickAxisSensitivity;
					this._inputBehavior.joystickAxisSensitivity = joystickAxisSensitivity;
					UISliderControl control2 = this._controlSet.GetControl<UISliderControl>(controlId);
					if (control2 != null)
					{
						control2.slider.value = joystickAxisSensitivity;
					}
				}
			}

			// Token: 0x06004C89 RID: 19593 RVA: 0x002739F4 File Offset: 0x00271DF4
			public void RefreshControls()
			{
				if (this._controlSet == null)
				{
					return;
				}
				if (this.idToProperty == null)
				{
					return;
				}
				foreach (KeyValuePair<int, InputBehaviorWindow.PropertyType> keyValuePair in this.idToProperty)
				{
					UISliderControl control = this._controlSet.GetControl<UISliderControl>(keyValuePair.Key);
					if (!(control == null))
					{
						InputBehaviorWindow.PropertyType value = keyValuePair.Value;
						if (value != InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
						{
							if (value == InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
							{
								control.slider.value = this._inputBehavior.mouseXYAxisSensitivity;
							}
						}
						else
						{
							control.slider.value = this._inputBehavior.joystickAxisSensitivity;
						}
					}
				}
			}

			// Token: 0x040050E0 RID: 20704
			private InputBehavior _inputBehavior;

			// Token: 0x040050E1 RID: 20705
			private UIControlSet _controlSet;

			// Token: 0x040050E2 RID: 20706
			private Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty;

			// Token: 0x040050E3 RID: 20707
			private InputBehavior copyOfOriginal;
		}

		// Token: 0x02000C30 RID: 3120
		public enum ButtonIdentifier
		{
			// Token: 0x040050E5 RID: 20709
			Done,
			// Token: 0x040050E6 RID: 20710
			Cancel,
			// Token: 0x040050E7 RID: 20711
			Default
		}

		// Token: 0x02000C31 RID: 3121
		private enum PropertyType
		{
			// Token: 0x040050E9 RID: 20713
			JoystickAxisSensitivity,
			// Token: 0x040050EA RID: 20714
			MouseXYAxisSensitivity
		}
	}
}
