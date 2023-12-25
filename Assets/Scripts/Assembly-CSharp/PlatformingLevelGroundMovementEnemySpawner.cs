using System;
using UnityEngine;

// Token: 0x02000868 RID: 2152
public class PlatformingLevelGroundMovementEnemySpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06003204 RID: 12804 RVA: 0x001D32F5 File Offset: 0x001D16F5
	protected override void Awake()
	{
		base.Awake();
		if (!this.chooseSideRandomly)
		{
			this.patternIndex = UnityEngine.Random.Range(0, this.patternString.Length);
		}
	}

	// Token: 0x06003205 RID: 12805 RVA: 0x001D3320 File Offset: 0x001D1720
	protected override void Spawn()
	{
		PlatformingLevelGroundMovementEnemy.Direction direction;
		if (this.chooseSideRandomly)
		{
			direction = ((!MathUtils.RandomBool()) ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left);
		}
		else
		{
			this.patternIndex = (this.patternIndex + 1) % this.patternString.Length;
			direction = ((this.patternString[this.patternIndex] != 'L') ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left);
		}
		Vector2 v = new Vector2((direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? (CupheadLevelCamera.Current.Bounds.xMin - 50f) : (CupheadLevelCamera.Current.Bounds.xMax + 50f), CupheadLevelCamera.Current.Bounds.yMax);
		PlatformingLevelGroundMovementEnemy platformingLevelGroundMovementEnemy = this.enemyPrefab.Spawn(v, direction, this.destroyEnemyAfterLeavingScreen);
		platformingLevelGroundMovementEnemy.GoToGround(true, "Run");
	}

	// Token: 0x04003A69 RID: 14953
	public PlatformingLevelGroundMovementEnemy enemyPrefab;

	// Token: 0x04003A6A RID: 14954
	public bool chooseSideRandomly = true;

	// Token: 0x04003A6B RID: 14955
	public string patternString = "LR";

	// Token: 0x04003A6C RID: 14956
	private int patternIndex;
}
