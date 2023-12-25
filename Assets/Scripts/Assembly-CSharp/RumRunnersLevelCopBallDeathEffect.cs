using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200078A RID: 1930
public class RumRunnersLevelCopBallDeathEffect : Effect
{
	// Token: 0x06002AA6 RID: 10918 RVA: 0x0018E9FC File Offset: 0x0018CDFC
	public override void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		int i = UnityEngine.Random.Range(0, base.animator.GetInteger("Count"));
		base.animator.SetInteger("Effect", i);
		Transform transform = base.transform;
		transform.position = position;
		transform.localScale = scale;
		if (randomR)
		{
			transform.eulerAngles = new Vector3(0f, 0f, (float)(UnityEngine.Random.Range(0, 8) * 45));
		}
		List<int> list = new List<int>();
		for (i = 0; i < 5; i++)
		{
			list.Add(i);
		}
		list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		for (i = 0; i < 3; i++)
		{
			this.shrapnel[i].Play(list[i].ToString());
			this.shrapnel[i].transform.parent = null;
		}
	}

	// Token: 0x0400336B RID: 13163
	[SerializeField]
	private Animator[] shrapnel;
}
