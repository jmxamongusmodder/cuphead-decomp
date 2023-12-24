using System;
using UnityEngine;
using Rewired;

namespace Rewired.Internal
{
	[Serializable]
	internal class StandaloneAxis
	{
		[SerializeField]
		private float _buttonActivationThreshold;
		[SerializeField]
		private AxisCalibration _calibration;
	}
}
