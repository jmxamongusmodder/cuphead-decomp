using System;

// Token: 0x020002FB RID: 763
public static class WeaponProperties
{
	// Token: 0x06000863 RID: 2147 RVA: 0x00079A28 File Offset: 0x00077E28
	public static string GetDisplayName(Weapon weapon)
	{
		TranslationElement translationElement = Localization.Find(weapon.ToString() + "_name");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00079A6C File Offset: 0x00077E6C
	public static string GetDisplayName(Super super)
	{
		TranslationElement translationElement = Localization.Find(super.ToString() + "_name");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00079AB0 File Offset: 0x00077EB0
	public static string GetDisplayName(Charm charm)
	{
		TranslationElement translationElement = Localization.Find(charm.ToString() + "_name");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00079AF4 File Offset: 0x00077EF4
	public static string GetSubtext(Weapon weapon)
	{
		TranslationElement translationElement = Localization.Find(weapon.ToString() + "_subtext");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00079B38 File Offset: 0x00077F38
	public static string GetSubtext(Super super)
	{
		TranslationElement translationElement = Localization.Find(super.ToString() + "_subtext");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00079B7C File Offset: 0x00077F7C
	public static string GetSubtext(Charm charm)
	{
		TranslationElement translationElement = Localization.Find(charm.ToString() + "_subtext");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00079BC0 File Offset: 0x00077FC0
	public static string GetIconPath(Weapon weapon)
	{
		if (weapon == Weapon.level_weapon_peashot)
		{
			return "Icons/equip_icon_weapon_peashot";
		}
		if (weapon == Weapon.level_weapon_spreadshot)
		{
			return "Icons/equip_icon_weapon_spread";
		}
		if (weapon == Weapon.plane_weapon_peashot)
		{
			return "Icons/equip_icon_weapon_peashot";
		}
		if (weapon == Weapon.level_weapon_arc)
		{
			return "Icons/equip_icon_weapon_peashot";
		}
		if (weapon == Weapon.level_weapon_homing)
		{
			return "Icons/equip_icon_weapon_homing";
		}
		if (weapon == Weapon.level_weapon_exploder)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_charge)
		{
			return "Icons/equip_icon_weapon_charge";
		}
		if (weapon == Weapon.level_weapon_boomerang)
		{
			return "Icons/equip_icon_weapon_boomerang";
		}
		if (weapon == Weapon.level_weapon_bouncer)
		{
			return "Icons/equip_icon_weapon_bouncer";
		}
		if (weapon == Weapon.arcade_weapon_peashot)
		{
			return "Icons/";
		}
		if (weapon == Weapon.plane_weapon_laser)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_wide_shot)
		{
			return "Icons/equip_icon_weapon_wide_shot";
		}
		if (weapon == Weapon.plane_weapon_bomb)
		{
			return "Icons/";
		}
		if (weapon == Weapon.arcade_weapon_rocket_peashot)
		{
			return "Icons/";
		}
		if (weapon == Weapon.plane_chalice_weapon_3way)
		{
			return "Icons/equip_icon_chalice_shmup_3way";
		}
		if (weapon == Weapon.level_weapon_accuracy)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_firecracker)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_firecrackerB)
		{
			return "Icons/";
		}
		if (weapon == Weapon.level_weapon_upshot)
		{
			return "Icons/equip_icon_weapon_upshot";
		}
		if (weapon == Weapon.level_weapon_pushback)
		{
			return "Icons/";
		}
		if (weapon == Weapon.plane_chalice_weapon_bomb)
		{
			return "Icons/equip_icon_chalice_shmup_bomb";
		}
		if (weapon == Weapon.level_weapon_crackshot)
		{
			return "Icons/equip_icon_weapon_crackshot";
		}
		if (weapon == Weapon.level_weapon_splitter)
		{
			return "Icons/";
		}
		if (weapon != Weapon.None)
		{
			return "ERROR";
		}
		return "Icons/equip_icon_empty";
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00079D70 File Offset: 0x00078170
	public static string GetIconPath(Super super)
	{
		if (super == Super.level_super_beam)
		{
			return "Icons/equip_icon_super_beam";
		}
		if (super == Super.level_super_ghost)
		{
			return "Icons/equip_icon_super_ghost";
		}
		if (super == Super.level_super_invincible)
		{
			return "Icons/equip_icon_super_invincible";
		}
		if (super == Super.plane_super_bomb)
		{
			return "Icons/";
		}
		if (super == Super.plane_super_chalice_bomb)
		{
			return "Icons/";
		}
		if (super == Super.level_super_chalice_iii)
		{
			return "Icons/equip_icon_super_ghost";
		}
		if (super == Super.level_super_chalice_vert_beam)
		{
			return "Icons/equip_icon_super_beam";
		}
		if (super == Super.level_super_chalice_shield)
		{
			return "Icons/equip_icon_super_invincible";
		}
		if (super == Super.level_super_chalice_bounce)
		{
			return "Icons/";
		}
		if (super != Super.None)
		{
			return "ERROR";
		}
		return "Icons/equip_icon_empty";
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00079E34 File Offset: 0x00078234
	public static string GetIconPath(Charm charm)
	{
		if (charm == Charm.charm_health_up_1)
		{
			return "Icons/equip_icon_charm_hp1";
		}
		if (charm == Charm.charm_super_builder)
		{
			return "Icons/equip_icon_charm_coffee";
		}
		if (charm == Charm.charm_smoke_dash)
		{
			return "Icons/equip_icon_charm_smoke-dash";
		}
		if (charm == Charm.charm_parry_plus)
		{
			return "Icons/equip_icon_charm_parry_slapper";
		}
		if (charm == Charm.charm_pit_saver)
		{
			return "Icons/equip_icon_charm_pitsaver";
		}
		if (charm == Charm.charm_parry_attack)
		{
			return "Icons/equip_icon_charm_parry_attack";
		}
		if (charm == Charm.charm_health_up_2)
		{
			return "Icons/equip_icon_charm_hp2";
		}
		if (charm == Charm.charm_chalice)
		{
			return "Icons/equip_icon_charm_chalice";
		}
		if (charm == Charm.charm_directional_dash)
		{
			return "Icons/";
		}
		if (charm == Charm.charm_healer)
		{
			return "Icons/equip_icon_charm_healer";
		}
		if (charm == Charm.charm_EX)
		{
			return "Icons/";
		}
		if (charm == Charm.charm_curse)
		{
			return "Icons/equip_icon_charm_curse";
		}
		if (charm == Charm.charm_float)
		{
			return "Icons/";
		}
		if (charm != Charm.None)
		{
			return "ERROR";
		}
		return "Icons/equip_icon_empty";
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00079F3C File Offset: 0x0007833C
	public static string GetDescription(Weapon weapon)
	{
		TranslationElement translationElement = Localization.Find(weapon.ToString() + "_description");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00079F80 File Offset: 0x00078380
	public static string GetDescription(Super super)
	{
		TranslationElement translationElement = Localization.Find(super.ToString() + "_description");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00079FC4 File Offset: 0x000783C4
	public static string GetDescription(Charm charm)
	{
		TranslationElement translationElement = Localization.Find(charm.ToString() + "_description");
		if (translationElement == null)
		{
			return "ERROR";
		}
		return translationElement.translation.text;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0007A008 File Offset: 0x00078408
	public static int GetValue(Weapon weapon)
	{
		if (weapon == Weapon.level_weapon_peashot)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_spreadshot)
		{
			return 4;
		}
		if (weapon == Weapon.plane_weapon_peashot)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_arc)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_homing)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_exploder)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_charge)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_boomerang)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_bouncer)
		{
			return 4;
		}
		if (weapon == Weapon.arcade_weapon_peashot)
		{
			return 2;
		}
		if (weapon == Weapon.plane_weapon_laser)
		{
			return 2;
		}
		if (weapon == Weapon.level_weapon_wide_shot)
		{
			return 4;
		}
		if (weapon == Weapon.plane_weapon_bomb)
		{
			return 2;
		}
		if (weapon == Weapon.arcade_weapon_rocket_peashot)
		{
			return 10;
		}
		if (weapon == Weapon.plane_chalice_weapon_3way)
		{
			return 10;
		}
		if (weapon == Weapon.level_weapon_accuracy)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_firecracker)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_firecrackerB)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_upshot)
		{
			return 4;
		}
		if (weapon == Weapon.level_weapon_pushback)
		{
			return 4;
		}
		if (weapon == Weapon.plane_chalice_weapon_bomb)
		{
			return 10;
		}
		if (weapon == Weapon.level_weapon_crackshot)
		{
			return 4;
		}
		if (weapon != Weapon.level_weapon_splitter)
		{
			return 0;
		}
		return 10;
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0007A14C File Offset: 0x0007854C
	public static int GetValue(Super super)
	{
		if (super == Super.level_super_beam)
		{
			return 0;
		}
		if (super == Super.level_super_ghost)
		{
			return 0;
		}
		if (super == Super.level_super_invincible)
		{
			return 0;
		}
		if (super == Super.plane_super_bomb)
		{
			return 10;
		}
		if (super == Super.plane_super_chalice_bomb)
		{
			return 10;
		}
		if (super == Super.level_super_chalice_iii)
		{
			return 10;
		}
		if (super == Super.level_super_chalice_vert_beam)
		{
			return 10;
		}
		if (super == Super.level_super_chalice_shield)
		{
			return 10;
		}
		if (super != Super.level_super_chalice_bounce)
		{
			return 0;
		}
		return 10;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x0007A1DC File Offset: 0x000785DC
	public static int GetValue(Charm charm)
	{
		if (charm == Charm.charm_health_up_1)
		{
			return 3;
		}
		if (charm == Charm.charm_super_builder)
		{
			return 3;
		}
		if (charm == Charm.charm_smoke_dash)
		{
			return 3;
		}
		if (charm == Charm.charm_parry_plus)
		{
			return 3;
		}
		if (charm == Charm.charm_pit_saver)
		{
			return 3;
		}
		if (charm == Charm.charm_parry_attack)
		{
			return 3;
		}
		if (charm == Charm.charm_health_up_2)
		{
			return 5;
		}
		if (charm == Charm.charm_chalice)
		{
			return 10;
		}
		if (charm == Charm.charm_directional_dash)
		{
			return 10;
		}
		if (charm == Charm.charm_healer)
		{
			return 3;
		}
		if (charm == Charm.charm_EX)
		{
			return 4;
		}
		if (charm == Charm.charm_curse)
		{
			return 1;
		}
		if (charm != Charm.charm_float)
		{
			return 0;
		}
		return 10;
	}

	// Token: 0x020002FC RID: 764
	public static class ArcadeWeaponPeashot
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0007A29B File Offset: 0x0007869B
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.arcade_weapon_peashot);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0007A2A7 File Offset: 0x000786A7
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.arcade_weapon_peashot);
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0007A2B3 File Offset: 0x000786B3
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.arcade_weapon_peashot);
			}
		}

		// Token: 0x040011FF RID: 4607
		public static readonly int value = 2;

		// Token: 0x04001200 RID: 4608
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001201 RID: 4609
		public static readonly Weapon id = Weapon.arcade_weapon_peashot;

		// Token: 0x020002FD RID: 765
		public static class Basic
		{
			// Token: 0x04001202 RID: 4610
			public static readonly float damage = 4f;

			// Token: 0x04001203 RID: 4611
			public static readonly float speed = 850f;

			// Token: 0x04001204 RID: 4612
			public static readonly bool rapidFire;

			// Token: 0x04001205 RID: 4613
			public static readonly float rapidFireRate;
		}

		// Token: 0x020002FE RID: 766
		public static class Ex
		{
		}
	}

	// Token: 0x020002FF RID: 767
	public static class ArcadeWeaponRocketPeashot
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0007A2F1 File Offset: 0x000786F1
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.arcade_weapon_rocket_peashot);
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0007A2FD File Offset: 0x000786FD
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.arcade_weapon_rocket_peashot);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0007A309 File Offset: 0x00078709
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.arcade_weapon_rocket_peashot);
			}
		}

		// Token: 0x04001206 RID: 4614
		public static readonly int value = 10;

		// Token: 0x04001207 RID: 4615
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001208 RID: 4616
		public static readonly Weapon id = Weapon.arcade_weapon_rocket_peashot;

		// Token: 0x02000300 RID: 768
		public static class Basic
		{
			// Token: 0x04001209 RID: 4617
			public static readonly float damage = 4f;

			// Token: 0x0400120A RID: 4618
			public static readonly float speed = 700f;

			// Token: 0x0400120B RID: 4619
			public static readonly bool rapidFire;

			// Token: 0x0400120C RID: 4620
			public static readonly float rapidFireRate;
		}

		// Token: 0x02000301 RID: 769
		public static class Ex
		{
		}
	}

	// Token: 0x02000302 RID: 770
	public static class CharmChalice
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0007A348 File Offset: 0x00078748
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_chalice);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x0007A354 File Offset: 0x00078754
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_chalice);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0007A360 File Offset: 0x00078760
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_chalice);
			}
		}

		// Token: 0x0400120D RID: 4621
		public static readonly int value = 10;

		// Token: 0x0400120E RID: 4622
		public static readonly string iconPath = "Icons/equip_icon_charm_chalice";

		// Token: 0x0400120F RID: 4623
		public static readonly Charm id = Charm.charm_chalice;
	}

	// Token: 0x02000303 RID: 771
	public static class CharmCharmParryPlus
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0007A389 File Offset: 0x00078789
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_parry_plus);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0007A395 File Offset: 0x00078795
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_parry_plus);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0007A3A1 File Offset: 0x000787A1
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_parry_plus);
			}
		}

		// Token: 0x04001210 RID: 4624
		public static readonly int value = 3;

		// Token: 0x04001211 RID: 4625
		public static readonly string iconPath = "Icons/equip_icon_charm_parry_slapper";

		// Token: 0x04001212 RID: 4626
		public static readonly Charm id = Charm.charm_parry_plus;
	}

	// Token: 0x02000304 RID: 772
	public static class CharmCurse
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0007A3C9 File Offset: 0x000787C9
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_curse);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x0007A3D5 File Offset: 0x000787D5
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_curse);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0007A3E1 File Offset: 0x000787E1
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_curse);
			}
		}

		// Token: 0x04001213 RID: 4627
		public static readonly int value = 1;

		// Token: 0x04001214 RID: 4628
		public static readonly string iconPath = "Icons/equip_icon_charm_curse";

		// Token: 0x04001215 RID: 4629
		public static readonly Charm id = Charm.charm_curse;

		// Token: 0x04001216 RID: 4630
		public static readonly int[] availableWeaponIDs = new int[]
		{
			1456773641,
			1456773649,
			1460621839,
			1466518900,
			1466416941,
			1467024095,
			1487081743,
			1568276855,
			1614768724
		};

		// Token: 0x04001217 RID: 4631
		public static readonly int[] availableShmupWeaponIDs = new int[]
		{
			1457006169,
			1492758857
		};

		// Token: 0x04001218 RID: 4632
		public static readonly int[] healthModifierValues = new int[]
		{
			-2,
			-2,
			-2,
			-2,
			0
		};

		// Token: 0x04001219 RID: 4633
		public static readonly float superMeterDelay = 1f;

		// Token: 0x0400121A RID: 4634
		public static readonly float[] superMeterAmount = new float[]
		{
			0f,
			0.13f,
			0.26f,
			0.39f,
			0.52f
		};

		// Token: 0x0400121B RID: 4635
		public static readonly int[] smokeDashInterval = new int[]
		{
			7,
			4,
			2,
			1,
			0
		};

		// Token: 0x0400121C RID: 4636
		public static readonly int[] whetstoneInterval = new int[]
		{
			7,
			4,
			2,
			1,
			0
		};

		// Token: 0x0400121D RID: 4637
		public static readonly string[] healerInterval = new string[]
		{
			"3,3,4",
			"2,3,4",
			"1,3,4",
			"1,2,4",
			"1,2,3"
		};

		// Token: 0x0400121E RID: 4638
		public static readonly int[] levelThreshold = new int[]
		{
			0,
			4,
			8,
			12,
			16
		};
	}

	// Token: 0x02000305 RID: 773
	public static class CharmDirectionalDash
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0007A4F4 File Offset: 0x000788F4
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_directional_dash);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0007A500 File Offset: 0x00078900
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_directional_dash);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0007A50C File Offset: 0x0007890C
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_directional_dash);
			}
		}

		// Token: 0x0400121F RID: 4639
		public static readonly int value = 10;

		// Token: 0x04001220 RID: 4640
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001221 RID: 4641
		public static readonly Charm id = Charm.charm_directional_dash;
	}

	// Token: 0x02000306 RID: 774
	public static class CharmEXCharm
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0007A535 File Offset: 0x00078935
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_EX);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0007A541 File Offset: 0x00078941
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_EX);
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0007A54D File Offset: 0x0007894D
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_EX);
			}
		}

		// Token: 0x04001222 RID: 4642
		public static readonly int value = 4;

		// Token: 0x04001223 RID: 4643
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001224 RID: 4644
		public static readonly Charm id = Charm.charm_EX;

		// Token: 0x04001225 RID: 4645
		public static readonly float planePeashotEXDebuff = 0.15f;
	}

	// Token: 0x02000307 RID: 775
	public static class CharmFloat
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0007A57F File Offset: 0x0007897F
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_float);
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x0007A58B File Offset: 0x0007898B
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_float);
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x0007A597 File Offset: 0x00078997
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_float);
			}
		}

		// Token: 0x04001226 RID: 4646
		public static readonly int value = 10;

		// Token: 0x04001227 RID: 4647
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001228 RID: 4648
		public static readonly Charm id = Charm.charm_float;

		// Token: 0x04001229 RID: 4649
		public static readonly float maxTime = 2f;

		// Token: 0x0400122A RID: 4650
		public static readonly float falloffStartTime = 1f;

		// Token: 0x0400122B RID: 4651
		public static readonly float minFallSpeed = 0.1f;

		// Token: 0x0400122C RID: 4652
		public static readonly float maxFallSpeed = 0.5f;
	}

	// Token: 0x02000308 RID: 776
	public static class CharmHealer
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0007A5F4 File Offset: 0x000789F4
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_healer);
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0007A600 File Offset: 0x00078A00
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_healer);
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x0007A60C File Offset: 0x00078A0C
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_healer);
			}
		}

		// Token: 0x0400122D RID: 4653
		public static readonly int value = 3;

		// Token: 0x0400122E RID: 4654
		public static readonly string iconPath = "Icons/equip_icon_charm_healer";

		// Token: 0x0400122F RID: 4655
		public static readonly Charm id = Charm.charm_healer;
	}

	// Token: 0x02000309 RID: 777
	public static class CharmHealthUpOne
	{
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x0007A634 File Offset: 0x00078A34
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_health_up_1);
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0007A640 File Offset: 0x00078A40
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_health_up_1);
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0007A64C File Offset: 0x00078A4C
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_health_up_1);
			}
		}

		// Token: 0x04001230 RID: 4656
		public static readonly int value = 3;

		// Token: 0x04001231 RID: 4657
		public static readonly string iconPath = "Icons/equip_icon_charm_hp1";

		// Token: 0x04001232 RID: 4658
		public static readonly Charm id = Charm.charm_health_up_1;

		// Token: 0x04001233 RID: 4659
		public static readonly int healthIncrease = 1;

		// Token: 0x04001234 RID: 4660
		public static readonly float weaponDebuff = 0.05f;
	}

	// Token: 0x0200030A RID: 778
	public static class CharmHealthUpTwo
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x0007A684 File Offset: 0x00078A84
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_health_up_2);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x0007A690 File Offset: 0x00078A90
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_health_up_2);
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x0007A69C File Offset: 0x00078A9C
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_health_up_2);
			}
		}

		// Token: 0x04001235 RID: 4661
		public static readonly int value = 5;

		// Token: 0x04001236 RID: 4662
		public static readonly string iconPath = "Icons/equip_icon_charm_hp2";

		// Token: 0x04001237 RID: 4663
		public static readonly Charm id = Charm.charm_health_up_2;

		// Token: 0x04001238 RID: 4664
		public static readonly int healthIncrease = 2;

		// Token: 0x04001239 RID: 4665
		public static readonly float weaponDebuff = 0.1f;
	}

	// Token: 0x0200030B RID: 779
	public static class CharmParryAttack
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0007A6D4 File Offset: 0x00078AD4
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_parry_attack);
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0007A6E0 File Offset: 0x00078AE0
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_parry_attack);
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0007A6EC File Offset: 0x00078AEC
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_parry_attack);
			}
		}

		// Token: 0x0400123A RID: 4666
		public static readonly int value = 3;

		// Token: 0x0400123B RID: 4667
		public static readonly string iconPath = "Icons/equip_icon_charm_parry_attack";

		// Token: 0x0400123C RID: 4668
		public static readonly Charm id = Charm.charm_parry_attack;

		// Token: 0x0400123D RID: 4669
		public static readonly float damage = 16f;

		// Token: 0x0400123E RID: 4670
		public static readonly float bounce;
	}

	// Token: 0x0200030C RID: 780
	public static class CharmPitSaver
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0007A71E File Offset: 0x00078B1E
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_pit_saver);
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0007A72A File Offset: 0x00078B2A
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_pit_saver);
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0007A736 File Offset: 0x00078B36
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_pit_saver);
			}
		}

		// Token: 0x0400123F RID: 4671
		public static readonly int value = 3;

		// Token: 0x04001240 RID: 4672
		public static readonly string iconPath = "Icons/equip_icon_charm_pitsaver";

		// Token: 0x04001241 RID: 4673
		public static readonly Charm id = Charm.charm_pit_saver;

		// Token: 0x04001242 RID: 4674
		public static readonly float meterAmount = 10f;

		// Token: 0x04001243 RID: 4675
		public static readonly float invulnerabilityMultiplier = 1.6f;
	}

	// Token: 0x0200030D RID: 781
	public static class CharmSmokeDash
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0007A772 File Offset: 0x00078B72
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_smoke_dash);
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0007A77E File Offset: 0x00078B7E
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_smoke_dash);
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0007A78A File Offset: 0x00078B8A
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_smoke_dash);
			}
		}

		// Token: 0x04001244 RID: 4676
		public static readonly int value = 3;

		// Token: 0x04001245 RID: 4677
		public static readonly string iconPath = "Icons/equip_icon_charm_smoke-dash";

		// Token: 0x04001246 RID: 4678
		public static readonly Charm id = Charm.charm_smoke_dash;
	}

	// Token: 0x0200030E RID: 782
	public static class CharmSuperBuilder
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0007A7B2 File Offset: 0x00078BB2
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Charm.charm_super_builder);
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0007A7BE File Offset: 0x00078BBE
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Charm.charm_super_builder);
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0007A7CA File Offset: 0x00078BCA
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Charm.charm_super_builder);
			}
		}

		// Token: 0x04001247 RID: 4679
		public static readonly int value = 3;

		// Token: 0x04001248 RID: 4680
		public static readonly string iconPath = "Icons/equip_icon_charm_coffee";

		// Token: 0x04001249 RID: 4681
		public static readonly Charm id = Charm.charm_super_builder;

		// Token: 0x0400124A RID: 4682
		public static readonly float delay = 1f;

		// Token: 0x0400124B RID: 4683
		public static readonly float amount = 0.4f;
	}

	// Token: 0x0200030F RID: 783
	public static class LevelSuperBeam
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0007A806 File Offset: 0x00078C06
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_beam);
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0007A812 File Offset: 0x00078C12
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_beam);
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0007A81E File Offset: 0x00078C1E
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_beam);
			}
		}

		// Token: 0x0400124C RID: 4684
		public static readonly int value;

		// Token: 0x0400124D RID: 4685
		public static readonly string iconPath = "Icons/equip_icon_super_beam";

		// Token: 0x0400124E RID: 4686
		public static readonly Super id = Super.level_super_beam;

		// Token: 0x0400124F RID: 4687
		public static readonly float time = 1.25f;

		// Token: 0x04001250 RID: 4688
		public static readonly float damage = 14.5f;

		// Token: 0x04001251 RID: 4689
		public static readonly float damageRate = 0.25f;
	}

	// Token: 0x02000310 RID: 784
	public static class LevelSuperChaliceBounce
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0007A85E File Offset: 0x00078C5E
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_chalice_bounce);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0007A86A File Offset: 0x00078C6A
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_chalice_bounce);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x0007A876 File Offset: 0x00078C76
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_chalice_bounce);
			}
		}

		// Token: 0x04001252 RID: 4690
		public static readonly int value = 10;

		// Token: 0x04001253 RID: 4691
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001254 RID: 4692
		public static readonly Super id = Super.level_super_chalice_bounce;

		// Token: 0x04001255 RID: 4693
		public static readonly bool launchedVersion = true;

		// Token: 0x04001256 RID: 4694
		public static readonly float damage = 30f;

		// Token: 0x04001257 RID: 4695
		public static readonly float damageRate = 1f;

		// Token: 0x04001258 RID: 4696
		public static readonly float maxDamage = 300f;

		// Token: 0x04001259 RID: 4697
		public static readonly float duration = 7.5f;

		// Token: 0x0400125A RID: 4698
		public static readonly float horizontalAcceleration = 1200f;

		// Token: 0x0400125B RID: 4699
		public static readonly float maxHorizontalSpeed = 1000f;

		// Token: 0x0400125C RID: 4700
		public static readonly float bounceVelocity = 2250f;

		// Token: 0x0400125D RID: 4701
		public static readonly float bounceModifierNoJump = 1f;

		// Token: 0x0400125E RID: 4702
		public static readonly float gravity = 7000f;

		// Token: 0x0400125F RID: 4703
		public static readonly float enemyReboundMultiplier = 2f;

		// Token: 0x04001260 RID: 4704
		public static readonly float enemyMultihitDelay = 0.5f;
	}

	// Token: 0x02000311 RID: 785
	public static class LevelSuperChaliceIII
	{
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x0007A920 File Offset: 0x00078D20
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_chalice_iii);
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0007A92C File Offset: 0x00078D2C
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_chalice_iii);
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0007A938 File Offset: 0x00078D38
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_chalice_iii);
			}
		}

		// Token: 0x04001261 RID: 4705
		public static readonly int value = 10;

		// Token: 0x04001262 RID: 4706
		public static readonly string iconPath = "Icons/equip_icon_super_ghost";

		// Token: 0x04001263 RID: 4707
		public static readonly Super id = Super.level_super_chalice_iii;

		// Token: 0x04001264 RID: 4708
		public static readonly float superDuration = 6.5f;
	}

	// Token: 0x02000312 RID: 786
	public static class LevelSuperChaliceShield
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0007A96B File Offset: 0x00078D6B
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_chalice_shield);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0007A977 File Offset: 0x00078D77
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_chalice_shield);
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0007A983 File Offset: 0x00078D83
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_chalice_shield);
			}
		}

		// Token: 0x04001265 RID: 4709
		public static readonly int value = 10;

		// Token: 0x04001266 RID: 4710
		public static readonly string iconPath = "Icons/equip_icon_super_invincible";

		// Token: 0x04001267 RID: 4711
		public static readonly Super id = Super.level_super_chalice_shield;
	}

	// Token: 0x02000313 RID: 787
	public static class LevelSuperChaliceVertBeam
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0007A9AC File Offset: 0x00078DAC
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_chalice_vert_beam);
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0007A9B8 File Offset: 0x00078DB8
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_chalice_vert_beam);
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0007A9C4 File Offset: 0x00078DC4
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_chalice_vert_beam);
			}
		}

		// Token: 0x04001268 RID: 4712
		public static readonly int value = 10;

		// Token: 0x04001269 RID: 4713
		public static readonly string iconPath = "Icons/equip_icon_super_beam";

		// Token: 0x0400126A RID: 4714
		public static readonly Super id = Super.level_super_chalice_vert_beam;

		// Token: 0x0400126B RID: 4715
		public static readonly float time = 1.25f;

		// Token: 0x0400126C RID: 4716
		public static readonly float damage = 21.5f;

		// Token: 0x0400126D RID: 4717
		public static readonly float damageRate = 0.25f;
	}

	// Token: 0x02000314 RID: 788
	public static class LevelSuperGhost
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0007AA0B File Offset: 0x00078E0B
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_ghost);
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0007AA17 File Offset: 0x00078E17
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_ghost);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0007AA23 File Offset: 0x00078E23
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_ghost);
			}
		}

		// Token: 0x0400126E RID: 4718
		public static readonly int value;

		// Token: 0x0400126F RID: 4719
		public static readonly string iconPath = "Icons/equip_icon_super_ghost";

		// Token: 0x04001270 RID: 4720
		public static readonly Super id = Super.level_super_ghost;

		// Token: 0x04001271 RID: 4721
		public static readonly float initialSpeed = 700f;

		// Token: 0x04001272 RID: 4722
		public static readonly float maxSpeed = 1250f;

		// Token: 0x04001273 RID: 4723
		public static readonly float initialSpeedTime = 1.8f;

		// Token: 0x04001274 RID: 4724
		public static readonly float maxSpeedTime = 3.8f;

		// Token: 0x04001275 RID: 4725
		public static readonly float noHeartMaxSpeedTime = 3.7f;

		// Token: 0x04001276 RID: 4726
		public static readonly float accelerationTime = 1f;

		// Token: 0x04001277 RID: 4727
		public static readonly float heartSpeed = 100f;

		// Token: 0x04001278 RID: 4728
		public static readonly float damage = 5.1f;

		// Token: 0x04001279 RID: 4729
		public static readonly float damageRate = 0.22f;

		// Token: 0x0400127A RID: 4730
		public static readonly float turnaroundEaseMultiplier = 4f;
	}

	// Token: 0x02000315 RID: 789
	public static class LevelSuperInvincibility
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0007AAB5 File Offset: 0x00078EB5
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.level_super_invincible);
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0007AAC1 File Offset: 0x00078EC1
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.level_super_invincible);
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0007AACD File Offset: 0x00078ECD
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.level_super_invincible);
			}
		}

		// Token: 0x0400127B RID: 4731
		public static readonly int value;

		// Token: 0x0400127C RID: 4732
		public static readonly string iconPath = "Icons/equip_icon_super_invincible";

		// Token: 0x0400127D RID: 4733
		public static readonly Super id = Super.level_super_invincible;

		// Token: 0x0400127E RID: 4734
		public static readonly float durationInvincible = 4.85f;

		// Token: 0x0400127F RID: 4735
		public static readonly float durationFX = 4.55f;
	}

	// Token: 0x02000316 RID: 790
	public static class LevelWeaponAccuracy
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0007AB03 File Offset: 0x00078F03
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_accuracy);
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0007AB0F File Offset: 0x00078F0F
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_accuracy);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x0007AB1B File Offset: 0x00078F1B
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_accuracy);
			}
		}

		// Token: 0x04001280 RID: 4736
		public static readonly int value = 4;

		// Token: 0x04001281 RID: 4737
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001282 RID: 4738
		public static readonly Weapon id = Weapon.level_weapon_accuracy;

		// Token: 0x02000317 RID: 791
		public static class Basic
		{
			// Token: 0x04001283 RID: 4739
			public static readonly float LvlOneFireRate = 0.28f;

			// Token: 0x04001284 RID: 4740
			public static readonly float LvlOneSpeed = 1200f;

			// Token: 0x04001285 RID: 4741
			public static readonly float LvlOneSize = 1.2f;

			// Token: 0x04001286 RID: 4742
			public static readonly float LvlOneDamage = 4f;

			// Token: 0x04001287 RID: 4743
			public static readonly int LvlTwoCounter = 20;

			// Token: 0x04001288 RID: 4744
			public static readonly float LvlTwoFireRate = 0.23f;

			// Token: 0x04001289 RID: 4745
			public static readonly float LvlTwoSpeed = 1500f;

			// Token: 0x0400128A RID: 4746
			public static readonly float LvlTwoSize = 1.8f;

			// Token: 0x0400128B RID: 4747
			public static readonly float LvlTwoDamage = 5.5f;

			// Token: 0x0400128C RID: 4748
			public static readonly int LvlThreeCounter = 40;

			// Token: 0x0400128D RID: 4749
			public static readonly float LvlThreeFireRate = 0.2f;

			// Token: 0x0400128E RID: 4750
			public static readonly float LvlThreeSpeed = 1900f;

			// Token: 0x0400128F RID: 4751
			public static readonly float LvlThreeSize = 3.2f;

			// Token: 0x04001290 RID: 4752
			public static readonly float LvlThreeDamage = 7.5f;

			// Token: 0x04001291 RID: 4753
			public static readonly int LvlFourCounter = 60;

			// Token: 0x04001292 RID: 4754
			public static readonly float LvlFourFireRate = 0.18f;

			// Token: 0x04001293 RID: 4755
			public static readonly float LvlFourSpeed = 2350f;

			// Token: 0x04001294 RID: 4756
			public static readonly float LvlFourSize = 5f;

			// Token: 0x04001295 RID: 4757
			public static readonly float LvlFourDamage = 8.5f;
		}

		// Token: 0x02000318 RID: 792
		public static class Ex
		{
			// Token: 0x04001296 RID: 4758
			public static readonly float exSpeed = 1800f;

			// Token: 0x04001297 RID: 4759
			public static readonly float exDamage = 25f;

			// Token: 0x04001298 RID: 4760
			public static readonly int exShotEquivalent = 15;

			// Token: 0x04001299 RID: 4761
			public static readonly float exShotSize = 8f;
		}
	}

	// Token: 0x02000319 RID: 793
	public static class LevelWeaponArc
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0007AC2D File Offset: 0x0007902D
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_arc);
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0007AC39 File Offset: 0x00079039
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_arc);
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x0007AC45 File Offset: 0x00079045
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_arc);
			}
		}

		// Token: 0x0400129A RID: 4762
		public static readonly int value = 2;

		// Token: 0x0400129B RID: 4763
		public static readonly string iconPath = "Icons/equip_icon_weapon_peashot";

		// Token: 0x0400129C RID: 4764
		public static readonly Weapon id = Weapon.level_weapon_arc;

		// Token: 0x0200031A RID: 794
		public static class Basic
		{
			// Token: 0x0400129D RID: 4765
			public static readonly int Movement;

			// Token: 0x0400129E RID: 4766
			public static readonly float launchSpeed = 1600f;

			// Token: 0x0400129F RID: 4767
			public static readonly float gravity = 2750f;

			// Token: 0x040012A0 RID: 4768
			public static readonly float straightShotAngle = 65f;

			// Token: 0x040012A1 RID: 4769
			public static readonly float fireRate = 0.4f;

			// Token: 0x040012A2 RID: 4770
			public static readonly bool rapidFire = true;

			// Token: 0x040012A3 RID: 4771
			public static readonly int maxNumMines = 1;

			// Token: 0x040012A4 RID: 4772
			public static readonly float baseDamage = 14f;

			// Token: 0x040012A5 RID: 4773
			public static readonly float timeStateTwo = 1.25f;

			// Token: 0x040012A6 RID: 4774
			public static readonly float damageStateTwo = 7.5f;

			// Token: 0x040012A7 RID: 4775
			public static readonly float timeStateThree = 2.5f;

			// Token: 0x040012A8 RID: 4776
			public static readonly float damageStateThree = 11.25f;

			// Token: 0x040012A9 RID: 4777
			public static readonly float diagLaunchSpeed = 600f;

			// Token: 0x040012AA RID: 4778
			public static readonly float diagGravity = 1000f;

			// Token: 0x040012AB RID: 4779
			public static readonly float diagShotAngle = 45f;
		}

		// Token: 0x0200031B RID: 795
		public static class Ex
		{
			// Token: 0x040012AC RID: 4780
			public static readonly float launchSpeed = 1600f;

			// Token: 0x040012AD RID: 4781
			public static readonly float gravity = 2750f;

			// Token: 0x040012AE RID: 4782
			public static readonly float damage = 28f;

			// Token: 0x040012AF RID: 4783
			public static readonly float explodeDelay = 2f;
		}
	}

	// Token: 0x0200031C RID: 796
	public static class LevelWeaponBoomerang
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0007AD2B File Offset: 0x0007912B
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_boomerang);
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0007AD37 File Offset: 0x00079137
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_boomerang);
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0007AD43 File Offset: 0x00079143
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_boomerang);
			}
		}

		// Token: 0x040012B0 RID: 4784
		public static readonly int value = 4;

		// Token: 0x040012B1 RID: 4785
		public static readonly string iconPath = "Icons/equip_icon_weapon_boomerang";

		// Token: 0x040012B2 RID: 4786
		public static readonly Weapon id = Weapon.level_weapon_boomerang;

		// Token: 0x0200031D RID: 797
		public static class Basic
		{
			// Token: 0x040012B3 RID: 4787
			public static readonly float fireRate = 0.25f;

			// Token: 0x040012B4 RID: 4788
			public static readonly float speed = 1400f;

			// Token: 0x040012B5 RID: 4789
			public static readonly float damage = 8.5f;

			// Token: 0x040012B6 RID: 4790
			public static readonly string xDistanceString = "550,450,520,480";

			// Token: 0x040012B7 RID: 4791
			public static readonly string yDistanceString = "100,  50,  80, 70";
		}

		// Token: 0x0200031E RID: 798
		public static class Ex
		{
			// Token: 0x040012B8 RID: 4792
			public static readonly float speed = 1000f;

			// Token: 0x040012B9 RID: 4793
			public static readonly float damage = 5f;

			// Token: 0x040012BA RID: 4794
			public static readonly float damageRate = 0.2f;

			// Token: 0x040012BB RID: 4795
			public static readonly float maxDamage = 35f;

			// Token: 0x040012BC RID: 4796
			public static readonly float xDistance = 400f;

			// Token: 0x040012BD RID: 4797
			public static readonly float yDistance = 110f;

			// Token: 0x040012BE RID: 4798
			public static readonly string pinkString = "2,3,2,4";

			// Token: 0x040012BF RID: 4799
			public static readonly float hitFreezeTime = 0.1f;
		}
	}

	// Token: 0x0200031F RID: 799
	public static class LevelWeaponBouncer
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0007ADFD File Offset: 0x000791FD
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_bouncer);
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0007AE09 File Offset: 0x00079209
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_bouncer);
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0007AE15 File Offset: 0x00079215
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_bouncer);
			}
		}

		// Token: 0x040012C0 RID: 4800
		public static readonly int value = 4;

		// Token: 0x040012C1 RID: 4801
		public static readonly string iconPath = "Icons/equip_icon_weapon_bouncer";

		// Token: 0x040012C2 RID: 4802
		public static readonly Weapon id = Weapon.level_weapon_bouncer;

		// Token: 0x02000320 RID: 800
		public static class Basic
		{
			// Token: 0x040012C3 RID: 4803
			public static readonly float launchSpeed = 1200f;

			// Token: 0x040012C4 RID: 4804
			public static readonly float gravity = 3600f;

			// Token: 0x040012C5 RID: 4805
			public static readonly float bounceRatio = 1.3f;

			// Token: 0x040012C6 RID: 4806
			public static readonly float bounceSpeedDampening = 800f;

			// Token: 0x040012C7 RID: 4807
			public static readonly float straightExtraAngle = 22.5f;

			// Token: 0x040012C8 RID: 4808
			public static readonly float diagonalUpExtraAngle;

			// Token: 0x040012C9 RID: 4809
			public static readonly float diagonalDownExtraAngle = 10f;

			// Token: 0x040012CA RID: 4810
			public static readonly float damage = 11.6f;

			// Token: 0x040012CB RID: 4811
			public static readonly float fireRate = 0.33f;

			// Token: 0x040012CC RID: 4812
			public static readonly int numBounces = 2;
		}

		// Token: 0x02000321 RID: 801
		public static class Ex
		{
			// Token: 0x040012CD RID: 4813
			public static readonly float launchSpeed = 1600f;

			// Token: 0x040012CE RID: 4814
			public static readonly float gravity = 2750f;

			// Token: 0x040012CF RID: 4815
			public static readonly float damage = 28f;

			// Token: 0x040012D0 RID: 4816
			public static readonly float explodeDelay = 2f;
		}
	}

	// Token: 0x02000322 RID: 802
	public static class LevelWeaponCharge
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0007AECD File Offset: 0x000792CD
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_charge);
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0007AED9 File Offset: 0x000792D9
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_charge);
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0007AEE5 File Offset: 0x000792E5
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_charge);
			}
		}

		// Token: 0x040012D1 RID: 4817
		public static readonly int value = 4;

		// Token: 0x040012D2 RID: 4818
		public static readonly string iconPath = "Icons/equip_icon_weapon_charge";

		// Token: 0x040012D3 RID: 4819
		public static readonly Weapon id = Weapon.level_weapon_charge;

		// Token: 0x02000323 RID: 803
		public static class Basic
		{
			// Token: 0x040012D4 RID: 4820
			public static readonly float fireRate = 0.25f;

			// Token: 0x040012D5 RID: 4821
			public static readonly float baseDamage = 6f;

			// Token: 0x040012D6 RID: 4822
			public static readonly float speed = 1050f;

			// Token: 0x040012D7 RID: 4823
			public static readonly float timeStateTwo = 9999f;

			// Token: 0x040012D8 RID: 4824
			public static readonly float damageStateTwo = 20f;

			// Token: 0x040012D9 RID: 4825
			public static readonly float speedStateTwo = 1300f;

			// Token: 0x040012DA RID: 4826
			public static readonly float timeStateThree = 1f;

			// Token: 0x040012DB RID: 4827
			public static readonly float damageStateThree = 46f;
		}

		// Token: 0x02000324 RID: 804
		public static class Ex
		{
			// Token: 0x040012DC RID: 4828
			public static readonly float damage = 26f;

			// Token: 0x040012DD RID: 4829
			public static readonly float radius = 300f;
		}
	}

	// Token: 0x02000325 RID: 805
	public static class LevelWeaponCrackshot
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0007AF83 File Offset: 0x00079383
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_crackshot);
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0007AF8F File Offset: 0x0007938F
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_crackshot);
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0007AF9B File Offset: 0x0007939B
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_crackshot);
			}
		}

		// Token: 0x040012DE RID: 4830
		public static readonly int value = 4;

		// Token: 0x040012DF RID: 4831
		public static readonly string iconPath = "Icons/equip_icon_weapon_crackshot";

		// Token: 0x040012E0 RID: 4832
		public static readonly Weapon id = Weapon.level_weapon_crackshot;

		// Token: 0x02000326 RID: 806
		public static class Basic
		{
			// Token: 0x040012E1 RID: 4833
			public static readonly float fireRate = 0.32f;

			// Token: 0x040012E2 RID: 4834
			public static readonly float initialSpeed = 1050f;

			// Token: 0x040012E3 RID: 4835
			public static readonly float crackDistance = 290f;

			// Token: 0x040012E4 RID: 4836
			public static readonly float crackedSpeed = 2500f;

			// Token: 0x040012E5 RID: 4837
			public static readonly float initialDamage = 10.56f;

			// Token: 0x040012E6 RID: 4838
			public static readonly float crackedDamage = 6.7f;

			// Token: 0x040012E7 RID: 4839
			public static readonly bool enableMaxAngle = true;

			// Token: 0x040012E8 RID: 4840
			public static readonly float maxAngle = 170f;
		}

		// Token: 0x02000327 RID: 807
		public static class Ex
		{
			// Token: 0x040012E9 RID: 4841
			public static readonly float launchDistance = 100f;

			// Token: 0x040012EA RID: 4842
			public static readonly float timeToHoverPoint = 0.5f;

			// Token: 0x040012EB RID: 4843
			public static readonly float hoverWidth = 37f;

			// Token: 0x040012EC RID: 4844
			public static readonly float hoverHeight = 35f;

			// Token: 0x040012ED RID: 4845
			public static readonly float hoverSpeed = 0.9f;

			// Token: 0x040012EE RID: 4846
			public static readonly float bulletSpeed = 2000f;

			// Token: 0x040012EF RID: 4847
			public static readonly float bulletDamage = 3.5f;

			// Token: 0x040012F0 RID: 4848
			public static readonly float collideDamage = 12f;

			// Token: 0x040012F1 RID: 4849
			public static readonly int shotNumber = 5;

			// Token: 0x040012F2 RID: 4850
			public static readonly float shootDelay = 1f;

			// Token: 0x040012F3 RID: 4851
			public static readonly float riseSpeed;

			// Token: 0x040012F4 RID: 4852
			public static readonly bool isPink = true;

			// Token: 0x040012F5 RID: 4853
			public static readonly float parryBulletDamage = 14f;

			// Token: 0x040012F6 RID: 4854
			public static readonly float parryBulletSpeed = 2000f;

			// Token: 0x040012F7 RID: 4855
			public static readonly float parryTimeOut = 0.15f;
		}
	}

	// Token: 0x02000328 RID: 808
	public static class LevelWeaponExploder
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0007B0B1 File Offset: 0x000794B1
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_exploder);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0007B0BD File Offset: 0x000794BD
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_exploder);
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0007B0C9 File Offset: 0x000794C9
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_exploder);
			}
		}

		// Token: 0x040012F8 RID: 4856
		public static readonly int value = 2;

		// Token: 0x040012F9 RID: 4857
		public static readonly string iconPath = "Icons/";

		// Token: 0x040012FA RID: 4858
		public static readonly Weapon id = Weapon.level_weapon_exploder;

		// Token: 0x02000329 RID: 809
		public static class Basic
		{
			// Token: 0x040012FB RID: 4859
			public static readonly float fireRate = 0.35f;

			// Token: 0x040012FC RID: 4860
			public static readonly bool rapideFire = true;

			// Token: 0x040012FD RID: 4861
			public static readonly float speed = 1200f;

			// Token: 0x040012FE RID: 4862
			public static readonly float sinSpeed = 10f;

			// Token: 0x040012FF RID: 4863
			public static readonly float sinSize = 0.1f;

			// Token: 0x04001300 RID: 4864
			public static readonly float baseDamage = 6f;

			// Token: 0x04001301 RID: 4865
			public static readonly float baseExplosionRadius = 15f;

			// Token: 0x04001302 RID: 4866
			public static readonly float baseScale = 0.1f;

			// Token: 0x04001303 RID: 4867
			public static readonly float timeStateTwo = 0.25f;

			// Token: 0x04001304 RID: 4868
			public static readonly float damageStateTwo = 10f;

			// Token: 0x04001305 RID: 4869
			public static readonly float explosionRadiusStateTwo = 70f;

			// Token: 0x04001306 RID: 4870
			public static readonly float scaleStateTwo = 0.5f;

			// Token: 0x04001307 RID: 4871
			public static readonly float timeStateThree = 0.5f;

			// Token: 0x04001308 RID: 4872
			public static readonly float damageStateThree = 12.75f;

			// Token: 0x04001309 RID: 4873
			public static readonly float explosionRadiusStateThree = 130f;

			// Token: 0x0400130A RID: 4874
			public static readonly float scaleStateThree = 1f;

			// Token: 0x0400130B RID: 4875
			public static readonly bool easing = true;

			// Token: 0x0400130C RID: 4876
			public static readonly MinMax easeSpeed = new MinMax(900f, 2500f);

			// Token: 0x0400130D RID: 4877
			public static readonly float easeTime = 1f;
		}

		// Token: 0x0200032A RID: 810
		public static class Ex
		{
			// Token: 0x0400130E RID: 4878
			public static readonly float speed = 1300f;

			// Token: 0x0400130F RID: 4879
			public static readonly float damage = 35f;

			// Token: 0x04001310 RID: 4880
			public static readonly float hitRate;

			// Token: 0x04001311 RID: 4881
			public static readonly float explodeRadius = 300f;

			// Token: 0x04001312 RID: 4882
			public static readonly float shrapnelSpeed = 1200f;

			// Token: 0x04001313 RID: 4883
			public static readonly bool damageOn = true;
		}
	}

	// Token: 0x0200032B RID: 811
	public static class LevelWeaponFirecracker
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0007B1F1 File Offset: 0x000795F1
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_firecracker);
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0007B1FD File Offset: 0x000795FD
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_firecracker);
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0007B209 File Offset: 0x00079609
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_firecracker);
			}
		}

		// Token: 0x04001314 RID: 4884
		public static readonly int value = 4;

		// Token: 0x04001315 RID: 4885
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001316 RID: 4886
		public static readonly Weapon id = Weapon.level_weapon_firecracker;

		// Token: 0x0200032C RID: 812
		public static class Basic
		{
			// Token: 0x04001317 RID: 4887
			public static readonly float fireRate = 0.06f;

			// Token: 0x04001318 RID: 4888
			public static readonly float bulletSpeed = 2250f;

			// Token: 0x04001319 RID: 4889
			public static readonly float bulletLife = 0.17f;

			// Token: 0x0400131A RID: 4890
			public static readonly float explosionDamage = 2.6f;

			// Token: 0x0400131B RID: 4891
			public static readonly float explosionSize = 10f;

			// Token: 0x0400131C RID: 4892
			public static readonly float explosionDuration = 0.1f;
		}

		// Token: 0x0200032D RID: 813
		public static class Ex
		{
			// Token: 0x0400131D RID: 4893
			public static readonly float exSpeed = 1700f;

			// Token: 0x0400131E RID: 4894
			public static readonly float explosionRadius = 20f;

			// Token: 0x0400131F RID: 4895
			public static readonly float damageRate = 0.5f;

			// Token: 0x04001320 RID: 4896
			public static readonly float explosionDamage = 3f;

			// Token: 0x04001321 RID: 4897
			public static readonly float explosionTime = 2f;

			// Token: 0x04001322 RID: 4898
			public static readonly float exLife = 1f;
		}
	}

	// Token: 0x0200032E RID: 814
	public static class LevelWeaponFirecrackerB
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0007B2AD File Offset: 0x000796AD
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_firecrackerB);
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x0007B2B9 File Offset: 0x000796B9
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_firecrackerB);
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0007B2C5 File Offset: 0x000796C5
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_firecrackerB);
			}
		}

		// Token: 0x04001323 RID: 4899
		public static readonly int value = 4;

		// Token: 0x04001324 RID: 4900
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001325 RID: 4901
		public static readonly Weapon id = Weapon.level_weapon_firecrackerB;

		// Token: 0x0200032F RID: 815
		public static class Basic
		{
			// Token: 0x04001326 RID: 4902
			public static readonly float fireRate = 0.09f;

			// Token: 0x04001327 RID: 4903
			public static readonly float bulletSpeed = 2000f;

			// Token: 0x04001328 RID: 4904
			public static readonly float bulletLife = 0.2f;

			// Token: 0x04001329 RID: 4905
			public static readonly float explosionDamage = 0.5f;

			// Token: 0x0400132A RID: 4906
			public static readonly float explosionSize = 5f;

			// Token: 0x0400132B RID: 4907
			public static readonly float explosionDuration = 0.16f;

			// Token: 0x0400132C RID: 4908
			public static readonly string explosionAngleString = "45,180,270,135,315,225,90,0";

			// Token: 0x0400132D RID: 4909
			public static readonly float explosionsRadiusSize = 68f;
		}

		// Token: 0x02000330 RID: 816
		public static class Ex
		{
			// Token: 0x0400132E RID: 4910
			public static readonly float exSpeed = 1700f;

			// Token: 0x0400132F RID: 4911
			public static readonly float explosionRadius = 20f;

			// Token: 0x04001330 RID: 4912
			public static readonly float damageRate = 0.5f;

			// Token: 0x04001331 RID: 4913
			public static readonly float explosionDamage = 3f;

			// Token: 0x04001332 RID: 4914
			public static readonly float explosionTime = 2f;

			// Token: 0x04001333 RID: 4915
			public static readonly float exLife = 1f;
		}
	}

	// Token: 0x02000331 RID: 817
	public static class LevelWeaponHoming
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x0007B38B File Offset: 0x0007978B
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_homing);
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0007B397 File Offset: 0x00079797
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_homing);
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0007B3A3 File Offset: 0x000797A3
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_homing);
			}
		}

		// Token: 0x04001334 RID: 4916
		public static readonly int value = 4;

		// Token: 0x04001335 RID: 4917
		public static readonly string iconPath = "Icons/equip_icon_weapon_homing";

		// Token: 0x04001336 RID: 4918
		public static readonly Weapon id = Weapon.level_weapon_homing;

		// Token: 0x02000332 RID: 818
		public static class Basic
		{
			// Token: 0x04001337 RID: 4919
			public static readonly MinMax fireRate = new MinMax(0.15f, 0.15f);

			// Token: 0x04001338 RID: 4920
			public static readonly float speed = 1000f;

			// Token: 0x04001339 RID: 4921
			public static readonly float damage = 2.85f;

			// Token: 0x0400133A RID: 4922
			public static readonly MinMax rotationSpeed = new MinMax(0f, 500f);

			// Token: 0x0400133B RID: 4923
			public static readonly float timeBeforeEaseRotationSpeed = 0f;

			// Token: 0x0400133C RID: 4924
			public static readonly float rotationSpeedEaseTime = 0.4f;

			// Token: 0x0400133D RID: 4925
			public static readonly float lockedShotAccelerationTime = 0.5f;

			// Token: 0x0400133E RID: 4926
			public static readonly float speedVariation = 100f;

			// Token: 0x0400133F RID: 4927
			public static readonly float angleVariation = 5f;

			// Token: 0x04001340 RID: 4928
			public static readonly int trailFrameDelay = 2;

			// Token: 0x04001341 RID: 4929
			public static readonly float maxHomingTime = 2.5f;
		}

		// Token: 0x02000333 RID: 819
		public static class Ex
		{
			// Token: 0x04001342 RID: 4930
			public static readonly float speed = 1500f;

			// Token: 0x04001343 RID: 4931
			public static readonly float damage = 7f;

			// Token: 0x04001344 RID: 4932
			public static readonly float spread = 90f;

			// Token: 0x04001345 RID: 4933
			public static readonly int bulletCount = 4;

			// Token: 0x04001346 RID: 4934
			public static readonly float swirlDistance = 100f;

			// Token: 0x04001347 RID: 4935
			public static readonly float swirlEaseTime = 0.75f;

			// Token: 0x04001348 RID: 4936
			public static readonly int trailFrameDelay = 2;
		}
	}

	// Token: 0x02000334 RID: 820
	public static class LevelWeaponPeashot
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0007B497 File Offset: 0x00079897
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_peashot);
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x0007B4A3 File Offset: 0x000798A3
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_peashot);
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0007B4AF File Offset: 0x000798AF
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_peashot);
			}
		}

		// Token: 0x04001349 RID: 4937
		public static readonly int value = 2;

		// Token: 0x0400134A RID: 4938
		public static readonly string iconPath = "Icons/equip_icon_weapon_peashot";

		// Token: 0x0400134B RID: 4939
		public static readonly Weapon id = Weapon.level_weapon_peashot;

		// Token: 0x02000335 RID: 821
		public static class Basic
		{
			// Token: 0x0400134C RID: 4940
			public static readonly float damage = 4f;

			// Token: 0x0400134D RID: 4941
			public static readonly float speed = 2250f;

			// Token: 0x0400134E RID: 4942
			public static readonly bool rapidFire = true;

			// Token: 0x0400134F RID: 4943
			public static readonly float rapidFireRate = 0.11f;
		}

		// Token: 0x02000336 RID: 822
		public static class Ex
		{
			// Token: 0x04001350 RID: 4944
			public static readonly float damage = 8.334f;

			// Token: 0x04001351 RID: 4945
			public static readonly float maxDamage = 25f;

			// Token: 0x04001352 RID: 4946
			public static readonly float damageDistance = 80f;

			// Token: 0x04001353 RID: 4947
			public static readonly float speed = 1500f;

			// Token: 0x04001354 RID: 4948
			public static readonly float freezeTime = 0.05f;
		}
	}

	// Token: 0x02000337 RID: 823
	public static class LevelWeaponPushback
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x0007B531 File Offset: 0x00079931
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_pushback);
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0007B53D File Offset: 0x0007993D
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_pushback);
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0007B549 File Offset: 0x00079949
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_pushback);
			}
		}

		// Token: 0x04001355 RID: 4949
		public static readonly int value = 4;

		// Token: 0x04001356 RID: 4950
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001357 RID: 4951
		public static readonly Weapon id = Weapon.level_weapon_pushback;

		// Token: 0x02000338 RID: 824
		public static class Basic
		{
			// Token: 0x04001358 RID: 4952
			public static readonly float damage = 4f;

			// Token: 0x04001359 RID: 4953
			public static readonly MinMax fireRate = new MinMax(0.1f, 0.7f);

			// Token: 0x0400135A RID: 4954
			public static readonly MinMax speed = new MinMax(700f, 1300f);

			// Token: 0x0400135B RID: 4955
			public static readonly float speedTime = 3f;

			// Token: 0x0400135C RID: 4956
			public static readonly float pushbackSpeed = 30f;
		}

		// Token: 0x02000339 RID: 825
		public static class Ex
		{
		}
	}

	// Token: 0x0200033A RID: 826
	public static class LevelWeaponSplitter
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x0007B5C7 File Offset: 0x000799C7
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_splitter);
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x0007B5D3 File Offset: 0x000799D3
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_splitter);
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0007B5DF File Offset: 0x000799DF
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_splitter);
			}
		}

		// Token: 0x0400135D RID: 4957
		public static readonly int value = 10;

		// Token: 0x0400135E RID: 4958
		public static readonly string iconPath = "Icons/";

		// Token: 0x0400135F RID: 4959
		public static readonly Weapon id = Weapon.level_weapon_splitter;

		// Token: 0x0200033B RID: 827
		public static class Basic
		{
			// Token: 0x04001360 RID: 4960
			public static readonly float fireRate = 0.22f;

			// Token: 0x04001361 RID: 4961
			public static readonly float speed = 1700f;

			// Token: 0x04001362 RID: 4962
			public static readonly float splitDistanceA = 200f;

			// Token: 0x04001363 RID: 4963
			public static readonly float splitDistanceB = 550f;

			// Token: 0x04001364 RID: 4964
			public static readonly float bulletDamage = 4f;

			// Token: 0x04001365 RID: 4965
			public static readonly float bulletDamageA = 2.15f;

			// Token: 0x04001366 RID: 4966
			public static readonly float bulletDamageB = 1.65f;

			// Token: 0x04001367 RID: 4967
			public static readonly float splitAngle = 20f;

			// Token: 0x04001368 RID: 4968
			public static readonly float angleDistance = 100f;
		}

		// Token: 0x0200033C RID: 828
		public static class Ex
		{
		}
	}

	// Token: 0x0200033D RID: 829
	public static class LevelWeaponSpreadshot
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x0007B66F File Offset: 0x00079A6F
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_spreadshot);
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x0007B67B File Offset: 0x00079A7B
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_spreadshot);
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0007B687 File Offset: 0x00079A87
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_spreadshot);
			}
		}

		// Token: 0x04001369 RID: 4969
		public static readonly int value = 4;

		// Token: 0x0400136A RID: 4970
		public static readonly string iconPath = "Icons/equip_icon_weapon_spread";

		// Token: 0x0400136B RID: 4971
		public static readonly Weapon id = Weapon.level_weapon_spreadshot;

		// Token: 0x0200033E RID: 830
		public static class Basic
		{
			// Token: 0x0400136C RID: 4972
			public static readonly float damage = 1.24f;

			// Token: 0x0400136D RID: 4973
			public static readonly float speed = 2250f;

			// Token: 0x0400136E RID: 4974
			public static readonly float distance = 375f;

			// Token: 0x0400136F RID: 4975
			public static readonly float rapidFireRate = 0.13f;
		}

		// Token: 0x0200033F RID: 831
		public static class Ex
		{
			// Token: 0x04001370 RID: 4976
			public static readonly float damage = 4.3f;

			// Token: 0x04001371 RID: 4977
			public static readonly float speed = 500f;

			// Token: 0x04001372 RID: 4978
			public static readonly int childCount = 8;

			// Token: 0x04001373 RID: 4979
			public static readonly float radius = 100f;
		}
	}

	// Token: 0x02000340 RID: 832
	public static class LevelWeaponUpshot
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x0007B6FF File Offset: 0x00079AFF
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_upshot);
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0007B70B File Offset: 0x00079B0B
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_upshot);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0007B717 File Offset: 0x00079B17
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_upshot);
			}
		}

		// Token: 0x04001374 RID: 4980
		public static readonly int value = 4;

		// Token: 0x04001375 RID: 4981
		public static readonly string iconPath = "Icons/equip_icon_weapon_upshot";

		// Token: 0x04001376 RID: 4982
		public static readonly Weapon id = Weapon.level_weapon_upshot;

		// Token: 0x02000341 RID: 833
		public static class Basic
		{
			// Token: 0x04001377 RID: 4983
			public static readonly float damage = 2.33f;

			// Token: 0x04001378 RID: 4984
			public static readonly float fireRate = 0.2f;

			// Token: 0x04001379 RID: 4985
			public static readonly float[] xSpeed = new float[]
			{
				630f,
				819f,
				945f
			};

			// Token: 0x0400137A RID: 4986
			public static readonly MinMax[] ySpeed = new MinMax[]
			{
				new MinMax(0f, 3240f),
				new MinMax(0f, 3240f),
				new MinMax(0f, 3240f)
			};

			// Token: 0x0400137B RID: 4987
			public static readonly float[] timeToMaxSpeed = new float[]
			{
				1.08f,
				0.81f,
				0.945f
			};
		}

		// Token: 0x02000342 RID: 834
		public static class Ex
		{
			// Token: 0x0400137C RID: 4988
			public static readonly float minRotationSpeed = 375f;

			// Token: 0x0400137D RID: 4989
			public static readonly float maxRotationSpeed = 185f;

			// Token: 0x0400137E RID: 4990
			public static readonly float rotationRampTime = 1.8f;

			// Token: 0x0400137F RID: 4991
			public static readonly float minRadiusSpeed = 195f;

			// Token: 0x04001380 RID: 4992
			public static readonly float maxRadiusSpeed = 365f;

			// Token: 0x04001381 RID: 4993
			public static readonly float radiusRampTime = 1.8f;

			// Token: 0x04001382 RID: 4994
			public static readonly float damage = 8f;

			// Token: 0x04001383 RID: 4995
			public static readonly float damageRate = 0.3f;

			// Token: 0x04001384 RID: 4996
			public static readonly float maxDamage = 37f;

			// Token: 0x04001385 RID: 4997
			public static readonly float freezeTime = 0.1f;
		}
	}

	// Token: 0x02000343 RID: 835
	public static class LevelWeaponWideShot
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0007B841 File Offset: 0x00079C41
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.level_weapon_wide_shot);
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0007B84D File Offset: 0x00079C4D
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.level_weapon_wide_shot);
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0007B859 File Offset: 0x00079C59
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.level_weapon_wide_shot);
			}
		}

		// Token: 0x04001386 RID: 4998
		public static readonly int value = 4;

		// Token: 0x04001387 RID: 4999
		public static readonly string iconPath = "Icons/equip_icon_weapon_wide_shot";

		// Token: 0x04001388 RID: 5000
		public static readonly Weapon id = Weapon.level_weapon_wide_shot;

		// Token: 0x02000344 RID: 836
		public static class Basic
		{
			// Token: 0x04001389 RID: 5001
			public static readonly float damage = 2.67f;

			// Token: 0x0400138A RID: 5002
			public static readonly float speed = 1800f;

			// Token: 0x0400138B RID: 5003
			public static readonly float distance = 2000f;

			// Token: 0x0400138C RID: 5004
			public static readonly float rapidFireRate = 0.22f;

			// Token: 0x0400138D RID: 5005
			public static readonly MinMax angleRange = new MinMax(50f, 8f);

			// Token: 0x0400138E RID: 5006
			public static readonly float closingAngleSpeed = 1.1f;

			// Token: 0x0400138F RID: 5007
			public static readonly float openingAngleSpeed = 1.8f;

			// Token: 0x04001390 RID: 5008
			public static readonly float projectileSpeed = 2f;
		}

		// Token: 0x02000345 RID: 837
		public static class Ex
		{
			// Token: 0x04001391 RID: 5009
			public static readonly float exDamage = 21f;

			// Token: 0x04001392 RID: 5010
			public static readonly float exDuration = 0.3f;

			// Token: 0x04001393 RID: 5011
			public static readonly float exHeight = 86.5f;
		}
	}

	// Token: 0x02000346 RID: 838
	public static class PlaneSuperBomb
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0007B90B File Offset: 0x00079D0B
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.plane_super_bomb);
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0007B917 File Offset: 0x00079D17
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.plane_super_bomb);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0007B923 File Offset: 0x00079D23
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.plane_super_bomb);
			}
		}

		// Token: 0x04001394 RID: 5012
		public static readonly int value = 10;

		// Token: 0x04001395 RID: 5013
		public static readonly string iconPath = "Icons/";

		// Token: 0x04001396 RID: 5014
		public static readonly Super id = Super.plane_super_bomb;

		// Token: 0x04001397 RID: 5015
		public static readonly float damage = 38f;

		// Token: 0x04001398 RID: 5016
		public static readonly float damageRate = 0.25f;

		// Token: 0x04001399 RID: 5017
		public static readonly float countdownTime = 3f;
	}

	// Token: 0x02000347 RID: 839
	public static class PlaneSuperChaliceSuperBomb
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0007B96A File Offset: 0x00079D6A
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Super.plane_super_chalice_bomb);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0007B976 File Offset: 0x00079D76
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Super.plane_super_chalice_bomb);
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0007B982 File Offset: 0x00079D82
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Super.plane_super_chalice_bomb);
			}
		}

		// Token: 0x0400139A RID: 5018
		public static readonly int value = 10;

		// Token: 0x0400139B RID: 5019
		public static readonly string iconPath = "Icons/";

		// Token: 0x0400139C RID: 5020
		public static readonly Super id = Super.plane_super_chalice_bomb;

		// Token: 0x0400139D RID: 5021
		public static readonly float damage = 25.5f;

		// Token: 0x0400139E RID: 5022
		public static readonly float damageRate = 0.25f;

		// Token: 0x0400139F RID: 5023
		public static readonly float turnRate = 1f;

		// Token: 0x040013A0 RID: 5024
		public static readonly float maxAngle = 60f;

		// Token: 0x040013A1 RID: 5025
		public static readonly float angleDamp = 0.98f;

		// Token: 0x040013A2 RID: 5026
		public static readonly float accel = 600f;
	}

	// Token: 0x02000348 RID: 840
	public static class PlaneWeaponBomb
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0007B9F4 File Offset: 0x00079DF4
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_weapon_bomb);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0007BA00 File Offset: 0x00079E00
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_weapon_bomb);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x0007BA0C File Offset: 0x00079E0C
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_weapon_bomb);
			}
		}

		// Token: 0x040013A3 RID: 5027
		public static readonly int value = 2;

		// Token: 0x040013A4 RID: 5028
		public static readonly string iconPath = "Icons/";

		// Token: 0x040013A5 RID: 5029
		public static readonly Weapon id = Weapon.plane_weapon_bomb;

		// Token: 0x02000349 RID: 841
		public static class Basic
		{
			// Token: 0x040013A6 RID: 5030
			public static readonly float damage = 11.5f;

			// Token: 0x040013A7 RID: 5031
			public static readonly float speed = 1200f;

			// Token: 0x040013A8 RID: 5032
			public static readonly bool Up;

			// Token: 0x040013A9 RID: 5033
			public static readonly float sizeExplosion = 1f;

			// Token: 0x040013AA RID: 5034
			public static readonly float size = 1f;

			// Token: 0x040013AB RID: 5035
			public static readonly float angle = 45f;

			// Token: 0x040013AC RID: 5036
			public static readonly float gravity = 4500f;

			// Token: 0x040013AD RID: 5037
			public static readonly bool rapidFire = true;

			// Token: 0x040013AE RID: 5038
			public static readonly float rapidFireRate = 0.6f;
		}

		// Token: 0x0200034A RID: 842
		public static class Ex
		{
			// Token: 0x040013AF RID: 5039
			public static readonly float damage = 6f;

			// Token: 0x040013B0 RID: 5040
			public static readonly float speed = 700f;

			// Token: 0x040013B1 RID: 5041
			public static readonly float[] angles = new float[]
			{
				180f,
				170f
			};

			// Token: 0x040013B2 RID: 5042
			public static readonly int[] counts = new int[]
			{
				6,
				3
			};

			// Token: 0x040013B3 RID: 5043
			public static readonly MinMax rotationSpeed = new MinMax(0f, 250f);

			// Token: 0x040013B4 RID: 5044
			public static readonly float timeBeforeEaseRotationSpeed = 0f;

			// Token: 0x040013B5 RID: 5045
			public static readonly float rotationSpeedEaseTime = 1f;

			// Token: 0x040013B6 RID: 5046
			public static readonly float maxHomingTime = 2.5f;
		}
	}

	// Token: 0x0200034B RID: 843
	public static class PlaneWeaponChaliceBomb
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0007BB11 File Offset: 0x00079F11
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_chalice_weapon_bomb);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0007BB1D File Offset: 0x00079F1D
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_chalice_weapon_bomb);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0007BB29 File Offset: 0x00079F29
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_chalice_weapon_bomb);
			}
		}

		// Token: 0x040013B7 RID: 5047
		public static readonly int value = 10;

		// Token: 0x040013B8 RID: 5048
		public static readonly string iconPath = "Icons/equip_icon_chalice_shmup_bomb";

		// Token: 0x040013B9 RID: 5049
		public static readonly Weapon id = Weapon.plane_chalice_weapon_bomb;

		// Token: 0x0200034C RID: 844
		public static class Basic
		{
			// Token: 0x040013BA RID: 5050
			public static readonly float damage = 6.6f;

			// Token: 0x040013BB RID: 5051
			public static readonly float size = 1f;

			// Token: 0x040013BC RID: 5052
			public static readonly float sizeExplosion = 1f;

			// Token: 0x040013BD RID: 5053
			public static readonly float angleRange = 35f;

			// Token: 0x040013BE RID: 5054
			public static readonly float gravity = 1700f;

			// Token: 0x040013BF RID: 5055
			public static readonly float speed = 700f;

			// Token: 0x040013C0 RID: 5056
			public static readonly bool rapidFire = true;

			// Token: 0x040013C1 RID: 5057
			public static readonly float rapidFireRate = 0.2f;

			// Token: 0x040013C2 RID: 5058
			public static readonly float damageExplosion = 2.5f;
		}

		// Token: 0x0200034D RID: 845
		public static class Ex
		{
			// Token: 0x040013C3 RID: 5059
			public static readonly float damage = 15.5f;

			// Token: 0x040013C4 RID: 5060
			public static readonly float damageRate = 0.17f;

			// Token: 0x040013C5 RID: 5061
			public static readonly float damageRateIncrease = 0.07f;

			// Token: 0x040013C6 RID: 5062
			public static readonly float startSpeed = 600f;

			// Token: 0x040013C7 RID: 5063
			public static readonly float gravity = 1900f;

			// Token: 0x040013C8 RID: 5064
			public static readonly float freezeTime = 0.125f;
		}
	}

	// Token: 0x0200034E RID: 846
	public static class PlaneWeaponChaliceWay
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0007BBF5 File Offset: 0x00079FF5
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_chalice_weapon_3way);
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0007BC01 File Offset: 0x0007A001
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_chalice_weapon_3way);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0007BC0D File Offset: 0x0007A00D
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_chalice_weapon_3way);
			}
		}

		// Token: 0x040013C9 RID: 5065
		public static readonly int value = 10;

		// Token: 0x040013CA RID: 5066
		public static readonly string iconPath = "Icons/equip_icon_chalice_shmup_3way";

		// Token: 0x040013CB RID: 5067
		public static readonly Weapon id = Weapon.plane_chalice_weapon_3way;

		// Token: 0x0200034F RID: 847
		public static class Basic
		{
			// Token: 0x040013CC RID: 5068
			public static readonly float damage = 3.65f;

			// Token: 0x040013CD RID: 5069
			public static readonly float speed = 1650f;

			// Token: 0x040013CE RID: 5070
			public static readonly float distance;

			// Token: 0x040013CF RID: 5071
			public static readonly float rapidFireRate = 0.23f;

			// Token: 0x040013D0 RID: 5072
			public static readonly float angle = 9f;
		}

		// Token: 0x02000350 RID: 848
		public static class Ex
		{
			// Token: 0x040013D1 RID: 5073
			public static readonly float damageBeforeLaunch = 2.4f;

			// Token: 0x040013D2 RID: 5074
			public static readonly float damageRateBeforeLaunch = 0.25f;

			// Token: 0x040013D3 RID: 5075
			public static readonly float arcSpeed = 5f;

			// Token: 0x040013D4 RID: 5076
			public static readonly float arcX = 250f;

			// Token: 0x040013D5 RID: 5077
			public static readonly float arcY = 40f;

			// Token: 0x040013D6 RID: 5078
			public static readonly float pauseTime;

			// Token: 0x040013D7 RID: 5079
			public static readonly float damageAfterLaunch = 17f;

			// Token: 0x040013D8 RID: 5080
			public static readonly float speedAfterLaunch = -1250f;

			// Token: 0x040013D9 RID: 5081
			public static readonly float accelAfterLaunch = 8000f;

			// Token: 0x040013DA RID: 5082
			public static readonly float freezeTime = 0.125f;

			// Token: 0x040013DB RID: 5083
			public static readonly float minXDistance = 75f;

			// Token: 0x040013DC RID: 5084
			public static readonly int xDistanceNoTarget = 500;
		}
	}

	// Token: 0x02000351 RID: 849
	public static class PlaneWeaponLaser
	{
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0007BCDB File Offset: 0x0007A0DB
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_weapon_laser);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0007BCE7 File Offset: 0x0007A0E7
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_weapon_laser);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0007BCF3 File Offset: 0x0007A0F3
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_weapon_laser);
			}
		}

		// Token: 0x040013DD RID: 5085
		public static readonly int value = 2;

		// Token: 0x040013DE RID: 5086
		public static readonly string iconPath = "Icons/";

		// Token: 0x040013DF RID: 5087
		public static readonly Weapon id = Weapon.plane_weapon_laser;

		// Token: 0x02000352 RID: 850
		public static class Basic
		{
			// Token: 0x040013E0 RID: 5088
			public static readonly float damage = 8f;

			// Token: 0x040013E1 RID: 5089
			public static readonly float speed = 4000f;

			// Token: 0x040013E2 RID: 5090
			public static readonly bool rapidFire = true;

			// Token: 0x040013E3 RID: 5091
			public static readonly float rapidFireRate = 0.1f;
		}

		// Token: 0x02000353 RID: 851
		public static class Ex
		{
			// Token: 0x040013E4 RID: 5092
			public static readonly float damage = 3f;

			// Token: 0x040013E5 RID: 5093
			public static readonly float speed = 2000f;

			// Token: 0x040013E6 RID: 5094
			public static readonly float[] angles = new float[]
			{
				180f,
				170f
			};

			// Token: 0x040013E7 RID: 5095
			public static readonly int[] counts = new int[]
			{
				12,
				6
			};
		}
	}

	// Token: 0x02000354 RID: 852
	public static class PlaneWeaponPeashot
	{
		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x0007BD94 File Offset: 0x0007A194
		public static string displayName
		{
			get
			{
				return WeaponProperties.GetDisplayName(Weapon.plane_weapon_peashot);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0007BDA0 File Offset: 0x0007A1A0
		public static string subtext
		{
			get
			{
				return WeaponProperties.GetSubtext(Weapon.plane_weapon_peashot);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0007BDAC File Offset: 0x0007A1AC
		public static string description
		{
			get
			{
				return WeaponProperties.GetDescription(Weapon.plane_weapon_peashot);
			}
		}

		// Token: 0x040013E8 RID: 5096
		public static readonly int value = 2;

		// Token: 0x040013E9 RID: 5097
		public static readonly string iconPath = "Icons/equip_icon_weapon_peashot";

		// Token: 0x040013EA RID: 5098
		public static readonly Weapon id = Weapon.plane_weapon_peashot;

		// Token: 0x02000355 RID: 853
		public static class Basic
		{
			// Token: 0x040013EB RID: 5099
			public static readonly float damage = 4f;

			// Token: 0x040013EC RID: 5100
			public static readonly float speed = 1800f;

			// Token: 0x040013ED RID: 5101
			public static readonly bool rapidFire = true;

			// Token: 0x040013EE RID: 5102
			public static readonly float rapidFireRate = 0.07f;
		}

		// Token: 0x02000356 RID: 854
		public static class Ex
		{
			// Token: 0x040013EF RID: 5103
			public static readonly float damage = 15f;

			// Token: 0x040013F0 RID: 5104
			public static readonly float damageDistance = 100f;

			// Token: 0x040013F1 RID: 5105
			public static readonly float acceleration = 2500f;

			// Token: 0x040013F2 RID: 5106
			public static readonly float maxSpeed = 1500f;

			// Token: 0x040013F3 RID: 5107
			public static readonly float freezeTime = 0.125f;
		}
	}
}
