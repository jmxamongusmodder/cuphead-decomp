using System;
using UnityEngine;

// Token: 0x0200087A RID: 2170
public class ForestPlatformingLevelAcornSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06003266 RID: 12902 RVA: 0x001D58B0 File Offset: 0x001D3CB0
	protected override void Awake()
	{
		base.Awake();
		this.leftRightIndex = UnityEngine.Random.Range(0, this.leftRightString.Length);
		this.yPattern = this.yString.Split(new char[]
		{
			','
		});
		this.yIndex = UnityEngine.Random.Range(0, this.yPattern.Length);
	}

	// Token: 0x06003267 RID: 12903 RVA: 0x001D590C File Offset: 0x001D3D0C
	protected override void Spawn()
	{
		this.leftRightIndex = (this.leftRightIndex + 1) % this.leftRightString.Length;
		ForestPlatformingLevelAcorn.Direction direction = (this.leftRightString[this.leftRightIndex] != 'L') ? ForestPlatformingLevelAcorn.Direction.Right : ForestPlatformingLevelAcorn.Direction.Left;
		this.yIndex = (this.yIndex + 1) % this.yPattern.Length;
		float num = 0f;
		Parser.FloatTryParse(this.yPattern[this.yIndex], out num);
		Vector2 position = new Vector2((direction != ForestPlatformingLevelAcorn.Direction.Left) ? (CupheadLevelCamera.Current.Bounds.xMin - 50f) : (CupheadLevelCamera.Current.Bounds.xMax + 50f), CupheadLevelCamera.Current.Bounds.yMax - num);
		this.enemyPrefab.Spawn(position, direction, false);
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x001D59EC File Offset: 0x001D3DEC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.enemyPrefab = null;
	}

	// Token: 0x04003AC5 RID: 15045
	public ForestPlatformingLevelAcorn enemyPrefab;

	// Token: 0x04003AC6 RID: 15046
	public string leftRightString = "LR";

	// Token: 0x04003AC7 RID: 15047
	public string yString = "150,50";

	// Token: 0x04003AC8 RID: 15048
	private int leftRightIndex;

	// Token: 0x04003AC9 RID: 15049
	private int yIndex;

	// Token: 0x04003ACA RID: 15050
	private string[] yPattern;
}
