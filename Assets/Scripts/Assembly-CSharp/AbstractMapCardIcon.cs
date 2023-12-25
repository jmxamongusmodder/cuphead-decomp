using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using RektTransform;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x02000985 RID: 2437
public class AbstractMapCardIcon : AbstractMonoBehaviour
{
	// Token: 0x060038F2 RID: 14578 RVA: 0x00205978 File Offset: 0x00203D78
	public void SetIcons(Weapon weapon, bool isGrey)
	{
		string atlasName = AbstractMapCardIcon.DefaultAtlas;
		if (Array.IndexOf<Weapon>(AbstractMapCardIcon.DLCWeapons, weapon) > -1)
		{
			atlasName = AbstractMapCardIcon.DLCAtlas;
			Color white = Color.white;
			if (isGrey)
			{
				white = new Color(1f, 1f, 1f, 0.5f);
			}
			this.iconImage.color = white;
		}
		this.setIcons(WeaponProperties.GetIconPath(weapon), isGrey, atlasName);
	}

	// Token: 0x060038F3 RID: 14579 RVA: 0x002059E3 File Offset: 0x00203DE3
	public void SetIcons(Super super, bool isGrey)
	{
		this.setIcons(WeaponProperties.GetIconPath(super), isGrey, AbstractMapCardIcon.DefaultAtlas);
	}

	// Token: 0x060038F4 RID: 14580 RVA: 0x002059F8 File Offset: 0x00203DF8
	public void SetIcons(Charm charm, bool isGrey)
	{
		string atlasName = AbstractMapCardIcon.DefaultAtlas;
		if (Array.IndexOf<Charm>(AbstractMapCardIcon.DLCCharms, charm) > -1)
		{
			atlasName = AbstractMapCardIcon.DLCAtlas;
		}
		this.setIcons(WeaponProperties.GetIconPath(charm), isGrey, atlasName);
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x00205A30 File Offset: 0x00203E30
	public void SetIconsManual(string iconPath, bool isGrey, bool isDLC = false)
	{
		this.setIcons(iconPath, isGrey, (!isDLC) ? AbstractMapCardIcon.DefaultAtlas : AbstractMapCardIcon.DLCAtlas);
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x00205A50 File Offset: 0x00203E50
	private void setIcons(string iconPath, bool isGrey, string atlasName)
	{
		SpriteAtlas cachedAsset = AssetLoader<SpriteAtlas>.GetCachedAsset(atlasName);
		List<Sprite> list = new List<Sprite>();
		string fileName = Path.GetFileName(iconPath);
		Sprite sprite = this.getSprite(cachedAsset, fileName);
		if (sprite != null)
		{
			list.Add(sprite);
		}
		for (int i = 1; i < 4; i++)
		{
			string arg = "_000";
			string fileName2 = Path.GetFileName(iconPath + arg + i);
			Sprite sprite2 = this.getSprite(cachedAsset, fileName2);
			if (!(sprite2 == null))
			{
				list.Add(sprite2);
			}
		}
		this.normalIcons = list.ToArray();
		list.Clear();
		if (sprite != null)
		{
			list.Add(sprite);
		}
		for (int j = 1; j < 4; j++)
		{
			string arg2 = "_grey_000";
			string fileName3 = Path.GetFileName(iconPath + arg2 + j);
			Sprite sprite3 = this.getSprite(cachedAsset, fileName3);
			if (!(sprite3 == null))
			{
				list.Add(sprite3);
			}
		}
		this.greyIcons = list.ToArray();
		this.icons = ((!isGrey) ? this.normalIcons : this.greyIcons);
		this.StopAllCoroutines();
		if (iconPath != WeaponProperties.GetIconPath(Weapon.None))
		{
			base.StartCoroutine(this.animate_cr());
		}
		else
		{
			this.SetIcon(this.icons[0]);
		}
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x00205BC8 File Offset: 0x00203FC8
	public void SetIcons(string iconPath)
	{
		SpriteAtlas cachedAsset = AssetLoader<SpriteAtlas>.GetCachedAsset("Equip_Icons");
		List<Sprite> list = new List<Sprite>();
		string fileName = Path.GetFileName(iconPath);
		Sprite sprite = this.getSprite(cachedAsset, fileName);
		if (sprite != null)
		{
			list.Add(sprite);
		}
		string fileName2 = Path.GetFileName(iconPath);
		Sprite sprite2 = this.getSprite(cachedAsset, fileName2);
		list.Add(sprite2);
		this.icons = list.ToArray();
		this.SetIcon(sprite2);
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x00205C38 File Offset: 0x00204038
	public virtual void SelectIcon()
	{
		if (base.animator != null)
		{
			base.animator.Play("Select");
		}
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x00205C5B File Offset: 0x0020405B
	public virtual void UnselectIcon()
	{
		if (base.animator != null)
		{
			base.animator.Play("Unselect");
		}
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x00205C7E File Offset: 0x0020407E
	public virtual void OnLocked()
	{
		if (base.animator != null)
		{
			base.animator.Play("Locked");
		}
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x00205CA4 File Offset: 0x002040A4
	private void SetIcon(Sprite sprite)
	{
		if (sprite == null)
		{
			return;
		}
		this.iconImage.sprite = sprite;
		this.iconImage.rectTransform.SetSize(sprite.rect.width, sprite.rect.height);
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x00205CF8 File Offset: 0x002040F8
	private IEnumerator animate_cr()
	{
		int i = 0;
		WaitForSeconds wait = new WaitForSeconds(0.07f);
		this.SetIcon((this.icons != null && this.icons.Length >= 1) ? this.icons[0] : null);
		for (;;)
		{
			yield return wait;
			if (this.icons == null || this.icons.Length < 1)
			{
				this.SetIcon(null);
			}
			else
			{
				i++;
				if (i > this.icons.Length - 1)
				{
					i = 0;
				}
				this.SetIcon(this.icons[i]);
			}
		}
		yield break;
	}

	// Token: 0x060038FD RID: 14589 RVA: 0x00205D14 File Offset: 0x00204114
	private Sprite getSprite(SpriteAtlas atlas, string spriteName)
	{
		return atlas.GetSprite(spriteName);
	}

	// Token: 0x04004088 RID: 16520
	private const float FRAME_DELAY = 0.07f;

	// Token: 0x04004089 RID: 16521
	private static readonly Weapon[] DLCWeapons = new Weapon[]
	{
		Weapon.level_weapon_crackshot,
		Weapon.level_weapon_upshot,
		Weapon.level_weapon_wide_shot
	};

	// Token: 0x0400408A RID: 16522
	private static readonly Charm[] DLCCharms = new Charm[]
	{
		Charm.charm_chalice,
		Charm.charm_curse,
		Charm.charm_healer
	};

	// Token: 0x0400408B RID: 16523
	private static readonly string DefaultAtlas = "Equip_Icons";

	// Token: 0x0400408C RID: 16524
	private static readonly string DLCAtlas = "Equip_Icons_DLC";

	// Token: 0x0400408D RID: 16525
	[SerializeField]
	private Image iconImage;

	// Token: 0x0400408E RID: 16526
	private Sprite[] icons;

	// Token: 0x0400408F RID: 16527
	private Sprite[] normalIcons;

	// Token: 0x04004090 RID: 16528
	private Sprite[] greyIcons;
}
