using System;
using UnityEngine;

// Token: 0x02000A10 RID: 2576
public class HouseElderKettlePlayerRun : MonoBehaviour
{
	// Token: 0x06003CE1 RID: 15585 RVA: 0x0021AD75 File Offset: 0x00219175
	private void Start()
	{
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x0021AD77 File Offset: 0x00219177
	private void Update()
	{
		base.transform.localPosition += new Vector3(-490f, 0f, 0f) * CupheadTime.FixedDelta;
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x0021ADAD File Offset: 0x002191AD
	private void OnRunDust()
	{
		this.runEffect.Create(this.runDustRoot.position);
	}

	// Token: 0x0400442E RID: 17454
	[SerializeField]
	private Effect runEffect;

	// Token: 0x0400442F RID: 17455
	[SerializeField]
	private Transform runDustRoot;
}
