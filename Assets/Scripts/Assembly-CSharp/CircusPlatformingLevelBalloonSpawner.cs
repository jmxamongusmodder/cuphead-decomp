using System;
using UnityEngine;

// Token: 0x0200089B RID: 2203
public class CircusPlatformingLevelBalloonSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06003349 RID: 13129 RVA: 0x001DD97C File Offset: 0x001DBD7C
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
		this.pinkSplits = this.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = UnityEngine.Random.Range(0, this.pinkSplits.Length);
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x001DDA2C File Offset: 0x001DBE2C
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
		CircusPlatformingLevelBalloon circusPlatformingLevelBalloon = UnityEngine.Object.Instantiate<CircusPlatformingLevelBalloon>(this.balloonPrefab);
		circusPlatformingLevelBalloon.Init(this.spawnPosition, this.rotation, this.spreadCount, this.pinkSplits[this.pinkIndex]);
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
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkSplits.Length;
	}

	// Token: 0x04003B8B RID: 15243
	[SerializeField]
	private CircusPlatformingLevelBalloon balloonPrefab;

	// Token: 0x04003B8C RID: 15244
	[SerializeField]
	private string spawnDelayString;

	// Token: 0x04003B8D RID: 15245
	[SerializeField]
	private string spawnPositionString;

	// Token: 0x04003B8E RID: 15246
	[SerializeField]
	private string spawnSideString;

	// Token: 0x04003B8F RID: 15247
	[SerializeField]
	private string spreadCount;

	// Token: 0x04003B90 RID: 15248
	[SerializeField]
	private string pinkString;

	// Token: 0x04003B91 RID: 15249
	private float rotation;

	// Token: 0x04003B92 RID: 15250
	private int delayIndex;

	// Token: 0x04003B93 RID: 15251
	private int posIndex;

	// Token: 0x04003B94 RID: 15252
	private int sideIndex;

	// Token: 0x04003B95 RID: 15253
	private Vector3 spawnPosition;

	// Token: 0x04003B96 RID: 15254
	private string[] pinkSplits;

	// Token: 0x04003B97 RID: 15255
	private int pinkIndex;
}
