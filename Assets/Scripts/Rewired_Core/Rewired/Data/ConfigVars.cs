using System;
using Rewired.Config;
using Rewired.Platforms;
using Rewired;

namespace Rewired.Data
{
	[Serializable]
	public class ConfigVars
	{
		[Serializable]
		public class PlatformVars
		{
			public bool disableKeyboard;
			public bool ignoreInputWhenAppNotInFocus;
		}

		[Serializable]
		public class PlatformVars_WindowsStandalone : PlatformVars
		{
			public bool useNativeKeyboard;
			public int joystickRefreshRate;
		}

		[Serializable]
		public class PlatformVars_WindowsUWP : PlatformVars
		{
			public bool useGamepadAPI;
			public bool useHIDAPI;
		}

		[Serializable]
		public class EditorVars
		{
			public bool exportConsts_useParentClass;
			public string exportConsts_parentClassName;
			public bool exportConsts_useNamespace;
			public string exportConsts_namespace;
			public bool exportConsts_actions;
			public string exportConsts_actionsClassName;
			public bool exportConsts_actionsIncludeActionCategory;
			public bool exportConsts_actionsCreateClassesForActionCategories;
			public bool exportConsts_mapCategories;
			public string exportConsts_mapCategoriesClassName;
			public bool exportConsts_layouts;
			public string exportConsts_layoutsClassName;
			public bool exportConsts_players;
			public string exportConsts_playersClassName;
			public bool exportConsts_customControllers;
			public string exportConsts_customControllersClassName;
			public string exportConsts_customControllersAxesClassName;
			public string exportConsts_customControllersButtonsClassName;
			public bool exportConsts_allCapsConstantNames;
		}

		public UpdateLoopSetting updateLoop;
		public bool alwaysUseUnityInput;
		public WindowsStandalonePrimaryInputSource windowsStandalonePrimaryInputSource;
		public OSXStandalonePrimaryInputSource osx_primaryInputSource;
		public LinuxStandalonePrimaryInputSource linux_primaryInputSource;
		public WindowsUWPPrimaryInputSource windowsUWP_primaryInputSource;
		public XboxOnePrimaryInputSource xboxOne_primaryInputSource;
		public PS4PrimaryInputSource ps4_primaryInputSource;
		public WebGLPrimaryInputSource webGL_primaryInputSource;
		public bool useXInput;
		public bool useNativeMouse;
		public bool useEnhancedDeviceSupport;
		public bool windowsStandalone_useSteamRawInputControllerWorkaround;
		public bool osxStandalone_useEnhancedDeviceSupport;
		public bool android_supportUnknownGamepads;
		public bool ps4_assignJoysticksByPS4JoyId;
		public bool useSteamControllerSupport;
		public PlatformVars_WindowsStandalone platformVars_windowsStandalone;
		public PlatformVars platformVars_linuxStandalone;
		public PlatformVars platformVars_osxStandalone;
		public PlatformVars platformVars_windows8Store;
		public PlatformVars_WindowsUWP platformVars_windowsUWP;
		public PlatformVars platformVars_iOS;
		public PlatformVars platformVars_tvOS;
		public PlatformVars platformVars_android;
		public PlatformVars platformVars_ps3;
		public PlatformVars platformVars_ps4;
		public PlatformVars platformVars_psVita;
		public PlatformVars platformVars_xbox360;
		public PlatformVars platformVars_xboxOne;
		public PlatformVars platformVars_wii;
		public PlatformVars platformVars_wiiu;
		public PlatformVars platformVars_switch;
		public PlatformVars platformVars_webGL;
		public int maxJoysticksPerPlayer;
		public bool autoAssignJoysticks;
		public bool assignJoysticksToPlayingPlayersOnly;
		public bool distributeJoysticksEvenly;
		public bool reassignJoystickToPreviousOwnerOnReconnect;
		public DeadZone2DType defaultJoystickAxis2DDeadZoneType;
		public bool force4WayHats;
		public bool deferControllerConnectedEventsOnStart;
		public EditorVars editorSettings;
	}
}
