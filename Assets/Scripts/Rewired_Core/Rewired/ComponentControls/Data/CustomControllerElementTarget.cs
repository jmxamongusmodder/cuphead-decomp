using System;
using UnityEngine;
using Rewired;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	public class CustomControllerElementTarget
	{
		internal enum ValueRange
		{
			Full = 0,
			Positive = 1,
			Negative = 2,
		}

		[SerializeField]
		private CustomControllerElementSelector _element;
		[SerializeField]
		private ValueRange _valueRange;
		[SerializeField]
		private Pole _valueContribution;
		[SerializeField]
		private bool _invert;
	}
}
