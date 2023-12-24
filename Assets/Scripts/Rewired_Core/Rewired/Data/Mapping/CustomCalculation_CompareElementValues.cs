using System;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public class CustomCalculation_CompareElementValues : CustomCalculation
	{
		public enum ComparisonType
		{
			Min = 0,
			Max = 1,
			MinAbs = 2,
			MaxAbs = 3,
		}

		[SerializeField]
		private ComparisonType _comparisonType;
	}
}
