using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B11 RID: 2833
public class EffectSpawner : AbstractMonoBehaviour
{
	// Token: 0x060044BB RID: 17595 RVA: 0x00246424 File Offset: 0x00244824
	private void Start()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x060044BC RID: 17596 RVA: 0x00246434 File Offset: 0x00244834
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(base.baseTransform.position, 5f);
		Gizmos.color = Color.red;
		Vector3 vector = base.baseTransform.position + this.offset;
		Gizmos.DrawLine(base.transform.position, vector);
		Gizmos.DrawWireSphere(vector, 5f);
	}

	// Token: 0x060044BD RID: 17597 RVA: 0x002464A8 File Offset: 0x002448A8
	private IEnumerator loop_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.delay);
			Transform t = this.effectPrefab.Create(base.transform.position).transform;
			t.SetParent(base.transform);
			t.ResetLocalTransforms();
			t.localPosition = this.offset;
			t.SetParent(null);
		}
		yield break;
	}

	// Token: 0x04004A73 RID: 19059
	[SerializeField]
	private Effect effectPrefab;

	// Token: 0x04004A74 RID: 19060
	public Vector2 offset;

	// Token: 0x04004A75 RID: 19061
	public float delay = 1f;
}
