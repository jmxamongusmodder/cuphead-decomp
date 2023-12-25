using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005DB RID: 1499
public class DicePalacePachinkoLevelPipes : LevelProperties.DicePalacePachinko.Entity
{
	// Token: 0x06001DA0 RID: 7584 RVA: 0x0011040C File Offset: 0x0010E80C
	public override void LevelInit(LevelProperties.DicePalacePachinko properties)
	{
		base.LevelInit(properties);
		this.spawnPointIndex = UnityEngine.Random.Range(0, properties.CurrentState.balls.spawnOrderString.Split(new char[]
		{
			','
		}).Length);
		this.spawnDelayIndex = UnityEngine.Random.Range(0, properties.CurrentState.balls.ballDelayString.Split(new char[]
		{
			','
		}).Length);
		this.pinkBallSpawnIndex = UnityEngine.Random.Range(0, properties.CurrentState.balls.pinkString.Split(new char[]
		{
			','
		}).Length);
		this.currentBallCount = 0;
		Level.Current.OnIntroEvent += this.StartAttack;
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x001104C7 File Offset: 0x0010E8C7
	private void StartAttack()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x001104D8 File Offset: 0x0010E8D8
	private IEnumerator attack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.balls.initialAttackDelay);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.balls.ballDelayString.Split(new char[]
			{
				','
			})[this.spawnDelayIndex]));
			AbstractProjectile ball = this.ballPrefab.Create(this.spawnPoints[Parser.IntParse(base.properties.CurrentState.balls.spawnOrderString.Split(new char[]
			{
				','
			})[this.spawnPointIndex]) - 1].position);
			ball.GetComponent<DicePalacePachinkoLevelPipeBall>().InitBall(base.properties);
			if (this.currentBallCount < Parser.IntParse(base.properties.CurrentState.balls.pinkString.Split(new char[]
			{
				','
			})[this.pinkBallSpawnIndex]))
			{
				this.currentBallCount++;
			}
			else
			{
				ball.SetParryable(true);
				ball.GetComponentInChildren<SpriteRenderer>().color = Color.red;
				this.pinkBallSpawnIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.balls.pinkString.Split(new char[]
				{
					','
				}).Length);
				this.currentBallCount = 0;
			}
			this.spawnPointIndex++;
			if (this.spawnPointIndex >= base.properties.CurrentState.balls.spawnOrderString.Split(new char[]
			{
				','
			}).Length)
			{
				this.spawnPointIndex = 0;
			}
			this.spawnDelayIndex++;
			if (this.spawnDelayIndex >= base.properties.CurrentState.balls.ballDelayString.Split(new char[]
			{
				','
			}).Length)
			{
				this.spawnDelayIndex = 0;
			}
		}
		yield break;
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x001104F3 File Offset: 0x0010E8F3
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x0400267C RID: 9852
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x0400267D RID: 9853
	[SerializeField]
	private DicePalacePachinkoLevelPipeBall ballPrefab;

	// Token: 0x0400267E RID: 9854
	private int spawnDelayIndex;

	// Token: 0x0400267F RID: 9855
	private int spawnPointIndex;

	// Token: 0x04002680 RID: 9856
	private int pinkBallSpawnIndex;

	// Token: 0x04002681 RID: 9857
	private int currentBallCount;
}
