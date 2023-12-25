using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
public class FunhousePlatformingLevelJackSpawner : AbstractPausableComponent
{
	// Token: 0x0600341C RID: 13340 RVA: 0x001E41B7 File Offset: 0x001E25B7
	private void Start()
	{
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x001E41C8 File Offset: 0x001E25C8
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
				this.jackPrefab.Spawn(spawnPos).SelectDirection(this.isBottom);
				hashadSuccessfulSpawn = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x001E41E4 File Offset: 0x001E25E4
	protected override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x04003C5F RID: 15455
	[SerializeField]
	private MinMax initialSpawnTime;

	// Token: 0x04003C60 RID: 15456
	[SerializeField]
	private MinMax spawnTime;

	// Token: 0x04003C61 RID: 15457
	[SerializeField]
	private float xRange;

	// Token: 0x04003C62 RID: 15458
	[SerializeField]
	private FunhousePlatformingLevelJack jackPrefab;

	// Token: 0x04003C63 RID: 15459
	[SerializeField]
	private bool isBottom;
}
