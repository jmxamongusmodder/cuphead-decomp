using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200066D RID: 1645
public class FlyingGenieLevelGoop : LevelProperties.FlyingGenie.Entity
{
	// Token: 0x06002290 RID: 8848 RVA: 0x00144C4C File Offset: 0x0014304C
	public override void LevelInit(LevelProperties.FlyingGenie properties)
	{
		base.LevelInit(properties);
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002291 RID: 8849 RVA: 0x00144C78 File Offset: 0x00143078
	public void ActivateGoop()
	{
		base.animator.SetTrigger("OnStartGoop");
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x00144CC7 File Offset: 0x001430C7
	private void DeactivateGoop()
	{
		this.moving = false;
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		this.StopAllCoroutines();
		base.animator.Play("Off");
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x00144CFE File Offset: 0x001430FE
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x00144D14 File Offset: 0x00143114
	public IEnumerator move_cr()
	{
		LevelProperties.FlyingGenie.Coffin p = base.properties.CurrentState.coffin;
		bool goingUp = false;
		this.moving = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		for (;;)
		{
			if (this.moving)
			{
				if (goingUp)
				{
					while (base.transform.position.y < this.yMax)
					{
						base.transform.AddPosition(0f, p.heartMovement * CupheadTime.Delta, 0f);
						yield return null;
					}
					goingUp = !goingUp;
				}
				else
				{
					while (base.transform.position.y > this.yMin)
					{
						base.transform.AddPosition(0f, -p.heartMovement * CupheadTime.Delta, 0f);
						yield return null;
					}
					goingUp = !goingUp;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x00144D30 File Offset: 0x00143130
	private IEnumerator shoot_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		for (;;)
		{
			if (this.moving)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.coffin.heartShotDelayRange.RandomFloat());
				base.animator.SetTrigger("OnAttack");
				yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x00144D4C File Offset: 0x0014314C
	private void ShootProjectiles()
	{
		AudioManager.Play("genie_sarcophagus_eye_plop");
		this.emitAudioFromObject.Add("genie_sarcophagus_eye_plop");
		this.projectile.Create(this.topRoot.position, base.properties.CurrentState.coffin, true);
		this.projectile.Create(this.bottomRoot.position, base.properties.CurrentState.coffin, false);
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x00144DC3 File Offset: 0x001431C3
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		base.StartCoroutine(this.death_cr());
		AudioManager.Play("genie_goop_voice_exit");
		this.emitAudioFromObject.Add("genie_goop_voice_exit");
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x00144E04 File Offset: 0x00143204
	private IEnumerator death_cr()
	{
		float moveSpeed = 50f;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		while (base.transform.localPosition.x < this.endRoot.localPosition.x)
		{
			base.transform.localPosition += base.transform.right * moveSpeed;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.DeactivateGoop();
		yield return null;
		yield break;
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x00144E1F File Offset: 0x0014321F
	private void SoundGenieGoopIntro()
	{
		AudioManager.Play("genie_goop_voice_enter");
		this.emitAudioFromObject.Add("genie_goop_voice_enter");
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x00144E3B File Offset: 0x0014323B
	private void SoundGenieGoopAttackPre()
	{
		AudioManager.Play("genie_goop_attack_pre");
		this.emitAudioFromObject.Add("genie_goop_attack_pre");
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x00144E57 File Offset: 0x00143257
	private void SoundGenieGoopAttack()
	{
		AudioManager.Play("gene_goop_voice_attack");
		this.emitAudioFromObject.Add("gene_goop_voice_attack");
	}

	// Token: 0x04002B40 RID: 11072
	[SerializeField]
	private Transform topRoot;

	// Token: 0x04002B41 RID: 11073
	[SerializeField]
	private Transform bottomRoot;

	// Token: 0x04002B42 RID: 11074
	[SerializeField]
	private FlyingGenieLevelHelixProjectile projectile;

	// Token: 0x04002B43 RID: 11075
	[SerializeField]
	private Transform endRoot;

	// Token: 0x04002B44 RID: 11076
	private bool moving;

	// Token: 0x04002B45 RID: 11077
	private float yMax = 60f;

	// Token: 0x04002B46 RID: 11078
	private float yMin = -260f;

	// Token: 0x04002B47 RID: 11079
	private DamageReceiver damageReceiver;
}
