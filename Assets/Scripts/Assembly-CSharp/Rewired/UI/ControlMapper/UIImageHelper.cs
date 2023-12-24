using UnityEngine;
using System;

namespace Rewired.UI.ControlMapper
{
	public class UIImageHelper : MonoBehaviour
	{
		[Serializable]
		private class State
		{
			[SerializeField]
			public Color color;
		}

		[SerializeField]
		private State enabledState;
		[SerializeField]
		private State disabledState;
	}
}
