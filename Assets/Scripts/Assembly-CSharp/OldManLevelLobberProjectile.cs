using System;
using UnityEngine;

// Token: 0x02000705 RID: 1797
public class OldManLevelLobberProjectile : BasicProjectile
{
	// Token: 0x060026A3 RID: 9891 RVA: 0x00169B5C File Offset: 0x00167F5C
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<LevelPlatform>())
		{
			this.Die();
			foreach (AbstractPlayerController abstractPlayerController in hit.GetComponentsInChildren<AbstractPlayerController>())
			{
				if (!(abstractPlayerController == null))
				{
					abstractPlayerController.transform.parent = null;
				}
			}
			hit.SetActive(false);
		}
	}

	// Token: 0x060026A4 RID: 9892 RVA: 0x00169BCA File Offset: 0x00167FCA
	protected override void Die()
	{
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}
}
