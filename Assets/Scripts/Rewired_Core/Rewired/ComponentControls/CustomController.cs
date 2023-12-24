using System;
using UnityEngine;
using Rewired;
using Rewired.ComponentControls.Data;

namespace Rewired.ComponentControls
{
	[Serializable]
	public class CustomController : ComponentController
	{
		[Serializable]
		public class CreateCustomControllerSettings
		{
			[SerializeField]
			private bool _createCustomController;
			[SerializeField]
			private int _customControllerSourceId;
			[SerializeField]
			private int _assignToPlayerId;
			[SerializeField]
			private bool _destroyCustomController;
		}

		[SerializeField]
		private InputManager_Base _rewiredInputManager;
		[SerializeField]
		private CustomControllerSelector _customControllerSelector;
		[SerializeField]
		private CreateCustomControllerSettings _createCustomControllerSettings;
	}
}
