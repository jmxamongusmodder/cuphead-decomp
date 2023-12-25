using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007F1 RID: 2033
public class SnowCultLevelPlatformManager : AbstractCollidableObject
{
	// Token: 0x06002EAA RID: 11946 RVA: 0x001B87B3 File Offset: 0x001B6BB3
	private void Start()
	{
		this.platforms = new List<SnowCultLevelPlatform>();
		this.InstantiatePlatforms();
	}

	// Token: 0x06002EAB RID: 11947 RVA: 0x001B87C8 File Offset: 0x001B6BC8
	private void InstantiatePlatforms()
	{
		for (int i = 0; i < 20; i++)
		{
			SnowCultLevelPlatform snowCultLevelPlatform = UnityEngine.Object.Instantiate<SnowCultLevelPlatform>(this.platformPrefab);
			snowCultLevelPlatform.gameObject.SetActive(false);
			snowCultLevelPlatform.transform.parent = base.transform;
			this.platforms.Add(snowCultLevelPlatform);
		}
	}

	// Token: 0x0400374C RID: 14156
	private const int NUM_OF_PLATFORMS = 20;

	// Token: 0x0400374D RID: 14157
	[SerializeField]
	private SnowCultLevelPlatform platformPrefab;

	// Token: 0x0400374E RID: 14158
	private List<SnowCultLevelPlatform> platforms;
}
