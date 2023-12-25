using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000993 RID: 2451
public class MapEquipUICardFront : AbstractMapEquipUICardSide
{
	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06003950 RID: 14672 RVA: 0x002086A6 File Offset: 0x00206AA6
	public MapEquipUICard.Slot Slot
	{
		get
		{
			return (MapEquipUICard.Slot)this.index;
		}
	}

	// Token: 0x06003951 RID: 14673 RVA: 0x002086AE File Offset: 0x00206AAE
	private void Update()
	{
		this.SetCursorPosition(this.index);
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x002086BC File Offset: 0x00206ABC
	private void Start()
	{
		Localization.OnLanguageChangedEvent += this.OnLanguageChanged;
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x002086CF File Offset: 0x00206ACF
	private void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x002086E2 File Offset: 0x00206AE2
	private void OnLanguageChanged()
	{
		this.ChangeSelection(0);
	}

	// Token: 0x06003955 RID: 14677 RVA: 0x002086EC File Offset: 0x00206AEC
	public override void Init(PlayerId playerID)
	{
		base.Init(playerID);
		this.icons = new MapEquipUICardFrontIcon[]
		{
			this.weaponA,
			this.weaponB,
			this.super,
			this.item,
			this.checklist
		};
		this.checklist.SetIconsManual("Icons/equip_icon_list", false, false);
		this.checkListSelected = false;
		this.Refresh();
		this.ChangeSelection(0);
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x00208760 File Offset: 0x00206B60
	public void Refresh()
	{
		this.loadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID);
		this.weaponA.SetIcons(this.loadout.primaryWeapon, false);
		this.weaponB.SetIcons(this.loadout.secondaryWeapon, false);
		this.super.SetIcons(this.loadout.super, false);
		if (this.loadout.charm == Charm.charm_curse)
		{
			this.item.SetIconsManual("Icons/equip_icon_charm_curse_" + (CharmCurse.CalculateLevel(base.playerID) + 1).ToString(), false, true);
		}
		else
		{
			this.item.SetIcons(this.loadout.charm, false);
		}
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x00208830 File Offset: 0x00206C30
	public void Unequip()
	{
		if (this.icons[this.index] != this.weaponA)
		{
			this.icons[this.index].SetIcons(WeaponProperties.GetIconPath(Weapon.None));
			if (this.icons[this.index] == this.weaponB)
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).secondaryWeapon = Weapon.None;
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon)
				{
					PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).HasEquippedSecondaryRegularWeapon = false;
					PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).MustNotifySwitchRegularWeapon = false;
				}
			}
			else if (this.icons[this.index] == this.super)
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).super = Super.None;
			}
			else if (this.icons[this.index] == this.item)
			{
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm == Charm.charm_chalice)
				{
					PlayerManager.OnChaliceCharmUnequipped(base.playerID);
				}
				PlayerData.Data.Loadouts.GetPlayerLoadout(base.playerID).charm = Charm.None;
			}
			else
			{
				global::Debug.LogError("Something went wrong", null);
			}
		}
		else
		{
			AudioManager.Play("menu_locked");
			this.cursor.OnLocked();
		}
		this.Refresh();
		this.ChangeSelection(0);
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x002089F0 File Offset: 0x00206DF0
	public void ChangeSelection(int direction)
	{
		if ((this.index != this.icons.Length - 1 && direction != -1) || (this.index != 0 && direction != 1))
		{
			AudioManager.Play("menu_equipment_move");
		}
		this.index = Mathf.Clamp(this.index + direction, 0, this.icons.Length - 1);
		this.SetCursorPosition(this.index);
		this.checkListSelected = (this.index == this.icons.Length - 1);
		string text = string.Empty;
		if (this.icons[this.index] == this.weaponA)
		{
			text = WeaponProperties.GetDisplayName(this.loadout.primaryWeapon);
			if (text.ToUpper() == "ERROR")
			{
				text = Localization.Translate("level_weapon_none_name").text;
			}
			this.title.text = text;
		}
		else if (this.icons[this.index] == this.weaponB)
		{
			text = WeaponProperties.GetDisplayName(this.loadout.secondaryWeapon);
			if (text.ToUpper() == "ERROR")
			{
				text = Localization.Translate("level_weapon_none_name").text;
			}
			this.title.text = text;
		}
		else if (this.icons[this.index] == this.super)
		{
			text = WeaponProperties.GetDisplayName(this.loadout.super);
			if (text.ToUpper() == "ERROR")
			{
				text = Localization.Translate("level_super_none_name").text;
			}
			this.title.text = text;
		}
		else if (this.icons[this.index] == this.item)
		{
			if (this.loadout.charm == Charm.charm_curse)
			{
				if (CharmCurse.CalculateLevel(base.playerID) == -1)
				{
					text = Localization.Translate("charm_broken_name").text;
				}
				else if (CharmCurse.IsMaxLevel(base.playerID))
				{
					text = Localization.Translate("charm_paladin_name").text;
				}
				else
				{
					text = Localization.Translate("charm_curse_name").text;
				}
			}
			else
			{
				text = WeaponProperties.GetDisplayName(this.loadout.charm);
			}
			if (text.ToUpper() == "ERROR")
			{
				text = Localization.Translate("charm_none_name").text;
			}
			this.title.text = text;
		}
		else
		{
			this.title.text = Localization.Translate("list_name").text;
		}
		this.title.font = Localization.Instance.fonts[(int)Localization.language][9].font;
		foreach (Outline outline in this.outlines)
		{
			outline.enabled = (Localization.language == Localization.Languages.Japanese);
		}
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x00208D26 File Offset: 0x00207126
	private void SetCursorPosition(int index)
	{
		if (this.icons == null || this.icons.Length <= index)
		{
			return;
		}
		this.cursor.SetPosition(this.icons[index].transform.position);
	}

	// Token: 0x040040DE RID: 16606
	public MapEquipUICardFrontIcon weaponA;

	// Token: 0x040040DF RID: 16607
	public MapEquipUICardFrontIcon weaponB;

	// Token: 0x040040E0 RID: 16608
	public MapEquipUICardFrontIcon super;

	// Token: 0x040040E1 RID: 16609
	public MapEquipUICardFrontIcon item;

	// Token: 0x040040E2 RID: 16610
	public MapEquipUICardFrontIcon checklist;

	// Token: 0x040040E3 RID: 16611
	public bool checkListSelected;

	// Token: 0x040040E4 RID: 16612
	[Space(10f)]
	public MapEquipUICursor cursor;

	// Token: 0x040040E5 RID: 16613
	private int index;

	// Token: 0x040040E6 RID: 16614
	private MapEquipUICardFrontIcon[] icons;

	// Token: 0x040040E7 RID: 16615
	public Text title;

	// Token: 0x040040E8 RID: 16616
	public Outline[] outlines;

	// Token: 0x040040E9 RID: 16617
	private PlayerData.PlayerLoadouts.PlayerLoadout loadout;
}
