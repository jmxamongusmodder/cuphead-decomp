using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DC RID: 732
public class TrainLevel : Level
{
	// Token: 0x0600081A RID: 2074 RVA: 0x00078094 File Offset: 0x00076494
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Train.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x0600081B RID: 2075 RVA: 0x0007812A File Offset: 0x0007652A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Train;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x0600081C RID: 2076 RVA: 0x0007812D File Offset: 0x0007652D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_train;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600081D RID: 2077 RVA: 0x00078134 File Offset: 0x00076534
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.currentPhase)
			{
			case 1:
				return this._bossPortraitSpecter;
			case 2:
				return this._bossPortraitSkeleton;
			case 3:
				return this._bossPortraitLollipop;
			case 4:
				return this._bossPortraitEngine;
			default:
				global::Debug.LogError("Couldn't find portrait for phase " + this.currentPhase + ". Using Main.", null);
				return this._bossPortraitEngine;
			}
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x0600081E RID: 2078 RVA: 0x000781A8 File Offset: 0x000765A8
	public override string BossQuote
	{
		get
		{
			switch (this.currentPhase)
			{
			case 1:
				return this._bossQuoteSpecter;
			case 2:
				return this._bossQuoteSkeleton;
			case 3:
				return this._bossQuoteLollipop;
			case 4:
				return this._bossQuoteEngine;
			default:
				global::Debug.LogError("Couldn't find portrait for phase " + this.currentPhase + ". Using Main.", null);
				return this._bossQuoteEngine;
			}
		}
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0007821C File Offset: 0x0007661C
	protected override void Start()
	{
		base.Start();
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		base.timeline.health += (float)this.properties.CurrentState.blindSpecter.health;
		base.timeline.health += this.properties.CurrentState.skeleton.health;
		base.timeline.health += this.properties.CurrentState.lollipopGhouls.health * 2f;
		base.timeline.health += this.properties.CurrentState.engine.health;
		base.timeline.AddEvent(new Level.Timeline.Event("Skeleton", 1f - (float)this.properties.CurrentState.blindSpecter.health / base.timeline.health));
		base.timeline.AddEvent(new Level.Timeline.Event("Lollipop Ghouls", 1f - ((float)this.properties.CurrentState.blindSpecter.health + this.properties.CurrentState.skeleton.health) / base.timeline.health));
		base.timeline.AddEvent(new Level.Timeline.Event("Engine", 1f - ((float)this.properties.CurrentState.blindSpecter.health + this.properties.CurrentState.skeleton.health + this.properties.CurrentState.lollipopGhouls.health * 2f) / base.timeline.health));
		this.train.LevelInit(this.properties);
		this.blindSpecter.LevelInit(this.properties);
		this.skeleton.LevelInit(this.properties);
		this.ghouls.LevelInit(this.properties);
		this.engine.LevelInit(this.properties);
		this.blindSpecter.OnDeathEvent += this.OnBlindSpecterDeath;
		this.skeleton.OnDeathEvent += this.OnSkeletonDeath;
		this.ghouls.OnDeathEvent += this.OnLollipopsDeath;
		this.engine.OnDeathEvent += this.OnEngineDeath;
		this.blindSpecter.OnDamageTakenEvent += base.timeline.DealDamage;
		this.skeleton.OnDamageTakenEvent += base.timeline.DealDamage;
		this.ghouls.OnDamageTakenEvent += base.timeline.DealDamage;
		this.engine.OnDamageTakenEvent += base.timeline.DealDamage;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00078529 File Offset: 0x00076929
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.pumpkinPrefab = null;
		this._bossPortraitEngine = null;
		this._bossPortraitLollipop = null;
		this._bossPortraitSkeleton = null;
		this._bossPortraitSpecter = null;
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00078554 File Offset: 0x00076954
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.pumpkins_cr());
		base.StartCoroutine(this.trainPattern_cr());
		this.setPhase(1);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00078577 File Offset: 0x00076977
	private void OnBlindSpecterDeath()
	{
		this.train.OnBlindSpectreDeath();
		this.setPhase(2);
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0007858B File Offset: 0x0007698B
	private void OnSkeletonDeath()
	{
		this.train.OnSkeletonDeath();
		this.setPhase(3);
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0007859F File Offset: 0x0007699F
	private void OnLollipopsDeath()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.properties.WinInstantly();
		}
		else
		{
			this.train.OnLollipopsDeath();
			this.setPhase(4);
		}
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x000785D2 File Offset: 0x000769D2
	private void OnEngineDeath()
	{
		this.properties.WinInstantly();
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x000785E0 File Offset: 0x000769E0
	private void setPhase(int phase)
	{
		this.currentPhase = phase;
		foreach (string s in this.properties.CurrentState.pumpkins.bossPhaseOn.Split(new char[]
		{
			','
		}))
		{
			int num = 0;
			Parser.IntTryParse(s, out num);
			if (num == phase)
			{
				this.pumpkinsEnabled = true;
				return;
			}
		}
		this.pumpkinsEnabled = false;
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00078654 File Offset: 0x00076A54
	private IEnumerator trainPattern_cr()
	{
		yield return new WaitForSeconds(1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00078670 File Offset: 0x00076A70
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Train.Pattern p = this.properties.CurrentState.NextPattern;
		yield return new WaitForSeconds(1f);
		yield break;
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0007868C File Offset: 0x00076A8C
	private IEnumerator pumpkins_cr()
	{
		int dir = (!Rand.Bool()) ? -1 : 1;
		Transform target = this.rightValve;
		LevelProperties.Train.Pumpkins p = this.properties.CurrentState.pumpkins;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.delay);
			if (this.pumpkinsEnabled)
			{
				this.pumpkinPrefab.Create(new Vector2((float)(840 * -(float)dir), 280f), dir, p.speed, p.health, p.fallTime, target);
				dir *= -1;
				if (this.train.state != TrainLevelTrain.State.BlindSpecter)
				{
					if (dir < 0)
					{
						target = this.rightValve;
					}
					else
					{
						target = this.leftValve;
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x04001087 RID: 4231
	private LevelProperties.Train properties;

	// Token: 0x04001088 RID: 4232
	[SerializeField]
	private TrainLevelTrain train;

	// Token: 0x04001089 RID: 4233
	[Space(10f)]
	[SerializeField]
	private TrainLevelPumpkin pumpkinPrefab;

	// Token: 0x0400108A RID: 4234
	[SerializeField]
	private Transform leftValve;

	// Token: 0x0400108B RID: 4235
	[SerializeField]
	private Transform rightValve;

	// Token: 0x0400108C RID: 4236
	[Space(10f)]
	[SerializeField]
	private TrainLevelBlindSpecter blindSpecter;

	// Token: 0x0400108D RID: 4237
	[SerializeField]
	private TrainLevelSkeleton skeleton;

	// Token: 0x0400108E RID: 4238
	[SerializeField]
	private TrainLevelLollipopGhoulsManager ghouls;

	// Token: 0x0400108F RID: 4239
	[SerializeField]
	private TrainLevelEngineBoss engine;

	// Token: 0x04001090 RID: 4240
	public Collider2D handCarCollider;

	// Token: 0x04001091 RID: 4241
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitSpecter;

	// Token: 0x04001092 RID: 4242
	[SerializeField]
	private Sprite _bossPortraitSkeleton;

	// Token: 0x04001093 RID: 4243
	[SerializeField]
	private Sprite _bossPortraitLollipop;

	// Token: 0x04001094 RID: 4244
	[SerializeField]
	private Sprite _bossPortraitEngine;

	// Token: 0x04001095 RID: 4245
	[SerializeField]
	private string _bossQuoteSpecter;

	// Token: 0x04001096 RID: 4246
	[SerializeField]
	private string _bossQuoteSkeleton;

	// Token: 0x04001097 RID: 4247
	[SerializeField]
	private string _bossQuoteLollipop;

	// Token: 0x04001098 RID: 4248
	[SerializeField]
	private string _bossQuoteEngine;

	// Token: 0x04001099 RID: 4249
	private bool pumpkinsEnabled;

	// Token: 0x0400109A RID: 4250
	private int currentPhase;

	// Token: 0x02000809 RID: 2057
	[Serializable]
	public class Prefabs
	{
	}
}
