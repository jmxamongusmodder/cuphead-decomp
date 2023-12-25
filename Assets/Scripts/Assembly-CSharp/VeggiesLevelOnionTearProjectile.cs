using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000851 RID: 2129
public class VeggiesLevelOnionTearProjectile : AbstractProjectile
{
	// Token: 0x0600315E RID: 12638 RVA: 0x001CE1F8 File Offset: 0x001CC5F8
	public AbstractProjectile Create(float time, float x)
	{
		VeggiesLevelOnionTearProjectile veggiesLevelOnionTearProjectile = base.Create(new Vector2(x, 420f)) as VeggiesLevelOnionTearProjectile;
		veggiesLevelOnionTearProjectile.direction = this.direction;
		veggiesLevelOnionTearProjectile.time = time;
		veggiesLevelOnionTearProjectile.Init();
		return veggiesLevelOnionTearProjectile;
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x001CE236 File Offset: 0x001CC636
	protected void Init()
	{
		base.StartCoroutine(this.projectile_cr());
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x001CE245 File Offset: 0x001CC645
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x001CE270 File Offset: 0x001CC670
	protected override void Die()
	{
		base.Die();
		base.transform.SetScale(new float?((float)((!MathUtils.RandomBool()) ? -1 : 1)), null, null);
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x001CE2B8 File Offset: 0x001CC6B8
	private IEnumerator projectile_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float startY = base.transform.position.y;
		float endY = (float)Level.Current.Ground;
		float calculatedTime = this.time;
		float t = 0f;
		while (t < calculatedTime)
		{
			float val = t / calculatedTime;
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeInQuad, startY, endY, val);
			base.transform.SetPosition(null, new float?(y), null);
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		base.transform.SetPosition(null, new float?(endY), null);
		AudioManager.Play("level_veggies_onion_teardrop");
		this.Die();
		yield break;
	}

	// Token: 0x040039EB RID: 14827
	private const float startY = 420f;

	// Token: 0x040039EC RID: 14828
	private float time;

	// Token: 0x040039ED RID: 14829
	private int direction;
}
