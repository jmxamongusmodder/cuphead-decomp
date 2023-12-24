using System;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public class CustomCalculation_Accelerometer : CustomCalculation
	{
		public enum CalculationType
		{
			Pitch = 0,
			Roll = 1,
		}

		public enum InputType
		{
			Acceleration = 0,
			UserAcceleration = 1,
			Gravity = 2,
		}

		public enum OutputType
		{
			Axis = 0,
			Angle = 1,
		}

		public CalculationType _calculationType;
		public InputType _inputType;
		public OutputType _outputType;
	}
}
