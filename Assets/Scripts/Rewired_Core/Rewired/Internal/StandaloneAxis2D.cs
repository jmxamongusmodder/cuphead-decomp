using System;
using Rewired;
using UnityEngine;

namespace Rewired.Internal
{
	[Serializable]
	internal class StandaloneAxis2D
	{
		[SerializeField]
		private Axis2DCalibration _calibration;
		[SerializeField]
		private StandaloneAxis _xAxis;
		[SerializeField]
		private StandaloneAxis _yAxis;
	}
}
