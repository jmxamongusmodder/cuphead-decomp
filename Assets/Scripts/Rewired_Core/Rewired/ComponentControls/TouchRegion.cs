using System;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	public class TouchRegion : TouchInteractable
	{
		private TouchRegion()
		{
		}

		[SerializeField]
		private bool _hideAtRuntime;
	}
}
