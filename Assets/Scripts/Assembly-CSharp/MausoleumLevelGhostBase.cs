using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006D7 RID: 1751
public class MausoleumLevelGhostBase : BasicProjectile
{
	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06002546 RID: 9542 RVA: 0x0015CD95 File Offset: 0x0015B195
	// (set) Token: 0x06002547 RID: 9543 RVA: 0x0015CD9D File Offset: 0x0015B19D
	public bool isDead { get; private set; }

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06002548 RID: 9544 RVA: 0x0015CDA6 File Offset: 0x0015B1A6
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06002549 RID: 9545 RVA: 0x0015CDAD File Offset: 0x0015B1AD
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x0015CDB4 File Offset: 0x0015B1B4
	protected override void Start()
	{
		base.Start();
		this.isDead = false;
		if (base.transform.position.x > 0f)
		{
			base.GetComponent<SpriteRenderer>().flipY = true;
		}
		this.SetParryable(true);
		if (this.Counts)
		{
			MausoleumLevel.SPAWNCOUNTER++;
		}
		base.StartCoroutine(this.check_dist_cr());
		if (this.hasIdleSFX)
		{
		}
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x0015CE2D File Offset: 0x0015B22D
	public void GetParent(MausoleumLevel parent)
	{
		this.parent = parent;
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x0015CE36 File Offset: 0x0015B236
	public override void OnParry(AbstractPlayerController player)
	{
		this.Die();
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x0015CE3E File Offset: 0x0015B23E
	public void OnBossDeath()
	{
		this.Die();
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x0015CE46 File Offset: 0x0015B246
	protected override void Die()
	{
		this.StopAllCoroutines();
		this.isDead = true;
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x0015CE67 File Offset: 0x0015B267
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x0015CE70 File Offset: 0x0015B270
	private IEnumerator check_dist_cr()
	{
		while (Vector3.Distance(base.transform.position, MausoleumLevelUrn.URN_POS) > 30f)
		{
			yield return null;
		}
		if (this.parent.LoseGame != null)
		{
			this.parent.LoseGame();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x0015CE8C File Offset: 0x0015B28C
	private IEnumerator idle_sound_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.idleDelay.RandomFloat());
			AudioManager.Play(this.idleSound);
			this.emitAudioFromObject.Add(this.idleSound);
			while (AudioManager.CheckIfPlaying(this.idleSound))
			{
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002552 RID: 9554 RVA: 0x0015CEA7 File Offset: 0x0015B2A7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x04002DE3 RID: 11747
	[SerializeField]
	private MinMax idleDelay;

	// Token: 0x04002DE4 RID: 11748
	[SerializeField]
	private string idleSound;

	// Token: 0x04002DE5 RID: 11749
	[SerializeField]
	private bool hasIdleSFX;

	// Token: 0x04002DE6 RID: 11750
	public bool Counts;

	// Token: 0x04002DE8 RID: 11752
	private const float DIST_TO_DIE = 30f;

	// Token: 0x04002DE9 RID: 11753
	protected MausoleumLevel parent;
}
