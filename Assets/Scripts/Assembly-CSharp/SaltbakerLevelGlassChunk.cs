using System;
using UnityEngine;

// Token: 0x020007C8 RID: 1992
public class SaltbakerLevelGlassChunk : AbstractMonoBehaviour
{
	// Token: 0x06002D2F RID: 11567 RVA: 0x001A9CB8 File Offset: 0x001A80B8
	public void Reset(Vector3 pos, float fallSpeed, bool isChunk, bool flip, bool reverse, bool inBack, int variant)
	{
		base.transform.position = pos;
		this.fallSpeed = fallSpeed;
		base.animator.SetFloat("Reverse", (float)((!reverse || isChunk) ? 1 : -1));
		base.animator.Play(((!isChunk) ? "Bit" : "Chunk") + variant.ToString(), 0, (float)UnityEngine.Random.Range(0, 1));
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)((!isChunk) ? UnityEngine.Random.Range(-30, 30) : 0));
		base.transform.localScale = new Vector3((float)((!flip) ? 1 : -1), 1f);
		foreach (SpriteRenderer spriteRenderer in this.rend)
		{
			spriteRenderer.sortingLayerName = ((!inBack) ? "Foreground" : "Background");
			spriteRenderer.color = ((!inBack) ? Color.white : new Color(0.7f, 0.7f, 0.7f, 1f));
		}
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x001A9DF7 File Offset: 0x001A81F7
	private void Update()
	{
		base.transform.position += Vector3.down * this.fallSpeed * CupheadTime.Delta;
	}

	// Token: 0x040035A7 RID: 13735
	private float fallSpeed;

	// Token: 0x040035A8 RID: 13736
	[SerializeField]
	private SpriteRenderer[] rend;
}
