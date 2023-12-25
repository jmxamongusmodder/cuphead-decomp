using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C3A RID: 3130
	[Serializable]
	public class ThemeSettings : ScriptableObject
	{
		// Token: 0x06004CEF RID: 19695 RVA: 0x00274BE0 File Offset: 0x00272FE0
		public void Apply(ThemedElement.ElementInfo[] elementInfo)
		{
			if (elementInfo == null)
			{
				return;
			}
			for (int i = 0; i < elementInfo.Length; i++)
			{
				if (elementInfo[i] != null)
				{
					this.Apply(elementInfo[i].themeClass, elementInfo[i].component);
				}
			}
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x00274C2C File Offset: 0x0027302C
		private void Apply(string themeClass, Component component)
		{
			if (component as Selectable != null)
			{
				this.Apply(themeClass, (Selectable)component);
				return;
			}
			if (component as Image != null)
			{
				this.Apply(themeClass, (Image)component);
				return;
			}
			if (component as Text != null)
			{
				this.Apply(themeClass, (Text)component);
				return;
			}
			if (component as UIImageHelper != null)
			{
				this.Apply(themeClass, (UIImageHelper)component);
				return;
			}
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x00274CB8 File Offset: 0x002730B8
		private void Apply(string themeClass, Selectable item)
		{
			if (item == null)
			{
				return;
			}
			ThemeSettings.SelectableSettings_Base selectableSettings_Base;
			if (item as Button != null)
			{
				if (themeClass != null)
				{
					if (themeClass == "inputGridField")
					{
						selectableSettings_Base = this._inputGridFieldSettings;
						goto IL_A5;
					}
					if (themeClass == "windowButton")
					{
						selectableSettings_Base = this._windowButtonSettings;
						goto IL_A5;
					}
					if (themeClass == "playerButton")
					{
						selectableSettings_Base = this._playerButtonSettings;
						goto IL_A5;
					}
					if (themeClass == "playerDropdownButton")
					{
						selectableSettings_Base = this._inputGridFieldSettings;
						goto IL_A5;
					}
				}
				selectableSettings_Base = this._buttonSettings;
				IL_A5:;
			}
			else if (item as Scrollbar != null)
			{
				selectableSettings_Base = this._scrollbarSettings;
			}
			else if (item as Slider != null)
			{
				selectableSettings_Base = this._sliderSettings;
			}
			else if (item as Toggle != null)
			{
				if (themeClass != null)
				{
					if (themeClass == "inputGridField")
					{
						selectableSettings_Base = this._inputGridFieldSettings;
						goto IL_144;
					}
					if (themeClass == "button")
					{
						selectableSettings_Base = this._buttonSettings;
						goto IL_144;
					}
				}
				selectableSettings_Base = this._selectableSettings;
				IL_144:;
			}
			else
			{
				selectableSettings_Base = this._selectableSettings;
			}
			selectableSettings_Base.Apply(item);
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x00274E1C File Offset: 0x0027321C
		private void Apply(string themeClass, Image item)
		{
			if (item == null)
			{
				return;
			}
			switch (themeClass)
			{
			case "area":
				this._areaBackground.CopyTo(item);
				break;
			case "popupWindow":
				this._popupWindowBackground.CopyTo(item);
				break;
			case "mainWindow":
				this._mainWindowBackground.CopyTo(item);
				break;
			case "calibrationValueMarker":
				this._calibrationValueMarker.CopyTo(item);
				break;
			case "calibrationRawValueMarker":
				this._calibrationRawValueMarker.CopyTo(item);
				break;
			case "invertToggle":
				this._invertToggle.CopyTo(item);
				break;
			case "invertToggleBackground":
				this._inputGridFieldSettings.imageSettings.CopyTo(item);
				item.sprite = this._inputGridFieldSettings.imageSettings.sprite;
				break;
			case "invertToggleButtonBackground":
				this._buttonSettings.imageSettings.CopyTo(item);
				break;
			}
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x00274F98 File Offset: 0x00273398
		private void Apply(string themeClass, Text item)
		{
			if (item == null)
			{
				return;
			}
			ThemeSettings.TextSettings textSettings;
			switch (themeClass)
			{
			case "button":
				textSettings = this._buttonTextSettings;
				goto IL_171;
			case "windowButton":
				textSettings = this._windowButtonTextSettings;
				goto IL_171;
			case "playerButton":
				textSettings = this._playerButtonTextSettings;
				goto IL_171;
			case "playerDropdownButton":
				textSettings = this._playerDropdownButtonTextSettings;
				goto IL_171;
			case "restoreDefaultButton":
				textSettings = this._restoreDefaultButtonTextSettings;
				goto IL_171;
			case "inputGridField":
				textSettings = this._inputGridFieldTextSettings;
				goto IL_171;
			case "actionsColumn":
				textSettings = this._actionColumnTextSettings;
				goto IL_171;
			case "actionsColumnDeactivated":
				textSettings = this._actionColumnDeactivatedTextSettings;
				goto IL_171;
			case "actionsColumnHeader":
				textSettings = this._actionColumnHeaderTextSettings;
				goto IL_171;
			case "inputColumnHeader":
				textSettings = this._inputColumnHeaderTextSettings;
				goto IL_171;
			}
			textSettings = this._textSettings;
			IL_171:
			if (textSettings.fontTypes != null && (Localization.Languages)textSettings.fontTypes.Length > Localization.language)
			{
				item.font = FontLoader.GetFont(textSettings.fontTypes[(int)Localization.language]);
			}
			item.color = textSettings.color;
			item.lineSpacing = textSettings.lineSpacing;
			if (textSettings.sizeMultiplier != 1f)
			{
				item.fontSize = (int)((float)item.fontSize * textSettings.sizeMultiplier);
				item.resizeTextMaxSize = (int)((float)item.resizeTextMaxSize * textSettings.sizeMultiplier);
				item.resizeTextMinSize = (int)((float)item.resizeTextMinSize * textSettings.sizeMultiplier);
			}
			if (textSettings.overrideSize != 0)
			{
				item.fontSize = textSettings.overrideSize;
				item.resizeTextMaxSize = textSettings.overrideSize;
				item.resizeTextMinSize = textSettings.overrideSize;
			}
			if ((Localization.Languages)textSettings.style.Length > Localization.language && textSettings.style[(int)Localization.language] != ThemeSettings.FontStyleOverride.Default)
			{
				item.fontStyle = (FontStyle)(textSettings.style[(int)Localization.language] - 1);
			}
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x00275217 File Offset: 0x00273617
		private void Apply(string themeClass, UIImageHelper item)
		{
			if (item == null)
			{
				return;
			}
			item.SetEnabledStateColor(this._invertToggle.color);
			item.SetDisabledStateColor(this._invertToggleDisabledColor);
			item.Refresh();
		}

		// Token: 0x0400513A RID: 20794
		[SerializeField]
		private ThemeSettings.ImageSettings _mainWindowBackground;

		// Token: 0x0400513B RID: 20795
		[SerializeField]
		private ThemeSettings.ImageSettings _popupWindowBackground;

		// Token: 0x0400513C RID: 20796
		[SerializeField]
		private ThemeSettings.ImageSettings _areaBackground;

		// Token: 0x0400513D RID: 20797
		[SerializeField]
		private ThemeSettings.SelectableSettings _selectableSettings;

		// Token: 0x0400513E RID: 20798
		[SerializeField]
		private ThemeSettings.SelectableSettings _buttonSettings;

		// Token: 0x0400513F RID: 20799
		[SerializeField]
		private ThemeSettings.SelectableSettings _windowButtonSettings;

		// Token: 0x04005140 RID: 20800
		[SerializeField]
		private ThemeSettings.SelectableSettings _playerButtonSettings;

		// Token: 0x04005141 RID: 20801
		[SerializeField]
		private ThemeSettings.SelectableSettings _inputGridFieldSettings;

		// Token: 0x04005142 RID: 20802
		[SerializeField]
		private ThemeSettings.ScrollbarSettings _scrollbarSettings;

		// Token: 0x04005143 RID: 20803
		[SerializeField]
		private ThemeSettings.SliderSettings _sliderSettings;

		// Token: 0x04005144 RID: 20804
		[SerializeField]
		private ThemeSettings.ImageSettings _invertToggle;

		// Token: 0x04005145 RID: 20805
		[SerializeField]
		private Color _invertToggleDisabledColor;

		// Token: 0x04005146 RID: 20806
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationValueMarker;

		// Token: 0x04005147 RID: 20807
		[SerializeField]
		private ThemeSettings.ImageSettings _calibrationRawValueMarker;

		// Token: 0x04005148 RID: 20808
		[SerializeField]
		private ThemeSettings.TextSettings _textSettings;

		// Token: 0x04005149 RID: 20809
		[SerializeField]
		private ThemeSettings.TextSettings _buttonTextSettings;

		// Token: 0x0400514A RID: 20810
		[SerializeField]
		private ThemeSettings.TextSettings _windowButtonTextSettings;

		// Token: 0x0400514B RID: 20811
		[SerializeField]
		private ThemeSettings.TextSettings _playerButtonTextSettings;

		// Token: 0x0400514C RID: 20812
		[SerializeField]
		private ThemeSettings.TextSettings _playerDropdownButtonTextSettings;

		// Token: 0x0400514D RID: 20813
		[SerializeField]
		private ThemeSettings.TextSettings _restoreDefaultButtonTextSettings;

		// Token: 0x0400514E RID: 20814
		[SerializeField]
		private ThemeSettings.TextSettings _actionColumnTextSettings;

		// Token: 0x0400514F RID: 20815
		[SerializeField]
		private ThemeSettings.TextSettings _actionColumnDeactivatedTextSettings;

		// Token: 0x04005150 RID: 20816
		[SerializeField]
		private ThemeSettings.TextSettings _actionColumnHeaderTextSettings;

		// Token: 0x04005151 RID: 20817
		[SerializeField]
		private ThemeSettings.TextSettings _inputColumnHeaderTextSettings;

		// Token: 0x04005152 RID: 20818
		[SerializeField]
		private ThemeSettings.TextSettings _inputGridFieldTextSettings;

		// Token: 0x02000C3B RID: 3131
		[Serializable]
		private abstract class SelectableSettings_Base
		{
			// Token: 0x17000797 RID: 1943
			// (get) Token: 0x06004CF6 RID: 19702 RVA: 0x00275251 File Offset: 0x00273651
			public Selectable.Transition transition
			{
				get
				{
					return this._transition;
				}
			}

			// Token: 0x17000798 RID: 1944
			// (get) Token: 0x06004CF7 RID: 19703 RVA: 0x00275259 File Offset: 0x00273659
			public ThemeSettings.CustomColorBlock selectableColors
			{
				get
				{
					return this._colors;
				}
			}

			// Token: 0x17000799 RID: 1945
			// (get) Token: 0x06004CF8 RID: 19704 RVA: 0x00275261 File Offset: 0x00273661
			public ThemeSettings.CustomSpriteState spriteState
			{
				get
				{
					return this._spriteState;
				}
			}

			// Token: 0x1700079A RID: 1946
			// (get) Token: 0x06004CF9 RID: 19705 RVA: 0x00275269 File Offset: 0x00273669
			public ThemeSettings.CustomAnimationTriggers animationTriggers
			{
				get
				{
					return this._animationTriggers;
				}
			}

			// Token: 0x06004CFA RID: 19706 RVA: 0x00275274 File Offset: 0x00273674
			public virtual void Apply(Selectable item)
			{
				Selectable.Transition transition = this._transition;
				bool flag = item.transition != transition;
				item.transition = transition;
				ICustomSelectable customSelectable = item as ICustomSelectable;
				ThemeSettings.CustomColorBlock colors = this._colors;
				colors.fadeDuration = 0f;
				item.colors = colors;
				colors.fadeDuration = this._colors.fadeDuration;
				item.colors = colors;
				if (customSelectable != null)
				{
					customSelectable.disabledHighlightedColor = colors.disabledHighlightedColor;
				}
				if (transition == Selectable.Transition.SpriteSwap)
				{
					item.spriteState = this._spriteState;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedSprite = this._spriteState.disabledHighlightedSprite;
					}
				}
				else if (transition == Selectable.Transition.Animation)
				{
					item.animationTriggers.disabledTrigger = this._animationTriggers.disabledTrigger;
					item.animationTriggers.highlightedTrigger = this._animationTriggers.highlightedTrigger;
					item.animationTriggers.normalTrigger = this._animationTriggers.normalTrigger;
					item.animationTriggers.pressedTrigger = this._animationTriggers.pressedTrigger;
					if (customSelectable != null)
					{
						customSelectable.disabledHighlightedTrigger = this._animationTriggers.disabledHighlightedTrigger;
					}
				}
				if (flag)
				{
					item.targetGraphic.CrossFadeColor(item.targetGraphic.color, 0f, true, true);
				}
			}

			// Token: 0x04005155 RID: 20821
			[SerializeField]
			protected Selectable.Transition _transition;

			// Token: 0x04005156 RID: 20822
			[SerializeField]
			protected ThemeSettings.CustomColorBlock _colors;

			// Token: 0x04005157 RID: 20823
			[SerializeField]
			protected ThemeSettings.CustomSpriteState _spriteState;

			// Token: 0x04005158 RID: 20824
			[SerializeField]
			protected ThemeSettings.CustomAnimationTriggers _animationTriggers;
		}

		// Token: 0x02000C3C RID: 3132
		[Serializable]
		private class SelectableSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x1700079B RID: 1947
			// (get) Token: 0x06004CFC RID: 19708 RVA: 0x002753C8 File Offset: 0x002737C8
			public ThemeSettings.ImageSettings imageSettings
			{
				get
				{
					return this._imageSettings;
				}
			}

			// Token: 0x06004CFD RID: 19709 RVA: 0x002753D0 File Offset: 0x002737D0
			public override void Apply(Selectable item)
			{
				if (item == null)
				{
					return;
				}
				base.Apply(item);
				if (this._imageSettings != null)
				{
					this._imageSettings.CopyTo(item.targetGraphic as Image);
				}
			}

			// Token: 0x04005159 RID: 20825
			[SerializeField]
			private ThemeSettings.ImageSettings _imageSettings;
		}

		// Token: 0x02000C3D RID: 3133
		[Serializable]
		private class SliderSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x1700079C RID: 1948
			// (get) Token: 0x06004CFF RID: 19711 RVA: 0x0027540F File Offset: 0x0027380F
			public ThemeSettings.ImageSettings handleImageSettings
			{
				get
				{
					return this._handleImageSettings;
				}
			}

			// Token: 0x1700079D RID: 1949
			// (get) Token: 0x06004D00 RID: 19712 RVA: 0x00275417 File Offset: 0x00273817
			public ThemeSettings.ImageSettings fillImageSettings
			{
				get
				{
					return this._fillImageSettings;
				}
			}

			// Token: 0x1700079E RID: 1950
			// (get) Token: 0x06004D01 RID: 19713 RVA: 0x0027541F File Offset: 0x0027381F
			public ThemeSettings.ImageSettings backgroundImageSettings
			{
				get
				{
					return this._backgroundImageSettings;
				}
			}

			// Token: 0x06004D02 RID: 19714 RVA: 0x00275428 File Offset: 0x00273828
			private void Apply(Slider item)
			{
				if (item == null)
				{
					return;
				}
				if (this._handleImageSettings != null)
				{
					this._handleImageSettings.CopyTo(item.targetGraphic as Image);
				}
				if (this._fillImageSettings != null)
				{
					RectTransform fillRect = item.fillRect;
					if (fillRect != null)
					{
						this._fillImageSettings.CopyTo(fillRect.GetComponent<Image>());
					}
				}
				if (this._backgroundImageSettings != null)
				{
					Transform transform = item.transform.Find("Background");
					if (transform != null)
					{
						this._backgroundImageSettings.CopyTo(transform.GetComponent<Image>());
					}
				}
			}

			// Token: 0x06004D03 RID: 19715 RVA: 0x002754CB File Offset: 0x002738CB
			public override void Apply(Selectable item)
			{
				base.Apply(item);
				this.Apply(item as Slider);
			}

			// Token: 0x0400515A RID: 20826
			[SerializeField]
			private ThemeSettings.ImageSettings _handleImageSettings;

			// Token: 0x0400515B RID: 20827
			[SerializeField]
			private ThemeSettings.ImageSettings _fillImageSettings;

			// Token: 0x0400515C RID: 20828
			[SerializeField]
			private ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		// Token: 0x02000C3E RID: 3134
		[Serializable]
		private class ScrollbarSettings : ThemeSettings.SelectableSettings_Base
		{
			// Token: 0x1700079F RID: 1951
			// (get) Token: 0x06004D05 RID: 19717 RVA: 0x002754E8 File Offset: 0x002738E8
			public ThemeSettings.ImageSettings handle
			{
				get
				{
					return this._handleImageSettings;
				}
			}

			// Token: 0x170007A0 RID: 1952
			// (get) Token: 0x06004D06 RID: 19718 RVA: 0x002754F0 File Offset: 0x002738F0
			public ThemeSettings.ImageSettings background
			{
				get
				{
					return this._backgroundImageSettings;
				}
			}

			// Token: 0x06004D07 RID: 19719 RVA: 0x002754F8 File Offset: 0x002738F8
			private void Apply(Scrollbar item)
			{
				if (item == null)
				{
					return;
				}
				if (this._handleImageSettings != null)
				{
					this._handleImageSettings.CopyTo(item.targetGraphic as Image);
				}
				if (this._backgroundImageSettings != null)
				{
					this._backgroundImageSettings.CopyTo(item.GetComponent<Image>());
				}
			}

			// Token: 0x06004D08 RID: 19720 RVA: 0x0027554F File Offset: 0x0027394F
			public override void Apply(Selectable item)
			{
				base.Apply(item);
				this.Apply(item as Scrollbar);
			}

			// Token: 0x0400515D RID: 20829
			[SerializeField]
			private ThemeSettings.ImageSettings _handleImageSettings;

			// Token: 0x0400515E RID: 20830
			[SerializeField]
			private ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		// Token: 0x02000C3F RID: 3135
		[Serializable]
		private class ImageSettings
		{
			// Token: 0x170007A1 RID: 1953
			// (get) Token: 0x06004D0A RID: 19722 RVA: 0x00275577 File Offset: 0x00273977
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x170007A2 RID: 1954
			// (get) Token: 0x06004D0B RID: 19723 RVA: 0x0027557F File Offset: 0x0027397F
			public Sprite sprite
			{
				get
				{
					return this._sprite;
				}
			}

			// Token: 0x170007A3 RID: 1955
			// (get) Token: 0x06004D0C RID: 19724 RVA: 0x00275587 File Offset: 0x00273987
			public Material materal
			{
				get
				{
					return this._materal;
				}
			}

			// Token: 0x170007A4 RID: 1956
			// (get) Token: 0x06004D0D RID: 19725 RVA: 0x0027558F File Offset: 0x0027398F
			public Image.Type type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x170007A5 RID: 1957
			// (get) Token: 0x06004D0E RID: 19726 RVA: 0x00275597 File Offset: 0x00273997
			public bool preserveAspect
			{
				get
				{
					return this._preserveAspect;
				}
			}

			// Token: 0x170007A6 RID: 1958
			// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0027559F File Offset: 0x0027399F
			public bool fillCenter
			{
				get
				{
					return this._fillCenter;
				}
			}

			// Token: 0x170007A7 RID: 1959
			// (get) Token: 0x06004D10 RID: 19728 RVA: 0x002755A7 File Offset: 0x002739A7
			public Image.FillMethod fillMethod
			{
				get
				{
					return this._fillMethod;
				}
			}

			// Token: 0x170007A8 RID: 1960
			// (get) Token: 0x06004D11 RID: 19729 RVA: 0x002755AF File Offset: 0x002739AF
			public float fillAmout
			{
				get
				{
					return this._fillAmout;
				}
			}

			// Token: 0x170007A9 RID: 1961
			// (get) Token: 0x06004D12 RID: 19730 RVA: 0x002755B7 File Offset: 0x002739B7
			public bool fillClockwise
			{
				get
				{
					return this._fillClockwise;
				}
			}

			// Token: 0x170007AA RID: 1962
			// (get) Token: 0x06004D13 RID: 19731 RVA: 0x002755BF File Offset: 0x002739BF
			public int fillOrigin
			{
				get
				{
					return this._fillOrigin;
				}
			}

			// Token: 0x06004D14 RID: 19732 RVA: 0x002755C8 File Offset: 0x002739C8
			public virtual void CopyTo(Image image)
			{
				if (image == null)
				{
					return;
				}
				image.color = this._color;
				image.sprite = this._sprite;
				image.material = this._materal;
				image.type = this._type;
				image.preserveAspect = this._preserveAspect;
				image.fillCenter = this._fillCenter;
				image.fillMethod = this._fillMethod;
				image.fillAmount = this._fillAmout;
				image.fillClockwise = this._fillClockwise;
				image.fillOrigin = this._fillOrigin;
			}

			// Token: 0x0400515F RID: 20831
			[SerializeField]
			private Color _color = Color.white;

			// Token: 0x04005160 RID: 20832
			[SerializeField]
			private Sprite _sprite;

			// Token: 0x04005161 RID: 20833
			[SerializeField]
			private Material _materal;

			// Token: 0x04005162 RID: 20834
			[SerializeField]
			private Image.Type _type;

			// Token: 0x04005163 RID: 20835
			[SerializeField]
			private bool _preserveAspect;

			// Token: 0x04005164 RID: 20836
			[SerializeField]
			private bool _fillCenter;

			// Token: 0x04005165 RID: 20837
			[SerializeField]
			private Image.FillMethod _fillMethod;

			// Token: 0x04005166 RID: 20838
			[SerializeField]
			private float _fillAmout;

			// Token: 0x04005167 RID: 20839
			[SerializeField]
			private bool _fillClockwise;

			// Token: 0x04005168 RID: 20840
			[SerializeField]
			private int _fillOrigin;
		}

		// Token: 0x02000C40 RID: 3136
		[Serializable]
		private struct CustomColorBlock
		{
			// Token: 0x170007AB RID: 1963
			// (get) Token: 0x06004D15 RID: 19733 RVA: 0x0027565A File Offset: 0x00273A5A
			// (set) Token: 0x06004D16 RID: 19734 RVA: 0x00275662 File Offset: 0x00273A62
			public float colorMultiplier
			{
				get
				{
					return this.m_ColorMultiplier;
				}
				set
				{
					this.m_ColorMultiplier = value;
				}
			}

			// Token: 0x170007AC RID: 1964
			// (get) Token: 0x06004D17 RID: 19735 RVA: 0x0027566B File Offset: 0x00273A6B
			// (set) Token: 0x06004D18 RID: 19736 RVA: 0x00275673 File Offset: 0x00273A73
			public Color disabledColor
			{
				get
				{
					return this.m_DisabledColor;
				}
				set
				{
					this.m_DisabledColor = value;
				}
			}

			// Token: 0x170007AD RID: 1965
			// (get) Token: 0x06004D19 RID: 19737 RVA: 0x0027567C File Offset: 0x00273A7C
			// (set) Token: 0x06004D1A RID: 19738 RVA: 0x00275684 File Offset: 0x00273A84
			public float fadeDuration
			{
				get
				{
					return this.m_FadeDuration;
				}
				set
				{
					this.m_FadeDuration = value;
				}
			}

			// Token: 0x170007AE RID: 1966
			// (get) Token: 0x06004D1B RID: 19739 RVA: 0x0027568D File Offset: 0x00273A8D
			// (set) Token: 0x06004D1C RID: 19740 RVA: 0x00275695 File Offset: 0x00273A95
			public Color highlightedColor
			{
				get
				{
					return this.m_HighlightedColor;
				}
				set
				{
					this.m_HighlightedColor = value;
				}
			}

			// Token: 0x170007AF RID: 1967
			// (get) Token: 0x06004D1D RID: 19741 RVA: 0x0027569E File Offset: 0x00273A9E
			// (set) Token: 0x06004D1E RID: 19742 RVA: 0x002756A6 File Offset: 0x00273AA6
			public Color normalColor
			{
				get
				{
					return this.m_NormalColor;
				}
				set
				{
					this.m_NormalColor = value;
				}
			}

			// Token: 0x170007B0 RID: 1968
			// (get) Token: 0x06004D1F RID: 19743 RVA: 0x002756AF File Offset: 0x00273AAF
			// (set) Token: 0x06004D20 RID: 19744 RVA: 0x002756B7 File Offset: 0x00273AB7
			public Color pressedColor
			{
				get
				{
					return this.m_PressedColor;
				}
				set
				{
					this.m_PressedColor = value;
				}
			}

			// Token: 0x170007B1 RID: 1969
			// (get) Token: 0x06004D21 RID: 19745 RVA: 0x002756C0 File Offset: 0x00273AC0
			// (set) Token: 0x06004D22 RID: 19746 RVA: 0x002756C8 File Offset: 0x00273AC8
			public Color disabledHighlightedColor
			{
				get
				{
					return this.m_DisabledHighlightedColor;
				}
				set
				{
					this.m_DisabledHighlightedColor = value;
				}
			}

			// Token: 0x06004D23 RID: 19747 RVA: 0x002756D4 File Offset: 0x00273AD4
			public static implicit operator ColorBlock(ThemeSettings.CustomColorBlock item)
			{
				return new ColorBlock
				{
					colorMultiplier = item.m_ColorMultiplier,
					disabledColor = item.m_DisabledColor,
					fadeDuration = item.m_FadeDuration,
					highlightedColor = item.m_HighlightedColor,
					normalColor = item.m_NormalColor,
					pressedColor = item.m_PressedColor
				};
			}

			// Token: 0x04005169 RID: 20841
			[SerializeField]
			private float m_ColorMultiplier;

			// Token: 0x0400516A RID: 20842
			[SerializeField]
			private Color m_DisabledColor;

			// Token: 0x0400516B RID: 20843
			[SerializeField]
			private float m_FadeDuration;

			// Token: 0x0400516C RID: 20844
			[SerializeField]
			private Color m_HighlightedColor;

			// Token: 0x0400516D RID: 20845
			[SerializeField]
			private Color m_NormalColor;

			// Token: 0x0400516E RID: 20846
			[SerializeField]
			private Color m_PressedColor;

			// Token: 0x0400516F RID: 20847
			[SerializeField]
			private Color m_DisabledHighlightedColor;
		}

		// Token: 0x02000C41 RID: 3137
		[Serializable]
		private struct CustomSpriteState
		{
			// Token: 0x170007B2 RID: 1970
			// (get) Token: 0x06004D24 RID: 19748 RVA: 0x0027573E File Offset: 0x00273B3E
			// (set) Token: 0x06004D25 RID: 19749 RVA: 0x00275746 File Offset: 0x00273B46
			public Sprite disabledSprite
			{
				get
				{
					return this.m_DisabledSprite;
				}
				set
				{
					this.m_DisabledSprite = value;
				}
			}

			// Token: 0x170007B3 RID: 1971
			// (get) Token: 0x06004D26 RID: 19750 RVA: 0x0027574F File Offset: 0x00273B4F
			// (set) Token: 0x06004D27 RID: 19751 RVA: 0x00275757 File Offset: 0x00273B57
			public Sprite highlightedSprite
			{
				get
				{
					return this.m_HighlightedSprite;
				}
				set
				{
					this.m_HighlightedSprite = value;
				}
			}

			// Token: 0x170007B4 RID: 1972
			// (get) Token: 0x06004D28 RID: 19752 RVA: 0x00275760 File Offset: 0x00273B60
			// (set) Token: 0x06004D29 RID: 19753 RVA: 0x00275768 File Offset: 0x00273B68
			public Sprite pressedSprite
			{
				get
				{
					return this.m_PressedSprite;
				}
				set
				{
					this.m_PressedSprite = value;
				}
			}

			// Token: 0x170007B5 RID: 1973
			// (get) Token: 0x06004D2A RID: 19754 RVA: 0x00275771 File Offset: 0x00273B71
			// (set) Token: 0x06004D2B RID: 19755 RVA: 0x00275779 File Offset: 0x00273B79
			public Sprite disabledHighlightedSprite
			{
				get
				{
					return this.m_DisabledHighlightedSprite;
				}
				set
				{
					this.m_DisabledHighlightedSprite = value;
				}
			}

			// Token: 0x06004D2C RID: 19756 RVA: 0x00275784 File Offset: 0x00273B84
			public static implicit operator SpriteState(ThemeSettings.CustomSpriteState item)
			{
				return new SpriteState
				{
					disabledSprite = item.m_DisabledSprite,
					highlightedSprite = item.m_HighlightedSprite,
					pressedSprite = item.m_PressedSprite
				};
			}

			// Token: 0x04005170 RID: 20848
			[SerializeField]
			private Sprite m_DisabledSprite;

			// Token: 0x04005171 RID: 20849
			[SerializeField]
			private Sprite m_HighlightedSprite;

			// Token: 0x04005172 RID: 20850
			[SerializeField]
			private Sprite m_PressedSprite;

			// Token: 0x04005173 RID: 20851
			[SerializeField]
			private Sprite m_DisabledHighlightedSprite;
		}

		// Token: 0x02000C42 RID: 3138
		[Serializable]
		private class CustomAnimationTriggers
		{
			// Token: 0x06004D2D RID: 19757 RVA: 0x002757C4 File Offset: 0x00273BC4
			public CustomAnimationTriggers()
			{
				this.m_DisabledTrigger = string.Empty;
				this.m_HighlightedTrigger = string.Empty;
				this.m_NormalTrigger = string.Empty;
				this.m_PressedTrigger = string.Empty;
				this.m_DisabledHighlightedTrigger = string.Empty;
			}

			// Token: 0x170007B6 RID: 1974
			// (get) Token: 0x06004D2E RID: 19758 RVA: 0x00275803 File Offset: 0x00273C03
			// (set) Token: 0x06004D2F RID: 19759 RVA: 0x0027580B File Offset: 0x00273C0B
			public string disabledTrigger
			{
				get
				{
					return this.m_DisabledTrigger;
				}
				set
				{
					this.m_DisabledTrigger = value;
				}
			}

			// Token: 0x170007B7 RID: 1975
			// (get) Token: 0x06004D30 RID: 19760 RVA: 0x00275814 File Offset: 0x00273C14
			// (set) Token: 0x06004D31 RID: 19761 RVA: 0x0027581C File Offset: 0x00273C1C
			public string highlightedTrigger
			{
				get
				{
					return this.m_HighlightedTrigger;
				}
				set
				{
					this.m_HighlightedTrigger = value;
				}
			}

			// Token: 0x170007B8 RID: 1976
			// (get) Token: 0x06004D32 RID: 19762 RVA: 0x00275825 File Offset: 0x00273C25
			// (set) Token: 0x06004D33 RID: 19763 RVA: 0x0027582D File Offset: 0x00273C2D
			public string normalTrigger
			{
				get
				{
					return this.m_NormalTrigger;
				}
				set
				{
					this.m_NormalTrigger = value;
				}
			}

			// Token: 0x170007B9 RID: 1977
			// (get) Token: 0x06004D34 RID: 19764 RVA: 0x00275836 File Offset: 0x00273C36
			// (set) Token: 0x06004D35 RID: 19765 RVA: 0x0027583E File Offset: 0x00273C3E
			public string pressedTrigger
			{
				get
				{
					return this.m_PressedTrigger;
				}
				set
				{
					this.m_PressedTrigger = value;
				}
			}

			// Token: 0x170007BA RID: 1978
			// (get) Token: 0x06004D36 RID: 19766 RVA: 0x00275847 File Offset: 0x00273C47
			// (set) Token: 0x06004D37 RID: 19767 RVA: 0x0027584F File Offset: 0x00273C4F
			public string disabledHighlightedTrigger
			{
				get
				{
					return this.m_DisabledHighlightedTrigger;
				}
				set
				{
					this.m_DisabledHighlightedTrigger = value;
				}
			}

			// Token: 0x06004D38 RID: 19768 RVA: 0x00275858 File Offset: 0x00273C58
			public static implicit operator AnimationTriggers(ThemeSettings.CustomAnimationTriggers item)
			{
				return new AnimationTriggers
				{
					disabledTrigger = item.m_DisabledTrigger,
					highlightedTrigger = item.m_HighlightedTrigger,
					normalTrigger = item.m_NormalTrigger,
					pressedTrigger = item.m_PressedTrigger
				};
			}

			// Token: 0x04005174 RID: 20852
			[SerializeField]
			private string m_DisabledTrigger;

			// Token: 0x04005175 RID: 20853
			[SerializeField]
			private string m_HighlightedTrigger;

			// Token: 0x04005176 RID: 20854
			[SerializeField]
			private string m_NormalTrigger;

			// Token: 0x04005177 RID: 20855
			[SerializeField]
			private string m_PressedTrigger;

			// Token: 0x04005178 RID: 20856
			[SerializeField]
			private string m_DisabledHighlightedTrigger;
		}

		// Token: 0x02000C43 RID: 3139
		[Serializable]
		private class TextSettings
		{
			// Token: 0x170007BB RID: 1979
			// (get) Token: 0x06004D3A RID: 19770 RVA: 0x002758C5 File Offset: 0x00273CC5
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x170007BC RID: 1980
			// (get) Token: 0x06004D3B RID: 19771 RVA: 0x002758CD File Offset: 0x00273CCD
			public FontLoader.FontType[] fontTypes
			{
				get
				{
					return this._fontTypes;
				}
			}

			// Token: 0x170007BD RID: 1981
			// (get) Token: 0x06004D3C RID: 19772 RVA: 0x002758D5 File Offset: 0x00273CD5
			public ThemeSettings.FontStyleOverride[] style
			{
				get
				{
					return this._style;
				}
			}

			// Token: 0x170007BE RID: 1982
			// (get) Token: 0x06004D3D RID: 19773 RVA: 0x002758DD File Offset: 0x00273CDD
			public float lineSpacing
			{
				get
				{
					return this._lineSpacing;
				}
			}

			// Token: 0x170007BF RID: 1983
			// (get) Token: 0x06004D3E RID: 19774 RVA: 0x002758E5 File Offset: 0x00273CE5
			public float sizeMultiplier
			{
				get
				{
					return this._sizeMultiplier;
				}
			}

			// Token: 0x170007C0 RID: 1984
			// (get) Token: 0x06004D3F RID: 19775 RVA: 0x002758ED File Offset: 0x00273CED
			public int overrideSize
			{
				get
				{
					return this._overrideSize;
				}
			}

			// Token: 0x04005179 RID: 20857
			[SerializeField]
			private Color _color = Color.white;

			// Token: 0x0400517A RID: 20858
			[SerializeField]
			private FontLoader.FontType[] _fontTypes;

			// Token: 0x0400517B RID: 20859
			[SerializeField]
			private ThemeSettings.FontStyleOverride[] _style;

			// Token: 0x0400517C RID: 20860
			[SerializeField]
			private float _lineSpacing = 1f;

			// Token: 0x0400517D RID: 20861
			[SerializeField]
			private float _sizeMultiplier = 1f;

			// Token: 0x0400517E RID: 20862
			[SerializeField]
			private int _overrideSize;
		}

		// Token: 0x02000C44 RID: 3140
		private enum FontStyleOverride
		{
			// Token: 0x04005180 RID: 20864
			Default,
			// Token: 0x04005181 RID: 20865
			Normal,
			// Token: 0x04005182 RID: 20866
			Bold,
			// Token: 0x04005183 RID: 20867
			Italic,
			// Token: 0x04005184 RID: 20868
			BoldAndItalic
		}
	}
}
