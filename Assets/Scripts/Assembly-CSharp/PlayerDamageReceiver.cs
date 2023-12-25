using System;
using UnityEngine;

// Token: 0x02000AC4 RID: 2756
public class PlayerDamageReceiver : DamageReceiver
{
	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x0600422D RID: 16941 RVA: 0x0023C0B8 File Offset: 0x0023A4B8
	// (set) Token: 0x0600422E RID: 16942 RVA: 0x0023C0C0 File Offset: 0x0023A4C0
	public PlayerDamageReceiver.State state { get; private set; }

	// Token: 0x0600422F RID: 16943 RVA: 0x0023C0C9 File Offset: 0x0023A4C9
	protected override void Awake()
	{
		base.Awake();
		if (this.type != DamageReceiver.Type.Player)
		{
		}
		this.type = DamageReceiver.Type.Player;
		this.player = base.GetComponent<AbstractPlayerController>();
		this.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06004230 RID: 16944 RVA: 0x0023C108 File Offset: 0x0023A508
	private void Update()
	{
		if (this.state != PlayerDamageReceiver.State.Invulnerable)
		{
			return;
		}
		if (this.timer > 0f)
		{
			this.timer -= CupheadTime.Delta;
			if (this.timer <= 0f)
			{
				this.Vulnerable();
			}
		}
	}

	// Token: 0x06004231 RID: 16945 RVA: 0x0023C160 File Offset: 0x0023A560
	private void HandleChaliceShmupSuper(DamageDealer.DamageInfo info)
	{
		if (this.player.stats.State == PlayerStatsManager.PlayerState.Super && this.player.stats.isChalice && this.player.stats.Loadout.super == Super.level_super_ghost)
		{
			base.TakeDamageBruteForce(info);
			return;
		}
	}

	// Token: 0x06004232 RID: 16946 RVA: 0x0023C1C0 File Offset: 0x0023A5C0
	public override void TakeDamage(DamageDealer.DamageInfo info)
	{
		if (this.player.stats.SuperInvincible)
		{
			return;
		}
		if (info.damage > 0f)
		{
			this.HandleChaliceShmupSuper(info);
			if (!base.enabled)
			{
				return;
			}
			if (info.damageSource == DamageDealer.DamageSource.Pit)
			{
				if (this.player.damageReceiver.state != PlayerDamageReceiver.State.Vulnerable)
				{
					return;
				}
			}
			else if (!this.player.CanTakeDamage)
			{
				return;
			}
			if (this.timer > 0f)
			{
				return;
			}
			float num = 1f;
			this.Invulnerable(2f * num);
			base.TakeDamage(info);
			if (this.player.stats.ChaliceShieldOn)
			{
				this.player.stats.SetChaliceShield(false);
			}
		}
		else if (info.stoneTime > 0f)
		{
			base.TakeDamage(info);
		}
	}

	// Token: 0x06004233 RID: 16947 RVA: 0x0023C2AB File Offset: 0x0023A6AB
	public void OnRevive(Vector3 pos)
	{
		this.Invulnerable(3f);
	}

	// Token: 0x06004234 RID: 16948 RVA: 0x0023C2B8 File Offset: 0x0023A6B8
	public void Invulnerable(float time)
	{
		this.state = PlayerDamageReceiver.State.Invulnerable;
		this.timer = time;
	}

	// Token: 0x06004235 RID: 16949 RVA: 0x0023C2C8 File Offset: 0x0023A6C8
	public void Vulnerable()
	{
		this.state = PlayerDamageReceiver.State.Vulnerable;
		this.timer = 0f;
	}

	// Token: 0x06004236 RID: 16950 RVA: 0x0023C2DC File Offset: 0x0023A6DC
	public void OnDeath()
	{
		this.state = PlayerDamageReceiver.State.Other;
	}

	// Token: 0x06004237 RID: 16951 RVA: 0x0023C2E5 File Offset: 0x0023A6E5
	public void OnWin()
	{
		this.state = PlayerDamageReceiver.State.Other;
	}

	// Token: 0x040048A7 RID: 18599
	private const float TIME_HIT = 2f;

	// Token: 0x040048A8 RID: 18600
	private const float TIME_REVIVED = 3f;

	// Token: 0x040048A9 RID: 18601
	private AbstractPlayerController player;

	// Token: 0x040048AA RID: 18602
	private float timer;

	// Token: 0x02000AC5 RID: 2757
	public enum State
	{
		// Token: 0x040048AC RID: 18604
		Vulnerable,
		// Token: 0x040048AD RID: 18605
		Invulnerable,
		// Token: 0x040048AE RID: 18606
		Other
	}
}
