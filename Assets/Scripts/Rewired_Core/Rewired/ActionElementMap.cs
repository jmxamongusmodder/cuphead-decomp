using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public class ActionElementMap
	{
		[SerializeField]
		private int _actionCategoryId;
		[SerializeField]
		internal int _actionId;
		[SerializeField]
		internal ControllerElementType _elementType;
		[SerializeField]
		internal int _elementIdentifierId;
		[SerializeField]
		internal AxisRange _axisRange;
		[SerializeField]
		internal bool _invert;
		[SerializeField]
		internal Pole _axisContribution;
		[SerializeField]
		internal KeyboardKeyCode _keyboardKeyCode;
		[SerializeField]
		internal ModifierKey _modifierKey1;
		[SerializeField]
		internal ModifierKey _modifierKey2;
		[SerializeField]
		internal ModifierKey _modifierKey3;
	}
}
