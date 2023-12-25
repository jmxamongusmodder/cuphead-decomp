using System;
using UnityEngine;

// Token: 0x02000823 RID: 2083
public class TrainLevelPassengerCar : AbstractTrainLevelTrainCar
{
	// Token: 0x06003060 RID: 12384 RVA: 0x001C81B4 File Offset: 0x001C65B4
	public void Explode(int i)
	{
		base.animator.SetInteger("State", i);
		base.animator.SetTrigger("OnDamaged");
		if (i != 0)
		{
			if (i == 1)
			{
				this.explosionEffects[1].Create(base.transform.position);
			}
		}
		else
		{
			this.explosionEffects[0].Create(base.transform.position);
		}
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x001C8235 File Offset: 0x001C6635
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.explosionEffects = null;
	}

	// Token: 0x0400390C RID: 14604
	[SerializeField]
	private Effect[] explosionEffects;
}
