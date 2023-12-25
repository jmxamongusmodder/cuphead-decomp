using System;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class VeggiesLevelOnionTearsStream : AbstractMonoBehaviour
{
	// Token: 0x06003164 RID: 12644 RVA: 0x001CE484 File Offset: 0x001CC884
	public VeggiesLevelOnionTearsStream Create(Vector2 pos, int scale)
	{
		VeggiesLevelOnionTearsStream veggiesLevelOnionTearsStream = this.InstantiatePrefab<VeggiesLevelOnionTearsStream>();
		veggiesLevelOnionTearsStream.transform.SetScale(new float?((float)scale), new float?(1f), new float?(1f));
		veggiesLevelOnionTearsStream.transform.position = pos;
		return veggiesLevelOnionTearsStream;
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x001CE4D0 File Offset: 0x001CC8D0
	public void End()
	{
		if (this.ending)
		{
			return;
		}
		this.ending = true;
		base.animator.Play("Out");
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x001CE4F5 File Offset: 0x001CC8F5
	private void OnAnimEnd()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040039EE RID: 14830
	private bool ending;
}
