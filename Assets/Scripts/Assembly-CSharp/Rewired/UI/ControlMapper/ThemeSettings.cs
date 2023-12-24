using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	[Serializable]
	public class ThemeSettings : ScriptableObject
	{
		[Serializable]
		private class ImageSettings
		{
			[SerializeField]
			private Color _color;
			[SerializeField]
			private Sprite _sprite;
			[SerializeField]
			private Material _materal;
			[SerializeField]
			private Image.Type _type;
			[SerializeField]
			private bool _preserveAspect;
			[SerializeField]
			private bool _fillCenter;
			[SerializeField]
			private Image.FillMethod _fillMethod;
			[SerializeField]
			private float _fillAmout;
			[SerializeField]
			private bool _fillClockwise;
			[SerializeField]
			private int _fillOrigin;
		}

		[Serializable]
		private struct CustomColorBlock
		{
			[SerializeField]
			private float m_ColorMultiplier;
			[SerializeField]
			private Color m_DisabledColor;
			[SerializeField]
			private float m_FadeDuration;
			[SerializeField]
			private Color m_HighlightedColor;
			[SerializeField]
			private Color m_NormalColor;
			[SerializeField]
			private Color m_PressedColor;
			[SerializeField]
			private Color m_DisabledHighlightedColor;
		}

		[Serializable]
		private struct CustomSpriteState
		{
			[SerializeField]
			private Sprite m_DisabledSprite;
			[SerializeField]
			private Sprite m_HighlightedSprite;
			[SerializeField]
			private Sprite m_PressedSprite;
			[SerializeField]
			private Sprite m_DisabledHighlightedSprite;
		}

		[Serializable]
		private class CustomAnimationTriggers
		{
			[SerializeField]
			private string m_DisabledTrigger;
			[SerializeField]
			private string m_HighlightedTrigger;
			[SerializeField]
			private string m_NormalTrigger;
			[SerializeField]
			private string m_PressedTrigger;
			[SerializeField]
			private string m_DisabledHighlightedTrigger;
		}

		[Serializable]
		private class SelectableSettings_Base
		{
			[SerializeField]
			protected Selectable.Transition _transition;
			[SerializeField]
			protected ThemeSettings.CustomColorBlock _colors;
			[SerializeField]
			protected ThemeSettings.CustomSpriteState _spriteState;
			[SerializeField]
			protected ThemeSettings.CustomAnimationTriggers _animationTriggers;
		}

		[Serializable]
		private class SelectableSettings : SelectableSettings_Base
		{
			[SerializeField]
			private ThemeSettings.ImageSettings _imageSettings;
		}

		[Serializable]
		private class ScrollbarSettings : SelectableSettings_Base
		{
			[SerializeField]
			private ThemeSettings.ImageSettings _handleImageSettings;
			[SerializeField]
			private ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		[Serializable]
		private class SliderSettings : SelectableSettings_Base
		{
			[SerializeField]
			private ThemeSettings.ImageSettings _handleImageSettings;
			[SerializeField]
			private ThemeSettings.ImageSettings _fillImageSettings;
			[SerializeField]
			private ThemeSettings.ImageSettings _backgroundImageSettings;
		}

		[Serializable]
		private class TextSettings
		{
			[SerializeField]
			private Color _color;
			[SerializeField]
			private FontLoader.FontType[] _fontTypes;
			[SerializeField]
			private ThemeSettings.FontStyleOverride[] _style;
			[SerializeField]
			private float _lineSpacing;
			[SerializeField]
			private float _sizeMultiplier;
			[SerializeField]
			private int _overrideSize;
		}

		private enum FontStyleOverride
		{
			Default = 0,
			Normal = 1,
			Bold = 2,
			Italic = 3,
			BoldAndItalic = 4,
		}

		[SerializeField]
		private ImageSettings _mainWindowBackground;
		[SerializeField]
		private ImageSettings _popupWindowBackground;
		[SerializeField]
		private ImageSettings _areaBackground;
		[SerializeField]
		private SelectableSettings _selectableSettings;
		[SerializeField]
		private SelectableSettings _buttonSettings;
		[SerializeField]
		private SelectableSettings _windowButtonSettings;
		[SerializeField]
		private SelectableSettings _playerButtonSettings;
		[SerializeField]
		private SelectableSettings _inputGridFieldSettings;
		[SerializeField]
		private ScrollbarSettings _scrollbarSettings;
		[SerializeField]
		private SliderSettings _sliderSettings;
		[SerializeField]
		private ImageSettings _invertToggle;
		[SerializeField]
		private Color _invertToggleDisabledColor;
		[SerializeField]
		private ImageSettings _calibrationValueMarker;
		[SerializeField]
		private ImageSettings _calibrationRawValueMarker;
		[SerializeField]
		private TextSettings _textSettings;
		[SerializeField]
		private TextSettings _buttonTextSettings;
		[SerializeField]
		private TextSettings _windowButtonTextSettings;
		[SerializeField]
		private TextSettings _playerButtonTextSettings;
		[SerializeField]
		private TextSettings _playerDropdownButtonTextSettings;
		[SerializeField]
		private TextSettings _restoreDefaultButtonTextSettings;
		[SerializeField]
		private TextSettings _actionColumnTextSettings;
		[SerializeField]
		private TextSettings _actionColumnDeactivatedTextSettings;
		[SerializeField]
		private TextSettings _actionColumnHeaderTextSettings;
		[SerializeField]
		private TextSettings _inputColumnHeaderTextSettings;
		[SerializeField]
		private TextSettings _inputGridFieldTextSettings;
	}
}
