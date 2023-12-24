using UnityEngine;
using Rewired.Data.Mapping;

namespace Rewired.Data
{
	public class ControllerDataFiles : ScriptableObject
	{
		[SerializeField]
		private HardwareJoystickMap defaultHardwareJoystickMap;
		[SerializeField]
		private HardwareJoystickMap[] hardwareJoystickMaps;
		[SerializeField]
		private HardwareJoystickTemplateMap[] joystickTemplates;
	}
}
