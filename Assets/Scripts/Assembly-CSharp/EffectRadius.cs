using System;
using UnityEngine;

// Token: 0x02000B10 RID: 2832
public class EffectRadius : AbstractPausableComponent
{
	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x060044B6 RID: 17590 RVA: 0x00246305 File Offset: 0x00244705
	public float radius
	{
		get
		{
			return this._radius;
		}
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x060044B7 RID: 17591 RVA: 0x0024630D File Offset: 0x0024470D
	public Vector2 offset
	{
		get
		{
			return this._offset;
		}
	}

	// Token: 0x060044B8 RID: 17592 RVA: 0x00246318 File Offset: 0x00244718
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position + this.offset, this.radius);
	}

	// Token: 0x060044B9 RID: 17593 RVA: 0x00246374 File Offset: 0x00244774
	public void CreateInRadius()
	{
		Vector2 a = base.baseTransform.position + this.offset;
		Vector2 vector = new Vector2(UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1), UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		this.target = a + vector.normalized * this.radius * UnityEngine.Random.value;
		this.effect.Create(this.target);
	}

	// Token: 0x04004A6F RID: 19055
	[SerializeField]
	private Effect effect;

	// Token: 0x04004A70 RID: 19056
	[SerializeField]
	private float _radius = 100f;

	// Token: 0x04004A71 RID: 19057
	[SerializeField]
	private Vector2 _offset = Vector2.zero;

	// Token: 0x04004A72 RID: 19058
	private Vector2 target;
}
