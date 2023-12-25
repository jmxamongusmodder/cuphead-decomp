using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired
{
	// Token: 0x02000C59 RID: 3161
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InputManager : InputManager_Base
	{
		// Token: 0x06004E65 RID: 20069 RVA: 0x0027A090 File Offset: 0x00278490
		protected override void DetectPlatform()
		{
			this.editorPlatform = EditorPlatform.None;
			this.platform = Platform.Unknown;
			this.webplayerPlatform = WebplayerPlatform.None;
			this.isEditor = false;
			string text = SystemInfo.deviceName ?? string.Empty;
			string text2 = SystemInfo.deviceModel ?? string.Empty;
			this.platform = Platform.Windows;
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x0027A0E4 File Offset: 0x002784E4
		protected override void CheckRecompile()
		{
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x0027A0E6 File Offset: 0x002784E6
		protected override IExternalTools GetExternalTools()
		{
			return new ExternalTools();
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x0027A0ED File Offset: 0x002784ED
		private bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
		{
			return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
		}
	}
}
