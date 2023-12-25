using System;
using UnityEngine;

// Token: 0x02000598 RID: 1432
public class DevilLevelSplitDevilWall : AbstractProjectile
{
	// Token: 0x06001B71 RID: 7025 RVA: 0x000FB094 File Offset: 0x000F9494
	public DevilLevelSplitDevilWall Create(float xPos, float xVelocity, float distance, DevilLevelSplitDevil devil)
	{
		DevilLevelSplitDevilWall devilLevelSplitDevilWall = base.Create(new Vector2(xPos, 30f)) as DevilLevelSplitDevilWall;
		devilLevelSplitDevilWall.xVelocity = xVelocity;
		devilLevelSplitDevilWall.DestroyDistance = distance;
		devilLevelSplitDevilWall.devil = devil;
		devilLevelSplitDevilWall.UpdateColor();
		CupheadLevelCamera.Current.StartShake(4f);
		return devilLevelSplitDevilWall;
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x000FB0E4 File Offset: 0x000F94E4
	protected override void Update()
	{
		base.Update();
		if (base.dead)
		{
			return;
		}
		if (this.devil == null)
		{
			this.Die();
			return;
		}
		base.transform.AddPosition(this.xVelocity * CupheadTime.Delta, 0f, 0f);
		base.transform.SetScale(new float?(UnityEngine.Random.Range(0.9f, 1f)), null, null);
		this.UpdateColor();
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x000FB178 File Offset: 0x000F9578
	private void UpdateColor()
	{
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x000FB17A File Offset: 0x000F957A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x000FB198 File Offset: 0x000F9598
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x000FB1AB File Offset: 0x000F95AB
	protected override void OnDestroy()
	{
		if (UnityEngine.Object.FindObjectsOfType<DevilLevelSplitDevilWall>().Length <= 1)
		{
			CupheadLevelCamera.Current.EndShake(0.5f);
		}
		base.OnDestroy();
	}

	// Token: 0x040024A4 RID: 9380
	private float xVelocity;

	// Token: 0x040024A5 RID: 9381
	private DevilLevelSplitDevil devil;

	// Token: 0x040024A6 RID: 9382
	private const float Y_POS = 30f;
}
