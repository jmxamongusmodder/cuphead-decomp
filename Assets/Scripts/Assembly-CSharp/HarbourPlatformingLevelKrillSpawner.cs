using System;
using UnityEngine;

// Token: 0x020008CC RID: 2252
public class HarbourPlatformingLevelKrillSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x060034A1 RID: 13473 RVA: 0x001E8B14 File Offset: 0x001E6F14
	protected override void Start()
	{
		base.Start();
		this.posIndex = UnityEngine.Random.Range(0, this.posString.Split(new char[]
		{
			','
		}).Length);
		this.typeIndex = UnityEngine.Random.Range(0, this.typeString.Split(new char[]
		{
			','
		}).Length);
		this.delayIndex = UnityEngine.Random.Range(0, this.delayString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x001E8B94 File Offset: 0x001E6F94
	protected override void Spawn()
	{
		base.Spawn();
		this.spawnDelay.min = Parser.FloatParse(this.delayString.Split(new char[]
		{
			','
		})[this.delayIndex]);
		this.spawnDelay.max = Parser.FloatParse(this.delayString.Split(new char[]
		{
			','
		})[this.delayIndex]);
		Vector2 v = CupheadLevelCamera.Current.transform.position;
		v.x = CupheadLevelCamera.Current.transform.position.x + (float)Parser.IntParse(this.posString.Split(new char[]
		{
			','
		})[this.posIndex]);
		v.y = CupheadLevelCamera.Current.Bounds.yMin - 50f;
		this.parryable = (this.typeString.Split(new char[]
		{
			','
		})[this.typeIndex][0] == 'A');
		HarbourPlatformingLevelKrill harbourPlatformingLevelKrill = this.krillPrefab.Spawn(null, v);
		harbourPlatformingLevelKrill.isParryable = this.parryable;
		harbourPlatformingLevelKrill.SetType(this.typeString.Split(new char[]
		{
			','
		})[this.typeIndex]);
		this.posIndex = (this.posIndex + 1) % this.posString.Split(new char[]
		{
			','
		}).Length;
		this.typeIndex = (this.typeIndex + 1) % this.typeString.Split(new char[]
		{
			','
		}).Length;
		this.delayIndex = (this.delayIndex + 1) % this.delayString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x04003CC8 RID: 15560
	[SerializeField]
	private HarbourPlatformingLevelKrill krillPrefab;

	// Token: 0x04003CC9 RID: 15561
	[SerializeField]
	private string posString = "305,640,356";

	// Token: 0x04003CCA RID: 15562
	[SerializeField]
	private string typeString;

	// Token: 0x04003CCB RID: 15563
	[SerializeField]
	private string delayString;

	// Token: 0x04003CCC RID: 15564
	private int posIndex;

	// Token: 0x04003CCD RID: 15565
	private int typeIndex;

	// Token: 0x04003CCE RID: 15566
	private int delayIndex;

	// Token: 0x04003CCF RID: 15567
	private bool parryable;
}
