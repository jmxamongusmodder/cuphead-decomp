using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002ED RID: 749
public class VeggiesLevel : Level
{
	// Token: 0x0600084E RID: 2126 RVA: 0x00078BE4 File Offset: 0x00076FE4
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Veggies.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x0600084F RID: 2127 RVA: 0x00078C7A File Offset: 0x0007707A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Veggies;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000850 RID: 2128 RVA: 0x00078C7D File Offset: 0x0007707D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_veggies;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000851 RID: 2129 RVA: 0x00078C84 File Offset: 0x00077084
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.currentBoss)
			{
			case VeggiesLevel.CurrentBoss.Potato:
				return this._bossPortraitPotato;
			case VeggiesLevel.CurrentBoss.Onion:
				return this._bossPortraitOnion;
			case VeggiesLevel.CurrentBoss.Carrot:
				return this._bossPortraitCarrot;
			}
			return this._bossPortraitPotato;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000852 RID: 2130 RVA: 0x00078CD4 File Offset: 0x000770D4
	public override string BossQuote
	{
		get
		{
			switch (this.currentBoss)
			{
			case VeggiesLevel.CurrentBoss.Potato:
				return this._bossQuotePotato;
			case VeggiesLevel.CurrentBoss.Onion:
				return this._bossQuoteOnion;
			case VeggiesLevel.CurrentBoss.Carrot:
				return this._bossQuoteCarrot;
			}
			return "QuoteNone";
		}
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00078D24 File Offset: 0x00077124
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.potatoStart_cr());
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		base.timeline.health += (float)this.properties.CurrentState.potato.hp;
		if (base.mode != Level.Mode.Easy)
		{
			base.timeline.health += (float)this.properties.CurrentState.onion.hp;
		}
		base.timeline.health += (float)this.properties.CurrentState.carrot.hp;
		if (base.mode != Level.Mode.Easy)
		{
			base.timeline.AddEventAtHealth("Onion", base.timeline.GetHealthOfLastEvent() + this.properties.CurrentState.potato.hp);
		}
		base.timeline.AddEventAtHealth("Carrot", base.timeline.GetHealthOfLastEvent() + ((base.mode != Level.Mode.Easy) ? this.properties.CurrentState.onion.hp : this.properties.CurrentState.potato.hp));
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x00078E90 File Offset: 0x00077290
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.prefabs = null;
		this._bossPortraitCarrot = null;
		this._bossPortraitOnion = null;
		this._bossPortraitPotato = null;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00078EB4 File Offset: 0x000772B4
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.veggiesPattern_cr());
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00078EC4 File Offset: 0x000772C4
	private IEnumerator veggiesPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return base.StartCoroutine(this.potato_cr());
		if (base.mode != Level.Mode.Easy)
		{
			yield return base.StartCoroutine(this.onion_cr());
		}
		yield return base.StartCoroutine(this.carrot_cr());
		yield return base.StartCoroutine(this.win_cr());
		yield break;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00078EE0 File Offset: 0x000772E0
	private IEnumerator potatoStart_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.potato = this.prefabs.potato.InstantiatePrefab<VeggiesLevelPotato>();
		this.potato.OnDamageTakenEvent += base.timeline.DealDamage;
		yield break;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00078EFC File Offset: 0x000772FC
	private IEnumerator potato_cr()
	{
		this.currentBoss = VeggiesLevel.CurrentBoss.Potato;
		this.potato.LevelInitWithGroup(this.properties.CurrentState.potato);
		while (this.potato.state != VeggiesLevelPotato.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00078F18 File Offset: 0x00077318
	private IEnumerator onion_cr()
	{
		this.currentBoss = VeggiesLevel.CurrentBoss.Onion;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		VeggiesLevelOnion v = this.prefabs.onion.InstantiatePrefab<VeggiesLevelOnion>();
		v.LevelInitWithGroup(this.properties.CurrentState.onion);
		v.OnDamageTakenEvent += base.timeline.DealDamage;
		v.OnHappyLeave += this.OnionHappyLeave;
		while (v.state != VeggiesLevelOnion.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00078F33 File Offset: 0x00077333
	private void OnionHappyLeave()
	{
		this.secretTriggered = true;
		base.timeline.DealDamage((float)this.properties.CurrentState.onion.hp);
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00078F60 File Offset: 0x00077360
	private IEnumerator beet_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		VeggiesLevelBeet v = this.prefabs.beet.InstantiatePrefab<VeggiesLevelBeet>();
		v.LevelInitWithGroup(this.properties.CurrentState.beet);
		v.OnDamageTakenEvent += base.timeline.DealDamage;
		while (v.state != VeggiesLevelBeet.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00078F7C File Offset: 0x0007737C
	private IEnumerator peas_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		VeggiesLevelPeas v = this.prefabs.peas.InstantiatePrefab<VeggiesLevelPeas>();
		v.LevelInitWithGroup(this.properties.CurrentState.peas);
		v.OnDamageTakenEvent += base.timeline.DealDamage;
		while (v.state != VeggiesLevelPeas.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00078F98 File Offset: 0x00077398
	private IEnumerator carrot_cr()
	{
		this.currentBoss = VeggiesLevel.CurrentBoss.Carrot;
		VeggiesLevelCarrot v = this.prefabs.carrot.InstantiatePrefab<VeggiesLevelCarrot>();
		v.LevelInit(this.properties);
		v.OnDamageTakenEvent += base.timeline.DealDamage;
		while (v.state != VeggiesLevelCarrot.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00078FB4 File Offset: 0x000773B4
	private IEnumerator win_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.properties.WinInstantly();
		yield break;
	}

	// Token: 0x040010E2 RID: 4322
	private LevelProperties.Veggies properties;

	// Token: 0x040010E3 RID: 4323
	[Space(10f)]
	[SerializeField]
	private VeggiesLevel.Prefabs prefabs;

	// Token: 0x040010E4 RID: 4324
	private VeggiesLevelPotato potato;

	// Token: 0x040010E5 RID: 4325
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitPotato;

	// Token: 0x040010E6 RID: 4326
	[SerializeField]
	private Sprite _bossPortraitOnion;

	// Token: 0x040010E7 RID: 4327
	[SerializeField]
	private Sprite _bossPortraitCarrot;

	// Token: 0x040010E8 RID: 4328
	[SerializeField]
	private string _bossQuotePotato;

	// Token: 0x040010E9 RID: 4329
	[SerializeField]
	private string _bossQuoteOnion;

	// Token: 0x040010EA RID: 4330
	[SerializeField]
	private string _bossQuoteCarrot;

	// Token: 0x040010EB RID: 4331
	private VeggiesLevel.CurrentBoss currentBoss;

	// Token: 0x02000838 RID: 2104
	private enum CurrentBoss
	{
		// Token: 0x04003966 RID: 14694
		Potato,
		// Token: 0x04003967 RID: 14695
		Onion,
		// Token: 0x04003968 RID: 14696
		Beet,
		// Token: 0x04003969 RID: 14697
		Peas,
		// Token: 0x0400396A RID: 14698
		Carrot
	}

	// Token: 0x02000839 RID: 2105
	[Serializable]
	public class Prefabs
	{
		// Token: 0x0400396B RID: 14699
		public VeggiesLevelPotato potato;

		// Token: 0x0400396C RID: 14700
		public VeggiesLevelOnion onion;

		// Token: 0x0400396D RID: 14701
		public VeggiesLevelBeet beet;

		// Token: 0x0400396E RID: 14702
		public VeggiesLevelPeas peas;

		// Token: 0x0400396F RID: 14703
		public VeggiesLevelCarrot carrot;
	}
}
