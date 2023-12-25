using System;
using UnityEngine;

// Token: 0x020007D9 RID: 2009
public class SaltbakerLevelTable : MonoBehaviour
{
	// Token: 0x06002DE0 RID: 11744 RVA: 0x001B0E4A File Offset: 0x001AF24A
	private void Start()
	{
		base.GetComponent<MeshRenderer>().sortingOrder = -1;
		this.tableMesh = this.tableMeshFilter.mesh;
		this.vertices = this.tableMesh.vertices;
		this.cam = CupheadLevelCamera.Current;
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x001B0E88 File Offset: 0x001AF288
	private void Update()
	{
		float num = Mathf.InverseLerp(this.cam.Right, this.cam.Left, this.cam.transform.position.x) - 0.5f;
		this.vertices[0].x = -0.5f + num * this.skewFactor;
		this.vertices[2].x = 0.5f + num * this.skewFactor;
		this.tableMesh.vertices = this.vertices;
		this.tableMesh.RecalculateBounds();
	}

	// Token: 0x04003662 RID: 13922
	[SerializeField]
	private float skewFactor = 0.02f;

	// Token: 0x04003663 RID: 13923
	[SerializeField]
	private MeshFilter tableMeshFilter;

	// Token: 0x04003664 RID: 13924
	private Mesh tableMesh;

	// Token: 0x04003665 RID: 13925
	private Vector3[] vertices;

	// Token: 0x04003666 RID: 13926
	private CupheadLevelCamera cam;
}
