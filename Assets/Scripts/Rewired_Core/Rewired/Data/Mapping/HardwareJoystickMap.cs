using UnityEngine;
using System;
using Rewired;
using Rewired.Platforms;

namespace Rewired.Data.Mapping
{
	public class HardwareJoystickMap : ScriptableObject
	{
		[Serializable]
		public class CompoundElement
		{
			public CompoundControllerElementType type;
			public int elementIdentifier;
			public int[] componentElementIdentifiers;
		}

		[Serializable]
		public class Platform
		{
			public string description;
		}

		[Serializable]
		public class MatchingCriteria_Base
		{
			public int axisCount;
			public int buttonCount;
			public bool disabled;
			public string tag;
		}

		[Serializable]
		public class Platform_RawOrDirectInput : Platform
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.MatchingCriteria_Base
			{
				public int hatCount;
				public bool productName_useRegex;
				public string[] productName;
				public string[] productGUID;
				public int[] productId;
				public HardwareJoystickMap.Platform_RawOrDirectInput.DeviceType deviceType;
			}

			[Serializable]
			public class Elements_Platform_Base : HardwareJoystickMap.Elements_Base
			{
			}

			[Serializable]
			public class CustomCalculationSourceData
			{
				public int sourceType;
				public int sourceAxis;
				public int sourceButton;
				public int sourceOtherAxis;
				public AxisRange sourceAxisRange;
				public float axisDeadZone;
				public bool invert;
				public AxisCalibrationType axisCalibrationType;
				public float axisZero;
				public float axisMin;
				public float axisMax;
			}

			[Serializable]
			public class Element
			{
				public CustomCalculation customCalculation;
				public HardwareJoystickMap.Platform_RawOrDirectInput.CustomCalculationSourceData[] customCalculationSourceData;
			}

			[Serializable]
			public class Axis_Base : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceAxis;
				public AxisRange sourceAxisRange;
				public bool invert;
				public float axisDeadZone;
				public bool calibrateAxis;
				public float axisZero;
				public float axisMin;
				public float axisMax;
				public HardwareAxisInfo axisInfo;
				public int sourceButton;
				public Pole buttonAxisContribution;
				public int sourceHat;
				public AxisDirection sourceHatDirection;
				public AxisRange sourceHatRange;
			}

