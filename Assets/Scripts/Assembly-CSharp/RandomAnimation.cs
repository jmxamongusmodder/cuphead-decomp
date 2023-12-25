using System;
using UnityEngine;

// Token: 0x02000B15 RID: 2837
public class RandomAnimation : AbstractPausableComponent
{
	// Token: 0x060044C7 RID: 17607 RVA: 0x00246924 File Offset: 0x00244D24
	protected override void Awake()
	{
		base.Awake();
		base.animator.SetInteger("Animation", UnityEngine.Random.Range(0, base.animator.GetInteger("Count")));
		base.animator.speed += UnityEngine.Random.Range(-this.randomSpeed, this.randomSpeed);
	}

	// Token: 0x04004A82 RID: 19074
	[SerializeField]
	[Range(0f, 1f)]
	private float randomSpeed = 0.1f;
}
