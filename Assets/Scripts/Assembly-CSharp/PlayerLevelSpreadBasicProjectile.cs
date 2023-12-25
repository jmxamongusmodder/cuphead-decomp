using System;
using UnityEngine;

// Token: 0x02000A85 RID: 2693
public class PlayerLevelSpreadBasicProjectile : BasicProjectile
{
	// Token: 0x06004061 RID: 16481 RVA: 0x002314C4 File Offset: 0x0022F8C4
	protected override void OnDieDistance()
	{
		if (base.dead)
		{
			return;
		}
		this.Die();
		base.animator.SetTrigger("OnDistanceDie");
	}

	// Token: 0x06004062 RID: 16482 RVA: 0x002314E8 File Offset: 0x0022F8E8
	protected override void Die()
	{
		base.Die();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)UnityEngine.Random.Range(0, 360)));
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?((float)MathUtils.PlusOrMinus()), new float?(1f));
	}

	// Token: 0x06004063 RID: 16483 RVA: 0x00231556 File Offset: 0x0022F956
	private void _OnDieAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
