using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public class InputAction
	{
		[SerializeField]
		private int _id;
		[SerializeField]
		private string _name;
		[SerializeField]
		private InputActionType _type;
		[SerializeField]
		private string _descriptiveName;
		[SerializeField]
		private string _positiveDescriptiveName;
		[SerializeField]
		private string _negativeDescriptiveName;
		[SerializeField]
		private int _behaviorId;
		[SerializeField]
		private bool _userAssignable;
		[SerializeField]
		private int _categoryId;
	}
}
