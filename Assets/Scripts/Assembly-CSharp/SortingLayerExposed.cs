using System;
using UnityEngine;

// Token: 0x02000B2B RID: 2859
[ExecuteInEditMode]
public sealed class SortingLayerExposed : MonoBehaviour
{
	// Token: 0x06004556 RID: 17750 RVA: 0x00247EB7 File Offset: 0x002462B7
	public void OnValidate()
	{
		this.Apply();
	}

	// Token: 0x06004557 RID: 17751 RVA: 0x00247EBF File Offset: 0x002462BF
	public void OnEnable()
	{
		this.Apply();
	}

	// Token: 0x06004558 RID: 17752 RVA: 0x00247EC8 File Offset: 0x002462C8
	private void Apply()
	{
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		component.sortingLayerName = this.sortingLayerName;
		component.sortingOrder = this.sortingOrder;
	}

	// Token: 0x04004AF1 RID: 19185
	[SerializeField]
	private string sortingLayerName = "Default";

	// Token: 0x04004AF2 RID: 19186
	[SerializeField]
	private int sortingOrder;
}
