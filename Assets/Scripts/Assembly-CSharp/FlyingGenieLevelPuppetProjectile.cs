using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000676 RID: 1654
public class FlyingGenieLevelPuppetProjectile : BasicProjectile
{
	// Token: 0x1700039A RID: 922
	// (get) Token: 0x060022D4 RID: 8916 RVA: 0x00146FE8 File Offset: 0x001453E8
	public float minRadius
	{
		get
		{
			return this._minRadius;
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x060022D5 RID: 8917 RVA: 0x00146FF0 File Offset: 0x001453F0
	public float maxRadius
	{
		get
		{
			return this._maxRadius;
		}
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x00146FF8 File Offset: 0x001453F8
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_spark_cr());
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x00147010 File Offset: 0x00145410
	private IEnumerator spawn_spark_cr()
	{
		string[] pattern = new string[]
		{
			"B",
			"P",
			"B",
			"P",
			"P",
			"B",
			"P",
			"B",
			"B",
			"P",
			"B",
			"P"
		};
		int patternIndex = UnityEngine.Random.Range(0, pattern.Length);
		for (;;)
		{
			Vector2 a = base.baseTransform.position;
			Vector2 vector = new Vector2(UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1), UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1));
			Vector2 target = a + vector.normalized * UnityEngine.Random.Range(this.minRadius, this.maxRadius);
			if (pattern[patternIndex] == "B")
			{
				this.sparksBlue[UnityEngine.Random.Range(0, this.sparksBlue.Length)].Create(target);
			}
			else
			{
				this.sparksPink[UnityEngine.Random.Range(0, this.sparksPink.Length)].Create(target);
			}
			patternIndex = (patternIndex + 1) % pattern.Length;
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.08f, 0.2f));
		}
		yield break;
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x0014702C File Offset: 0x0014542C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position, this.minRadius);
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position, this.maxRadius);
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x001470BB File Offset: 0x001454BB
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x04002B74 RID: 11124
	[SerializeField]
	private float _minRadius = 100f;

	// Token: 0x04002B75 RID: 11125
	[SerializeField]
	private float _maxRadius = 200f;

	// Token: 0x04002B76 RID: 11126
	[SerializeField]
	private Effect[] sparksBlue;

	// Token: 0x04002B77 RID: 11127
	[SerializeField]
	private Effect[] sparksPink;
}
