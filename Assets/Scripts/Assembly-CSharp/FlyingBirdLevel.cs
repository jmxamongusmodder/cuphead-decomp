using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class FlyingBirdLevel : Level
{
	// Token: 0x0600047D RID: 1149 RVA: 0x000638EC File Offset: 0x00061CEC
	protected override void PartialInit()
	{
		this.properties = LevelProperties.FlyingBird.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x0600047E RID: 1150 RVA: 0x00063982 File Offset: 0x00061D82
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingBird;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x0600047F RID: 1151 RVA: 0x00063989 File Offset: 0x00061D89
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_bird;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000480 RID: 1152 RVA: 0x00063990 File Offset: 0x00061D90
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingBird.States.Main:
			case LevelProperties.FlyingBird.States.Generic:
			case LevelProperties.FlyingBird.States.Whistle:
				return this._bossPortraitMain;
			case LevelProperties.FlyingBird.States.HouseDeath:
				return this._bossPortraitHouseDeath;
			case LevelProperties.FlyingBird.States.BirdRevival:
				return this._bossPortraitBirdRevival;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000481 RID: 1153 RVA: 0x00063A14 File Offset: 0x00061E14
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingBird.States.Main:
			case LevelProperties.FlyingBird.States.Generic:
			case LevelProperties.FlyingBird.States.Whistle:
				return this._bossQuoteMain;
			case LevelProperties.FlyingBird.States.HouseDeath:
				return this._bossQuoteHouseDeath;
			case LevelProperties.FlyingBird.States.BirdRevival:
				return this._bossQuoteBirdRevival;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00063A96 File Offset: 0x00061E96
	protected override void Start()
	{
		base.Start();
		this.bird.LevelInit(this.properties);
		this.smallBird.LevelInit(this.properties);
		this.skybirdPattern = this.skybirdPattern_cr();
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00063ACC File Offset: 0x00061ECC
	protected override void OnLevelStart()
	{
		this.bird.IntroContinue();
		base.StartCoroutine(this.skybirdPattern_cr());
		base.StartCoroutine(this.enemies_cr());
		base.StartCoroutine(this.turrets_cr());
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00063B00 File Offset: 0x00061F00
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingBird.States.HouseDeath)
		{
			base.StopCoroutine(this.skybirdPattern);
			base.StartCoroutine(this.houseDie_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBird.States.BirdRevival)
		{
			base.StopCoroutine(this.skybirdPattern);
			base.StartCoroutine(this.birdHouseRevival_cr());
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00063B76 File Offset: 0x00061F76
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitBirdRevival = null;
		this._bossPortraitHouseDeath = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00063B94 File Offset: 0x00061F94
	private IEnumerator skybirdPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00063BB0 File Offset: 0x00061FB0
	private IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.FlyingBird.Pattern.Feathers:
			yield return base.StartCoroutine(this.feathers_cr());
			break;
		case LevelProperties.FlyingBird.Pattern.Eggs:
			yield return base.StartCoroutine(this.eggs_cr());
			break;
		case LevelProperties.FlyingBird.Pattern.Lasers:
			yield return base.StartCoroutine(this.lasers_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		case LevelProperties.FlyingBird.Pattern.Garbage:
			yield return base.StartCoroutine(this.garbage_cr());
			break;
		case LevelProperties.FlyingBird.Pattern.Heart:
			yield return base.StartCoroutine(this.heartAttack_cr());
			break;
		}
		yield break;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00063BCC File Offset: 0x00061FCC
	private IEnumerator feathers_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		this.bird.StartFeathers();
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00063BE8 File Offset: 0x00061FE8
	private IEnumerator eggs_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		this.bird.StartEggs();
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00063C04 File Offset: 0x00062004
	private IEnumerator lasers_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		this.bird.StartLasers();
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00063C20 File Offset: 0x00062020
	private IEnumerator houseDie_cr()
	{
		this.bird.BirdFall();
		yield return null;
		yield break;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00063C3C File Offset: 0x0006203C
	private IEnumerator enemies_cr()
	{
		bool firstTime = true;
		AbstractPlayerController target = PlayerManager.GetNext();
		int r = 1;
		for (;;)
		{
			if (!this.properties.CurrentState.enemies.active)
			{
				firstTime = true;
				while (!this.properties.CurrentState.enemies.active)
				{
					yield return null;
				}
			}
			LevelProperties.FlyingBird.Enemies properties = this.properties.CurrentState.enemies;
			int i = 0;
			FlyingBirdLevelEnemy.Properties p = new FlyingBirdLevelEnemy.Properties(properties.health, properties.speed, properties.floatRange, properties.floatTime, properties.projectileHeight, properties.projectileFallTime, properties.projectileDelay);
			target = PlayerManager.GetNext();
			Vector2 pos = this.enemyRoot.position;
			if (!this.properties.CurrentState.enemies.aim)
			{
				pos.y *= (float)r;
				r *= -1;
			}
			else
			{
				pos.y = target.center.y;
			}
			while (i < properties.count)
			{
				yield return CupheadTime.WaitForSeconds(this, properties.delay);
				bool parryable = i == properties.count - 1;
				this.prefabs.formationBird.Create(pos, p).SetParryable(parryable);
				i++;
			}
			yield return CupheadTime.WaitForSeconds(this, firstTime ? properties.initalGroupDelay : properties.groupDelay);
			firstTime = false;
		}
		yield break;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00063C58 File Offset: 0x00062058
	private IEnumerator turrets_cr()
	{
		FlyingBirdLevelTurret top = null;
		FlyingBirdLevelTurret bottom = null;
		for (;;)
		{
			if (!this.properties.CurrentState.turrets.active)
			{
				while (!this.properties.CurrentState.turrets.active)
				{
					yield return null;
				}
			}
			if (top == null || top.transform == null || top.state == FlyingBirdLevelTurret.State.Respawn)
			{
				top = this.CreateTurret(this.turretRootTop.position);
			}
			if (bottom == null || bottom.transform == null || bottom.state == FlyingBirdLevelTurret.State.Respawn)
			{
				bottom = this.CreateTurret(this.turretRootBottom.position);
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.turrets.respawnDelay);
		}
		yield break;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00063C74 File Offset: 0x00062074
	private IEnumerator birdHouseRevival_cr()
	{
		while (this.smallBird.isActiveAndEnabled)
		{
			yield return null;
		}
		this.bird.OnBossRevival();
		while (this.bird.state == FlyingBirdLevelBird.State.Reviving)
		{
			yield return null;
		}
		base.StartCoroutine(this.skybirdPattern_cr());
		yield break;
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00063C90 File Offset: 0x00062090
	private IEnumerator garbage_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Revived)
		{
			yield return null;
		}
		this.bird.StartGarbageOne();
		while (this.bird.state != FlyingBirdLevelBird.State.Revived)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00063CAC File Offset: 0x000620AC
	private IEnumerator heartAttack_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Revived)
		{
			yield return null;
		}
		this.bird.StartHeartAttack();
		while (this.bird.state != FlyingBirdLevelBird.State.Revived)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00063CC8 File Offset: 0x000620C8
	private FlyingBirdLevelTurret CreateTurret(Vector2 pos)
	{
		FlyingBirdLevelTurret.Properties properties = new FlyingBirdLevelTurret.Properties((float)this.properties.CurrentState.turrets.health, this.properties.CurrentState.turrets.inTime, pos.x, this.properties.CurrentState.turrets.bulletSpeed, this.properties.CurrentState.turrets.bulletDelay, this.properties.CurrentState.turrets.floatRange, this.properties.CurrentState.turrets.floatTime);
		return this.prefabs.turretBird.Create(new Vector2(690f, pos.y), properties);
	}

	// Token: 0x040007C4 RID: 1988
	private LevelProperties.FlyingBird properties;

	// Token: 0x040007C5 RID: 1989
	[SerializeField]
	private FlyingBirdLevelBird bird;

	// Token: 0x040007C6 RID: 1990
	[SerializeField]
	private FlyingBirdLevelSmallBird smallBird;

	// Token: 0x040007C7 RID: 1991
	[Space(10f)]
	[SerializeField]
	private Transform enemyRoot;

	// Token: 0x040007C8 RID: 1992
	[Space(10f)]
	[SerializeField]
	private Transform turretRootTop;

	// Token: 0x040007C9 RID: 1993
	[SerializeField]
	private Transform turretRootBottom;

	// Token: 0x040007CA RID: 1994
	[Space(10f)]
	[SerializeField]
	private FlyingBirdLevel.Prefabs prefabs;

	// Token: 0x040007CB RID: 1995
	private IEnumerator skybirdPattern;

	// Token: 0x040007CC RID: 1996
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x040007CD RID: 1997
	[SerializeField]
	private Sprite _bossPortraitHouseDeath;

	// Token: 0x040007CE RID: 1998
	[SerializeField]
	private Sprite _bossPortraitBirdRevival;

	// Token: 0x040007CF RID: 1999
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x040007D0 RID: 2000
	[SerializeField]
	private string _bossQuoteHouseDeath;

	// Token: 0x040007D1 RID: 2001
	[SerializeField]
	private string _bossQuoteBirdRevival;

	// Token: 0x02000615 RID: 1557
	[Serializable]
	public class Prefabs
	{
		// Token: 0x0400280B RID: 10251
		public FlyingBirdLevelEnemy formationBird;

		// Token: 0x0400280C RID: 10252
		public FlyingBirdLevelTurret turretBird;
	}
}
