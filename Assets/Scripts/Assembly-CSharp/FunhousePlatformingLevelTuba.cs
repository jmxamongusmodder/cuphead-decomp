using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008BF RID: 2239
public class FunhousePlatformingLevelTuba : AbstractPlatformingLevelEnemy
{
	// Token: 0x06003443 RID: 13379 RVA: 0x001E5158 File Offset: 0x001E3558
	protected override void Start()
	{
		base.Start();
		base.transform.position = this.startPos.transform.position;
		this.start = this.startPos.transform.position;
		this.end = this.endPos.transform.position;
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06003444 RID: 13380 RVA: 0x001E51C9 File Offset: 0x001E35C9
	protected override void OnStart()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06003445 RID: 13381 RVA: 0x001E51D8 File Offset: 0x001E35D8
	private IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x06003446 RID: 13382 RVA: 0x001E51F4 File Offset: 0x001E35F4
	private IEnumerator attack_cr()
	{
		float time = base.Properties.MoveSpeed;
		float t = 0f;
		yield return CupheadTime.WaitForSeconds(this, base.Properties.tubaInitialDelay);
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			t = 0f;
			base.animator.SetBool("isAttacking", true);
			yield return base.animator.WaitForAnimationToEnd(this, "Tuba_Anti", false, true);
			base.animator.Play("Attack_" + ((!Rand.Bool()) ? "B" : "A"), 1);
			base.StartCoroutine(this.shoot_cr());
			while (t < time)
			{
				t += CupheadTime.Delta;
				Vector2 pos = base.transform.position;
				pos.y = Mathf.Lerp(base.transform.position.y, this.end.y, t / time);
				base.transform.position = pos;
				yield return null;
			}
			t = 0f;
			while (t < time)
			{
				t += CupheadTime.Delta;
				Vector2 pos2 = base.transform.position;
				pos2.y = Mathf.Lerp(base.transform.position.y, this.start.y, t / time);
				base.transform.position = pos2;
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.tubaMainDelayRange.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x001E5210 File Offset: 0x001E3610
	private IEnumerator shoot_cr()
	{
		AudioManager.Play("funhouse_tuba_attack");
		this.emitAudioFromObject.Add("funhouse_tuba_attack");
		float delay = 0f;
		BasicProjectile p = this.projectile.Create(this.root.transform.position, 180f, base.Properties.ProjectileSpeed);
		p.animator.Play("BW");
		p.transform.parent = base.transform;
		p.OnDie += this.OnBwaaDie;
		this.bwaaList.Add(p.gameObject);
		delay = p.transform.GetComponent<SpriteRenderer>().bounds.size.x / 1.4f / base.Properties.ProjectileSpeed;
		yield return CupheadTime.WaitForSeconds(this, delay);
		for (int i = 0; i < base.Properties.tubaACount; i++)
		{
			p = this.projectile.Create(this.root.transform.position, 180f, base.Properties.ProjectileSpeed);
			p.animator.Play("A" + UnityEngine.Random.Range(1, 4).ToStringInvariant());
			p.transform.parent = base.transform;
			p.OnDie += this.OnBwaaDie;
			this.bwaaList.Add(p.gameObject);
			delay = p.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2f / base.Properties.ProjectileSpeed;
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		p = this.projectile.Create(this.root.transform.position, 180f, base.Properties.ProjectileSpeed);
		p.animator.Play("EXCLAIM");
		p.transform.parent = base.transform;
		p.OnDie += this.OnBwaaDie;
		this.bwaaList.Add(p.gameObject);
		yield return CupheadTime.WaitForSeconds(this, delay);
		base.animator.SetBool("isAttacking", false);
		yield return null;
		yield break;
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x001E522B File Offset: 0x001E362B
	private void OnBwaaDie(AbstractProjectile p)
	{
		p.OnDie -= this.OnBwaaDie;
		if (this.bwaaList != null)
		{
			this.bwaaList.Remove(p.gameObject);
		}
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x001E525C File Offset: 0x001E365C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		Gizmos.DrawLine(this.startPos.transform.position, this.endPos.transform.position);
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(this.startPos.transform.position, 10f);
		Gizmos.DrawWireSphere(this.endPos.transform.position, 10f);
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x001E5304 File Offset: 0x001E3704
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		base.StartCoroutine(this.slide_off_cr());
	}

	// Token: 0x0600344B RID: 13387 RVA: 0x001E532C File Offset: 0x001E372C
	private IEnumerator slide_off_cr()
	{
		for (int i = 0; i < this.bwaaList.Count; i++)
		{
			if (this.bwaaList[i] != null)
			{
				this.bwaaList[i].transform.SetParent(null);
			}
		}
		float t = 0f;
		float time = 3f;
		float start = base.transform.position.y;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			if (base.transform.localScale.y > 0f)
			{
				base.transform.SetPosition(null, new float?(Mathf.Lerp(start, -860f, t / time)), null);
			}
			else
			{
				base.transform.SetPosition(null, new float?(Mathf.Lerp(start, 1220f, t / time)), null);
			}
			yield return wait;
		}
		base.Die();
		yield return null;
		yield break;
	}

	// Token: 0x0600344C RID: 13388 RVA: 0x001E5347 File Offset: 0x001E3747
	private void SoundTubaAnti()
	{
		AudioManager.Play("funhouse_tuba_anti");
		this.emitAudioFromObject.Add("funhouse_tuba_anti");
	}

	// Token: 0x0600344D RID: 13389 RVA: 0x001E5363 File Offset: 0x001E3763
	private void SoundTubaDeath()
	{
		AudioManager.Play("funhouse_tuba_death");
		this.emitAudioFromObject.Add("funhouse_tuba_death");
	}

	// Token: 0x0600344E RID: 13390 RVA: 0x001E537F File Offset: 0x001E377F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
	}

	// Token: 0x04003C7D RID: 15485
	[SerializeField]
	private Transform root;

	// Token: 0x04003C7E RID: 15486
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04003C7F RID: 15487
	[SerializeField]
	private Transform startPos;

	// Token: 0x04003C80 RID: 15488
	[SerializeField]
	private Transform endPos;

	// Token: 0x04003C81 RID: 15489
	private float offset = 50f;

	// Token: 0x04003C82 RID: 15490
	private Vector2 start;

	// Token: 0x04003C83 RID: 15491
	private Vector2 end;

	// Token: 0x04003C84 RID: 15492
	private List<GameObject> bwaaList = new List<GameObject>();
}
