using System;
using UnityEngine;

// Token: 0x020008C8 RID: 2248
public class HarbourPlatformingLevelFishSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x0600348D RID: 13453 RVA: 0x001E8358 File Offset: 0x001E6758
	protected override void Start()
	{
		base.Start();
		this.delayIndex = UnityEngine.Random.Range(0, this.spawnDelayString.Split(new char[]
		{
			','
		}).Length);
		this.posIndex = UnityEngine.Random.Range(0, this.spawnPositionString.Split(new char[]
		{
			','
		}).Length);
		this.sideIndex = UnityEngine.Random.Range(0, this.spawnSideString.Split(new char[]
		{
			','
		}).Length);
		this.typeIndex = UnityEngine.Random.Range(0, this.typeString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x001E83FC File Offset: 0x001E67FC
	protected override void Spawn()
	{
		base.Spawn();
		this.spawnDelay.min = Parser.FloatParse(this.spawnDelayString.Split(new char[]
		{
			','
		})[this.delayIndex]);
		this.spawnDelay.max = Parser.FloatParse(this.spawnDelayString.Split(new char[]
		{
			','
		})[this.delayIndex]);
		if (this.spawnSideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'L')
		{
			this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMin - 50f;
			this.rotation = 0f;
		}
		else if (this.spawnSideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'R')
		{
			this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
			this.rotation = 180f;
		}
		this.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y + Parser.FloatParse(this.spawnPositionString.Split(new char[]
		{
			','
		})[this.posIndex]);
		HarbourPlatformingLevelFish harbourPlatformingLevelFish = UnityEngine.Object.Instantiate<HarbourPlatformingLevelFish>(this.fishPrefab);
		harbourPlatformingLevelFish.Init(this.spawnPosition, this.rotation, this.typeString.Split(new char[]
		{
			','
		})[this.typeIndex]);
		this.sideIndex = (this.sideIndex + 1) % this.spawnSideString.Split(new char[]
		{
			','
		}).Length;
		this.posIndex = (this.posIndex + 1) % this.spawnPositionString.Split(new char[]
		{
			','
		}).Length;
		this.delayIndex = (this.delayIndex + 1) % this.spawnDelayString.Split(new char[]
		{
			','
		}).Length;
		this.typeIndex = (this.typeIndex + 1) % this.typeString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x04003CB0 RID: 15536
	[SerializeField]
	private HarbourPlatformingLevelFish fishPrefab;

	// Token: 0x04003CB1 RID: 15537
	[SerializeField]
	private string spawnDelayString;

	// Token: 0x04003CB2 RID: 15538
	[SerializeField]
	private string spawnPositionString;

	// Token: 0x04003CB3 RID: 15539
	[SerializeField]
	private string spawnSideString;

	// Token: 0x04003CB4 RID: 15540
	[SerializeField]
	private string typeString;

	// Token: 0x04003CB5 RID: 15541
	[SerializeField]
	private float movementSpeed;

	// Token: 0x04003CB6 RID: 15542
	[SerializeField]
	private float sineSpeed;

	// Token: 0x04003CB7 RID: 15543
	[SerializeField]
	private float sineSize;

	// Token: 0x04003CB8 RID: 15544
	private float rotation;

	// Token: 0x04003CB9 RID: 15545
	private int delayIndex;

	// Token: 0x04003CBA RID: 15546
	private int posIndex;

	// Token: 0x04003CBB RID: 15547
	private int sideIndex;

	// Token: 0x04003CBC RID: 15548
	private int typeIndex;

	// Token: 0x04003CBD RID: 15549
	private Vector3 spawnPosition;
}
