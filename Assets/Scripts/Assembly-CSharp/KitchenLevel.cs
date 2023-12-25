using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class KitchenLevel : Level
{
	// Token: 0x060005BC RID: 1468 RVA: 0x00069A24 File Offset: 0x00067E24
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Kitchen.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060005BD RID: 1469 RVA: 0x00069ABA File Offset: 0x00067EBA
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Kitchen;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060005BE RID: 1470 RVA: 0x00069AC1 File Offset: 0x00067EC1
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_kitchen;
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x00069AC5 File Offset: 0x00067EC5
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00069ACD File Offset: 0x00067ECD
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00069AD8 File Offset: 0x00067ED8
	protected override void Start()
	{
		base.Start();
		this.CheckIfBossesCompleted();
		base.StartCoroutine(this.check_camera_cr());
		base.StartCoroutine(this.cycle_sunbeams_cr());
		this.beforeGettingIngredients.SetActive(!this.trapDoorOpen);
		this.afterGettingIngredients.SetActive(this.trapDoorOpen);
		this.AddDialoguerEvents();
		PlayerManager.OnPlayerJoinedEvent += this.SetPlayerBasementMaterial;
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00069B47 File Offset: 0x00067F47
	protected override void OnDestroy()
	{
		this.RemoveDialoguerEvents();
		PlayerManager.OnPlayerJoinedEvent -= this.SetPlayerBasementMaterial;
		base.OnDestroy();
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00069B68 File Offset: 0x00067F68
	private void SetPlayerBasementMaterial(PlayerId p)
	{
		if (!this.basementBG.activeInHierarchy)
		{
			return;
		}
		foreach (SpriteRenderer spriteRenderer in PlayerManager.GetPlayer(p).GetComponentsInChildren<SpriteRenderer>())
		{
			if (spriteRenderer.material.name == "Sprites-Default (Instance)" || (spriteRenderer.sharedMaterial.name == "ChaliceRecolor (Instance)" && spriteRenderer.sharedMaterial.GetFloat("_RecolorFactor") == 0f))
			{
				spriteRenderer.material = this.playerBasementMaterial;
				spriteRenderer.color = new Color(0.7137255f, 0.4862745f, 0.12941177f);
			}
		}
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00069C1E File Offset: 0x0006801E
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00069C36 File Offset: 0x00068036
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00069C4E File Offset: 0x0006804E
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "MetSaltbaker")
		{
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x00069C65 File Offset: 0x00068065
	private void CheckIfBossesCompleted()
	{
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal))
		{
			this.trapDoorOpen = true;
			base.StartCoroutine(this.check_trigger_cr());
		}
		else
		{
			this.trapDoorOpen = false;
		}
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x00069C9C File Offset: 0x0006809C
	protected override void OnLevelStart()
	{
		if (Dialoguer.GetGlobalFloat(23) == 1f)
		{
			AudioManager.Play("sfx_dlc_bakery_doorenter");
		}
		if (this.trapDoorOpen)
		{
			AudioManager.StartBGMAlternate(1);
		}
		else if (PlayerData.Data.pianoAudioEnabled)
		{
			AudioManager.StartBGMAlternate(2);
		}
		else
		{
			AudioManager.PlayBGM();
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x00069CFC File Offset: 0x000680FC
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (this.triggerEndGame != null)
		{
			Vector2 v = new Vector2(this.triggerEndGame.position.x, this.triggerEndGame.position.y + 1000f);
			Vector2 v2 = new Vector2(this.triggerEndGame.position.x, this.triggerEndGame.position.y - 1000f);
			Gizmos.DrawLine(v, v2);
		}
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00069D9C File Offset: 0x0006819C
	private IEnumerator check_trigger_cr()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		bool hasntPassed = true;
		while (hasntPassed)
		{
			if (player.transform.position.x >= this.triggerEndGame.position.x)
			{
				hasntPassed = false;
			}
			if (player2 != null && player2.transform.position.x >= this.triggerEndGame.position.x)
			{
				hasntPassed = false;
			}
			yield return null;
		}
		PlayerManager.playerWasChalice[0] = player.stats.isChalice;
		PlayerManager.playerWasChalice[1] = (player2 != null && player2.stats.isChalice);
		if (Level.CurrentMode == Level.Mode.Easy)
		{
			Level.SetCurrentMode(Level.Mode.Normal);
		}
		Cutscene.Load(Scenes.scene_level_saltbaker, Scenes.scene_cutscene_dlc_saltbaker_prebattle, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass);
		yield break;
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00069DB8 File Offset: 0x000681B8
	private IEnumerator check_camera_cr()
	{
		this.camera.mode = CupheadLevelCamera.Mode.Relative;
		bool inPit = false;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		float lastP1YPos = 0f;
		float lastP2YPos = 0f;
		while (!inPit)
		{
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (player2 != null && !player2.IsDead)
			{
				inPit = (player2.transform.position.y < -400f && player.transform.position.y < -400f);
				if (!inPit)
				{
					if (player.transform.position.y < -400f)
					{
						player.gameObject.SetActive(false);
					}
					if (player2.transform.position.y < -400f)
					{
						player2.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				inPit = (player.transform.position.y < -400f);
			}
			if (player != null && Mathf.Sign(player.transform.position.y + 208f) != Mathf.Sign(lastP1YPos + 208f))
			{
				foreach (SpriteRenderer spriteRenderer in player.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer.sortingLayerName = ((player.transform.position.y >= -208f) ? "Player" : "Enemies");
				}
				lastP1YPos = player.transform.position.y;
			}
			if (player2 != null && Mathf.Sign(player2.transform.position.y + 208f) != Mathf.Sign(lastP2YPos + 208f))
			{
				foreach (SpriteRenderer spriteRenderer2 in player2.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer2.sortingLayerName = ((player2.transform.position.y >= -208f) ? "Player" : "Enemies");
				}
				lastP2YPos = player.transform.position.y;
			}
			yield return null;
		}
		this.kitchenBG.SetActive(false);
		this.basementBG.SetActive(true);
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementamb_loop", 0.0001f, 0.0001f);
		AudioManager.PlayLoop("sfx_dlc_bakery_basementamb_loop");
		AudioManager.PlayLoop("sfx_dlc_bakery_basementtorch_loop");
		this.afterGettingIngredients.SetActive(false);
		CupheadLevelCamera.Current.ChangeHorizontalBounds(740, 3500);
		Level.Current.SetBounds(new int?(680), new int?(6860), null, null);
		CupheadLevelCamera.Current.ChangeCameraMode(CupheadLevelCamera.Mode.Lerp);
		CupheadLevelCamera.Current.LERP_SPEED = 5f;
		CupheadLevelCamera.Current.SetPosition(new Vector3(-100f, 0f));
		player.transform.position = new Vector3(-500f, 800f);
		player.gameObject.SetActive(true);
		if (player2 != null && !player2.IsDead)
		{
			player2.transform.position = new Vector3(-400f, 800f);
			player2.gameObject.SetActive(true);
		}
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			if (abstractPlayerController != null)
			{
				this.SetPlayerBasementMaterial(abstractPlayerController.id);
				foreach (SpriteRenderer spriteRenderer3 in abstractPlayerController.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer3.sortingLayerName = "Player";
				}
			}
		}
		AudioManager.StartBGMAlternate(0);
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementamb_loop", 0.5f, 1f);
		while (CupheadLevelCamera.Current.transform.position.x < 2320f)
		{
			this.HandleTorchSFX();
			yield return null;
		}
		this.saltbakerShadow.SetTrigger("Continue");
		CupheadLevelCamera.Current.ChangeCameraMode(CupheadLevelCamera.Mode.Platforming);
		AudioManager.Play("sfx_dlc_saltbaker_evilbasementlaugh");
		while (CupheadLevelCamera.Current.transform.position.x < 2800f)
		{
			this.HandleTorchSFX();
			yield return null;
		}
		this.saltbakerShadow.SetTrigger("Continue");
		yield break;
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x00069DD4 File Offset: 0x000681D4
	private void HandleTorchSFX()
	{
		float num = float.MaxValue;
		float num2 = 0f;
		foreach (Transform transform in this.torchPositions)
		{
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				if (abstractPlayerController != null)
				{
					float num3 = Mathf.Abs(abstractPlayerController.center.x - transform.position.x);
					if (num3 < num)
					{
						num2 = Mathf.Sign(transform.position.x - abstractPlayerController.center.x);
						num = num3;
					}
				}
			}
		}
		float num4 = Mathf.InverseLerp(320f, 0f, num);
		float value = num2 * (1f - num4);
		AudioManager.FadeSFXVolume("sfx_dlc_bakery_basementtorch_loop", Mathf.Lerp(0.01f, 0.8f, num4), 0.0001f);
		AudioManager.Pan("sfx_dlc_bakery_basementtorch_loop", value);
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00069F0C File Offset: 0x0006830C
	private IEnumerator cycle_sunbeams_cr()
	{
		float t = 0f;
		for (;;)
		{
			for (int i = 0; i < 3; i++)
			{
				this.sunbeams[i].color = new Color(1f, 1f, 1f, (Mathf.Sin((float)i * 2.0943952f + t) + 1f) / 2f);
			}
			t += CupheadTime.Delta * this.sunbeamCycleSpeed;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000A84 RID: 2692
	private LevelProperties.Kitchen properties;

	// Token: 0x04000A85 RID: 2693
	private const int DIALOGUER_VAR_ID = 23;

	// Token: 0x04000A86 RID: 2694
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000A87 RID: 2695
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x04000A88 RID: 2696
	[SerializeField]
	private GameObject beforeGettingIngredients;

	// Token: 0x04000A89 RID: 2697
	[SerializeField]
	private GameObject afterGettingIngredients;

	// Token: 0x04000A8A RID: 2698
	[SerializeField]
	private SpriteRenderer[] sunbeams;

	// Token: 0x04000A8B RID: 2699
	[SerializeField]
	private float sunbeamCycleSpeed = 2f;

	// Token: 0x04000A8C RID: 2700
	[SerializeField]
	private Animator saltbakerShadow;

	// Token: 0x04000A8D RID: 2701
	[SerializeField]
	private Transform triggerEndGame;

	// Token: 0x04000A8E RID: 2702
	private bool trapDoorOpen;

	// Token: 0x04000A8F RID: 2703
	[SerializeField]
	private SpriteRenderer trapDoorOverlay;

	// Token: 0x04000A90 RID: 2704
	private bool forceUnlockSaltbakerBattle;

	// Token: 0x04000A91 RID: 2705
	[SerializeField]
	private GameObject kitchenBG;

	// Token: 0x04000A92 RID: 2706
	[SerializeField]
	private GameObject basementBG;

	// Token: 0x04000A93 RID: 2707
	[SerializeField]
	private Material playerBasementMaterial;

	// Token: 0x04000A94 RID: 2708
	[SerializeField]
	private Transform[] torchPositions;
}
