using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008FB RID: 2299
public class PlatformingLevel : Level
{
	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x060035E7 RID: 13799 RVA: 0x001D495E File Offset: 0x001D2D5E
	// (set) Token: 0x060035E8 RID: 13800 RVA: 0x001D4965 File Offset: 0x001D2D65
	public new static PlatformingLevel Current { get; private set; }

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x060035E9 RID: 13801 RVA: 0x001D496D File Offset: 0x001D2D6D
	public override Levels CurrentLevel
	{
		get
		{
			return this._currentLevel;
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x060035EA RID: 13802 RVA: 0x001D4975 File Offset: 0x001D2D75
	public override Scenes CurrentScene
	{
		get
		{
			return this._currentScene;
		}
	}

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x060035EB RID: 13803 RVA: 0x001D497D File Offset: 0x001D2D7D
	public override Sprite BossPortrait
	{
		get
		{
			return (!this.useAltQuote) ? this._bossPortrait : this._bossPortraitAlt;
		}
	}

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x060035EC RID: 13804 RVA: 0x001D499B File Offset: 0x001D2D9B
	public override string BossQuote
	{
		get
		{
			return (!this.useAltQuote) ? this._bossQuote : this._bossQuoteAlt;
		}
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x001D49BC File Offset: 0x001D2DBC
	protected override void Awake()
	{
		this._currentLevel = SceneLoader.CurrentLevel;
		this._currentScene = EnumUtils.Parse<Scenes>(LevelProperties.GetLevelScene(this._currentLevel));
		this.goalTimes = new Level.GoalTimes(this.goalTime, this.goalTime, this.goalTime);
		Level.OverrideDifficulty = true;
		base.mode = Level.Mode.Normal;
		base.Awake();
		PlatformingLevel.Current = this;
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x001D4A20 File Offset: 0x001D2E20
	protected override void Start()
	{
		base.Start();
		this.LevelCoinsIDs.Sort((CoinPositionAndID a, CoinPositionAndID b) => a.xPos.CompareTo(b.xPos));
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x001D4A50 File Offset: 0x001D2E50
	protected override void OnLevelStart()
	{
		base.OnLevelStart();
		base.timeline = new Level.Timeline();
		base.timeline.health = 100f;
		base.StartCoroutine(this.checkPosition_cr());
		Level.ScoringData.pacifistRun = true;
		PlatformingLevelExit.OnWinStartEvent += this.OnWinStart;
		PlatformingLevelExit.OnWinCompleteEvent += this.OnWinComplete;
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x001D4AB8 File Offset: 0x001D2EB8
	private void OnWinStart()
	{
		base.Ending = true;
		CupheadLevelCamera.Current.MoveRightCollider();
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x001D4ACB File Offset: 0x001D2ECB
	private void OnWinComplete()
	{
		LevelCoin.OnLevelComplete();
		Level.ScoringData.coinsCollected = PlayerData.Data.GetNumCoinsCollectedInLevel(this.CurrentLevel);
		Level.ScoringData.useCoinsInsteadOfSuperMeter = true;
		base.zHack_OnWin();
	}

	// Token: 0x060035F2 RID: 13810 RVA: 0x001D4AFD File Offset: 0x001D2EFD
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (PlatformingLevel.Current == this)
		{
			PlatformingLevel.Current = null;
		}
		this._bossPortrait = null;
		this._bossPortraitAlt = null;
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x001D4B2C File Offset: 0x001D2F2C
	protected override void Reset()
	{
		base.Reset();
		this.type = Level.Type.Platforming;
		this.bounds.bottom = 500;
		this.camera.moveX = true;
		this.camera.moveY = true;
		this.camera.mode = CupheadLevelCamera.Mode.Platforming;
		this.camera.colliders = true;
		this.camera.bounds.rightEnabled = false;
		this.camera.bounds.topEnabled = false;
	}

	// Token: 0x060035F4 RID: 13812 RVA: 0x001D4BA8 File Offset: 0x001D2FA8
	private IEnumerator checkPosition_cr()
	{
		for (;;)
		{
			foreach (AbstractPlayerController abstractPlayerController in this.players)
			{
				if (abstractPlayerController != null && !abstractPlayerController.IsDead && base.LevelType == Level.Type.Platforming && this.camera.mode == CupheadLevelCamera.Mode.Path)
				{
					float value = this.camera.path.GetClosestNormalizedPoint(abstractPlayerController.center, abstractPlayerController.center, true, true) * 100f;
					base.timeline.SetPlayerDamage(abstractPlayerController.id, value);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x001D4BC3 File Offset: 0x001D2FC3
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x001D4BD6 File Offset: 0x001D2FD6
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x001D4BE9 File Offset: 0x001D2FE9
	private void DrawGizmos(float a)
	{
		if (this.camera.mode != CupheadLevelCamera.Mode.Path)
		{
			return;
		}
		this.camera.path.DrawGizmos(a, base.baseTransform.position);
	}

	// Token: 0x04003DFA RID: 15866
	public const float TIMELINE_LENGTH = 100f;

	// Token: 0x04003DFC RID: 15868
	public List<CoinPositionAndID> LevelCoinsIDs = new List<CoinPositionAndID>();

	// Token: 0x04003DFD RID: 15869
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04003DFE RID: 15870
	[SerializeField]
	private Sprite _bossPortraitAlt;

	// Token: 0x04003DFF RID: 15871
	[SerializeField]
	private string _bossQuote;

	// Token: 0x04003E00 RID: 15872
	[SerializeField]
	private string _bossQuoteAlt;

	// Token: 0x04003E01 RID: 15873
	[SerializeField]
	private float goalTime;

	// Token: 0x04003E02 RID: 15874
	private Levels _currentLevel;

	// Token: 0x04003E03 RID: 15875
	private Scenes _currentScene;

	// Token: 0x04003E04 RID: 15876
	public bool useAltQuote;

	// Token: 0x020008FC RID: 2300
	public enum Theme
	{
		// Token: 0x04003E07 RID: 15879
		Forest
	}
}
