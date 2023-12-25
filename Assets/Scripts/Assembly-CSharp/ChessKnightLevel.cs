using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class ChessKnightLevel : ChessLevel
{
	// Token: 0x060001EC RID: 492 RVA: 0x0005A630 File Offset: 0x00058A30
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessKnight.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060001ED RID: 493 RVA: 0x0005A6C6 File Offset: 0x00058AC6
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessKnight;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060001EE RID: 494 RVA: 0x0005A6CD File Offset: 0x00058ACD
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_knight;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060001EF RID: 495 RVA: 0x0005A6D1 File Offset: 0x00058AD1
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x0005A6D9 File Offset: 0x00058AD9
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0005A6E1 File Offset: 0x00058AE1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this.knight = null;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0005A6F8 File Offset: 0x00058AF8
	protected override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.knight.LevelInit(this.properties);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.gameObject.layer = 31;
				foreach (Transform transform in levelPlayerController.gameObject.GetComponentsInChildren<Transform>(true))
				{
					transform.gameObject.layer = 31;
				}
			}
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0005A7C0 File Offset: 0x00058BC0
	protected override void OnPlayerJoined(PlayerId playerId)
	{
		base.OnPlayerJoined(playerId);
		AbstractPlayerController player = PlayerManager.GetPlayer(playerId);
		if (player)
		{
			foreach (SpriteRenderer spriteRenderer in player.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.gameObject.layer = 31;
			}
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0005A812 File Offset: 0x00058C12
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.chessknightPattern_cr());
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0005A824 File Offset: 0x00058C24
	private IEnumerator chessknightPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0005A840 File Offset: 0x00058C40
	private IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.ChessKnight.Pattern.Long:
			yield return base.StartCoroutine(this.long_cr());
			break;
		case LevelProperties.ChessKnight.Pattern.Short:
			yield return base.StartCoroutine(this.short_cr());
			break;
		case LevelProperties.ChessKnight.Pattern.Up:
			yield return base.StartCoroutine(this.up_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0005A85C File Offset: 0x00058C5C
	private IEnumerator short_cr()
	{
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		this.knight.Short();
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0005A878 File Offset: 0x00058C78
	private IEnumerator long_cr()
	{
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		this.knight.Long();
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0005A894 File Offset: 0x00058C94
	private IEnumerator up_cr()
	{
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		this.knight.Up();
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000380 RID: 896
	private LevelProperties.ChessKnight properties;

	// Token: 0x04000381 RID: 897
	[SerializeField]
	private ChessKnightLevelKnight knight;

	// Token: 0x04000382 RID: 898
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000383 RID: 899
	[SerializeField]
	private string _bossQuoteMain;
}
