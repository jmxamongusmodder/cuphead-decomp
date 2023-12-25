using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000687 RID: 1671
public class FlyingMermaidLevelFish : AbstractProjectile
{
	// Token: 0x0600233A RID: 9018 RVA: 0x0014B049 File Offset: 0x00149449
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x0014B051 File Offset: 0x00149451
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x0014B06F File Offset: 0x0014946F
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x0014B088 File Offset: 0x00149488
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x0014B0A8 File Offset: 0x001494A8
	public FlyingMermaidLevelFish Create(Vector2 pos, LevelProperties.FlyingMermaid.Fish properties)
	{
		FlyingMermaidLevelFish flyingMermaidLevelFish = base.Create() as FlyingMermaidLevelFish;
		flyingMermaidLevelFish.properties = properties;
		flyingMermaidLevelFish.transform.position = pos;
		flyingMermaidLevelFish.Init();
		return flyingMermaidLevelFish;
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x0014B0E0 File Offset: 0x001494E0
	private void Init()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x0014B0F0 File Offset: 0x001494F0
	private IEnumerator loop_cr()
	{
		float velocityY = this.properties.flyingUpSpeed;
		for (;;)
		{
			base.transform.AddPosition(-this.properties.flyingSpeed * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0f);
			velocityY -= this.properties.flyingGravity * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002BE7 RID: 11239
	private LevelProperties.FlyingMermaid.Fish properties;
}
