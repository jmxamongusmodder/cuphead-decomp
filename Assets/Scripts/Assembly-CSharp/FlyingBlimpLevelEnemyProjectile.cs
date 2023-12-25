using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class FlyingBlimpLevelEnemyProjectile : BasicProjectile
{
	// Token: 0x0600209A RID: 8346 RVA: 0x0012C8D3 File Offset: 0x0012ACD3
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_fx_cr());
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x0012C8E8 File Offset: 0x0012ACE8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.animator.SetTrigger("dead");
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x0012C902 File Offset: 0x0012AD02
	private void Destroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x0012C910 File Offset: 0x0012AD10
	private IEnumerator spawn_fx_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.17f);
		for (;;)
		{
			this.FX.Create(this.root.transform.position).transform.SetEulerAngles(null, null, new float?(base.transform.eulerAngles.z));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x0400291F RID: 10527
	[SerializeField]
	private Effect FX;

	// Token: 0x04002920 RID: 10528
	[SerializeField]
	private Transform root;
}
