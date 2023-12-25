using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000884 RID: 2180
public class ForestPlatformingLevelMushroomProjectile : BasicProjectile
{
	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x0600329C RID: 12956 RVA: 0x001D6987 File Offset: 0x001D4D87
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x001D6990 File Offset: 0x001D4D90
	protected override void Start()
	{
		base.Start();
		ForestPlatformingLevelMushroomProjectile.numUntilPink--;
		this.DestroyDistance = -1f;
		if (ForestPlatformingLevelMushroomProjectile.numUntilPink == 0)
		{
			ForestPlatformingLevelMushroomProjectile.numUntilPink = EnemyDatabase.GetProperties(EnemyID.mushroom).MushroomPinkNumber.RandomInt();
			this.SetInt(AbstractProjectile.Variant, 1);
			this.SetParryable(true);
		}
		else
		{
			this.SetInt(AbstractProjectile.Variant, 0);
			this.SetParryable(false);
		}
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x001D6A18 File Offset: 0x001D4E18
	private IEnumerator trail_cr()
	{
		while (!base.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.trailPeriod.RandomFloat());
			this.trailPrefab.Create(this.trailRoot.position + this.trailMaxOffset * MathUtils.RandomPointInUnitCircle());
		}
		yield break;
	}

	// Token: 0x0600329F RID: 12959 RVA: 0x001D6A33 File Offset: 0x001D4E33
	public override void OnParryDie()
	{
		base.OnParryDie();
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x04003AE0 RID: 15072
	public static int numUntilPink;

	// Token: 0x04003AE1 RID: 15073
	[SerializeField]
	private Effect trailPrefab;

	// Token: 0x04003AE2 RID: 15074
	[SerializeField]
	private MinMax trailPeriod;

	// Token: 0x04003AE3 RID: 15075
	[SerializeField]
	private float trailMaxOffset;

	// Token: 0x04003AE4 RID: 15076
	[SerializeField]
	private Transform trailRoot;
}
