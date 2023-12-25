using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A7A RID: 2682
public class WeaponFirecrackerEXProjectile : BasicProjectile
{
	// Token: 0x06004019 RID: 16409 RVA: 0x0022F36B File Offset: 0x0022D76B
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x0600401A RID: 16410 RVA: 0x0022F373 File Offset: 0x0022D773
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			base.StartCoroutine(this.explosion_cr());
		}
	}

	// Token: 0x0600401B RID: 16411 RVA: 0x0022F390 File Offset: 0x0022D790
	private IEnumerator explosion_cr()
	{
		this.move = false;
		base.transform.SetScale(new float?(this.explosionSize), new float?(this.explosionSize), null);
		yield return CupheadTime.WaitForSeconds(this, this.explosionDuration);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040046DD RID: 18141
	public float bulletLife;

	// Token: 0x040046DE RID: 18142
	public float explosionSize;

	// Token: 0x040046DF RID: 18143
	public float explosionDuration;
}
