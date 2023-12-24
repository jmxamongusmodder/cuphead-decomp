using UnityEngine;

namespace Rewired.Data
{
	public class UserDataStore_Cuphead : UserDataStore
	{
		[SerializeField]
		private bool isEnabled;
		[SerializeField]
		private bool loadDataOnStart;
		[SerializeField]
		private string playerPrefsKeyPrefix;
	}
}
