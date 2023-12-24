using UnityEngine;
using System;
using UnityEngine.UI;
using Rewired;
using UnityEngine.Events;

namespace Rewired.UI.ControlMapper
{
	public class ControlMapper : MonoBehaviour
	{
		[Serializable]
		public class MappingSet
		{
			public enum ActionListMode
			{
				ActionCategory = 0,
				Action = 1,
			}

			[SerializeField]
			private int _mapCategoryId;
			[SerializeField]
			private ActionListMode _actionListMode;
			[SerializeField]
			private int[] _actionCategoryIds;
			[SerializeField]
			private int[] _actionIds;
		}

		[Serializable]
		public class InputBehaviorSettings
		{
			[SerializeField]
			private int _inputBehaviorId;
			[SerializeField]
			private bool _showJoystickAxisSensitivity;
			[SerializeField]
			private bool _showMouseXYAxisSensitivity;
			[SerializeField]
			private string _labelLanguageKey;
			[SerializeField]
			private string _joystickAxisSensitivityLabelLanguageKey;
			[SerializeField]
			private string _mouseXYAxisSensitivityLabelLanguageKey;
			[SerializeField]
			private Sprite _joystickAxisSensitivityIcon;
			[SerializeField]
			private Sprite _mouseXYAxisSensitivityIcon;
			[SerializeField]
			private float _joystickAxisSensitivityMin;
			[SerializeField]
			private float _joystickAxisSensitivityMax;
			[SerializeField]
			private float _mouseXYAxisSensitivityMin;
			[SerializeField]
			private float _mouseXYAxisSensitivityMax;
		}

		[Serializable]
		private class Prefabs
		{
			[SerializeField]
			private GameObject _button;
			[SerializeField]
			private GameObject _playerButton;
			[SerializeField]
			private GameObject _fitButton;
			[SerializeField]
			private GameObject _inputGridLabel;
			[SerializeField]
			private GameObject _inputGridDeactivatedLabel;
			[SerializeField]
			private GameObject _inputGridHeaderLabel;
			[SerializeField]
			private GameObject _actionsHeaderLabel;
			[SerializeField]
			private GameObject _inputGridFieldButton;
			[SerializeField]
			private GameObject _inputGridFieldInvertToggle;
			[SerializeField]
			private GameObject _window;
			[SerializeField]
			private GameObject _windowTitleText;
			[SerializeField]
			private GameObject _windowContentText;
			[SerializeField]
			private GameObject _fader;
			[SerializeField]
			private GameObject _calibrationWindow;
			[SerializeField]
			private GameObject _inputBehaviorsWindow;
			[SerializeField]
			private GameObject _centerStickGraphic;
			[SerializeField]
			private GameObject _moveStickGraphic;
		}

		[Serializable]
		private class References
		{
			[SerializeField]
			private Canvas _canvas;
			[SerializeField]
			private CanvasGroup _mainCanvasGroup;
			[SerializeField]
			private Transform _mainContent;
			[SerializeField]
			private Transform _mainContentInner;
			[SerializeField]
			private UIGroup _playersGroup;
			[SerializeField]
			private Transform _controllerGroup;
			[SerializeField]
			private Transform _controllerGroupLabelGroup;
			[SerializeField]
			private UIGroup _controllerSettingsGroup;
			[SerializeField]
			private UIGroup _assignedControllersGroup;
			[SerializeField]
			private Transform _settingsAndMapCategoriesGroup;
			[SerializeField]
			private UIGroup _settingsGroup;
			[SerializeField]
			private UIGroup _mapCategoriesGroup;
			[SerializeField]
			private Transform _inputGridGroup;
			[SerializeField]
			private Transform _inputGridContainer;
			[SerializeField]
			private Transform _inputGridHeadersGroup;
			[SerializeField]
			private Transform _inputGridInnerGroup;
			[SerializeField]
			private Transform _actionsColumnHeadersGroup;
			[SerializeField]
			private Transform _actionsColumn;
			[SerializeField]
			private Text _controllerNameLabel;
			[SerializeField]
			private Button _removeControllerButton;
			[SerializeField]
			private Button _assignControllerButton;
			[SerializeField]
			private Button _calibrateControllerButton;
			[SerializeField]
			private Button _doneButton;
			[SerializeField]
			private Button _restoreDefaultsButton;
			[SerializeField]
			private Selectable _defaultSelection;
			[SerializeField]
			private GameObject[] _fixedSelectableUIElements;
			[SerializeField]
			private Image _mainBackgroundImage;
		}

