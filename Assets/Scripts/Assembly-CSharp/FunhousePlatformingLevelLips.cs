using System;
using UnityEngine;

// Token: 0x020008BA RID: 2234
public class FunhousePlatformingLevelLips : BasicProjectile
{
	// Token: 0x06003420 RID: 13344 RVA: 0x001E443A File Offset: 0x001E283A
	protected override void Awake()
	{
		base.Awake();
		PlatformingLevelExit.OnWinStartEvent += this.OnWin;
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x001E4453 File Offset: 0x001E2853
	private void Kiss()
	{
		AudioManager.Play("funhouse_honkbullet_kiss");
		this.emitAudioFromObject.Add("funhouse_honkbullet_kiss");
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x001E446F File Offset: 0x001E286F
	protected override void OnDestroy()
	{
		PlatformingLevelExit.OnWinStartEvent -= this.OnWin;
		base.OnDestroy();
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x001E4488 File Offset: 0x001E2888
	private void OnWin()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
