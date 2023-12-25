using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200088B RID: 2187
public class TreePlatformingLevelDragonflyProjectile : BasicProjectile
{
	// Token: 0x060032DF RID: 13023 RVA: 0x001D9534 File Offset: 0x001D7934
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.bullet_trail_cr());
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x001D954C File Offset: 0x001D794C
	private IEnumerator bullet_trail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.16f, 0.2f));
			Effect e = this.bulletFX.Create(base.transform.position);
			SpriteRenderer r = e.GetComponent<SpriteRenderer>();
			r.sortingOrder = -1;
			r.sortingLayerName = "Projectiles";
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003B0D RID: 15117
	private const string ProjectilesLayerName = "Projectiles";

	// Token: 0x04003B0E RID: 15118
	[SerializeField]
	private Effect bulletFX;
}
