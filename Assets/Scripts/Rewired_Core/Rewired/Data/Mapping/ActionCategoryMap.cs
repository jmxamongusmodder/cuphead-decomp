using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public class ActionCategoryMap
	{
		[Serializable]
		public class Entry
		{
			public Entry(int categoryId)
			{
			}

			public int categoryId;
			public List<int> actionIds;
		}

		[SerializeField]
		private List<ActionCategoryMap.Entry> list;
	}
}
