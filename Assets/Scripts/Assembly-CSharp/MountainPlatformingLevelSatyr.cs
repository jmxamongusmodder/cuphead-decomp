using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008ED RID: 2285
public class MountainPlatformingLevelSatyr : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06003599 RID: 13721 RVA: 0x001F389B File Offset: 0x001F1C9B
	protected override void Start()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.Start();
		base.StartCoroutine(this.satyr_land_cr());
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x001F38BC File Offset: 0x001F1CBC
	public void Init(PlatformingLevelGroundMovementEnemy.Direction direction, bool isForeground)
	{
		this._direction = direction;
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x001F38EC File Offset: 0x001F1CEC
	private IEnumerator satyr_land_cr()
	{
		AudioManager.Play("castle_imp_spawn");
		this.emitAudioFromObject.Add("castle_imp_spawn");
		this.floating = false;
		base.Jump();
		base.StartCoroutine(this.change_layer_cr());
		while (!base.Grounded)
		{
			yield return null;
		}
		this.landing = true;
		AudioManager.Play("castle_imp_land");
		this.emitAudioFromObject.Add("castle_imp_land");
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_Continue", false, true);
		this.landing = false;
		yield return null;
		yield break;
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x001F3908 File Offset: 0x001F1D08
	private IEnumerator change_layer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		base.GetComponent<SpriteRenderer>().sortingOrder = 20;
		base.GetComponent<Collider2D>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x001F3923 File Offset: 0x001F1D23
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<MountainPlatformingLevelWall>())
		{
			this.Turn();
		}
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x001F394A File Offset: 0x001F1D4A
	protected override void Die()
	{
		AudioManager.Play("castle_generic_death_honk");
		this.emitAudioFromObject.Add("castle_generic_death_honk");
		base.Die();
	}
}
