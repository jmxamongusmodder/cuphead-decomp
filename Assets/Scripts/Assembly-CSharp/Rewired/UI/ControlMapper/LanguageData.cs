using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C34 RID: 3124
	public class LanguageData : ScriptableObject
	{
		// Token: 0x06004C9B RID: 19611 RVA: 0x00273E44 File Offset: 0x00272244
		public void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			this.customDict = LanguageData.CustomEntry.ToDictionary(this._customEntries);
			this._initialized = true;
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x00273E6C File Offset: 0x0027226C
		public string GetCustomEntry(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return string.Empty;
			}
			string result;
			if (!this.customDict.TryGetValue(key, out result))
			{
				return string.Empty;
			}
			return result;
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x00273EA4 File Offset: 0x002722A4
		public bool ContainsCustomEntryKey(string key)
		{
			return !string.IsNullOrEmpty(key) && this.customDict.ContainsKey(key);
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x00273EC0 File Offset: 0x002722C0
		public string yes
		{
			get
			{
				return Localization.Translate(this._yes).text;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06004C9F RID: 19615 RVA: 0x00273EE0 File Offset: 0x002722E0
		public string no
		{
			get
			{
				return Localization.Translate(this._no).text;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06004CA0 RID: 19616 RVA: 0x00273F00 File Offset: 0x00272300
		public string add
		{
			get
			{
				return Localization.Translate(this._add).text;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06004CA1 RID: 19617 RVA: 0x00273F20 File Offset: 0x00272320
		public string replace
		{
			get
			{
				return Localization.Translate(this._replace).text;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x00273F40 File Offset: 0x00272340
		public string remove
		{
			get
			{
				return Localization.Translate(this._remove).text;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06004CA3 RID: 19619 RVA: 0x00273F60 File Offset: 0x00272360
		public string cancel
		{
			get
			{
				return Localization.Translate(this._cancel).text;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x00273F80 File Offset: 0x00272380
		public string none
		{
			get
			{
				return Localization.Translate(this._none).text;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06004CA5 RID: 19621 RVA: 0x00273FA0 File Offset: 0x002723A0
		public string okay
		{
			get
			{
				return Localization.Translate(this._okay).text;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06004CA6 RID: 19622 RVA: 0x00273FC0 File Offset: 0x002723C0
		public string done
		{
			get
			{
				return Localization.Translate(this._done).text;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06004CA7 RID: 19623 RVA: 0x00273FE0 File Offset: 0x002723E0
		public string default_
		{
			get
			{
				return Localization.Translate(this._default).text;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06004CA8 RID: 19624 RVA: 0x00274000 File Offset: 0x00272400
		public string assignControllerWindowTitle
		{
			get
			{
				return Localization.Translate(this._assignControllerWindowTitle).text;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x00274020 File Offset: 0x00272420
		public string assignControllerWindowMessage
		{
			get
			{
				return Localization.Translate(this._assignControllerWindowMessage).text;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06004CAA RID: 19626 RVA: 0x00274040 File Offset: 0x00272440
		public string controllerAssignmentConflictWindowTitle
		{
			get
			{
				return Localization.Translate(this._controllerAssignmentConflictWindowTitle).text;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06004CAB RID: 19627 RVA: 0x00274060 File Offset: 0x00272460
		public string elementAssignmentPrePollingWindowMessage
		{
			get
			{
				return Localization.Translate(this._elementAssignmentPrePollingWindowMessage).text;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06004CAC RID: 19628 RVA: 0x00274080 File Offset: 0x00272480
		public string elementAssignmentConflictWindowMessage
		{
			get
			{
				return Localization.Translate(this._elementAssignmentConflictWindowMessage).text;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06004CAD RID: 19629 RVA: 0x002740A0 File Offset: 0x002724A0
		public string mouseAssignmentConflictWindowTitle
		{
			get
			{
				return Localization.Translate(this._mouseAssignmentConflictWindowTitle).text;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06004CAE RID: 19630 RVA: 0x002740C0 File Offset: 0x002724C0
		public string calibrateControllerWindowTitle
		{
			get
			{
				return Localization.Translate(this._calibrateControllerWindowTitle).text;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06004CAF RID: 19631 RVA: 0x002740E0 File Offset: 0x002724E0
		public string calibrateAxisStep1WindowTitle
		{
			get
			{
				return Localization.Translate(this._calibrateAxisStep1WindowTitle).text;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06004CB0 RID: 19632 RVA: 0x00274100 File Offset: 0x00272500
		public string calibrateAxisStep2WindowTitle
		{
			get
			{
				return Localization.Translate(this._calibrateAxisStep2WindowTitle).text;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06004CB1 RID: 19633 RVA: 0x00274120 File Offset: 0x00272520
		public string inputBehaviorSettingsWindowTitle
		{
			get
			{
				return Localization.Translate(this._inputBehaviorSettingsWindowTitle).text;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x00274140 File Offset: 0x00272540
		public string restoreDefaultsWindowTitle
		{
			get
			{
				return Localization.Translate(this._restoreDefaultsWindowTitle).text;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x00274160 File Offset: 0x00272560
		public string actionColumnLabel
		{
			get
			{
				return Localization.Translate(this._actionColumnLabel).text;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x00274180 File Offset: 0x00272580
		public string keyboardColumnLabel
		{
			get
			{
				return Localization.Translate(this._keyboardColumnLabel).text;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06004CB5 RID: 19637 RVA: 0x002741A0 File Offset: 0x002725A0
		public string mouseColumnLabel
		{
			get
			{
				return Localization.Translate(this._mouseColumnLabel).text;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x002741C0 File Offset: 0x002725C0
		public string controllerColumnLabel
		{
			get
			{
				return Localization.Translate(this._controllerColumnLabel).text;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x002741E0 File Offset: 0x002725E0
		public string removeControllerButtonLabel
		{
			get
			{
				return Localization.Translate(this._removeControllerButtonLabel).text;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x00274200 File Offset: 0x00272600
		public string calibrateControllerButtonLabel
		{
			get
			{
				return Localization.Translate(this._calibrateControllerButtonLabel).text;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x00274220 File Offset: 0x00272620
		public string assignControllerButtonLabel
		{
			get
			{
				return Localization.Translate(this._assignControllerButtonLabel).text;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x00274240 File Offset: 0x00272640
		public string inputBehaviorSettingsButtonLabel
		{
			get
			{
				return Localization.Translate(this._inputBehaviorSettingsButtonLabel).text;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x00274260 File Offset: 0x00272660
		public string doneButtonLabel
		{
			get
			{
				return Localization.Translate(this._doneButtonLabel).text;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x00274280 File Offset: 0x00272680
		public string restoreDefaultsButtonLabel
		{
			get
			{
				return Localization.Translate(this._restoreDefaultsButtonLabel).text;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06004CBD RID: 19645 RVA: 0x002742A0 File Offset: 0x002726A0
		public string controllerSettingsGroupLabel
		{
			get
			{
				return Localization.Translate(this._controllerSettingsGroupLabel).text;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06004CBE RID: 19646 RVA: 0x002742C0 File Offset: 0x002726C0
		public string playersGroupLabel
		{
			get
			{
				return Localization.Translate(this._playersGroupLabel).text;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06004CBF RID: 19647 RVA: 0x002742E0 File Offset: 0x002726E0
		public string assignedControllersGroupLabel
		{
			get
			{
				return Localization.Translate(this._assignedControllersGroupLabel).text;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06004CC0 RID: 19648 RVA: 0x00274300 File Offset: 0x00272700
		public string settingsGroupLabel
		{
			get
			{
				return Localization.Translate(this._settingsGroupLabel).text;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x00274320 File Offset: 0x00272720
		public string mapCategoriesGroupLabel
		{
			get
			{
				return Localization.Translate(this._mapCategoriesGroupLabel).text;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06004CC2 RID: 19650 RVA: 0x00274340 File Offset: 0x00272740
		public string restoreDefaultsWindowMessage
		{
			get
			{
				if (ReInput.players.playerCount > 1)
				{
					return Localization.Translate(this._restoreDefaultsWindowMessage_multiPlayer).text;
				}
				return Localization.Translate(this._restoreDefaultsWindowMessage_onePlayer).text;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x00274384 File Offset: 0x00272784
		public string calibrateWindow_deadZoneSliderLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_deadZoneSliderLabel).text;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x002743A4 File Offset: 0x002727A4
		public string calibrateWindow_zeroSliderLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_zeroSliderLabel).text;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x002743C4 File Offset: 0x002727C4
		public string calibrateWindow_sensitivitySliderLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_sensitivitySliderLabel).text;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06004CC6 RID: 19654 RVA: 0x002743E4 File Offset: 0x002727E4
		public string calibrateWindow_invertToggleLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_invertToggleLabel).text;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06004CC7 RID: 19655 RVA: 0x00274404 File Offset: 0x00272804
		public string calibrateWindow_calibrateButtonLabel
		{
			get
			{
				return Localization.Translate(this._calibrateWindow_calibrateButtonLabel).text;
			}
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x00274424 File Offset: 0x00272824
		public string GetControllerAssignmentConflictWindowMessage(string joystickName, string otherPlayerName, string currentPlayerName)
		{
			return string.Format(Localization.Translate(this._controllerAssignmentConflictWindowMessage).text, joystickName, otherPlayerName, currentPlayerName);
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x0027444C File Offset: 0x0027284C
		public string GetJoystickElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(Localization.Translate(this._joystickElementAssignmentPollingWindowMessage).text, actionName);
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x00274472 File Offset: 0x00272872
		public string GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
		{
			return string.Format(this._joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName);
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x00274480 File Offset: 0x00272880
		public string GetKeyboardElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(Localization.Translate(this._keyboardElementAssignmentPollingWindowMessage).text, actionName);
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x002744A8 File Offset: 0x002728A8
		public string GetMouseElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(Localization.Translate(this._mouseElementAssignmentPollingWindowMessage).text, actionName);
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x002744CE File Offset: 0x002728CE
		public string GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
		{
			return string.Format(this._mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName);
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x002744DC File Offset: 0x002728DC
		public string GetElementAlreadyInUseBlocked(string elementName)
		{
			return string.Format(Localization.Translate(this._elementAlreadyInUseBlocked).text, elementName);
		}

		// Token: 0x06004CCF RID: 19663 RVA: 0x00274504 File Offset: 0x00272904
		public string GetElementAlreadyInUseCanReplace(string elementName, bool allowConflicts)
		{
			if (!allowConflicts)
			{
				return string.Format(Localization.Translate(this._elementAlreadyInUseCanReplace).text, elementName);
			}
			return string.Format(Localization.Translate(this._elementAlreadyInUseCanReplace_conflictAllowed).text, elementName);
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x0027454C File Offset: 0x0027294C
		public int GetElementAlreadyInUseCanReplaceFontSize(bool allowConflicts)
		{
			if (!allowConflicts)
			{
				return Localization.Translate(this._elementAlreadyInUseCanReplace).fonts.fontSize;
			}
			return Localization.Translate(this._elementAlreadyInUseCanReplace_conflictAllowed).fonts.fontSize;
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x00274590 File Offset: 0x00272990
		public string GetMouseAssignmentConflictWindowMessage(string otherPlayerName, string thisPlayerName)
		{
			return string.Format(Localization.Translate(this._mouseAssignmentConflictWindowMessage).text, otherPlayerName, thisPlayerName);
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x002745B8 File Offset: 0x002729B8
		public string GetCalibrateAxisStep1WindowMessage(string axisName)
		{
			return string.Format(Localization.Translate(this._calibrateAxisStep1WindowMessage).text, axisName);
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x002745E0 File Offset: 0x002729E0
		public string GetCalibrateAxisStep2WindowMessage(string axisName)
		{
			return string.Format(Localization.Translate(this._calibrateAxisStep2WindowMessage).text, axisName);
		}

		// Token: 0x040050F4 RID: 20724
		[SerializeField]
		private string _yes = "Yes";

		// Token: 0x040050F5 RID: 20725
		[SerializeField]
		private string _no = "No";

		// Token: 0x040050F6 RID: 20726
		[SerializeField]
		private string _add = "Add";

		// Token: 0x040050F7 RID: 20727
		[SerializeField]
		private string _replace = "Replace";

		// Token: 0x040050F8 RID: 20728
		[SerializeField]
		private string _remove = "Remove";

		// Token: 0x040050F9 RID: 20729
		[SerializeField]
		private string _cancel = "Cancel";

		// Token: 0x040050FA RID: 20730
		[SerializeField]
		private string _none = "None";

		// Token: 0x040050FB RID: 20731
		[SerializeField]
		private string _okay = "Okay";

		// Token: 0x040050FC RID: 20732
		[SerializeField]
		private string _done = "Done";

		// Token: 0x040050FD RID: 20733
		[SerializeField]
		private string _default = "Default";

		// Token: 0x040050FE RID: 20734
		[SerializeField]
		private string _assignControllerWindowTitle = "Choose Controller";

		// Token: 0x040050FF RID: 20735
		[SerializeField]
		private string _assignControllerWindowMessage = "Press any button or move an axis on the controller you would like to use.";

		// Token: 0x04005100 RID: 20736
		[SerializeField]
		private string _controllerAssignmentConflictWindowTitle = "Controller Assignment";

		// Token: 0x04005101 RID: 20737
		[SerializeField]
		[Tooltip("{0} = Joystick Name\n{1} = Other Player Name\n{2} = This Player Name")]
		private string _controllerAssignmentConflictWindowMessage = "{0} is already assigned to {1}. Do you want to assign this controller to {2} instead?";

		// Token: 0x04005102 RID: 20738
		[SerializeField]
		private string _elementAssignmentPrePollingWindowMessage = "First center or zero all sticks and axes and press any button or wait for the timer to finish.";

		// Token: 0x04005103 RID: 20739
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _joystickElementAssignmentPollingWindowMessage = "Now press a button or move an axis to assign it to {0}.";

		// Token: 0x04005104 RID: 20740
		[SerializeField]
		[Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
		private string _joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Now move an axis to assign it to {0}.";

		// Token: 0x04005105 RID: 20741
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _keyboardElementAssignmentPollingWindowMessage = "Press a key to assign it to {0}. Modifier keys may also be used. To assign a modifier key alone, hold it down for 1 second.";

		// Token: 0x04005106 RID: 20742
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _mouseElementAssignmentPollingWindowMessage = "Press a mouse button or move an axis to assign it to {0}.";

		// Token: 0x04005107 RID: 20743
		[SerializeField]
		[Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
		private string _mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Move an axis to assign it to {0}.";

		// Token: 0x04005108 RID: 20744
		[SerializeField]
		private string _elementAssignmentConflictWindowMessage = "Assignment Conflict";

		// Token: 0x04005109 RID: 20745
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseBlocked = "{0} is already in use cannot be replaced.";

		// Token: 0x0400510A RID: 20746
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseCanReplace = "{0} is already in use. Do you want to replace it?";

		// Token: 0x0400510B RID: 20747
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseCanReplace_conflictAllowed = "{0} is already in use. Do you want to replace it? You may also choose to add the assignment anyway.";

		// Token: 0x0400510C RID: 20748
		[SerializeField]
		private string _mouseAssignmentConflictWindowTitle = "Mouse Assignment";

		// Token: 0x0400510D RID: 20749
		[SerializeField]
		[Tooltip("{0} = Other Player Name\n{1} = This Player Name")]
		private string _mouseAssignmentConflictWindowMessage = "The mouse is already assigned to {0}. Do you want to assign the mouse to {1} instead?";

		// Token: 0x0400510E RID: 20750
		[SerializeField]
		private string _calibrateControllerWindowTitle = "Calibrate Controller";

		// Token: 0x0400510F RID: 20751
		[SerializeField]
		private string _calibrateAxisStep1WindowTitle = "Calibrate Zero";

		// Token: 0x04005110 RID: 20752
		[SerializeField]
		[Tooltip("{0} = Axis Name")]
		private string _calibrateAxisStep1WindowMessage = "Center or zero {0} and press any button or wait for the timer to finish.";

		// Token: 0x04005111 RID: 20753
		[SerializeField]
		private string _calibrateAxisStep2WindowTitle = "Calibrate Range";

		// Token: 0x04005112 RID: 20754
		[SerializeField]
		[Tooltip("{0} = Axis Name")]
		private string _calibrateAxisStep2WindowMessage = "Move {0} through its entire range then press any button or wait for the timer to finish.";

		// Token: 0x04005113 RID: 20755
		[SerializeField]
		private string _inputBehaviorSettingsWindowTitle = "Sensitivity Settings";

		// Token: 0x04005114 RID: 20756
		[SerializeField]
		private string _restoreDefaultsWindowTitle = "Restore Defaults";

		// Token: 0x04005115 RID: 20757
		[SerializeField]
		[Tooltip("Message for a single player game.")]
		private string _restoreDefaultsWindowMessage_onePlayer = "This will restore the default input configuration. Are you sure you want to do this?";

		// Token: 0x04005116 RID: 20758
		[SerializeField]
		[Tooltip("Message for a multi-player game.")]
		private string _restoreDefaultsWindowMessage_multiPlayer = "This will restore the default input configuration for all players. Are you sure you want to do this?";

		// Token: 0x04005117 RID: 20759
		[SerializeField]
		private string _actionColumnLabel = "Actions";

		// Token: 0x04005118 RID: 20760
		[SerializeField]
		private string _keyboardColumnLabel = "Keyboard";

		// Token: 0x04005119 RID: 20761
		[SerializeField]
		private string _mouseColumnLabel = "Mouse";

		// Token: 0x0400511A RID: 20762
		[SerializeField]
		private string _controllerColumnLabel = "Controller";

		// Token: 0x0400511B RID: 20763
		[SerializeField]
		private string _removeControllerButtonLabel = "Remove";

		// Token: 0x0400511C RID: 20764
		[SerializeField]
		private string _calibrateControllerButtonLabel = "Calibrate";

		// Token: 0x0400511D RID: 20765
		[SerializeField]
		private string _assignControllerButtonLabel = "Assign Controller";

		// Token: 0x0400511E RID: 20766
		[SerializeField]
		private string _inputBehaviorSettingsButtonLabel = "Sensitivity";

		// Token: 0x0400511F RID: 20767
		[SerializeField]
		private string _doneButtonLabel = "Done";

		// Token: 0x04005120 RID: 20768
		[SerializeField]
		private string _restoreDefaultsButtonLabel = "Restore Defaults";

		// Token: 0x04005121 RID: 20769
		[SerializeField]
		private string _playersGroupLabel = "Players:";

		// Token: 0x04005122 RID: 20770
		[SerializeField]
		private string _controllerSettingsGroupLabel = "Controller:";

		// Token: 0x04005123 RID: 20771
		[SerializeField]
		private string _assignedControllersGroupLabel = "Assigned Controllers:";

		// Token: 0x04005124 RID: 20772
		[SerializeField]
		private string _settingsGroupLabel = "Settings:";

		// Token: 0x04005125 RID: 20773
		[SerializeField]
		private string _mapCategoriesGroupLabel = "Categories:";

		// Token: 0x04005126 RID: 20774
		[SerializeField]
		private string _calibrateWindow_deadZoneSliderLabel = "Dead Zone:";

		// Token: 0x04005127 RID: 20775
		[SerializeField]
		private string _calibrateWindow_zeroSliderLabel = "Zero:";

		// Token: 0x04005128 RID: 20776
		[SerializeField]
		private string _calibrateWindow_sensitivitySliderLabel = "Sensitivity:";

		// Token: 0x04005129 RID: 20777
		[SerializeField]
		private string _calibrateWindow_invertToggleLabel = "Invert";

		// Token: 0x0400512A RID: 20778
		[SerializeField]
		private string _calibrateWindow_calibrateButtonLabel = "Calibrate";

		// Token: 0x0400512B RID: 20779
		[SerializeField]
		private LanguageData.CustomEntry[] _customEntries;

		// Token: 0x0400512C RID: 20780
		private bool _initialized;

		// Token: 0x0400512D RID: 20781
		private Dictionary<string, string> customDict;

		// Token: 0x02000C35 RID: 3125
		[Serializable]
		private class CustomEntry
		{
			// Token: 0x06004CD4 RID: 19668 RVA: 0x00274606 File Offset: 0x00272A06
			public CustomEntry()
			{
			}

			// Token: 0x06004CD5 RID: 19669 RVA: 0x0027460E File Offset: 0x00272A0E
			public CustomEntry(string key, string value)
			{
				this.key = key;
				this.value = value;
			}

			// Token: 0x06004CD6 RID: 19670 RVA: 0x00274624 File Offset: 0x00272A24
			public static Dictionary<string, string> ToDictionary(LanguageData.CustomEntry[] array)
			{
				if (array == null)
				{
					return new Dictionary<string, string>();
				}
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						if (!string.IsNullOrEmpty(array[i].key) && !string.IsNullOrEmpty(array[i].value))
						{
							if (dictionary.ContainsKey(array[i].key))
							{
								UnityEngine.Debug.LogError("Key \"" + array[i].key + "\" is already in dictionary!");
							}
							else
							{
								dictionary.Add(array[i].key, array[i].value);
							}
						}
					}
				}
				return dictionary;
			}

			// Token: 0x0400512E RID: 20782
			public string key;

			// Token: 0x0400512F RID: 20783
			public string value;
		}
	}
}
