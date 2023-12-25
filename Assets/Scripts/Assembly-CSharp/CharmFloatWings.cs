using System;
using UnityEngine;

// Token: 0x02000A44 RID: 2628
public class CharmFloatWings : MonoBehaviour
{
	// Token: 0x06003EAB RID: 16043 RVA: 0x00225D5C File Offset: 0x0022415C
	private void Start()
	{
		if (this.useWindEffect)
		{
			SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer spriteRenderer in componentsInChildren)
			{
				spriteRenderer.enabled = false;
			}
		}
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x00225D9C File Offset: 0x0022419C
	private void OnEnable()
	{
		if (!this.useWindEffect)
		{
			this.SpawnFeathers();
		}
	}

	// Token: 0x06003EAD RID: 16045 RVA: 0x00225DAF File Offset: 0x002241AF
	private void OnDisable()
	{
		if (!this.useWindEffect)
		{
			this.SpawnFeathers();
		}
	}

	// Token: 0x06003EAE RID: 16046 RVA: 0x00225DC4 File Offset: 0x002241C4
	private void SpawnFeathers()
	{
		for (int i = 0; i < 10; i++)
		{
			this.feather.Create(base.transform.position, new Vector3(UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f)));
		}
	}

	// Token: 0x06003EAF RID: 16047 RVA: 0x00225E20 File Offset: 0x00224220
	private void Update()
	{
		if (this.useWindEffect)
		{
			this.spawnTime += CupheadTime.Delta;
			if (this.spawnTime > this.spawnFreq)
			{
				this.spawnTime -= this.spawnFreq;
				this.featherAlt.Create(base.transform.parent.position + Vector3.down * 10f);
			}
		}
	}

	// Token: 0x040045B3 RID: 17843
	[SerializeField]
	private Effect feather;

	// Token: 0x040045B4 RID: 17844
	[SerializeField]
	private Effect featherAlt;

	// Token: 0x040045B5 RID: 17845
	[SerializeField]
	private bool useWindEffect;

	// Token: 0x040045B6 RID: 17846
	private float spawnFreq = 0.1f;

	// Token: 0x040045B7 RID: 17847
	private float spawnTime;
}
