using UnityEngine;
using System;

namespace Rewired.UI.ControlMapper
{
	public class ThemedElement : MonoBehaviour
	{
		[Serializable]
		public class ElementInfo
		{
			[SerializeField]
			private string _themeClass;
			[SerializeField]
			private Component _component;
		}

		[SerializeField]
		private ElementInfo[] _elements;
	}
}
