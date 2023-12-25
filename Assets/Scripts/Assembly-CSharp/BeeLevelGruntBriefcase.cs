using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class BeeLevelGruntBriefcase : AbstractProjectile
{
	// Token: 0x06001725 RID: 5925 RVA: 0x000CFCDC File Offset: 0x000CE0DC
	public BeeLevelGruntBriefcase Create(int xScale, Vector2 pos)
	{
		BeeLevelGruntBriefcase beeLevelGruntBriefcase = UnityEngine.Object.Instantiate<BeeLevelGruntBriefcase>(this);
		beeLevelGruntBriefcase.transform.position = pos;
		beeLevelGruntBriefcase.transform.SetScale(new float?((float)xScale), new float?(1f), new float?(1f));
		beeLevelGruntBriefcase.CollisionDeath.OnlyPlayer();
		beeLevelGruntBriefcase.DamagesType.OnlyPlayer();
		return beeLevelGruntBriefcase;
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x000CFD3F File Offset: 0x000CE13F
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.y_cr());
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x000CFD61 File Offset: 0x000CE161
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x000CFD80 File Offset: 0x000CE180
	private IEnumerator x_cr()
	{
		for (;;)
		{
			if (base.transform.position.x < -1280f || base.transform.position.x > 1280f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			base.transform.AddPosition(200f * CupheadTime.Delta * -base.transform.localScale.x, 0f, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x000CFD9C File Offset: 0x000CE19C
	private IEnumerator y_cr()
	{
		float t = 0f;
		float time = 0.5f;
		while (t < time)
		{
			float val = t / time;
			float speed = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 1500f, 0f, val) * CupheadTime.Delta;
			base.transform.AddPosition(0f, speed, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		while (t < time)
		{
			float val2 = t / time;
			float speed2 = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, -1500f, val2) * CupheadTime.Delta;
			base.transform.AddPosition(0f, speed2, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		for (;;)
		{
			if (base.transform.position.y < -720f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			base.transform.AddPosition(0f, -1500f * CupheadTime.Delta, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002067 RID: 8295
	private const float Y_SPEED = 1500f;

	// Token: 0x04002068 RID: 8296
	private const float X_SPEED = 200f;

	// Token: 0x04002069 RID: 8297
	private const float TIME = 0.5f;
}
