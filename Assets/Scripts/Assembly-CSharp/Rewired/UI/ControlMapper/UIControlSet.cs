using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C47 RID: 3143
	[AddComponentMenu("")]
	public class UIControlSet : MonoBehaviour
	{
		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06004D49 RID: 19785 RVA: 0x00275990 File Offset: 0x00273D90
		private Dictionary<int, UIControl> controls
		{
			get
			{
				Dictionary<int, UIControl> result;
				if ((result = this._controls) == null)
				{
					result = (this._controls = new Dictionary<int, UIControl>());
				}
				return result;
			}
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x002759B8 File Offset: 0x00273DB8
		public void SetTitle(string text)
		{
			if (this.title == null)
			{
				return;
			}
			this.title.text = text;
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x002759D8 File Offset: 0x00273DD8
		public T GetControl<T>(int uniqueId) where T : UIControl
		{
			UIControl uicontrol;
			this.controls.TryGetValue(uniqueId, out uicontrol);
			return uicontrol as T;
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x00275A00 File Offset: 0x00273E00
		public UISliderControl CreateSlider(GameObject prefab, Sprite icon, float minValue, float maxValue, Action<int, float> valueChangedCallback, Action<int> cancelCallback)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			UISliderControl control = gameObject.GetComponent<UISliderControl>();
			if (control == null)
			{
				UnityEngine.Object.Destroy(gameObject);
				UnityEngine.Debug.LogError("Prefab missing UISliderControl component!");
				return null;
			}
			gameObject.transform.SetParent(base.transform, false);
			if (control.iconImage != null)
			{
				control.iconImage.sprite = icon;
			}
			if (control.slider != null)
			{
				control.slider.minValue = minValue;
				control.slider.maxValue = maxValue;
				if (valueChangedCallback != null)
				{
					control.slider.onValueChanged.AddListener(delegate(float value)
					{
						valueChangedCallback(control.id, value);
					});
				}
				if (cancelCallback != null)
				{
					control.SetCancelCallback(delegate
					{
						cancelCallback(control.id);
					});
				}
			}
			this.controls.Add(control.id, control);
			return control;
		}

		// Token: 0x04005189 RID: 20873
		[SerializeField]
		private Text title;

		// Token: 0x0400518A RID: 20874
		private Dictionary<int, UIControl> _controls;
	}
}
