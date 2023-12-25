using System;
using UnityEngine;

// Token: 0x020006C5 RID: 1733
public class GraveyardLevelPlatform : AbstractMonoBehaviour
{
	// Token: 0x060024D5 RID: 9429 RVA: 0x00159371 File Offset: 0x00157771
	private void Start()
	{
		this.center = base.transform.position;
		this.t = -0.8f;
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x00159390 File Offset: 0x00157790
	private void Update()
	{
		this.t += CupheadTime.Delta * this.speed;
		base.transform.position = this.center + MathUtils.AngleToDirection(-90f + Mathf.Sin(this.t) * this.maxAngle) * this.radius;
	}

	// Token: 0x04002D73 RID: 11635
	private float t;

	// Token: 0x04002D74 RID: 11636
	private Vector3 center;

	// Token: 0x04002D75 RID: 11637
	[SerializeField]
	private float radius = 700f;

	// Token: 0x04002D76 RID: 11638
	[SerializeField]
	private float speed = 315f;

	// Token: 0x04002D77 RID: 11639
	[SerializeField]
	private float maxAngle = 30f;
}
