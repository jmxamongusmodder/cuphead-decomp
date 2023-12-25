using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AAC RID: 2732
public class PlaneSuperBomb : AbstractPlaneSuper
{
	// Token: 0x0600418E RID: 16782 RVA: 0x0023802C File Offset: 0x0023642C
	protected override void StartSuper()
	{
		base.StartSuper();
		this.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.player.stats.OnStoned += this.OnStoned;
		if ((this.player.id == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (this.player.id == PlayerId.PlayerTwo && PlayerManager.player1IsMugman))
		{
			this.boom.gameObject.SetActive(true);
		}
		else
		{
			this.boomMM.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x002380D4 File Offset: 0x002364D4
	private IEnumerator super_cr()
	{
		float t = 0f;
		this.damageDealer = new DamageDealer(WeaponProperties.PlaneSuperBomb.damage, WeaponProperties.PlaneSuperBomb.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		tracker.Add(this.damageDealer);
		while (t < WeaponProperties.PlaneSuperBomb.countdownTime && !this.earlyExplosion)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.Fire();
		if (this.player != null)
		{
			this.player.PauseAll();
			this.player.SetSpriteVisible(false);
			base.transform.position = this.player.transform.position;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		base.animator.SetTrigger("Explode");
		AudioManager.Stop("player_plane_bomb_ticktock_loop");
		AudioManager.Play("player_plane_bomb_explosion");
		yield break;
	}

	// Token: 0x06004190 RID: 16784 RVA: 0x002380EF File Offset: 0x002364EF
	private void OnStoned()
	{
		this.earlyExplosion = true;
	}

	// Token: 0x06004191 RID: 16785 RVA: 0x002380F8 File Offset: 0x002364F8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.earlyExplosion = true;
	}

	// Token: 0x06004192 RID: 16786 RVA: 0x00238101 File Offset: 0x00236501
	private void EndIntroAnimation()
	{
		this.StartCountdown();
		AudioManager.PlayLoop("player_plane_bomb_ticktock_loop");
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06004193 RID: 16787 RVA: 0x00238120 File Offset: 0x00236520
	private void PlayerReappear()
	{
		if (this.player != null)
		{
			this.player.UnpauseAll(false);
			this.player.SetSpriteVisible(true);
		}
	}

	// Token: 0x06004194 RID: 16788 RVA: 0x0023814B File Offset: 0x0023654B
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x00238158 File Offset: 0x00236558
	private void StartBoomScale()
	{
		this.boomRoutine = base.StartCoroutine(this.boomScale_cr());
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x0023816C File Offset: 0x0023656C
	private IEnumerator boomScale_cr()
	{
		float t = 0f;
		float frameTime = 0.041666668f;
		float scale = 1f;
		for (;;)
		{
			t += CupheadTime.Delta;
			while (t > frameTime)
			{
				t -= frameTime;
				scale *= 1.15f;
				this.boom.SetScale(new float?(scale), new float?(scale), null);
				this.boomMM.SetScale(new float?(scale), new float?(scale), null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x00238187 File Offset: 0x00236587
	public void Pause()
	{
		if (this.boomRoutine != null)
		{
			base.StopCoroutine(this.boomRoutine);
		}
	}

	// Token: 0x06004198 RID: 16792 RVA: 0x002381A0 File Offset: 0x002365A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.player != null)
		{
			this.player.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
			this.player.stats.OnStoned -= this.OnStoned;
		}
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x002381FC File Offset: 0x002365FC
	private void PlaneSuperBombLaughAudio()
	{
		AudioManager.Play("player_plane_bomb_laugh");
	}

	// Token: 0x0400480F RID: 18447
	private bool earlyExplosion;

	// Token: 0x04004810 RID: 18448
	private Coroutine boomRoutine;

	// Token: 0x04004811 RID: 18449
	[SerializeField]
	private Transform boom;

	// Token: 0x04004812 RID: 18450
	[SerializeField]
	private Transform boomMM;
}
