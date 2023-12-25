using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000887 RID: 2183
public class TreePlatformingLevelBeetleSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x060032BB RID: 12987 RVA: 0x001D7714 File Offset: 0x001D5B14
	protected override void Awake()
	{
		base.Awake();
		this.beetles = new TreePlatformingLevelBeetle[base.GetComponentsInChildren<TreePlatformingLevelBeetle>().Length];
		this.beetles = base.GetComponentsInChildren<TreePlatformingLevelBeetle>();
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x001D773B File Offset: 0x001D5B3B
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.check_to_play_sfx());
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x001D7750 File Offset: 0x001D5B50
	protected override void Spawn()
	{
		base.Spawn();
		this.Activate();
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x001D7760 File Offset: 0x001D5B60
	private void Activate()
	{
		int num = UnityEngine.Random.Range(0, this.beetles.Length);
		if (!this.beetles[num].isActivated)
		{
			this.beetles[num].Activate();
		}
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x001D779C File Offset: 0x001D5B9C
	private IEnumerator check_to_play_sfx()
	{
		for (;;)
		{
			foreach (TreePlatformingLevelBeetle treePlatformingLevelBeetle in this.beetles)
			{
				if (treePlatformingLevelBeetle.onCamera)
				{
					treePlatformingLevelBeetle.PlayIdleSFX();
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003AEC RID: 15084
	private TreePlatformingLevelBeetle[] beetles;
}
