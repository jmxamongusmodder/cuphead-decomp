using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020003D5 RID: 981
public class CameraAnimationController : MonoBehaviour
{
	// Token: 0x06000CEB RID: 3307 RVA: 0x0008A418 File Offset: 0x00088818
	private void Start()
	{
		GameObject gameObject = GameObject.FindWithTag("MainCamera");
		this.Camera = gameObject.GetComponent<Camera>();
		this.MapCamera = gameObject.GetComponent<CupheadMapCamera>();
		this.BlurOptimized = gameObject.GetComponent<BlurOptimized>();
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0008A454 File Offset: 0x00088854
	private void Update()
	{
		this.ApplyProperties();
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x0008A45C File Offset: 0x0008885C
	private void OnEnable()
	{
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x0008A45E File Offset: 0x0008885E
	private void OnDisable()
	{
		this.ApplyProperties();
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0008A468 File Offset: 0x00088868
	private void ApplyProperties()
	{
		if (this.Blur > 0f && !this.BlurOptimized.enabled)
		{
			this.BlurOptimized.enabled = true;
		}
		if (this.Blur <= 0f && this.BlurOptimized.enabled)
		{
			this.BlurOptimized.enabled = false;
		}
		if (this.MapCamera != null)
		{
			this.MapCamera.centerOnPlayer = this.CenterOnPlayer;
			this.Camera.orthographicSize = this.OrthoSize;
		}
		this.BlurOptimized.blurSize = this.Blur;
	}

	// Token: 0x04001657 RID: 5719
	public bool CenterOnPlayer = true;

	// Token: 0x04001658 RID: 5720
	public float OrthoSize;

	// Token: 0x04001659 RID: 5721
	public float Blur;

	// Token: 0x0400165A RID: 5722
	private Camera Camera;

	// Token: 0x0400165B RID: 5723
	private CupheadMapCamera MapCamera;

	// Token: 0x0400165C RID: 5724
	private BlurOptimized BlurOptimized;
}
