using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200061A RID: 1562
public class FlyingBirdLevelBirdFeather : AbstractProjectile
{
	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06001FBB RID: 8123 RVA: 0x00123B3B File Offset: 0x00121F3B
	// (set) Token: 0x06001FBC RID: 8124 RVA: 0x00123B43 File Offset: 0x00121F43
	public float Speed { get; private set; }

	// Token: 0x06001FBD RID: 8125 RVA: 0x00123B4C File Offset: 0x00121F4C
	public virtual AbstractProjectile Init(float speed)
	{
		this.Speed = speed;
		base.ResetLifetime();
		base.ResetDistance();
		return this;
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x00123B64 File Offset: 0x00121F64
	protected override void Update()
	{
		base.Update();
		base.transform.position += -base.transform.right * this.Speed * CupheadTime.Delta;
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x00123BB7 File Offset: 0x00121FB7
	private void OnEnable()
	{
		this.DamagesType.OnlyPlayer();
		this.CollisionDeath.OnlyPlayer();
		this.SetCollider(true);
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x00123BD7 File Offset: 0x00121FD7
	private void OnDisable()
	{
		this.SetCollider(false);
		this.StopAllCoroutines();
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x00123BE6 File Offset: 0x00121FE6
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x00123C04 File Offset: 0x00122004
	private void SetCollider(bool c)
	{
		base.GetComponent<BoxCollider2D>().enabled = c;
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x00123C14 File Offset: 0x00122014
	private IEnumerator effect_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 0.3f));
		for (;;)
		{
			this.effectPrefab.Create(this.effectRoot.position);
			yield return CupheadTime.WaitForSeconds(this, 0.3f);
		}
		yield break;
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x00123C2F File Offset: 0x0012202F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x00123C3E File Offset: 0x0012203E
	public override void OnParryDie()
	{
		this.Recycle<FlyingBirdLevelBirdFeather>();
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x00123C46 File Offset: 0x00122046
	protected override void OnDieDistance()
	{
		this.Recycle<FlyingBirdLevelBirdFeather>();
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x00123C4E File Offset: 0x0012204E
	protected override void OnDieLifetime()
	{
		this.Recycle<FlyingBirdLevelBirdFeather>();
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x00123C56 File Offset: 0x00122056
	protected override void OnDieAnimationComplete()
	{
		this.Recycle<FlyingBirdLevelBirdFeather>();
	}

	// Token: 0x04002848 RID: 10312
	[SerializeField]
	private Effect effectPrefab;

	// Token: 0x04002849 RID: 10313
	[SerializeField]
	private Transform effectRoot;
}
