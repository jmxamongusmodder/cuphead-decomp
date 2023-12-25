using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056B RID: 1387
public class ClownLevelPenguinBullet : BasicProjectile
{
	// Token: 0x06001A37 RID: 6711 RVA: 0x000EFF46 File Offset: 0x000EE346
	protected override void Start()
	{
		base.Start();
		this.move = false;
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x000EFF64 File Offset: 0x000EE364
	private IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		this.move = true;
		this.bulletFX.Create(this.root.transform.position).transform.SetEulerAngles(null, null, new float?(base.transform.eulerAngles.z - 90f));
		yield return null;
		yield break;
	}

	// Token: 0x04002354 RID: 9044
	[SerializeField]
	private Effect bulletFX;

	// Token: 0x04002355 RID: 9045
	[SerializeField]
	private Transform root;
}
