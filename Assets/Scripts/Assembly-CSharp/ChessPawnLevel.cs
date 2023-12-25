using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class ChessPawnLevel : ChessLevel
{
	// Token: 0x06000206 RID: 518 RVA: 0x0005ADD8 File Offset: 0x000591D8
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessPawn.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000207 RID: 519 RVA: 0x0005AE6E File Offset: 0x0005926E
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessPawn;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000208 RID: 520 RVA: 0x0005AE75 File Offset: 0x00059275
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_pawn;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000209 RID: 521 RVA: 0x0005AE79 File Offset: 0x00059279
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600020A RID: 522 RVA: 0x0005AE81 File Offset: 0x00059281
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0005AE89 File Offset: 0x00059289
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		base.OnIntroEvent -= this.onIntroEvent;
		this.pawn = null;
		this.pawns = null;
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0005AEB8 File Offset: 0x000592B8
	protected override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		base.OnIntroEvent += this.onIntroEvent;
		int num = (int)this.properties.TotalHealth / 10;
		this.pawns = new ChessPawnLevelPawn[num];
		for (int i = 0; i < num; i++)
		{
			this.pawns[i] = this.pawn.Init(this);
			this.pawns[i].transform.position = this.GetPosition(i) + new Vector3(0f, 300f);
			this.pawns[i].SetIndex(i);
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0005AF60 File Offset: 0x00059360
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0005AF6F File Offset: 0x0005936F
	public void TakeDamage()
	{
		this.properties.DealDamage(10f);
		if (this.properties.CurrentHealth <= 0f)
		{
			this.die();
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0005AF9C File Offset: 0x0005939C
	private void onIntroEvent()
	{
		for (int i = 0; i < this.pawns.Length; i++)
		{
			this.pawns[i].StartIntro();
			this.SFX_KOG_PAWN_IntroJeers();
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0005AFD8 File Offset: 0x000593D8
	private IEnumerator main_cr()
	{
		LevelProperties.ChessPawn.Pawn p = this.properties.CurrentState.pawn;
		PatternString pawnAttackDelay = new PatternString(p.pawnAttackDelayString, true);
		PatternString pawnDirection = new PatternString(p.pawnDirectionString, true);
		PatternString pawnOrder = new PatternString(p.pawnOrderString, true);
		for (;;)
		{
			bool pink = pawnOrder.PopLetter() == 'P';
			List<int> availableList = new List<int>();
			for (int j = 0; j < this.pawns.Length; j++)
			{
				if (pink == this.pawns[j].CanParry && !this.pawns[j].inUse)
				{
					availableList.Add(j);
				}
			}
			if (availableList.Count > 0)
			{
				float dir = 0f;
				char c = pawnDirection.PopLetter();
				if (c != 'L')
				{
					if (c != 'D')
					{
						if (c == 'R')
						{
							dir = 1f;
						}
					}
					else
					{
						dir = 0f;
					}
				}
				else
				{
					dir = -1f;
				}
				int i = UnityEngine.Random.Range(0, availableList.Count);
				if (Mathf.Abs(this.GetPosition(this.pawns[availableList[i]].currentIndex).x + dir * p.pawnDropDistance) > 800f)
				{
					dir = 0f;
				}
				dir *= p.pawnDropDistance;
				this.pawns[availableList[i]].Attack(p.pawnWarningTime, dir, p.pawnDropSpeed, p.pawnRunHesitation, p.pawnRunSpeed, p.pawnReturnDelay);
				yield return CupheadTime.WaitForSeconds(this, pawnAttackDelay.PopFloat() - p.pawnDelayReduction * (float)this.damageCount());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0005AFF4 File Offset: 0x000593F4
	private void die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.SFX_KOG_PAWN_BeatLevelHarp();
		foreach (ChessPawnLevelPawn chessPawnLevelPawn in this.pawns)
		{
			chessPawnLevelPawn.Death();
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0005B03F File Offset: 0x0005943F
	private int damageCount()
	{
		return (int)(this.properties.TotalHealth - this.properties.CurrentHealth) / 10;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0005B05C File Offset: 0x0005945C
	public int GetReturnIndex()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.pawns.Length; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < this.pawns.Length; j++)
		{
			list.Remove(this.pawns[j].currentIndex);
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0005B0CE File Offset: 0x000594CE
	public Vector3 GetPosition(int index)
	{
		return new Vector3(-622f + (float)index * 180f, 340f);
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0005B0E8 File Offset: 0x000594E8
	public bool ClearToRun(float testDir, Vector3 pos)
	{
		bool result = true;
		for (int i = 0; i < this.pawns.Length; i++)
		{
			if (this.pawns[i].speed != 0f && Mathf.Sign(this.pawns[i].speed) == testDir && Vector3.Distance(this.pawns[i].transform.position, pos) < 200f)
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0005B164 File Offset: 0x00059564
	private void SFX_KOG_PAWN_IntroJeers()
	{
		AudioManager.Play("sfx_DLC_KOG_Pawn_IntroJeers");
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0005B170 File Offset: 0x00059570
	private void SFX_KOG_PAWN_BeatLevelHarp()
	{
		AudioManager.Play("sfx_dlc_kog_pawn_beatlevelharp");
	}

	// Token: 0x04000396 RID: 918
	private LevelProperties.ChessPawn properties;

	// Token: 0x04000397 RID: 919
	private const float WAIT_TO_RUN_DIST = 200f;

	// Token: 0x04000398 RID: 920
	private const float SPACING = 180f;

	// Token: 0x04000399 RID: 921
	private const float LEFTMOST_X = -622f;

	// Token: 0x0400039A RID: 922
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x0400039B RID: 923
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x0400039C RID: 924
	[SerializeField]
	private ChessPawnLevelPawn pawn;

	// Token: 0x0400039D RID: 925
	private ChessPawnLevelPawn[] pawns;

	// Token: 0x0400039E RID: 926
	private bool dead;
}
