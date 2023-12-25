using System;
using UnityEngine;

// Token: 0x020008AC RID: 2220
public class CircusPlatformingLevelPretzelSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x060033BD RID: 13245 RVA: 0x001E0920 File Offset: 0x001DED20
	protected override void Start()
	{
		base.Start();
		this.sideIndex = UnityEngine.Random.Range(0, this.sideString.Split(new char[]
		{
			','
		}).Length);
		if (this.path == null || this.path.Length == 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x001E097C File Offset: 0x001DED7C
	protected override void Spawn()
	{
		base.Spawn();
		bool goingLeft = false;
		int startPosition = -1;
		if (this.sideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'L')
		{
			goingLeft = true;
			startPosition = this.path.Length - 1;
			for (int i = 0; i < this.path.Length; i++)
			{
				if (this.path[i].position.x > CupheadLevelCamera.Current.Bounds.xMax + 100f)
				{
					startPosition = i;
					break;
				}
			}
		}
		else if (this.sideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'R')
		{
			startPosition = 0;
			for (int j = this.path.Length - 1; j >= 0; j--)
			{
				if (this.path[j].position.x < CupheadLevelCamera.Current.Bounds.xMin - 100f)
				{
					startPosition = j;
					break;
				}
			}
			goingLeft = false;
		}
		this.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y;
		CircusPlatformingLevelPretzel circusPlatformingLevelPretzel = this.pretzelPrefab.Spawn<CircusPlatformingLevelPretzel>();
		circusPlatformingLevelPretzel.SetPath(this.path);
		circusPlatformingLevelPretzel.goingLeft = goingLeft;
		circusPlatformingLevelPretzel.SetStartPosition(startPosition);
		this.sideIndex = (this.sideIndex + 1) % this.sideString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x04003C0A RID: 15370
	[SerializeField]
	private CircusPlatformingLevelPretzel pretzelPrefab;

	// Token: 0x04003C0B RID: 15371
	[SerializeField]
	private string sideString;

	// Token: 0x04003C0C RID: 15372
	[SerializeField]
	private Transform[] path;

	// Token: 0x04003C0D RID: 15373
	private int sideIndex;

	// Token: 0x04003C0E RID: 15374
	private Vector3 spawnPosition;
}
