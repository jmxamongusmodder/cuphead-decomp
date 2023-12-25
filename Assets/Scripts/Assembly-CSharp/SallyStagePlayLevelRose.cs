using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B4 RID: 1972
public class SallyStagePlayLevelRose : AbstractProjectile
{
	// Token: 0x06002C66 RID: 11366 RVA: 0x001A1840 File Offset: 0x0019FC40
	public SallyStagePlayLevelRose Create(Vector2 pos, LevelProperties.SallyStagePlay.Roses properties)
	{
		SallyStagePlayLevelRose sallyStagePlayLevelRose = base.Create(pos) as SallyStagePlayLevelRose;
		sallyStagePlayLevelRose.properties = properties;
		return sallyStagePlayLevelRose;
	}

	// Token: 0x06002C67 RID: 11367 RVA: 0x001A1864 File Offset: 0x0019FC64
	protected override void Start()
	{
		base.Start();
		base.transform.SetScale(new float?((float)((!Rand.Bool()) ? -1 : 1)), null, null);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x001A18B8 File Offset: 0x0019FCB8
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x001A18D6 File Offset: 0x0019FCD6
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x001A18F4 File Offset: 0x0019FCF4
	private IEnumerator move_cr()
	{
		float speed = this.properties.fallSpeed.min;
		while (base.transform.position.y > (float)(Level.Current.Ground + 10))
		{
			base.transform.position += Vector3.down * speed * CupheadTime.Delta;
			if (speed < this.properties.fallSpeed.max)
			{
				speed += this.properties.fallAcceleration;
			}
			yield return null;
		}
		base.animator.SetTrigger("Land");
		base.GetComponent<BoxCollider2D>().enabled = false;
		base.animator.SetBool("IsA", Rand.Bool());
		yield return CupheadTime.WaitForSeconds(this, this.properties.groundDuration);
		base.StartCoroutine(this.despawn_cr());
		yield break;
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x001A1910 File Offset: 0x0019FD10
	private IEnumerator despawn_cr()
	{
		SpriteRenderer s = base.GetComponentInChildren<SpriteRenderer>(false);
		float t = 0f;
		float time = 2f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			s.color = new Color(s.color.r, s.color.b, s.color.g, 1f - t / time);
			yield return null;
		}
		base.GetComponent<Collider2D>().enabled = false;
		this.Die();
		yield break;
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x001A192B File Offset: 0x0019FD2B
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.GetComponentInChildren<SpriteRenderer>(false).enabled = false;
		base.Die();
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x001A1948 File Offset: 0x0019FD48
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		this.pinkRose.SetActive(false);
		this.normalRose.SetActive(false);
		if (parryable)
		{
			this.pinkRose.SetActive(true);
		}
		else
		{
			this.normalRose.SetActive(true);
		}
	}

	// Token: 0x040034FC RID: 13564
	[SerializeField]
	private GameObject normalRose;

	// Token: 0x040034FD RID: 13565
	[SerializeField]
	private GameObject pinkRose;

	// Token: 0x040034FE RID: 13566
	private LevelProperties.SallyStagePlay.Roses properties;

	// Token: 0x040034FF RID: 13567
	private float speed;
}
