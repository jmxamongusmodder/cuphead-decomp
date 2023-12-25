using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000820 RID: 2080
public class TrainLevelLollipopGhoulsManager : LevelProperties.Train.Entity
{
	// Token: 0x14000055 RID: 85
	// (add) Token: 0x0600304A RID: 12362 RVA: 0x001C79C4 File Offset: 0x001C5DC4
	// (remove) Token: 0x0600304B RID: 12363 RVA: 0x001C79FC File Offset: 0x001C5DFC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TrainLevelLollipopGhoulsManager.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x0600304C RID: 12364 RVA: 0x001C7A34 File Offset: 0x001C5E34
	// (remove) Token: 0x0600304D RID: 12365 RVA: 0x001C7A6C File Offset: 0x001C5E6C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x0600304E RID: 12366 RVA: 0x001C7AA2 File Offset: 0x001C5EA2
	public void Setup()
	{
		this.cars[1].Explode(2);
	}

	// Token: 0x0600304F RID: 12367 RVA: 0x001C7AB4 File Offset: 0x001C5EB4
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.ghoulLeft.LevelInit(properties);
		this.ghoulRight.LevelInit(properties);
		this.cannons.LevelInit(properties);
		this.ghoulLeft.OnDamageTakenEvent += this.OnDamageTaken;
		this.ghoulLeft.OnDeathEvent += this.OnDeath;
		this.ghoulRight.OnDamageTakenEvent += this.OnDamageTaken;
		this.ghoulRight.OnDeathEvent += this.OnDeath;
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x001C7B48 File Offset: 0x001C5F48
	private void OnDeath()
	{
		this.deadCount++;
		if (this.deadCount > 1)
		{
			this.EndGhouls();
		}
	}

	// Token: 0x06003051 RID: 12369 RVA: 0x001C7B6A File Offset: 0x001C5F6A
	private void OnDamageTaken(float damage)
	{
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(damage);
		}
	}

	// Token: 0x06003052 RID: 12370 RVA: 0x001C7B84 File Offset: 0x001C5F84
	private IEnumerator start_cr()
	{
		AudioManager.Play("level_train_top_explode");
		this.cars[0].Explode(0);
		this.cars[2].Explode(1);
		yield return null;
		this.ghoulLeft.AnimateIn();
		this.ghoulRight.AnimateIn();
		AudioManager.Play("train_lollipop_ghoul_intro");
		this.emitAudioFromObject.Add("train_lollipop_ghoul_intro");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.lollipopGhouls.initDelay);
		base.StartCoroutine(this.ghouls_cr());
		base.StartCoroutine(this.cannons_cr());
		yield break;
	}

	// Token: 0x06003053 RID: 12371 RVA: 0x001C7B9F File Offset: 0x001C5F9F
	public void StartGhouls()
	{
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06003054 RID: 12372 RVA: 0x001C7BAE File Offset: 0x001C5FAE
	private void EndGhouls()
	{
		this.StopAllCoroutines();
		this.cannons.End();
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
	}

	// Token: 0x06003055 RID: 12373 RVA: 0x001C7BD8 File Offset: 0x001C5FD8
	private TrainLevelLollipopGhoul NextGhoul()
	{
		if (this.deadCount > 1)
		{
			return null;
		}
		if (this.ghoulRight.state == TrainLevelLollipopGhoul.State.Dead || this.ghoulRight.transform == null)
		{
			return this.ghoulLeft;
		}
		if (this.ghoulLeft.state == TrainLevelLollipopGhoul.State.Dead || this.ghoulLeft.transform == null)
		{
			return this.ghoulRight;
		}
		this.current = (int)Mathf.Repeat((float)(this.current + 1), 2f);
		int num = this.current;
		if (num == 0 || num != 1)
		{
			return this.ghoulLeft;
		}
		return this.ghoulRight;
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x001C7C90 File Offset: 0x001C6090
	private IEnumerator ghouls_cr()
	{
		this.current = UnityEngine.Random.Range(0, 2);
		for (;;)
		{
			TrainLevelLollipopGhoul ghoul = this.NextGhoul();
			yield return null;
			if (ghoul != null)
			{
				ghoul.Attack();
				while (ghoul.state == TrainLevelLollipopGhoul.State.Attacking)
				{
					yield return null;
				}
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.lollipopGhouls.mainDelay);
			}
		}
		yield break;
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x001C7CAC File Offset: 0x001C60AC
	private IEnumerator cannons_cr()
	{
		int cannon = UnityEngine.Random.Range(0, 3);
		int direction = 1;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.lollipopGhouls.cannonDelay);
			this.cannons.Shoot(cannon);
			yield return CupheadTime.WaitForSeconds(this, 1f);
			cannon += direction;
			if (cannon >= 3)
			{
				direction = -1;
				cannon = 1;
			}
			else if (cannon < 0)
			{
				cannon = 1;
				direction = 1;
			}
		}
		yield break;
	}

	// Token: 0x04003904 RID: 14596
	[SerializeField]
	private TrainLevelLollipopGhoul ghoulLeft;

	// Token: 0x04003905 RID: 14597
	[SerializeField]
	private TrainLevelLollipopGhoul ghoulRight;

	// Token: 0x04003906 RID: 14598
	[Space(10f)]
	[SerializeField]
	private TrainLevelGhostCannons cannons;

	// Token: 0x04003907 RID: 14599
	[Space(10f)]
	[SerializeField]
	private TrainLevelPassengerCar[] cars;

	// Token: 0x0400390A RID: 14602
	private int deadCount;

	// Token: 0x0400390B RID: 14603
	private int current;

	// Token: 0x02000821 RID: 2081
	// (Invoke) Token: 0x06003059 RID: 12377
	public delegate void OnDamageTakenHandler(float damage);
}
