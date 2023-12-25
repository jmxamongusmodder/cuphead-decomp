using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200085F RID: 2143
public class PlatformingLevelFloatingSpawner : AbstractPausableComponent
{
	// Token: 0x060031CB RID: 12747 RVA: 0x001D13CD File Offset: 0x001CF7CD
	private void Start()
	{
		base.Awake();
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x001D13E4 File Offset: 0x001CF7E4
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
			spawnPos.x += UnityEngine.Random.Range(-this.xRange, this.xRange);
			if (CupheadLevelCamera.Current.ContainsPoint(spawnPos, new Vector2(0f, 1000f)))
			{
				PlatformingLevelGroundMovementEnemy platformingLevelGroundMovementEnemy = this.enemyPrefab.Spawn(spawnPos, (!MathUtils.RandomBool()) ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left, true);
				platformingLevelGroundMovementEnemy.Float(true);
				hashadSuccessfulSpawn = true;
			}
		}
		yield break;
	}

	// Token: 0x060031CD RID: 12749 RVA: 0x001D13FF File Offset: 0x001CF7FF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.enemyPrefab = null;
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x001D140E File Offset: 0x001CF80E
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x001D1421 File Offset: 0x001CF821
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x001D1434 File Offset: 0x001CF834
	private void DrawGizmos(float a)
	{
		Gizmos.color = new Color(1f, 1f, 0f, a);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x04003A29 RID: 14889
	[SerializeField]
	private PlatformingLevelGroundMovementEnemy enemyPrefab;

	// Token: 0x04003A2A RID: 14890
	[SerializeField]
	private float xRange;

	// Token: 0x04003A2B RID: 14891
	[SerializeField]
	private MinMax initialSpawnTime;

	// Token: 0x04003A2C RID: 14892
	[SerializeField]
	private MinMax spawnTime;
}
