using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007D1 RID: 2001
public class SaltbakerLevelPhaseOneProjectile : BasicProjectile
{
	// Token: 0x06002D6C RID: 11628 RVA: 0x001A8604 File Offset: 0x001A6A04
	protected override void Start()
	{
		base.Start();
		this.createSparks = true;
		base.StartCoroutine(this.spawn_sparkles_cr());
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x001A8620 File Offset: 0x001A6A20
	protected virtual bool SparksFollow()
	{
		return false;
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x001A8624 File Offset: 0x001A6A24
	private IEnumerator spawn_sparkles_cr()
	{
		this.sparkAngle = (float)UnityEngine.Random.Range(0, 360);
		while (this.createSparks)
		{
			yield return CupheadTime.WaitForSeconds(this, this.sparkSpawnDelay);
			int count = 1;
			if (this.sparkSpawnDelay < CupheadTime.Delta)
			{
				count = (int)(CupheadTime.Delta / this.sparkSpawnDelay);
			}
			for (int i = 0; i < count; i++)
			{
				Effect effect = this.sparkEffect.Create(base.transform.position + MathUtils.AngleToDirection(this.sparkAngle) * this.sparkDistanceRange.RandomFloat());
				if (this.SparksFollow())
				{
					effect.transform.parent = base.transform;
				}
				this.sparkAngle = (this.sparkAngle + this.sparkAngleShiftRange.RandomFloat()) % 360f;
			}
		}
		yield break;
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x001A8640 File Offset: 0x001A6A40
	protected void HandleShadow(float heightOffset, float shadowPosOffset)
	{
		this.shadow.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground + shadowPosOffset);
		float t = Mathf.InverseLerp(this.shadowScaleHeightRange.max, this.shadowScaleHeightRange.min, base.transform.position.y - heightOffset - (float)Level.Current.Ground);
		this.shadow.transform.eulerAngles = Vector3.zero;
		this.shadow.transform.localScale = Vector3.Lerp(new Vector3(0.25f, 0.25f), new Vector3(1f, 1f), t);
		this.shadow.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.25f, 1f, t));
	}

	// Token: 0x040035EF RID: 13807
	[SerializeField]
	protected SpriteRenderer shadow;

	// Token: 0x040035F0 RID: 13808
	[SerializeField]
	protected MinMax shadowScaleHeightRange = new MinMax(100f, 500f);

	// Token: 0x040035F1 RID: 13809
	[SerializeField]
	protected Effect sparkEffect;

	// Token: 0x040035F2 RID: 13810
	[SerializeField]
	private float sparkSpawnDelay = 0.15f;

	// Token: 0x040035F3 RID: 13811
	[SerializeField]
	private MinMax sparkAngleShiftRange = new MinMax(60f, 300f);

	// Token: 0x040035F4 RID: 13812
	[SerializeField]
	private MinMax sparkDistanceRange = new MinMax(0f, 20f);

	// Token: 0x040035F5 RID: 13813
	private float sparkAngle;

	// Token: 0x040035F6 RID: 13814
	protected bool createSparks = true;
}
