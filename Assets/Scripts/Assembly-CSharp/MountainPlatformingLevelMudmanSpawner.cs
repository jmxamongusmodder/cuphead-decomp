using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E9 RID: 2281
public class MountainPlatformingLevelMudmanSpawner : AbstractPausableComponent
{
	// Token: 0x0600357E RID: 13694 RVA: 0x001F295F File Offset: 0x001F0D5F
	public void SpawnMudmen()
	{
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x001F2970 File Offset: 0x001F0D70
	private IEnumerator spawn_cr()
	{
		string[] mudmanSize = this.mudmanSizeString.Split(new char[]
		{
			','
		});
		string[] mudmanBig = this.mudmanBigSpawnString.Split(new char[]
		{
			','
		});
		string[] mudmanSmall = this.mudmanSmallSpawnString.Split(new char[]
		{
			','
		});
		int mudmanSizeIndex = UnityEngine.Random.Range(0, mudmanSize.Length);
		int mudmanBigIndex = UnityEngine.Random.Range(0, mudmanBig.Length);
		int mudmanSmallIndex = UnityEngine.Random.Range(0, mudmanSmall.Length);
		PlatformingLevelGroundMovementEnemy.Direction dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
		yield return CupheadTime.WaitForSeconds(this, this.initialDelayRange.RandomFloat());
		while (MountainPlatformingLevelElevatorHandler.elevatorIsMoving)
		{
			if (mudmanSize[mudmanSizeIndex][0] == 'B')
			{
				string[] array = mudmanBig[mudmanBigIndex].Split(new char[]
				{
					'-'
				});
				foreach (string s in array)
				{
					int num = 1;
					Parser.IntTryParse(s, out num);
					dir = ((num >= 3) ? PlatformingLevelGroundMovementEnemy.Direction.Left : PlatformingLevelGroundMovementEnemy.Direction.Right);
					MountainPlatformingLevelMudman mountainPlatformingLevelMudman = UnityEngine.Object.Instantiate<MountainPlatformingLevelMudman>(this.bigMudman);
					mountainPlatformingLevelMudman.Init(this.spawnPoints[num - 1].position, dir);
				}
				mudmanBigIndex = (mudmanBigIndex + 1) % mudmanBig.Length;
			}
			else if (mudmanSize[mudmanSizeIndex][0] == 'S')
			{
				string[] array3 = mudmanSmall[mudmanSmallIndex].Split(new char[]
				{
					'-'
				});
				foreach (string s2 in array3)
				{
					int num2 = 1;
					Parser.IntTryParse(s2, out num2);
					dir = ((num2 >= 3) ? PlatformingLevelGroundMovementEnemy.Direction.Left : PlatformingLevelGroundMovementEnemy.Direction.Right);
					MountainPlatformingLevelMudman mountainPlatformingLevelMudman2 = UnityEngine.Object.Instantiate<MountainPlatformingLevelMudman>(this.smallMudman);
					mountainPlatformingLevelMudman2.Init(this.spawnPoints[num2 - 1].position, dir);
				}
				mudmanSmallIndex = (mudmanSmallIndex + 1) % mudmanSmall.Length;
			}
			mudmanSizeIndex = (mudmanSizeIndex + 1) % mudmanSize.Length;
			yield return CupheadTime.WaitForSeconds(this, this.spawnDelayRange.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x001F298B File Offset: 0x001F0D8B
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x001F299E File Offset: 0x001F0D9E
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x001F29B4 File Offset: 0x001F0DB4
	private void DrawGizmos(float a)
	{
		Gizmos.color = new Color(1f, 0f, 0f, a);
		foreach (Transform transform in this.spawnPoints)
		{
			Gizmos.DrawWireSphere(transform.position, 30f);
		}
	}

	// Token: 0x04003D94 RID: 15764
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x04003D95 RID: 15765
	[SerializeField]
	private MountainPlatformingLevelMudman bigMudman;

	// Token: 0x04003D96 RID: 15766
	[SerializeField]
	private MountainPlatformingLevelMudman smallMudman;

	// Token: 0x04003D97 RID: 15767
	[SerializeField]
	private MinMax spawnDelayRange;

	// Token: 0x04003D98 RID: 15768
	[SerializeField]
	private MinMax initialDelayRange;

	// Token: 0x04003D99 RID: 15769
	[SerializeField]
	private string mudmanSizeString;

	// Token: 0x04003D9A RID: 15770
	[SerializeField]
	private string mudmanBigSpawnString;

	// Token: 0x04003D9B RID: 15771
	[SerializeField]
	private string mudmanSmallSpawnString;
}
