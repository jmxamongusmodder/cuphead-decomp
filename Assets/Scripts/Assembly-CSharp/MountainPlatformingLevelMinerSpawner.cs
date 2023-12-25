using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E7 RID: 2279
public class MountainPlatformingLevelMinerSpawner : AbstractPausableComponent
{
	// Token: 0x06003566 RID: 13670 RVA: 0x001F211D File Offset: 0x001F051D
	private void Start()
	{
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x06003567 RID: 13671 RVA: 0x001F212C File Offset: 0x001F052C
	private IEnumerator spawn_cr()
	{
		for (;;)
		{
			Vector2 spawnPos = base.transform.position;
			spawnPos.x += UnityEngine.Random.Range(-this.xRange, this.xRange);
			if (this.isRespawning)
			{
				yield return CupheadTime.WaitForSeconds(this, this.deathDelayTime.RandomFloat());
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, this.spawnTime.RandomFloat());
			}
			this.enemySpawned = this.enemyPrefab.Spawn(spawnPos, (!MathUtils.RandomBool()) ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left, false);
			this.enemySpawned.Float(false);
			while (this.enemySpawned != null)
			{
				yield return null;
			}
			this.isRespawning = true;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003568 RID: 13672 RVA: 0x001F2147 File Offset: 0x001F0547
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003569 RID: 13673 RVA: 0x001F215A File Offset: 0x001F055A
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x0600356A RID: 13674 RVA: 0x001F2170 File Offset: 0x001F0570
	private void DrawGizmos(float a)
	{
		Gizmos.color = new Color(1f, 1f, 0f, a);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x04003D89 RID: 15753
	[SerializeField]
	private PlatformingLevelGroundMovementEnemy enemyPrefab;

	// Token: 0x04003D8A RID: 15754
	private PlatformingLevelGroundMovementEnemy enemySpawned;

	// Token: 0x04003D8B RID: 15755
	[SerializeField]
	private float xRange;

	// Token: 0x04003D8C RID: 15756
	[SerializeField]
	private MinMax deathDelayTime;

	// Token: 0x04003D8D RID: 15757
	[SerializeField]
	private MinMax spawnTime;

	// Token: 0x04003D8E RID: 15758
	private bool isRespawning;
}
