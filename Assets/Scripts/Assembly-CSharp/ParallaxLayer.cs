using System;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class ParallaxLayer : AbstractPausableComponent
{
	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000D69 RID: 3433 RVA: 0x0008DB48 File Offset: 0x0008BF48
	protected Vector2 _offset
	{
		get
		{
			return this._startPosition - this._cameraStartPosition;
		}
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x0008DB60 File Offset: 0x0008BF60
	protected virtual void Start()
	{
		this._camera = CupheadLevelCamera.Current;
		this._startPosition = base.transform.position;
		this._cameraStartPosition = this._camera.transform.position;
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x0008DB94 File Offset: 0x0008BF94
	private void LateUpdate()
	{
		switch (this.type)
		{
		case ParallaxLayer.Type.MinMax:
			this.UpdateMinMax();
			break;
		default:
			this.UpdateComparative();
			break;
		case ParallaxLayer.Type.Centered:
			this.UpdateCentered();
			break;
		}
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x0008DBE0 File Offset: 0x0008BFE0
	protected virtual void UpdateComparative()
	{
		Vector3 position = base.transform.position;
		position.x = this._offset.x + this._camera.transform.position.x * this.percentage;
		position.y = this._offset.y + this._camera.transform.position.y * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x0008DC70 File Offset: 0x0008C070
	protected virtual void UpdateMinMax()
	{
		Vector3 position = base.transform.position;
		Vector2 vector = this._camera.transform.position;
		Vector2 zero = Vector2.zero;
		float num = vector.x + Mathf.Abs(this._camera.Left);
		float num2 = this._camera.Right + Mathf.Abs(this._camera.Left);
		float num3 = vector.y + Mathf.Abs(this._camera.Bottom);
		float num4 = this._camera.Top + Mathf.Abs(this._camera.Bottom);
		if (this.overrideCameraRange)
		{
			num = vector.x + Mathf.Abs(this.overrideCameraX.min);
			num3 = vector.y + Mathf.Abs(this.overrideCameraY.min);
			num2 = this.overrideCameraX.max - this.overrideCameraX.min;
			num4 = this.overrideCameraY.max - this.overrideCameraY.min;
		}
		zero.x = num / num2;
		zero.y = num3 / num4;
		if (float.IsNaN(zero.x))
		{
			zero.x = 0.5f;
		}
		if (float.IsNaN(zero.y))
		{
			zero.y = 0.5f;
		}
		position.x = Mathf.Lerp(this.bottomLeft.x, this.topRight.x, zero.x) + this._camera.transform.position.x;
		position.y = Mathf.Lerp(this.bottomLeft.y, this.topRight.y, zero.y) + this._camera.transform.position.y;
		base.transform.position = position;
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x0008DE68 File Offset: 0x0008C268
	private void UpdateCentered()
	{
		Vector3 position = base.transform.position;
		position.x = this._startPosition.x + (this._camera.transform.position.x - this._startPosition.x) * this.percentage;
		position.y = this._startPosition.y + (this._camera.transform.position.y - this._startPosition.y) * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x040016F2 RID: 5874
	public ParallaxLayer.Type type;

	// Token: 0x040016F3 RID: 5875
	[Range(-3f, 3f)]
	public float percentage;

	// Token: 0x040016F4 RID: 5876
	public Vector2 bottomLeft;

	// Token: 0x040016F5 RID: 5877
	public Vector2 topRight;

	// Token: 0x040016F6 RID: 5878
	protected CupheadLevelCamera _camera;

	// Token: 0x040016F7 RID: 5879
	private bool _initialized;

	// Token: 0x040016F8 RID: 5880
	private Vector3 _startPosition;

	// Token: 0x040016F9 RID: 5881
	private Vector3 _cameraStartPosition;

	// Token: 0x040016FA RID: 5882
	[SerializeField]
	private bool overrideCameraRange;

	// Token: 0x040016FB RID: 5883
	[SerializeField]
	private MinMax overrideCameraX;

	// Token: 0x040016FC RID: 5884
	[SerializeField]
	private MinMax overrideCameraY;

	// Token: 0x020003E8 RID: 1000
	public enum Type
	{
		// Token: 0x040016FE RID: 5886
		MinMax,
		// Token: 0x040016FF RID: 5887
		Comparative,
		// Token: 0x04001700 RID: 5888
		Centered
	}
}
