using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003D1 RID: 977
[RequireComponent(typeof(Camera))]
public abstract class AbstractCupheadCamera : AbstractMonoBehaviour
{
	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x000891E7 File Offset: 0x000875E7
	public Camera camera
	{
		get
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			return this._camera;
		}
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x0008920C File Offset: 0x0008760C
	public bool ContainsPoint(Vector2 point)
	{
		return this.ContainsPoint(point, Vector2.zero);
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x0008921C File Offset: 0x0008761C
	public bool ContainsPoint(Vector2 point, Vector2 padding)
	{
		return this.CalculateContainsBounds(padding).Contains(point);
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x0008923C File Offset: 0x0008763C
	public Rect CalculateContainsBounds(Vector2 padding)
	{
		float orthographicSize = this.camera.orthographicSize;
		Vector3 position = base.transform.position;
		float width = orthographicSize * 1.7777778f * 2f + padding.x * 2f;
		float height = orthographicSize * 2f + padding.y * 2f;
		return RectUtils.NewFromCenter(position.x, position.y, width, height);
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x000892AE File Offset: 0x000876AE
	protected override void Awake()
	{
		base.Awake();
		this.camera.clearFlags = CameraClearFlags.Nothing;
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x000892C4 File Offset: 0x000876C4
	public Rect Bounds
	{
		get
		{
			float width = this.camera.orthographicSize * 1.7777778f * 2f;
			float height = this.camera.orthographicSize * 2f;
			return RectUtils.NewFromCenter(base.transform.position.x, base.transform.position.y, width, height);
		}
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x0008932D File Offset: 0x0008772D
	protected virtual void LateUpdate()
	{
		this.UpdateRect();
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00089338 File Offset: 0x00087738
	public void UpdateRect()
	{
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = 1f - 0.1f * SettingsData.Data.overscan;
		Rect rect;
		if (num > 1.7777778f)
		{
			rect = RectUtils.NewFromCenter(0.5f, 0.5f, num2 * 1.7777778f / num, num2 * 1f);
		}
		else
		{
			rect = RectUtils.NewFromCenter(0.5f, 0.5f, num2 * 1f, num2 * num / 1.7777778f);
		}
		if (this.camera.rect != rect)
		{
			this.camera.rect = rect;
			CanvasScaler[] array = UnityEngine.Object.FindObjectsOfType<CanvasScaler>();
			foreach (CanvasScaler canvasScaler in array)
			{
				canvasScaler.referenceResolution = new Vector2(1280f / rect.height, 720f / rect.height);
			}
		}
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0008942B File Offset: 0x0008782B
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.1f);
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0008943E File Offset: 0x0008783E
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x00089454 File Offset: 0x00087854
	private void DrawGizmos(float a)
	{
		Gizmos.color = Color.white * new Color(1f, 1f, 1f, a);
		Gizmos.DrawWireCube(this.camera.transform.position, new Vector3(this.camera.orthographicSize * this.camera.aspect * 2f, this.camera.orthographicSize * 2f, 0f));
		Gizmos.DrawWireSphere(this.camera.transform.position, 50f);
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Gizmos.DrawWireSphere(this.camera.transform.position, 10f);
	}

	// Token: 0x04001642 RID: 5698
	private Camera _camera;
}
