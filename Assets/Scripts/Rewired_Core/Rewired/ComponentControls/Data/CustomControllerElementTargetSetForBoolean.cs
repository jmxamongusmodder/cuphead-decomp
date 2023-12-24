using System;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	public class CustomControllerElementTargetSetForBoolean : CustomControllerElementTargetSet
	{
		[SerializeField]
		private CustomControllerElementTarget _target;
	}
}
