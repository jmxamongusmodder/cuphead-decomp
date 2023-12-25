using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x020006F6 RID: 1782
public class MouseLevelSawBladeManager : AbstractPausableComponent
{
	// Token: 0x06002630 RID: 9776 RVA: 0x00164FAD File Offset: 0x001633AD
	public void Begin(LevelProperties.Mouse properties)
	{
		this.properties = properties;
		this.leftSawBlades.Begin(properties);
		this.rightSawBlades.Begin(properties);
		base.StartCoroutine(this.pattern_cr());
		base.StartCoroutine(this.fullAttack_cr());
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x00164FE8 File Offset: 0x001633E8
	public void Leave()
	{
		this.StopAllCoroutines();
		this.leftSawBlades.Leave();
		this.rightSawBlades.Leave();
	}

	// Token: 0x06002632 RID: 9778 RVA: 0x00165008 File Offset: 0x00163408
	private IEnumerator pattern_cr()
	{
		LevelProperties.Mouse.State patternState = this.properties.CurrentState;
		if (patternState.brokenCanSawBlades.patternString.Length == 0)
		{
			yield break;
		}
		string patternString = patternState.brokenCanSawBlades.patternString.RandomChoice<string>();
		this.leftSawBlades.SetPattern(patternString);
		this.rightSawBlades.SetPattern(patternString);
		for (;;)
		{
			if (this.properties.CurrentState != patternState)
			{
				if (!patternState.brokenCanSawBlades.patternString.SequenceEqual(this.properties.CurrentState.brokenCanSawBlades.patternString))
				{
					patternString = this.properties.CurrentState.brokenCanSawBlades.patternString.RandomChoice<string>();
					this.leftSawBlades.SetPattern(patternString);
					this.rightSawBlades.SetPattern(patternString);
				}
				patternState = this.properties.CurrentState;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002633 RID: 9779 RVA: 0x00165024 File Offset: 0x00163424
	private IEnumerator fullAttack_cr()
	{
		for (;;)
		{
			while (this.leftSawBlades.state != MouseLevelSawBladeSide.State.Pattern || this.rightSawBlades.state != MouseLevelSawBladeSide.State.Pattern)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.brokenCanSawBlades.fullAttackTime.RandomFloat());
			AbstractPlayerController player = PlayerManager.GetNext();
			if (player.transform.position.x > 0f)
			{
				this.rightSawBlades.FullAttack();
			}
			else
			{
				this.leftSawBlades.FullAttack();
			}
		}
		yield break;
	}

	// Token: 0x04002EBA RID: 11962
	[SerializeField]
	private MouseLevelSawBladeSide leftSawBlades;

	// Token: 0x04002EBB RID: 11963
	[SerializeField]
	private MouseLevelSawBladeSide rightSawBlades;

	// Token: 0x04002EBC RID: 11964
	private LevelProperties.Mouse properties;

	// Token: 0x020006F7 RID: 1783
	public enum State
	{
		// Token: 0x04002EBE RID: 11966
		Init,
		// Token: 0x04002EBF RID: 11967
		Idle,
		// Token: 0x04002EC0 RID: 11968
		Warning,
		// Token: 0x04002EC1 RID: 11969
		Attack
	}
}
