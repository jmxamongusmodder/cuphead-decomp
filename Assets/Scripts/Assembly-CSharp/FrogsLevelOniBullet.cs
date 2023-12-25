using System;
using UnityEngine;

// Token: 0x020006B4 RID: 1716
public class FrogsLevelOniBullet : AbstractFrogsLevelSlotBullet
{
	// Token: 0x0600246D RID: 9325 RVA: 0x0015593C File Offset: 0x00153D3C
	public FrogsLevelOniBullet Create(Vector2 pos, float speed, LevelProperties.Frogs.Demon properties)
	{
		FrogsLevelOniBullet frogsLevelOniBullet = base.Create(pos, speed) as FrogsLevelOniBullet;
		frogsLevelOniBullet.properties = properties;
		return frogsLevelOniBullet;
	}

	// Token: 0x0600246E RID: 9326 RVA: 0x0015595F File Offset: 0x00153D5F
	protected override void Start()
	{
		base.Start();
		this.SetSize();
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x00155970 File Offset: 0x00153D70
	private void SetSize()
	{
		this.parryBox.SetScale(null, new float?(this.properties.demonParryHeight), null);
		this.hurtBox.SetScale(null, new float?(this.properties.demonFlameHeight), null);
	}

	// Token: 0x04002D20 RID: 11552
	[SerializeField]
	private Transform parryBox;

	// Token: 0x04002D21 RID: 11553
	[SerializeField]
	private Transform hurtBox;

	// Token: 0x04002D22 RID: 11554
	private LevelProperties.Frogs.Demon properties;
}
