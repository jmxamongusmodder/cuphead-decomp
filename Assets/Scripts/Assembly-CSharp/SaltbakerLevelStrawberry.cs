using System;
using UnityEngine;

// Token: 0x020007D6 RID: 2006
public class SaltbakerLevelStrawberry : SaltbakerLevelPhaseOneProjectile
{
	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06002DCB RID: 11723 RVA: 0x001B0778 File Offset: 0x001AEB78
	protected override Vector3 Direction
	{
		get
		{
			return -base.transform.up;
		}
	}

	// Token: 0x06002DCC RID: 11724 RVA: 0x001B078C File Offset: 0x001AEB8C
	public BasicProjectile Create(Vector2 position, float rotation, float speed, int anim)
	{
		SaltbakerLevelStrawberry saltbakerLevelStrawberry = (SaltbakerLevelStrawberry)base.Create(position, rotation, speed);
		saltbakerLevelStrawberry.anim.Play(anim.ToString());
		return saltbakerLevelStrawberry;
	}

	// Token: 0x06002DCD RID: 11725 RVA: 0x001B07C4 File Offset: 0x001AEBC4
	protected override void Move()
	{
		if (!this.coll.enabled)
		{
			return;
		}
		base.Move();
		if (base.transform.position.y - 40f < (float)Level.Current.Ground)
		{
			this.shadow.enabled = false;
			this.Die();
		}
		else
		{
			base.HandleShadow(40f, 0f);
		}
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x001B0838 File Offset: 0x001AEC38
	protected override void Die()
	{
		this.coll.enabled = false;
		this.createSparks = false;
		base.transform.eulerAngles = Vector3.zero;
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x001B086D File Offset: 0x001AEC6D
	private void OnDeathAnimationEnd()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003654 RID: 13908
	private const float OFFSET = 40f;

	// Token: 0x04003655 RID: 13909
	[SerializeField]
	private Animator anim;

	// Token: 0x04003656 RID: 13910
	[SerializeField]
	private Collider2D coll;
}
