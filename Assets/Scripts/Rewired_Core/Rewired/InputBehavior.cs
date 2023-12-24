using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public class InputBehavior
	{
		[SerializeField]
		private int _id;
		[SerializeField]
		private string _name;
		[SerializeField]
		private float _joystickAxisSensitivity;
		[SerializeField]
		private bool _digitalAxisSimulation;
		[SerializeField]
		private bool _digitalAxisSnap;
		[SerializeField]
		private bool _digitalAxisInstantReverse;
		[SerializeField]
		private float _digitalAxisGravity;
		[SerializeField]
		private float _digitalAxisSensitivity;
		[SerializeField]
		private MouseXYAxisMode _mouseXYAxisMode;
		[SerializeField]
		private MouseOtherAxisMode _mouseOtherAxisMode;
		[SerializeField]
		private float _mouseXYAxisSensitivity;
		[SerializeField]
		private MouseXYAxisDeltaCalc _mouseXYAxisDeltaCalc;
		[SerializeField]
		private float _mouseOtherAxisSensitivity;
		[SerializeField]
		private float _customControllerAxisSensitivity;
		[SerializeField]
		private float _buttonDoublePressSpeed;
		[SerializeField]
		private float _buttonShortPressTime;
		[SerializeField]
		private float _buttonShortPressExpiresIn;
		[SerializeField]
		private float _buttonLongPressTime;
		[SerializeField]
		private float _buttonLongPressExpiresIn;
		[SerializeField]
		private float _buttonDeadZone;
		[SerializeField]
		private float _buttonDownBuffer;
		[SerializeField]
		private float _buttonRepeatRate;
		[SerializeField]
		private float _buttonRepeatDelay;
	}
}
