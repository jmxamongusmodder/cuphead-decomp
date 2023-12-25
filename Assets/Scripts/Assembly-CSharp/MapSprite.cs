using System;
using UnityEngine;

// Token: 0x02000971 RID: 2417
public class MapSprite : AbstractPausableComponent
{
	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06003849 RID: 14409 RVA: 0x00098849 File Offset: 0x00096C49
	protected virtual bool ChangesDepth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600384A RID: 14410 RVA: 0x0009884C File Offset: 0x00096C4C
	protected override void Awake()
	{
		base.Awake();
		this.SetLayer(base.GetComponent<SpriteRenderer>());
		foreach (SpriteRenderer layer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			this.SetLayer(layer);
		}
	}

	// Token: 0x0600384B RID: 14411 RVA: 0x00098891 File Offset: 0x00096C91
	protected void SetLayer(SpriteRenderer renderer)
	{
		if (!this.ChangesDepth || renderer == null)
		{
			return;
		}
		renderer.sortingLayerName = "Map";
		renderer.sortingOrder = 0;
	}

	// Token: 0x0600384C RID: 14412 RVA: 0x000988C0 File Offset: 0x00096CC0
	protected virtual void Update()
	{
		Vector3 position = base.transform.position;
		base.transform.position = new Vector3(position.x, position.y, position.y + this.zOffset);
	}

	// Token: 0x0400402C RID: 16428
	[SerializeField]
	protected float zOffset;
}
