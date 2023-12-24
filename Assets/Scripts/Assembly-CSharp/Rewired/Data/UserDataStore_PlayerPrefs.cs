using UnityEngine;

namespace Rewired.Data
{
	public class UserDataStore_PlayerPrefs : UserDataStore
	{
		[SerializeField]
		private bool isEnabled;
		[SerializeField]
		private bool loadDataOnStart;
		[SerializeField]
		private bool loadJoystickAssignments;
		[SerializeField]
		private bool loadKeyboardAssignments;
		[SerializeField]
		private bool loadMouseAssignments;
		[SerializeField]
		private string playerPrefsKeyPrefix;
	}
}
