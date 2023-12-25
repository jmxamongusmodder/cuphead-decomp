using System;
using UnityEngine;

// Token: 0x02000878 RID: 2168
public class ForestPlatformingLevelAcornMaker : PlatformingLevelShootingEnemy
{
	// Token: 0x0600325B RID: 12891 RVA: 0x001D5604 File Offset: 0x001D3A04
	protected override void Shoot()
	{
		ForestPlatformingLevelAcorn.Direction direction;
		if (this._target.transform.position.x < base.transform.position.x)
		{
			direction = ForestPlatformingLevelAcorn.Direction.Left;
		}
		else
		{
			direction = ForestPlatformingLevelAcorn.Direction.Right;
		}
		this.acornPrefab.Spawn(this, this.spawnRoot.transform.position, direction, true);
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x001D5670 File Offset: 0x001D3A70
	protected override void Die()
	{
		if (!this.isDying)
		{
			if (this.killAcorns != null)
			{
				this.killAcorns();
			}
			base.animator.SetTrigger("Death");
			Collider2D[] componentsInChildren = base.GetComponentsInChildren<Collider2D>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			this.isDying = true;
			this.explosion.Create(this.gruntRoot.transform.position);
			this.gruntSprite.enabled = false;
		}
		else
		{
			base.Die();
		}
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x001D570C File Offset: 0x001D3B0C
	private void PlayGruntSFX()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
		{
			AudioManager.Play("level_acorn_maker_grunt");
			this.emitAudioFromObject.Add("level_acorn_maker_grunt");
		}
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x001D5764 File Offset: 0x001D3B64
	private void PlayIdleSFX()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
		{
			AudioManager.Play("level_acorn_maker_idle");
			this.emitAudioFromObject.Add("level_acorn_maker_idle");
		}
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x001D57B9 File Offset: 0x001D3BB9
	private void PlayDeathSFX()
	{
		AudioManager.Play("level_acorn_maker_death");
		this.emitAudioFromObject.Add("level_acorn_maker_death");
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x001D57D5 File Offset: 0x001D3BD5
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.acornPrefab = null;
		this.explosion = null;
	}

	// Token: 0x04003ABA RID: 15034
	private const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x04003ABB RID: 15035
	[SerializeField]
	private Effect explosion;

	// Token: 0x04003ABC RID: 15036
	[SerializeField]
	private Transform gruntRoot;

	// Token: 0x04003ABD RID: 15037
	[SerializeField]
	private SpriteRenderer gruntSprite;

	// Token: 0x04003ABE RID: 15038
	[SerializeField]
	private ForestPlatformingLevelAcorn acornPrefab;

	// Token: 0x04003ABF RID: 15039
	[SerializeField]
	private Transform spawnRoot;

	// Token: 0x04003AC0 RID: 15040
	private bool isDying;

	// Token: 0x04003AC1 RID: 15041
	public Action killAcorns;
}
