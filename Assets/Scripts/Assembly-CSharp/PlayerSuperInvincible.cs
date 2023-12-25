using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A5C RID: 2652
public class PlayerSuperInvincible : AbstractPlayerSuper
{
	// Token: 0x06003F34 RID: 16180 RVA: 0x00229964 File Offset: 0x00227D64
	protected override void StartSuper()
	{
		base.StartSuper();
		AudioManager.Play("player_super_invincibility");
		if (this.player.id == PlayerId.PlayerOne)
		{
			this.shadowMugman.SetActive(false);
			this.shadow = this.shadowCuphead;
		}
		else
		{
			this.shadowCuphead.SetActive(false);
			this.shadow = this.shadowMugman;
		}
		base.transform.position = this.player.transform.position;
		base.StartCoroutine(this.super_cr());
		if (!this.player.motor.Grounded)
		{
			this.shadow.SetActive(false);
			this.shadow.transform.position = this.player.GetComponent<LevelPlayerShadow>().ShadowPosition() + this.shadowOffset;
		}
		Level.ScoringData.superMeterUsed += 5;
	}

	// Token: 0x06003F35 RID: 16181 RVA: 0x00229A4C File Offset: 0x00227E4C
	public override void Interrupt()
	{
		this.StopAllCoroutines();
		AudioManager.ChangeBGMPitch(1f, 1.5f);
		if (this.player != null)
		{
			this.player.animationController.SetOldMaterial();
			this.player.stats.SetInvincible(false);
		}
	}

	// Token: 0x06003F36 RID: 16182 RVA: 0x00229AA0 File Offset: 0x00227EA0
	private IEnumerator super_cr()
	{
		if (this.player != null)
		{
			this.player.stats.SetInvincible(true);
		}
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelSuperInvincibility.durationInvincible);
		if (this.player != null)
		{
			this.player.stats.SetInvincible(false);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003F37 RID: 16183 RVA: 0x00229ABC File Offset: 0x00227EBC
	private IEnumerator invincibility_fx_cr()
	{
		IEnumerator sparkleRoutine = this.sparkle_cr();
		base.StartCoroutine(sparkleRoutine);
		if (this.player != null)
		{
			this.player.animationController.SetMaterial(this.superMaterial);
		}
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelSuperInvincibility.durationFX - 1.25f);
		AudioManager.ChangeBGMPitch(1.8f, 1.5f);
		for (int i = 0; i < 5; i++)
		{
			if (this.player != null)
			{
				this.player.animationController.SetOldMaterial();
			}
			yield return CupheadTime.WaitForSeconds(this, 0.125f);
			if (this.player != null)
			{
				this.player.animationController.SetMaterial(this.superMaterial);
			}
			yield return CupheadTime.WaitForSeconds(this, 0.125f);
		}
		AudioManager.ChangeBGMPitch(1f, 1.5f);
		if (this.player != null)
		{
			this.player.animationController.SetOldMaterial();
		}
		base.StopCoroutine(sparkleRoutine);
		yield return null;
		yield break;
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x00229AD8 File Offset: 0x00227ED8
	private IEnumerator sparkle_cr()
	{
		while (true && this.player != null)
		{
			float x = UnityEngine.Random.Range(-this.player.colliderManager.Width, this.player.colliderManager.Width);
			float y = UnityEngine.Random.Range(this.player.colliderManager.Height * -0.5f, this.player.colliderManager.Height * 1.5f);
			this.sparkle.Create(this.player.transform.position + new Vector3(x, y, 0f));
			yield return CupheadTime.WaitForSeconds(this, this.sparkleSpawnTime);
		}
		yield break;
	}

	// Token: 0x06003F39 RID: 16185 RVA: 0x00229AF4 File Offset: 0x00227EF4
	private void EndPlayerAnimation()
	{
		this.Fire();
		this.EndSuper(false);
		base.StartCoroutine(this.invincibility_fx_cr());
		base.StartCoroutine(this.super_cr());
		if (this.player != null)
		{
			this.player.animationController.SetSpriteProperties(SpriteLayer.Effects, 3000);
		}
	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x00229B50 File Offset: 0x00227F50
	private void BigCupAppears()
	{
		if (!this.player.motor.Grounded)
		{
			this.shadow.SetActive(true);
			float num = Mathf.Abs(this.player.transform.position.y - this.shadow.transform.position.y);
			float d = Mathf.Max(0f, 1f - num / 500f);
			this.shadow.transform.localScale = Vector3.one * d;
		}
	}

	// Token: 0x06003F3B RID: 16187 RVA: 0x00229BE8 File Offset: 0x00227FE8
	private void ResetSpriteOrder()
	{
		if (this.player != null)
		{
			this.player.animationController.ResetSpriteProperties();
		}
	}

	// Token: 0x04004646 RID: 17990
	private const float maxShadowDistance = 500f;

	// Token: 0x04004647 RID: 17991
	[SerializeField]
	private Material superMaterial;

	// Token: 0x04004648 RID: 17992
	[SerializeField]
	private Effect sparkle;

	// Token: 0x04004649 RID: 17993
	[SerializeField]
	private float sparkleSpawnTime;

	// Token: 0x0400464A RID: 17994
	[SerializeField]
	private Vector3 shadowOffset;

	// Token: 0x0400464B RID: 17995
	[SerializeField]
	private GameObject shadowCuphead;

	// Token: 0x0400464C RID: 17996
	[SerializeField]
	private GameObject shadowMugman;

	// Token: 0x0400464D RID: 17997
	private GameObject shadow;
}
