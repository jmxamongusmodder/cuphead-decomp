using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009CE RID: 2510
public class PlatformHandlingTitleScreenOverride
{
	// Token: 0x06003AF2 RID: 15090 RVA: 0x00212A83 File Offset: 0x00210E83
	public PlatformHandlingTitleScreenOverride(StartScreen.InitialLoadData startScreenLoadData)
	{
		this.startScreenLoadData = startScreenLoadData;
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x00212A94 File Offset: 0x00210E94
	public IEnumerator GetTitleScreenOverrideStatus_cr(MonoBehaviour parent)
	{
		yield return null;
		yield return null;
		this.startScreenLoadData.forceOriginalTitleScreen = SettingsData.Data.forceOriginalTitleScreen;
		yield break;
	}

	// Token: 0x040042AE RID: 17070
	public static readonly string XboxOneForceOriginalTitleScreenKey = "XboxOne_ForceOriginalTitleScreen";

	// Token: 0x040042AF RID: 17071
	public static readonly string UWPForceOriginalTitleScreenKey = "UWP_ForceOriginalTitleScreen";

	// Token: 0x040042B0 RID: 17072
	private StartScreen.InitialLoadData startScreenLoadData;
}
