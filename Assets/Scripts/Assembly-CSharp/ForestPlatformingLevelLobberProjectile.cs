using System;
using UnityEngine;

// Token: 0x02000881 RID: 2177
public class ForestPlatformingLevelLobberProjectile : BasicProjectile
{
	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x0600328A RID: 12938 RVA: 0x001D66D9 File Offset: 0x001D4AD9
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600328B RID: 12939 RVA: 0x001D66DC File Offset: 0x001D4ADC
	protected override void Awake()
	{
		base.Awake();
		base.transform.SetScale(new float?((float)((!MathUtils.RandomBool()) ? -1 : 1)), null, null);
		base.animator.Play("A", 0, UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x001D6744 File Offset: 0x001D4B44
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (component == null || (!component.canFallThrough && Mathf.Abs(this._accumulativeGravity) > base.transform.right.y * this.Speed))
		{
			this.explode();
		}
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x001D67A8 File Offset: 0x001D4BA8
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (component != null && !component.canFallThrough && Mathf.Abs(this._accumulativeGravity) > base.transform.right.y * this.Speed)
		{
			this.explode();
		}
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x001D680B File Offset: 0x001D4C0B
	private void explode()
	{
		if (!base.dead)
		{
			this.explosionPrefab.Create(base.transform.position);
			this.Die();
		}
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x001D6835 File Offset: 0x001D4C35
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x001D6848 File Offset: 0x001D4C48
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.explosionPrefab = null;
	}

	// Token: 0x04003ADE RID: 15070
	[SerializeField]
	private ForestPlatformingLevelLobberProjectileExplosion explosionPrefab;
}
