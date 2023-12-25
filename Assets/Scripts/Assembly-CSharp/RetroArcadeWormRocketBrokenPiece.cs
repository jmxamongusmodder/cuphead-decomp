using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200076D RID: 1901
public class RetroArcadeWormRocketBrokenPiece : BasicProjectile
{
	// Token: 0x06002958 RID: 10584 RVA: 0x00181C18 File Offset: 0x00180018
	protected override void Awake()
	{
		base.Awake();
		this.Damage = PlayerManager.DamageMultiplier;
		base.StartCoroutine(this.turnOnCollider_cr());
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x00181C38 File Offset: 0x00180038
	private IEnumerator turnOnCollider_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x00181C53 File Offset: 0x00180053
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
