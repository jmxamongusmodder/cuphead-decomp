using System;
using UnityEngine;

// Token: 0x02000A38 RID: 2616
public class LevelPlayerShadow : AbstractLevelPlayerComponent
{
	// Token: 0x06003E43 RID: 15939 RVA: 0x00223CA0 File Offset: 0x002220A0
	private void Start()
	{
		this.shadow = new GameObject(base.gameObject.name + "_Shadow").transform;
		this.spriteRenderer = this.shadow.gameObject.AddComponent<SpriteRenderer>();
		this.shadow.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		this.spriteRenderer.sprite = this.shadowSprites[0];
		if (Level.Current != null)
		{
			this.spriteRenderer.sortingOrder = Level.Current.playerShadowSortingOrder;
		}
		if (SceneLoader.CurrentLevel == Levels.ChaliceTutorial)
		{
			this.spriteRenderer.gameObject.layer = 31;
		}
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x00223D74 File Offset: 0x00222174
	private void Update()
	{
		if ((base.player.motor.Grounded && !base.player.motor.Dashing) || base.player.IsDead || ((base.player.stats.Loadout.charm == Charm.charm_smoke_dash || base.player.stats.CurseSmokeDash) && !Level.IsChessBoss && base.player.motor.Dashing))
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		this.spriteRenderer.enabled = true;
		Vector3 position = this.shadow.position;
		position.x = base.transform.position.x;
		BoxCollider2D collider = base.player.collider;
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(base.player.transform.position, new Vector2(base.player.collider.size.x, 1f), 0f, (!base.player.motor.GravityReversed) ? Vector2.down : Vector2.up, (float)this.maxDistance, (!base.player.motor.GravityReversed) ? this.groundMask : this.ceilingMask);
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

	// Token: 0x06003E45 RID: 15941 RVA: 0x00223F70 File Offset: 0x00222370
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

	// Token: 0x06003E46 RID: 15942 RVA: 0x00223FFE File Offset: 0x002223FE
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.shadow != null)
		{
			UnityEngine.Object.Destroy(this.shadow.gameObject);
		}
	}

	// Token: 0x06003E47 RID: 15943 RVA: 0x00224027 File Offset: 0x00222427
	public Vector3 ShadowPosition()
	{
		return this.shadow.position;
	}

	// Token: 0x04004567 RID: 17767
	[Range(1f, 1000f)]
	[SerializeField]
	private int maxDistance = 250;

	// Token: 0x04004568 RID: 17768
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x04004569 RID: 17769
	private Transform shadow;

	// Token: 0x0400456A RID: 17770
	private SpriteRenderer spriteRenderer;

	// Token: 0x0400456B RID: 17771
	private readonly int groundMask = 1048576;

	// Token: 0x0400456C RID: 17772
	private readonly int ceilingMask = 524288;
}
