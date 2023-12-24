using UnityEngine;
using System;
using Rewired;
using System.Collections.Generic;

namespace Rewired.Data.Mapping
{
	public class HardwareJoystickTemplateMap : ScriptableObject
	{
		[Serializable]
		public class ElementIdentifierMap
		{
			public int templateId;
			public int joystickId;
			public int joystickId2;
			public bool splitAxis;
			public Pole sourceAxisPole;
		}

		[Serializable]
		public class Entry
		{
			public int id;
			public string name;
			public string joystickGuid;
			public string fileGuid;
			public List<HardwareJoystickTemplateMap.ElementIdentifierMap> elementIdentifierMappings;
		}

		[SerializeField]
		private string controllerName;
		[SerializeField]
		private string description;
		[SerializeField]
		private string templateGuid;
		[SerializeField]
		private ControllerElementIdentifier[] elementIdentifiers;
		[SerializeField]
		private List<HardwareJoystickTemplateMap.Entry> joysticks;
		[SerializeField]
		private int elementIdentifierIdCounter;
		[SerializeField]
		private int joystickIdCounter;
	}
}
