using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200098F RID: 2447
public class MapEquipUICardBackSelect : AbstractMapEquipUICardSide
{
	// Token: 0x06003937 RID: 14647 RVA: 0x00206DB6 File Offset: 0x002051B6
	public void ChangeSelection(Trilean2 direction)
	{
		this.index = this.selectedIcons[this.index].GetIndexOfNeighbor(direction);
		this.SetCursorPosition(this.index);
		this.UpdateText();
	}

	// Token: 0x06003938 RID: 14648 RVA: 0x00206DE4 File Offset: 0x002051E4
	public void ChangeSlot(int direction)
	{
		this.cursor.Show();
		int num = (int)this.slot;
		this.slot = (MapEquipUICard.Slot)Mathf.Repeat((float)(num + direction), (float)EnumUtils.GetCount<MapEquipUICard.Slot>());
		this.Setup(this.slot);
	}

	// Token: 0x06003939 RID: 14649 RVA: 0x00206E28 File Offset: 0x00205228
	private void UpdateText()
	{
		bool flag = false;
		switch (this.slot)
		{
		case MapEquipUICard.Slot.SHOT_A:
		case MapEquipUICard.Slot.SHOT_B:
			flag = PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]);
			this.titleText.text = ((!flag) ? Localization.Translate("EquipItemLocked").text : WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.WEAPONS[this.index]).ToUpper());
			this.exText.text = ((!flag) ? "? ? ? ? ? ? ? ? ?" : WeaponProperties.GetSubtext(MapEquipUICardBackSelect.WEAPONS[this.index]));
			this.descriptionText.text = ((!flag) ? "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?" : WeaponProperties.GetDescription(MapEquipUICardBackSelect.WEAPONS[this.index]));
			break;
		case MapEquipUICard.Slot.SUPER:
		{
			flag = PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[this.index]);
			PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID);
			Super super = (playerLoadout.charm != Charm.charm_chalice) ? MapEquipUICardBackSelect.SUPERS[this.index] : MapEquipUICardBackSelect.CHALICESUPERS[this.index];
			this.titleText.text = ((!flag) ? Localization.Translate("EquipItemLocked").text : WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.SUPERS[this.index]).ToUpper());
			this.exText.text = ((!flag) ? "? ? ? ? ? ? ? ? ?" : WeaponProperties.GetSubtext(super));
			this.descriptionText.text = ((!flag) ? "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?" : WeaponProperties.GetDescription(super));
			break;
		}
		case MapEquipUICard.Slot.CHARM:
			flag = PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[this.index]);
			if (MapEquipUICardBackSelect.CHARMS[this.index] == Charm.charm_curse && CharmCurse.IsMaxLevel(base.playerID))
			{
				this.titleText.text = Localization.Translate("charm_paladin_name").text;
				this.exText.text = Localization.Translate("charm_paladin_subtext").text;
				this.descriptionText.text = Localization.Translate("charm_paladin_description").text;
			}
			else if (flag && MapEquipUICardBackSelect.CHARMS[this.index] == Charm.charm_curse && (CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1 || CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1))
			{
				this.titleText.text = Localization.Translate("charm_curse_name").text;
				this.exText.text = Localization.Translate("charm_curse_subtext").text;
				this.descriptionText.text = Localization.Translate("charm_curse_description").text;
			}
			else if (flag && MapEquipUICardBackSelect.CHARMS[this.index] == Charm.charm_curse)
			{
				this.titleText.text = Localization.Translate("charm_broken_name").text;
				this.exText.text = Localization.Translate("charm_broken_subtext").text;
				this.descriptionText.text = Localization.Translate("charm_broken_description").text;
			}
			else
			{
				this.titleText.text = ((!flag) ? Localization.Translate("EquipItemLocked").text : WeaponProperties.GetDisplayName(MapEquipUICardBackSelect.CHARMS[this.index]).ToUpper());
				this.exText.text = ((!flag) ? "? ? ? ? ? ? ? ? ?" : WeaponProperties.GetSubtext(MapEquipUICardBackSelect.CHARMS[this.index]));
				this.descriptionText.text = ((!flag) ? "? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ? ?" : WeaponProperties.GetDescription(MapEquipUICardBackSelect.CHARMS[this.index]));
			}
			break;
		}
		this.titleText.font = Localization.Instance.fonts[(int)Localization.language][9].font;
		if (flag)
		{
			this.exText.font = Localization.Instance.fonts[(int)Localization.language][10].font;
			this.descriptionText.font = Localization.Instance.fonts[(int)Localization.language][11].fontAsset;
		}
		else
		{
			this.exText.font = Localization.Instance.fonts[(int)Localization.language1][10].font;
			this.descriptionText.font = Localization.Instance.fonts[(int)Localization.language1][11].fontAsset;
		}
	}

	// Token: 0x0600393A RID: 14650 RVA: 0x00207330 File Offset: 0x00205730
	private void SetCursorPosition(int index)
	{
		if (this.lastIndex != index)
		{
			AudioManager.Play("menu_equipment_move");
			this.lastIndex = index;
		}
		this.cursor.SetPosition(this.selectedIcons[index].transform.position);
		if (!this.noneUnlocked && this.itemSelected)
		{
			this.selectionCursor.Show();
		}
		else
		{
			this.selectionCursor.Hide();
		}
	}

	// Token: 0x0600393B RID: 14651 RVA: 0x002073A8 File Offset: 0x002057A8
	public void Setup(MapEquipUICard.Slot slot)
	{
		this.slot = slot;
		this.headerText.ApplyTranslation(Localization.Find(MapEquipUICardBackSelect.slotLocalesKey[(int)slot]), null);
		bool flag = slot == MapEquipUICard.Slot.SUPER;
		bool flag2 = DLCManager.DLCEnabled();
		this.selectedIcons = ((!flag) ? ((!flag2) ? this.normalIcons : this.DLCIcons) : this.superIcons);
		foreach (MapEquipUICardBackSelectIcon mapEquipUICardBackSelectIcon in this.superIcons)
		{
			mapEquipUICardBackSelectIcon.gameObject.SetActive(flag);
		}
		foreach (MapEquipUICardBackSelectIcon mapEquipUICardBackSelectIcon2 in this.normalIcons)
		{
			mapEquipUICardBackSelectIcon2.gameObject.SetActive(!flag && !flag2);
		}
		foreach (MapEquipUICardBackSelectIcon mapEquipUICardBackSelectIcon3 in this.DLCIcons)
		{
			mapEquipUICardBackSelectIcon3.gameObject.SetActive(!flag && flag2);
		}
		this.superIconsBack.enabled = flag;
		this.iconsBack.enabled = (!flag && !flag2);
		this.DLCIconsBack.enabled = (!flag && flag2);
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID);
		this.selectionCursor.Hide();
		this.noneUnlocked = true;
		this.index = -1;
		bool isGrey = false;
		this.itemSelected = false;
		for (int l = 0; l < this.selectedIcons.Length; l++)
		{
			this.selectedIcons[l].Index = l;
			string str = "_000" + (l % 8 + 1).ToStringInvariant();
			string iconPath = Weapon.None.ToString();
			switch (slot)
			{
			case MapEquipUICard.Slot.SHOT_A:
				if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[l]))
				{
					isGrey = (MapEquipUICardBackSelect.WEAPONS[l] == playerLoadout.secondaryWeapon);
					this.noneUnlocked = false;
					this.selectedIcons[l].SetIcons(MapEquipUICardBackSelect.WEAPONS[l], isGrey);
				}
				else
				{
					iconPath = WeaponProperties.GetIconPath(Weapon.None) + str;
					this.selectedIcons[l].SetIconsManual(iconPath, isGrey, false);
				}
				if (MapEquipUICardBackSelect.WEAPONS[l] == playerLoadout.primaryWeapon && playerLoadout.primaryWeapon != Weapon.None)
				{
					this.index = l;
					this.itemSelected = true;
				}
				break;
			case MapEquipUICard.Slot.SHOT_B:
				if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[l]))
				{
					isGrey = (MapEquipUICardBackSelect.WEAPONS[l] == playerLoadout.primaryWeapon);
					this.noneUnlocked = false;
					this.selectedIcons[l].SetIcons(MapEquipUICardBackSelect.WEAPONS[l], isGrey);
				}
				else
				{
					iconPath = WeaponProperties.GetIconPath(Weapon.None) + str;
					this.selectedIcons[l].SetIconsManual(iconPath, isGrey, false);
				}
				if (MapEquipUICardBackSelect.WEAPONS[l] == playerLoadout.secondaryWeapon && playerLoadout.secondaryWeapon != Weapon.None)
				{
					this.index = l;
					this.itemSelected = true;
				}
				break;
			case MapEquipUICard.Slot.SUPER:
				if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[l]))
				{
					this.noneUnlocked = false;
					this.selectedIcons[l].SetIcons(MapEquipUICardBackSelect.SUPERS[l], isGrey);
				}
				else
				{
					iconPath = WeaponProperties.GetIconPath(Super.None) + str;
					this.selectedIcons[l].SetIconsManual(iconPath, isGrey, false);
				}
				if (playerLoadout.super != Super.None && (MapEquipUICardBackSelect.SUPERS[l] == playerLoadout.super || (playerLoadout.charm == Charm.charm_chalice && MapEquipUICardBackSelect.CHALICESUPERS[l] == playerLoadout.super)))
				{
					this.index = l;
					this.itemSelected = true;
				}
				break;
			case MapEquipUICard.Slot.CHARM:
				if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[l]))
				{
					this.noneUnlocked = false;
					if (MapEquipUICardBackSelect.CHARMS[l] == Charm.charm_curse)
					{
						this.selectedIcons[l].SetIconsManual("Icons/equip_icon_charm_curse_" + (CharmCurse.CalculateLevel(base.playerID) + 1).ToString(), false, true);
					}
					else
					{
						this.selectedIcons[l].SetIcons(MapEquipUICardBackSelect.CHARMS[l], isGrey);
					}
				}
				else
				{
					iconPath = WeaponProperties.GetIconPath(Charm.None) + str;
					this.selectedIcons[l].SetIconsManual(iconPath, isGrey, false);
				}
				if (MapEquipUICardBackSelect.CHARMS[l] == playerLoadout.charm && playerLoadout.charm != Charm.None)
				{
					this.index = l;
					this.itemSelected = true;
				}
				break;
			}
			if (this.index == -1)
			{
				this.index = 0;
			}
			this.cursor.SetPosition(this.selectedIcons[this.index].transform.position);
			this.UpdateText();
		}
		this.selectionCursor.selectedIndex = -1;
		if (!this.noneUnlocked && this.itemSelected)
		{
			if (slot != this.lastSlot)
			{
				this.selectionCursor.Show();
				this.selectionCursor.selectedIndex = this.index;
				this.selectionCursor.SetPosition(this.selectedIcons[this.index].transform.position);
			}
			else
			{
				base.StartCoroutine(this.set_selection_cursor());
			}
			this.cursor.SelectIcon(true);
		}
		this.lastSlot = slot;
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x002079AC File Offset: 0x00205DAC
	private IEnumerator set_selection_cursor()
	{
		base.StartCoroutine(this.lock_input_cr());
		while (!this.selectionCursor.animator.GetCurrentAnimatorStateInfo(0).IsName("Off") && this.lockInput)
		{
			yield return null;
		}
		this.selectionCursor.Show();
		this.selectionCursor.selectedIndex = this.index;
		this.selectionCursor.SetPosition(this.selectedIcons[this.index].transform.position);
		yield return null;
		yield break;
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x002079C8 File Offset: 0x00205DC8
	private IEnumerator lock_input_cr()
	{
		this.lockInput = true;
		yield return new WaitForSeconds(0.2f);
		this.lockInput = false;
		yield break;
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x002079E4 File Offset: 0x00205DE4
	public void Accept()
	{
		AudioManager.Play("menu_equipment_equip");
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID);
		switch (this.slot)
		{
		case MapEquipUICard.Slot.SHOT_A:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]))
			{
				this.Selection();
			}
			break;
		case MapEquipUICard.Slot.SHOT_B:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]) && (playerLoadout.primaryWeapon != MapEquipUICardBackSelect.WEAPONS[this.index] || playerLoadout.secondaryWeapon != Weapon.None))
			{
				this.Selection();
			}
			break;
		case MapEquipUICard.Slot.SUPER:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[this.index]))
			{
				this.Selection();
			}
			break;
		case MapEquipUICard.Slot.CHARM:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[this.index]))
			{
				this.Selection();
			}
			break;
		}
		switch (this.slot)
		{
		case MapEquipUICard.Slot.SHOT_A:
			if (MapEquipUICardBackSelect.WEAPONS[this.index] == Weapon.None || !PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]))
			{
				this.OnLocked();
				return;
			}
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon == MapEquipUICardBackSelect.WEAPONS[this.index])
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).primaryWeapon;
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).primaryWeapon = MapEquipUICardBackSelect.WEAPONS[this.index];
			break;
		case MapEquipUICard.Slot.SHOT_B:
			if (MapEquipUICardBackSelect.WEAPONS[this.index] == Weapon.None || !PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]) || (playerLoadout.primaryWeapon == MapEquipUICardBackSelect.WEAPONS[this.index] && playerLoadout.secondaryWeapon == Weapon.None))
			{
				this.OnLocked();
				return;
			}
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).primaryWeapon == MapEquipUICardBackSelect.WEAPONS[this.index])
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).primaryWeapon = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon;
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon = MapEquipUICardBackSelect.WEAPONS[this.index];
			if (!PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).HasEquippedSecondaryRegularWeapon)
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon = true;
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).HasEquippedSecondaryRegularWeapon = true;
			break;
		case MapEquipUICard.Slot.SUPER:
			if (MapEquipUICardBackSelect.SUPERS[this.index] == Super.None || !PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[this.index]))
			{
				this.OnLocked();
				return;
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).super = MapEquipUICardBackSelect.SUPERS[this.index];
			break;
		case MapEquipUICard.Slot.CHARM:
			if (MapEquipUICardBackSelect.CHARMS[this.index] == Charm.None || !PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[this.index]))
			{
				this.OnLocked();
				return;
			}
			if (MapEquipUICardBackSelect.CHARMS[this.index] != Charm.charm_chalice && PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm == Charm.charm_chalice)
			{
				PlayerManager.OnChaliceCharmUnequipped(base.playerID);
			}
			PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm = MapEquipUICardBackSelect.CHARMS[this.index];
			break;
		}
		this.Setup(this.slot);
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x00207E60 File Offset: 0x00206260
	public void Unequip()
	{
		AudioManager.Play("menu_equipment_equip");
		switch (this.slot)
		{
		case MapEquipUICard.Slot.SHOT_A:
			this.OnLocked();
			break;
		case MapEquipUICard.Slot.SHOT_B:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.WEAPONS[this.index]) && PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon != Weapon.None)
			{
				this.Deselect();
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon = Weapon.None;
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon)
				{
					PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).HasEquippedSecondaryRegularWeapon = false;
					PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon = false;
				}
			}
			else
			{
				this.OnLocked();
			}
			break;
		case MapEquipUICard.Slot.SUPER:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.SUPERS[this.index]) && PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).super != Super.None)
			{
				this.Deselect();
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).super = Super.None;
			}
			else
			{
				this.OnLocked();
			}
			break;
		case MapEquipUICard.Slot.CHARM:
			if (PlayerData.Data.IsUnlocked(base.playerID, MapEquipUICardBackSelect.CHARMS[this.index]) && PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm != Charm.None)
			{
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm == Charm.charm_chalice)
				{
					PlayerManager.OnChaliceCharmUnequipped(base.playerID);
				}
				this.Deselect();
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm = Charm.None;
			}
			else
			{
				this.OnLocked();
			}
			break;
		}
	}

	// Token: 0x06003940 RID: 14656 RVA: 0x00208098 File Offset: 0x00206498
	private void Selection()
	{
		this.selectedIcons[this.index].SelectIcon();
		if (this.selectionCursor.selectedIndex >= 0)
		{
			this.selectedIcons[this.selectionCursor.selectedIndex].UnselectIcon();
		}
		if (this.cursor.transform.position != this.selectionCursor.transform.position)
		{
			this.selectionCursor.selectedIndex = this.index;
			this.selectionCursor.Select();
			this.cursor.SelectIcon(false);
		}
		else
		{
			this.cursor.SelectIcon(true);
		}
	}

	// Token: 0x06003941 RID: 14657 RVA: 0x00208142 File Offset: 0x00206542
	private void Deselect()
	{
		base.StartCoroutine(this.remove_selection_cr());
		this.itemSelected = false;
	}

	// Token: 0x06003942 RID: 14658 RVA: 0x00208158 File Offset: 0x00206558
	private IEnumerator remove_selection_cr()
	{
		this.selectionCursor.Select();
		yield return this.selectionCursor.animator.WaitForAnimationToEnd(this, "Select", false, true);
		this.selectionCursor.Hide();
		yield return null;
		yield break;
	}

	// Token: 0x06003943 RID: 14659 RVA: 0x00208173 File Offset: 0x00206573
	private void OnLocked()
	{
		AudioManager.Play("menu_locked");
		this.selectedIcons[this.index].OnLocked();
		this.cursor.OnLocked();
	}

	// Token: 0x040040BE RID: 16574
	private static readonly Weapon[] WEAPONS = new Weapon[]
	{
		Weapon.level_weapon_peashot,
		Weapon.level_weapon_spreadshot,
		Weapon.level_weapon_homing,
		Weapon.level_weapon_bouncer,
		Weapon.level_weapon_charge,
		Weapon.level_weapon_boomerang,
		Weapon.level_weapon_crackshot,
		Weapon.level_weapon_wide_shot,
		Weapon.level_weapon_upshot
	};

	// Token: 0x040040BF RID: 16575
	private static readonly Super[] SUPERS = new Super[]
	{
		Super.level_super_beam,
		Super.level_super_invincible,
		Super.level_super_ghost
	};

	// Token: 0x040040C0 RID: 16576
	private static readonly Super[] CHALICESUPERS = new Super[]
	{
		Super.level_super_chalice_vert_beam,
		Super.level_super_chalice_shield,
		Super.level_super_chalice_iii
	};

	// Token: 0x040040C1 RID: 16577
	private static readonly Charm[] CHARMS = new Charm[]
	{
		Charm.charm_health_up_1,
		Charm.charm_super_builder,
		Charm.charm_smoke_dash,
		Charm.charm_parry_plus,
		Charm.charm_health_up_2,
		Charm.charm_parry_attack,
		Charm.charm_chalice,
		Charm.charm_curse,
		Charm.charm_healer
	};

	// Token: 0x040040C2 RID: 16578
	private static readonly string[] slotLocalesKey = new string[]
	{
		"ShotABackTitle",
		"ShotBBackTitle",
		"SuperBackTitle",
		"CharmBackTitle"
	};

	// Token: 0x040040C3 RID: 16579
	public bool lockInput;

	// Token: 0x040040C4 RID: 16580
	[Header("Text")]
	[SerializeField]
	private LocalizationHelper headerText;

	// Token: 0x040040C5 RID: 16581
	[SerializeField]
	private Text titleText;

	// Token: 0x040040C6 RID: 16582
	[SerializeField]
	private Text exText;

	// Token: 0x040040C7 RID: 16583
	[SerializeField]
	private TMP_Text descriptionText;

	// Token: 0x040040C8 RID: 16584
	[Header("Cursors")]
	[SerializeField]
	private MapEquipUICursor cursor;

	// Token: 0x040040C9 RID: 16585
	[SerializeField]
	private MapEquipUICardBackSelectSelectionCursor selectionCursor;

	// Token: 0x040040CA RID: 16586
	[Header("Backs")]
	[SerializeField]
	private Image iconsBack;

	// Token: 0x040040CB RID: 16587
	[SerializeField]
	private Image superIconsBack;

	// Token: 0x040040CC RID: 16588
	[SerializeField]
	private Image DLCIconsBack;

	// Token: 0x040040CD RID: 16589
	[Header("Icons")]
	[SerializeField]
	private MapEquipUICardBackSelectIcon[] normalIcons;

	// Token: 0x040040CE RID: 16590
	[Header("Super Icons")]
	[SerializeField]
	private MapEquipUICardBackSelectIcon[] superIcons;

	// Token: 0x040040CF RID: 16591
	[Header("DLC Icons")]
	[SerializeField]
	private MapEquipUICardBackSelectIcon[] DLCIcons;

	// Token: 0x040040D0 RID: 16592
	private int index;

	// Token: 0x040040D1 RID: 16593
	private int lastIndex;

	// Token: 0x040040D2 RID: 16594
	private MapEquipUICard.Slot slot;

	// Token: 0x040040D3 RID: 16595
	private MapEquipUICard.Slot lastSlot;

	// Token: 0x040040D4 RID: 16596
	private MapEquipUICardBackSelectIcon[] selectedIcons;

	// Token: 0x040040D5 RID: 16597
	private bool noneUnlocked;

	// Token: 0x040040D6 RID: 16598
	private bool itemSelected;
}
