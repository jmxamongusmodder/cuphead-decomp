using System;
using UnityEngine;

// Token: 0x020006E1 RID: 1761
public class MouseLevelCanCatapultProjectile : BasicProjectile
{
	// Token: 0x0600258A RID: 9610 RVA: 0x0015F604 File Offset: 0x0015DA04
	public MouseLevelCanCatapultProjectile CreateFromPrefab(Vector2 pos, float rotation, float speed, char c)
	{
		MouseLevelCanCatapultProjectile mouseLevelCanCatapultProjectile = base.Create(pos, rotation, speed) as MouseLevelCanCatapultProjectile;
		mouseLevelCanCatapultProjectile.Set(c);
		return mouseLevelCanCatapultProjectile;
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x0015F629 File Offset: 0x0015DA29
	protected override void RandomizeVariant()
	{
	}

	// Token: 0x0600258C RID: 9612 RVA: 0x0015F62C File Offset: 0x0015DA2C
	private void Set(char c)
	{
		int i;
		switch (c)
		{
		case 'b':
			break;
		case 'c':
			i = 4;
			goto IL_67;
		default:
			switch (c)
			{
			case 'n':
				i = 1;
				goto IL_67;
			case 'p':
				i = 3;
				goto IL_67;
			}
			break;
		case 'g':
			i = 2;
			this.SetParryable(true);
			goto IL_67;
		}
		i = 0;
		IL_67:
		this.SetInt(AbstractProjectile.Variant, i);
	}
}
