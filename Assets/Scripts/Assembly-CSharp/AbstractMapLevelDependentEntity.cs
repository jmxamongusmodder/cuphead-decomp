using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200092A RID: 2346
public abstract class AbstractMapLevelDependentEntity : AbstractMonoBehaviour
{
	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x060036DE RID: 14046 RVA: 0x001FA587 File Offset: 0x001F8987
	// (set) Token: 0x060036DF RID: 14047 RVA: 0x001FA58E File Offset: 0x001F898E
	public static List<AbstractMapLevelDependentEntity> RegisteredEntities { get; private set; }

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x060036E0 RID: 14048 RVA: 0x001FA596 File Offset: 0x001F8996
	protected virtual bool ReactToGradeChange
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x060036E1 RID: 14049 RVA: 0x001FA599 File Offset: 0x001F8999
	protected virtual bool ReactToDifficultyChange
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x060036E2 RID: 14050 RVA: 0x001FA59C File Offset: 0x001F899C
	public Vector2 CameraPosition
	{
		get
		{
			return base.baseTransform.position + this._cameraPosition;
		}
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x060036E3 RID: 14051 RVA: 0x001FA5B9 File Offset: 0x001F89B9
	// (set) Token: 0x060036E4 RID: 14052 RVA: 0x001FA5C1 File Offset: 0x001F89C1
	public AbstractMapLevelDependentEntity.State CurrentState { get; protected set; }

	// Token: 0x060036E5 RID: 14053
	public abstract void OnConditionNotMet();

	// Token: 0x060036E6 RID: 14054
	public abstract void OnConditionMet();

	// Token: 0x060036E7 RID: 14055
	public abstract void OnConditionAlreadyMet();

	// Token: 0x060036E8 RID: 14056
	public abstract void DoTransition();

	// Token: 0x060036E9 RID: 14057 RVA: 0x001FA5CA File Offset: 0x001F89CA
	protected override void Awake()
	{
		base.Awake();
		if (AbstractMapLevelDependentEntity.RegisteredEntities == null)
		{
			AbstractMapLevelDependentEntity.RegisteredEntities = new List<AbstractMapLevelDependentEntity>();
		}
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x001FA5E6 File Offset: 0x001F89E6
	protected virtual void Start()
	{
		this.Check();
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x001FA5EE File Offset: 0x001F89EE
	private void OnDestroy()
	{
		AbstractMapLevelDependentEntity.RegisteredEntities = null;
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x001FA5F8 File Offset: 0x001F89F8
	private void Check()
	{
		bool flag = this.ValidateSucess();
		if (flag)
		{
			bool flag2 = false;
			foreach (Levels levels2 in this._levels)
			{
				if (this.anyLevelPassesCheck)
				{
					if ((Level.PreviousLevel != levels2 || Level.PreviouslyWon) && PlayerData.Data.GetLevelData(levels2).completed)
					{
						flag2 = false;
						break;
					}
					if (Level.PreviousLevel == levels2 && Level.Won && !Level.PreviouslyWon)
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = this.ValidateCondition(levels2);
				}
			}
			if (flag2)
			{
				this.CallOnConditionMet();
			}
			else
			{
				this.CallOnConditionAlreadyMet();
			}
			return;
		}
		this.CallOnConditionNotMet();
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x001FA6C0 File Offset: 0x001F8AC0
	protected virtual bool ValidateCondition(Levels level)
	{
		if (!Level.Won)
		{
			return false;
		}
		if (Level.PreviousLevel != level)
		{
			return false;
		}
		bool result = false;
		if (!Level.PreviouslyWon && Level.Won)
		{
			this.firstTimeWon = true;
			result = true;
		}
		if (this.ReactToGradeChange && Level.Grade > Level.PreviousGrade)
		{
			this.gradeChanged = true;
			result = true;
		}
		if (this.ReactToDifficultyChange && Level.Difficulty > Level.PreviousDifficulty)
		{
			this.difficultyChanged = true;
			result = true;
		}
		return result;
	}

	// Token: 0x060036EE RID: 14062 RVA: 0x001FA74C File Offset: 0x001F8B4C
	protected virtual bool ValidateSucess()
	{
		bool result = true;
		foreach (Levels levelID in this._levels)
		{
			PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(levelID);
			if (!levelData.completed)
			{
				result = false;
				if (!this.anyLevelPassesCheck)
				{
					break;
				}
			}
			else
			{
				this.difficulty = levelData.difficultyBeaten;
				this.grade = levelData.grade;
				if (this.anyLevelPassesCheck)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060036EF RID: 14063 RVA: 0x001FA7D7 File Offset: 0x001F8BD7
	private void CallOnConditionNotMet()
	{
		this.CurrentState = AbstractMapLevelDependentEntity.State.Incomplete;
		this.OnConditionNotMet();
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x001FA7E6 File Offset: 0x001F8BE6
	private void CallOnConditionAlreadyMet()
	{
		this.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		this.OnConditionAlreadyMet();
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x001FA7F5 File Offset: 0x001F8BF5
	private void CallOnConditionMet()
	{
		this.CurrentState = AbstractMapLevelDependentEntity.State.Incomplete;
		this.OnConditionMet();
		AbstractMapLevelDependentEntity.RegisteredEntities.Add(this);
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x001FA80F File Offset: 0x001F8C0F
	public void MapMeetCondition()
	{
		this.DoTransition();
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x001FA817 File Offset: 0x001F8C17
	private void OnValidate()
	{
		if (this._levels == null)
		{
			this._levels = new Levels[1];
		}
		if (this._levels.Length < 1)
		{
			Array.Resize<Levels>(ref this._levels, 1);
		}
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x001FA84C File Offset: 0x001F8C4C
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Vector2 v = base.baseTransform.position + this._cameraPosition;
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(v, 0.19f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(v, 0.2f);
	}

	// Token: 0x04003F1B RID: 16155
	[SerializeField]
	protected bool anyLevelPassesCheck;

	// Token: 0x04003F1C RID: 16156
	[SerializeField]
	protected Levels[] _levels;

	// Token: 0x04003F1D RID: 16157
	[SerializeField]
	private Vector2 _cameraPosition = Vector2.zero;

	// Token: 0x04003F1E RID: 16158
	public bool panCamera;

	// Token: 0x04003F20 RID: 16160
	protected bool firstTimeWon;

	// Token: 0x04003F21 RID: 16161
	protected bool gradeChanged;

	// Token: 0x04003F22 RID: 16162
	protected bool difficultyChanged;

	// Token: 0x04003F23 RID: 16163
	protected Level.Mode difficulty;

	// Token: 0x04003F24 RID: 16164
	protected LevelScoringData.Grade grade;

	// Token: 0x0200092B RID: 2347
	public enum State
	{
		// Token: 0x04003F26 RID: 16166
		Incomplete,
		// Token: 0x04003F27 RID: 16167
		Transitioning,
		// Token: 0x04003F28 RID: 16168
		Complete
	}
}
