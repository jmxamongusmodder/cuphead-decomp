using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class BeeLevel : Level
{
	// Token: 0x0600010A RID: 266 RVA: 0x00056118 File Offset: 0x00054518
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Bee.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600010B RID: 267 RVA: 0x000561AE File Offset: 0x000545AE
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Bee;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x0600010C RID: 268 RVA: 0x000561B5 File Offset: 0x000545B5
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_bee;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x0600010D RID: 269 RVA: 0x000561B9 File Offset: 0x000545B9
	public float Speed
	{
		get
		{
			return this.speed * CupheadTime.GlobalSpeed;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x0600010E RID: 270 RVA: 0x000561C7 File Offset: 0x000545C7
	public int MissingPlatformCount
	{
		get
		{
			return this.missingPlatformCount;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x0600010F RID: 271 RVA: 0x000561D0 File Offset: 0x000545D0
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Bee.States.Main:
				return this._bossPortraitGuard;
			case LevelProperties.Bee.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Bee.States.Airplane:
				return this._bossPortraitAirplane;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000110 RID: 272 RVA: 0x0005624C File Offset: 0x0005464C
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Bee.States.Main:
				return this._bossQuoteGuard;
			case LevelProperties.Bee.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Bee.States.Airplane:
				return this._bossQuoteAirplane;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000562C8 File Offset: 0x000546C8
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.drip_cr());
		this.queen.LevelInit(this.properties);
		this.guard.LevelInit(this.properties);
		this.background.LevelInit(this.properties);
		this.airplane.LevelInit(this.properties);
	}

	// Token: 0x06000112 RID: 274 RVA: 0x0005632C File Offset: 0x0005472C
	protected override void Update()
	{
		base.Update();
		this.UpdateSpeed();
	}

	// Token: 0x06000113 RID: 275 RVA: 0x0005633C File Offset: 0x0005473C
	protected override void CreatePlayers()
	{
		base.CreatePlayers();
		if (PlayerManager.Multiplayer && this.allowMultiplayer && this.players[1].stats.isChalice)
		{
			this.players[1].transform.position = this.p2ChaliceSpawnPoint;
		}
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00056398 File Offset: 0x00054798
	protected override void OnLevelStart()
	{
		this.missingPlatformCount = this.properties.CurrentState.movement.missingPlatforms;
		this.targetSpeed = -this.properties.CurrentState.movement.speed;
		base.StartCoroutine(this.beePattern_cr());
		this.CheckGrunts();
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000563EF File Offset: 0x000547EF
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Bee.States.Airplane)
		{
			base.StartCoroutine(this.airplane_cr());
		}
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0005641A File Offset: 0x0005481A
	private void UpdateSpeed()
	{
		this.speed = Mathf.Lerp(this.speed, this.targetSpeed, 0.5f * CupheadTime.Delta);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00056443 File Offset: 0x00054843
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.prefabs = null;
		this._bossPortraitAirplane = null;
		this._bossPortraitGuard = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00056468 File Offset: 0x00054868
	private void CheckGrunts()
	{
		if (this.gruntCoroutine != null)
		{
			base.StopCoroutine(this.gruntCoroutine);
		}
		if (this.properties.CurrentState.grunts.active)
		{
			this.gruntCoroutine = base.StartCoroutine(this.grunts_cr());
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x000564B8 File Offset: 0x000548B8
	private IEnumerator beePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x000564D4 File Offset: 0x000548D4
	private IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.Bee.Pattern.BlackHole:
			yield return base.StartCoroutine(this.blackHole_cr());
			break;
		case LevelProperties.Bee.Pattern.Chain:
			yield return base.StartCoroutine(this.chain_cr());
			break;
		case LevelProperties.Bee.Pattern.Triangle:
			yield return base.StartCoroutine(this.triangle_cr());
			break;
		case LevelProperties.Bee.Pattern.Follower:
			yield return base.StartCoroutine(this.follower_cr());
			break;
		case LevelProperties.Bee.Pattern.SecurityGuard:
			yield return base.StartCoroutine(this.security_cr());
			break;
		case LevelProperties.Bee.Pattern.Wing:
			yield return base.StartCoroutine(this.wing_cr());
			break;
		case LevelProperties.Bee.Pattern.Turbine:
			yield return base.StartCoroutine(this.turbine_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000564F0 File Offset: 0x000548F0
	private IEnumerator airplane_cr()
	{
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		this.queen.StartMorph();
		this.honeyDripping = false;
		this.targetSpeed = -this.properties.CurrentState.general.screenScrollSpeed;
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0005650C File Offset: 0x0005490C
	private IEnumerator turbine_cr()
	{
		while (this.airplane.state != BeeLevelAirplane.State.Idle)
		{
			yield return null;
		}
		this.airplane.StartTurbine();
		while (this.airplane.state != BeeLevelAirplane.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00056528 File Offset: 0x00054928
	private IEnumerator wing_cr()
	{
		while (this.airplane.state != BeeLevelAirplane.State.Idle)
		{
			yield return null;
		}
		this.airplane.StartWing();
		while (this.airplane.state != BeeLevelAirplane.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00056544 File Offset: 0x00054944
	private IEnumerator security_cr()
	{
		this.guard.StartSecurityGuard();
		while (this.guard.state != BeeLevelSecurityGuard.State.Ready)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00056560 File Offset: 0x00054960
	private IEnumerator blackHole_cr()
	{
		this.queen.StartBlackHole();
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0005657C File Offset: 0x0005497C
	private IEnumerator triangle_cr()
	{
		this.queen.StartTriangle();
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00056598 File Offset: 0x00054998
	private IEnumerator follower_cr()
	{
		this.queen.StartFollower();
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x000565B4 File Offset: 0x000549B4
	private IEnumerator chain_cr()
	{
		this.queen.StartChain();
		while (this.queen.state != BeeLevelQueen.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000565D0 File Offset: 0x000549D0
	private IEnumerator drip_cr()
	{
		while (this.honeyDripping)
		{
			yield return CupheadTime.WaitForSeconds(this, (float)UnityEngine.Random.Range(1, 3));
			this.prefabs.drip.Create();
		}
		yield break;
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000565EC File Offset: 0x000549EC
	private IEnumerator grunts_cr()
	{
		string[] strings = this.properties.CurrentState.grunts.entrancePoints[UnityEngine.Random.Range(0, this.properties.CurrentState.grunts.entrancePoints.Length)].Split(new char[]
		{
			','
		});
		int[] positions = new int[strings.Length];
		for (int i = 0; i < strings.Length; i++)
		{
			Parser.IntTryParse(strings[i], out positions[i]);
			positions[i] = Mathf.Clamp(positions[i], 0, this.gruntRoots.Length);
		}
		int index = UnityEngine.Random.Range(0, positions.Length);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.grunts.delay);
			int scale = (PlayerManager.Center.x <= 0f) ? 1 : -1;
			if (PlayerManager.Center.x > 0f)
			{
				scale = -1;
			}
			this.prefabs.grunt.Create(this.gruntRoots[positions[index]].position + new Vector3((float)(840 * scale), 0f, 0f), scale, this.properties.CurrentState.grunts.health, this.properties.CurrentState.grunts.speed);
			index = (int)Mathf.Repeat((float)(index + 1), (float)positions.Length);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000276 RID: 630
	private LevelProperties.Bee properties;

	// Token: 0x04000277 RID: 631
	private const float SPEED_TIME = 0.5f;

	// Token: 0x04000278 RID: 632
	[SerializeField]
	private Vector2 p2ChaliceSpawnPoint;

	// Token: 0x04000279 RID: 633
	[SerializeField]
	private BeeLevelAirplane airplane;

	// Token: 0x0400027A RID: 634
	[Space(10f)]
	[SerializeField]
	private BeeLevelQueen queen;

	// Token: 0x0400027B RID: 635
	[SerializeField]
	private BeeLevelSecurityGuard guard;

	// Token: 0x0400027C RID: 636
	[Space(10f)]
	[SerializeField]
	private Transform[] gruntRoots;

	// Token: 0x0400027D RID: 637
	[Space(10f)]
	[SerializeField]
	private BeeLevelBackground background;

	// Token: 0x0400027E RID: 638
	[Space(10f)]
	[SerializeField]
	private BeeLevel.Prefabs prefabs;

	// Token: 0x0400027F RID: 639
	private bool honeyDripping = true;

	// Token: 0x04000280 RID: 640
	private float speed;

	// Token: 0x04000281 RID: 641
	private float targetSpeed;

	// Token: 0x04000282 RID: 642
	private int missingPlatformCount;

	// Token: 0x04000283 RID: 643
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitGuard;

	// Token: 0x04000284 RID: 644
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000285 RID: 645
	[SerializeField]
	private Sprite _bossPortraitAirplane;

	// Token: 0x04000286 RID: 646
	[SerializeField]
	private string _bossQuoteGuard;

	// Token: 0x04000287 RID: 647
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000288 RID: 648
	[SerializeField]
	private string _bossQuoteAirplane;

	// Token: 0x04000289 RID: 649
	private Coroutine gruntCoroutine;

	// Token: 0x0200050D RID: 1293
	[Serializable]
	public class Prefabs
	{
		// Token: 0x04002038 RID: 8248
		public BeeLevelGrunt grunt;

		// Token: 0x04002039 RID: 8249
		public BeeLevelHoneyDrip drip;
	}
}
