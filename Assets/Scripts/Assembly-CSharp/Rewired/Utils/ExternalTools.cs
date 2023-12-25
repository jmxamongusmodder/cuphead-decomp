using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Rewired.Utils.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Utils
{
	// Token: 0x02000C5A RID: 3162
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ExternalTools : IExternalTools
	{
		// Token: 0x06004E6A RID: 20074 RVA: 0x0027A10F File Offset: 0x0027850F
		public object GetPlatformInitializer()
		{
			return null;
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x0027A112 File Offset: 0x00278512
		public string GetFocusedEditorWindowTitle()
		{
			return string.Empty;
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x0027A119 File Offset: 0x00278519
		public bool LinuxInput_IsJoystickPreconfigured(string name)
		{
			return false;
		}

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x06004E6D RID: 20077 RVA: 0x0027A11C File Offset: 0x0027851C
		// (remove) Token: 0x06004E6E RID: 20078 RVA: 0x0027A154 File Offset: 0x00278554
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

		// Token: 0x06004E6F RID: 20079 RVA: 0x0027A18A File Offset: 0x0027858A
		public int XboxOneInput_GetUserIdForGamepad(uint id)
		{
			return 0;
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x0027A18D File Offset: 0x0027858D
		public ulong XboxOneInput_GetControllerId(uint unityJoystickId)
		{
			return 0UL;
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x0027A191 File Offset: 0x00278591
		public bool XboxOneInput_IsGamepadActive(uint unityJoystickId)
		{
			return false;
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x0027A194 File Offset: 0x00278594
		public string XboxOneInput_GetControllerType(ulong xboxControllerId)
		{
			return string.Empty;
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x0027A19B File Offset: 0x0027859B
		public uint XboxOneInput_GetJoystickId(ulong xboxControllerId)
		{
			return 0U;
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x0027A19E File Offset: 0x0027859E
		public void XboxOne_Gamepad_UpdatePlugin()
		{
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x0027A1A0 File Offset: 0x002785A0
		public bool XboxOne_Gamepad_SetGamepadVibration(ulong xboxOneJoystickId, float leftMotor, float rightMotor, float leftTriggerLevel, float rightTriggerLevel)
		{
			return false;
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x0027A1A3 File Offset: 0x002785A3
		public void XboxOne_Gamepad_PulseVibrateMotor(ulong xboxOneJoystickId, int motorInt, float startLevel, float endLevel, ulong durationMS)
		{
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x0027A1A5 File Offset: 0x002785A5
		public Vector3 PS4Input_GetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x0027A1AC File Offset: 0x002785AC
		public Vector3 PS4Input_GetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x0027A1B3 File Offset: 0x002785B3
		public Vector4 PS4Input_GetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x0027A1BA File Offset: 0x002785BA
		public void PS4Input_GetLastTouchData(int id, out int touchNum, out int touch0x, out int touch0y, out int touch0id, out int touch1x, out int touch1y, out int touch1id)
		{
			touchNum = 0;
			touch0x = 0;
			touch0y = 0;
			touch0id = 0;
			touch1x = 0;
			touch1y = 0;
			touch1id = 0;
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x0027A1D6 File Offset: 0x002785D6
		public void PS4Input_GetPadControllerInformation(int id, out float touchpixelDensity, out int touchResolutionX, out int touchResolutionY, out int analogDeadZoneLeft, out int analogDeadZoneright, out int connectionType)
		{
			touchpixelDensity = 0f;
			touchResolutionX = 0;
			touchResolutionY = 0;
			analogDeadZoneLeft = 0;
			analogDeadZoneright = 0;
			connectionType = 0;
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x0027A1F2 File Offset: 0x002785F2
		public void PS4Input_PadSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x0027A1F4 File Offset: 0x002785F4
		public void PS4Input_PadSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x0027A1F6 File Offset: 0x002785F6
		public void PS4Input_PadSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x0027A1F8 File Offset: 0x002785F8
		public void PS4Input_PadSetLightBar(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x0027A1FA File Offset: 0x002785FA
		public void PS4Input_PadResetLightBar(int id)
		{
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0027A1FC File Offset: 0x002785FC
		public void PS4Input_PadSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x0027A1FE File Offset: 0x002785FE
		public void PS4Input_PadResetOrientation(int id)
		{
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x0027A200 File Offset: 0x00278600
		public bool PS4Input_PadIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x0027A203 File Offset: 0x00278603
		public object PS4Input_PadGetUsersDetails(int slot)
		{
			return null;
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x0027A206 File Offset: 0x00278606
		public Vector3 PS4Input_GetLastMoveAcceleration(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x0027A20D File Offset: 0x0027860D
		public Vector3 PS4Input_GetLastMoveGyro(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x0027A214 File Offset: 0x00278614
		public int PS4Input_MoveGetButtons(int id, int index)
		{
			return 0;
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x0027A217 File Offset: 0x00278617
		public int PS4Input_MoveGetAnalogButton(int id, int index)
		{
			return 0;
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x0027A21A File Offset: 0x0027861A
		public bool PS4Input_MoveIsConnected(int id, int index)
		{
			return false;
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x0027A21D File Offset: 0x0027861D
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles, int[] secondaryHandles)
		{
			return 0;
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x0027A220 File Offset: 0x00278620
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles)
		{
			return 0;
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x0027A223 File Offset: 0x00278623
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers)
		{
			return 0;
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x0027A226 File Offset: 0x00278626
		public IntPtr PS4Input_MoveGetControllerInputForTracking()
		{
			return IntPtr.Zero;
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x0027A22D File Offset: 0x0027862D
		public void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids)
		{
			vids = new List<int>();
			pids = new List<int>();
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x0027A23D File Offset: 0x0027863D
		public bool UnityUI_Graphic_GetRaycastTarget(object graphic)
		{
			return !(graphic as Graphic == null) && (graphic as Graphic).raycastTarget;
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x0027A25D File Offset: 0x0027865D
		public void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value)
		{
			if (graphic as Graphic == null)
			{
				return;
			}
			(graphic as Graphic).raycastTarget = value;
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06004E91 RID: 20113 RVA: 0x0027A27D File Offset: 0x0027867D
		public bool UnityInput_IsTouchPressureSupported
		{
			get
			{
				return Input.touchPressureSupported;
			}
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x0027A284 File Offset: 0x00278684
		public float UnityInput_GetTouchPressure(ref Touch touch)
		{
			return touch.pressure;
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x0027A28C File Offset: 0x0027868C
		public float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch)
		{
			return touch.maximumPossiblePressure;
		}
	}
}
