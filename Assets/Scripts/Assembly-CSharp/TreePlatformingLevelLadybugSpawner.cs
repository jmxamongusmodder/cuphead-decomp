using System;
using UnityEngine;

// Token: 0x02000890 RID: 2192
public class TreePlatformingLevelLadybugSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x060032F6 RID: 13046 RVA: 0x001D9D2C File Offset: 0x001D812C
	protected override void Start()
	{
		base.Start();
		this.typeIndex = UnityEngine.Random.Range(0, this.typeString.Split(new char[]
		{
			','
		}).Length);
		this.sideIndex = UnityEngine.Random.Range(0, this.sideString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x001D9D88 File Offset: 0x001D8188
	protected override void Spawn()
	{
		PlatformingLevelGroundMovementEnemy.Direction dir = PlatformingLevelGroundMovementEnemy.Direction.Right;
		TreePlatformingLevelLadyBug.Type type = TreePlatformingLevelLadyBug.Type.BounceFast;
		if (this.sideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'R')
		{
			this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMin - 50f;
			dir = PlatformingLevelGroundMovementEnemy.Direction.Right;
		}
		else if (this.sideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'L')
		{
			this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
			dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
		}
		string text = this.typeString.Split(new char[]
		{
			','
		})[this.typeIndex];
		if (text != null)
		{
			if (!(text == "BS"))
			{
				if (!(text == "BF"))
				{
					if (!(text == "GS"))
					{
						if (!(text == "GF"))
						{
							if (text == "P")
							{
								type = TreePlatformingLevelLadyBug.Type.BouncePink;
								this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
								dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
							}
						}
						else
						{
							type = TreePlatformingLevelLadyBug.Type.GroundFast;
						}
					}
					else
					{
						type = TreePlatformingLevelLadyBug.Type.GroundSlow;
					}
				}
				else
				{
					type = TreePlatformingLevelLadyBug.Type.BounceFast;
					this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
					dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
				}
			}
			else
			{
				type = TreePlatformingLevelLadyBug.Type.BounceSlow;
				this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
				dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
			}
		}
		this.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y;
		this.ladybugPrefab.Spawn(this.spawnPosition, dir, true, type);
		this.typeIndex = (this.typeIndex + 1) % this.typeString.Split(new char[]
		{
			','
		}).Length;
		this.sideIndex = (this.sideIndex + 1) % this.sideString.Split(new char[]
		{
			','
		}).Length;
		base.Spawn();
	}

	// Token: 0x04003B1A RID: 15130
	[SerializeField]
	private TreePlatformingLevelLadyBug ladybugPrefab;

	// Token: 0x04003B1B RID: 15131
	[SerializeField]
	private string typeString;

	// Token: 0x04003B1C RID: 15132
	[SerializeField]
	private string sideString;

	// Token: 0x04003B1D RID: 15133
	private int typeIndex;

	// Token: 0x04003B1E RID: 15134
	private int sideIndex;

	// Token: 0x04003B1F RID: 15135
	private Vector3 spawnPosition;
}
