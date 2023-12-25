using System;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
public class AirplaneLevelTurretDog : AbstractPausableComponent
{
	// Token: 0x060014BC RID: 5308 RVA: 0x000BA1F4 File Offset: 0x000B85F4
	private void ShootProjectile()
	{
		this.FX.Create(base.transform.position, base.transform.localScale);
		Vector3 v = new Vector3(this.rootPos.position.x + 30f, this.rootPos.position.y);
		Vector3 v2 = new Vector3(this.rootPos.position.x - 30f, this.rootPos.position.y);
		AirplaneLevelTurretBullet airplaneLevelTurretBullet = this.bulletPrefab.Create(v, new Vector3(this.velocityX, this.velocityY), this.gravity);
		airplaneLevelTurretBullet.GetComponent<SpriteRenderer>().sortingOrder = 1;
		airplaneLevelTurretBullet.GetComponent<Animator>().Play("TennisBallA");
		AirplaneLevelTurretBullet airplaneLevelTurretBullet2 = this.bulletPrefab.Create(this.rootPos.position, new Vector3(0f, this.velocityY), this.gravity);
		airplaneLevelTurretBullet2.GetComponent<SpriteRenderer>().sortingOrder = 2;
		airplaneLevelTurretBullet2.GetComponent<Animator>().Play("TennisBallB");
		AirplaneLevelTurretBullet airplaneLevelTurretBullet3 = this.bulletPrefab.Create(v2, new Vector3(-this.velocityX, this.velocityY), this.gravity);
		airplaneLevelTurretBullet3.GetComponent<SpriteRenderer>().sortingOrder = 1;
		airplaneLevelTurretBullet3.GetComponent<Animator>().Play("TennisBallC");
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x000BA37C File Offset: 0x000B877C
	public void StartAttack(float velocityX, float velocityY, float gravity)
	{
		base.animator.Play("Flap");
		this.velocityX = velocityX;
		this.velocityY = velocityY;
		this.gravity = gravity;
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x000BA3A3 File Offset: 0x000B87A3
	private void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogHatchOpen()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_hatchopen");
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x000BA3AF File Offset: 0x000B87AF
	private void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogBark()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_bark");
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x000BA3BB File Offset: 0x000B87BB
	private void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogWhistle()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_whistle");
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x000BA3C7 File Offset: 0x000B87C7
	private void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogToss()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_toss");
	}

	// Token: 0x04001E29 RID: 7721
	private const float BALL_OFFSET = 30f;

	// Token: 0x04001E2A RID: 7722
	[SerializeField]
	private AirplaneLevelTurretBullet bulletPrefab;

	// Token: 0x04001E2B RID: 7723
	[SerializeField]
	private Transform rootPos;

	// Token: 0x04001E2C RID: 7724
	[SerializeField]
	private Effect FX;

	// Token: 0x04001E2D RID: 7725
	private float velocityX;

	// Token: 0x04001E2E RID: 7726
	private float velocityY;

	// Token: 0x04001E2F RID: 7727
	private float gravity;
}
