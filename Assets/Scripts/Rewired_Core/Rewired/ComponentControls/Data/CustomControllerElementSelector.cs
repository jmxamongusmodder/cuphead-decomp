using System;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	public class CustomControllerElementSelector
	{
		public enum ElementType
		{
			Axis = 0,
			Button = 1,
		}

		public enum SelectorType
		{
			Name = 0,
			Index = 1,
			Id = 2,
		}

		[SerializeField]
		private ElementType _elementType;
		[SerializeField]
		private SelectorType _selectorType;
		[SerializeField]
		private string _elementName;
		[SerializeField]
		private int _elementIndex;
		[SerializeField]
		private int _elementId;
	}
}
