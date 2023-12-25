using System;
using UnityEngine;

// Token: 0x020008CA RID: 2250
public class HarbourPlatformingLevelIcebergSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06003497 RID: 13463 RVA: 0x001E874C File Offset: 0x001E6B4C
	protected override void Start()
	{
		base.Start();
		this.spawn = this.spawnDelayString.Split(new char[]
		{
			','
		});
		this.spawnIndex = UnityEngine.Random.Range(0, this.spawn.Length);
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x001E8784 File Offset: 0x001E6B84
	protected override void Spawn()
	{
		this.spawnDelay.min = Parser.FloatParse(this.spawn[this.spawnIndex]);
		this.spawnDelay.max = Parser.FloatParse(this.spawn[this.spawnIndex]);
		int num = UnityEngine.Random.Range(0, this.icebergPrefabs.Length);
		float x = CupheadLevelCamera.Current.transform.position.x + CupheadLevelCamera.Current.Width / 2f + (this.icebergPrefabs[num].GetComponent<Renderer>().bounds.size.x + this.icebergPrefabs[num].GetComponent<Renderer>().bounds.size.x / 2f);
		float y = CupheadLevelCamera.Current.transform.position.y - 100f;
		this.icebergPrefabs[num].Spawn(new Vector3(x, y));
		base.Spawn();
		this.spawnIndex = (this.spawnIndex + 1) % this.spawn.Length;
	}

	// Token: 0x04003CC1 RID: 15553
	[SerializeField]
	private HarbourPlatformingLevelIceberg[] icebergPrefabs;

	// Token: 0x04003CC2 RID: 15554
	[SerializeField]
	private string spawnDelayString = "5.5,7.0";

	// Token: 0x04003CC3 RID: 15555
	private string[] spawn;

	// Token: 0x04003CC4 RID: 15556
	private int spawnIndex;
}
