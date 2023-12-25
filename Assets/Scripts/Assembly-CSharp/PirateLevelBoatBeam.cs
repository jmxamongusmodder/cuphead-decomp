using System;
using UnityEngine;

// Token: 0x0200071D RID: 1821
public class PirateLevelBoatBeam : ParrySwitch
{
	// Token: 0x060027A3 RID: 10147 RVA: 0x00173EDC File Offset: 0x001722DC
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = new DamageDealer(1f, 0.1f, DamageDealer.DamageSource.Enemy, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Left, base.transform);
	}

	// Token: 0x060027A4 RID: 10148 RVA: 0x00173F0F File Offset: 0x0017230F
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x060027A5 RID: 10149 RVA: 0x00173F1C File Offset: 0x0017231C
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Exit)
		{
			return;
		}
		LevelPlayerController component = hit.GetComponent<LevelPlayerController>();
		if (component == null || component.Ducking)
		{
			return;
		}
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x00173F60 File Offset: 0x00172360
	public PirateLevelBoatBeam Create(Transform parent)
	{
		PirateLevelBoatBeam pirateLevelBoatBeam = this.InstantiatePrefab<PirateLevelBoatBeam>();
		pirateLevelBoatBeam.Init(parent);
		return pirateLevelBoatBeam;
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x00173F7C File Offset: 0x0017237C
	private void Init(Transform parent)
	{
		AudioManager.Play("level_pirate_ship_beam_fire");
		base.transform.SetParent(parent);
		base.transform.ResetLocalPosition();
		base.transform.ResetLocalRotation();
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x00173FAA File Offset: 0x001723AA
	public void StartBeam()
	{
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x00173FAC File Offset: 0x001723AC
	public void EndBeam()
	{
		base.animator.SetTrigger("OnEnd");
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x00173FBE File Offset: 0x001723BE
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x00173FD2 File Offset: 0x001723D2
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.StartParryCooldown();
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x00173FE1 File Offset: 0x001723E1
	private void OnEndAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400306A RID: 12394
	private DamageDealer damageDealer;
}
