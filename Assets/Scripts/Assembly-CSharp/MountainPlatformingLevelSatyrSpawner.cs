using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008EE RID: 2286
public class MountainPlatformingLevelSatyrSpawner : AbstractPausableComponent
{
	// Token: 0x060035A0 RID: 13728 RVA: 0x001F3BF4 File Offset: 0x001F1FF4
	private void Start()
	{
		base.StartCoroutine(this.spawn_cr());
		this.directionIndex = UnityEngine.Random.Range(0, this.directionString.Split(new char[]
		{
			','
		}).Length);
		this.spawnIndex = UnityEngine.Random.Range(0, this.spawnString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x001F3C58 File Offset: 0x001F2058
	private IEnumerator spawn_cr()
	{
		PlatformingLevelGroundMovementEnemy.Direction direction = PlatformingLevelGroundMovementEnemy.Direction.Right;
		bool isForeground = false;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.spawnDelayRange.RandomFloat());
			Vector2 spawnPos = base.transform.position;
			spawnPos.y = base.transform.position.y;
			spawnPos.x += UnityEngine.Random.Range(-this.xRange, this.xRange);
			AbstractPlayerController player = PlayerManager.GetNext();
			if (CupheadLevelCamera.Current.ContainsPoint(spawnPos, new Vector2(0f, 1000f)))
			{
				if (this.spawnString.Split(new char[]
				{
					','
				})[this.spawnIndex][0] == 'F')
				{
					isForeground = true;
				}
				else if (this.spawnString.Split(new char[]
				{
					','
				})[this.spawnIndex][0] == 'B')
				{
					isForeground = false;
				}
				if (this.directionString.Split(new char[]
				{
					','
				})[this.directionIndex][0] == 'L')
				{
					direction = PlatformingLevelGroundMovementEnemy.Direction.Left;
				}
				else if (this.directionString.Split(new char[]
				{
					','
				})[this.directionIndex][0] == 'R')
				{
					direction = PlatformingLevelGroundMovementEnemy.Direction.Right;
				}
				else if (this.directionString.Split(new char[]
				{
					','
				})[this.directionIndex][0] == 'P')
				{
					if (player.transform.position.x < spawnPos.x)
					{
						direction = PlatformingLevelGroundMovementEnemy.Direction.Left;
					}
					else
					{
						direction = PlatformingLevelGroundMovementEnemy.Direction.Right;
					}
				}
				MountainPlatformingLevelSatyr mountainPlatformingLevelSatyr = this.satyrPrefab.Spawn(spawnPos, direction, true) as MountainPlatformingLevelSatyr;
				mountainPlatformingLevelSatyr.Init(direction, isForeground);
				this.directionIndex = (this.directionIndex + 1) % this.directionString.Split(new char[]
				{
					','
				}).Length;
				this.spawnIndex = (this.spawnIndex + 1) % this.spawnString.Split(new char[]
				{
					','
				}).Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x001F3C74 File Offset: 0x001F2074
	protected override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x04003DB1 RID: 15793
	[SerializeField]
	private string directionString;

	// Token: 0x04003DB2 RID: 15794
	[SerializeField]
	private string spawnString;

	// Token: 0x04003DB3 RID: 15795
	[SerializeField]
	private float xRange;

	// Token: 0x04003DB4 RID: 15796
	[SerializeField]
	private MountainPlatformingLevelSatyr satyrPrefab;

	// Token: 0x04003DB5 RID: 15797
	[SerializeField]
	private MinMax spawnDelayRange;

	// Token: 0x04003DB6 RID: 15798
	private int directionIndex;

	// Token: 0x04003DB7 RID: 15799
	private int spawnIndex;
}
