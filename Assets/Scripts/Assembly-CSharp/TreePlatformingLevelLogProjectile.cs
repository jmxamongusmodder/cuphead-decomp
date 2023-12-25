using System;
using UnityEngine;

// Token: 0x02000894 RID: 2196
public class TreePlatformingLevelLogProjectile : BasicProjectile
{
	// Token: 0x06003311 RID: 13073 RVA: 0x001DB268 File Offset: 0x001D9668
	public AbstractProjectile Create(Vector2 position, float rotation, float speed, bool isLeft, bool parry)
	{
		TreePlatformingLevelLogProjectile treePlatformingLevelLogProjectile = base.Create(position, rotation, speed) as TreePlatformingLevelLogProjectile;
		treePlatformingLevelLogProjectile.animator.SetFloat("Direction", (float)((!isLeft) ? -1 : 1));
		treePlatformingLevelLogProjectile.animator.SetTrigger("Start");
		treePlatformingLevelLogProjectile.SetParryable(parry);
		return treePlatformingLevelLogProjectile;
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x001DB2BC File Offset: 0x001D96BC
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetBool("Parry", parryable);
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x001DB2D6 File Offset: 0x001D96D6
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
