using System;
using UnityEngine;

// Token: 0x020003DD RID: 989
public class DLCCutsceneParallaxLayer : AbstractPausableComponent
{
	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0008C85D File Offset: 0x0008AC5D
	protected Vector2 _offset
	{
		get
		{
			return this._startPosition - this._cameraStartPosition;
		}
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0008C875 File Offset: 0x0008AC75
	protected virtual void Start()
	{
		this._camera = CupheadCutsceneCamera.Current;
		this._startPosition = base.transform.position;
		this._cameraStartPosition = this._camera.transform.position;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0008C8AC File Offset: 0x0008ACAC
	private void LateUpdate()
	{
		DLCCutsceneParallaxLayer.Type type = this.type;
		if (type == DLCCutsceneParallaxLayer.Type.Comparative || type != DLCCutsceneParallaxLayer.Type.Centered)
		{
			this.UpdateComparative();
		}
		else
		{
			this.UpdateCentered();
		}
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0008C8EC File Offset: 0x0008ACEC
	protected virtual void UpdateComparative()
	{
		Vector3 position = base.transform.position;
		position.x = this._offset.x + this._camera.transform.position.x * this.percentage;
		position.y = this._offset.y + this._camera.transform.position.y * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x0008C97C File Offset: 0x0008AD7C
	private void UpdateCentered()
	{
		Vector3 position = base.transform.position;
		position.x = this._startPosition.x + (this._camera.transform.position.x - this._startPosition.x) * this.percentage;
		position.y = this._startPosition.y + (this._camera.transform.position.y - this._startPosition.y) * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x040016AF RID: 5807
	public DLCCutsceneParallaxLayer.Type type;

	// Token: 0x040016B0 RID: 5808
	[Range(-3f, 3f)]
	public float percentage;

	// Token: 0x040016B1 RID: 5809
	public Vector2 bottomLeft;

	// Token: 0x040016B2 RID: 5810
	public Vector2 topRight;

	// Token: 0x040016B3 RID: 5811
	protected AbstractCupheadCamera _camera;

	// Token: 0x040016B4 RID: 5812
	private bool _initialized;

	// Token: 0x040016B5 RID: 5813
	private Vector3 _startPosition;

	// Token: 0x040016B6 RID: 5814
	private Vector3 _cameraStartPosition;

	// Token: 0x040016B7 RID: 5815
	[SerializeField]
	private bool overrideCameraRange;

	// Token: 0x040016B8 RID: 5816
	[SerializeField]
	private MinMax overrideCameraX;

	// Token: 0x040016B9 RID: 5817
	[SerializeField]
	private MinMax overrideCameraY;

	// Token: 0x020003DE RID: 990
	public enum Type
	{
		// Token: 0x040016BB RID: 5819
		MinMax,
		// Token: 0x040016BC RID: 5820
		Comparative,
		// Token: 0x040016BD RID: 5821
		Centered
	}
}
