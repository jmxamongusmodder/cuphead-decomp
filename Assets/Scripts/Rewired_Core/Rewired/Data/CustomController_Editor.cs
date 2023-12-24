using System;
using Rewired;
using Rewired.Data.Mapping;
using UnityEngine;
using System.Collections.Generic;

namespace Rewired.Data
{
	[Serializable]
	public class CustomController_Editor
	{
		[Serializable]
		public class Element
		{
			public Element(string name, int elementIdentifierId)
			{
			}

			public int elementIdentifierId;
			public string name;
		}

		[Serializable]
		public class Axis : Element
		{
			public Axis(string name) : base(default(string), default(int))
			{
			}

			public AxisRange range;
			public bool invert;
			public float deadZone;
			public float zero;
			public float min;
			public float max;
			public bool doNotCalibrateRange;
			public HardwareAxisInfo axisInfo;
		}

		[Serializable]
		public class Button : Element
		{
			public Button(string name) : base(default(string), default(int))
			{
			}

		}

		[SerializeField]
		private string _name;
		[SerializeField]
		private string _descriptiveName;
		[SerializeField]
		private int _id;
		[SerializeField]
		private List<ControllerElementIdentifier> _elementIdentifiers;
		[SerializeField]
		private List<CustomController_Editor.Axis> _axes;
		[SerializeField]
		private List<CustomController_Editor.Button> _buttons;
		[SerializeField]
		private int _elementIdentifierIdCounter;
	}
}
