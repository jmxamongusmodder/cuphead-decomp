using System;
using System.Collections.Generic;

// Token: 0x02000A4B RID: 2635
public class MeterScoreTracker
{
	// Token: 0x06003EC3 RID: 16067 RVA: 0x0022668B File Offset: 0x00224A8B
	public MeterScoreTracker(MeterScoreTracker.Type type)
	{
		this.type = type;
	}

	// Token: 0x06003EC4 RID: 16068 RVA: 0x0022669A File Offset: 0x00224A9A
	public void Add(DamageDealer damageDealer)
	{
		damageDealer.OnDealDamage += this.OnDealDamage;
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x002266AE File Offset: 0x00224AAE
	public void Add(AbstractProjectile projectile)
	{
		projectile.AddToMeterScoreTracker(this);
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x002266B7 File Offset: 0x00224AB7
	private void OnDealDamage(float damage, DamageReceiver damageReceiver, DamageDealer damageDealer)
	{
		if (!this.alreadyAddedScore)
		{
			Level.ScoringData.superMeterUsed += ((this.type != MeterScoreTracker.Type.Super) ? 1 : 5);
			this.alreadyAddedScore = true;
		}
	}

	// Token: 0x040045CD RID: 17869
	private MeterScoreTracker.Type type;

	// Token: 0x040045CE RID: 17870
	private bool alreadyAddedScore;

	// Token: 0x040045CF RID: 17871
	private List<AbstractProjectile> projectilesToAdd;

	// Token: 0x02000A4C RID: 2636
	public enum Type
	{
		// Token: 0x040045D1 RID: 17873
		Super,
		// Token: 0x040045D2 RID: 17874
		Ex
	}
}
