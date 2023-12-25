using System;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class LevelAudio : AbstractMonoBehaviour
{
	// Token: 0x06001338 RID: 4920 RVA: 0x000AA30C File Offset: 0x000A870C
	public static LevelAudio Create()
	{
		return UnityEngine.Object.Instantiate<LevelAudio>(Level.Current.LevelResources.levelAudio);
	}
}
