using System;
using UnityEngine.Events;
using UnityEngine;
using Rewired.ComponentControls.Data;
using Rewired.Internal;

namespace Rewired.ComponentControls
{
	[Serializable]
	public class TouchJoystick : TouchInteractable
	{
		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<Vector2>
		{
		}

		[Serializable]
		public class TouchStartedEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class TouchEndedEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class TapEventHandler : UnityEvent
		{
		}

		public enum JoystickMode
		{
			Analog = 0,
			Digital = 1,
		}

		public enum StickBounds
		{
			Circle = 0,
			Square = 1,
		}

		public enum AxisDirection
		{
			Both = 0,
			Horizontal = 1,
			Vertical = 2,
		}

		public enum SnapDirections
		{
			None = 0,
			Four = 4,
			Eight = 8,
			Sixteen = 16,
			ThirtyTwo = 32,
			SixtyFour = 64,
		}

		private TouchJoystick()
		{
		}

		[SerializeField]
		private CustomControllerElementTargetSetForFloat _horizontalAxisCustomControllerElement;
		[SerializeField]
		private CustomControllerElementTargetSetForFloat _verticalAxisCustomControllerElement;
		[SerializeField]
		private CustomControllerElementTargetSetForBoolean _tapCustomControllerElement;
		[SerializeField]
		private RectTransform _stickTransform;
		[SerializeField]
		private JoystickMode _joystickMode;
		[SerializeField]
		private float _digitalModeDeadZone;
		[SerializeField]
		private float _stickRange;
		[SerializeField]
		private bool _scaleStickRange;
		[SerializeField]
		private StickBounds _stickBounds;
		[SerializeField]
		private AxisDirection _axesToUse;
		[SerializeField]
		private SnapDirections _snapDirections;
		[SerializeField]
		private bool _snapStickToTouch;
		[SerializeField]
		private bool _centerStickOnRelease;
		[SerializeField]
		private StandaloneAxis2D _axis2D;
		[SerializeField]
		private bool _activateOnSwipeIn;
		[SerializeField]
		private bool _stayActiveOnSwipeOut;
		[SerializeField]
		private bool _allowTap;
		[SerializeField]
		private float _tapTimeout;
		[SerializeField]
		private int _tapDistanceLimit;
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
		private ValueChangedEventHandler _onValueChanged;
		[SerializeField]
		private ValueChangedEventHandler _onStickPositionChanged;
		[SerializeField]
		private TouchStartedEventHandler _onTouchStarted;
		[SerializeField]
		private TouchEndedEventHandler _onTouchEnded;
		[SerializeField]
		private TapEventHandler _onTap;
	}
}
