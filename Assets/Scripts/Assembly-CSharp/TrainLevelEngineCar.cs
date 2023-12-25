using System;
using UnityEngine;

// Token: 0x02000817 RID: 2071
public class TrainLevelEngineCar : AbstractPausableComponent
{
	// Token: 0x06003006 RID: 12294 RVA: 0x001C62B0 File Offset: 0x001C46B0
	public void PlayRage()
	{
		AudioManager.Play("train_engine_car_rage_loop");
		this.emitAudioFromObject.Add("train_engine_car_rage_loop");
		base.animator.Play("Rage");
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x001C62DC File Offset: 0x001C46DC
	public void End()
	{
		base.animator.Play("Idle");
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x001C62EE File Offset: 0x001C46EE
	private void SteamEffect()
	{
		this.steamEffect.Create(this.steamRoot.position);
	}

	// Token: 0x06003009 RID: 12297 RVA: 0x001C6307 File Offset: 0x001C4707
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.steamEffect = null;
	}

	// Token: 0x040038DD RID: 14557
	[SerializeField]
	private Transform steamRoot;

	// Token: 0x040038DE RID: 14558
	[SerializeField]
	private Effect steamEffect;
}
