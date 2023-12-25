using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051F RID: 1311
public class BeeLevelQueenSpitProjectile : AbstractProjectile
{
	// Token: 0x06001778 RID: 6008 RVA: 0x000D3470 File Offset: 0x000D1870
	public BeeLevelQueenSpitProjectile Create(Vector2 pos, Vector2 scale, float speed, Vector2 time)
	{
		BeeLevelQueenSpitProjectile beeLevelQueenSpitProjectile = base.Create(pos, 0f, scale) as BeeLevelQueenSpitProjectile;
		beeLevelQueenSpitProjectile.speed = speed;
		beeLevelQueenSpitProjectile.time = time;
		return beeLevelQueenSpitProjectile;
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06001779 RID: 6009 RVA: 0x000D34A0 File Offset: 0x000D18A0
	protected override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x000D34A7 File Offset: 0x000D18A7
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.rotate_cr());
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x000D34D6 File Offset: 0x000D18D6
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x000D34F4 File Offset: 0x000D18F4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x000D351D File Offset: 0x000D191D
	private void End()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x000D3530 File Offset: 0x000D1930
	private IEnumerator trail_cr()
	{
		for (;;)
		{
			this.trailPrefab.Create(base.transform.position);
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
		}
		yield break;
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x000D354C File Offset: 0x000D194C
	private IEnumerator move_cr()
	{
		float scale = base.transform.localScale.x;
		for (;;)
		{
			Vector2 move = base.transform.right * this.speed * CupheadTime.Delta * scale;
			base.transform.AddPosition(move.x, move.y, 0f);
			yield return null;
			if (base.transform.position.y > 720f)
			{
				this.End();
			}
		}
		yield break;
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x000D3568 File Offset: 0x000D1968
	private IEnumerator rotate_cr()
	{
		float rotTime = 0.15f;
		float scale = base.transform.localScale.x;
		yield return CupheadTime.WaitForSeconds(this, 0.05f);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.time.x);
			yield return base.StartCoroutine(this.tweenRotation_cr(0f, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.y);
			yield return base.StartCoroutine(this.tweenRotation_cr(90f * scale, 180f * scale, rotTime));
			AudioManager.Play("bee_spit_bullet_turn");
			this.emitAudioFromObject.Add("bee_spit_bullet_turn");
			yield return CupheadTime.WaitForSeconds(this, this.time.x);
			yield return base.StartCoroutine(this.tweenRotation_cr(180f * scale, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.y);
			yield return base.StartCoroutine(this.tweenRotation_cr(90f * scale, 0f, rotTime));
		}
		yield break;
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x000D3584 File Offset: 0x000D1984
	private IEnumerator tweenRotation_cr(float start, float end, float time)
	{
		base.transform.SetEulerAngles(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetEulerAngles(null, null, new float?(EaseUtils.Ease(EaseUtils.EaseType.linear, start, end, val)));
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetEulerAngles(null, null, new float?(end));
		yield break;
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x000D35B4 File Offset: 0x000D19B4
	private IEnumerator tween_cr(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease)
	{
		base.transform.position = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(ease, start.x, end.x, val);
			float y = EaseUtils.Ease(ease, start.y, end.y, val);
			base.transform.SetLocalPosition(new float?(x), new float?(y), new float?(0f));
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		yield break;
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x000D35EC File Offset: 0x000D19EC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.trailPrefab = null;
	}

	// Token: 0x040020B3 RID: 8371
	[SerializeField]
	private Effect trailPrefab;

	// Token: 0x040020B4 RID: 8372
	private Vector2 time = new Vector2(0.43f, 0.06f);

	// Token: 0x040020B5 RID: 8373
	private float speed = 700f;
}
