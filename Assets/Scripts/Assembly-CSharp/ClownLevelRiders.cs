using System;
using UnityEngine;

// Token: 0x0200056C RID: 1388
public class ClownLevelRiders : AbstractCollidableObject
{
	// Token: 0x06001A3A RID: 6714 RVA: 0x000F00A8 File Offset: 0x000EE4A8
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		if (this.inFront)
		{
			base.animator.SetBool("InFront", true);
		}
		else
		{
			base.animator.SetBool("InFront", false);
		}
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000F00E7 File Offset: 0x000EE4E7
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x000F00FF File Offset: 0x000EE4FF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x000F0120 File Offset: 0x000EE520
	public void BackLayers(int cartLayer)
	{
		this.sprites = base.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < this.sprites.Length; i++)
		{
			this.sprites[i].sortingLayerName = "Background";
			this.sprites[i].sortingOrder = cartLayer;
		}
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x000F0174 File Offset: 0x000EE574
	public void FrontLayers(int cartLayer)
	{
		this.sprites = base.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < this.sprites.Length; i++)
		{
			if (this.sprites[i] == this.backRider || this.sprites[i] == this.backSeat)
			{
				this.sprites[i].sortingLayerName = "Default";
				this.sprites[i].sortingOrder = 10 - i;
			}
			else
			{
				this.sprites[i].sortingLayerName = "Player";
				this.sprites[i].sortingOrder = cartLayer;
			}
		}
	}

	// Token: 0x04002356 RID: 9046
	[SerializeField]
	private SpriteRenderer backSeat;

	// Token: 0x04002357 RID: 9047
	[SerializeField]
	private SpriteRenderer backRider;

	// Token: 0x04002358 RID: 9048
	public bool inFront;

	// Token: 0x04002359 RID: 9049
	private DamageDealer damageDealer;

	// Token: 0x0400235A RID: 9050
	private SpriteRenderer[] sprites;
}
