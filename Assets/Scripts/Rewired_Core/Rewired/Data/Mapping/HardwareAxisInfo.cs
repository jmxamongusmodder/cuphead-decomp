using System;
using Rewired;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public class HardwareAxisInfo
	{
		[SerializeField]
		internal AxisCoordinateMode _dataFormat;
		[SerializeField]
		internal bool _excludeFromPolling;
	}
}
