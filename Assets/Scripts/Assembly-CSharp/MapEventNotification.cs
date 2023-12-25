using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200099E RID: 2462
public class MapEventNotification : AbstractMonoBehaviour
{
	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x060039B0 RID: 14768 RVA: 0x0020C145 File Offset: 0x0020A545
	// (set) Token: 0x060039B1 RID: 14769 RVA: 0x0020C14C File Offset: 0x0020A54C
	public static MapEventNotification Current { get; private set; }

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x060039B2 RID: 14770 RVA: 0x0020C154 File Offset: 0x0020A554
	// (set) Token: 0x060039B3 RID: 14771 RVA: 0x0020C15C File Offset: 0x0020A55C
	public bool showing { get; set; }

	// Token: 0x060039B4 RID: 14772 RVA: 0x0020C168 File Offset: 0x0020A568
	protected override void Awake()
	{
		base.Awake();
		MapEventNotification.Current = this;
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		for (int i = 0; i < this.sparkleAnimatorsContract.Length; i++)
		{
			this.sparkleAnimatorsContract[i] = UnityEngine.Object.Instantiate<GameObject>(this.sparklePrefab, this.sparkleTransformContract).GetComponent<Animator>();
		}
		for (int j = 0; j < this.sparkleAnimatorsCoin1.Length; j++)
		{
			this.sparkleAnimatorsCoin1[j] = UnityEngine.Object.Instantiate<GameObject>(this.sparklePrefab, this.sparkleTransformCoin1).GetComponent<Animator>();
		}
		for (int k = 0; k < this.sparkleAnimatorsCoin2.Length; k++)
		{
			this.sparkleAnimatorsCoin2[k] = UnityEngine.Object.Instantiate<GameObject>(this.sparklePrefab, this.sparkleTransformCoin2).GetComponent<Animator>();
		}
		for (int l = 0; l < this.sparkleAnimatorsCoin3.Length; l++)
		{
			this.sparkleAnimatorsCoin3[l] = UnityEngine.Object.Instantiate<GameObject>(this.sparklePrefab, this.sparkleTransformCoin3).GetComponent<Animator>();
		}
		this.dlcUI = UnityEngine.Object.Instantiate<GameObject>(this.dlcUIPrefab, this.dlcUIRoot).GetComponent<MapDLCUI>();
		this.dlcUI.Init(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060039B5 RID: 14773 RVA: 0x0020C2B9 File Offset: 0x0020A6B9
	private void OnDestroy()
	{
		if (MapEventNotification.Current == this)
		{
			MapEventNotification.Current = null;
		}
	}

	// Token: 0x060039B6 RID: 14774 RVA: 0x0020C2D4 File Offset: 0x0020A6D4
	private void Update()
	{
		if (this.superShowing)
		{
			if (this.input.GetAnyButtonDown())
			{
				base.StartCoroutine(this.tweenOut_cr(1.5f));
				base.animator.SetTrigger("hide_super");
				this.superShowing = false;
			}
			this.timeBeforeNextSparkleCoin1 -= CupheadTime.Delta;
			for (int i = 0; i < this.sparkleAnimatorsCoin1.Length; i++)
			{
				if (this.timeBeforeNextSparkleCoin1 <= 0f)
				{
					if (this.sparkleAnimatorsCoin1[i].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleCoin1 = this.timeBetweenSparkle;
						this.sparkleAnimatorsCoin1[i].transform.position = new Vector3(this.sparkleTransformCoin1.position.x + UnityEngine.Random.Range(this.sparkleTransformCoin1.sizeDelta.x * -0.5f, this.sparkleTransformCoin1.sizeDelta.x * 0.5f), this.sparkleTransformCoin1.position.y + UnityEngine.Random.Range(this.sparkleTransformCoin1.sizeDelta.y * -0.5f, this.sparkleTransformCoin1.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsCoin1[i].SetTrigger(UnityEngine.Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
		}
		if (this.tooltipShowing && this.input.GetAnyButtonDown())
		{
			base.StartCoroutine(this.tweenOut_cr(1.5f));
			base.animator.SetTrigger("hide_tooltip");
			this.tooltipShowing = false;
		}
		if (this.tooltipEquipShowing && this.input.GetButtonDown(CupheadButton.EquipMenu))
		{
			base.StartCoroutine(this.tweenOut_cr(0.5f));
			base.animator.SetTrigger("hide_tooltip");
			this.tooltipShowing = false;
		}
		if (this.coinShowing)
		{
			if (this.input.GetAnyButtonDown())
			{
				base.StartCoroutine(this.tweenOut_cr(1.5f));
				base.animator.SetTrigger("hide_coin");
				this.coinShowing = false;
			}
			this.timeBeforeNextSparkleCoin1 -= CupheadTime.Delta;
			this.timeBeforeNextSparkleCoin2 -= CupheadTime.Delta;
			this.timeBeforeNextSparkleCoin3 -= CupheadTime.Delta;
			for (int j = 0; j < this.sparkleAnimatorsCoin1.Length; j++)
			{
				if (this.timeBeforeNextSparkleCoin1 <= 0f)
				{
					if (this.sparkleAnimatorsCoin1[j].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleCoin1 = this.timeBetweenSparkle;
						this.sparkleAnimatorsCoin1[j].transform.position = new Vector3(this.sparkleTransformCoin1.position.x + UnityEngine.Random.Range(this.sparkleTransformCoin1.sizeDelta.x * -0.5f, this.sparkleTransformCoin1.sizeDelta.x * 0.5f), this.sparkleTransformCoin1.position.y + UnityEngine.Random.Range(this.sparkleTransformCoin1.sizeDelta.y * -0.5f, this.sparkleTransformCoin1.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsCoin1[j].SetTrigger(UnityEngine.Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
			for (int k = 0; k < this.sparkleAnimatorsCoin2.Length; k++)
			{
				if (this.timeBeforeNextSparkleCoin2 <= 0f)
				{
					if (this.sparkleAnimatorsCoin2[k].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleCoin2 = this.timeBetweenSparkle;
						this.sparkleAnimatorsCoin2[k].transform.position = new Vector3(this.sparkleTransformCoin2.position.x + UnityEngine.Random.Range(this.sparkleTransformCoin2.sizeDelta.x * -0.5f, this.sparkleTransformCoin2.sizeDelta.x * 0.5f), this.sparkleTransformCoin2.position.y + UnityEngine.Random.Range(this.sparkleTransformCoin2.sizeDelta.y * -0.5f, this.sparkleTransformCoin2.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsCoin2[k].SetTrigger(UnityEngine.Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
			for (int l = 0; l < this.sparkleAnimatorsCoin3.Length; l++)
			{
				if (this.timeBeforeNextSparkleCoin3 <= 0f)
				{
					if (this.sparkleAnimatorsCoin3[l].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleCoin3 = this.timeBetweenSparkle;
						this.sparkleAnimatorsCoin3[l].transform.position = new Vector3(this.sparkleTransformCoin3.position.x + UnityEngine.Random.Range(this.sparkleTransformCoin3.sizeDelta.x * -0.5f, this.sparkleTransformCoin3.sizeDelta.x * 0.5f), this.sparkleTransformCoin3.position.y + UnityEngine.Random.Range(this.sparkleTransformCoin3.sizeDelta.y * -0.5f, this.sparkleTransformCoin3.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsCoin3[l].SetTrigger(UnityEngine.Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
		}
		if (this.sparkling)
		{
			if (this.input.GetAnyButtonDown())
			{
				base.StartCoroutine(this.tweenOut_cr(1.5f));
				base.animator.SetTrigger("hide");
				this.sparkling = false;
			}
			this.timeBeforeNextSparkleContract -= CupheadTime.Delta;
			for (int m = 0; m < this.sparkleAnimatorsContract.Length; m++)
			{
				if (this.timeBeforeNextSparkleContract <= 0f)
				{
					if (this.sparkleAnimatorsContract[m].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleContract = this.timeBetweenSparkle;
						this.sparkleAnimatorsContract[m].transform.position = new Vector3(this.sparkleTransformContract.position.x + UnityEngine.Random.Range(this.sparkleTransformContract.sizeDelta.x * -0.5f, this.sparkleTransformContract.sizeDelta.x * 0.5f), this.sparkleTransformContract.position.y + UnityEngine.Random.Range(this.sparkleTransformContract.sizeDelta.y * -0.5f, this.sparkleTransformContract.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsContract[m].SetTrigger(UnityEngine.Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
		}
		if (this.dlcAvailableShowing && !this.dlcUI.visible)
		{
			base.StartCoroutine(this.tweenOut_cr(0.25f));
			this.dlcAvailableShowing = false;
		}
		if (this.ingredientShowing && this.input.GetAnyButtonDown())
		{
			base.StartCoroutine(this.tweenOut_cr(1.5f));
			base.animator.SetTrigger("hide_ingred");
			this.ingredientShowing = false;
		}
		if (this.djimmiShowing && this.input.GetAnyButtonDown())
		{
			base.animator.SetTrigger("hide_djimmi");
			this.djimmiShowing = false;
		}
	}

	// Token: 0x060039B7 RID: 14775 RVA: 0x0020CB49 File Offset: 0x0020AF49
	public void SparkleStart()
	{
		this.sparkling = true;
		base.StartCoroutine(this.showGlyphs_cr());
	}

	// Token: 0x060039B8 RID: 14776 RVA: 0x0020CB60 File Offset: 0x0020AF60
	protected IEnumerator showGlyphs_cr()
	{
		yield return new WaitForSeconds(0.5f);
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.glyphCanvasGroup.alpha = Mathf.Lerp(0f, 1f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.glyphCanvasGroup.alpha = 1f;
		while (!this.input.GetButtonDown(CupheadButton.Accept))
		{
			yield return null;
		}
		base.animator.SetTrigger("hide");
		yield return null;
		yield return base.animator.WaitForAnimationToEnd(this, "anim_map_ui_contract_end", 0, false, true);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060039B9 RID: 14777 RVA: 0x0020CB7C File Offset: 0x0020AF7C
	public void DebugShowContract(Levels level)
	{
		base.gameObject.SetActive(true);
		this.super1.SetActive(false);
		this.super2.SetActive(false);
		this.super3.SetActive(false);
		this.coin2.SetActive(false);
		this.coin3.SetActive(false);
		this.coinVariable.SetActive(false);
		this.coinVariableText.enabled = false;
		this.curseCharm.SetActive(false);
		this.airplaneIngred.SetActive(false);
		this.rumIngred.SetActive(false);
		this.oldManIngred.SetActive(false);
		this.snowCultIngred.SetActive(false);
		this.cowboyIngred.SetActive(false);
		InterruptingPrompt.SetCanInterrupt(true);
		this.tooltipEquipGlyph.SetActive(false);
		AudioManager.Play("world_map_soul_contract_open");
		AudioManager.PlayLoop("world_map_soul_contract_stamp_shimmer_loop");
		base.animator.SetTrigger("show");
		TranslationElement translationElement = Localization.Find(level.ToString());
		this.localizationHelper.ApplyTranslation(translationElement, null);
		this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
		string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
		this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockContract"), new LocalizationHelper.LocalizationSubtext[]
		{
			new LocalizationHelper.LocalizationSubtext("CONTRACT", translationElement.translation.text.Replace("\\n", newValue), false)
		});
		this.showing = true;
		this.canvasGroup.alpha = 1f;
	}

	// Token: 0x060039BA RID: 14778 RVA: 0x0020CD39 File Offset: 0x0020B139
	public void DebugShowEvent(Levels level)
	{
		this.DebugShowContract(level);
	}

	// Token: 0x060039BB RID: 14779 RVA: 0x0020CD44 File Offset: 0x0020B144
	public IEnumerator HideContract()
	{
		this.showing = true;
		base.animator.SetTrigger("hide");
		yield return null;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060039BC RID: 14780 RVA: 0x0020CD60 File Offset: 0x0020B160
	public void ShowEvent(MapEventNotification.Type eventType)
	{
		this.EventQueue.Enqueue(delegate
		{
			this.InternalShowEvent(eventType);
		});
	}

	// Token: 0x060039BD RID: 14781 RVA: 0x0020CD98 File Offset: 0x0020B198
	public void ShowVariableCoinEvent(int coinCount)
	{
		if (coinCount > 1)
		{
			this.coinVariableCount = coinCount;
			this.EventQueue.Enqueue(delegate
			{
				this.InternalShowEvent(MapEventNotification.Type.CoinVariable);
			});
		}
		else
		{
			this.EventQueue.Enqueue(delegate
			{
				this.InternalShowEvent(MapEventNotification.Type.Coin);
			});
		}
	}

	// Token: 0x060039BE RID: 14782 RVA: 0x0020CDE8 File Offset: 0x0020B1E8
	private void InternalShowEvent(MapEventNotification.Type eventType)
	{
		base.gameObject.SetActive(true);
		this.super1.SetActive(false);
		this.super2.SetActive(false);
		this.super3.SetActive(false);
		this.coin2.SetActive(false);
		this.coin3.SetActive(false);
		this.coinVariable.SetActive(false);
		this.coinVariableText.enabled = false;
		this.curseCharm.SetActive(false);
		this.airplaneIngred.SetActive(false);
		this.rumIngred.SetActive(false);
		this.oldManIngred.SetActive(false);
		this.snowCultIngred.SetActive(false);
		this.cowboyIngred.SetActive(false);
		InterruptingPrompt.SetCanInterrupt(true);
		switch (eventType)
		{
		case MapEventNotification.Type.SoulContract:
		{
			this.confirmGlyph.SetActive(true);
			this.tooltipEquipGlyph.SetActive(false);
			AudioManager.Play("world_map_soul_contract_open");
			AudioManager.PlayLoop("world_map_soul_contract_stamp_shimmer_loop");
			base.animator.SetTrigger("show");
			TranslationElement translationElement = Localization.Find(Level.PreviousLevel.ToString());
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockContract"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("CONTRACT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.Super:
		{
			this.confirmGlyph.SetActive(true);
			base.animator.SetTrigger("show_super");
			AudioManager.Stop("world_level_bridge_building_poof");
			AudioManager.Play("world_map_super_open");
			AudioManager.PlayLoop("world_map_super_loop");
			base.StartCoroutine(this.SuperInRoutine());
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockSuper"), null);
			Scenes currentMap = PlayerData.Data.CurrentMap;
			if (currentMap != Scenes.scene_map_world_1)
			{
				if (currentMap != Scenes.scene_map_world_2)
				{
					if (currentMap == Scenes.scene_map_world_3)
					{
						this.super3.SetActive(true);
					}
				}
				else
				{
					this.super2.SetActive(true);
				}
			}
			else
			{
				this.super1.SetActive(true);
			}
			break;
		}
		case MapEventNotification.Type.Coin:
			this.confirmGlyph.SetActive(true);
			AudioManager.Play("world_map_coin_open");
			base.StartCoroutine(this.CoinInRoutine());
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotACoin"), null);
			base.animator.SetTrigger("show_coin");
			break;
		case MapEventNotification.Type.ThreeCoins:
			this.confirmGlyph.SetActive(true);
			this.coin2.SetActive(true);
			this.coin3.SetActive(true);
			AudioManager.Play("world_map_coin_open");
			base.StartCoroutine(this.CoinInRoutine());
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotThreeCoins"), null);
			base.animator.SetTrigger("show_coin");
			break;
		case MapEventNotification.Type.Tooltip:
			this.confirmGlyph.SetActive(true);
			this.tooltipEquipGlyph.SetActive(false);
			base.StartCoroutine(this.TooltipInRoutine());
			base.animator.SetTrigger("show_tooltip");
			AudioManager.Play("menu_cardup");
			break;
		case MapEventNotification.Type.TooltipEquip:
			this.confirmGlyph.SetActive(false);
			this.tooltipEquipGlyph.SetActive(true);
			base.StartCoroutine(this.TooltipEquipInRoutine());
			base.animator.SetTrigger("show_tooltip");
			AudioManager.Play("menu_cardup");
			break;
		case MapEventNotification.Type.DLCAvailable:
			base.GetComponent<Animator>().enabled = false;
			base.transform.Find("Darker").gameObject.SetActive(false);
			base.transform.Find("Background").gameObject.SetActive(false);
			base.transform.Find("Text").gameObject.SetActive(false);
			base.transform.Find("LetterboxTop").gameObject.SetActive(false);
			base.transform.Find("LetterboxBottom").gameObject.SetActive(false);
			this.confirmGlyph.SetActive(true);
			this.notificationLocalizationHelper.textComponent.text = string.Empty;
			this.dlcUI.ShowMenu();
			base.StartCoroutine(this.DLCAvailableRoutine());
			break;
		case MapEventNotification.Type.AirplaneIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.airplaneIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_airplane");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			TranslationElement translationElement = Localization.Find("AirplaneIngredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.RumIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.rumIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_rum");
			TranslationElement translationElement = Localization.Find("RumIngredient");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.OldManIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.oldManIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_oldman");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			TranslationElement translationElement = Localization.Find("OldManIngredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.SnowIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.snowCultIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_snowcult");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			TranslationElement translationElement = Localization.Find("SnowCultIngredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.CowboyIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.cowboyIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_cowboy");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			TranslationElement translationElement = Localization.Find("CowboyIngredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.CoinVariable:
			this.confirmGlyph.SetActive(true);
			this.coinVariable.SetActive(true);
			this.coinVariableText.text = "x" + this.coinVariableCount.ToString();
			this.coinVariableText.enabled = true;
			AudioManager.Play("world_map_coin_open");
			base.StartCoroutine(this.CoinInRoutine());
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotACoin"), null);
			base.animator.SetTrigger("show_coinvariable");
			break;
		case MapEventNotification.Type.Djimmi:
		{
			AudioManager.Play("sfx_worldmap_djimmi_open");
			TranslationElement translationElement = Localization.Find("GameDjimmi_Tooltip_Wish" + (3 - PlayerData.Data.djimmiWishes).ToString());
			this.notificationLocalizationHelper.ApplyTranslation(translationElement, null);
			base.animator.SetTrigger("show_djimmi");
			base.StartCoroutine(this.DjimmiRoutine());
			break;
		}
		case MapEventNotification.Type.DjimmiFreed:
		{
			AudioManager.Play("sfx_worldmap_djimmi_open");
			TranslationElement translationElement = Localization.Find("GameDjimmi_Tooltip_Freed");
			this.notificationLocalizationHelper.ApplyTranslation(translationElement, null);
			base.animator.SetTrigger("show_djimmi");
			base.StartCoroutine(this.DjimmiRoutine());
			break;
		}
		}
		this.showing = true;
		base.StartCoroutine(this.tweenIn_cr());
	}

	// Token: 0x060039BF RID: 14783 RVA: 0x0020D8DC File Offset: 0x0020BCDC
	public void ShowTooltipEvent(TooltipEvent tooltipEvent)
	{
		InterruptingPrompt.SetCanInterrupt(true);
		switch (tooltipEvent)
		{
		case TooltipEvent.Turtle:
			this.tooltipPortrait.sprite = this.TurtleSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Pacifist_Tooltip_NewAudioVisMode"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.Canteen:
			this.tooltipPortrait.sprite = this.CanteenSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Canteen_Tooltip_ShmupWeapons"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.ShopKeep:
			this.tooltipPortrait.sprite = this.ShopkeepSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Shopkeeper_Tooltip_NewPurchase"), null);
			this.ShowEvent(MapEventNotification.Type.TooltipEquip);
			break;
		case TooltipEvent.Professional:
			this.tooltipPortrait.sprite = this.ForkSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Professional_Tooltip_SuperEquip"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.KingDice:
			this.tooltipPortrait.sprite = this.KingDiceSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("KingDice_Tooltip_RegularSoulContracts"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.Mausoleum:
			this.tooltipPortrait.sprite = this.MausoleumSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_NewSuperEquip"), null);
			this.ShowEvent(MapEventNotification.Type.TooltipEquip);
			break;
		case TooltipEvent.Boatman:
			this.tooltipPortrait.sprite = this.BoatmanSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Boatman_Tooltip_UpgradedSave"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.Chalice:
			this.tooltipPortrait.sprite = this.SaltbakerSpriteB;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_CharmEquip"), null);
			this.ShowEvent(MapEventNotification.Type.TooltipEquip);
			break;
		case TooltipEvent.ChaliceTutorialEquipCharm:
			this.tooltipPortrait.sprite = this.SaltbakerSpriteA;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find((!PlayerManager.Multiplayer) ? "Saltbaker_Tooltip_ChaliceTutorialSingle" : "Saltbaker_Tooltip_ChaliceTutorialMulti"), null);
			this.ShowEvent(MapEventNotification.Type.TooltipEquip);
			break;
		case TooltipEvent.SimpleIngredient:
			this.tooltipPortrait.sprite = this.SaltbakerSpriteA;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Saltbaker_Tooltip_SimpleIngredient"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.BackToKitchen:
			this.tooltipPortrait.sprite = this.ChaliceSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_GotAllIngredients"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.ChaliceFan:
			this.tooltipPortrait.sprite = this.ChaliceFanSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("ChaliceFan_Tooltip_NewFilter"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		default:
			this.tooltipPortrait.sprite = null;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Shopkeeper_Tooltip_NewPurchase"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		}
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x0020DBD0 File Offset: 0x0020BFD0
	protected IEnumerator CoinInRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.coinShowing = true;
		yield break;
	}

	// Token: 0x060039C1 RID: 14785 RVA: 0x0020DBEC File Offset: 0x0020BFEC
	protected IEnumerator TooltipInRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.tooltipShowing = true;
		yield break;
	}

	// Token: 0x060039C2 RID: 14786 RVA: 0x0020DC08 File Offset: 0x0020C008
	protected IEnumerator TooltipEquipInRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.tooltipEquipShowing = true;
		yield break;
	}

	// Token: 0x060039C3 RID: 14787 RVA: 0x0020DC24 File Offset: 0x0020C024
	protected IEnumerator SuperInRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.superShowing = true;
		yield break;
	}

	// Token: 0x060039C4 RID: 14788 RVA: 0x0020DC40 File Offset: 0x0020C040
	protected IEnumerator DLCAvailableRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.dlcAvailableShowing = true;
		yield break;
	}

	// Token: 0x060039C5 RID: 14789 RVA: 0x0020DC5C File Offset: 0x0020C05C
	protected IEnumerator IngredientRoutine()
	{
		this.ingredientStarburst.SetActive(true);
		yield return new WaitForSeconds(1f);
		this.ingredientShowing = true;
		yield break;
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x0020DC77 File Offset: 0x0020C077
	private void AniEvent_DjimmiAppear()
	{
		AudioManager.Play("sfx_worldmap_djimmi_entrance");
	}

	// Token: 0x060039C7 RID: 14791 RVA: 0x0020DC83 File Offset: 0x0020C083
	private void AniEvent_DjimmiLaugh()
	{
		AudioManager.Play("sfx_worldmap_djimmi_laugh");
	}

	// Token: 0x060039C8 RID: 14792 RVA: 0x0020DC8F File Offset: 0x0020C08F
	private void AniEvent_DjimmiMagicLoop()
	{
		AudioManager.PlayLoop("sfx_worldmap_djimmi_magic");
		AudioManager.FadeSFXVolumeLinear("sfx_worldmap_djimmi_magic", 0.5f, 0.5f);
	}

	// Token: 0x060039C9 RID: 14793 RVA: 0x0020DCB0 File Offset: 0x0020C0B0
	protected IEnumerator DjimmiRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.djimmiShowing = true;
		yield return base.animator.WaitForAnimationToStart(this, "anim_map_djimmi_out", false);
		AudioManager.Play("sfx_worldmap_djimmi_disappear");
		AudioManager.Stop("sfx_worldmap_djimmi_magic");
		base.StartCoroutine(this.tweenOut_cr(1.5f));
		yield break;
	}

	// Token: 0x060039CA RID: 14794 RVA: 0x0020DCCC File Offset: 0x0020C0CC
	protected IEnumerator tweenIn_cr()
	{
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(0f, 1f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 1f;
		yield break;
	}

	// Token: 0x060039CB RID: 14795 RVA: 0x0020DCE8 File Offset: 0x0020C0E8
	protected IEnumerator tweenOut_cr(float time = 1.5f)
	{
		AudioManager.FadeSFXVolume("world_map_soul_contract_stamp_shimmer_loop", 0f, 5f);
		AudioManager.FadeSFXVolume("world_map_super_loop", 0f, 5f);
		yield return new WaitForSeconds(time);
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(1f, 0f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 0f;
		while (InterruptingPrompt.IsInterrupting())
		{
			yield return null;
		}
		this.showing = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060039CC RID: 14796 RVA: 0x0020DD0C File Offset: 0x0020C10C
	private IEnumerator text_cr()
	{
		yield return base.StartCoroutine(this.textScale_cr(0.9f, 1.1f, 0.5f));
		yield return base.StartCoroutine(this.textScale_cr(1.1f, 0.9f, 0.5f));
		while (!this.input.GetButtonDown(CupheadButton.Accept))
		{
			yield return null;
		}
		this.showing = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060039CD RID: 14797 RVA: 0x0020DD28 File Offset: 0x0020C128
	protected IEnumerator textScale_cr(float start, float end, float time)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.text.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.text.transform.localScale = Vector3.one * end;
		yield return null;
		yield break;
	}

	// Token: 0x0400416F RID: 16751
	[SerializeField]
	private Image background;

	// Token: 0x04004170 RID: 16752
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04004171 RID: 16753
	[SerializeField]
	private LocalizationHelper localizationHelper;

	// Token: 0x04004172 RID: 16754
	[SerializeField]
	private LocalizationHelper notificationLocalizationHelper;

	// Token: 0x04004173 RID: 16755
	[SerializeField]
	private RectTransform sparkleTransformContract;

	// Token: 0x04004174 RID: 16756
	[SerializeField]
	private RectTransform sparkleTransformCoin1;

	// Token: 0x04004175 RID: 16757
	[SerializeField]
	private RectTransform sparkleTransformCoin2;

	// Token: 0x04004176 RID: 16758
	[SerializeField]
	private RectTransform sparkleTransformCoin3;

	// Token: 0x04004177 RID: 16759
	[SerializeField]
	private GameObject sparklePrefab;

	// Token: 0x04004178 RID: 16760
	[SerializeField]
	private CanvasGroup glyphCanvasGroup;

	// Token: 0x04004179 RID: 16761
	[SerializeField]
	private GameObject coin2;

	// Token: 0x0400417A RID: 16762
	[SerializeField]
	private GameObject coin3;

	// Token: 0x0400417B RID: 16763
	[SerializeField]
	private GameObject coinVariable;

	// Token: 0x0400417C RID: 16764
	[SerializeField]
	private Text coinVariableText;

	// Token: 0x0400417D RID: 16765
	[SerializeField]
	private GameObject super1;

	// Token: 0x0400417E RID: 16766
	[SerializeField]
	private GameObject super2;

	// Token: 0x0400417F RID: 16767
	[SerializeField]
	private GameObject super3;

	// Token: 0x04004180 RID: 16768
	[SerializeField]
	private GameObject curseCharm;

	// Token: 0x04004181 RID: 16769
	[SerializeField]
	private GameObject ingredientStarburst;

	// Token: 0x04004182 RID: 16770
	[SerializeField]
	private GameObject airplaneIngred;

	// Token: 0x04004183 RID: 16771
	[SerializeField]
	private GameObject rumIngred;

	// Token: 0x04004184 RID: 16772
	[SerializeField]
	private GameObject oldManIngred;

	// Token: 0x04004185 RID: 16773
	[SerializeField]
	private GameObject snowCultIngred;

	// Token: 0x04004186 RID: 16774
	[SerializeField]
	private GameObject cowboyIngred;

	// Token: 0x04004187 RID: 16775
	[SerializeField]
	private GameObject confirmGlyph;

	// Token: 0x04004188 RID: 16776
	[SerializeField]
	private GameObject dlcUIPrefab;

	// Token: 0x04004189 RID: 16777
	[SerializeField]
	private Transform dlcUIRoot;

	// Token: 0x0400418A RID: 16778
	[Header("Tooltips")]
	[SerializeField]
	private CanvasGroup tooltipCanvasGroup;

	// Token: 0x0400418B RID: 16779
	[SerializeField]
	private Image tooltipPortrait;

	// Token: 0x0400418C RID: 16780
	[SerializeField]
	private LocalizationHelper tooltipLocalizationHelper;

	// Token: 0x0400418D RID: 16781
	[SerializeField]
	private GameObject tooltipEquipGlyph;

	// Token: 0x0400418E RID: 16782
	[SerializeField]
	private Sprite TurtleSprite;

	// Token: 0x0400418F RID: 16783
	[SerializeField]
	private Sprite CanteenSprite;

	// Token: 0x04004190 RID: 16784
	[SerializeField]
	private Sprite ShopkeepSprite;

	// Token: 0x04004191 RID: 16785
	[SerializeField]
	private Sprite ForkSprite;

	// Token: 0x04004192 RID: 16786
	[SerializeField]
	private Sprite KingDiceSprite;

	// Token: 0x04004193 RID: 16787
	[SerializeField]
	private Sprite MausoleumSprite;

	// Token: 0x04004194 RID: 16788
	[SerializeField]
	private Sprite SaltbakerSpriteA;

	// Token: 0x04004195 RID: 16789
	[SerializeField]
	private Sprite SaltbakerSpriteB;

	// Token: 0x04004196 RID: 16790
	[SerializeField]
	private Sprite ChaliceSprite;

	// Token: 0x04004197 RID: 16791
	[SerializeField]
	private Sprite ChaliceFanSprite;

	// Token: 0x04004198 RID: 16792
	[SerializeField]
	private Sprite BoatmanSprite;

	// Token: 0x04004199 RID: 16793
	private CanvasGroup canvasGroup;

	// Token: 0x0400419B RID: 16795
	private bool sparkling;

	// Token: 0x0400419C RID: 16796
	private bool coinShowing;

	// Token: 0x0400419D RID: 16797
	private bool tooltipShowing;

	// Token: 0x0400419E RID: 16798
	private bool tooltipEquipShowing;

	// Token: 0x0400419F RID: 16799
	private bool superShowing;

	// Token: 0x040041A0 RID: 16800
	private bool dlcAvailableShowing;

	// Token: 0x040041A1 RID: 16801
	private bool ingredientShowing;

	// Token: 0x040041A2 RID: 16802
	private bool djimmiShowing;

	// Token: 0x040041A3 RID: 16803
	private int coinVariableCount;

	// Token: 0x040041A4 RID: 16804
	private Animator[] sparkleAnimatorsContract = new Animator[3];

	// Token: 0x040041A5 RID: 16805
	private Animator[] sparkleAnimatorsCoin1 = new Animator[3];

	// Token: 0x040041A6 RID: 16806
	private Animator[] sparkleAnimatorsCoin2 = new Animator[3];

	// Token: 0x040041A7 RID: 16807
	private Animator[] sparkleAnimatorsCoin3 = new Animator[3];

	// Token: 0x040041A8 RID: 16808
	private float timeBeforeNextSparkleContract = 0.2f;

	// Token: 0x040041A9 RID: 16809
	private float timeBeforeNextSparkleCoin1 = 0.2f;

	// Token: 0x040041AA RID: 16810
	private float timeBeforeNextSparkleCoin2 = 0.2f;

	// Token: 0x040041AB RID: 16811
	private float timeBeforeNextSparkleCoin3 = 0.2f;

	// Token: 0x040041AC RID: 16812
	[SerializeField]
	private float timeBetweenSparkle = 0.3f;

	// Token: 0x040041AD RID: 16813
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x040041AE RID: 16814
	public Queue<Action> EventQueue = new Queue<Action>();

	// Token: 0x040041AF RID: 16815
	private MapDLCUI dlcUI;

	// Token: 0x0200099F RID: 2463
	public enum Type
	{
		// Token: 0x040041B1 RID: 16817
		SoulContract,
		// Token: 0x040041B2 RID: 16818
		Super,
		// Token: 0x040041B3 RID: 16819
		Coin,
		// Token: 0x040041B4 RID: 16820
		ThreeCoins,
		// Token: 0x040041B5 RID: 16821
		Blueprint,
		// Token: 0x040041B6 RID: 16822
		Tooltip,
		// Token: 0x040041B7 RID: 16823
		TooltipEquip,
		// Token: 0x040041B8 RID: 16824
		DLCAvailable,
		// Token: 0x040041B9 RID: 16825
		AirplaneIngredient,
		// Token: 0x040041BA RID: 16826
		RumIngredient,
		// Token: 0x040041BB RID: 16827
		OldManIngredient,
		// Token: 0x040041BC RID: 16828
		SnowIngredient,
		// Token: 0x040041BD RID: 16829
		CowboyIngredient,
		// Token: 0x040041BE RID: 16830
		CoinVariable,
		// Token: 0x040041BF RID: 16831
		Djimmi,
		// Token: 0x040041C0 RID: 16832
		DjimmiFreed
	}
}
