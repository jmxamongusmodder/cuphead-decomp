using System;
using UnityEngine.Events;
using UnityEngine;
using Rewired.ComponentControls.Data;
using Rewired.Internal;

namespace Rewired.ComponentControls
{
	[Serializable]
	public class TouchPad : TouchInteractable
	{
		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<Vector2>
		{
		}

		[Serializable]
		public class TapEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class PressDownEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class PressUpEventHandler : UnityEvent
		{
		}

		public enum AxisDirection
		{
			Both = 0,
			Horizontal = 1,
			Vertical = 2,
		}

		public enum TouchPadMode
		{
			Delta = 0,
			ScreenPosition = 1,
			VectorFromCenter = 2,
			VectorFromInitialTouch = 3,
		}

		public enum ValueFormat
		{
			Pixels = 0,
			Screen = 1,
			Physical = 2,
			Direction = 3,
		}

		private TouchPad()
		{
		}

		[SerializeField]
		private CustomControllerElementTargetSetForFloat _horizontalAxisCustomControllerElement;
		[SerializeField]
		private CustomControllerElementTargetSetForFloat _verticalAxisCustomControllerElement;
		[SerializeField]
		private CustomControllerElementTargetSetForBoolean _tapCustomControllerElement;
		[SerializeField]
		private CustomControllerElementTargetSetForBoolean _pressCustomControllerElement;
		[SerializeField]
		private AxisDirection _axesToUse;
		[SerializeField]
		private TouchPadMode _touchPadMode;
		[SerializeField]
		private ValueFormat _valueFormat;
		[SerializeField]
		private bool _useInertia;
		[SerializeField]
		private float _inertiaFriction;
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
		private bool _allowPress;
		[SerializeField]
		private float _pressStartDelay;
		[SerializeField]
		private int _pressDistanceLimit;
		[SerializeField]
		private bool _hideAtRuntime;
		[SerializeField]
		private StandaloneAxis2D _axis2D;
		[SerializeField]
		private ValueChangedEventHandler _onValueChanged;
		[SerializeField]
		private TapEventHandler _onTap;
		[SerializeField]
		private PressDownEventHandler _onPressDown;
		[SerializeField]
		private PressUpEventHandler _onPressUp;
	}
}
