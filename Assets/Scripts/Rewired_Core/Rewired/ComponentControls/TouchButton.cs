using System;
using UnityEngine.Events;
using Rewired.ComponentControls.Data;
using UnityEngine;
using Rewired.Internal;

namespace Rewired.ComponentControls
{
	[Serializable]
	public class TouchButton : TouchInteractable
	{
		[Serializable]
		public class AxisValueChangedEventHandler : UnityEvent<float>
		{
		}

		[Serializable]
		public class ButtonValueChangedEventHandler : UnityEvent<bool>
		{
		}

		[Serializable]
		public class ButtonDownEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class ButtonUpEventHandler : UnityEvent
		{
		}

		public enum ButtonType
		{
			Standard = 0,
			ToggleSwitch = 1,
		}

		private TouchButton()
		{
		}

		[SerializeField]
		private CustomControllerElementTargetSetForFloat _targetCustomControllerElement;
		[SerializeField]
		private ButtonType _buttonType;
		[SerializeField]
		private bool _activateOnSwipeIn;
		[SerializeField]
		private bool _stayActiveOnSwipeOut;
		[SerializeField]
		private bool _useDigitalAxisSimulation;
		[SerializeField]
		private float _digitalAxisGravity;
		[SerializeField]
		private float _digitalAxisSensitivity;
		[SerializeField]
		private StandaloneAxis _axis;
		[SerializeField]
		private TouchRegion _touchRegion;
		[SerializeField]
		private bool _useTouchRegionOnly;
		[SerializeField]
		private bool _moveToTouchPosition;
		[SerializeField]
		private bool _returnOnRelease;
		[SerializeField]
		private bool _followTouchPosition;
		[SerializeField]
		private bool _animateOnMoveToTouch;
		[SerializeField]
		private float _moveToTouchSpeed;
		[SerializeField]
		private bool _animateOnReturn;
		[SerializeField]
		private float _returnSpeed;
		[SerializeField]
		private bool _manageRaycasting;
		[SerializeField]
		private AxisValueChangedEventHandler _onAxisValueChanged;
		[SerializeField]
		private ButtonValueChangedEventHandler _onButtonValueChanged;
		[SerializeField]
		private ButtonDownEventHandler _onButtonDown;
		[SerializeField]
		private ButtonUpEventHandler _onButtonUp;
	}
}
