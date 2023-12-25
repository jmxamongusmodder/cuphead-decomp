using System;

// Token: 0x020007A3 RID: 1955
public class SallyStagePlayHusbandExplosion : LevelBossDeathExploder
{
	// Token: 0x06002BCB RID: 11211 RVA: 0x001992A6 File Offset: 0x001976A6
	protected override void Start()
	{
		this.effectPrefab = Level.Current.LevelResources.levelBossDeathExplosion;
	}
}
