using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008DF RID: 2271
public class MountainPlatformingLevelDragonProjectile : BasicProjectile
{
	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06003527 RID: 13607 RVA: 0x001EEF97 File Offset: 0x001ED397
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06003528 RID: 13608 RVA: 0x001EEFA0 File Offset: 0x001ED3A0
	protected override void Start()
	{
		base.Start();
		MountainPlatformingLevelDragonProjectile.numUntilPink--;
		this.DestroyDistance = -1f;
		if (MountainPlatformingLevelDragonProjectile.numUntilPink <= 0)
		{
			MountainPlatformingLevelDragonProjectile.numUntilPink = EnemyDatabase.GetProperties(EnemyID.dragon).MushroomPinkNumber.RandomInt();
			this.SetParryable(true);
		}
		else
		{
			this.SetParryable(false);
		}
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x06003529 RID: 13609 RVA: 0x001EF010 File Offset: 0x001ED410
	private IEnumerator trail_cr()
	{
		while (!base.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.trailPeriod.RandomFloat());
			Effect effect = this.trailPrefab.Create(this.trailRoot.position + this.trailMaxOffset * MathUtils.RandomPointInUnitCircle());
			effect.animator.Play("PuffA");
		}
		yield break;
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x001EF02B File Offset: 0x001ED42B
	public override void OnParryDie()
	{
		base.OnParryDie();
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x04003D4E RID: 15694
	public static int numUntilPink;

	// Token: 0x04003D4F RID: 15695
	[SerializeField]
	private Effect trailPrefab;

	// Token: 0x04003D50 RID: 15696
	[SerializeField]
	private MinMax trailPeriod;

	// Token: 0x04003D51 RID: 15697
	[SerializeField]
	private float trailMaxOffset;

	// Token: 0x04003D52 RID: 15698
	[SerializeField]
	private Transform trailRoot;
}
