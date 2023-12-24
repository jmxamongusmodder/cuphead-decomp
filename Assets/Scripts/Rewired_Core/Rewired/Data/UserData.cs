using System;
using UnityEngine;
using System.Collections.Generic;
using Rewired;
using Rewired.Data.Mapping;

namespace Rewired.Data
{
	[Serializable]
	public class UserData
	{
		[SerializeField]
		private ConfigVars configVars;
		[SerializeField]
		private List<Player_Editor> players;
		[SerializeField]
		private List<InputAction> actions;
		[SerializeField]
		private List<InputCategory> actionCategories;
		[SerializeField]
		private ActionCategoryMap actionCategoryMap;
		[SerializeField]
		private List<InputBehavior> inputBehaviors;
		[SerializeField]
		private List<InputMapCategory> mapCategories;
		[SerializeField]
		private List<InputLayout> joystickLayouts;
		[SerializeField]
		private List<InputLayout> keyboardLayouts;
		[SerializeField]
		private List<InputLayout> mouseLayouts;
		[SerializeField]
		private List<InputLayout> customControllerLayouts;
		[SerializeField]
		private List<ControllerMap_Editor> joystickMaps;
		[SerializeField]
		private List<ControllerMap_Editor> keyboardMaps;
		[SerializeField]
		private List<ControllerMap_Editor> mouseMaps;
		[SerializeField]
		private List<ControllerMap_Editor> customControllerMaps;
		[SerializeField]
		private List<CustomController_Editor> customControllers;
		[SerializeField]
		private int playerIdCounter;
		[SerializeField]
		private int actionIdCounter;
		[SerializeField]
		private int actionCategoryIdCounter;
		[SerializeField]
		private int inputBehaviorIdCounter;
		[SerializeField]
		private int mapCategoryIdCounter;
		[SerializeField]
		private int joystickLayoutIdCounter;
		[SerializeField]
		private int keyboardLayoutIdCounter;
		[SerializeField]
		private int mouseLayoutIdCounter;
		[SerializeField]
		private int customControllerLayoutIdCounter;
		[SerializeField]
		private int joystickMapIdCounter;
		[SerializeField]
		private int keyboardMapIdCounter;
		[SerializeField]
		private int mouseMapIdCounter;
		[SerializeField]
		private int customControllerMapIdCounter;
		[SerializeField]
		private int customControllerIdCounter;
	}
}
