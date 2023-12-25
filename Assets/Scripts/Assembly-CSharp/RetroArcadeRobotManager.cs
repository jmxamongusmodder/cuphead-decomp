using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000754 RID: 1876
public class RetroArcadeRobotManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x060028E3 RID: 10467 RVA: 0x0017CB18 File Offset: 0x0017AF18
	public void StartRobots()
	{
		this.p = base.properties.CurrentState.robots;
		this.phase = 0;
		base.StartCoroutine(this.bonus_cr());
		this.StartNewPhase();
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x0017CB4C File Offset: 0x0017AF4C
	public void StartNewPhase()
	{
		this.numDied = 0;
		string[] array = this.p.robotWaves[this.phase].Split(new char[]
		{
			','
		});
		this.numRobotsToKill = array.Length;
		for (int i = 0; i < this.numRobotsToKill; i++)
		{
			int num;
			Parser.IntTryParse(array[i], out num);
			if (num > 0 && num <= this.p.robotsXPositions.Length)
			{
				string[] array2 = this.p.robotColorPattern[this.phase].Split(new char[]
				{
					','
				});
				string[] orbiterPattern = array2[i].Split(new char[]
				{
					'-'
				});
				float xPos = this.p.robotsXPositions[num - 1];
				this.bigRobotPrefab.Create(xPos, this.p, (float)i / 3f, this, orbiterPattern);
			}
		}
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x0017CC30 File Offset: 0x0017B030
	private IEnumerator bonus_cr()
	{
		for (int i = 0; i < this.p.bonusCount; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, this.p.bonusDelay.RandomFloat());
			this.bonusRobotPrefab.Create((!Rand.Bool()) ? RetroArcadeBonusRobot.Direction.Right : RetroArcadeBonusRobot.Direction.Left, this.p);
		}
		yield break;
	}

	// Token: 0x060028E6 RID: 10470 RVA: 0x0017CC4C File Offset: 0x0017B04C
	public void OnRobotGroupDie()
	{
		this.numDied++;
		if (this.numDied >= this.numRobotsToKill)
		{
			if (this.phase >= this.p.robotWaves.Length - 1)
			{
				base.properties.DealDamageToNextNamedState();
				this.StopAllCoroutines();
			}
			else
			{
				this.phase++;
				this.StartNewPhase();
			}
		}
	}

	// Token: 0x040031BE RID: 12734
	private const float BIG_ROBOT_SPACING = 160f;

	// Token: 0x040031BF RID: 12735
	[SerializeField]
	private RetroArcadeBigRobot bigRobotPrefab;

	// Token: 0x040031C0 RID: 12736
	[SerializeField]
	private RetroArcadeBonusRobot bonusRobotPrefab;

	// Token: 0x040031C1 RID: 12737
	private LevelProperties.RetroArcade.Robots p;

	// Token: 0x040031C2 RID: 12738
	private int numDied;

	// Token: 0x040031C3 RID: 12739
	private int phase;

	// Token: 0x040031C4 RID: 12740
	private int numRobotsToKill;
}
