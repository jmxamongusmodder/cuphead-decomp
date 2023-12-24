using UnityEngine;
using System;

namespace Rewired.Platforms.Switch
{
	public class NintendoSwitchInputManager : MonoBehaviour
	{
		[Serializable]
		private class NpadSettings_Internal
		{
			internal NpadSettings_Internal(int playerId)
			{
			}

			[SerializeField]
			private bool _isAllowed;
			[SerializeField]
			private int _rewiredPlayerId;
			[SerializeField]
			private int _joyConAssignmentMode;
		}

		[Serializable]
		private class DebugPadSettings_Internal
		{
			internal DebugPadSettings_Internal(int playerId)
			{
			}

			[SerializeField]
			private bool _enabled;
			[SerializeField]
			private int _rewiredPlayerId;
		}

		[Serializable]
		private class UserData
		{
			[SerializeField]
			private int _allowedNpadStyles;
			[SerializeField]
			private int _joyConGripStyle;
			[SerializeField]
			private bool _adjustIMUsForGripStyle;
			[SerializeField]
			private int _handheldActivationMode;
			[SerializeField]
			private bool _assignJoysticksByNpadId;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo1;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo2;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo3;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo4;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo5;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo6;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo7;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo8;
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadHandheld;
			[SerializeField]
			private NintendoSwitchInputManager.DebugPadSettings_Internal _debugPad;
		}

		[SerializeField]
		private UserData _userData;
	}
}
