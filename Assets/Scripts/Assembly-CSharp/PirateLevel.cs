using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200023B RID: 571
public class PirateLevel : Level
{
	// Token: 0x06000667 RID: 1639 RVA: 0x0006F88C File Offset: 0x0006DC8C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Pirate.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000668 RID: 1640 RVA: 0x0006F922 File Offset: 0x0006DD22
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Pirate;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000669 RID: 1641 RVA: 0x0006F925 File Offset: 0x0006DD25
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_pirate;
		}
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600066A RID: 1642 RVA: 0x0006F92C File Offset: 0x0006DD2C
	// (remove) Token: 0x0600066B RID: 1643 RVA: 0x0006F964 File Offset: 0x0006DD64
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PirateLevel.WhistleDelegate OnWhistleEvent;

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x0600066C RID: 1644 RVA: 0x0006F99C File Offset: 0x0006DD9C
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Pirate.States.Main:
			case LevelProperties.Pirate.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Pirate.States.Boat:
				return this._bossPortraitBoat;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x0600066D RID: 1645 RVA: 0x0006FA10 File Offset: 0x0006DE10
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Pirate.States.Main:
			case LevelProperties.Pirate.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Pirate.States.Boat:
				return this._bossQuoteBoat;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0006FA83 File Offset: 0x0006DE83
	protected override void Awake()
	{
		base.Awake();
		this.inkOverlay = this.prefabs.inkOverlay.InstantiatePrefab<PirateLevelSquidInkOverlay>();
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0006FAA4 File Offset: 0x0006DEA4
	protected override void Start()
	{
		base.Start();
		this.barrel.LevelInit(this.properties);
		this.inkOverlay.LevelInit(this.properties);
		this.pirate.LevelInit(this.properties);
		this.boat.LevelInit(this.properties);
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0006FAFB File Offset: 0x0006DEFB
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.piratePattern_cr());
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0006FB0A File Offset: 0x0006DF0A
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0006FB12 File Offset: 0x0006DF12
	private void StartBoat()
	{
		this.StopAllCoroutines();
		this.boat.OnLaunchPirate += this.OnBoatLaunchPirate;
		this.boat.StartTransformation();
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0006FB3C File Offset: 0x0006DF3C
	private void OnBoatLaunchPirate()
	{
		base.StartCoroutine(this.launchPirate_cr());
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0006FB4B File Offset: 0x0006DF4B
	private void Whistle(PirateLevel.Creature creature)
	{
		if (this.OnWhistleEvent != null)
		{
			this.OnWhistleEvent(creature);
		}
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0006FB64 File Offset: 0x0006DF64
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.prefabs = null;
		this._bossPortraitBoat = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0006FB84 File Offset: 0x0006DF84
	private IEnumerator piratePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0006FBA0 File Offset: 0x0006DFA0
	private IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.Pirate.Pattern.Shark:
			yield return base.StartCoroutine(this.shark_cr());
			break;
		case LevelProperties.Pirate.Pattern.Squid:
			yield return base.StartCoroutine(this.squid_cr());
			break;
		case LevelProperties.Pirate.Pattern.DogFish:
			yield return base.StartCoroutine(this.dogFish_cr());
			break;
		case LevelProperties.Pirate.Pattern.Peashot:
			yield return base.StartCoroutine(this.peashot_cr());
			break;
		case LevelProperties.Pirate.Pattern.Boat:
			this.StartBoat();
			break;
		default:
			yield return new WaitForSeconds(1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0006FBBC File Offset: 0x0006DFBC
	private IEnumerator squid_cr()
	{
		this.Whistle(PirateLevel.Creature.Squid);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.squid.startDelay);
		PirateLevelSquid squid = this.prefabs.squid.InstantiatePrefab<PirateLevelSquid>();
		squid.LevelInit(this.properties);
		while (squid.state != PirateLevelSquid.State.Exit && squid.state != PirateLevelSquid.State.Die)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, (float)this.properties.CurrentState.squid.endDelay);
		yield break;
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0006FBD8 File Offset: 0x0006DFD8
	private IEnumerator shark_cr()
	{
		this.Whistle(PirateLevel.Creature.Shark);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.shark.startDelay);
		PirateLevelShark shark = this.prefabs.shark.InstantiatePrefab<PirateLevelShark>();
		shark.LevelInitWithGroup(this.properties.CurrentState.shark);
		while (shark.state != PirateLevelShark.State.Complete)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.shark.endDelay);
		yield break;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0006FBF4 File Offset: 0x0006DFF4
	private IEnumerator dogFish_cr()
	{
		bool secretHitBox = false;
		this.Whistle(PirateLevel.Creature.DogFish);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.scope.In();
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.dogFish.startDelay);
		LevelProperties.Pirate.DogFish properties = this.properties.CurrentState.dogFish;
		for (int i = 0; i < properties.count; i++)
		{
			secretHitBox = (i == 3);
			PirateLevelDogFish dogFish = this.prefabs.dogFish.InstantiatePrefab<PirateLevelDogFish>();
			dogFish.transform.SetPosition(new float?(0f), new float?(-210f), new float?(0f));
			dogFish.Init(this.properties, secretHitBox);
			yield return CupheadTime.WaitForSeconds(this, properties.nextFishDelay);
		}
		yield return CupheadTime.WaitForSeconds(this, properties.endDelay);
		yield break;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0006FC10 File Offset: 0x0006E010
	private IEnumerator peashot_cr()
	{
		LevelProperties.Pirate.Peashot properties = this.properties.CurrentState.peashot;
		KeyValue[] pattern = KeyValue.ListFromString(properties.patterns[UnityEngine.Random.Range(0, properties.patterns.Length)], new char[]
		{
			'P',
			'D'
		});
		this.pirate.StartGun();
		yield return CupheadTime.WaitForSeconds(this, properties.startDelay);
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "P")
			{
				int p = 0;
				while ((float)p < pattern[i].value)
				{
					yield return CupheadTime.WaitForSeconds(this, properties.shotDelay);
					this.pirate.FireGun(properties);
					p++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
			yield return null;
		}
		this.pirate.EndGun();
		yield return CupheadTime.WaitForSeconds(this, (float)properties.endDelay);
		yield break;
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x0006FC2C File Offset: 0x0006E02C
	private IEnumerator launchPirate_cr()
	{
		LevelProperties.Pirate.Boat p = this.properties.CurrentState.boat;
		this.deadPirate.Go(p.pirateFallDelay, p.pirateFallTime);
		float t = 0f;
		float time = 1f;
		float speed = 1200f;
		while (t < time)
		{
			float y = speed * CupheadTime.Delta;
			foreach (Transform transform in this.boatParts)
			{
				transform.AddPosition(0f, y, 0f);
			}
			this.pirate.transform.AddPosition(0f, y, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		foreach (Transform transform2 in this.boatParts)
		{
			UnityEngine.Object.Destroy(transform2.gameObject);
		}
		this.pirate.CleanUp();
		yield break;
	}

	// Token: 0x04000C29 RID: 3113
	private LevelProperties.Pirate properties;

	// Token: 0x04000C2A RID: 3114
	private const float WHISTLE_ANIM_TIME = 1.5f;

	// Token: 0x04000C2C RID: 3116
	[Space(10f)]
	public PirateLevelPirate pirate;

	// Token: 0x04000C2D RID: 3117
	public PirateLevelPirateDead deadPirate;

	// Token: 0x04000C2E RID: 3118
	public PirateLevelBoat boat;

	// Token: 0x04000C2F RID: 3119
	public PirateLevelBarrel barrel;

	// Token: 0x04000C30 RID: 3120
	public PirateLevelDogFishScope scope;

	// Token: 0x04000C31 RID: 3121
	public Transform[] boatParts;

	// Token: 0x04000C32 RID: 3122
	[Space(10f)]
	[SerializeField]
	private PirateLevel.Prefabs prefabs;

	// Token: 0x04000C33 RID: 3123
	private PirateLevelSquidInkOverlay inkOverlay;

	// Token: 0x04000C34 RID: 3124
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000C35 RID: 3125
	[SerializeField]
	private Sprite _bossPortraitBoat;

	// Token: 0x04000C36 RID: 3126
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000C37 RID: 3127
	[SerializeField]
	private string _bossQuoteBoat;

	// Token: 0x02000731 RID: 1841
	public enum Creature
	{
		// Token: 0x040030E3 RID: 12515
		Squid,
		// Token: 0x040030E4 RID: 12516
		Shark,
		// Token: 0x040030E5 RID: 12517
		DogFish
	}

	// Token: 0x02000732 RID: 1842
	// (Invoke) Token: 0x06002825 RID: 10277
	public delegate void WhistleDelegate(PirateLevel.Creature creature);

	// Token: 0x02000733 RID: 1843
	[Serializable]
	public class Prefabs
	{
		// Token: 0x040030E6 RID: 12518
		public PirateLevelSquid squid;

		// Token: 0x040030E7 RID: 12519
		public PirateLevelShark shark;

		// Token: 0x040030E8 RID: 12520
		public PirateLevelDogFish dogFish;

		// Token: 0x040030E9 RID: 12521
		[Space(10f)]
		public PirateLevelSquidInkOverlay inkOverlay;
	}
}
