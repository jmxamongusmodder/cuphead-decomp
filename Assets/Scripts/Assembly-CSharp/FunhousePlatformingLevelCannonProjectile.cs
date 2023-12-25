using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B0 RID: 2224
public class FunhousePlatformingLevelCannonProjectile : BasicProjectile
{
	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x060033D3 RID: 13267 RVA: 0x001E14D9 File Offset: 0x001DF8D9
	// (set) Token: 0x060033D4 RID: 13268 RVA: 0x001E14E1 File Offset: 0x001DF8E1
	public EnemyProperties Properties { get; set; }

	// Token: 0x060033D5 RID: 13269 RVA: 0x001E14EA File Offset: 0x001DF8EA
	protected override void Start()
	{
		base.Start();
		base.animator.Play("anim_level_starcannon_bullet", -1, UnityEngine.Random.value);
	}

	// Token: 0x060033D6 RID: 13270 RVA: 0x001E1508 File Offset: 0x001DF908
	public void Init()
	{
		base.StartCoroutine(this.delayedDeath_cr());
	}

	// Token: 0x060033D7 RID: 13271 RVA: 0x001E1518 File Offset: 0x001DF918
	private IEnumerator delayedDeath_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.Properties.bulletDeathTime);
		this.Die();
		yield break;
	}

	// Token: 0x060033D8 RID: 13272 RVA: 0x001E1534 File Offset: 0x001DF934
	protected override void Die()
	{
		base.Die();
		Effect effect = this.deathFx.Create(base.transform.position, new Vector3(1.25f, 1.25f, 1f));
		effect.animator.SetInteger("PickAni", UnityEngine.Random.Range(0, 3));
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060033D9 RID: 13273 RVA: 0x001E1594 File Offset: 0x001DF994
	protected override void Move()
	{
		if (this.Speed == 0f)
		{
		}
		base.transform.position += this.direction * this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
		this._accumulativeGravity += this.Gravity * CupheadTime.FixedDelta;
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x001E161B File Offset: 0x001DFA1B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.deathFx = null;
	}

	// Token: 0x04003C20 RID: 15392
	[SerializeField]
	private Effect deathFx;

	// Token: 0x04003C22 RID: 15394
	public Vector3 direction;
}
