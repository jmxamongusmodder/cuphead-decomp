using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008A6 RID: 2214
public class CircusPlatformingLevelMagician : AbstractPlatformingLevelEnemy
{
	// Token: 0x0600338F RID: 13199 RVA: 0x001DF6D4 File Offset: 0x001DDAD4
	protected override void Start()
	{
		base.Start();
		this.spawnPoints = new List<Transform>();
		this.spawnPoints.AddRange(this.spawnPointHolder.GetComponentsInChildren<Transform>());
		this.spawnPoints.RemoveAt(0);
		base.StartCoroutine(this.check_cr());
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x001DF721 File Offset: 0x001DDB21
	protected override void OnStart()
	{
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x001DF724 File Offset: 0x001DDB24
	private IEnumerator check_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AbstractPlayerController player = PlayerManager.GetNext();
		while (player.transform.position.x < this.startPos.transform.position.x)
		{
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			yield return null;
		}
		base.StartCoroutine(this.appear_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x001DF740 File Offset: 0x001DDB40
	private IEnumerator appear_cr()
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		yield return CupheadTime.WaitForSeconds(this, base.Properties.magicianAppearDelayRange.RandomFloat());
		for (;;)
		{
			while (player.transform.position.x < this.startPos.transform.position.x || player.transform.position.x > this.endPos.transform.position.x)
			{
				yield return null;
			}
			this.EnableMagician(true);
			while (!this.attackTrigger)
			{
				yield return null;
			}
			while (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(0f, 1000f)))
			{
				yield return null;
			}
			player = PlayerManager.GetFirst();
			Vector2 dir = player.transform.position - base.transform.position;
			this.projectileInstance = (this.projectile.Create(base.transform.position, MathUtils.DirectionToAngle(dir), base.Properties.ProjectileSpeed) as CircusPlatformingLevelMagicianBullet);
			this.projectileInstance.OnProjectileDeath += this.OnProjectileDeath;
			while (!this.disappearTrigger)
			{
				yield return null;
			}
			this.disappearTrigger = false;
			this.attackTrigger = false;
			this.EnableMagician(false);
			while (this.t < base.Properties.magicianAppearDelayRange.RandomFloat())
			{
				this.t += CupheadTime.Delta;
				yield return null;
			}
			this.t = 0f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x001DF75B File Offset: 0x001DDB5B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.projectileInstance != null)
		{
			this.projectileInstance.OnProjectileDeath -= this.OnProjectileDeath;
		}
		this.projectile = null;
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x001DF792 File Offset: 0x001DDB92
	private void OnProjectileDeath()
	{
		base.animator.SetTrigger("EndAttack");
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x001DF7A4 File Offset: 0x001DDBA4
	public void Attack()
	{
		this.attackTrigger = true;
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x001DF7AD File Offset: 0x001DDBAD
	public void Disappear()
	{
		this.disappearTrigger = true;
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x001DF7B8 File Offset: 0x001DDBB8
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.white;
		List<Transform> list = new List<Transform>();
		list.AddRange(this.spawnPointHolder.GetComponentsInChildren<Transform>());
		list.RemoveAt(0);
		for (int i = 0; i < list.Count; i++)
		{
			Gizmos.DrawWireSphere(list[i].transform.position, 50f);
		}
		Gizmos.DrawLine(new Vector2(this.startPos.transform.position.x, this.startPos.transform.position.y + 1000f), new Vector2(this.startPos.transform.position.x, this.startPos.transform.position.y - 1000f));
		Gizmos.DrawLine(new Vector2(this.endPos.transform.position.x, this.endPos.transform.position.y + 1000f), new Vector2(this.endPos.transform.position.x, this.endPos.transform.position.y - 1000f));
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x001DF938 File Offset: 0x001DDD38
	private void EnableMagician(bool enabled)
	{
		base.GetComponent<Animator>().enabled = enabled;
		base.GetComponent<Collider2D>().enabled = enabled;
		base.GetComponent<SpriteRenderer>().enabled = enabled;
		if (enabled)
		{
			base.transform.position = this.spawnPoints[UnityEngine.Random.Range(0, this.spawnPoints.Count)].transform.position;
		}
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x001DF9A0 File Offset: 0x001DDDA0
	protected override void Die()
	{
		AudioManager.Play("circus_generic_death_big");
		this.emitAudioFromObject.Add("circus_generic_death_big");
		base.Die();
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x001DF9C2 File Offset: 0x001DDDC2
	private void AttackAppearSFX()
	{
		AudioManager.Play("circus_magician_appears");
		this.emitAudioFromObject.Add("circus_magician_appears");
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x001DF9DE File Offset: 0x001DDDDE
	private void AttackIntroSFX()
	{
		AudioManager.Play("circus_magician_attack_intro");
		this.emitAudioFromObject.Add("circus_magician_attack_intro");
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x001DF9FA File Offset: 0x001DDDFA
	private void AttackOutroSFX()
	{
		AudioManager.Play("circus_magician_attack_outro");
		this.emitAudioFromObject.Add("circus_magician_attack_outro");
	}

	// Token: 0x04003BE4 RID: 15332
	private const string EndAttackParameterName = "EndAttack";

	// Token: 0x04003BE5 RID: 15333
	[SerializeField]
	private Transform startPos;

	// Token: 0x04003BE6 RID: 15334
	[SerializeField]
	private Transform endPos;

	// Token: 0x04003BE7 RID: 15335
	[SerializeField]
	private Transform spawnPointHolder;

	// Token: 0x04003BE8 RID: 15336
	[SerializeField]
	private CircusPlatformingLevelMagicianBullet projectile;

	// Token: 0x04003BE9 RID: 15337
	private List<Transform> spawnPoints;

	// Token: 0x04003BEA RID: 15338
	private bool attackTrigger;

	// Token: 0x04003BEB RID: 15339
	private bool disappearTrigger;

	// Token: 0x04003BEC RID: 15340
	private float t;

	// Token: 0x04003BED RID: 15341
	private CircusPlatformingLevelMagicianBullet projectileInstance;
}
