using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007AE RID: 1966
public class SallyStagePlayLevelHeart : AbstractProjectile
{
	// Token: 0x06002C2E RID: 11310 RVA: 0x0019FA24 File Offset: 0x0019DE24
	protected override void Update()
	{
		this.damageDealer.Update();
		base.Update();
	}

	// Token: 0x06002C2F RID: 11311 RVA: 0x0019FA38 File Offset: 0x0019DE38
	public void InitHeart(LevelProperties.SallyStagePlay properties, int direction, bool isParryable)
	{
		this.properties = properties;
		this.time = 0f;
		this.direction = direction;
		this.pos = base.transform.position;
		if (!isParryable)
		{
			base.GetComponent<SpriteRenderer>().color = Color.blue;
		}
		else
		{
			this.SetParryable(true);
		}
		base.StartCoroutine(this.wave_cr());
	}

	// Token: 0x06002C30 RID: 11312 RVA: 0x0019FAA0 File Offset: 0x0019DEA0
	private IEnumerator wave_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		for (;;)
		{
			Vector3 newPos = this.pos;
			newPos.y = Mathf.Sin(this.time * this.properties.CurrentState.kiss.sineWaveSpeed) * this.properties.CurrentState.kiss.sineWaveStrength;
			base.transform.position = newPos;
			this.pos += Vector3.left * (float)this.direction * this.properties.CurrentState.kiss.heartSpeed * CupheadTime.Delta;
			this.time += CupheadTime.Delta;
			yield return null;
			if (base.transform.position.x < (float)(Level.Current.Left - 150) || base.transform.position.x > (float)(Level.Current.Right + 150))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		yield break;
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x0019FABB File Offset: 0x0019DEBB
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x0019FAE4 File Offset: 0x0019DEE4
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x040034DB RID: 13531
	private int direction;

	// Token: 0x040034DC RID: 13532
	private float time;

	// Token: 0x040034DD RID: 13533
	private Vector3 pos;

	// Token: 0x040034DE RID: 13534
	private LevelProperties.SallyStagePlay properties;
}
