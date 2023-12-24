using UnityEngine;
using Rewired.Data;

namespace Rewired
{
	public class InputManager_Base : MonoBehaviour
	{
		[SerializeField]
		private bool _dontDestroyOnLoad;
		[SerializeField]
		private UserData _userData;
		[SerializeField]
		private ControllerDataFiles _controllerDataFiles;
	}
}
