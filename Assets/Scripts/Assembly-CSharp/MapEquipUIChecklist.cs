using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000995 RID: 2453
public class MapEquipUIChecklist : AbstractMapEquipUICardSide
{
	// Token: 0x0600395C RID: 14684 RVA: 0x00208E50 File Offset: 0x00207250
	public override void Init(PlayerId playerID)
	{
		base.Init(playerID);
		this.darkText = new Color(0.2f, 0.188f, 0.188f);
		this.lightText = new Color(0.827f, 0.765f, 0.702f);
		this.disabledText = new Color(0.537f, 0.498f, 0.463f);
		this.selectableLength = this.worldSelectionIcons.Length;
		for (int i = 0; i < this.worldSelectionIcons.Length; i++)
		{
			this.worldSelectionIcons[i].SetIcons("Icons/" + this.worldPaths[i] + "_dark");
			this.worldSelectionIcons[i].SetTextColor(this.darkText);
		}
		this.worldSelectionIcons[this.index].SetIcons("Icons/" + this.worldPaths[this.index] + "_light");
		this.worldSelectionIcons[this.index].SetTextColor(this.lightText);
		if (!PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels))
		{
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 1].SetTextColor(this.disabledText);
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 2].SetTextColor(this.disabledText);
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 3].SetTextColor(this.disabledText);
			this.selectableLength -= 3;
		}
		else if (!PlayerData.Data.CheckLevelsCompleted(Level.world2BossLevels))
		{
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 1].SetTextColor(this.disabledText);
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 2].SetTextColor(this.disabledText);
			this.selectableLength -= 2;
		}
		else if (!PlayerData.Data.CheckLevelsCompleted(Level.world3BossLevels))
		{
			this.worldSelectionIcons[this.worldSelectionIcons.Length - 1].SetTextColor(this.disabledText);
			this.selectableLength--;
		}
		this.UpdateList();
	}

	// Token: 0x0600395D RID: 14685 RVA: 0x00209074 File Offset: 0x00207474
	private void SetArrow(bool showRight)
	{
		this.rightArrow.SetActive(showRight);
		this.leftArrow.SetActive(!showRight);
	}

	// Token: 0x0600395E RID: 14686 RVA: 0x00209094 File Offset: 0x00207494
	public void ChangeSelection(int direction)
	{
		this.index = Mathf.Clamp(this.index + direction, 0, this.selectableLength - 1);
		bool flag = false;
		this.skippedOver = false;
		if (this.showDLCMenu)
		{
			if (this.selectableLength < this.worldSelectionIcons.Length)
			{
				flag = true;
				if (this.DLCIndex == this.worldNames.Length - 1 && direction < 0)
				{
					this.DLCIndex = this.selectableLength - 1;
					this.skippedOver = true;
					this.SetArrow(true);
				}
				else if (this.DLCIndex + direction > this.selectableLength - 1)
				{
					this.DLCIndex = this.worldNames.Length - 1;
					this.skippedOver = true;
					this.SetArrow(false);
				}
				else if (this.DLCIndex + direction < 0)
				{
					this.DLCIndex = 0;
					this.SetArrow(true);
				}
				else
				{
					this.DLCIndex += direction;
				}
			}
			else if (this.DLCIndex + direction < 0)
			{
				this.DLCIndex = 0;
				this.SetArrow(true);
			}
			else if (this.DLCIndex + direction > this.worldNames.Length - 1)
			{
				this.DLCIndex = this.worldNames.Length - 1;
				this.SetArrow(false);
			}
			else
			{
				this.DLCIndex += direction;
			}
			this.ChangeDLCMenu(this.index, this.lastIndex);
		}
		if (flag)
		{
			int num = (this.DLCIndex != this.worldNames.Length - 1) ? this.index : (this.worldSelectionIcons.Length - 1);
			this.SetCursorPosition(num, false);
		}
		else
		{
			this.SetCursorPosition(this.index, false);
		}
	}

	// Token: 0x0600395F RID: 14687 RVA: 0x00209250 File Offset: 0x00207650
	public void SetCursorPosition(int index, bool openingChecklist)
	{
		if (openingChecklist)
		{
			this.showDLCMenu = ((DLCManager.DLCEnabled() && PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted) || this.editorShowDLCChecklist);
			if (index >= this.worldNames.Length - 1)
			{
				this.DLCIndex = index;
				index = this.worldSelectionIcons.Length - 1;
			}
			else
			{
				this.DLCIndex = index;
			}
		}
		this.index = index;
		if (this.lastIndex != index)
		{
			this.worldSelectionIcons[index].SetIcons("Icons/" + this.worldPaths[index] + "_light");
			this.worldSelectionIcons[index].SetTextColor(new Color(0.827f, 0.765f, 0.702f));
			if (!this.skippedOver)
			{
				this.worldSelectionIcons[this.lastIndex].SetIcons("Icons/" + this.worldPaths[this.lastIndex] + "_dark");
				this.worldSelectionIcons[this.lastIndex].SetTextColor(new Color(0.2f, 0.188f, 0.188f));
			}
			AudioManager.Play("menu_equipment_move");
			this.lastIndex = index;
		}
		if (this.showDLCMenu)
		{
			if (openingChecklist)
			{
				this.skippedOver = true;
				if (this.DLCIndex < this.worldNames.Length - 1)
				{
					this.SetArrow(true);
				}
				this.ChangeDLCMenu(index, this.lastIndex);
			}
			if (this.DLCIndex == 0)
			{
				this.SetArrow(true);
			}
			else if (this.DLCIndex == this.worldNames.Length - 1)
			{
				this.SetArrow(false);
			}
		}
		this.cursor.SetPosition(this.worldSelectionIcons[index].transform.position);
		this.UpdateList();
	}

	// Token: 0x06003960 RID: 14688 RVA: 0x00209420 File Offset: 0x00207820
	private void ChangeDLCMenu(int index, int lastIndex)
	{
		if ((index == lastIndex && (index <= 0 || index >= this.selectableLength - 1)) || this.skippedOver)
		{
			int num = (this.DLCIndex != this.worldNames.Length - 1) ? 0 : 1;
			int num2 = 0;
			bool flag = index >= this.worldSelectionIcons.Length - 1;
			for (int i = 0; i < this.worldSelectionIcons.Length; i++)
			{
				TranslationElement translationElement = Localization.Find(this.worldNames[num].ToString());
				this.worldSelectionIcons[num2].iconText.text = translationElement.translation.text;
				int num3 = (this.DLCIndex != this.worldNames.Length - 1) ? 0 : 1;
				if (i > this.selectableLength - 1 - num3 && (this.DLCIndex != this.worldNames.Length - 1 || i != this.worldSelectionIcons.Length - 1))
				{
					this.worldSelectionIcons[i].SetTextColor(this.disabledText);
				}
				num = (num + 1) % this.worldNames.Length;
				num2 = (num2 + 1) % this.worldSelectionIcons.Length;
			}
		}
	}

	// Token: 0x06003961 RID: 14689 RVA: 0x0020955C File Offset: 0x0020795C
	private void UpdateList()
	{
		List<Levels> list = new List<Levels>();
		List<string> list2 = new List<string>();
		list.Clear();
		list2.Clear();
		for (int i = 0; i < this.checklistItems.Count; i++)
		{
			this.checklistItems[i].gameObject.SetActive(false);
			this.checklistItems[i].ClearDescription(this.selectedFinale);
		}
		for (int j = 0; j < this.finaleItems.Count; j++)
		{
			this.finaleItems[j].gameObject.SetActive(false);
			if (this.finaleItems[j].checkMark != null)
			{
				this.finaleItems[j].checkMark.enabled = false;
				this.finaleItems[j].ClearDescription(this.selectedFinale);
			}
		}
		bool flag = false;
		switch ((!this.showDLCMenu) ? this.index : this.DLCIndex)
		{
		case 0:
			list.AddRange(this.world1Levels);
			this.selectedFinale = false;
			break;
		case 1:
			list.AddRange(this.world2Levels);
			this.selectedFinale = false;
			break;
		case 2:
			list.AddRange(this.world3Levels);
			this.selectedFinale = false;
			break;
		case 3:
			list.AddRange(this.finaleLevels);
			this.selectedFinale = true;
			break;
		case 4:
			list.AddRange(this.DLClevels);
			this.selectedFinale = false;
			flag = true;
			break;
		}
		foreach (Levels level in list)
		{
			list2.Add(Level.GetLevelName(level).Replace("\\n", " "));
		}
		this.worldTop.SetActive(false);
		this.finaleTop.SetActive(false);
		this.localizedTop.SetActive(true);
		this.worldTopLocalized.SetActive(!this.selectedFinale && !flag);
		this.finaleTopLocalized.SetActive(this.selectedFinale);
		this.worldTopDLCLocalized.SetActive(flag);
		this.finaleGrid.SetActive(this.selectedFinale);
		bool played = PlayerData.Data.GetLevelData(Levels.Saltbaker).played;
		for (int k = 0; k < list2.Count; k++)
		{
			if (flag)
			{
				if (k != list2.Count - 1 || (k == list2.Count - 1 && played))
				{
					this.checklistItems[k].gameObject.SetActive(true);
					this.checklistItems[k].EnableCheckbox(k < list2.Count - 1);
					this.checklistItems[k].SetDescription(list[k], list2[k], this.selectedFinale);
				}
			}
			else if (!this.selectedFinale)
			{
				this.checklistItems[k].gameObject.SetActive(true);
				this.checklistItems[k].EnableCheckbox(k < list2.Count - 2);
				this.checklistItems[k].SetDescription(list[k], list2[k], this.selectedFinale);
			}
			else
			{
				this.finaleItems[k].gameObject.SetActive(true);
				this.finaleItems[k].SetDescription(list[k], list2[k], this.selectedFinale);
			}
		}
	}

	// Token: 0x040040EA RID: 16618
	private readonly string[] worldPaths = new string[]
	{
		"equip_checklist_world_1",
		"equip_checklist_world_2",
		"equip_checklist_world_3",
		"equip_checklist_finale"
	};

	// Token: 0x040040EB RID: 16619
	private readonly Levels[] world1Levels = new Levels[]
	{
		Levels.Veggies,
		Levels.Slime,
		Levels.FlyingBlimp,
		Levels.Flower,
		Levels.Frogs,
		Levels.Platforming_Level_1_1,
		Levels.Platforming_Level_1_2
	};

	// Token: 0x040040EC RID: 16620
	private readonly Levels[] world2Levels = new Levels[]
	{
		Levels.Baroness,
		Levels.Clown,
		Levels.FlyingGenie,
		Levels.Dragon,
		Levels.FlyingBird,
		Levels.Platforming_Level_2_1,
		Levels.Platforming_Level_2_2
	};

	// Token: 0x040040ED RID: 16621
	private readonly Levels[] world3Levels = new Levels[]
	{
		Levels.Bee,
		Levels.Pirate,
		Levels.SallyStagePlay,
		Levels.Mouse,
		Levels.Robot,
		Levels.FlyingMermaid,
		Levels.Train,
		Levels.Platforming_Level_3_1,
		Levels.Platforming_Level_3_2
	};

	// Token: 0x040040EE RID: 16622
	private readonly Levels[] finaleLevels = new Levels[]
	{
		Levels.DicePalaceBooze,
		Levels.DicePalaceChips,
		Levels.DicePalaceCigar,
		Levels.DicePalaceDomino,
		Levels.DicePalaceEightBall,
		Levels.DicePalaceFlyingHorse,
		Levels.DicePalaceFlyingMemory,
		Levels.DicePalaceRabbit,
		Levels.DicePalaceRoulette,
		Levels.DicePalaceMain,
		Levels.Devil
	};

	// Token: 0x040040EF RID: 16623
	private readonly Levels[] DLClevels = new Levels[]
	{
		Levels.OldMan,
		Levels.RumRunners,
		Levels.Airplane,
		Levels.SnowCult,
		Levels.FlyingCowboy,
		Levels.Saltbaker
	};

	// Token: 0x040040F0 RID: 16624
	private readonly string[] worldNames = new string[]
	{
		"CheckListWorld1",
		"CheckListWorld2",
		"CheckListWorld3",
		"CheckListFinale",
		"ChecklistDLC"
	};

	// Token: 0x040040F1 RID: 16625
	[Header("Headers")]
	[SerializeField]
	private GameObject worldTop;

	// Token: 0x040040F2 RID: 16626
	[SerializeField]
	private GameObject finaleTop;

	// Token: 0x040040F3 RID: 16627
	[SerializeField]
	private GameObject localizedTop;

	// Token: 0x040040F4 RID: 16628
	[SerializeField]
	private GameObject worldTopLocalized;

	// Token: 0x040040F5 RID: 16629
	[SerializeField]
	private GameObject worldTopDLCLocalized;

	// Token: 0x040040F6 RID: 16630
	[SerializeField]
	private GameObject finaleTopLocalized;

	// Token: 0x040040F7 RID: 16631
	[Header("Cursors")]
	[SerializeField]
	private MapEquipUICursor cursor;

	// Token: 0x040040F8 RID: 16632
	[Header("Icons")]
	[SerializeField]
	private MapEquipUICardChecklistIcon[] worldSelectionIcons;

	// Token: 0x040040F9 RID: 16633
	[Header("Bosses + Platforming items")]
	[SerializeField]
	private List<MapEquipUIChecklistItem> checklistItems;

	// Token: 0x040040FA RID: 16634
	[SerializeField]
	private List<MapEquipUIChecklistItem> finaleItems;

	// Token: 0x040040FB RID: 16635
	[SerializeField]
	private GameObject finaleGrid;

	// Token: 0x040040FC RID: 16636
	[SerializeField]
	private GameObject rightArrow;

	// Token: 0x040040FD RID: 16637
	[SerializeField]
	private GameObject leftArrow;

	// Token: 0x040040FE RID: 16638
	private int index;

	// Token: 0x040040FF RID: 16639
	private int lastIndex;

	// Token: 0x04004100 RID: 16640
	private int DLCIndex;

	// Token: 0x04004101 RID: 16641
	private bool selectedFinale;

	// Token: 0x04004102 RID: 16642
	private bool showDLCMenu;

	// Token: 0x04004103 RID: 16643
	private bool skippedOver;

	// Token: 0x04004104 RID: 16644
	private bool editorShowDLCChecklist;

	// Token: 0x04004105 RID: 16645
	private Color darkText;

	// Token: 0x04004106 RID: 16646
	private Color lightText;

	// Token: 0x04004107 RID: 16647
	private Color disabledText;

	// Token: 0x04004108 RID: 16648
	private int selectableLength;
}
