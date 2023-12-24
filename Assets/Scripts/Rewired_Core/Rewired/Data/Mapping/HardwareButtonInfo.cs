using System;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public class HardwareButtonInfo
	{
		[SerializeField]
		internal bool _excludeFromPolling;
		[SerializeField]
		internal bool _isPressureSensitive;
	}
}
