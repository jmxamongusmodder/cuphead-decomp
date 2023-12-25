using System;
using UnityEngine;

// Token: 0x0200086C RID: 2156
public class PlatformingLevelPathMovementEnemySpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06003227 RID: 12839 RVA: 0x001D46A5 File Offset: 0x001D2AA5
	protected override void Spawn()
	{
		this.enemyPrefab.Spawn(base.transform.position, this.path, this.startPosition, this.destroyEnemyAfterLeavingScreen);
	}

	// Token: 0x06003228 RID: 12840 RVA: 0x001D46D0 File Offset: 0x001D2AD0
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x001D46E3 File Offset: 0x001D2AE3
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x0600322A RID: 12842 RVA: 0x001D46F8 File Offset: 0x001D2AF8
	private void DrawGizmos(float a)
	{
		this.path.DrawGizmos(a, base.baseTransform.position);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawSphere(this.path.Lerp(this.startPosition) + base.baseTransform.position, 10f);
		Gizmos.DrawWireSphere(this.path.Lerp(this.startPosition) + base.baseTransform.position, 11f);
	}

	// Token: 0x04003A81 RID: 14977
	public PlatformingLevelPathMovementEnemy enemyPrefab;

	// Token: 0x04003A82 RID: 14978
	[Header("Path")]
	public float startPosition = 0.5f;

	// Token: 0x04003A83 RID: 14979
	public PlatformingLevelPathMovementEnemy.Direction direction = PlatformingLevelPathMovementEnemy.Direction.Forward;

	// Token: 0x04003A84 RID: 14980
	public VectorPath path;
}
