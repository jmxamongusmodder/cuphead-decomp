using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000479 RID: 1145
public abstract class AbstractLevelProperties<STATE, PATTERN, STATE_NAMES> where STATE : AbstractLevelState<PATTERN, STATE_NAMES>
{
	// Token: 0x0600119A RID: 4506 RVA: 0x000037CE File Offset: 0x00001BCE
	public AbstractLevelProperties(float hp, Level.GoalTimes goalTimes, STATE[] states)
	{
		this.TotalHealth = hp;
		this.CurrentHealth = this.TotalHealth;
		this.goalTimes = goalTimes;
		this.states = states;
		this.stateIndex = 0;
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x0600119B RID: 4507 RVA: 0x000037FE File Offset: 0x00001BFE
	// (set) Token: 0x0600119C RID: 4508 RVA: 0x00003806 File Offset: 0x00001C06
	public float CurrentHealth { get; private set; }

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x0600119D RID: 4509 RVA: 0x00003810 File Offset: 0x00001C10
	// (remove) Token: 0x0600119E RID: 4510 RVA: 0x00003848 File Offset: 0x00001C48
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AbstractLevelProperties<STATE, PATTERN, STATE_NAMES>.OnBossDamagedHandler OnBossDamaged;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x0600119F RID: 4511 RVA: 0x00003880 File Offset: 0x00001C80
	// (remove) Token: 0x060011A0 RID: 4512 RVA: 0x000038B8 File Offset: 0x00001CB8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnBossDeath;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x060011A1 RID: 4513 RVA: 0x000038F0 File Offset: 0x00001CF0
	// (remove) Token: 0x060011A2 RID: 4514 RVA: 0x00003928 File Offset: 0x00001D28
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnStateChange;

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x060011A3 RID: 4515 RVA: 0x0000395E File Offset: 0x00001D5E
	public STATE CurrentState
	{
		get
		{
			this.stateIndex = Mathf.Clamp(this.stateIndex, 0, this.states.Length - 1);
			return this.states[this.stateIndex];
		}
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x00003990 File Offset: 0x00001D90
	public void DealDamage(float damage)
	{
		this.CurrentHealth -= damage;
		if (this.OnBossDamaged != null)
		{
			this.OnBossDamaged(damage);
		}
		if (this.CurrentHealth <= 0f)
		{
			this.WinInstantly();
			return;
		}
		int num = 0;
		for (int i = 0; i < this.states.Length; i++)
		{
			float num2 = this.CurrentHealth / this.TotalHealth;
			if (num2 < this.states[i].healthTrigger)
			{
				num = i;
			}
		}
		if (this.stateIndex != num)
		{
			this.stateIndex = num;
			if (this.OnStateChange != null)
			{
				this.OnStateChange();
			}
		}
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x00003A4C File Offset: 0x00001E4C
	public void DealDamageToNextNamedState()
	{
		STATE_NAMES stateName = this.CurrentState.stateName;
		string text = stateName.ToString();
		int num = 0;
		while ((float)num < this.TotalHealth)
		{
			this.DealDamage(1f);
			STATE_NAMES stateName2 = this.CurrentState.stateName;
			if (stateName2.ToString() != "Generic")
			{
				string a = text;
				STATE_NAMES stateName3 = this.CurrentState.stateName;
				if (a != stateName3.ToString())
				{
					return;
				}
			}
			num++;
		}
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x00003AF3 File Offset: 0x00001EF3
	public float GetNextStateHealthTrigger()
	{
		if (this.stateIndex < this.states.Length - 1)
		{
			return this.states[this.stateIndex + 1].healthTrigger;
		}
		return 0f;
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x00003B2D File Offset: 0x00001F2D
	public void WinInstantly()
	{
		if (this.OnBossDeath != null)
		{
			this.OnBossDeath();
		}
		this.OnBossDeath = null;
	}

	// Token: 0x04001B28 RID: 6952
	public readonly float TotalHealth;

	// Token: 0x04001B2A RID: 6954
	public readonly Level.GoalTimes goalTimes;

	// Token: 0x04001B2B RID: 6955
	private readonly STATE[] states;

	// Token: 0x04001B2C RID: 6956
	private int stateIndex;

	// Token: 0x0200047A RID: 1146
	// (Invoke) Token: 0x060011A9 RID: 4521
	public delegate void OnBossDamagedHandler(float damage);
}
