using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000AEC RID: 2796
public class DamageDealer
{
	// Token: 0x060043BA RID: 17338 RVA: 0x00240253 File Offset: 0x0023E653
	public DamageDealer(float damage, float damageRate)
	{
		this.Setup(damage, damageRate);
	}

	// Token: 0x060043BB RID: 17339 RVA: 0x00240290 File Offset: 0x0023E690
	public DamageDealer(float damage, float damageRate, bool damagesPlayer, bool damagesEnemy, bool damagesOther)
	{
		this.Setup(damage, damageRate, DamageDealer.DamageSource.Neutral, damagesPlayer, damagesEnemy, damagesOther, 1f);
	}

	// Token: 0x060043BC RID: 17340 RVA: 0x002402E4 File Offset: 0x0023E6E4
	public DamageDealer(float damage, float damageRate, DamageDealer.DamageSource damageSource, bool damagesPlayer, bool damagesEnemy, bool damagesOther)
	{
		this.Setup(damage, damageRate, damageSource, damagesPlayer, damagesEnemy, damagesOther, 1f);
	}

	// Token: 0x060043BD RID: 17341 RVA: 0x00240338 File Offset: 0x0023E738
	public DamageDealer(AbstractProjectile projectile)
	{
		this.Setup(projectile.Damage, projectile.DamageRate, projectile.DamageSource, projectile.GetDamagesType(DamageReceiver.Type.Player), projectile.GetDamagesType(DamageReceiver.Type.Enemy), projectile.GetDamagesType(DamageReceiver.Type.Other), projectile.DamageMultiplier);
		this.SetDirection(DamageDealer.Direction.Neutral, projectile.transform);
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x060043BE RID: 17342 RVA: 0x002403B7 File Offset: 0x0023E7B7
	// (set) Token: 0x060043BF RID: 17343 RVA: 0x002403BF File Offset: 0x0023E7BF
	public float DamageDealt { get; private set; }

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x060043C0 RID: 17344 RVA: 0x002403C8 File Offset: 0x0023E7C8
	// (set) Token: 0x060043C1 RID: 17345 RVA: 0x002403D0 File Offset: 0x0023E7D0
	public float DamageMultiplier
	{
		get
		{
			return this.damageMultiplier;
		}
		set
		{
			this.damageMultiplier = value;
		}
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x060043C2 RID: 17346 RVA: 0x002403D9 File Offset: 0x0023E7D9
	// (set) Token: 0x060043C3 RID: 17347 RVA: 0x002403E1 File Offset: 0x0023E7E1
	public PlayerId PlayerId
	{
		get
		{
			return this.playerId;
		}
		set
		{
			this.playerId = value;
		}
	}

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x060043C4 RID: 17348 RVA: 0x002403EA File Offset: 0x0023E7EA
	// (set) Token: 0x060043C5 RID: 17349 RVA: 0x002403F2 File Offset: 0x0023E7F2
	public bool isDLCWeapon { get; set; }

	// Token: 0x140000BE RID: 190
	// (add) Token: 0x060043C6 RID: 17350 RVA: 0x002403FC File Offset: 0x0023E7FC
	// (remove) Token: 0x060043C7 RID: 17351 RVA: 0x00240434 File Offset: 0x0023E834
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DamageDealer.OnDealDamageHandler OnDealDamage;

	// Token: 0x060043C8 RID: 17352 RVA: 0x0024046A File Offset: 0x0023E86A
	public static DamageDealer NewEnemy()
	{
		return DamageDealer.NewEnemy(0.2f);
	}

	// Token: 0x060043C9 RID: 17353 RVA: 0x00240476 File Offset: 0x0023E876
	public static DamageDealer NewEnemy(float rate)
	{
		return new DamageDealer(1f, rate, DamageDealer.DamageSource.Enemy, true, false, false);
	}

	// Token: 0x060043CA RID: 17354 RVA: 0x00240487 File Offset: 0x0023E887
	private void Setup(float damage, float damageRate)
	{
		this.Setup(damage, damageRate, DamageDealer.DamageSource.Neutral, true, false, false, 1f);
	}

	// Token: 0x060043CB RID: 17355 RVA: 0x0024049A File Offset: 0x0023E89A
	private void Setup(float damage, float damageRate, DamageDealer.DamageSource damageSource)
	{
		this.Setup(damage, damageRate, damageSource, true, false, false, 1f);
	}

	// Token: 0x060043CC RID: 17356 RVA: 0x002404B0 File Offset: 0x0023E8B0
	private void Setup(float damage, float damageRate, DamageDealer.DamageSource damageSource, bool damagesPlayer, bool damagesEnemy, bool damagesOther, float damageMultiplier = 1f)
	{
		this.damage = damage;
		this.damageRate = damageRate;
		this.damageMultiplier = damageMultiplier;
		this.damageTypes = new DamageDealer.DamageTypesManager();
		this.SetDamageFlags(damagesPlayer, damagesEnemy, damagesOther);
		this.SetDamageSource(damageSource);
		this.timers = new Dictionary<int, float>();
		this.timersList = new List<int>();
		this.StoneTime = -1f;
	}

	// Token: 0x060043CD RID: 17357 RVA: 0x00240512 File Offset: 0x0023E912
	public void SetDamage(float damage)
	{
		this.damage = damage;
	}

	// Token: 0x060043CE RID: 17358 RVA: 0x0024051B File Offset: 0x0023E91B
	public void SetRate(float rate)
	{
		this.damageRate = rate;
	}

	// Token: 0x060043CF RID: 17359 RVA: 0x00240524 File Offset: 0x0023E924
	public void SetDamageSource(DamageDealer.DamageSource source)
	{
		this.damageSource = source;
	}

	// Token: 0x060043D0 RID: 17360 RVA: 0x0024052D File Offset: 0x0023E92D
	public void SetDamageFlags(bool damagesPlayer, bool damagesEnemy, bool damagesOther)
	{
		this.damageTypes.Player = damagesPlayer;
		this.damageTypes.Enemies = damagesEnemy;
		this.damageTypes.Other = damagesOther;
	}

	// Token: 0x060043D1 RID: 17361 RVA: 0x00240553 File Offset: 0x0023E953
	public void SetDirection(DamageDealer.Direction direction, Transform origin)
	{
		this.direction = direction;
		this.origin = origin;
	}

	// Token: 0x060043D2 RID: 17362 RVA: 0x00240564 File Offset: 0x0023E964
	public float DealDamage(GameObject hit)
	{
		DamageReceiver damageReceiver = hit.GetComponent<DamageReceiver>();
		if (damageReceiver == null)
		{
			DamageReceiverChild component = hit.GetComponent<DamageReceiverChild>();
			if (component != null && component.enabled)
			{
				damageReceiver = component.Receiver;
			}
		}
		if (!(damageReceiver != null) || !damageReceiver.enabled)
		{
			return 0f;
		}
		int instanceID = damageReceiver.GetInstanceID();
		if (!this.damageTypes.GetType(damageReceiver.type))
		{
			return 0f;
		}
		if (!this.timers.ContainsKey(instanceID))
		{
			this.timers.Add(instanceID, this.damageRate);
			this.timersList.Add(instanceID);
		}
		else if (this.damageRate == 0f)
		{
			return 0f;
		}
		if (this.timers[instanceID] < this.damageRate)
		{
			return 0f;
		}
		Vector2 vector = (!(this.origin != null)) ? Vector2.zero : this.origin.position;
		DamageDealer.DamageInfo damageInfo = new DamageDealer.DamageInfo(this.damage * this.damageMultiplier, this.direction, vector, this.damageSource);
		damageInfo.SetStoneTime(this.StoneTime);
		damageReceiver.TakeDamage(damageInfo);
		this.DamageDealt += this.damage * this.damageMultiplier;
		this.timers[damageReceiver.GetInstanceID()] = 0f;
		if (this.OnDealDamage != null)
		{
			this.OnDealDamage(this.damage * this.damageMultiplier, damageReceiver, this);
		}
		if (this.playerId != PlayerId.None && damageReceiver.type == DamageReceiver.Type.Enemy)
		{
			DamageDealer.lastPlayer = this.playerId;
			DamageDealer.lastPlayerDamageSource = this.damageSource;
			if (this.damageSource != DamageDealer.DamageSource.SmallPlane)
			{
				DamageDealer.didDamageWithNonSmallPlaneWeapon = true;
			}
			DamageDealer.lastDamageWasDLCWeapon = this.isDLCWeapon;
		}
		return this.damage;
	}

	// Token: 0x060043D3 RID: 17363 RVA: 0x0024075C File Offset: 0x0023EB5C
	public void Update()
	{
		foreach (int num in this.timersList)
		{
			Dictionary<int, float> dictionary;
			int key;
			(dictionary = this.timers)[key = num] = dictionary[key] + CupheadTime.Delta;
		}
	}

	// Token: 0x060043D4 RID: 17364 RVA: 0x002407D4 File Offset: 0x0023EBD4
	public void FixedUpdate()
	{
		foreach (int num in this.timersList)
		{
			Dictionary<int, float> dictionary;
			int key;
			(dictionary = this.timers)[key = num] = dictionary[key] + CupheadTime.FixedDelta;
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x060043D5 RID: 17365 RVA: 0x00240848 File Offset: 0x0023EC48
	// (set) Token: 0x060043D6 RID: 17366 RVA: 0x00240850 File Offset: 0x0023EC50
	public float StoneTime { get; private set; }

	// Token: 0x060043D7 RID: 17367 RVA: 0x00240859 File Offset: 0x0023EC59
	public void SetStoneTime(float stoneTime)
	{
		this.StoneTime = stoneTime;
	}

	// Token: 0x0400496D RID: 18797
	public static DamageDealer.DamageSource lastPlayerDamageSource;

	// Token: 0x0400496E RID: 18798
	public static PlayerId lastPlayer;

	// Token: 0x0400496F RID: 18799
	public static bool lastDamageWasDLCWeapon;

	// Token: 0x04004970 RID: 18800
	public static bool didDamageWithNonSmallPlaneWeapon;

	// Token: 0x04004971 RID: 18801
	private Dictionary<int, float> timers;

	// Token: 0x04004972 RID: 18802
	private List<int> timersList;

	// Token: 0x04004973 RID: 18803
	private float damage = 1f;

	// Token: 0x04004974 RID: 18804
	private float damageRate = 1f;

	// Token: 0x04004975 RID: 18805
	private float damageMultiplier = 1f;

	// Token: 0x04004976 RID: 18806
	private DamageDealer.Direction direction;

	// Token: 0x04004977 RID: 18807
	private Transform origin;

	// Token: 0x04004978 RID: 18808
	private DamageDealer.DamageSource damageSource;

	// Token: 0x04004979 RID: 18809
	private DamageDealer.DamageTypesManager damageTypes;

	// Token: 0x0400497A RID: 18810
	private PlayerId playerId = PlayerId.None;

	// Token: 0x02000AED RID: 2797
	public enum Direction
	{
		// Token: 0x0400497F RID: 18815
		Neutral,
		// Token: 0x04004980 RID: 18816
		Left,
		// Token: 0x04004981 RID: 18817
		Right
	}

	// Token: 0x02000AEE RID: 2798
	public enum DamageSource
	{
		// Token: 0x04004983 RID: 18819
		Neutral,
		// Token: 0x04004984 RID: 18820
		Enemy,
		// Token: 0x04004985 RID: 18821
		Ex,
		// Token: 0x04004986 RID: 18822
		SmallPlane,
		// Token: 0x04004987 RID: 18823
		Super,
		// Token: 0x04004988 RID: 18824
		Pit
	}

	// Token: 0x02000AEF RID: 2799
	// (Invoke) Token: 0x060043D9 RID: 17369
	public delegate void OnDealDamageHandler(float damage, DamageReceiver receiver, DamageDealer dealer);

	// Token: 0x02000AF0 RID: 2800
	public class DamageInfo
	{
		// Token: 0x060043DC RID: 17372 RVA: 0x00240862 File Offset: 0x0023EC62
		public DamageInfo(float damage, DamageDealer.Direction direction, Vector2 origin, DamageDealer.DamageSource source)
		{
			this.direction = direction;
			this.origin = origin;
			this.damageSource = source;
			this.damage = damage;
			this.stoneTime = -1f;
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060043DD RID: 17373 RVA: 0x00240892 File Offset: 0x0023EC92
		// (set) Token: 0x060043DE RID: 17374 RVA: 0x0024089A File Offset: 0x0023EC9A
		public float damage { get; private set; }

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060043DF RID: 17375 RVA: 0x002408A3 File Offset: 0x0023ECA3
		// (set) Token: 0x060043E0 RID: 17376 RVA: 0x002408AB File Offset: 0x0023ECAB
		public DamageDealer.Direction direction { get; private set; }

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060043E1 RID: 17377 RVA: 0x002408B4 File Offset: 0x0023ECB4
		// (set) Token: 0x060043E2 RID: 17378 RVA: 0x002408BC File Offset: 0x0023ECBC
		public Vector2 origin { get; private set; }

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060043E3 RID: 17379 RVA: 0x002408C5 File Offset: 0x0023ECC5
		// (set) Token: 0x060043E4 RID: 17380 RVA: 0x002408CD File Offset: 0x0023ECCD
		public DamageDealer.DamageSource damageSource { get; private set; }

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060043E5 RID: 17381 RVA: 0x002408D6 File Offset: 0x0023ECD6
		// (set) Token: 0x060043E6 RID: 17382 RVA: 0x002408DE File Offset: 0x0023ECDE
		public float stoneTime { get; private set; }

		// Token: 0x060043E7 RID: 17383 RVA: 0x002408E7 File Offset: 0x0023ECE7
		public void SetStoneTime(float stoneTime)
		{
			this.stoneTime = stoneTime;
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x002408F0 File Offset: 0x0023ECF0
		public void SetEditorPlayer()
		{
			this.damage *= 10f;
		}
	}

	// Token: 0x02000AF1 RID: 2801
	[Serializable]
	public class DamageTypesManager
	{
		// Token: 0x060043EA RID: 17386 RVA: 0x0024090C File Offset: 0x0023ED0C
		public DamageDealer.DamageTypesManager Copy()
		{
			return base.MemberwiseClone() as DamageDealer.DamageTypesManager;
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x00240919 File Offset: 0x0023ED19
		public void SetAll(bool b)
		{
			this.Player = b;
			this.Enemies = b;
			this.Other = b;
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x00240930 File Offset: 0x0023ED30
		public DamageDealer.DamageTypesManager OnlyPlayer()
		{
			this.SetAll(false);
			this.Player = true;
			return this;
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x00240941 File Offset: 0x0023ED41
		public DamageDealer.DamageTypesManager OnlyEnemies()
		{
			this.SetAll(false);
			this.Enemies = true;
			return this;
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x00240952 File Offset: 0x0023ED52
		public DamageDealer.DamageTypesManager PlayerProjectileDefault()
		{
			this.SetAll(false);
			this.Enemies = true;
			this.Other = true;
			return this;
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0024096A File Offset: 0x0023ED6A
		public bool GetType(DamageReceiver.Type type)
		{
			switch (type)
			{
			case DamageReceiver.Type.Enemy:
				return this.Enemies;
			case DamageReceiver.Type.Player:
				return this.Player;
			case DamageReceiver.Type.Other:
				return this.Other;
			default:
				return false;
			}
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0024099C File Offset: 0x0023ED9C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Player:",
				this.Player,
				", Enemies:",
				this.Enemies,
				", Other:",
				this.Other
			});
		}

		// Token: 0x0400498E RID: 18830
		public bool Player;

		// Token: 0x0400498F RID: 18831
		public bool Enemies;

		// Token: 0x04004990 RID: 18832
		public bool Other;
	}
}
