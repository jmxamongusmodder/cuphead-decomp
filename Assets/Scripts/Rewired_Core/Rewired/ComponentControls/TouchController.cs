using System;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	public class TouchController : CustomController
	{
		private TouchController()
		{
		}

		[SerializeField]
		private bool _disableMouseInputWhenEnabled;
		[SerializeField]
		private bool _useCustomController;
	}
}
