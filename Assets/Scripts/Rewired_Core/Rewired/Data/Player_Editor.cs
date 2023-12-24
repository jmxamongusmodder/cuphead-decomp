using System;
using UnityEngine;
using System.Collections.Generic;

namespace Rewired.Data
{
	[Serializable]
	public class Player_Editor
	{
		[Serializable]
		public class Mapping
		{
			[SerializeField]
			private bool _enabled;
			[SerializeField]
			private int _categoryId;
			[SerializeField]
			private int _layoutId;
		}

		[Serializable]
		public class CreateControllerInfo
		{
			public CreateControllerInfo(Player_Editor.CreateControllerInfo source)
			{
			}

			[SerializeField]
			private int _sourceId;
			[SerializeField]
			private string _tag;
		}

		[SerializeField]
		private int _id;
		[SerializeField]
		private string _name;
		[SerializeField]
		private string _descriptiveName;
		[SerializeField]
		private bool _startPlaying;
		[SerializeField]
		private List<Player_Editor.Mapping> _defaultJoystickMaps;
		[SerializeField]
		private List<Player_Editor.Mapping> _defaultMouseMaps;
		[SerializeField]
		private List<Player_Editor.Mapping> _defaultKeyboardMaps;
		[SerializeField]
		private List<Player_Editor.Mapping> _defaultCustomControllerMaps;
		[SerializeField]
		private List<Player_Editor.CreateControllerInfo> _startingCustomControllers;
		[SerializeField]
		private bool _assignMouseOnStart;
		[SerializeField]
		private bool _assignKeyboardOnStart;
		[SerializeField]
		private bool _excludeFromControllerAutoAssignment;
	}
}
