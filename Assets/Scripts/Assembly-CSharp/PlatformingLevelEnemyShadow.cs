using System;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class PlatformingLevelEnemyShadow : AbstractCollidableObject
{
	// Token: 0x06000FDE RID: 4062 RVA: 0x0009D314 File Offset: 0x0009B714
	private void Start()
	{
		this.shadow = new GameObject(base.gameObject.name + "_Shadow").transform;
		this.spriteRenderer = this.shadow.gameObject.AddComponent<SpriteRenderer>();
		this.shadow.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		this.spriteRenderer.sprite = this.shadowSprites[0];
		this.enemy = base.GetComponent<PlatformingLevelGroundMovementEnemy>();
		this.boxCollider = this.enemy.GetComponent<BoxCollider2D>();
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x0009D3C0 File Offset: 0x0009B7C0
	private void Update()
	{
		if (this.enemy.Grounded || this.enemy.Dead)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		this.spriteRenderer.enabled = true;
		Vector3 position = this.shadow.position;
		position.x = base.transform.position.x;
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(base.transform.position, new Vector2(this.boxCollider.size.x, 1f), 0f, Vector2.down, (float)this.maxDistance, this.groundMask);
		if (raycastHit2D.collider == null)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		LevelPlatform component = raycastHit2D.collider.gameObject.GetComponent<LevelPlatform>();
		if (component != null && !component.AllowShadows)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		position.y = raycastHit2D.point.y;
		this.shadow.position = position;
		this.SetSprite();
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x0009D4F8 File Offset: 0x0009B8F8
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

	// Token: 0x06000FE1 RID: 4065 RVA: 0x0009D586 File Offset: 0x0009B986
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.shadow != null)
		{
			UnityEngine.Object.Destroy(this.shadow.gameObject);
		}
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x0009D5AF File Offset: 0x0009B9AF
	public Vector3 ShadowPosition()
	{
		return this.shadow.position;
	}

	// Token: 0x04001975 RID: 6517
	[Range(1f, 1000f)]
	[SerializeField]
	private int maxDistance = 250;

	// Token: 0x04001976 RID: 6518
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x04001977 RID: 6519
	private Transform shadow;

	// Token: 0x04001978 RID: 6520
	private SpriteRenderer spriteRenderer;

	// Token: 0x04001979 RID: 6521
	private PlatformingLevelGroundMovementEnemy enemy;

	// Token: 0x0400197A RID: 6522
	private BoxCollider2D boxCollider;

	// Token: 0x0400197B RID: 6523
	private readonly int groundMask = 1048576;
}
