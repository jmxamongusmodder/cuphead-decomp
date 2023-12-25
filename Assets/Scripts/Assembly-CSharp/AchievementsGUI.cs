using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x02000452 RID: 1106
public class AchievementsGUI : AbstractMonoBehaviour
{
	// Token: 0x1700029E RID: 670
	// (get) Token: 0x060010A1 RID: 4257 RVA: 0x0009FA03 File Offset: 0x0009DE03
	// (set) Token: 0x060010A2 RID: 4258 RVA: 0x0009FA0B File Offset: 0x0009DE0B
	public bool achievementsMenuOpen { get; private set; }

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x060010A3 RID: 4259 RVA: 0x0009FA14 File Offset: 0x0009DE14
	// (set) Token: 0x060010A4 RID: 4260 RVA: 0x0009FA1C File Offset: 0x0009DE1C
	public bool inputEnabled { get; private set; }

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x060010A5 RID: 4261 RVA: 0x0009FA25 File Offset: 0x0009DE25
	// (set) Token: 0x060010A6 RID: 4262 RVA: 0x0009FA2D File Offset: 0x0009DE2D
	public bool justClosed { get; private set; }

	// Token: 0x060010A7 RID: 4263 RVA: 0x0009FA38 File Offset: 0x0009DE38
	protected override void Awake()
	{
		base.Awake();
		this.defaultAtlas = AssetLoader<SpriteAtlas>.GetCachedAsset("Achievements");
		this.stringBuilder = new StringBuilder();
		this.background.sprite = this.defaultAtlas.GetSprite("cheev_bg");
		this.unearnedBackground.sprite = this.defaultAtlas.GetSprite("cheev_card_unearned");
		this.achievementsMenuOpen = false;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x0009FABF File Offset: 0x0009DEBF
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0009FAD0 File Offset: 0x0009DED0
	private void Update()
	{
		this.justClosed = false;
		this.timeSinceStart += Time.deltaTime;
		if (this.timeSinceStart < 0.25f)
		{
			return;
		}
		if (this.activeNavigationButton != CupheadButton.None && this.input.GetButtonUp(this.activeNavigationButton))
		{
			this.activeNavigationButton = CupheadButton.None;
		}
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.Cancel))
		{
			AudioManager.Play("level_menu_select");
			this.HideAchievements();
		}
		if (this.activeNavigationButton == CupheadButton.None)
		{
			if (this.GetButtonDown(CupheadButton.MenuUp))
			{
				if (this.achievementIndex.y == 0)
				{
					this.rowOffset = this.currentGridSize.y - AchievementsGUI.VisualGridSize.y;
					this.cursorIndex.y = AchievementsGUI.VisualGridSize.y - 1;
					this.achievementIndex.y = this.currentGridSize.y - 1;
				}
				else
				{
					if (this.cursorIndex.y == 0)
					{
						this.rowOffset--;
					}
					this.cursorIndex.y = Mathf.Max(this.cursorIndex.y - 1, 0);
					this.achievementIndex.y = Mathf.Max(this.achievementIndex.y - 1, 0);
				}
				this.refreshIcons();
				this.updateSelection();
			}
			else if (this.GetButtonDown(CupheadButton.MenuDown))
			{
				if (this.achievementIndex.y == this.currentGridSize.y - 1)
				{
					this.rowOffset = 0;
					this.cursorIndex.y = 0;
					this.achievementIndex.y = 0;
				}
				else
				{
					if (this.cursorIndex.y == AchievementsGUI.VisualGridSize.y - 1)
					{
						this.rowOffset = Mathf.Min(this.rowOffset + 1, this.currentGridSize.y - AchievementsGUI.VisualGridSize.y);
					}
					this.cursorIndex.y = Mathf.Min(this.cursorIndex.y + 1, AchievementsGUI.VisualGridSize.y - 1);
					this.achievementIndex.y = Mathf.Min(this.achievementIndex.y + 1, this.currentGridSize.y - 1);
				}
				this.refreshIcons();
				this.updateSelection();
			}
			else if (this.GetButtonDown(CupheadButton.MenuLeft))
			{
				this.cursorIndex.x = this.cursorIndex.x - 1;
				if (this.cursorIndex.x < 0)
				{
					this.cursorIndex.x = AchievementsGUI.VisualGridSize.x - 1;
				}
				this.achievementIndex.x = this.achievementIndex.x - 1;
				if (this.achievementIndex.x < 0)
				{
					this.achievementIndex.x = this.currentGridSize.x - 1;
				}
				this.updateSelection();
			}
			else if (this.GetButtonDown(CupheadButton.MenuRight))
			{
				this.cursorIndex.x = this.cursorIndex.x + 1;
				if (this.cursorIndex.x >= AchievementsGUI.VisualGridSize.x)
				{
					this.cursorIndex.x = 0;
				}
				this.achievementIndex.x = this.achievementIndex.x + 1;
				if (this.achievementIndex.x >= this.currentGridSize.x)
				{
					this.achievementIndex.x = 0;
				}
				this.updateSelection();
			}
		}
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x0009FE70 File Offset: 0x0009E270
	public void ShowAchievements()
	{
		this.handleDLCStatus();
		this.cursorIndex = Vector2Int.zero;
		this.achievementIndex = Vector2Int.zero;
		this.rowOffset = 0;
		this.refreshIcons();
		this.updateSelection();
		if (this.dlcEnabled)
		{
			this.arrowCoroutine = base.StartCoroutine(this.arrow_cr());
		}
		this.timeSinceStart = 0f;
		this.achievementsMenuOpen = true;
		this.canvasGroup.alpha = 1f;
		base.FrameDelayedCallback(new Action(this.interactable), 1);
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0009FF00 File Offset: 0x0009E300
	public void HideAchievements()
	{
		if (this.dlcEnabled)
		{
			base.StopCoroutine(this.arrowCoroutine);
			this.arrowCoroutine = null;
		}
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		this.achievementsMenuOpen = false;
		this.justClosed = true;
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0009FF68 File Offset: 0x0009E368
	private void interactable()
	{
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x0009FF8C File Offset: 0x0009E38C
	private void updateSelection()
	{
		AchievementIcon achievementIcon = this.iconRows[this.cursorIndex.y].achievementIcons[this.cursorIndex.x];
		this.cursor.position = achievementIcon.transform.position;
		int num = this.achievementIndex.y * this.currentGridSize.x + this.achievementIndex.x;
		LocalAchievementsManager.Achievement achievement = (LocalAchievementsManager.Achievement)num;
		string text = achievement.ToString();
		bool flag = LocalAchievementsManager.IsAchievementUnlocked(achievement);
		bool flag2 = LocalAchievementsManager.IsHiddenAchievement(achievement);
		if (flag || !flag2)
		{
			string key = "Achievement" + text + "Title";
			this.titleLocalization.ApplyTranslation(Localization.Find(key), null);
			string key2 = "Achievement" + text + "Desc";
			this.descriptionLocalization.ApplyTranslation(Localization.Find(key2), null);
		}
		else
		{
			this.titleText.text = AchievementsGUI.TitleHidden;
			this.titleText.font = FontLoader.GetFont(AchievementsGUI.TitleHiddenFont);
			this.descriptionText.text = AchievementsGUI.DescriptionHidden;
			this.descriptionText.font = FontLoader.GetFont(AchievementsGUI.DescriptionHiddenFont);
		}
		string text2 = text;
		if (flag)
		{
			text2 += "_earned";
		}
		Sprite achievementSprite = this.getAchievementSprite(text2, achievement);
		this.largeIcon.sprite = achievementSprite;
		this.titleText.color = ((!flag) ? AchievementsGUI.LockedTextColor : AchievementsGUI.UnlockedTextColor);
		this.descriptionText.color = ((!flag) ? AchievementsGUI.LockedTextColor : AchievementsGUI.UnlockedTextColor);
		this.unearnedBackground.enabled = !flag;
		this.noise.sprite = this.getSprite((!flag) ? "cheev_card_noise_unearned" : "cheev_card_noise_earned", this.defaultAtlas);
		AudioManager.Play("level_menu_move");
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x000A0177 File Offset: 0x0009E577
	protected bool GetButtonDown(CupheadButton button)
	{
		if (this.input.GetButtonDown(button))
		{
			this.activeNavigationButton = button;
			return true;
		}
		return false;
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x000A0194 File Offset: 0x0009E594
	private void handleDLCStatus()
	{
		this.dlcEnabled = DLCManager.DLCEnabled();
		this.currentGridSize = ((!this.dlcEnabled) ? AchievementsGUI.VisualGridSize : AchievementsGUI.GridSize);
		if (this.dlcEnabled && this.dlcAtlas == null)
		{
			this.dlcAtlas = AssetLoader<SpriteAtlas>.GetCachedAsset("Achievements_DLC");
		}
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x000A01F8 File Offset: 0x0009E5F8
	private IEnumerator arrow_cr()
	{
		int index = 0;
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.083333336f, true);
		for (;;)
		{
			this.topArrow.sprite = this.arrowSprites[index];
			index = MathUtilities.NextIndex(index, this.arrowSprites.Length);
			this.bottomArrow.sprite = this.arrowSprites[MathUtilities.NextIndex(index, this.arrowSprites.Length)];
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x000A0214 File Offset: 0x0009E614
	private void refreshIcons()
	{
		if (this.dlcEnabled)
		{
			this.topArrow.enabled = (this.rowOffset != 0);
			this.bottomArrow.enabled = (this.rowOffset != 2);
		}
		else
		{
			Behaviour behaviour = this.topArrow;
			bool enabled = false;
			this.bottomArrow.enabled = enabled;
			behaviour.enabled = enabled;
		}
		int num = this.rowOffset * this.currentGridSize.x;
		foreach (AchievementsGUI.IconRow iconRow in this.iconRows)
		{
			foreach (AchievementIcon achievementIcon in iconRow.achievementIcons)
			{
				this.stringBuilder.Length = 0;
				LocalAchievementsManager.Achievement achievement = (LocalAchievementsManager.Achievement)num;
				this.stringBuilder.Append(AchievementsGUI.AchievementNames[num]);
				if (LocalAchievementsManager.IsAchievementUnlocked(achievement))
				{
					this.stringBuilder.Append("_earned");
				}
				this.stringBuilder.Append("_sm");
				Sprite achievementSprite = this.getAchievementSprite(this.stringBuilder.ToString(), achievement);
				achievementIcon.SetIcon(achievementSprite);
				num++;
			}
		}
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x000A034C File Offset: 0x0009E74C
	private Sprite getSprite(string spriteName, SpriteAtlas atlas)
	{
		Sprite sprite;
		if (!this.spriteCache.TryGetValue(spriteName, out sprite))
		{
			sprite = atlas.GetSprite(spriteName);
			this.spriteCache.Add(spriteName, sprite);
		}
		return sprite;
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x000A0384 File Offset: 0x0009E784
	private Sprite getAchievementSprite(string spriteName, LocalAchievementsManager.Achievement achievement)
	{
		SpriteAtlas atlas;
		if (Array.IndexOf<LocalAchievementsManager.Achievement>(LocalAchievementsManager.DLCAchievements, achievement) >= 0)
		{
			atlas = this.dlcAtlas;
		}
		else
		{
			atlas = this.defaultAtlas;
		}
		return this.getSprite(spriteName, atlas);
	}

	// Token: 0x040019D4 RID: 6612
	private static readonly string[] AchievementNames = Enum.GetNames(typeof(LocalAchievementsManager.Achievement));

	// Token: 0x040019D5 RID: 6613
	private static readonly Color UnlockedTextColor = new Color(0.9098039f, 0.8235294f, 0.68235296f);

	// Token: 0x040019D6 RID: 6614
	private static readonly Color LockedTextColor = new Color(0.27058825f, 0.26666668f, 0.2627451f);

	// Token: 0x040019D7 RID: 6615
	private static readonly string TitleHidden = "? ? ? ? ? ? ?";

	// Token: 0x040019D8 RID: 6616
	private static readonly FontLoader.FontType TitleHiddenFont = FontLoader.FontType.CupheadMemphis_Medium_merged;

	// Token: 0x040019D9 RID: 6617
	private static readonly string DescriptionHidden = "?  ?  ?  ?  ?  ?";

	// Token: 0x040019DA RID: 6618
	private static readonly FontLoader.FontType DescriptionHiddenFont = FontLoader.FontType.CupheadVogue_Bold_merged;

	// Token: 0x040019DB RID: 6619
	private static readonly Vector2Int GridSize = new Vector2Int(7, 6);

	// Token: 0x040019DC RID: 6620
	private static readonly Vector2Int VisualGridSize = new Vector2Int(7, 4);

	// Token: 0x040019DD RID: 6621
	[SerializeField]
	private AchievementsGUI.IconRow[] iconRows;

	// Token: 0x040019DE RID: 6622
	[SerializeField]
	private RectTransform cursor;

	// Token: 0x040019DF RID: 6623
	[SerializeField]
	private Image topArrow;

	// Token: 0x040019E0 RID: 6624
	[SerializeField]
	private Image bottomArrow;

	// Token: 0x040019E1 RID: 6625
	[SerializeField]
	private Image background;

	// Token: 0x040019E2 RID: 6626
	[SerializeField]
	private Image unearnedBackground;

	// Token: 0x040019E3 RID: 6627
	[SerializeField]
	private Text titleText;

	// Token: 0x040019E4 RID: 6628
	[SerializeField]
	private Text descriptionText;

	// Token: 0x040019E5 RID: 6629
	[SerializeField]
	private LocalizationHelper titleLocalization;

	// Token: 0x040019E6 RID: 6630
	[SerializeField]
	private LocalizationHelper descriptionLocalization;

	// Token: 0x040019E7 RID: 6631
	[SerializeField]
	private Image largeIcon;

	// Token: 0x040019E8 RID: 6632
	[SerializeField]
	private Image noise;

	// Token: 0x040019E9 RID: 6633
	[SerializeField]
	private Sprite[] arrowSprites;

	// Token: 0x040019EA RID: 6634
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x040019EB RID: 6635
	private float timeSinceStart;

	// Token: 0x040019EC RID: 6636
	private Vector2Int achievementIndex;

	// Token: 0x040019ED RID: 6637
	private Vector2Int cursorIndex;

	// Token: 0x040019EE RID: 6638
	private int rowOffset;

	// Token: 0x040019EF RID: 6639
	private SpriteAtlas defaultAtlas;

	// Token: 0x040019F0 RID: 6640
	private SpriteAtlas dlcAtlas;

	// Token: 0x040019F1 RID: 6641
	private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

	// Token: 0x040019F2 RID: 6642
	private CupheadButton activeNavigationButton = CupheadButton.None;

	// Token: 0x040019F3 RID: 6643
	private StringBuilder stringBuilder;

	// Token: 0x040019F4 RID: 6644
	private Coroutine arrowCoroutine;

	// Token: 0x040019F5 RID: 6645
	private bool dlcEnabled;

	// Token: 0x040019F6 RID: 6646
	private Vector2Int currentGridSize;

	// Token: 0x040019F7 RID: 6647
	private CanvasGroup canvasGroup;

	// Token: 0x02000453 RID: 1107
	[Serializable]
	public class IconRow
	{
		// Token: 0x040019FB RID: 6651
		public AchievementIcon[] achievementIcons;
	}
}
