using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009AF RID: 2479
public class SlotSelectScreenSlot : AbstractMonoBehaviour
{
	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06003A18 RID: 14872 RVA: 0x002109F8 File Offset: 0x0020EDF8
	// (set) Token: 0x06003A19 RID: 14873 RVA: 0x00210A00 File Offset: 0x0020EE00
	public bool IsEmpty { get; private set; }

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06003A1A RID: 14874 RVA: 0x00210A09 File Offset: 0x0020EE09
	// (set) Token: 0x06003A1B RID: 14875 RVA: 0x00210A11 File Offset: 0x0020EE11
	public bool isPlayer1Mugman { get; private set; }

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06003A1C RID: 14876 RVA: 0x00210A1A File Offset: 0x0020EE1A
	// (set) Token: 0x06003A1D RID: 14877 RVA: 0x00210A22 File Offset: 0x0020EE22
	public Image noise
	{
		get
		{
			return this.noiseImage;
		}
		set
		{
			this.noiseImage = value;
		}
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x00210A2C File Offset: 0x0020EE2C
	public void Init(int slotNumber)
	{
		PlayerData dataForSlot = PlayerData.GetDataForSlot(slotNumber);
		this.cuphead.SetActive(false);
		this.mugman.SetActive(false);
		if (!dataForSlot.GetMapData(Scenes.scene_map_world_1).sessionStarted && !dataForSlot.IsTutorialCompleted && dataForSlot.CountLevelsCompleted(Level.world1BossLevels) == 0)
		{
			this.emptyChild.gameObject.SetActive(true);
			this.mainChild.gameObject.SetActive(false);
			this.mainDLCChild.gameObject.SetActive(false);
			this.IsEmpty = true;
			return;
		}
		this.IsEmpty = false;
		this.emptyChild.gameObject.SetActive(false);
		this.mainChild.gameObject.SetActive(true);
		Localization.Translation translation;
		if (slotNumber == 0)
		{
			if (dataForSlot.isPlayer1Mugman)
			{
				translation = Localization.Translate("TitleScreenMugmanSlot1");
			}
			else
			{
				translation = Localization.Translate("TitleScreenSlot1");
			}
		}
		else if (slotNumber == 1)
		{
			if (dataForSlot.isPlayer1Mugman)
			{
				translation = Localization.Translate("TitleScreenMugmanSlot2");
			}
			else
			{
				translation = Localization.Translate("TitleScreenSlot2");
			}
		}
		else if (dataForSlot.isPlayer1Mugman)
		{
			translation = Localization.Translate("TitleScreenMugmanSlot3");
		}
		else
		{
			translation = Localization.Translate("TitleScreenSlot3");
		}
		this.slotTitle.text = translation.text;
		this.slotSeparator.font = translation.fonts.fontAsset;
		this.slotTitle.font = translation.fonts.fontAsset;
		this.isPlayer1Mugman = dataForSlot.isPlayer1Mugman;
		this.isExpert = dataForSlot.IsHardModeAvailable;
		this.isExpertDLC = dataForSlot.IsHardModeAvailableDLC;
		int num = Mathf.RoundToInt(dataForSlot.GetCompletionPercentage());
		this.isComplete = (num == 200);
		int num2 = Mathf.RoundToInt(dataForSlot.GetCompletionPercentageDLC());
		this.isCompleteDLC = (num2 == 100);
		this.slotPercentage.text = num + num2 + "%";
		if (DLCManager.DLCEnabled())
		{
			this.slotPercentageSelectedBase.text = num + "%";
			this.slotPercentageSelectedDLC.text = num2 + "%";
		}
		Scenes currentMap = dataForSlot.CurrentMap;
		switch (currentMap)
		{
		case Scenes.scene_map_world_2:
			translation = Localization.Translate("TitleScreenWorld2");
			break;
		case Scenes.scene_map_world_3:
			translation = Localization.Translate("TitleScreenWorld3");
			break;
		case Scenes.scene_map_world_4:
			translation = Localization.Translate("TitleScreenWorld4");
			break;
		default:
			if (currentMap != Scenes.scene_map_world_DLC)
			{
				translation = Localization.Translate("TitleScreenWorld1");
			}
			else if (DLCManager.DLCEnabled())
			{
				translation = Localization.Translate("TitleScreenWorldDLC");
			}
			else
			{
				translation = Localization.Translate("TitleScreenWorld1");
			}
			break;
		}
		this.worldMapText.text = translation.text;
		this.worldMapText.font = translation.fonts.fontAsset;
		this.worldMapTextDLC.text = translation.text;
		this.worldMapTextDLC.font = translation.fonts.fontAsset;
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x00210D4C File Offset: 0x0020F14C
	public void SetSelected(bool selected)
	{
		if (DLCManager.DLCEnabled() && !this.IsEmpty)
		{
			this.mainChild.gameObject.SetActive(!selected);
			this.mainDLCChild.gameObject.SetActive(selected);
		}
		this.slotTitle.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.slotSeparator.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.slotPercentage.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.worldMapText.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.emptyText.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.boxImage.sprite = ((!selected) ? this.unselectedBoxSprite : ((!this.isPlayer1Mugman) ? this.selectedBoxSprite : this.selectedBoxSpriteMugman));
		if (!this.IsEmpty && this.isComplete)
		{
			this.starImage.sprite = ((!selected) ? this.unselectedBoxSpriteComplete : this.selectedBoxSpriteComplete);
			this.starImage.gameObject.SetActive(true);
		}
		else if (!this.IsEmpty && this.isExpert)
		{
			this.starImage.sprite = ((!selected) ? this.unselectedBoxSpriteExpert : this.selectedBoxSpriteExpert);
			this.starImage.gameObject.SetActive(true);
		}
		else
		{
			this.starImage.gameObject.SetActive(false);
		}
		if (!this.IsEmpty && this.isCompleteDLC)
		{
			this.starImageDLC.sprite = ((!selected) ? this.unselectedBoxSpriteCompleteDLC : this.selectedBoxSpriteCompleteDLC);
			this.starImageDLC.gameObject.SetActive(true);
		}
		else if (!this.IsEmpty && this.isExpertDLC)
		{
			this.starImageDLC.sprite = ((!selected) ? this.unselectedBoxSpriteExpertDLC : this.selectedBoxSpriteExpertDLC);
			this.starImageDLC.gameObject.SetActive(true);
		}
		else
		{
			this.starImageDLC.gameObject.SetActive(false);
		}
		if (this.starImage.gameObject.activeInHierarchy && !this.starImageDLC.gameObject.activeInHierarchy)
		{
			this.starImage.transform.position = this.starImageDLC.transform.position;
		}
		this.noiseImage.sprite = ((!selected) ? this.unselectedNoise : ((!this.isPlayer1Mugman) ? this.selectedNoise : this.selectedNoiseMugman));
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x00211051 File Offset: 0x0020F451
	public string GetSlotTitle()
	{
		return this.slotTitle.text;
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x0021105E File Offset: 0x0020F45E
	public TMP_FontAsset GetSlotTitleFont()
	{
		return this.slotTitle.font;
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x0021106B File Offset: 0x0020F46B
	public string GetSlotSeparator()
	{
		return this.slotSeparator.text;
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x00211078 File Offset: 0x0020F478
	public TMP_FontAsset GetSlotSeparatorFont()
	{
		return this.slotSeparator.font;
	}

	// Token: 0x06003A24 RID: 14884 RVA: 0x00211085 File Offset: 0x0020F485
	public string GetSlotPercentage()
	{
		return this.slotPercentage.text;
	}

	// Token: 0x06003A25 RID: 14885 RVA: 0x00211092 File Offset: 0x0020F492
	public TMP_FontAsset GetSlotPercentageFont()
	{
		return this.slotPercentage.font;
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x002110A0 File Offset: 0x0020F4A0
	public void EnterSelectMenu()
	{
		this.selectingMugman = this.isPlayer1Mugman;
		if (this.selectingMugman)
		{
			this.mugman.SetActive(true);
			this.mugmanSelect.Play("Zoom_In");
		}
		else
		{
			this.cuphead.SetActive(true);
			this.cupheadSelect.Play("Zoom_In");
		}
		this.mainDLCChild.gameObject.SetActive(false);
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x00211114 File Offset: 0x0020F514
	public void SwapSprite()
	{
		this.noiseImage.enabled = false;
		this.selectingMugman = !this.selectingMugman;
		this.cuphead.SetActive(!this.selectingMugman);
		this.mugman.SetActive(this.selectingMugman);
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x00211161 File Offset: 0x0020F561
	public void StopSelectingPlayer()
	{
		base.StartCoroutine(this.player_zoomout_cr());
	}

	// Token: 0x06003A29 RID: 14889 RVA: 0x00211170 File Offset: 0x0020F570
	private IEnumerator player_zoomout_cr()
	{
		if (this.selectingMugman)
		{
			this.mugmanSelect.Play("Zoom_Out");
			yield return this.mugmanSelect.WaitForAnimationToEnd(this, "Zoom_Out", false, true);
			this.mugman.SetActive(false);
		}
		else
		{
			this.cupheadSelect.Play("Zoom_Out");
			yield return this.cupheadSelect.WaitForAnimationToEnd(this, "Zoom_Out", false, true);
			this.cuphead.SetActive(false);
		}
		yield return null;
		this.selectingMugman = this.isPlayer1Mugman;
		this.noiseImage.enabled = true;
		yield break;
	}

	// Token: 0x06003A2A RID: 14890 RVA: 0x0021118C File Offset: 0x0020F58C
	public void PlayAnimation(int slotNumber)
	{
		this.isPlayer1Mugman = this.selectingMugman;
		PlayerData dataForSlot = PlayerData.GetDataForSlot(slotNumber);
		Animator animator = (!this.isPlayer1Mugman) ? this.cupheadAnimator : this.mugmanAnimator;
		if (dataForSlot.IsHardModeAvailable)
		{
			if (dataForSlot.NumCoinsCollected >= 40 && dataForSlot.NumSupers(PlayerId.PlayerOne) >= 3)
			{
				animator.Play("100Percent");
			}
			else
			{
				animator.Play("DefeatedDevil");
			}
		}
		else
		{
			animator.Play("Default");
		}
	}

	// Token: 0x04004230 RID: 16944
	[SerializeField]
	private RectTransform emptyChild;

	// Token: 0x04004231 RID: 16945
	[SerializeField]
	private RectTransform mainChild;

	// Token: 0x04004232 RID: 16946
	[SerializeField]
	private RectTransform mainDLCChild;

	// Token: 0x04004233 RID: 16947
	[SerializeField]
	private TMP_Text worldMapText;

	// Token: 0x04004234 RID: 16948
	[SerializeField]
	private TMP_Text worldMapTextDLC;

	// Token: 0x04004235 RID: 16949
	[SerializeField]
	private Image boxImage;

	// Token: 0x04004236 RID: 16950
	[SerializeField]
	private Image starImage;

	// Token: 0x04004237 RID: 16951
	[SerializeField]
	private Image starImageDLC;

	// Token: 0x04004238 RID: 16952
	[SerializeField]
	private Image starImageSelectedBase;

	// Token: 0x04004239 RID: 16953
	[SerializeField]
	private Image starImageSelectedDLC;

	// Token: 0x0400423A RID: 16954
	[SerializeField]
	private Image noiseImage;

	// Token: 0x0400423B RID: 16955
	[SerializeField]
	private Sprite unselectedBoxSprite;

	// Token: 0x0400423C RID: 16956
	[SerializeField]
	private Sprite unselectedBoxSpriteExpert;

	// Token: 0x0400423D RID: 16957
	[SerializeField]
	private Sprite unselectedBoxSpriteComplete;

	// Token: 0x0400423E RID: 16958
	[SerializeField]
	private Sprite unselectedBoxSpriteExpertDLC;

	// Token: 0x0400423F RID: 16959
	[SerializeField]
	private Sprite unselectedBoxSpriteCompleteDLC;

	// Token: 0x04004240 RID: 16960
	[SerializeField]
	private Sprite unselectedNoise;

	// Token: 0x04004241 RID: 16961
	[SerializeField]
	private Sprite selectedBoxSpriteMugman;

	// Token: 0x04004242 RID: 16962
	[SerializeField]
	private Sprite selectedBoxSprite;

	// Token: 0x04004243 RID: 16963
	[SerializeField]
	private Sprite selectedBoxSpriteExpert;

	// Token: 0x04004244 RID: 16964
	[SerializeField]
	private Sprite selectedBoxSpriteComplete;

	// Token: 0x04004245 RID: 16965
	[SerializeField]
	private Sprite selectedBoxSpriteExpertDLC;

	// Token: 0x04004246 RID: 16966
	[SerializeField]
	private Sprite selectedBoxSpriteCompleteDLC;

	// Token: 0x04004247 RID: 16967
	[SerializeField]
	private Sprite selectedNoiseMugman;

	// Token: 0x04004248 RID: 16968
	[SerializeField]
	private Sprite selectedNoise;

	// Token: 0x04004249 RID: 16969
	[SerializeField]
	private GameObject cuphead;

	// Token: 0x0400424A RID: 16970
	[SerializeField]
	private Animator cupheadSelect;

	// Token: 0x0400424B RID: 16971
	[SerializeField]
	private Animator cupheadAnimator;

	// Token: 0x0400424C RID: 16972
	[SerializeField]
	private GameObject mugman;

	// Token: 0x0400424D RID: 16973
	[SerializeField]
	private Animator mugmanSelect;

	// Token: 0x0400424E RID: 16974
	[SerializeField]
	private Animator mugmanAnimator;

	// Token: 0x0400424F RID: 16975
	[SerializeField]
	private TMP_Text slotTitle;

	// Token: 0x04004250 RID: 16976
	[SerializeField]
	private TMP_Text slotSeparator;

	// Token: 0x04004251 RID: 16977
	[SerializeField]
	private TMP_Text slotPercentage;

	// Token: 0x04004252 RID: 16978
	[SerializeField]
	private TMP_Text slotPercentageSelectedBase;

	// Token: 0x04004253 RID: 16979
	[SerializeField]
	private TMP_Text slotPercentageSelectedDLC;

	// Token: 0x04004254 RID: 16980
	[SerializeField]
	private Text emptyText;

	// Token: 0x04004255 RID: 16981
	[SerializeField]
	private Color selectedTextColor;

	// Token: 0x04004256 RID: 16982
	[SerializeField]
	private Color unselectedTextColor;

	// Token: 0x04004259 RID: 16985
	private bool selectingMugman;

	// Token: 0x0400425A RID: 16986
	private bool isExpert;

	// Token: 0x0400425B RID: 16987
	private bool isExpertDLC;

	// Token: 0x0400425C RID: 16988
	private bool isComplete;

	// Token: 0x0400425D RID: 16989
	private bool isCompleteDLC;
}
