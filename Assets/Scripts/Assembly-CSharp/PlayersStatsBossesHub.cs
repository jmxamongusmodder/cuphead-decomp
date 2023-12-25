using System;

// Token: 0x02000734 RID: 1844
public class PlayersStatsBossesHub
{
	// Token: 0x0600282A RID: 10282 RVA: 0x00176B95 File Offset: 0x00174F95
	public void LoseBonusHP()
	{
		if (this.BonusHP > 0)
		{
			this.BonusHP--;
		}
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x00176BB1 File Offset: 0x00174FB1
	public void LoseHealerHP()
	{
		if (this.healerHP > 0)
		{
			this.healerHP--;
		}
	}

	// Token: 0x040030EA RID: 12522
	public int HP;

	// Token: 0x040030EB RID: 12523
	public int BonusHP;

	// Token: 0x040030EC RID: 12524
	public float SuperCharge;

	// Token: 0x040030ED RID: 12525
	public Weapon basePrimaryWeapon;

	// Token: 0x040030EE RID: 12526
	public Weapon baseSecondaryWeapon;

	// Token: 0x040030EF RID: 12527
	public Super BaseSuper;

	// Token: 0x040030F0 RID: 12528
	public Charm BaseCharm;

	// Token: 0x040030F1 RID: 12529
	public int tokenCount;

	// Token: 0x040030F2 RID: 12530
	public int healerHP;

	// Token: 0x040030F3 RID: 12531
	public int healerHPReceived;

	// Token: 0x040030F4 RID: 12532
	public int healerHPCounter;
}
