using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class DicePalaceChipsLevel : AbstractDicePalaceLevel
{
	// Token: 0x060002FB RID: 763 RVA: 0x0005EEAC File Offset: 0x0005D2AC
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceChips.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060002FC RID: 764 RVA: 0x0005EF42 File Offset: 0x0005D342
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceChips;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060002FD RID: 765 RVA: 0x0005EF49 File Offset: 0x0005D349
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceChips;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060002FE RID: 766 RVA: 0x0005EF50 File Offset: 0x0005D350
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_chips;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060002FF RID: 767 RVA: 0x0005EF54 File Offset: 0x0005D354
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000300 RID: 768 RVA: 0x0005EF5C File Offset: 0x0005D35C
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0005EF64 File Offset: 0x0005D364
	protected override void Start()
	{
		base.Start();
		this.chips.LevelInit(this.properties);
		base.StartCoroutine(CupheadLevelCamera.Current.rotate_camera());
		base.StartCoroutine(this.rotate_background_cr());
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0005EF9B File Offset: 0x0005D39B
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacechipsPattern_cr());
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0005EFAA File Offset: 0x0005D3AA
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0005EFBC File Offset: 0x0005D3BC
	private IEnumerator dicepalacechipsPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0005EFD8 File Offset: 0x0005D3D8
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceChips.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceChips.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0005EFF4 File Offset: 0x0005D3F4
	private IEnumerator rotate_background_cr()
	{
		float time = 1.5f;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			float phase = Mathf.Sin(t / time);
			this.background.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * 1f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000561 RID: 1377
	private LevelProperties.DicePalaceChips properties;

	// Token: 0x04000562 RID: 1378
	[SerializeField]
	private GameObject background;

	// Token: 0x04000563 RID: 1379
	[SerializeField]
	private DicePalaceChipsLevelChips chips;

	// Token: 0x04000564 RID: 1380
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000565 RID: 1381
	[SerializeField]
	private string _bossQuote;
}