		[SerializeField]
		private InputManager _rewiredInputManager;
		[SerializeField]
		private bool _dontDestroyOnLoad;
		[SerializeField]
		private bool _openOnStart;
		[SerializeField]
		private int _keyboardMapDefaultLayout;
		[SerializeField]
		private int _mouseMapDefaultLayout;
		[SerializeField]
		private int _joystickMapDefaultLayout;
		[SerializeField]
		private MappingSet[] _mappingSets;
		[SerializeField]
		private bool _showPlayers;
		[SerializeField]
		private bool _showControllers;
		[SerializeField]
		private bool _showKeyboard;
		[SerializeField]
		private bool _showMouse;
		[SerializeField]
		private int _maxControllersPerPlayer;
		[SerializeField]
		private bool _showActionCategoryLabels;
		[SerializeField]
		private int _keyboardInputFieldCount;
		[SerializeField]
		private int _mouseInputFieldCount;
		[SerializeField]
		private int _controllerInputFieldCount;
		[SerializeField]
		private bool _showFullAxisInputFields;
		[SerializeField]
		private bool _showSplitAxisInputFields;
		[SerializeField]
		private bool _allowElementAssignmentConflicts;
		[SerializeField]
		private int _actionLabelWidth;
		[SerializeField]
		private int _keyboardColMaxWidth;
		[SerializeField]
		private int _mouseColMaxWidth;
		[SerializeField]
		private int _controllerColMaxWidth;
		[SerializeField]
		private float _inputRowHeight;
		[SerializeField]
		private float _inputColumnSpacing;
		[SerializeField]
		private int _inputRowCategorySpacing;
		[SerializeField]
		private int _invertToggleWidth;
		[SerializeField]
		private int _defaultWindowWidth;
		[SerializeField]
		private int _defaultWindowHeight;
		[SerializeField]
		private float _controllerAssignmentTimeout;
		[SerializeField]
		private float _preInputAssignmentTimeout;
		[SerializeField]
		private float _inputAssignmentTimeout;
		[SerializeField]
		private float _axisCalibrationTimeout;
		[SerializeField]
		private bool _ignoreMouseXAxisAssignment;
		[SerializeField]
		private bool _ignoreMouseYAxisAssignment;
		[SerializeField]
		private int _screenToggleAction;
		[SerializeField]
		private int _screenOpenAction;
		[SerializeField]
		private int _screenCloseAction;
		[SerializeField]
		private int _universalCancelAction;
		[SerializeField]
		private bool _universalCancelClosesScreen;
		[SerializeField]
		private bool _showInputBehaviorSettings;
		[SerializeField]
		private InputBehaviorSettings[] _inputBehaviorSettings;
		[SerializeField]
		private bool _useThemeSettings;
		[SerializeField]
		private ThemeSettings[] _themeSettings;
		[SerializeField]
		private LanguageData _language;
		[SerializeField]
		private Prefabs prefabs;
		[SerializeField]
		private References references;
		[SerializeField]
		private bool _showPlayersGroupLabel;
		[SerializeField]
		private bool _showControllerGroupLabel;
		[SerializeField]
		private bool _showAssignedControllersGroupLabel;
		[SerializeField]
		private bool _showSettingsGroupLabel;
		[SerializeField]
		private bool _showMapCategoriesGroupLabel;
		[SerializeField]
		private bool _showControllerNameLabel;
		[SerializeField]
		private bool _showAssignedControllers;
		[SerializeField]
		private Text _rumbleButtonText;
		[SerializeField]
		private UnityEvent _onScreenClosed;
		[SerializeField]
		private UnityEvent _onScreenOpened;
		[SerializeField]
		private UnityEvent _onPopupWindowClosed;
		[SerializeField]
		private UnityEvent _onPopupWindowOpened;
		[SerializeField]
		private UnityEvent _onInputPollingStarted;
		[SerializeField]
		private UnityEvent _onInputPollingEnded;
		public int currentPlayerId;
	}
}
