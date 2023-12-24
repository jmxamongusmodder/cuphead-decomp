using System;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	public class CustomControllerElementTargetSetForFloat : CustomControllerElementTargetSet
	{
		[SerializeField]
		private bool _splitValue;
		[SerializeField]
		private CustomControllerElementTarget _target;
		[SerializeField]
		private CustomControllerElementTarget _positiveTarget;
		[SerializeField]
		private CustomControllerElementTarget _negativeTarget;
	}
}
