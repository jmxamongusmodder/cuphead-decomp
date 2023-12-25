using System;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public class TreePlatformingLevelDragonflyProjectileHandler : PlatformingLevelEnemySpawner
{
	// Token: 0x060032E2 RID: 13026 RVA: 0x001D9684 File Offset: 0x001D7A84
	protected override void Start()
	{
		base.Start();
		this.dragonflyShots = new TreePlatformingLevelDragonflyShot[base.GetComponentsInChildren<TreePlatformingLevelDragonflyShot>().Length];
		this.dragonflyShots = base.GetComponentsInChildren<TreePlatformingLevelDragonflyShot>();
		this.spawnIndex = UnityEngine.Random.Range(0, this.delaySpawnString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x001D96DC File Offset: 0x001D7ADC
	protected override void Spawn()
	{
		this.spawnDelay.min = Parser.FloatParse(this.delaySpawnString.Split(new char[]
		{
			','
		})[this.spawnIndex]);
		this.spawnDelay.max = Parser.FloatParse(this.delaySpawnString.Split(new char[]
		{
			','
		})[this.spawnIndex]);
		this.Activate();
		base.Spawn();
		this.spawnIndex = (this.spawnIndex + 1) % this.delaySpawnString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x060032E4 RID: 13028 RVA: 0x001D9778 File Offset: 0x001D7B78
	private void Activate()
	{
		int num = UnityEngine.Random.Range(0, this.dragonflyShots.Length);
		if (!this.dragonflyShots[num].isActivated)
		{
			this.dragonflyShots[num].Activate();
		}
	}

	// Token: 0x04003B0F RID: 15119
	[SerializeField]
	private string delaySpawnString;

	// Token: 0x04003B10 RID: 15120
	private TreePlatformingLevelDragonflyShot[] dragonflyShots;

	// Token: 0x04003B11 RID: 15121
	private int spawnIndex;
}
