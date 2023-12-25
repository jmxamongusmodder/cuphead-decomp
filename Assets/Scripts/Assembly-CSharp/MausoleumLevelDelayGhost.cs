using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006D6 RID: 1750
public class MausoleumLevelDelayGhost : MausoleumLevelGhostBase
{
	// Token: 0x06002542 RID: 9538 RVA: 0x0015D460 File Offset: 0x0015B860
	public MausoleumLevelDelayGhost Create(Vector2 position, float rotation, float speed, LevelProperties.Mausoleum.DelayGhost properties)
	{
		MausoleumLevelDelayGhost mausoleumLevelDelayGhost = base.Create(position, rotation, speed) as MausoleumLevelDelayGhost;
		mausoleumLevelDelayGhost.properties = properties;
		return mausoleumLevelDelayGhost;
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x0015D485 File Offset: 0x0015B885
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.wait_cr());
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x0015D49C File Offset: 0x0015B89C
	private IEnumerator wait_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.dashDelay);
		this.Speed = this.properties.speed;
		yield return null;
		yield break;
	}

	// Token: 0x04002DE2 RID: 11746
	private LevelProperties.Mausoleum.DelayGhost properties;
}
