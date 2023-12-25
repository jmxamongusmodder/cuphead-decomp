using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
public class PlayerSuperGhost : AbstractPlayerSuper
{
	// Token: 0x06003F26 RID: 16166 RVA: 0x00229254 File Offset: 0x00227654
	protected override void StartSuper()
	{
		base.StartSuper();
		AudioManager.Play("player_super_ghost");
		if (!this.player.motor.Grounded)
		{
			this.cupheadBottom.enabled = false;
			this.mugmanBottom.enabled = false;
		}
		this.createHeart = true;
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06003F27 RID: 16167 RVA: 0x002292B4 File Offset: 0x002276B4
	private void FixedUpdate()
	{
		if (this.state != PlayerSuperGhost.State.Spinning)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		Quaternion localRotation = base.transform.localRotation;
		float num;
		if (this.t < WeaponProperties.LevelSuperGhost.initialSpeedTime)
		{
			num = Mathf.Clamp01(this.t / WeaponProperties.LevelSuperGhost.accelerationTime) * WeaponProperties.LevelSuperGhost.initialSpeed;
		}
		else
		{
			float num2 = Mathf.Clamp01((this.t - WeaponProperties.LevelSuperGhost.initialSpeedTime) / WeaponProperties.LevelSuperGhost.accelerationTime);
			num = Mathf.Lerp(WeaponProperties.LevelSuperGhost.initialSpeed, WeaponProperties.LevelSuperGhost.maxSpeed, num2);
		}
		Trilean2 lookDirection = new Trilean2(0, 0);
		if (this.player != null)
		{
			lookDirection = this.player.motor.LookDirection;
			if (this.player.motor.GravityReversed)
			{
				lookDirection.y *= -1;
			}
			if (this.player.IsDead || (lookDirection.x == 0 && lookDirection.y == 0))
			{
				lookDirection = this.lookDir;
			}
		}
		this.lookDir = lookDirection;
		Vector2 vector = new Vector2(this.lookDir.x, this.lookDir.y);
		Vector2 normalized = vector.normalized;
		Vector2 b = new Vector2(normalized.x * num, normalized.y * num);
		this.velocity = Vector2.Lerp(this.velocity, b, CupheadTime.FixedDelta * WeaponProperties.LevelSuperGhost.turnaroundEaseMultiplier);
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		if (lookDirection.x > 0)
		{
			if (localRotation.z < 0.034906585f)
			{
				localRotation.z += 0.01f;
			}
		}
		else if (localRotation.z > -0.034906585f)
		{
			localRotation.z -= 0.01f;
		}
		base.transform.localRotation = localRotation;
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position + new Vector2(0f, 150f * base.transform.localScale.y), new Vector2(200f, 200f)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x00229547 File Offset: 0x00227947
	private void EndPlayerAnimation()
	{
		this.Fire();
		this.EndSuper(true);
	}

	// Token: 0x06003F29 RID: 16169 RVA: 0x00229558 File Offset: 0x00227958
	private IEnumerator super_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Start", false, true);
		AudioManager.Play("player_super_beam");
		this.state = PlayerSuperGhost.State.Spinning;
		this.damageDealer = new DamageDealer(WeaponProperties.LevelSuperGhost.damage, WeaponProperties.LevelSuperGhost.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		tracker.Add(this.damageDealer);
		this.lookDir = this.player.motor.TrueLookDirection;
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelSuperGhost.initialSpeedTime);
		base.animator.SetTrigger("Continue");
		float t = 0f;
		float duration = (!this.createHeart) ? WeaponProperties.LevelSuperGhost.noHeartMaxSpeedTime : WeaponProperties.LevelSuperGhost.maxSpeedTime;
		while (t < duration && !this.interrupted)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.state = PlayerSuperGhost.State.Dying;
		base.animator.SetTrigger("Death");
		yield break;
	}

	// Token: 0x06003F2A RID: 16170 RVA: 0x00229573 File Offset: 0x00227973
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003F2B RID: 16171 RVA: 0x00229580 File Offset: 0x00227980
	private void SpawnHeart()
	{
		if (this.player != null && this.createHeart)
		{
			this.heartPrefab.Create(this.heartRoot.position, this.player.motor.GravityReversalMultiplier);
		}
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x002295D5 File Offset: 0x002279D5
	public override void Interrupt()
	{
		this.createHeart = false;
		base.Interrupt();
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x002295E4 File Offset: 0x002279E4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x002295EC File Offset: 0x002279EC
	private void SoundSuperGhostVoice()
	{
		AudioManager.Play("player_super_ghost_voice");
		this.emitAudioFromObject.Add("player_super_ghost_voice");
	}

	// Token: 0x04004637 RID: 17975
	[SerializeField]
	private PlayerSuperGhostHeart heartPrefab;

	// Token: 0x04004638 RID: 17976
	[SerializeField]
	private Transform heartRoot;

	// Token: 0x04004639 RID: 17977
	[SerializeField]
	private SpriteRenderer cupheadBottom;

	// Token: 0x0400463A RID: 17978
	[SerializeField]
	private SpriteRenderer mugmanBottom;

	// Token: 0x0400463B RID: 17979
	private PlayerSuperGhost.State state;

	// Token: 0x0400463C RID: 17980
	private Vector2 velocity = Vector2.zero;

	// Token: 0x0400463D RID: 17981
	private float t;

	// Token: 0x0400463E RID: 17982
	private Trilean2 lookDir;

	// Token: 0x0400463F RID: 17983
	private bool createHeart;

	// Token: 0x02000A5A RID: 2650
	public enum State
	{
		// Token: 0x04004641 RID: 17985
		Intro,
		// Token: 0x04004642 RID: 17986
		Spinning,
		// Token: 0x04004643 RID: 17987
		Dying
	}
}
