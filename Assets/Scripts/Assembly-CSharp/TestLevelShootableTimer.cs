using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class TestLevelShootableTimer : AbstractCollidableObject
{
	// Token: 0x0600138B RID: 5003 RVA: 0x000AC0CC File Offset: 0x000AA4CC
	private void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.child.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x000AC120 File Offset: 0x000AA520
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			this.timerStarted = true;
		}
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x000AC135 File Offset: 0x000AA535
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.timerStarted)
		{
			this.damageTaken += info.damage;
		}
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x000AC158 File Offset: 0x000AA558
	private IEnumerator timer_cr()
	{
		for (;;)
		{
			float t = 0f;
			if (this.timerStarted)
			{
				while (t < this.maxTime)
				{
					t += CupheadTime.Delta;
					yield return null;
				}
				yield return null;
				this.damageTaken = 0f;
				this.timerStarted = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001C9A RID: 7322
	[SerializeField]
	private float maxTime = 3f;

	// Token: 0x04001C9B RID: 7323
	[SerializeField]
	private DamageReceiver child;

	// Token: 0x04001C9C RID: 7324
	private DamageReceiver damageReceiver;

	// Token: 0x04001C9D RID: 7325
	private float damageTaken;

	// Token: 0x04001C9E RID: 7326
	private bool timerStarted;
}
