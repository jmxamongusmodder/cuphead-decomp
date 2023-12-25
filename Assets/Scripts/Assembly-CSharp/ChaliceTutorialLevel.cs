using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class ChaliceTutorialLevel : Level
{
	// Token: 0x06000130 RID: 304 RVA: 0x00057334 File Offset: 0x00055734
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChaliceTutorial.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000131 RID: 305 RVA: 0x000573CA File Offset: 0x000557CA
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChaliceTutorial;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000132 RID: 306 RVA: 0x000573D1 File Offset: 0x000557D1
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chalice_tutorial;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000133 RID: 307 RVA: 0x000573D5 File Offset: 0x000557D5
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000134 RID: 308 RVA: 0x000573DD File Offset: 0x000557DD
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x000573E8 File Offset: 0x000557E8
	protected override void Start()
	{
		base.Start();
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			if (abstractPlayerController != null)
			{
				foreach (Transform transform in abstractPlayerController.GetComponentsInChildren<Transform>())
				{
					transform.gameObject.layer = 31;
				}
			}
		}
		base.StartCoroutine(this.parryables_cr());
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0005748C File Offset: 0x0005588C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0005749A File Offset: 0x0005589A
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.intro_cr());
		base.StartCoroutine(this.chalicetutorialPattern_cr());
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000574B8 File Offset: 0x000558B8
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.backgroundAnimator.Play("Zoom");
		yield break;
	}

	// Token: 0x06000139 RID: 313 RVA: 0x000574D4 File Offset: 0x000558D4
	private IEnumerator parryables_cr()
	{
		for (;;)
		{
			for (int j = 0; j < this.parrybles.Length; j++)
			{
				this.parrybles[j].Deactivated();
			}
			while (!this.backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				Effect[] es = UnityEngine.Object.FindObjectsOfType<Effect>();
				foreach (Effect effect in es)
				{
					effect.gameObject.layer = 31;
					if (effect.transform.childCount > 0)
					{
						foreach (Transform transform in effect.transform.GetChildTransforms())
						{
							transform.gameObject.layer = 31;
						}
					}
				}
				AbstractProjectile[] ps = UnityEngine.Object.FindObjectsOfType<AbstractProjectile>();
				foreach (AbstractProjectile abstractProjectile in ps)
				{
					abstractProjectile.gameObject.layer = 31;
					if (abstractProjectile.transform.childCount > 0)
					{
						foreach (Transform transform2 in abstractProjectile.transform.GetChildTransforms())
						{
							transform2.gameObject.layer = 31;
						}
					}
				}
				PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
				yield return null;
			}
			PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
			for (int i = 0; i < this.parrybles.Length; i++)
			{
				this.parrybles[i].Activated();
				while (!this.parrybles[i].isDeactivated)
				{
					if (this.resetParryables)
					{
						break;
					}
					yield return null;
				}
				if (this.resetParryables)
				{
					break;
				}
			}
			this.resetParryables = false;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x000574F0 File Offset: 0x000558F0
	public void Exit()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.playerExitEffects[0].gameObject.SetActive(true);
		this.playerExitEffects[0].transform.position = player.transform.position;
		player.gameObject.SetActive(false);
		this.playerExitEffects[0].animator.SetTrigger("OnStartTutorial");
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.playerExitEffects[1].gameObject.SetActive(true);
			this.playerExitEffects[1].transform.position = player.transform.position;
			player.gameObject.SetActive(false);
			this.playerExitEffects[1].animator.SetTrigger("OnStartTutorial");
		}
	}

	// Token: 0x0600013B RID: 315 RVA: 0x000575C0 File Offset: 0x000559C0
	private IEnumerator chalicetutorialPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000575DC File Offset: 0x000559DC
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.ChaliceTutorial.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x000575F7 File Offset: 0x000559F7
	private void WORKAROUND_NullifyFields()
	{
		this._bossPortrait = null;
		this._bossQuote = null;
		this.backgroundAnimator = null;
		this.parrybles = null;
		this.playerExitEffects = null;
	}

	// Token: 0x04000291 RID: 657
	private LevelProperties.ChaliceTutorial properties;

	// Token: 0x04000292 RID: 658
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000293 RID: 659
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x04000294 RID: 660
	[SerializeField]
	private Animator backgroundAnimator;

	// Token: 0x04000295 RID: 661
	[SerializeField]
	private ChaliceTutorialLevelParryable[] parrybles;

	// Token: 0x04000296 RID: 662
	private bool finishedPuzzle;

	// Token: 0x04000297 RID: 663
	public bool resetParryables;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	private PlayerDeathEffect[] playerExitEffects;
}
