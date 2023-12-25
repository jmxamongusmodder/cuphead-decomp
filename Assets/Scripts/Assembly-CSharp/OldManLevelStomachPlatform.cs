using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000716 RID: 1814
public class OldManLevelStomachPlatform : LevelPlatform
{
	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x0600276D RID: 10093 RVA: 0x00172256 File Offset: 0x00170656
	// (set) Token: 0x0600276E RID: 10094 RVA: 0x0017225E File Offset: 0x0017065E
	public bool isActivated { get; private set; }

	// Token: 0x0600276F RID: 10095 RVA: 0x00172267 File Offset: 0x00170667
	private void Start()
	{
		this.isActivated = true;
		base.animator.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x06002770 RID: 10096 RVA: 0x00172290 File Offset: 0x00170690
	private void aniEvent_SpawnParryable()
	{
		this.main.SpawnParryable(base.transform.position + this.tongueOffset);
		this.splashAnimator.Play("OpenSplash");
		this.splashAnimator.Update(0f);
		this.SFX_BellLoop();
	}

	// Token: 0x06002771 RID: 10097 RVA: 0x001722E4 File Offset: 0x001706E4
	public void FlipX()
	{
		foreach (SpriteRenderer spriteRenderer in this.rend)
		{
			spriteRenderer.flipX = true;
		}
		this.boxColl.offset = new Vector2(-this.boxColl.offset.x, this.boxColl.offset.y);
		this.tongueOffset.x = -this.tongueOffset.x;
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x00172366 File Offset: 0x00170766
	public void Anticipation()
	{
		if (this.isActivated)
		{
			this.isTargeted = true;
			base.animator.SetTrigger("Anticipation");
		}
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x0017238C File Offset: 0x0017078C
	public void CancelAnticipation()
	{
		this.isTargeted = false;
		if (this.isActivated)
		{
			base.animator.Play("Idle");
		}
		else
		{
			this.isActivated = true;
			base.animator.SetBool("IsEating", false);
			base.animator.Play("ReverseEat", 0, 1f - base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			base.animator.Play("Eat_Ripple_End", 1, 0f);
			if (this.bubbleCoroutine != null)
			{
				base.StopCoroutine(this.bubbleCoroutine);
			}
		}
	}

	// Token: 0x06002774 RID: 10100 RVA: 0x00172430 File Offset: 0x00170830
	public Vector3 GetTonguePos()
	{
		return base.transform.position + this.tongueOffset;
	}

	// Token: 0x06002775 RID: 10101 RVA: 0x00172448 File Offset: 0x00170848
	public void DeactivatePlatform(bool spawnsParryable)
	{
		this.spawnsParryable = spawnsParryable;
		string name = (!spawnsParryable) ? "IsEating" : "IsBell";
		if (spawnsParryable)
		{
			this.sparkAnimator.Play("Spark");
			this.SFX_BonkHead();
		}
		else
		{
			this.SFX_Chomp();
		}
		base.animator.SetBool(name, true);
		this.isActivated = false;
		this.isTargeted = false;
		if (!spawnsParryable)
		{
			this.bubbleCoroutine = base.StartCoroutine(this.bubble_cr());
		}
	}

	// Token: 0x06002776 RID: 10102 RVA: 0x001724CC File Offset: 0x001708CC
	private IEnumerator bubble_cr()
	{
		float noseTimer = this.noseBubbleRange.RandomFloat();
		float mouthTimer = this.mouthBubbleRange.RandomFloat();
		for (;;)
		{
			noseTimer -= CupheadTime.Delta;
			mouthTimer -= CupheadTime.Delta;
			if (noseTimer <= 0f)
			{
				this.noseBubble.Play("Bubble", 0, 0f);
				this.noseBubbleRend.flipX = Rand.Bool();
				noseTimer += this.noseBubbleRange.RandomFloat();
			}
			if (mouthTimer <= 0f)
			{
				this.mouthBubble.Play("Bubble", 0, 0f);
				this.mouthBubbleRend.flipX = Rand.Bool();
				mouthTimer += this.mouthBubbleRange.RandomFloat();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002777 RID: 10103 RVA: 0x001724E7 File Offset: 0x001708E7
	public void ActivatePlatform()
	{
		base.StartCoroutine(this.activate_cr());
	}

	// Token: 0x06002778 RID: 10104 RVA: 0x001724F8 File Offset: 0x001708F8
	private IEnumerator activate_cr()
	{
		if (this.bubbleCoroutine != null)
		{
			base.StopCoroutine(this.bubbleCoroutine);
		}
		this.mouthBubble.Play("None");
		this.noseBubble.Play("None");
		if (base.animator.GetBool("IsBell"))
		{
			base.animator.SetBool("IsBell", false);
			base.animator.Play("Bell_End");
		}
		else
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 0.2f));
			base.animator.SetBool("IsEating", false);
		}
		this.isActivated = true;
		this.spawnsParryable = false;
		yield break;
	}

	// Token: 0x06002779 RID: 10105 RVA: 0x00172513 File Offset: 0x00170913
	private void SFX_Chomp()
	{
		AudioManager.Play("sfx_dlc_omm_p3_dino_chomp");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_dino_chomp");
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x0017252F File Offset: 0x0017092F
	private void SFX_BonkHead()
	{
		AudioManager.Play("sfx_dlc_omm_p3_dinobells_bonkhead");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_bonkhead");
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x0017254B File Offset: 0x0017094B
	private void SFX_BellLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_omm_p3_dinobells_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_loop");
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x00172567 File Offset: 0x00170967
	private void AniEvent_SFX_BellLoopEnd()
	{
		AudioManager.Stop("sfx_dlc_omm_p3_dinobells_loop");
	}

	// Token: 0x0400302D RID: 12333
	[SerializeField]
	private SpriteRenderer[] rend;

	// Token: 0x0400302E RID: 12334
	[SerializeField]
	private BoxCollider2D boxColl;

	// Token: 0x04003030 RID: 12336
	public bool isTargeted;

	// Token: 0x04003031 RID: 12337
	private bool spawnsParryable;

	// Token: 0x04003032 RID: 12338
	public Animator sparkAnimator;

	// Token: 0x04003033 RID: 12339
	private Vector3 tongueOffset = new Vector3(20f, 30f);

	// Token: 0x04003034 RID: 12340
	public OldManLevelGnomeLeader main;

	// Token: 0x04003035 RID: 12341
	[SerializeField]
	private Animator splashAnimator;

	// Token: 0x04003036 RID: 12342
	[SerializeField]
	private Animator mouthBubble;

	// Token: 0x04003037 RID: 12343
	[SerializeField]
	private Animator noseBubble;

	// Token: 0x04003038 RID: 12344
	[SerializeField]
	private SpriteRenderer mouthBubbleRend;

	// Token: 0x04003039 RID: 12345
	[SerializeField]
	private SpriteRenderer noseBubbleRend;

	// Token: 0x0400303A RID: 12346
	[SerializeField]
	private MinMax noseBubbleRange = new MinMax(0.5833333f, 1f);

	// Token: 0x0400303B RID: 12347
	[SerializeField]
	private MinMax mouthBubbleRange = new MinMax(1f, 1.5833334f);

	// Token: 0x0400303C RID: 12348
	private Coroutine bubbleCoroutine;
}
