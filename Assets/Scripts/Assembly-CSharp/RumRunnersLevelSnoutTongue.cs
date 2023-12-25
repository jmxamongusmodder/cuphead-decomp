using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200079E RID: 1950
public class RumRunnersLevelSnoutTongue : ParrySwitch
{
	// Token: 0x1400004A RID: 74
	// (add) Token: 0x06002B65 RID: 11109 RVA: 0x001940A0 File Offset: 0x001924A0
	// (remove) Token: 0x06002B66 RID: 11110 RVA: 0x001940D8 File Offset: 0x001924D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnPlayerCollision;

	// Token: 0x06002B67 RID: 11111 RVA: 0x0019410E File Offset: 0x0019250E
	private void OnEnable()
	{
		base.GetComponent<CollisionChild>().OnPlayerCollision += this.onPlayerCollision;
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x00194127 File Offset: 0x00192527
	private void OnDisable()
	{
		base.GetComponent<CollisionChild>().OnPlayerCollision -= this.onPlayerCollision;
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x00194140 File Offset: 0x00192540
	protected override void Awake()
	{
		base.Awake();
		base.gameObject.tag = "Enemy";
		this.collisionChild = base.GetComponent<CollisionChild>();
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x00194164 File Offset: 0x00192564
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		if (this.parrySpark)
		{
			this.parrySpark.Create(player.transform.position);
		}
		base.FirePrePauseEvent();
		player.stats.ParryOneQuarter();
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x0019419E File Offset: 0x0019259E
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.IsParryable = false;
		this.collisionChild.enabled = false;
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06002B6C RID: 11116 RVA: 0x001941C8 File Offset: 0x001925C8
	private IEnumerator parryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.IsParryable = true;
		this.collisionChild.enabled = true;
		yield break;
	}

	// Token: 0x06002B6D RID: 11117 RVA: 0x001941E3 File Offset: 0x001925E3
	private void onPlayerCollision(GameObject hit, CollisionPhase phase)
	{
		if (base.IsParryable && this.OnPlayerCollision != null)
		{
			this.OnPlayerCollision(hit, phase);
		}
	}

	// Token: 0x0400341A RID: 13338
	private CollisionChild collisionChild;
}
