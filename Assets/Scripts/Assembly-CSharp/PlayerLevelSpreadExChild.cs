using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A87 RID: 2695
public class PlayerLevelSpreadExChild : BasicProjectile
{
	// Token: 0x0600406B RID: 16491 RVA: 0x0023161F File Offset: 0x0022FA1F
	protected override void Start()
	{
		base.Start();
		this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Ex);
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x0600406C RID: 16492 RVA: 0x00231640 File Offset: 0x0022FA40
	private IEnumerator trail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.15f);
			Transform t = this.trailEffectPrefab.Create(base.transform.position).transform;
			t.SetParent(base.transform);
			t.ResetLocalTransforms();
			t.AddPositionForward2D(100f);
			t.SetParent(null);
		}
		yield break;
	}

	// Token: 0x0600406D RID: 16493 RVA: 0x0023165B File Offset: 0x0022FA5B
	private void _OnDieAnimComplete()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004735 RID: 18229
	private const float TRAIL_TIME = 0.15f;

	// Token: 0x04004736 RID: 18230
	[SerializeField]
	private Effect trailEffectPrefab;
}
