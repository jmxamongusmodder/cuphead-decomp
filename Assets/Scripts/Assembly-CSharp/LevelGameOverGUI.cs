using System;
using System.Collections;
using RektTransform;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200047E RID: 1150
[RequireComponent(typeof(CanvasGroup))]
public class LevelGameOverGUI : AbstractMonoBehaviour
{
	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x060011B5 RID: 4533 RVA: 0x000A60BE File Offset: 0x000A44BE
	// (set) Token: 0x060011B6 RID: 4534 RVA: 0x000A60C5 File Offset: 0x000A44C5
	public static Color COLOR_SELECTED { get; private set; }

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x060011B7 RID: 4535 RVA: 0x000A60CD File Offset: 0x000A44CD
	// (set) Token: 0x060011B8 RID: 4536 RVA: 0x000A60D4 File Offset: 0x000A44D4
	public static Color COLOR_INACTIVE { get; private set; }

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x060011B9 RID: 4537 RVA: 0x000A60DC File Offset: 0x000A44DC
	// (set) Token: 0x060011BA RID: 4538 RVA: 0x000A60E3 File Offset: 0x000A44E3
	public static Color COLOR_DESABLE { get; private set; }

	// Token: 0x060011BB RID: 4539 RVA: 0x000A60EC File Offset: 0x000A44EC
	protected override void Awake()
	{
		base.Awake();
		LevelGameOverGUI.Current = this;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		base.gameObject.SetActive(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.cardCanvasGroup.alpha = 0f;
		this.helpCanvasGroup.alpha = 0f;
		this.ignoreGlobalTime = true;
		this.timeLayer = CupheadTime.Layer.UI;
		LevelGameOverGUI.COLOR_SELECTED = this.menuItems[0].color;
		LevelGameOverGUI.COLOR_INACTIVE = this.menuItems[this.menuItems.Length - 1].color;
		if (Level.IsTowerOfPower)
		{
			this.equipToolTip.SetActive(false);
			if (!TowerOfPowerLevelGameInfo.IsTokenLeft())
			{
				this.menuItems[0].gameObject.SetActive(false);
				this.selection = 1;
				this.UpdateSelection();
			}
			else
			{
				this.retryLocHelper.currentID = Localization.Find("OptionMenuRetryTowerBattle").id;
				this.retryLocHelper.ApplyTranslation();
			}
		}
		this.state = LevelGameOverGUI.State.Init;
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x000A61F5 File Offset: 0x000A45F5
	private void Start()
	{
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			this.updateRotateControlsToggleVisualValue();
		}
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x000A6221 File Offset: 0x000A4621
	private void OnDestroy()
	{
		LevelGameOverGUI.Current = null;
		this.youDiedText = null;
		this.bossPortraitImage = null;
		this.timeline.cuphead = null;
		this.timeline.mugman = null;
		this.timeline = null;
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x000A6258 File Offset: 0x000A4658
	private void Update()
	{
		if (this.state != LevelGameOverGUI.State.Ready)
		{
			return;
		}
		if (this.selection == 2 && Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane && (this.getButtonDown(CupheadButton.Accept) || this.getButtonDown(CupheadButton.MenuLeft) || this.getButtonDown(CupheadButton.MenuRight)))
		{
			AudioManager.Play("level_menu_card_down");
			this.toggleRotateControls();
			return;
		}
		int num = 0;
		if (this.getButtonDown(CupheadButton.Accept))
		{
			this.Select();
			AudioManager.Play("level_menu_select");
			this.state = LevelGameOverGUI.State.Exiting;
		}
		if (!Level.IsTowerOfPower && this.getButtonDown(CupheadButton.EquipMenu))
		{
			this.ChangeEquipment();
		}
		if (this.getButtonDown(CupheadButton.MenuDown))
		{
			AudioManager.Play("level_menu_move");
			num++;
		}
		if (this.getButtonDown(CupheadButton.MenuUp))
		{
			AudioManager.Play("level_menu_move");
			num--;
		}
		this.selection += num;
		this.selection = Mathf.Clamp(this.selection, 0, this.menuItems.Length - 1);
		if (!this.menuItems[this.selection].gameObject.activeSelf)
		{
			this.selection -= num;
			this.selection = Mathf.Clamp(this.selection, 0, this.menuItems.Length - 1);
		}
		this.UpdateSelection();
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x000A63C9 File Offset: 0x000A47C9
	private bool getButtonDown(CupheadButton button)
	{
		return this.input.GetButtonDown(button);
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000A63D8 File Offset: 0x000A47D8
	private void UpdateSelection()
	{
		for (int i = 0; i < this.menuItems.Length; i++)
		{
			Text text = this.menuItems[i];
			if (i == this.selection)
			{
				text.color = LevelGameOverGUI.COLOR_SELECTED;
			}
			else
			{
				text.color = LevelGameOverGUI.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x000A6430 File Offset: 0x000A4830
	private void Select()
	{
		if (!Level.IsGraveyard)
		{
			AudioManager.SnapshotReset(SceneLoader.SceneName, 2f);
			AudioManager.ChangeBGMPitch(1f, 2f);
		}
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			SettingsData.Save();
			if (PlatformHelper.IsConsole)
			{
				SettingsData.SaveToCloud();
			}
		}
		switch (this.selection)
		{
		default:
			this.Retry();
			AudioManager.Play("level_menu_card_down");
			break;
		case 1:
			this.ExitToMap();
			AudioManager.Play("level_menu_card_down");
			break;
		case 2:
			this.QuitGame();
			AudioManager.Play("level_menu_card_down");
			break;
		}
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x000A64FA File Offset: 0x000A48FA
	private void Retry()
	{
		if (Level.IsDicePalaceMain || Level.IsDicePalace)
		{
			DicePalaceMainLevelGameInfo.CleanUpRetry();
		}
		SceneLoader.ReloadLevel();
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x000A651A File Offset: 0x000A491A
	private void ExitToMap()
	{
		SceneLoader.LoadLastMap();
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x000A6521 File Offset: 0x000A4921
	private void QuitGame()
	{
		Level.IsGraveyard = false;
		PlayerManager.ResetPlayers();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x000A6538 File Offset: 0x000A4938
	private void ChangeEquipment()
	{
		base.StartCoroutine(this.outforequip_cr());
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x000A6547 File Offset: 0x000A4947
	public void ReactivateOnChangeEquipmentClosed()
	{
		base.StartCoroutine(this.inforequip_cr());
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x000A6556 File Offset: 0x000A4956
	private void SetAlpha(float value)
	{
		this.canvasGroup.alpha = value;
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x000A6564 File Offset: 0x000A4964
	private void SetTextAlpha(float value)
	{
		Color color = this.youDiedText.color;
		color.a = value;
		this.youDiedText.color = color;
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x000A6594 File Offset: 0x000A4994
	private void SetCardValue(float value)
	{
		this.cardCanvasGroup.alpha = value;
		this.helpCanvasGroup.alpha = value;
		this.cardCanvasGroup.transform.SetLocalEulerAngles(null, null, new float?(Mathf.Lerp(30f, 4f, value)));
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x000A65F0 File Offset: 0x000A49F0
	private void SetCardValueEquipSwap(float value)
	{
		this.cardCanvasGroup.alpha = value;
		this.helpCanvasGroup.alpha = value;
		this.cardCanvasGroup.transform.SetLocalEulerAngles(null, null, new float?(Mathf.Lerp(30f, 4f, value)));
		this.cardCanvasGroup.transform.SetLocalPosition(null, new float?(Mathf.Lerp(-720f, 0f, value)), null);
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x000A6684 File Offset: 0x000A4A84
	public void In(bool secretTriggered)
	{
		base.gameObject.SetActive(true);
		this.bossPortraitImage.sprite = Level.Current.BossPortrait;
		if (secretTriggered)
		{
			this.cardCanvasGroup.GetComponent<Image>().sprite = this.timelineSecret;
			this.timelineObj.SetActive(false);
		}
		if (this.bossQuoteLocalization == null)
		{
			this.bossQuoteText.text = "\"" + Level.Current.BossQuote + "\"";
		}
		else
		{
			this.bossQuoteLocalization.ApplyTranslation(Localization.Find(Level.Current.BossQuote), null);
			if (Localization.language == Localization.Languages.Korean)
			{
				this.bossQuoteLocalization.textMeshProComponent.fontStyle = FontStyles.Bold;
			}
		}
		if (this.bossPortraitImage.sprite != null)
		{
			this.bossPortraitImage.rectTransform.SetSize(this.bossPortraitImage.sprite.rect.width, this.bossPortraitImage.sprite.rect.height);
		}
		base.StartCoroutine(this.in_cr());
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x000A67B0 File Offset: 0x000A4BB0
	private IEnumerator in_cr()
	{
		AudioManager.Play("level_menu_card_up");
		yield return base.TweenValue(0f, 1f, 0.05f, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetAlpha));
		yield return new WaitForSeconds(1f);
		foreach (PlayerDeathEffect playerDeathEffect in UnityEngine.Object.FindObjectsOfType<PlayerDeathEffect>())
		{
			playerDeathEffect.GameOverUnpause();
		}
		foreach (PlanePlayerDeathPart planePlayerDeathPart in UnityEngine.Object.FindObjectsOfType<PlanePlayerDeathPart>())
		{
			planePlayerDeathPart.GameOverUnpause();
		}
		yield return base.TweenValue(1f, 0f, 0.25f, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetTextAlpha));
		yield return new WaitForSeconds(0.3f);
		if (!Level.IsGraveyard && !Level.IsChessBoss)
		{
			AudioManager.Play("player_die_vinylscratch");
			AudioManager.HandleSnapshot(AudioManager.Snapshots.Death.ToString(), 4f);
			AudioManager.ChangeBGMPitch(0.7f, 6f);
		}
		CupheadLevelCamera.Current.StartBlur();
		this.timeline.Setup(this, Level.Current.timeline);
		base.TweenValue(0f, 1f, 0.3f, EaseUtils.EaseType.easeOutCubic, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetCardValue));
		this.state = LevelGameOverGUI.State.Ready;
		yield return null;
		yield break;
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x000A67CC File Offset: 0x000A4BCC
	private IEnumerator outforequip_cr()
	{
		this.state = LevelGameOverGUI.State.Init;
		this.equipUI.gameObject.SetActive(true);
		this.equipUI.Activate();
		yield return base.TweenValue(1f, 0f, 0.3f, EaseUtils.EaseType.easeOutCubic, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetCardValueEquipSwap));
		yield break;
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x000A67E8 File Offset: 0x000A4BE8
	private IEnumerator inforequip_cr()
	{
		yield return base.TweenValue(0f, 1f, 0.3f, EaseUtils.EaseType.easeOutCubic, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetCardValueEquipSwap));
		this.state = LevelGameOverGUI.State.Ready;
		yield break;
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x000A6803 File Offset: 0x000A4C03
	private void toggleRotateControls()
	{
		SettingsData.Data.rotateControlsWithCamera = !SettingsData.Data.rotateControlsWithCamera;
		this.updateRotateControlsToggleVisualValue();
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x000A6824 File Offset: 0x000A4C24
	private void updateRotateControlsToggleVisualValue()
	{
		Text text = this.menuItems[2];
		text.GetComponent<LocalizationHelper>().ApplyTranslation(Localization.Find("CameraRotationControl"), null);
		text.text = string.Format(text.text, (!SettingsData.Data.rotateControlsWithCamera) ? "A" : "B");
	}

	// Token: 0x04001B38 RID: 6968
	public static LevelGameOverGUI Current;

	// Token: 0x04001B39 RID: 6969
	[SerializeField]
	private Image youDiedText;

	// Token: 0x04001B3A RID: 6970
	[Space(10f)]
	[SerializeField]
	private CanvasGroup cardCanvasGroup;

	// Token: 0x04001B3B RID: 6971
	[Space(10f)]
	[SerializeField]
	private CanvasGroup helpCanvasGroup;

	// Token: 0x04001B3C RID: 6972
	[Space(10f)]
	[SerializeField]
	private Image bossPortraitImage;

	// Token: 0x04001B3D RID: 6973
	[SerializeField]
	private Text bossQuoteText;

	// Token: 0x04001B3E RID: 6974
	[SerializeField]
	private LocalizationHelper bossQuoteLocalization;

	// Token: 0x04001B3F RID: 6975
	[Space(10f)]
	[SerializeField]
	private Text[] menuItems;

	// Token: 0x04001B40 RID: 6976
	[SerializeField]
	private LevelGameOverGUI.TimelineObjects timeline;

	// Token: 0x04001B41 RID: 6977
	[SerializeField]
	private GameObject timelineObj;

	// Token: 0x04001B42 RID: 6978
	[SerializeField]
	private Sprite timelineSecret;

	// Token: 0x04001B43 RID: 6979
	[SerializeField]
	private LevelEquipUI equipUI;

	// Token: 0x04001B44 RID: 6980
	[SerializeField]
	private GameObject equipToolTip;

	// Token: 0x04001B45 RID: 6981
	private LevelGameOverGUI.State state;

	// Token: 0x04001B46 RID: 6982
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001B47 RID: 6983
	private CanvasGroup canvasGroup;

	// Token: 0x04001B48 RID: 6984
	private int selection;

	// Token: 0x04001B49 RID: 6985
	[SerializeField]
	private LocalizationHelper retryLocHelper;

	// Token: 0x0200047F RID: 1151
	private enum State
	{
		// Token: 0x04001B4B RID: 6987
		Init,
		// Token: 0x04001B4C RID: 6988
		Ready,
		// Token: 0x04001B4D RID: 6989
		Exiting
	}

	// Token: 0x02000480 RID: 1152
	[Serializable]
	public class TimelineObjects
	{
		// Token: 0x060011D2 RID: 4562 RVA: 0x000A6888 File Offset: 0x000A4C88
		public void Setup(LevelGameOverGUI gui, Level.Timeline properties)
		{
			int num = 0;
			foreach (Level.Timeline.Event @event in properties.events)
			{
				RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.line);
				rectTransform.SetParent(this.line.parent, false);
				rectTransform.SetAsFirstSibling();
				rectTransform.name = "Line " + num++;
				Vector3 localPosition = Vector3.Lerp(this.end.localPosition, this.start.localPosition, @event.percentage);
				localPosition.y -= 7f;
				rectTransform.localPosition = localPosition;
			}
			this.line.gameObject.SetActive(false);
			Image image = (!PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman) : this.chalice;
			float num2 = (!PlayerManager.player1IsMugman) ? properties.cuphead : properties.mugman;
			gui.StartCoroutine(this.timelineIcon_cr(image, num2 / properties.health));
			Image image2 = null;
			if (PlayerManager.Multiplayer)
			{
				image2 = ((!PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice) ? ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead) : this.chalice);
				float num3 = (!PlayerManager.player1IsMugman) ? properties.mugman : properties.cuphead;
				gui.StartCoroutine(this.timelineIcon_cr(image2, num3 / properties.health));
			}
			this.cuphead.gameObject.SetActive(image == this.cuphead || image2 == this.cuphead);
			this.mugman.gameObject.SetActive(image == this.mugman || image2 == this.mugman);
			this.chalice.gameObject.SetActive(image == this.chalice || image2 == this.chalice);
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x000A6AF8 File Offset: 0x000A4EF8
		private IEnumerator timelineIcon_cr(Image icon, float percent)
		{
			Color startColor = new Color(1f, 1f, 1f, 0f);
			Color endColor = new Color(1f, 1f, 1f, 1f);
			float t = 0f;
			Vector3 endPosition = Vector3.Lerp(this.start.localPosition, this.end.localPosition, percent);
			icon.rectTransform.localPosition = this.start.localPosition;
			while (t < 2f)
			{
				float val = t / 2f;
				Vector3 newPosition = Vector3.Lerp(this.start.localPosition, endPosition, EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, val));
				icon.rectTransform.localPosition = newPosition;
				icon.color = Color.Lerp(startColor, endColor, val * 8f);
				t += Time.deltaTime;
				yield return null;
			}
			icon.rectTransform.localPosition = endPosition;
			yield break;
		}

		// Token: 0x04001B4E RID: 6990
		public RectTransform timeline;

		// Token: 0x04001B4F RID: 6991
		public RectTransform line;

		// Token: 0x04001B50 RID: 6992
		[Header("Players")]
		public Image cuphead;

		// Token: 0x04001B51 RID: 6993
		public Image mugman;

		// Token: 0x04001B52 RID: 6994
		public Image chalice;

		// Token: 0x04001B53 RID: 6995
		[Header("Positions")]
		public Transform start;

		// Token: 0x04001B54 RID: 6996
		public Transform end;

		// Token: 0x04001B55 RID: 6997
		private LevelGameOverGUI gui;
	}
}
