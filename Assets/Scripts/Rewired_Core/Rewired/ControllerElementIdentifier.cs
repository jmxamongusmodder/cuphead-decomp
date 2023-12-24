using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public class ControllerElementIdentifier
	{
		[SerializeField]
		private int _id;
		[SerializeField]
		private string _name;
		[SerializeField]
		private string _positiveName;
		[SerializeField]
		private string _negativeName;
		[SerializeField]
		private ControllerElementType _elementType;
		[SerializeField]
		private CompoundControllerElementType _compoundElementType;
	}
}
