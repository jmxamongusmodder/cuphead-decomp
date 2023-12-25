using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006D9 RID: 1753
public class MausoleumLevelSineGhost : MausoleumLevelGhostBase
{
	// Token: 0x06002557 RID: 9559 RVA: 0x0015D5E0 File Offset: 0x0015B9E0
	public MausoleumLevelSineGhost Create(Vector2 position, float rotation, float speed, LevelProperties.Mausoleum.SineGhost properties)
	{
		MausoleumLevelSineGhost mausoleumLevelSineGhost = base.Create(position, rotation, speed) as MausoleumLevelSineGhost;
		mausoleumLevelSineGhost.rotation = rotation;
		mausoleumLevelSineGhost.properties = properties;
		return mausoleumLevelSineGhost;
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x0015D60C File Offset: 0x0015BA0C
	protected override void Start()
	{
		base.Start();
		this.CalculateDirection();
		this.CalculateSin();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x0015D630 File Offset: 0x0015BA30
	private void CalculateSin()
	{
		Vector2 b = Vector2.zero;
		b = MathUtils.AngleToDirection(this.rotation) / 2f;
		float num = -((b.x - base.transform.position.x) / (b.y - base.transform.position.y));
		float num2 = b.y - num * b.x;
		Vector2 zero = Vector2.zero;
		zero.x = b.x + 1f;
		zero.y = num * zero.x + num2;
		this.normalized = Vector3.zero;
		this.normalized = zero - b;
		this.normalized.Normalize();
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x0015D6FC File Offset: 0x0015BAFC
	private void CalculateDirection()
	{
		Vector2 vector = Vector2.zero;
		vector = MathUtils.AngleToDirection(this.rotation);
		float value = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		this.pointAtTarget = MathUtils.AngleToDirection(value);
		base.transform.SetEulerAngles(null, null, new float?(value));
	}

	// Token: 0x0600255B RID: 9563 RVA: 0x0015D76C File Offset: 0x0015BB6C
	private IEnumerator move_cr()
	{
		Vector3 pos = base.transform.position;
		for (;;)
		{
			this.angle += this.properties.waveSpeed * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				pos += this.normalized * Mathf.Sin(this.angle + this.properties.waveAmount) * (this.properties.waveAmount / 2f);
			}
			pos += this.pointAtTarget * this.properties.ghostSpeed * CupheadTime.Delta;
			base.transform.position = pos;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x0015D788 File Offset: 0x0015BB88
	protected override void Die()
	{
		if (!base.isDead)
		{
			SpriteDeathParts spriteDeathParts = this.hat.CreatePart(base.transform.position);
			spriteDeathParts.animator.SetBool("HatA", Rand.Bool());
		}
		base.Die();
	}

	// Token: 0x0600255D RID: 9565 RVA: 0x0015D7D2 File Offset: 0x0015BBD2
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.hat = null;
	}

	// Token: 0x04002DEA RID: 11754
	[SerializeField]
	private SpriteDeathParts hat;

	// Token: 0x04002DEB RID: 11755
	private Vector3 pointAtTarget;

	// Token: 0x04002DEC RID: 11756
	private Vector3 normalized;

	// Token: 0x04002DED RID: 11757
	private float rotation;

	// Token: 0x04002DEE RID: 11758
	private float angle;

	// Token: 0x04002DEF RID: 11759
	private LevelProperties.Mausoleum.SineGhost properties;
}
