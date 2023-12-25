using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200099A RID: 2458
public class MapConfirmStartUI : AbstractMapSceneStartUI
{
	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x0600397E RID: 14718 RVA: 0x0020A055 File Offset: 0x00208455
	// (set) Token: 0x0600397F RID: 14719 RVA: 0x0020A05C File Offset: 0x0020845C
	public static MapConfirmStartUI Current { get; protected set; }

	// Token: 0x06003980 RID: 14720 RVA: 0x0020A064 File Offset: 0x00208464
	protected override void Awake()
	{
		base.Awake();
		MapConfirmStartUI.Current = this;
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x0020A074 File Offset: 0x00208474
	private void UpdateCursor()
	{
		this.cursor.transform.position = this.enter.transform.position;
		this.cursor.sizeDelta = new Vector2(this.enter.sizeDelta.x + 30f, this.enter.sizeDelta.y + 20f);
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x0020A0E3 File Offset: 0x002084E3
	private void OnDestroy()
	{
		if (MapConfirmStartUI.Current == this)
		{
			MapConfirmStartUI.Current = null;
		}
	}

	// Token: 0x06003983 RID: 14723 RVA: 0x0020A0FB File Offset: 0x002084FB
	private void Update()
	{
		this.UpdateCursor();
		if (base.CurrentState == AbstractMapSceneStartUI.State.Active)
		{
			this.CheckInput();
		}
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x0020A115 File Offset: 0x00208515
	private void CheckInput()
	{
		if (!base.Able)
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.Cancel))
		{
			base.Out();
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			base.LoadLevel();
		}
	}

	// Token: 0x06003985 RID: 14725 RVA: 0x0020A14C File Offset: 0x0020854C
	public void InitUI(string level)
	{
		TranslationElement translationElement = Localization.Find(level);
		if (translationElement != null)
		{
			this.Title.ApplyTranslation(translationElement, null);
		}
		this.EmptyCoin2.enabled = true;
		this.Coin2.enabled = false;
		this.EmptyCoin3.enabled = true;
		this.Coin3.enabled = false;
		this.EmptyCoin4.enabled = true;
		this.Coin4.enabled = false;
		this.EmptyCoin5.enabled = true;
		this.Coin5.enabled = false;
		List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins = PlayerData.Data.coinManager.LevelsAndCoins;
		for (int i = 0; i < levelsAndCoins.Count; i++)
		{
			if (levelsAndCoins[i].level.ToString() == level)
			{
				if (levelsAndCoins[i].Coin1Collected)
				{
					this.EmptyCoin1.enabled = false;
					this.Coin1.enabled = true;
				}
				else
				{
					this.EmptyCoin1.enabled = true;
					this.Coin1.enabled = false;
				}
				if (levelsAndCoins[i].Coin2Collected)
				{
					this.EmptyCoin2.enabled = false;
					this.Coin2.enabled = true;
				}
				else
				{
					this.EmptyCoin2.enabled = true;
					this.Coin2.enabled = false;
				}
				if (levelsAndCoins[i].Coin3Collected)
				{
					this.EmptyCoin3.enabled = false;
					this.Coin3.enabled = true;
				}
				else
				{
					this.EmptyCoin3.enabled = true;
					this.Coin3.enabled = false;
				}
				if (levelsAndCoins[i].Coin4Collected)
				{
					this.EmptyCoin4.enabled = false;
					this.Coin4.enabled = true;
				}
				else
				{
					this.EmptyCoin4.enabled = true;
					this.Coin4.enabled = false;
				}
				if (levelsAndCoins[i].Coin5Collected)
				{
					this.EmptyCoin5.enabled = false;
					this.Coin5.enabled = true;
				}
				else
				{
					this.EmptyCoin5.enabled = true;
					this.Coin5.enabled = false;
				}
			}
		}
	}

	// Token: 0x06003986 RID: 14726 RVA: 0x0020A37A File Offset: 0x0020877A
	public new void In(MapPlayerController playerController)
	{
		base.In(playerController);
		if (this.Animator != null)
		{
			this.Animator.SetTrigger("ZoomIn");
			AudioManager.Play("world_map_level_menu_open");
		}
		this.InitUI(this.level);
	}

	// Token: 0x0400411F RID: 16671
	public Animator Animator;

	// Token: 0x04004120 RID: 16672
	public LocalizationHelper Title;

	// Token: 0x04004121 RID: 16673
	[Header("Coins")]
	public Image EmptyCoin1;

	// Token: 0x04004122 RID: 16674
	public Image Coin1;

	// Token: 0x04004123 RID: 16675
	public Image EmptyCoin2;

	// Token: 0x04004124 RID: 16676
	public Image Coin2;

	// Token: 0x04004125 RID: 16677
	public Image EmptyCoin3;

	// Token: 0x04004126 RID: 16678
	public Image Coin3;

	// Token: 0x04004127 RID: 16679
	public Image EmptyCoin4;

	// Token: 0x04004128 RID: 16680
	public Image Coin4;

	// Token: 0x04004129 RID: 16681
	public Image EmptyCoin5;

	// Token: 0x0400412A RID: 16682
	public Image Coin5;

	// Token: 0x0400412B RID: 16683
	[SerializeField]
	private RectTransform cursor;

	// Token: 0x0400412C RID: 16684
	[Header("Options")]
	[SerializeField]
	private RectTransform enter;
}
