using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public abstract class AbstractLevelEntity : AbstractCollidableObject
{
	// Token: 0x170002BB RID: 699
	// (get) Token: 0x0600117F RID: 4479 RVA: 0x0000880D File Offset: 0x00006C0D
	public bool canParry
	{
		get
		{
			return this._canParry;
		}
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x00008815 File Offset: 0x00006C15
	public virtual void OnParry(AbstractPlayerController player)
	{
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x00008818 File Offset: 0x00006C18
	protected IEnumerator flash_cr(Color start, Color end, float time, Action onComplete = null)
	{
		SpriteRenderer renderer = base.GetComponent<SpriteRenderer>();
		renderer.color = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			renderer.color = Color.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		renderer.color = end;
		if (onComplete != null)
		{
			onComplete();
		}
		yield break;
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x00008850 File Offset: 0x00006C50
	protected IEnumerator dieFlash_cr()
	{
		for (int i = 0; i < 4; i++)
		{
			yield return base.StartCoroutine(this.flash_cr(Color.red, Color.black, 0.3f, null));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x04001B0F RID: 6927
	protected const float DIE_FLASH_TIME = 0.3f;

	// Token: 0x04001B10 RID: 6928
	protected const float DIE_FLASH_DELAY = 0.2f;

	// Token: 0x04001B11 RID: 6929
	protected const int DIE_FLASH_LOOPS = 4;

	// Token: 0x04001B12 RID: 6930
	[SerializeField]
	protected bool _canParry;
}
