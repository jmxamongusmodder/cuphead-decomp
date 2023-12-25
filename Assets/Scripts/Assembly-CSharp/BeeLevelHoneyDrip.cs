using System;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class BeeLevelHoneyDrip : AbstractMonoBehaviour
{
	// Token: 0x0600172B RID: 5931 RVA: 0x000D0134 File Offset: 0x000CE534
	public BeeLevelHoneyDrip Create()
	{
		return UnityEngine.Object.Instantiate<BeeLevelHoneyDrip>(this);
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x000D014C File Offset: 0x000CE54C
	private BeeLevelHoneyDrip Create(int number)
	{
		BeeLevelHoneyDrip beeLevelHoneyDrip = this.Create();
		beeLevelHoneyDrip.i = number;
		return beeLevelHoneyDrip;
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x000D0168 File Offset: 0x000CE568
	protected override void Awake()
	{
		base.Awake();
		base.GetComponent<Animator>().SetInteger("I", UnityEngine.Random.Range(0, 6));
		base.transform.SetParent(Camera.main.transform);
		base.transform.SetLocalPosition(new float?((float)UnityEngine.Random.Range(-540, 540)), new float?(415f), new float?(100f));
		base.transform.SetParent(null);
		AudioManager.Play("bee_honey_glug_sweet");
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x000D01F1 File Offset: 0x000CE5F1
	private void OnAnimationEnd()
	{
		if (this.i < 4 && UnityEngine.Random.value < 0.5f)
		{
			this.Create(this.i + 1);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400206A RID: 8298
	private const float START_Y = 415f;

	// Token: 0x0400206B RID: 8299
	private const int MAX = 5;

	// Token: 0x0400206C RID: 8300
	private int i;
}
