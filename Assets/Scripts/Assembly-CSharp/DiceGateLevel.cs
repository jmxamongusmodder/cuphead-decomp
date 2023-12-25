using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class DiceGateLevel : Level
{
	// Token: 0x060002B1 RID: 689 RVA: 0x0005DF54 File Offset: 0x0005C354
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DiceGate.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x0005DFEA File Offset: 0x0005C3EA
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DiceGate;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060002B3 RID: 691 RVA: 0x0005DFF1 File Offset: 0x0005C3F1
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_gate;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x0005DFF5 File Offset: 0x0005C3F5
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060002B5 RID: 693 RVA: 0x0005DFFD File Offset: 0x0005C3FD
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0005E008 File Offset: 0x0005C408
	protected override void Start()
	{
		base.Start();
		SceneLoader.OnLoaderCompleteEvent += this.SetMusic;
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			this.world1Background.SetActive(true);
			if (PlayerData.Data.CheckLevelCompleted(Levels.Veggies))
			{
				this.chalkboardCrosses[0].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Slime))
			{
				this.chalkboardCrosses[1].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Frogs))
			{
				this.chalkboardCrosses[2].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.FlyingBlimp))
			{
				this.chalkboardCrosses[3].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Flower))
			{
				this.chalkboardCrosses[4].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels))
			{
				this.dialogueInteractionPoint.animationTriggerOnEnd = this.completeLevelAnimationTrigger;
				this.OpenWay();
			}
			else
			{
				this.CloseWay();
			}
		}
		else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
		{
			this.world2Background.SetActive(true);
			this.toPrevWorld.dialogueProperties = this.world2PrevProperties;
			this.toNextWorld.dialogueProperties = this.world2NextProperties;
			if (PlayerData.Data.CheckLevelCompleted(Levels.Baroness))
			{
				this.chalkboardCrosses[0].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.FlyingGenie))
			{
				this.chalkboardCrosses[1].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Clown))
			{
				this.chalkboardCrosses[2].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.FlyingBird))
			{
				this.chalkboardCrosses[3].SetActive(true);
			}
			if (PlayerData.Data.CheckLevelCompleted(Levels.Dragon))
			{
				this.chalkboardCrosses[4].SetActive(true);
			}
			this.dialogueInteractionPoint.dialogueInteraction = this.dialogueWorld2;
			if (PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels))
			{
				this.dialogueInteractionPoint.animationTriggerOnEnd = this.completeLevelAnimationTrigger;
				this.OpenWay();
			}
			else
			{
				this.CloseWay();
			}
		}
		else
		{
			global::Debug.LogError("SOMETHING BAD HAPPENED", null);
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0005E28C File Offset: 0x0005C68C
	private void SetMusic()
	{
		AudioManager.PlayBGMPlaylistManually(true);
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0005E294 File Offset: 0x0005C694
	protected override void OnLevelStart()
	{
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0005E296 File Offset: 0x0005C696
	protected override void OnDestroy()
	{
		SceneLoader.OnLoaderCompleteEvent -= this.SetMusic;
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0005E2B8 File Offset: 0x0005C6B8
	private void CloseWay()
	{
		this.toNextWorld.enabled = false;
		this.kingDice.SetActive(true);
		if (PlayerData.Data.CurrentMapData.hasVisitedDieHouse)
		{
			if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
			{
				Dialoguer.SetGlobalFloat(16, 1f);
			}
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0005E314 File Offset: 0x0005C714
	private void OpenWay()
	{
		this.toNextWorld.enabled = true;
		if (PlayerData.Data.CurrentMapData.hasKingDiceDisappeared)
		{
			this.kingDice.SetActive(false);
		}
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			Dialoguer.SetGlobalFloat(16, 2f);
		}
		else
		{
			Dialoguer.SetGlobalFloat(17, 1f);
		}
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0005E380 File Offset: 0x0005C780
	private IEnumerator dicegatePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0005E39C File Offset: 0x0005C79C
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DiceGate.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0400050A RID: 1290
	private LevelProperties.DiceGate properties;

	// Token: 0x0400050B RID: 1291
	[SerializeField]
	private AbstractLevelInteractiveEntity toNextWorld;

	// Token: 0x0400050C RID: 1292
	[SerializeField]
	private AbstractLevelInteractiveEntity toPrevWorld;

	// Token: 0x0400050D RID: 1293
	public AbstractUIInteractionDialogue.Properties world2PrevProperties;

	// Token: 0x0400050E RID: 1294
	public AbstractUIInteractionDialogue.Properties world2NextProperties;

	// Token: 0x0400050F RID: 1295
	[SerializeField]
	private GameObject kingDice;

	// Token: 0x04000510 RID: 1296
	[SerializeField]
	private List<GameObject> chalkboardCrosses;

	// Token: 0x04000511 RID: 1297
	[SerializeField]
	private DialogueInteractionPoint dialogueInteractionPoint;

	// Token: 0x04000512 RID: 1298
	[SerializeField]
	private DialoguerDialogues dialogueWorld2;

	// Token: 0x04000513 RID: 1299
	[SerializeField]
	private string completeLevelAnimationTrigger;

	// Token: 0x04000514 RID: 1300
	[SerializeField]
	private GameObject world1Background;

	// Token: 0x04000515 RID: 1301
	[SerializeField]
	private GameObject world2Background;

	// Token: 0x04000516 RID: 1302
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000517 RID: 1303
	[SerializeField]
	[Multiline]
	private string _bossQuote;
}
