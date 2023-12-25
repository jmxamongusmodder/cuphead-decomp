using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000886 RID: 2182
public class TreePlatformingLevelBeetle : PlatformingLevelPathMovementEnemy
{
	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x060032A9 RID: 12969 RVA: 0x001D70A5 File Offset: 0x001D54A5
	// (set) Token: 0x060032AA RID: 12970 RVA: 0x001D70AD File Offset: 0x001D54AD
	public bool isActivated { get; private set; }

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x060032AB RID: 12971 RVA: 0x001D70B6 File Offset: 0x001D54B6
	// (set) Token: 0x060032AC RID: 12972 RVA: 0x001D70BE File Offset: 0x001D54BE
	public bool onCamera { get; private set; }

	// Token: 0x060032AD RID: 12973 RVA: 0x001D70C7 File Offset: 0x001D54C7
	protected override void Awake()
	{
		base.Awake();
		this.isActivated = false;
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x001D70D6 File Offset: 0x001D54D6
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.check_hit_box_cr());
		if (this.sprite != null)
		{
			base.StartCoroutine(this.motion_cr());
		}
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x001D710C File Offset: 0x001D550C
	protected override void Die()
	{
		if (this.explosion != null)
		{
			this.explosion.Create(base.transform.position);
		}
		this.hasStarted = false;
		this.onCamera = false;
		AudioManager.Stop("level_platform_beetle_idle_loop");
		AudioManager.Play("level_platform_beetle_death");
		this.emitAudioFromObject.Add("level_platform_beetle_death");
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x001D717F File Offset: 0x001D557F
	public void Activate()
	{
		this.isActivated = true;
		this.PrepareBeetle();
	}

	// Token: 0x060032B1 RID: 12977 RVA: 0x001D718E File Offset: 0x001D558E
	public void Deactivate()
	{
		this.isActivated = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.ResetStartingCondition();
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x001D71AC File Offset: 0x001D55AC
	private void PrepareBeetle()
	{
		this.pathIndex = 1;
		this.startPosition = base.allValues[0];
		base.StartFromCustom();
		base.StartCoroutine(this.check_for_death_cr());
		if ((this.pathIndex - 1) % 2 != 0)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		else
		{
			base.transform.SetScale(new float?(1f), null, null);
		}
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x001D7243 File Offset: 0x001D5643
	protected override void OnStart()
	{
		base.OnStart();
		base.GetComponent<SpriteRenderer>().enabled = true;
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x001D7257 File Offset: 0x001D5657
	protected override void EndPath()
	{
		base.EndPath();
		this.Deactivate();
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x001D7268 File Offset: 0x001D5668
	private void Flip()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(base.transform.localScale.y), new float?(base.transform.localScale.z));
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x001D72C9 File Offset: 0x001D56C9
	public void PlayIdleSFX()
	{
		if (!AudioManager.CheckIfPlaying("level_platform_beetle_idle_loop"))
		{
			AudioManager.PlayLoop("level_platform_beetle_idle_loop");
		}
		this.emitAudioFromObject.Add("level_platform_beetle_idle_loop");
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x001D72F4 File Offset: 0x001D56F4
	private IEnumerator check_for_death_cr()
	{
		while (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 50f)
		{
			yield return null;
		}
		this.hasStarted = false;
		this.onCamera = false;
		yield return null;
		yield break;
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x001D7310 File Offset: 0x001D5710
	private IEnumerator check_hit_box_cr()
	{
		for (;;)
		{
			if (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 50f && this.hasStarted)
			{
				this.sprite.GetComponent<SpriteRenderer>().enabled = true;
				if (base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 50f)
				{
					if (this.hasStarted)
					{
						this.onCamera = true;
					}
				}
				else
				{
					this.onCamera = false;
				}
			}
			else
			{
				this.sprite.GetComponent<SpriteRenderer>().enabled = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x001D732C File Offset: 0x001D572C
	private IEnumerator motion_cr()
	{
		float time = 0.1f;
		float t = 0f;
		float amount = 2f;
		for (;;)
		{
			while (t < time)
			{
				while (!this.isActivated || PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
				t += CupheadTime.Delta;
				this.sprite.transform.AddPosition(0f, amount, 0f);
				yield return null;
			}
			t = 0f;
			amount = -amount;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003AE8 RID: 15080
	[SerializeField]
	private Effect explosion;

	// Token: 0x04003AE9 RID: 15081
	[SerializeField]
	private SpriteRenderer sprite;
}
