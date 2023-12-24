using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public class AxisCalibration
	{
		[SerializeField]
		private bool _enabled;
		[SerializeField]
		private float _deadZone;
		[SerializeField]
		private float _calibratedZero;
		[SerializeField]
		private float _calibratedMin;
		[SerializeField]
		private float _calibratedMax;
		[SerializeField]
		private bool _invert;
		[SerializeField]
		private AxisSensitivityType _sensitivityType;
		[SerializeField]
		private float _sensitivity;
		[SerializeField]
		private AnimationCurve _sensitivityCurve;
		[SerializeField]
		private bool _applyRangeCalibration;
	}
}
