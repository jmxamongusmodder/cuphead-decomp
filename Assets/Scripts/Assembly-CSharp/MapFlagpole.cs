using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000937 RID: 2359
public class MapFlagpole : AbstractMapLevelDependentEntity
{
	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06003724 RID: 14116 RVA: 0x001FC331 File Offset: 0x001FA731
	protected override bool ReactToDifficultyChange
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06003725 RID: 14117 RVA: 0x001FC334 File Offset: 0x001FA734
	protected override bool ReactToGradeChange
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x001FC337 File Offset: 0x001FA737
	public override void OnConditionMet()
	{
		if (Level.PreviouslyWon || this.forceNoAppearAnimation)
		{
			this.Init(this.difficulty, this.grade);
		}
	}

	// Token: 0x06003727 RID: 14119 RVA: 0x001FC360 File Offset: 0x001FA760
	public override void DoTransition()
	{
		if (Level.PreviouslyWon)
		{
			base.StartCoroutine(this.shake_cr());
		}
		else
		{
			base.StartCoroutine(this.raise_cr());
		}
	}

	// Token: 0x06003728 RID: 14120 RVA: 0x001FC38B File Offset: 0x001FA78B
	public override void OnConditionAlreadyMet()
	{
		this.Init(this.difficulty, this.grade);
	}

	// Token: 0x06003729 RID: 14121 RVA: 0x001FC39F File Offset: 0x001FA79F
	public override void OnConditionNotMet()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600372A RID: 14122 RVA: 0x001FC3B0 File Offset: 0x001FA7B0
	private void Init(Level.Mode difficulty, LevelScoringData.Grade grade)
	{
		if (this._levels.Length == 0)
		{
			return;
		}
		string stateName = string.Empty;
		bool flag = false;
		for (int i = 0; i < Level.platformingLevels.Length; i++)
		{
			if (Level.platformingLevels[i] == this._levels[0])
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (grade < LevelScoringData.Grade.AMinus)
			{
				stateName = "IdleBelowA";
			}
			else if (grade < LevelScoringData.Grade.P)
			{
				stateName = "IdleBelowP";
			}
			else
			{
				stateName = "IdleP";
			}
		}
		else if (difficulty == Level.Mode.Easy)
		{
			stateName = "IdleEasy";
		}
		else if (difficulty == Level.Mode.Normal && grade < LevelScoringData.Grade.AMinus)
		{
			stateName = "IdleNormalBelowA";
		}
		else if (difficulty == Level.Mode.Normal && grade >= LevelScoringData.Grade.AMinus)
		{
			stateName = "IdleNormalA";
		}
		else if (difficulty == Level.Mode.Hard && grade < LevelScoringData.Grade.S)
		{
			stateName = "IdleExpert";
		}
		else if (difficulty == Level.Mode.Hard && grade >= LevelScoringData.Grade.S)
		{
			stateName = "IdleExpertS";
		}
		base.animator.Play(stateName);
		if (this.forceNoAppearAnimation)
		{
			base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		}
	}

	// Token: 0x0600372B RID: 14123 RVA: 0x001FC4D0 File Offset: 0x001FA8D0
	private IEnumerator raise_cr()
	{
		if (this._levels.Length == 0)
		{
			yield break;
		}
		string trigger = string.Empty;
		bool platformingLevel = false;
		for (int i = 0; i < Level.platformingLevels.Length; i++)
		{
			if (Level.platformingLevels[i] == this._levels[0])
			{
				platformingLevel = true;
				break;
			}
		}
		if (platformingLevel)
		{
			if (this.grade < LevelScoringData.Grade.AMinus)
			{
				trigger = "RaiseBelowA";
			}
			else if (this.grade < LevelScoringData.Grade.P)
			{
				trigger = "RaiseBelowP";
			}
			else
			{
				trigger = "RaiseP";
			}
		}
		else if (this.difficulty == Level.Mode.Easy)
		{
			trigger = "RaiseEasy";
		}
		else if (this.difficulty == Level.Mode.Normal && this.grade < LevelScoringData.Grade.AMinus)
		{
			trigger = "RaiseNormalBelowA";
		}
		else if (this.difficulty == Level.Mode.Normal && this.grade >= LevelScoringData.Grade.AMinus)
		{
			trigger = "RaiseNormalA";
		}
		else if (this.difficulty == Level.Mode.Hard && this.grade < LevelScoringData.Grade.S)
		{
			trigger = "RaiseExpert";
		}
		else if (this.difficulty == Level.Mode.Hard && this.grade >= LevelScoringData.Grade.S)
		{
			trigger = "RaiseExpertS";
		}
		base.animator.SetTrigger(trigger);
		if (PlayerManager.playerWasChalice[0])
		{
			AudioManager.Play("worldmap_level_raise_flag_chalice");
		}
		else if (PlayerManager.player1IsMugman)
		{
			AudioManager.Play("worldmap_level_raise_flag_mugman");
		}
		else
		{
			AudioManager.Play("world_map_flag_raise");
		}
		yield return base.animator.WaitForAnimationToEnd(this, trigger, false, true);
		base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		yield break;
	}

	// Token: 0x0600372C RID: 14124 RVA: 0x001FC4EC File Offset: 0x001FA8EC
	private IEnumerator shake_cr()
	{
		base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		yield return null;
		yield break;
	}

	// Token: 0x04003F51 RID: 16209
	[SerializeField]
	private bool forceNoAppearAnimation;
}
