using System;
using UnityEngine;

// Token: 0x020008BD RID: 2237
public class FunhousePlatformingLevelRocketSpawner : PlatformingLevelGroundMovementEnemySpawner
{
	// Token: 0x06003435 RID: 13365 RVA: 0x001E49F4 File Offset: 0x001E2DF4
	protected override void Start()
	{
		base.Start();
		this.topBottomIndex = UnityEngine.Random.Range(0, this.topBottomString.Split(new char[]
		{
			','
		}).Length);
		this.directionPattern = this.patternString.Split(new char[]
		{
			','
		});
		this.directionIndex = UnityEngine.Random.Range(0, this.directionPattern.Length);
	}

	// Token: 0x06003436 RID: 13366 RVA: 0x001E4A5C File Offset: 0x001E2E5C
	protected override void Spawn()
	{
		if (this.topBottomString.Split(new char[]
		{
			','
		})[this.topBottomIndex][0] == 'T')
		{
			this.isTop = true;
		}
		else if (this.topBottomString.Split(new char[]
		{
			','
		})[this.topBottomIndex][0] == 'B')
		{
			this.isTop = false;
		}
		bool flag = (!this.chooseSideRandomly) ? (this.directionPattern[this.directionIndex] == "R") : Rand.Bool();
		this.directionIndex = (this.directionIndex + 1) % this.directionPattern.Length;
		float x = (!flag) ? (CupheadLevelCamera.Current.Bounds.xMin - 50f) : (CupheadLevelCamera.Current.Bounds.xMax + 50f);
		float y = CupheadLevelCamera.Current.transform.position.y;
		PlatformingLevelGroundMovementEnemy platformingLevelGroundMovementEnemy = this.enemyPrefab.Spawn<PlatformingLevelGroundMovementEnemy>();
		platformingLevelGroundMovementEnemy.GetComponent<FunhousePlatformingLevelRocket>().Init(new Vector2(x, y), this.isTop, flag);
		platformingLevelGroundMovementEnemy.GoToGround(true, "Idle");
		this.topBottomIndex = (this.topBottomIndex + 1) % this.topBottomString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x04003C6F RID: 15471
	private const string Right = "R";

	// Token: 0x04003C70 RID: 15472
	[SerializeField]
	private string topBottomString;

	// Token: 0x04003C71 RID: 15473
	private int topBottomIndex;

	// Token: 0x04003C72 RID: 15474
	private bool isTop;

	// Token: 0x04003C73 RID: 15475
	private string[] directionPattern;

	// Token: 0x04003C74 RID: 15476
	private int directionIndex;
}
