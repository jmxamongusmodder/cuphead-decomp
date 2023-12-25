using System;
using UnityEngine;

// Token: 0x0200081B RID: 2075
public class TrainLevelGhostCannons : LevelProperties.Train.Entity
{
	// Token: 0x06003025 RID: 12325 RVA: 0x001C6D1E File Offset: 0x001C511E
	public void Shoot(int cannon)
	{
		if (!this.shooting)
		{
			return;
		}
		this.cannon = cannon;
		base.animator.SetInteger("Cannon", cannon);
		base.animator.SetTrigger("OnShoot");
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x001C6D54 File Offset: 0x001C5154
	public void End()
	{
		this.shooting = false;
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x001C6D60 File Offset: 0x001C5160
	private void ShootAnim()
	{
		if (!this.shooting)
		{
			return;
		}
		AudioManager.Play("train_cannon_shoot");
		this.emitAudioFromObject.Add("train_cannon_shoot");
		this.cannonSmoke.Create(this.cannonRoots[this.cannon].position);
		this.ghostPrefab.Create(this.cannonRoots[this.cannon].position, base.properties.CurrentState.lollipopGhouls.ghostDelay, base.properties.CurrentState.lollipopGhouls.ghostSpeed, base.properties.CurrentState.lollipopGhouls.ghostAimSpeed, base.properties.CurrentState.lollipopGhouls.ghostHealth, base.properties.CurrentState.lollipopGhouls.skullSpeed);
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x001C6E38 File Offset: 0x001C5238
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.ghostPrefab = null;
		this.cannonSmoke = null;
	}

	// Token: 0x040038EE RID: 14574
	[SerializeField]
	private Effect cannonSmoke;

	// Token: 0x040038EF RID: 14575
	[SerializeField]
	private Transform[] cannonRoots;

	// Token: 0x040038F0 RID: 14576
	[SerializeField]
	private TrainLevelGhostCannonGhost ghostPrefab;

	// Token: 0x040038F1 RID: 14577
	private int cannon;

	// Token: 0x040038F2 RID: 14578
	private bool shooting = true;
}
