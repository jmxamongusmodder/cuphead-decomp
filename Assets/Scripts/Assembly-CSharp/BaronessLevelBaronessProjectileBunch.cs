using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004DE RID: 1246
public class BaronessLevelBaronessProjectileBunch : AbstractProjectile
{
	// Token: 0x06001560 RID: 5472 RVA: 0x000BF464 File Offset: 0x000BD864
	public void Init(Vector2 pos, float velocity, float pointAt, LevelProperties.Baroness.BaronessVonBonbon properties, BaronessLevelCastle parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.pointAt = MathUtils.AngleToDirection(pointAt);
		this.velocity = velocity;
		this.parent = parent;
		this.parent.OnDeathEvent += this.KillProjectileBunch;
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x000BF4C1 File Offset: 0x000BD8C1
	private void KillProjectileBunch()
	{
		this.isActive = false;
	}

	// Token: 0x06001562 RID: 5474 RVA: 0x000BF4CA File Offset: 0x000BD8CA
	protected override void Awake()
	{
		base.Awake();
		this.isActive = true;
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x000BF4D9 File Offset: 0x000BD8D9
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.scale_up_cr());
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x000BF4EE File Offset: 0x000BD8EE
	protected override void Update()
	{
		base.Update();
		if (!this.isActive)
		{
			this.Dying();
		}
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x000BF507 File Offset: 0x000BD907
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		base.transform.position += this.pointAt * this.velocity * CupheadTime.FixedDelta;
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x000BF540 File Offset: 0x000BD940
	private IEnumerator scale_up_cr()
	{
		float t = 0f;
		float time = 0.3f;
		base.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetScale(new float?(t / time), new float?(t / time), new float?(t / time));
			yield return new WaitForFixedUpdate();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x000BF55B File Offset: 0x000BD95B
	private void Dying()
	{
		if (base.GetComponent<SpriteRenderer>() != null)
		{
			base.GetComponent<SpriteRenderer>().enabled = false;
		}
		base.Die();
	}

	// Token: 0x04001EB7 RID: 7863
	public LevelProperties.Baroness.BaronessVonBonbon properties;

	// Token: 0x04001EB8 RID: 7864
	private BaronessLevelCastle parent;

	// Token: 0x04001EB9 RID: 7865
	private float velocity;

	// Token: 0x04001EBA RID: 7866
	private bool isActive;

	// Token: 0x04001EBB RID: 7867
	private Vector3 pointAt;
}
