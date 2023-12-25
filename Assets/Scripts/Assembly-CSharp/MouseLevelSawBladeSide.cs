using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006F8 RID: 1784
public class MouseLevelSawBladeSide : AbstractPausableComponent
{
	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06002635 RID: 9781 RVA: 0x00165352 File Offset: 0x00163752
	// (set) Token: 0x06002636 RID: 9782 RVA: 0x0016535A File Offset: 0x0016375A
	public MouseLevelSawBladeSide.State state { get; private set; }

	// Token: 0x06002637 RID: 9783 RVA: 0x00165364 File Offset: 0x00163764
	public void Begin(LevelProperties.Mouse properties)
	{
		this.properties = properties;
		foreach (MouseLevelSawBlade mouseLevelSawBlade in this.sawBlades)
		{
			mouseLevelSawBlade.Begin(properties);
		}
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x001653AC File Offset: 0x001637AC
	public void Leave()
	{
		this.StopAllCoroutines();
		foreach (MouseLevelSawBlade mouseLevelSawBlade in this.sawBlades)
		{
			mouseLevelSawBlade.Leave();
		}
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x001653E4 File Offset: 0x001637E4
	public void SetPattern(string pattern)
	{
		this.pattern = pattern.Split(new char[]
		{
			','
		});
		this.patternIndex = UnityEngine.Random.Range(0, this.pattern.Length);
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x00165411 File Offset: 0x00163811
	public void FullAttack()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.fullAttack_cr());
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x00165428 File Offset: 0x00163828
	private IEnumerator intro_cr()
	{
		while (this.sawBlades[0].state != MouseLevelSawBlade.State.Idle)
		{
			yield return null;
		}
		this.state = MouseLevelSawBladeSide.State.Pattern;
		base.StartCoroutine(this.pattern_cr());
		yield break;
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x00165444 File Offset: 0x00163844
	private IEnumerator pattern_cr()
	{
		if (this.pattern == null)
		{
			yield break;
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.brokenCanSawBlades.delayBeforeNextSaw);
			int sawBladeIndex = 0;
			Parser.IntTryParse(this.pattern[this.patternIndex], out sawBladeIndex);
			this.sawBlades[sawBladeIndex - 1].Attack();
			this.patternIndex = (this.patternIndex + 1) % this.pattern.Length;
		}
		yield break;
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x00165460 File Offset: 0x00163860
	private IEnumerator fullAttack_cr()
	{
		this.state = MouseLevelSawBladeSide.State.FullAttack;
		bool canFullAttack = false;
		while (!canFullAttack)
		{
			canFullAttack = true;
			foreach (MouseLevelSawBlade mouseLevelSawBlade in this.sawBlades)
			{
				if (mouseLevelSawBlade.state != MouseLevelSawBlade.State.Idle)
				{
					canFullAttack = false;
				}
			}
			yield return null;
		}
		AudioManager.Play("level_mouse_buzzsaw_wall");
		foreach (MouseLevelSawBlade mouseLevelSawBlade2 in this.sawBlades)
		{
			mouseLevelSawBlade2.FullAttack();
		}
		while (this.sawBlades[0].state != MouseLevelSawBlade.State.Idle)
		{
			yield return null;
		}
		this.state = MouseLevelSawBladeSide.State.Pattern;
		base.StartCoroutine(this.pattern_cr());
		yield break;
	}

	// Token: 0x04002EC2 RID: 11970
	[SerializeField]
	private MouseLevelSawBlade[] sawBlades;

	// Token: 0x04002EC3 RID: 11971
	private LevelProperties.Mouse properties;

	// Token: 0x04002EC4 RID: 11972
	private string[] pattern;

	// Token: 0x04002EC5 RID: 11973
	private int patternIndex;

	// Token: 0x020006F9 RID: 1785
	public enum State
	{
		// Token: 0x04002EC8 RID: 11976
		Init,
		// Token: 0x04002EC9 RID: 11977
		Pattern,
		// Token: 0x04002ECA RID: 11978
		FullAttack
	}
}
