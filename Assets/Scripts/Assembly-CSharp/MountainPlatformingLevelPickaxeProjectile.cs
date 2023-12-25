using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008EA RID: 2282
public class MountainPlatformingLevelPickaxeProjectile : AbstractProjectile
{
	// Token: 0x06003584 RID: 13700 RVA: 0x001F2D66 File Offset: 0x001F1166
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.throw_pickaxe_cr());
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x001F2D7B File Offset: 0x001F117B
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x001F2D9C File Offset: 0x001F119C
	public MountainPlatformingLevelPickaxeProjectile Create(Vector2 pos, float rotation, float speed, MountainPlatformingLevelMiner miner, Vector3 targetPos, Vector3 minerPos)
	{
		MountainPlatformingLevelPickaxeProjectile mountainPlatformingLevelPickaxeProjectile = base.Create(pos, rotation) as MountainPlatformingLevelPickaxeProjectile;
		mountainPlatformingLevelPickaxeProjectile.miner = miner;
		mountainPlatformingLevelPickaxeProjectile.speed = speed;
		mountainPlatformingLevelPickaxeProjectile.minerPosition = minerPos;
		mountainPlatformingLevelPickaxeProjectile.targetPos = targetPos;
		return mountainPlatformingLevelPickaxeProjectile;
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x001F2DD7 File Offset: 0x001F11D7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x001F2DF8 File Offset: 0x001F11F8
	private IEnumerator throw_pickaxe_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 startPos = base.transform.position;
		float time = Vector3.Distance(base.transform.position, this.targetPos) / this.speed;
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / time);
			base.transform.position = Vector3.Lerp(startPos, this.targetPos, val);
			yield return wait;
		}
		yield return wait;
		base.transform.position = this.targetPos;
		t = 0f;
		Vector3 dir = startPos - this.targetPos;
		for (;;)
		{
			if (this.miner != null)
			{
				if (t >= time)
				{
					break;
				}
				t += CupheadTime.FixedDelta;
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
				base.transform.position = Vector3.Lerp(this.targetPos, this.minerPosition, t2);
			}
			else
			{
				base.transform.position += dir.normalized * this.speed * CupheadTime.FixedDelta;
			}
			yield return wait;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003D9C RID: 15772
	private Vector3 minerPosition;

	// Token: 0x04003D9D RID: 15773
	private Vector3 targetPos;

	// Token: 0x04003D9E RID: 15774
	private MountainPlatformingLevelMiner miner;

	// Token: 0x04003D9F RID: 15775
	private float speed;

	// Token: 0x04003DA0 RID: 15776
	private const float MAX_DIST = 5f;
}
