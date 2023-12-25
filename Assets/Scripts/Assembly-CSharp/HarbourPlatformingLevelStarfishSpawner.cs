using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
public class HarbourPlatformingLevelStarfishSpawner : AbstractPausableComponent
{
	// Token: 0x060034E0 RID: 13536 RVA: 0x001EBFDC File Offset: 0x001EA3DC
	private void Start()
	{
		this.speedX = this.speedXString.Split(new char[]
		{
			','
		});
		this.speedXIndex = UnityEngine.Random.Range(0, this.speedX.Length);
		this.typeIndex = UnityEngine.Random.Range(0, this.typeString.Split(new char[]
		{
			','
		}).Length);
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x001EC04C File Offset: 0x001EA44C
	private IEnumerator spawn_cr()
	{
		bool hashadSuccessfulSpawn = false;
		for (;;)
		{
			if (hashadSuccessfulSpawn)
			{
				yield return CupheadTime.WaitForSeconds(this, this.spawnTime.RandomFloat());
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, this.initialSpawnTime.RandomFloat());
			}
			Vector2 spawnPos = base.transform.position;
			spawnPos.y = base.transform.position.y;
			spawnPos.x += UnityEngine.Random.Range(-this.xRange, this.xRange);
			if (CupheadLevelCamera.Current.ContainsPoint(spawnPos, new Vector2(0f, 1000f)))
			{
				this.starfishPrefab.Spawn(spawnPos).Init(90f, Parser.FloatParse(this.speedX[this.speedXIndex]), this.speedYRange.RandomFloat(), this.loopSize, this.typeString.Split(new char[]
				{
					','
				})[this.typeIndex]);
				hashadSuccessfulSpawn = true;
				this.speedXIndex = (this.speedXIndex + 1) % this.speedX.Length;
				this.typeIndex = (this.typeIndex + 1) % this.typeString.Split(new char[]
				{
					','
				}).Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x001EC068 File Offset: 0x001EA468
	protected override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x04003D0A RID: 15626
	[SerializeField]
	private MinMax initialSpawnTime;

	// Token: 0x04003D0B RID: 15627
	[SerializeField]
	private MinMax spawnTime;

	// Token: 0x04003D0C RID: 15628
	[SerializeField]
	private HarbourPlatformingLevelStarfish starfishPrefab;

	// Token: 0x04003D0D RID: 15629
	[SerializeField]
	private string speedXString;

	// Token: 0x04003D0E RID: 15630
	[SerializeField]
	private string typeString;

	// Token: 0x04003D0F RID: 15631
	[SerializeField]
	private MinMax speedYRange;

	// Token: 0x04003D10 RID: 15632
	[SerializeField]
	private float xRange;

	// Token: 0x04003D11 RID: 15633
	[SerializeField]
	private float loopSize;

	// Token: 0x04003D12 RID: 15634
	private int typeIndex;

	// Token: 0x04003D13 RID: 15635
	private int speedXIndex;

	// Token: 0x04003D14 RID: 15636
	private string[] speedX;
}
