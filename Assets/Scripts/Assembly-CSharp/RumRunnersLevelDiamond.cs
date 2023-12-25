using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200078B RID: 1931
public class RumRunnersLevelDiamond : AbstractCollidableObject
{
	// Token: 0x06002AA8 RID: 10920 RVA: 0x0018EAFE File Offset: 0x0018CEFE
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x0018EB11 File Offset: 0x0018CF11
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x0018EB29 File Offset: 0x0018CF29
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x0018EB40 File Offset: 0x0018CF40
	public void Die()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x0018EB53 File Offset: 0x0018CF53
	public void SetAttack(bool attack)
	{
		if (attack)
		{
			this.horn.enabled = false;
			this.hornAttack.enabled = true;
		}
		else
		{
			this.horn.enabled = true;
			this.hornAttack.enabled = false;
		}
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x0018EB90 File Offset: 0x0018CF90
	public void StartSparkle()
	{
		base.StartCoroutine(this.startSparkle_cr());
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x0018EBA0 File Offset: 0x0018CFA0
	private IEnumerator startSparkle_cr()
	{
		Color color = this.sparkle.color;
		color.a = 0f;
		this.sparkle.color = color;
		base.animator.Play("On", 1);
		float elapsedTime = 0f;
		while (elapsedTime < 0.2f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			color.a = Mathf.Lerp(0f, 1f, elapsedTime / 0.2f);
			this.sparkle.color = color;
		}
		yield break;
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x0018EBBB File Offset: 0x0018CFBB
	public void EndSparkle()
	{
		base.StartCoroutine(this.endSparkle_cr());
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x0018EBCC File Offset: 0x0018CFCC
	private IEnumerator endSparkle_cr()
	{
		Color color = this.sparkle.color;
		this.sparkle.color = color;
		float elapsedTime = 0f;
		while (elapsedTime < 0.45f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			color.a = Mathf.Lerp(1f, 0f, elapsedTime / 0.45f);
			this.sparkle.color = color;
		}
		base.animator.Play("Off", 1);
		yield break;
	}

	// Token: 0x0400336C RID: 13164
	[SerializeField]
	private SpriteRenderer horn;

	// Token: 0x0400336D RID: 13165
	[SerializeField]
	private SpriteRenderer hornAttack;

	// Token: 0x0400336E RID: 13166
	[SerializeField]
	private SpriteRenderer sparkle;

	// Token: 0x0400336F RID: 13167
	private DamageDealer damageDealer;
}
