using System;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class MapParallaxLayer : AbstractPausableComponent
{
	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000D64 RID: 3428 RVA: 0x0008E045 File Offset: 0x0008C445
	private Vector2 _offset
	{
		get
		{
			return this._startPosition - this._cameraStartPosition;
		}
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x0008E05D File Offset: 0x0008C45D
	private void Start()
	{
		this._camera = CupheadMapCamera.Current;
		this._startPosition = base.transform.position;
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x0008E07B File Offset: 0x0008C47B
	private void LateUpdate()
	{
		this.UpdateComparative();
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x0008E084 File Offset: 0x0008C484
	private void UpdateComparative()
	{
		Vector3 position = base.transform.position;
		position.x = this._offset.x + this._camera.transform.position.x * this.percentage;
		position.y = this._offset.y + this._camera.transform.position.y * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x040016EE RID: 5870
	[Range(-3f, 3f)]
	public float percentage;

	// Token: 0x040016EF RID: 5871
	[SerializeField]
	private Vector3 _cameraStartPosition;

	// Token: 0x040016F0 RID: 5872
	private CupheadMapCamera _camera;

	// Token: 0x040016F1 RID: 5873
	private Vector3 _startPosition;
}
