using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class HouseLevel : Level
{
	// Token: 0x060005A1 RID: 1441 RVA: 0x000693CC File Offset: 0x000677CC
	protected override void PartialInit()
	{
		this.properties = LevelProperties.House.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00069462 File Offset: 0x00067862
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.House;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00069469 File Offset: 0x00067869
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_house_elder_kettle;
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0006946D File Offset: 0x0006786D
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00069475 File Offset: 0x00067875
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00069480 File Offset: 0x00067880
	protected override void Start()
	{
		base.Start();
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(new Levels[]
		{
			Levels.Devil
		}, Level.Mode.Hard))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 8f);
		}
		else if (PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Hard) + PlayerData.Data.CountLevelsHaveMinDifficulty(Level.world4BossLevels, Level.Mode.Hard) > 0)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 7f);
		}
		else if (PlayerData.Data.IsHardModeAvailable)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 6f);
		}
		else if (PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 5f);
		}
		else if (PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 4f);
		}
		else if (PlayerData.Data.CountLevelsCompleted(Level.world1BossLevels) > 1)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 3f);
		}
		else if (PlayerData.Data.IsTutorialCompleted)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
		}
		else if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) == 0f)
		{
			this.tutorialGameObject.SetActive(false);
			base.Ending = true;
		}
		SceneLoader.OnLoaderCompleteEvent += this.SelectMusic;
		this.AddDialoguerEvents();
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00069627 File Offset: 0x00067A27
	private void SelectMusic()
	{
		if (PlayerData.Data.pianoAudioEnabled)
		{
			AudioManager.PlayBGMPlaylistManually(false);
		}
		else
		{
			AudioManager.PlayBGM();
		}
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00069648 File Offset: 0x00067A48
	protected override void OnDestroy()
	{
		base.OnDestroy();
		SceneLoader.OnLoaderCompleteEvent -= this.SelectMusic;
		this.RemoveDialoguerEvents();
		this.playerTutorialEffects = null;
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00069670 File Offset: 0x00067A70
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x000696BF File Offset: 0x00067ABF
	private void OnDialogueEndedHandler()
	{
		base.Ending = false;
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x000696C8 File Offset: 0x00067AC8
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x000696E0 File Offset: 0x00067AE0
	public void StartTutorial()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.playerTutorialEffects[0].gameObject.SetActive(true);
		this.playerTutorialEffects[0].transform.position = player.transform.position;
		player.gameObject.SetActive(false);
		this.playerTutorialEffects[0].animator.SetTrigger("OnStartTutorial");
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.playerTutorialEffects[1].gameObject.SetActive(true);
			this.playerTutorialEffects[1].transform.position = player.transform.position;
			player.gameObject.SetActive(false);
			this.playerTutorialEffects[1].animator.SetTrigger("OnStartTutorial");
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x000697B0 File Offset: 0x00067BB0
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "ElderKettleFirstWeapon")
		{
			this.tutorialGameObject.SetActive(true);
			base.StartCoroutine(this.power_up_cr());
		}
		if (message == "EndJoy")
		{
		}
		if (message == "Sleep")
		{
		}
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00069808 File Offset: 0x00067C08
	private IEnumerator power_up_cr()
	{
		yield return new WaitForSeconds(0.15f);
		AudioManager.Play("sfx_potion_poof");
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.animator.Play("Power_Up");
			}
		}
		yield break;
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00069823 File Offset: 0x00067C23
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.housePattern_cr());
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00069834 File Offset: 0x00067C34
	private IEnumerator housePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) == 0f)
		{
			this.elderDialoguePoint.BeginDialogue();
		}
		yield break;
	}

	// Token: 0x04000A76 RID: 2678
	private LevelProperties.House properties;

	// Token: 0x04000A77 RID: 2679
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000A78 RID: 2680
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x04000A79 RID: 2681
	[SerializeField]
	private PlayerDeathEffect[] playerTutorialEffects;

	// Token: 0x04000A7A RID: 2682
	[SerializeField]
	private HouseElderKettle elderDialoguePoint;

	// Token: 0x04000A7B RID: 2683
	[SerializeField]
	private GameObject tutorialGameObject;

	// Token: 0x04000A7C RID: 2684
	[SerializeField]
	private int dialoguerVariableID;
}