			[Serializable]
			public class Button_Base : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceButton;
				public int sourceAxis;
				public Pole sourceAxisPole;
				public float axisDeadZone;
				public int sourceHat;
				public HatType sourceHatType;
				public HatDirection sourceHatDirection;
				public bool requireMultipleButtons;
				public int[] requiredButtons;
				public bool ignoreIfButtonsActive;
				public int[] ignoreIfButtonsActiveButtons;
				public HardwareButtonInfo buttonInfo;
			}

			public enum DeviceType
			{
				Any = 0,
				Device = 17,
				Mouse = 18,
				Keyboard = 19,
				Joystick = 20,
				Gamepad = 21,
				Driving = 22,
				Flight = 23,
				FirstPerson = 24,
				ControlDevice = 25,
				ScreenPointer = 26,
				Remote = 27,
				Supplemental = 28,
			}

			public MatchingCriteria matchingCriteria;
		}

		[Serializable]
		public class Elements_Base
		{
		}

		[Serializable]
		public class Platform_DirectInput_Base : Platform_RawOrDirectInput
		{
			[Serializable]
			public class Axis : HardwareJoystickMap.Platform_RawOrDirectInput.Axis_Base
			{
			}

			[Serializable]
			public class Button : HardwareJoystickMap.Platform_RawOrDirectInput.Button_Base
			{
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Platform_RawOrDirectInput.Elements_Platform_Base
			{
				public HardwareJoystickMap.Platform_DirectInput_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_DirectInput_Base.Button[] buttons;
			}

			public Elements elements;
		}

		[Serializable]
		public class Platform_DirectInput : Platform_DirectInput_Base
		{
			public HardwareJoystickMap.Platform_DirectInput_Base[] variants;
		}

		[Serializable]
		public class Platform_RawInput_Base : Platform_RawOrDirectInput
		{
			[Serializable]
			public class Axis : HardwareJoystickMap.Platform_RawOrDirectInput.Axis_Base
			{
				public int sourceOtherAxis;
			}

			[Serializable]
			public class Button : HardwareJoystickMap.Platform_RawOrDirectInput.Button_Base
			{
				public int sourceOtherAxis;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Platform_RawOrDirectInput.Elements_Platform_Base
			{
				public HardwareJoystickMap.Platform_RawInput_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_RawInput_Base.Button[] buttons;
			}

			public Elements elements;
		}

		[Serializable]
		public class Platform_RawInput : Platform_RawInput_Base
		{
			public HardwareJoystickMap.Platform_RawInput_Base[] variants;
		}

		[Serializable]
		public class Platform_XInput_Base : Platform
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.MatchingCriteria_Base
			{
				public HardwareJoystickMap.Platform_XInput_Base.SubType[] subType;
			}

			[Serializable]
			public class Element
			{
				public int elementIdentifier;
				public HardwareElementSourceType sourceType;
				public XInputButton sourceButton;
				public XInputAxis sourceAxis;
				public float axisDeadZone;
			}

			[Serializable]
			public class Axis : Element
			{
				public bool invert;
				public Pole buttonAxisContribution;
				public AxisRange sourceAxisRange;
				public bool calibrateAxis;
				public float axisZero;
				public float axisMin;
				public float axisMax;
				public HardwareAxisInfo axisInfo;
			}

			[Serializable]
			public class Button : Element
			{
				public Pole sourceAxisPole;
				public HardwareButtonInfo buttonInfo;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Elements_Base
			{
				public HardwareJoystickMap.Platform_XInput_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_XInput_Base.Button[] buttons;
			}

			public enum SubType
			{
				None = 0,
				Gamepad = 1,
				Wheel = 2,
				ArcadeStick = 3,
				FlightSick = 4,
				DancePad = 5,
				Guitar = 6,
				DrumKit = 8,
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_XInput : Platform_XInput_Base
		{
			public HardwareJoystickMap.Platform_XInput_Base[] variants;
		}

		[Serializable]
		public class Platform_OSX_Base : Platform
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.MatchingCriteria_Base
			{
				public int hatCount;
				public bool productName_useRegex;
				public string[] productName;
				public string[] manufacturer;
				public int[] productId;
				public int[] vendorId;
			}

			[Serializable]
			public class Element
			{
			}

			[Serializable]
			public class Axis : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceStick;
				public OSXAxis sourceAxis;
				public int sourceOtherAxis;
				public AxisRange sourceAxisRange;
				public bool invert;
				public float axisDeadZone;
				public bool calibrateAxis;
				public float axisZero;
				public float axisMin;
				public float axisMax;
				public HardwareAxisInfo axisInfo;
				public int sourceButton;
				public Pole buttonAxisContribution;
				public int sourceHat;
				public AxisDirection sourceHatDirection;
				public AxisRange sourceHatRange;
			}

			[Serializable]
			public class Button : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceButton;
				public int sourceStick;
				public OSXAxis sourceAxis;
				public int sourceOtherAxis;
				public Pole sourceAxisPole;
				public float axisDeadZone;
				public int sourceHat;
				public HatType sourceHatType;
				public HatDirection sourceHatDirection;
				public bool requireMultipleButtons;
				public int[] requiredButtons;
				public bool ignoreIfButtonsActive;
				public int[] ignoreIfButtonsActiveButtons;
				public HardwareButtonInfo buttonInfo;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Elements_Base
			{
				public HardwareJoystickMap.Platform_OSX_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_OSX_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_OSX : Platform_OSX_Base
		{
			public HardwareJoystickMap.Platform_OSX_Base[] variants;
		}

		[Serializable]
		public class Platform_Linux_Base : Platform
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.MatchingCriteria_Base
			{
				public int hatCount;
				public bool manufacturer_useRegex;
				public bool productName_useRegex;
				public bool systemName_useRegex;
				public string[] manufacturer;
				public string[] productName;
				public string[] systemName;
				public string[] productGUID;
			}

			[Serializable]
			public class Element
			{
			}

			[Serializable]
			public class Axis : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceAxis;
				public AxisRange sourceAxisRange;
				public bool invert;
				public float axisDeadZone;
				public bool calibrateAxis;
				public float axisZero;
				public float axisMin;
				public float axisMax;
				public HardwareAxisInfo axisInfo;
				public int sourceButton;
				public Pole buttonAxisContribution;
				public int sourceHat;
				public AxisDirection sourceHatDirection;
				public AxisRange sourceHatRange;
			}

			[Serializable]
			public class Button : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceButton;
				public int sourceAxis;
				public Pole sourceAxisPole;
				public float axisDeadZone;
				public int sourceHat;
				public HatType sourceHatType;
				public HatDirection sourceHatDirection;
				public bool requireMultipleButtons;
				public int[] requiredButtons;
				public bool ignoreIfButtonsActive;
				public int[] ignoreIfButtonsActiveButtons;
				public HardwareButtonInfo buttonInfo;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Elements_Base
			{
				public HardwareJoystickMap.Platform_Linux_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_Linux_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_Linux : Platform_Linux_Base
		{
			public HardwareJoystickMap.Platform_Linux_Base[] variants;
		}

		[Serializable]
		public class Platform_WindowsUWP_Base : Platform
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.MatchingCriteria_Base
			{
				public int hatCount;
				public bool manufacturer_useRegex;
				public bool productName_useRegex;
				public string[] manufacturer;
				public string[] productName;
				public string[] productGUID;
			}

			[Serializable]
			public class Element
			{
			}

			[Serializable]
			public class Axis : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceAxis;
				public AxisRange sourceAxisRange;
				public bool invert;
				public float axisDeadZone;
				public bool calibrateAxis;
				public float axisZero;
				public float axisMin;
				public float axisMax;
				public HardwareAxisInfo axisInfo;
				public int sourceButton;
				public Pole buttonAxisContribution;
				public int sourceHat;
				public AxisDirection sourceHatDirection;
				public AxisRange sourceHatRange;
			}

			[Serializable]
			public class Button : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceButton;
				public int sourceAxis;
				public Pole sourceAxisPole;
				public float axisDeadZone;
				public int sourceHat;
				public HatType sourceHatType;
				public HatDirection sourceHatDirection;
				public bool requireMultipleButtons;
				public int[] requiredButtons;
				public bool ignoreIfButtonsActive;
				public int[] ignoreIfButtonsActiveButtons;
				public HardwareButtonInfo buttonInfo;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Elements_Base
			{
				public HardwareJoystickMap.Platform_WindowsUWP_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_WindowsUWP_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_WindowsUWP : Platform_WindowsUWP_Base
		{
			public HardwareJoystickMap.Platform_WindowsUWP_Base[] variants;
		}

		[Serializable]
		public class Platform_Fallback_Base : Platform
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.MatchingCriteria_Base
			{
				public bool alwaysMatch;
				public bool productName_useRegex;
				public string[] productName;
				public bool matchUnityVersion;
				public string matchUnityVersion_min;
				public string matchUnityVersion_max;
			}

			[Serializable]
			public class CustomCalculationSourceData
			{
				public int sourceType;
				public int sourceElement;
				public AxisRange sourceAxisRange;
				public float deadzone;
				public bool invert;
			}

			[Serializable]
			public class Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public UnityAxis sourceAxis;
				public float axisDeadZone;
				public UnityButton sourceButton;
				public KeyCode sourceKeyCode;
				public CustomCalculation customCalculation;
				public HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData[] customCalculationSourceData;
			}

			[Serializable]
			public class Axis : Element
			{
				public bool invert;
				public AxisRange sourceAxisRange;
				public Pole buttonAxisContribution;
				public bool calibrateAxis;
				public float axisZero;
				public float axisMin;
				public float axisMax;
				public HardwareAxisInfo axisInfo;
			}

			[Serializable]
			public class Button : Element
			{
				public Pole sourceAxisPole;
				public UnityAxis unityHat_sourceAxis1;
				public UnityAxis unityHat_sourceAxis2;
				public Vector2 unityHat_isActiveAxisValues1;
				public Vector2 unityHat_isActiveAxisValues2;
				public Vector2 unityHat_isActiveAxisValues3;
				public Vector2 unityHat_zeroValues;
				public bool unityHat_checkNeverPressed;
				public Vector2 unityHat_neverPressedZeroValues;
				public bool requireMultipleButtons;
				public UnityButton[] requiredButtons;
				public bool ignoreIfButtonsActive;
				public UnityButton[] ignoreIfButtonsActiveButtons;
				public HardwareButtonInfo buttonInfo;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Elements_Base
			{
				public HardwareJoystickMap.Platform_Fallback_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_Fallback_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_Fallback : Platform_Fallback_Base
		{
			public HardwareJoystickMap.Platform_Fallback_Base[] variants;
		}

		[Serializable]
		public class Platform_Custom : Platform
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.MatchingCriteria_Base
			{
				public bool alwaysMatch;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Elements_Base
			{
			}

			[Serializable]
			public class CustomCalculationSourceData
			{
				public int sourceType;
				public int sourceAxis;
				public int sourceButton;
				public int sourceOtherAxis;
				public AxisRange sourceAxisRange;
				public float axisDeadZone;
				public bool invert;
				public AxisCalibrationType axisCalibrationType;
				public float axisZero;
				public float axisMin;
				public float axisMax;
			}

			[Serializable]
			public class Element
			{
				public int elementIdentifier;
				public int sourceType;
				public int sourceAxis;
				public float axisDeadZone;
				public int sourceButton;
				public CustomCalculation customCalculation;
				public HardwareJoystickMap.Platform_Custom.CustomCalculationSourceData[] customCalculationSourceData;
			}

			[Serializable]
			public class Axis : Element
			{
				public bool invert;
				public AxisRange sourceAxisRange;
				public Pole buttonAxisContribution;
				public bool calibrateAxis;
				public float axisZero;
				public float axisMin;
				public float axisMax;
				public HardwareAxisInfo axisInfo;
			}

			[Serializable]
			public class Button : Element
			{
				public Pole sourceAxisPole;
				public bool requireMultipleButtons;
				public int[] requiredButtons;
				public bool ignoreIfButtonsActive;
				public int[] ignoreIfButtonsActiveButtons;
				public HardwareButtonInfo buttonInfo;
			}

		}

		[Serializable]
		public class Platform_WebGL_Base : Platform_Custom
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.Platform_Custom.MatchingCriteria
			{
				[Serializable]
				public class ElementCount
				{
					public int axisCount;
					public int buttonCount;
				}

				[Serializable]
				public class ClientInfo
				{
					public int browser;
					public string browserVersionMin;
					public string browserVersionMax;
					public int os;
					public string osVersionMin;
					public string osVersionMax;
				}

				public bool productName_useRegex;
				public string[] productName;
				public string[] productGUID;
				public int[] mapping;
				public ElementCount[] elementCount;
				public ClientInfo[] clientInfo;
			}

			[Serializable]
			public class Axis : HardwareJoystickMap.Platform_Custom.Axis
			{
			}

			[Serializable]
			public class Button : HardwareJoystickMap.Platform_Custom.Button
			{
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Platform_Custom.Elements
			{
				public HardwareJoystickMap.Platform_WebGL_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_WebGL_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_WebGL : Platform_WebGL_Base
		{
			public HardwareJoystickMap.Platform_WebGL_Base[] variants;
		}

		[Serializable]
		public class Platform_Ouya_Base : Platform_Custom
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.Platform_Custom.MatchingCriteria
			{
			}

			[Serializable]
			public class Axis : HardwareJoystickMap.Platform_Custom.Axis
			{
			}

			[Serializable]
			public class Button : HardwareJoystickMap.Platform_Custom.Button
			{
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Platform_Custom.Elements
			{
				public HardwareJoystickMap.Platform_Ouya_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_Ouya_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_Ouya : Platform_Ouya_Base
		{
			public HardwareJoystickMap.Platform_Ouya_Base[] variants;
		}

		[Serializable]
		public class Platform_XboxOne_Base : Platform_Custom
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.Platform_Custom.MatchingCriteria
			{
				public bool productName_useRegex;
				public string[] productName;
			}

			[Serializable]
			public class Axis : HardwareJoystickMap.Platform_Custom.Axis
			{
			}

			[Serializable]
			public class Button : HardwareJoystickMap.Platform_Custom.Button
			{
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Platform_Custom.Elements
			{
				public HardwareJoystickMap.Platform_XboxOne_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_XboxOne_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_XboxOne : Platform_XboxOne_Base
		{
			public HardwareJoystickMap.Platform_XboxOne_Base[] variants;
		}

		[Serializable]
		public class Platform_PS4_Base : Platform_Custom
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.Platform_Custom.MatchingCriteria
			{
				public bool productName_useRegex;
				public string[] productName;
			}

			[Serializable]
			public class Axis : HardwareJoystickMap.Platform_Custom.Axis
			{
			}

			[Serializable]
			public class Button : HardwareJoystickMap.Platform_Custom.Button
			{
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Platform_Custom.Elements
			{
				public HardwareJoystickMap.Platform_PS4_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_PS4_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_PS4 : Platform_PS4_Base
		{
			public HardwareJoystickMap.Platform_PS4_Base[] variants;
		}

		[Serializable]
		public class Platform_NintendoSwitch_Base : Platform_Custom
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.Platform_Custom.MatchingCriteria
			{
				public bool productName_useRegex;
				public string[] productName;
			}

			[Serializable]
			public class Axis : HardwareJoystickMap.Platform_Custom.Axis
			{
			}

			[Serializable]
			public class Button : HardwareJoystickMap.Platform_Custom.Button
			{
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Platform_Custom.Elements
			{
				public HardwareJoystickMap.Platform_NintendoSwitch_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_NintendoSwitch_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_NintendoSwitch : Platform_NintendoSwitch_Base
		{
			public HardwareJoystickMap.Platform_NintendoSwitch_Base[] variants;
		}

		[Serializable]
		public class VidPid
		{
			public int vendorId;
			public int productId;
		}

		[Serializable]
		public class Platform_InternalDriver_Base : Platform_Custom
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.Platform_Custom.MatchingCriteria
			{
				public bool productName_useRegex;
				public string[] productName;
				public HardwareJoystickMap.VidPid[] vidPid;
				public int hatCount;
			}

			[Serializable]
			public class Axis : HardwareJoystickMap.Platform_Custom.Axis
			{
				public int sourceHat;
				public AxisDirection sourceHatDirection;
				public HatType sourceHatType;
				public AxisRange sourceHatRange;
			}

			[Serializable]
			public class Button : HardwareJoystickMap.Platform_Custom.Button
			{
				public int sourceHat;
				public HatDirection sourceHatDirection;
				public HatType sourceHatType;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Platform_Custom.Elements
			{
				public HardwareJoystickMap.Platform_InternalDriver_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_InternalDriver_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_InternalDriver : Platform_InternalDriver_Base
		{
			public HardwareJoystickMap.Platform_InternalDriver_Base[] variants;
		}

		[Serializable]
		public class Platform_SDL2_Base : Platform
		{
			[Serializable]
			public class MatchingCriteria : HardwareJoystickMap.MatchingCriteria_Base
			{
				public int hatCount;
				public bool manufacturer_useRegex;
				public bool productName_useRegex;
				public bool systemName_useRegex;
				public string[] manufacturer;
				public string[] productName;
				public string[] systemName;
				public string[] productGUID;
			}

			[Serializable]
			public class Element
			{
			}

			[Serializable]
			public class Axis : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceAxis;
				public AxisRange sourceAxisRange;
				public bool invert;
				public float axisDeadZone;
				public bool calibrateAxis;
				public float axisZero;
				public float axisMin;
				public float axisMax;
				public HardwareAxisInfo axisInfo;
				public int sourceButton;
				public Pole buttonAxisContribution;
				public int sourceHat;
				public AxisDirection sourceHatDirection;
				public AxisRange sourceHatRange;
			}

			[Serializable]
			public class Button : Element
			{
				public int elementIdentifier;
				public HardwareElementSourceTypeWithHat sourceType;
				public int sourceButton;
				public int sourceAxis;
				public Pole sourceAxisPole;
				public float axisDeadZone;
				public int sourceHat;
				public HatType sourceHatType;
				public HatDirection sourceHatDirection;
				public bool requireMultipleButtons;
				public int[] requiredButtons;
				public bool ignoreIfButtonsActive;
				public int[] ignoreIfButtonsActiveButtons;
				public HardwareButtonInfo buttonInfo;
			}

			[Serializable]
			public class Elements : HardwareJoystickMap.Elements_Base
			{
				public HardwareJoystickMap.Platform_SDL2_Base.Axis[] axes;
				public HardwareJoystickMap.Platform_SDL2_Base.Button[] buttons;
			}

			public MatchingCriteria matchingCriteria;
			public Elements elements;
		}

		[Serializable]
		public class Platform_SDL2 : Platform_SDL2_Base
		{
			public HardwareJoystickMap.Platform_SDL2_Base[] variants;
		}

		[SerializeField]
		private string controllerName;
		[SerializeField]
		private string editorControllerName;
		[SerializeField]
		private string description;
		[SerializeField]
		private string controllerGuid;
		[SerializeField]
		private string[] templateGuids;
		[SerializeField]
		private bool hideInLists;
		[SerializeField]
		private JoystickType[] joystickTypes;
		[SerializeField]
		private ControllerElementIdentifier[] elementIdentifiers;
		[SerializeField]
		private CompoundElement[] compoundElements;
		[SerializeField]
		private Platform_DirectInput directInput;
		[SerializeField]
		private Platform_RawInput rawInput;
		[SerializeField]
		private Platform_XInput xInput;
		[SerializeField]
		private Platform_OSX osx;
		[SerializeField]
		private Platform_Linux linux;
		[SerializeField]
		private Platform_WindowsUWP windowsUWP;
		[SerializeField]
		private Platform_Fallback fallback_Windows;
		[SerializeField]
		private Platform_Fallback fallback_WindowsUWP;
		[SerializeField]
		private Platform_Fallback fallback_OSX;
		[SerializeField]
		private Platform_Fallback fallback_Linux;
		[SerializeField]
		private Platform_Fallback fallback_Linux_PreConfigured;
		[SerializeField]
		private Platform_Fallback fallback_Android;
		[SerializeField]
		private Platform_Fallback fallback_iOS;
		[SerializeField]
		private Platform_Fallback fallback_Blackberry;
		[SerializeField]
		private Platform_Fallback fallback_WindowsPhone8;
		[SerializeField]
		private Platform_Fallback fallback_XBox360;
		[SerializeField]
		private Platform_Fallback fallback_XBoxOne;
		[SerializeField]
		private Platform_Fallback fallback_PS3;
		[SerializeField]
		private Platform_Fallback fallback_PS4;
		[SerializeField]
		private Platform_Fallback fallback_PSM;
		[SerializeField]
		private Platform_Fallback fallback_PSVita;
		[SerializeField]
		private Platform_Fallback fallback_Wii;
		[SerializeField]
		private Platform_Fallback fallback_WiiU;
		[SerializeField]
		private Platform_Fallback fallback_AmazonFireTV;
		[SerializeField]
		private Platform_Fallback fallback_RazerForgeTV;
		[SerializeField]
		private Platform_WebGL webGL;
		[SerializeField]
		private Platform_Ouya ouya;
		[SerializeField]
		private Platform_XboxOne xboxOne;
		[SerializeField]
		private Platform_PS4 ps4;
		[SerializeField]
		private Platform_NintendoSwitch nintendoSwitch;
		[SerializeField]
		private Platform_InternalDriver internalDriver;
		[SerializeField]
		private Platform_SDL2 sdl2_Linux;
		[SerializeField]
		private Platform_SDL2 sdl2_Windows;
		[SerializeField]
		private Platform_SDL2 sdl2_OSX;
		[SerializeField]
		private int elementIdentifierIdCounter;
	}
}
