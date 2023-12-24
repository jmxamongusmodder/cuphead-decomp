using System;
using System.Collections.Generic;
using Rewired;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public class ControllerMap_Editor
	{
		public int id;
		public int categoryId;
		public int layoutId;
		public string name;
		public string hardwareGuidString;
		public int customControllerUid;
		public List<ActionElementMap> actionElementMaps;
	}
}
