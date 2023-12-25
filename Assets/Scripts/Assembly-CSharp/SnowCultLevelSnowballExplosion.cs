using System;
using UnityEngine;

// Token: 0x020007F6 RID: 2038
public class SnowCultLevelSnowballExplosion : MonoBehaviour
{
	// Token: 0x06002ED1 RID: 11985 RVA: 0x001BA038 File Offset: 0x001B8438
	public void Init(Vector3 pos, SnowCultLevelSnowball.Size size, SnowCultLevelYeti main)
	{
		base.transform.position = pos;
		if (size != SnowCultLevelSnowball.Size.Large)
		{
			if (size != SnowCultLevelSnowball.Size.Medium)
			{
				if (size == SnowCultLevelSnowball.Size.Small)
				{
					int smallExplosion = main.GetSmallExplosion();
					if (smallExplosion != 0)
					{
						if (smallExplosion != 1)
						{
							if (smallExplosion == 2)
							{
								this.animator.Play("SmallC");
							}
						}
						else
						{
							this.animator.Play("SmallB");
						}
					}
					else
					{
						this.animator.Play("SmallA");
					}
				}
			}
			else
			{
				this.animator.Play((main.GetMediumExplosion() != 0) ? "MediumB" : "MediumA");
			}
		}
		else
		{
			this.animator.Play("Large");
		}
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x001BA10D File Offset: 0x001B850D
	private void Update()
	{
		if (!this.rend.enabled)
		{
			this.Recycle<SnowCultLevelSnowballExplosion>();
		}
	}

	// Token: 0x04003783 RID: 14211
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04003784 RID: 14212
	[SerializeField]
	private Animator animator;
}
