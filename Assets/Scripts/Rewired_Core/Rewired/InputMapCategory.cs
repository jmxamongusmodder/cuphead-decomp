using System;
using UnityEngine;
using System.Collections.Generic;

namespace Rewired
{
	[Serializable]
	public class InputMapCategory : InputCategory
	{
		[SerializeField]
		private bool _checkConflictsWithAllCategories;
		[SerializeField]
		private List<int> _checkConflictsCategoryIds;
	}
}
