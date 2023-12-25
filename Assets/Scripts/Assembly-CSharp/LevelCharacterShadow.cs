using System;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class LevelCharacterShadow : AbstractPausableComponent
{
	// Token: 0x06001345 RID: 4933 RVA: 0x000AA7C8 File Offset: 0x000A8BC8
	private void Start()
	{
		this.shadow = new GameObject(base.gameObject.name + "_Shadow").transform;
		this.spriteRenderer = this.shadow.gameObject.AddComponent<SpriteRenderer>();
		this.shadow.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		this.spriteRenderer.sprite = this.shadowSprites[0];
		if (this.isBGLayer)
		{
			this.spriteRenderer.sortingLayerName = SpriteLayer.Background.ToString();
			this.spriteRenderer.sortingOrder = 100;
		}
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x000AA888 File Offset: 0x000A8C88
	private void Update()
	{
		Vector3 position = this.shadow.position;
		position.x = base.transform.position.x;
		Collider2D component = this.root.GetComponent<Collider2D>();
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(this.root.transform.position, new Vector2(component.bounds.size.x, 1f), 0f, Vector2.down, (float)this.maxDistance, this.groundMask);
		if (raycastHit2D.collider == null)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		LevelPlatform component2 = raycastHit2D.collider.gameObject.GetComponent<LevelPlatform>();
		if (component2 != null && !component2.AllowShadows)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		position.y = raycastHit2D.point.y;
		this.shadow.position = position;
		this.SetSprite();
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x000AA99C File Offset: 0x000A8D9C
	private void SetSprite()
	{
		int num = (int)(Mathf.Abs(base.transform.position.y - this.shadow.position.y) / (float)this.maxDistance * (float)this.shadowSprites.Length);
		if (num < 0 || num >= this.shadowSprites.Length)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		this.spriteRenderer.enabled = true;
		this.spriteRenderer.sprite = this.shadowSprites[num];
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x000AAA2A File Offset: 0x000A8E2A
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.shadow != null)
		{
			UnityEngine.Object.Destroy(this.shadow.gameObject);
		}
	}

	// Token: 0x04001C69 RID: 7273
	[Range(1f, 1000f)]
	[SerializeField]
	private int maxDistance = 250;

	// Token: 0x04001C6A RID: 7274
	[SerializeField]
	private Transform root;

	// Token: 0x04001C6B RID: 7275
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x04001C6C RID: 7276
	[SerializeField]
	private bool isBGLayer;

	// Token: 0x04001C6D RID: 7277
	private Transform shadow;

	// Token: 0x04001C6E RID: 7278
	private SpriteRenderer spriteRenderer;

	// Token: 0x04001C6F RID: 7279
	private readonly int groundMask = 1048576;
}
