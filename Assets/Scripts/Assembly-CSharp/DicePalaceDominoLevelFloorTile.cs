using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005BB RID: 1467
public class DicePalaceDominoLevelFloorTile : DicePalaceDominoLevelBaseTile
{
	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06001C7D RID: 7293 RVA: 0x00104BB1 File Offset: 0x00102FB1
	// (set) Token: 0x06001C7E RID: 7294 RVA: 0x00104BB9 File Offset: 0x00102FB9
	public bool spikesActive { get; private set; }

	// Token: 0x06001C7F RID: 7295 RVA: 0x00104BC2 File Offset: 0x00102FC2
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		base.Awake();
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00104BE1 File Offset: 0x00102FE1
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x00104BF9 File Offset: 0x00102FF9
	public override void InitTile()
	{
		base.InitTile();
		this.OnMoveStart();
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00104C07 File Offset: 0x00103007
	public void SetColour(int colourIndex, LevelProperties.DicePalaceDomino properties)
	{
		this.properties = properties;
		base.currentColourIndex = colourIndex;
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.spriteRenderer.sprite = this.colours[base.currentColourIndex];
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x00104C3B File Offset: 0x0010303B
	public void OnMoveStart()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x00104C4C File Offset: 0x0010304C
	private IEnumerator move_cr()
	{
		yield return null;
		while (base.isActivated)
		{
			base.transform.position += Vector3.left * this.properties.CurrentState.domino.floorSpeed * CupheadTime.Delta;
			if (base.transform.position.x + 200f < (float)Level.Current.Left)
			{
				this.DeactivateTile();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x00104C67 File Offset: 0x00103067
	public void TriggerSpikes(bool spikesActive)
	{
		base.StartCoroutine(this.toggleSpikes_cr(spikesActive));
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00104C77 File Offset: 0x00103077
	public override void DeactivateTile()
	{
		base.DeactivateTile();
		this.toggleSpikes_cr(false);
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x00104C88 File Offset: 0x00103088
	private IEnumerator toggleSpikes_cr(bool spikesActive)
	{
		if (spikesActive)
		{
			base.animator.Play("Spikes_Up");
			this.spikesActive = true;
			this.boxCollider.enabled = true;
		}
		else
		{
			if (this.spikesActive)
			{
				base.animator.Play("Spikes_Down");
				base.StartCoroutine(this.disableCollider_cr());
			}
			else
			{
				base.animator.Play("Off");
				this.boxCollider.enabled = false;
			}
			this.spikesActive = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x00104CAC File Offset: 0x001030AC
	private IEnumerator disableCollider_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Spikes_Down", true, true);
		this.boxCollider.enabled = false;
		yield break;
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x00104CC7 File Offset: 0x001030C7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x00104CE5 File Offset: 0x001030E5
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04002577 RID: 9591
	private SpriteRenderer spriteRenderer;

	// Token: 0x04002578 RID: 9592
	private DamageDealer damageDealer;

	// Token: 0x0400257A RID: 9594
	private BoxCollider2D boxCollider;
}
