using System;
using UnityEngine;

// Token: 0x020005BC RID: 1468
public class DicePalaceDominoLevelRandomSpike : AbstractMonoBehaviour
{
	// Token: 0x06001C8C RID: 7308 RVA: 0x0010501F File Offset: 0x0010341F
	private void Start()
	{
		this.ChangeSpikes();
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x00105027 File Offset: 0x00103427
	public void ChangeSpikes()
	{
		base.animator.SetTrigger(this.states[UnityEngine.Random.Range(0, this.states.Length)]);
		this.melt = false;
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x00105050 File Offset: 0x00103450
	private void Update()
	{
		if (this.melt)
		{
			return;
		}
		if (base.transform.position.x <= -410f)
		{
			this.melt = true;
			base.animator.SetTrigger("Melt");
		}
	}

	// Token: 0x0400257B RID: 9595
	[SerializeField]
	private string[] states;

	// Token: 0x0400257C RID: 9596
	private bool melt;
}
