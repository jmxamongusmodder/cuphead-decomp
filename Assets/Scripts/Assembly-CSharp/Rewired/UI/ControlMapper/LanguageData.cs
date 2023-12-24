using UnityEngine;
using System;

namespace Rewired.UI.ControlMapper
{
	public class LanguageData : ScriptableObject
	{
		[Serializable]
		private class CustomEntry
		{
			public string key;
			public string value;
		}

		[SerializeField]
		private string _yes;
		[SerializeField]
		private string _no;
		[SerializeField]
		private string _add;
		[SerializeField]
		private string _replace;
		[SerializeField]
		private string _remove;
		[SerializeField]
		private string _cancel;
		[SerializeField]
		private string _none;
		[SerializeField]
		private string _okay;
		[SerializeField]
		private string _done;
		[SerializeField]
		private string _default;
		[SerializeField]
		private string _assignControllerWindowTitle;
		[SerializeField]
		private string _assignControllerWindowMessage;
		[SerializeField]
		private string _controllerAssignmentConflictWindowTitle;
		[SerializeField]
		private string _controllerAssignmentConflictWindowMessage;
		[SerializeField]
		private string _elementAssignmentPrePollingWindowMessage;
		[SerializeField]
		private string _joystickElementAssignmentPollingWindowMessage;
		[SerializeField]
		private string _joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly;
		[SerializeField]
		private string _keyboardElementAssignmentPollingWindowMessage;
		[SerializeField]
		private string _mouseElementAssignmentPollingWindowMessage;
		[SerializeField]
		private string _mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly;
		[SerializeField]
		private string _elementAssignmentConflictWindowMessage;
		[SerializeField]
		private string _elementAlreadyInUseBlocked;
		[SerializeField]
		private string _elementAlreadyInUseCanReplace;
		[SerializeField]
		private string _elementAlreadyInUseCanReplace_conflictAllowed;
		[SerializeField]
		private string _mouseAssignmentConflictWindowTitle;
		[SerializeField]
		private string _mouseAssignmentConflictWindowMessage;
		[SerializeField]
		private string _calibrateControllerWindowTitle;
		[SerializeField]
		private string _calibrateAxisStep1WindowTitle;
		[SerializeField]
		private string _calibrateAxisStep1WindowMessage;
		[SerializeField]
		private string _calibrateAxisStep2WindowTitle;
		[SerializeField]
		private string _calibrateAxisStep2WindowMessage;
		[SerializeField]
		private string _inputBehaviorSettingsWindowTitle;
		[SerializeField]
		private string _restoreDefaultsWindowTitle;
		[SerializeField]
		private string _restoreDefaultsWindowMessage_onePlayer;
		[SerializeField]
		private string _restoreDefaultsWindowMessage_multiPlayer;
		[SerializeField]
		private string _actionColumnLabel;
		[SerializeField]
		private string _keyboardColumnLabel;
		[SerializeField]
		private string _mouseColumnLabel;
		[SerializeField]
		private string _controllerColumnLabel;
		[SerializeField]
		private string _removeControllerButtonLabel;
		[SerializeField]
		private string _calibrateControllerButtonLabel;
		[SerializeField]
		private string _assignControllerButtonLabel;
		[SerializeField]
		private string _inputBehaviorSettingsButtonLabel;
		[SerializeField]
		private string _doneButtonLabel;
		[SerializeField]
		private string _restoreDefaultsButtonLabel;
		[SerializeField]
		private string _playersGroupLabel;
		[SerializeField]
		private string _controllerSettingsGroupLabel;
		[SerializeField]
		private string _assignedControllersGroupLabel;
		[SerializeField]
		private string _settingsGroupLabel;
		[SerializeField]
		private string _mapCategoriesGroupLabel;
		[SerializeField]
		private string _calibrateWindow_deadZoneSliderLabel;
		[SerializeField]
		private string _calibrateWindow_zeroSliderLabel;
		[SerializeField]
		private string _calibrateWindow_sensitivitySliderLabel;
		[SerializeField]
		private string _calibrateWindow_invertToggleLabel;
		[SerializeField]
		private string _calibrateWindow_calibrateButtonLabel;
		[SerializeField]
		private CustomEntry[] _customEntries;
	}
}
