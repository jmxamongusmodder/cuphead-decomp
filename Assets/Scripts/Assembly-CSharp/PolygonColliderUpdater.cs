using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EB RID: 1003
public class PolygonColliderUpdater : AbstractMonoBehaviour
{
	// Token: 0x06000D8A RID: 3466 RVA: 0x0008E1C8 File Offset: 0x0008C5C8
	protected override void Awake()
	{
		base.Awake();
		this.colliders = new Dictionary<string, PolygonCollider2D>();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.sprite = null;
		if (base.GetComponent<Collider2D>() != null)
		{
			UnityEngine.Object.Destroy(base.GetComponent<Collider2D>());
		}
		base.StartCoroutine(this.collider_cr());
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x0008E224 File Offset: 0x0008C624
	private IEnumerator collider_cr()
	{
		for (;;)
		{
			if (this.spriteRenderer.sprite != this.sprite)
			{
				this.sprite = this.spriteRenderer.sprite;
				foreach (PolygonCollider2D polygonCollider2D in base.GetComponents<PolygonCollider2D>())
				{
					polygonCollider2D.enabled = false;
				}
				if (this.colliders.ContainsKey(this.sprite.name))
				{
					this.colliders[this.sprite.name].enabled = true;
				}
				else
				{
					this.colliders[this.sprite.name] = base.gameObject.AddComponent<PolygonCollider2D>();
					this.colliders[this.sprite.name].isTrigger = true;
				}
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x04001703 RID: 5891
	private SpriteRenderer spriteRenderer;

	// Token: 0x04001704 RID: 5892
	private Sprite sprite;

	// Token: 0x04001705 RID: 5893
	private Dictionary<string, PolygonCollider2D> colliders;
}
