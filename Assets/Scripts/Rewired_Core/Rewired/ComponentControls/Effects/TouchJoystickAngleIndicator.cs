using UnityEngine;

namespace Rewired.ComponentControls.Effects
{
	public class TouchJoystickAngleIndicator : MonoBehaviour
	{
		private TouchJoystickAngleIndicator()
		{
		}

		[SerializeField]
		private bool _visible;
		[SerializeField]
		private bool _targetAngleFromRotation;
		[SerializeField]
		private float _targetAngle;
		[SerializeField]
		private bool _fadeWithValue;
		[SerializeField]
		private bool _fadeWithAngle;
		[SerializeField]
		private float _fadeRange;
		[SerializeField]
		private Color _activeColor;
		[SerializeField]
		private Color _normalColor;
	}
}
