using System;
using UnityEngine;

// Token: 0x02000A42 RID: 2626
public class PlayerScreenEffectController : AbstractMonoBehaviour
{
	// Token: 0x06003E9C RID: 16028 RVA: 0x00225A88 File Offset: 0x00223E88
	private void Update()
	{
		if (!this.dontCenter)
		{
			this.UpdateToCamera();
		}
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x00225A9B File Offset: 0x00223E9B
	private void LateUpdate()
	{
		if (!this.dontCenter)
		{
			this.UpdateToCamera();
		}
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x00225AB0 File Offset: 0x00223EB0
	private void UpdateToCamera()
	{
		Camera main = Camera.main;
		Transform transform = main.transform;
		base.transform.position = transform.position;
		base.transform.localScale = Vector3.one * (main.orthographicSize / 360f);
		base.transform.rotation = transform.rotation;
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x00225B0D File Offset: 0x00223F0D
	public void SetSpriteLayer(int index, SpriteLayer layer)
	{
		this.spriteRenderers[index].sortingLayerName = layer.ToString();
	}

	// Token: 0x06003EA0 RID: 16032 RVA: 0x00225B29 File Offset: 0x00223F29
	public void SetSpriteOrder(int index, int order)
	{
		this.spriteRenderers[index].sortingOrder = order;
	}

	// Token: 0x06003EA1 RID: 16033 RVA: 0x00225B3C File Offset: 0x00223F3C
	public void ResetSprites()
	{
		for (int i = 0; i < this.spriteRenderers.Length; i++)
		{
			this.spriteRenderers[i].sortingOrder = -2010 - i;
			this.spriteRenderers[i].sortingLayerName = "Player";
			this.spriteRenderers[i].sprite = null;
		}
	}

	// Token: 0x040045AF RID: 17839
	[SerializeField]
	private bool dontCenter;

	// Token: 0x040045B0 RID: 17840
	[SerializeField]
	private SpriteRenderer[] spriteRenderers;
}
