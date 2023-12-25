using System;
using UnityEngine;

// Token: 0x020006EE RID: 1774
public class MouseLevelFlame : AbstractCollidableObject
{
	// Token: 0x060025FF RID: 9727 RVA: 0x00163A8B File Offset: 0x00161E8B
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.transform.parent = null;
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x00163AAA File Offset: 0x00161EAA
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x00163AC2 File Offset: 0x00161EC2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x00163AEC File Offset: 0x00161EEC
	public void SetColliderEnabled(bool enabled)
	{
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = enabled;
		}
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x00163B20 File Offset: 0x00161F20
	public void UpdateParentTransform(Transform mouseTransform)
	{
		base.transform.position = ((mouseTransform.localScale.x != 1f) ? this.flippedRoot.position : this.root.position);
	}

	// Token: 0x04002E86 RID: 11910
	[SerializeField]
	private Transform root;

	// Token: 0x04002E87 RID: 11911
	[SerializeField]
	private Transform flippedRoot;

	// Token: 0x04002E88 RID: 11912
	private DamageDealer damageDealer;
}
