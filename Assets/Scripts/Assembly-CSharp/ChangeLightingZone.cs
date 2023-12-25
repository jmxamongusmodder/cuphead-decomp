using System;
using UnityEngine;

// Token: 0x0200092D RID: 2349
public class ChangeLightingZone : AbstractCollidableObject
{
	// Token: 0x060036FA RID: 14074 RVA: 0x001FB118 File Offset: 0x001F9518
	private void Start()
	{
		this._filter = default(ContactFilter2D).NoFilter();
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x001FB13C File Offset: 0x001F953C
	private void Update()
	{
		int num = this._collider.OverlapCollider(this._filter, this.buffer);
		for (int i = 0; i < num; i++)
		{
			MapPlayerAnimationController component = this.buffer[i].GetComponent<MapPlayerAnimationController>();
			if (!(component == null))
			{
				float magnitude = (component.transform.position - base.transform.position).magnitude;
				float t = Mathf.Clamp(magnitude / this._maxDistance, 0f, 1f);
				component.spriteRenderer.color = Color.Lerp(this._minTint, this._maxTint, t);
			}
		}
	}

	// Token: 0x04003F2A RID: 16170
	[SerializeField]
	private Color _minTint;

	// Token: 0x04003F2B RID: 16171
	[SerializeField]
	private Color _maxTint;

	// Token: 0x04003F2C RID: 16172
	[SerializeField]
	private BoxCollider2D _collider;

	// Token: 0x04003F2D RID: 16173
	[SerializeField]
	private float _maxDistance;

	// Token: 0x04003F2E RID: 16174
	private ContactFilter2D _filter;

	// Token: 0x04003F2F RID: 16175
	private Collider2D[] buffer = new Collider2D[10];
}
