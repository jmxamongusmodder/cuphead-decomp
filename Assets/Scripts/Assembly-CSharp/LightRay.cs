using System;
using UnityEngine;

// Token: 0x02000B12 RID: 2834
public class LightRay : AbstractMonoBehaviour
{
	// Token: 0x060044BF RID: 17599 RVA: 0x002465F8 File Offset: 0x002449F8
	private void Start()
	{
		float num = (!this.randomOffset) ? this.customOffset : UnityEngine.Random.Range(0f, 1f);
		this.t = 4f * num / this.speed;
	}

	// Token: 0x060044C0 RID: 17600 RVA: 0x00246640 File Offset: 0x00244A40
	private void Update()
	{
		if (!this.widthCached)
		{
			Texture2D texture = base.GetComponent<SpriteRenderer>().sprite.texture;
			if (texture == null)
			{
				return;
			}
			this.textureWidth = 2.3232484f / ((float)texture.width / (float)texture.height);
			this.widthCached = true;
		}
		this.accumulator += CupheadTime.Delta;
		while (this.accumulator > 0.041666668f)
		{
			this.accumulator -= 0.041666668f;
			this.t += 0.041666668f;
		}
		Material material = base.GetComponent<SpriteRenderer>().material;
		material.SetFloat("t", this.t);
		material.SetFloat("textureWidth", this.textureWidth);
		material.SetFloat("textureSpeed", this.speed);
	}

	// Token: 0x04004A76 RID: 19062
	private float t;

	// Token: 0x04004A77 RID: 19063
	private float accumulator;

	// Token: 0x04004A78 RID: 19064
	private float textureWidth;

	// Token: 0x04004A79 RID: 19065
	[SerializeField]
	private float speed = 0.03f;

	// Token: 0x04004A7A RID: 19066
	[SerializeField]
	private bool randomOffset = true;

	// Token: 0x04004A7B RID: 19067
	[SerializeField]
	[Range(0f, 1f)]
	private float customOffset = 0.5f;

	// Token: 0x04004A7C RID: 19068
	private bool widthCached;
}
