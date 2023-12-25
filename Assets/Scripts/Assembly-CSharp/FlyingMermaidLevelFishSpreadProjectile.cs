using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000689 RID: 1673
public class FlyingMermaidLevelFishSpreadProjectile : BasicProjectile
{
	// Token: 0x06002349 RID: 9033 RVA: 0x0014B4F7 File Offset: 0x001498F7
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.smoke_cr());
		base.StartCoroutine(this.layer_change_cr());
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x0014B51C File Offset: 0x0014991C
	private IEnumerator smoke_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.25f, 0.5f));
		SpriteRenderer sprite = base.GetComponentInChildren<SpriteRenderer>();
		while (!base.dead)
		{
			Effect smoke = this.smokeEffectPrefab.Create(this.smokeEffectRoot.position + MathUtils.RandomPointInUnitCircle() * 15f);
			SpriteRenderer smokeSprite = smoke.GetComponentInChildren<SpriteRenderer>();
			smokeSprite.sortingLayerID = sprite.sortingLayerID;
			smokeSprite.sortingOrder = sprite.sortingOrder - 1;
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.1f, 0.2f));
		}
		yield break;
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x0014B538 File Offset: 0x00149938
	private IEnumerator layer_change_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		SpriteRenderer sprite = base.GetComponentInChildren<SpriteRenderer>();
		sprite.sortingLayerName = "Foreground";
		sprite.sortingOrder = 30;
		yield break;
	}

	// Token: 0x04002BEA RID: 11242
	[SerializeField]
	private Effect smokeEffectPrefab;

	// Token: 0x04002BEB RID: 11243
	[SerializeField]
	private Transform smokeEffectRoot;
}
