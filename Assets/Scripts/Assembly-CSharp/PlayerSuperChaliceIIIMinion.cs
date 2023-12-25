using System;
using UnityEngine;

// Token: 0x02000A53 RID: 2643
public class PlayerSuperChaliceIIIMinion : BasicProjectileContinuesOnLevelEnd
{
	// Token: 0x06003EFE RID: 16126 RVA: 0x0022868B File Offset: 0x00226A8B
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x06003EFF RID: 16127 RVA: 0x0022868D File Offset: 0x00226A8D
	protected override void OnDieDistance()
	{
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x00228690 File Offset: 0x00226A90
	protected override void Start()
	{
		base.Start();
		this.startY = base.transform.position.y;
		this.t = UnityEngine.Random.Range(0f, 6.2831855f);
		this.wavelength = UnityEngine.Random.Range(150f, 300f);
		this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Super);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		meterScoreTracker.Add(this.damageDealer);
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x00228708 File Offset: 0x00226B08
	protected override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		this.impactFX.Create(Vector3.Lerp(base.transform.position, receiver.transform.position, UnityEngine.Random.Range(0f, 1f)) + UnityEngine.Random.insideUnitSphere * 25f);
		AudioManager.Play("player_super_chalice_barrage_impact");
	}

	// Token: 0x06003F02 RID: 16130 RVA: 0x00228774 File Offset: 0x00226B74
	protected override void Move()
	{
		base.transform.position += this.Direction * this.Speed * CupheadTime.FixedDelta;
		if (this.wave)
		{
			this.t += this.Speed * CupheadTime.FixedDelta;
			base.transform.position = new Vector3(base.transform.position.x, this.startY + Mathf.Sin(this.t / this.wavelength) * this.amplitude);
		}
	}

	// Token: 0x04004616 RID: 17942
	public int elementIndex;

	// Token: 0x04004617 RID: 17943
	private float wavelength = 180f;

	// Token: 0x04004618 RID: 17944
	private float amplitude = 20f;

	// Token: 0x04004619 RID: 17945
	private float t;

	// Token: 0x0400461A RID: 17946
	private float startY;

	// Token: 0x0400461B RID: 17947
	public bool wave;

	// Token: 0x0400461C RID: 17948
	[SerializeField]
	private Effect impactFX;
}
