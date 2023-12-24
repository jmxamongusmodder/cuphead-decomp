using System;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	public class InputCategory
	{
		[SerializeField]
		protected string _name;
		[SerializeField]
		protected string _descriptiveName;
		[SerializeField]
		protected string _tag;
		[SerializeField]
		protected int _id;
		[SerializeField]
		protected bool _userAssignable;
	}
}
