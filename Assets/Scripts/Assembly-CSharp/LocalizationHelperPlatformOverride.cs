using System;
using UnityEngine;

// Token: 0x02000921 RID: 2337
public class LocalizationHelperPlatformOverride : MonoBehaviour
{
	// Token: 0x060036A7 RID: 13991 RVA: 0x001FA094 File Offset: 0x001F8494
	public bool HasOverrideForCurrentPlatform(out int newID)
	{
		RuntimePlatform platform = Application.platform;
		for (int i = 0; i < this.overrides.Length; i++)
		{
			LocalizationHelperPlatformOverride.OverrideInfo overrideInfo = this.overrides[i];
			if (overrideInfo.platform == platform)
			{
				newID = overrideInfo.id;
				return true;
			}
		}
		newID = -1;
		return false;
	}

	// Token: 0x04003ED9 RID: 16089
	public LocalizationHelperPlatformOverride.OverrideInfo[] overrides;

	// Token: 0x02000922 RID: 2338
	[Serializable]
	public class OverrideInfo
	{
		// Token: 0x04003EDA RID: 16090
		public RuntimePlatform platform;

		// Token: 0x04003EDB RID: 16091
		public int id;
	}
}
