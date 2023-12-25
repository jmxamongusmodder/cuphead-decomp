using System;
using UnityEngine;

// Token: 0x02000967 RID: 2407
public class MapZoomOut : AbstractCollidableObject
{
	// Token: 0x06003818 RID: 14360 RVA: 0x00201814 File Offset: 0x001FFC14
	private void Start()
	{
		this._filter = default(ContactFilter2D).NoFilter();
		this._startSize = this._camera.camera.orthographicSize;
		this._collider = base.GetComponent<BoxCollider2D>();
		this._maxDistance = this._collider.bounds.extents.y;
		this._zoomDistance = this._maxZoomOut - this._startSize;
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x0020188C File Offset: 0x001FFC8C
	private void Update()
	{
		int num = this._collider.OverlapCollider(this._filter, this.buffer);
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < num; i++)
		{
			MapPlayerController component = this.buffer[i].GetComponent<MapPlayerController>();
			if (!(component == null))
			{
				num3 += 1f;
				float magnitude = (component.transform.position - base.transform.position).magnitude;
				num2 = ((num2 < magnitude) ? magnitude : num2);
			}
		}
		if ((PlayerManager.Multiplayer && num3 == 2f) || (!PlayerManager.Multiplayer && num3 == 1f))
		{
			this._currentZoomRatio = 1f - Mathf.Clamp(num2 / this._maxDistance, 0f, 1f);
		}
		else
		{
			this._currentZoomRatio = 0f;
		}
		this._camera.camera.orthographicSize = Mathf.Lerp(this._camera.camera.orthographicSize, this.EaseInOutQuad(this._startSize, this._zoomDistance, this._currentZoomRatio), Time.deltaTime * this.ZoomSharpness);
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x002019D8 File Offset: 0x001FFDD8
	private float EaseInOutQuad(float startValue, float endValue, float time)
	{
		time *= 2f;
		if (time < 1f)
		{
			return endValue / 2f * time * time + startValue;
		}
		time -= 1f;
		return -endValue / 2f * (time * (time - 2f) - 1f) + startValue;
	}

	// Token: 0x04003FF3 RID: 16371
	[SerializeField]
	private CupheadMapCamera _camera;

	// Token: 0x04003FF4 RID: 16372
	[SerializeField]
	private float _maxZoomOut;

	// Token: 0x04003FF5 RID: 16373
	[SerializeField]
	private float ZoomSharpness = 1f;

	// Token: 0x04003FF6 RID: 16374
	private float _startSize;

	// Token: 0x04003FF7 RID: 16375
	private float _maxDistance;

	// Token: 0x04003FF8 RID: 16376
	private float _zoomDistance;

	// Token: 0x04003FF9 RID: 16377
	private float _currentZoomRatio;

	// Token: 0x04003FFA RID: 16378
	private BoxCollider2D _collider;

	// Token: 0x04003FFB RID: 16379
	private ContactFilter2D _filter;

	// Token: 0x04003FFC RID: 16380
	private Collider2D[] buffer = new Collider2D[10];
}
