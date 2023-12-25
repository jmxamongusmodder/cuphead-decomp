using System;

// Token: 0x02000859 RID: 2137
public class VeggiesLevelSpit : BasicProjectile
{
	// Token: 0x06003191 RID: 12689 RVA: 0x001CED44 File Offset: 0x001CD144
	protected override void Die()
	{
		if (base.CanParry)
		{
			AudioManager.Play("level_veggies_potato_worm_explode");
		}
		base.Die();
	}
}
