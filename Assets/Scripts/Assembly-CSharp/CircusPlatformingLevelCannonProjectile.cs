using System;
using UnityEngine;

// Token: 0x020008A0 RID: 2208
public class CircusPlatformingLevelCannonProjectile : BasicProjectile
{
	// Token: 0x06003364 RID: 13156 RVA: 0x001DE640 File Offset: 0x001DCA40
	public void SetColor(string color)
	{
		int num = UnityEngine.Random.Range(0, 2);
		if (color != null)
		{
			if (!(color == "P"))
			{
				if (!(color == "G"))
				{
					if (color == "O")
					{
						base.animator.SetInteger("Variation", num + 4);
					}
				}
				else
				{
					base.animator.SetInteger("Variation", num + 2);
				}
			}
			else
			{
				this.SetParryable(true);
				base.animator.SetInteger("Variation", num);
			}
		}
	}

	// Token: 0x04003BB0 RID: 15280
	private const string VariationParameterName = "Variation";

	// Token: 0x04003BB1 RID: 15281
	private const string Pink = "P";

	// Token: 0x04003BB2 RID: 15282
	private const string Green = "G";

	// Token: 0x04003BB3 RID: 15283
	private const string Orange = "O";
}
