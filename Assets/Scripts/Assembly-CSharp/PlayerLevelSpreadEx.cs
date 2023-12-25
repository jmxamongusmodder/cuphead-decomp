using System;
using UnityEngine;

// Token: 0x02000A86 RID: 2694
public class PlayerLevelSpreadEx : AbstractProjectile
{
	// Token: 0x06004065 RID: 16485 RVA: 0x0023156C File Offset: 0x0022F96C
	public void Init(float speed, float damage, int childCount, float radius)
	{
		float num = (float)(360 / childCount);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		for (int i = 0; i < childCount; i++)
		{
			BasicProjectile projectile = this.childPrefab.Create(base.transform.position, num * (float)i, Vector2.one, speed);
			this.childPrefab.Damage = damage;
			this.childPrefab.Speed = speed;
			this.childPrefab.PlayerId = this.PlayerId;
			this.childPrefab.transform.AddPositionForward2D(radius);
			meterScoreTracker.Add(projectile);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004066 RID: 16486 RVA: 0x0023160E File Offset: 0x0022FA0E
	protected override int GetVariants()
	{
		return 1;
	}

	// Token: 0x06004067 RID: 16487 RVA: 0x00231611 File Offset: 0x0022FA11
	protected override void SetBool(string boolean, bool b)
	{
	}

	// Token: 0x06004068 RID: 16488 RVA: 0x00231613 File Offset: 0x0022FA13
	protected override void SetInt(string integer, int i)
	{
	}

	// Token: 0x06004069 RID: 16489 RVA: 0x00231615 File Offset: 0x0022FA15
	protected override void SetTrigger(string trigger)
	{
	}

	// Token: 0x04004734 RID: 18228
	[SerializeField]
	private PlayerLevelSpreadExChild childPrefab;
}
