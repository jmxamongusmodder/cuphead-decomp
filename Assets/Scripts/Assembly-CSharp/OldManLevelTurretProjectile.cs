using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000717 RID: 1815
public class OldManLevelTurretProjectile : BasicProjectile
{
	// Token: 0x0600277E RID: 10110 RVA: 0x0017287C File Offset: 0x00170C7C
	protected override void Start()
	{
		base.Start();
		this.rend.flipX = Rand.Bool();
		base.animator.Play("Projectile", 0, UnityEngine.Random.Range(0f, 1f));
		base.StartCoroutine(this.spawn_sparkles_cr());
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x001728CC File Offset: 0x00170CCC
	private IEnumerator spawn_sparkles_cr()
	{
		this.sparkleAngle = (float)UnityEngine.Random.Range(0, 360);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.sparkleSpawnDelay);
			((OldManLevel)Level.Current).CreateFX(base.transform.position + MathUtils.AngleToDirection(this.sparkleAngle) * this.sparkleDistanceRange.RandomFloat(), true, base.CanParry);
			this.sparkleAngle = (this.sparkleAngle + this.sparkleAngleShiftRange.RandomFloat()) % 360f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x001728E8 File Offset: 0x00170CE8
	protected override void Move()
	{
		if (this.Speed == 0f)
		{
		}
		base.transform.position += base.transform.up * this.Speed * CupheadTime.FixedDelta;
	}

	// Token: 0x0400303D RID: 12349
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x0400303E RID: 12350
	[SerializeField]
	private float sparkleSpawnDelay;

	// Token: 0x0400303F RID: 12351
	[SerializeField]
	private MinMax sparkleAngleShiftRange;

	// Token: 0x04003040 RID: 12352
	[SerializeField]
	private MinMax sparkleDistanceRange;

	// Token: 0x04003041 RID: 12353
	private float sparkleAngle;
}
