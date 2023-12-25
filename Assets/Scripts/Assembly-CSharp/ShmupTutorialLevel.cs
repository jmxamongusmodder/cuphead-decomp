using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A3 RID: 675
public class ShmupTutorialLevel : Level
{
	// Token: 0x0600077C RID: 1916 RVA: 0x00075980 File Offset: 0x00073D80
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ShmupTutorial.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x0600077D RID: 1917 RVA: 0x00075A16 File Offset: 0x00073E16
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ShmupTutorial;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x00075A1D File Offset: 0x00073E1D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_shmup_tutorial;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x0600077F RID: 1919 RVA: 0x00075A21 File Offset: 0x00073E21
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000780 RID: 1920 RVA: 0x00075A29 File Offset: 0x00073E29
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00075A31 File Offset: 0x00073E31
	protected override void Start()
	{
		base.Start();
		this.canvasAnimator.SetTrigger("StartAnimation");
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00075A49 File Offset: 0x00073E49
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.shmuptutorialPattern_cr());
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00075A58 File Offset: 0x00073E58
	private IEnumerator shmuptutorialPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00075A74 File Offset: 0x00073E74
	private IEnumerator shmupTutorialStartAnimation_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.waitForAnimationTime);
		this.canvasAnimator.SetTrigger("StartAnimation");
		yield break;
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00075A90 File Offset: 0x00073E90
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.ShmupTutorial.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.ShmupTutorial.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000F2E RID: 3886
	private LevelProperties.ShmupTutorial properties;

	// Token: 0x04000F2F RID: 3887
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000F30 RID: 3888
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x04000F31 RID: 3889
	public Animator canvasAnimator;

	// Token: 0x04000F32 RID: 3890
	public float waitForAnimationTime;
}
