using System;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
public class DragonLevelPeashotFlash : AbstractPausableComponent
{
	// Token: 0x06001E6F RID: 7791 RVA: 0x00118B34 File Offset: 0x00116F34
	public void Flash()
	{
		base.transform.SetScale(new float?(1f), new float?((float)MathUtils.PlusOrMinus()), new float?(1f));
		base.animator.SetInteger("i", UnityEngine.Random.Range(0, 4));
		base.animator.SetTrigger("OnChange");
	}
}
