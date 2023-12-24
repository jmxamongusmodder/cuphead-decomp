using UnityEngine;

namespace Rewired.ComponentControls.Effects
{
	public class RotateAroundAxis : MonoBehaviour
	{
		public enum Speed
		{
			Stopped = 0,
			Slow = 1,
			Fast = 2,
		}

		public enum RotationAxis
		{
			X = 0,
			Y = 1,
			Z = 2,
		}

		[SerializeField]
		private Speed _speed;
		[SerializeField]
		private float _slowRotationSpeed;
		[SerializeField]
		private float _fastRotationSpeed;
		[SerializeField]
		private RotationAxis _rotateAroundAxis;
		[SerializeField]
		private Space _relativeTo;
		[SerializeField]
		private bool _reverse;
	}
}
