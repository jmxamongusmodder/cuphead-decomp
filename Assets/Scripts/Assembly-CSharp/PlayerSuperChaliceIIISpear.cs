using System;
using UnityEngine;

// Token: 0x02000A54 RID: 2644
public class PlayerSuperChaliceIIISpear : AbstractProjectile
{
	// Token: 0x06003F04 RID: 16132 RVA: 0x00228837 File Offset: 0x00226C37
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x06003F05 RID: 16133 RVA: 0x00228839 File Offset: 0x00226C39
	protected override void Start()
	{
		this._countParryTowardsScore = false;
		this.basePos = base.transform.position;
	}

	// Token: 0x06003F06 RID: 16134 RVA: 0x00228853 File Offset: 0x00226C53
	public void DetachFromSuper(LevelPlayerController p)
	{
		this.sourcePlayer = p;
		this.sourcePlayer.weaponManager.OnSuperStart += this.Die;
		base.transform.parent = null;
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x00228885 File Offset: 0x00226C85
	public override void OnParry(AbstractPlayerController player)
	{
		AudioManager.Play("player_super_chalice_barrage_spearparry");
		base.OnParry(player);
	}

	// Token: 0x06003F08 RID: 16136 RVA: 0x00228898 File Offset: 0x00226C98
	public override void OnParryDie()
	{
		this.Die();
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x002288A0 File Offset: 0x00226CA0
	protected override void Die()
	{
		this.coll.enabled = false;
		base.animator.Play("Die");
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x002288BE File Offset: 0x00226CBE
	protected override void OnDestroy()
	{
		if (this.sourcePlayer != null)
		{
			this.sourcePlayer.weaponManager.OnSuperStart -= this.Die;
		}
		base.OnDestroy();
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x002288F4 File Offset: 0x00226CF4
	protected override void FixedUpdate()
	{
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x002288F8 File Offset: 0x00226CF8
	protected override void Update()
	{
		this.floatT += CupheadTime.Delta * this.floatSpeed;
		base.transform.position = new Vector3(this.basePos.x, this.basePos.y + Mathf.Sin(this.floatT) * this.floatAmplitude);
		this.timer += CupheadTime.Delta;
		if (this.timer > 10f)
		{
			this.Die();
		}
		if (base.transform.parent == null && this.sourcePlayer == null)
		{
			this.Die();
		}
	}

	// Token: 0x0400461D RID: 17949
	private const float EXPIRE_TIME = 10f;

	// Token: 0x0400461E RID: 17950
	[SerializeField]
	private BoxCollider2D coll;

	// Token: 0x0400461F RID: 17951
	[SerializeField]
	private float floatAmplitude = 20f;

	// Token: 0x04004620 RID: 17952
	[SerializeField]
	private float floatT;

	// Token: 0x04004621 RID: 17953
	[SerializeField]
	private float floatSpeed = 1f;

	// Token: 0x04004622 RID: 17954
	private Vector3 basePos;

	// Token: 0x04004623 RID: 17955
	private float timer;

	// Token: 0x04004624 RID: 17956
	public LevelPlayerController sourcePlayer;
}
