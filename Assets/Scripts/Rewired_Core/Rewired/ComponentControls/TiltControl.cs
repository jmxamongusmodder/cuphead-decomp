using System;
using UnityEngine;
using Rewired.ComponentControls.Data;
using Rewired.Internal;

namespace Rewired.ComponentControls
{
	[Serializable]
	public class TiltControl : CustomControllerControl
	{
		public enum TiltDirection
		{
			Both = 0,
			Horizontal = 1,
			Forward = 2,
		}

		[SerializeField]
		private TiltDirection _allowedTiltDirections;
		[SerializeField]
		private CustomControllerElementTargetSetForFloat _horizontalTiltCustomControllerElement;
		[SerializeField]
		private float _horizontalTiltLimit;
		[SerializeField]
		private float _horizontalRestAngle;
		[SerializeField]
		private CustomControllerElementTargetSetForFloat _forwardTiltCustomControllerElement;
		[SerializeField]
		private float _forwardTiltLimit;
		[SerializeField]
		private float _forwardRestAngle;
		[SerializeField]
		private StandaloneAxis2D _axis2D;
	}
}
