using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class FlowerLevel : Level
{
	// Token: 0x06000456 RID: 1110 RVA: 0x00063084 File Offset: 0x00061484
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Flower.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000457 RID: 1111 RVA: 0x0006311A File Offset: 0x0006151A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Flower;
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06000458 RID: 1112 RVA: 0x00063121 File Offset: 0x00061521
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flower;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000459 RID: 1113 RVA: 0x00063128 File Offset: 0x00061528
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Flower.States.Main:
			case LevelProperties.Flower.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Flower.States.PhaseTwo:
				return this._bossPortraitPhaseTwo;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x0600045A RID: 1114 RVA: 0x0006319C File Offset: 0x0006159C
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Flower.States.Main:
			case LevelProperties.Flower.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Flower.States.PhaseTwo:
				return this._bossQuotePhaseTwo;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0006320F File Offset: 0x0006160F
	protected override void Start()
	{
		base.Start();
		this.flower.LevelInit(this.properties);
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x00063228 File Offset: 0x00061628
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.flowerPattern_cr());
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00063238 File Offset: 0x00061638
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Flower.States.PhaseTwo)
		{
			if (Level.Current.mode == Level.Mode.Easy)
			{
				this.properties.WinInstantly();
				AudioManager.PlayLoop("flower_phase1_death_loop");
				AudioManager.Play("flower_phase1_death_scream");
			}
			else
			{
				this.StopAllCoroutines();
				this.flower.PhaseTwoTrigger();
			}
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x000632A5 File Offset: 0x000616A5
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x000632BC File Offset: 0x000616BC
	private IEnumerator flowerPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x000632D8 File Offset: 0x000616D8
	private IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.Flower.Pattern.Laser:
			yield return base.StartCoroutine(this.laserAttack_cr());
			break;
		case LevelProperties.Flower.Pattern.PodHands:
			yield return base.StartCoroutine(this.potHands_cr());
			break;
		case LevelProperties.Flower.Pattern.GattlingGun:
			yield return base.StartCoroutine(this.gattlingGun_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x000632F4 File Offset: 0x000616F4
	private IEnumerator laserAttack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.laser.hesitateAfterAttack);
		this.attacking = true;
		this.flower.StartLaser(new Action(this.OnLaserAttackComplete));
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0006330F File Offset: 0x0006170F
	private void OnLaserAttackComplete()
	{
		this.attacking = false;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00063318 File Offset: 0x00061718
	private IEnumerator potHands_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.podHands.hesitateAfterAttack);
		this.attacking = true;
		this.flower.StartPotHands(new Action(this.OnPotHandsAttackComplete));
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00063333 File Offset: 0x00061733
	private void OnPotHandsAttackComplete()
	{
		this.attacking = false;
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0006333C File Offset: 0x0006173C
	private IEnumerator gattlingGun_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.gattlingGun.hesitateAfterAttack);
		this.attacking = true;
		this.flower.StartGattlingGun(new Action(this.OnGattlingGunComplete));
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00063357 File Offset: 0x00061757
	private void OnGattlingGunComplete()
	{
		this.attacking = false;
	}

	// Token: 0x04000757 RID: 1879
	private LevelProperties.Flower properties;

	// Token: 0x04000758 RID: 1880
	[SerializeField]
	public FlowerLevelFlower flower;

	// Token: 0x04000759 RID: 1881
	private bool attacking;

	// Token: 0x0400075A RID: 1882
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x0400075B RID: 1883
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;

	// Token: 0x0400075C RID: 1884
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x0400075D RID: 1885
	[SerializeField]
	private string _bossQuotePhaseTwo;

	// Token: 0x02000603 RID: 1539
	[Serializable]
	public class Prefabs
	{
	}
}
