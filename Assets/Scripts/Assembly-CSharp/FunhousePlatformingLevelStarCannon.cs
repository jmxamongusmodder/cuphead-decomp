using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008BE RID: 2238
public class FunhousePlatformingLevelStarCannon : PlatformingLevelPathMovementEnemy
{
	// Token: 0x06003438 RID: 13368 RVA: 0x001E4BDC File Offset: 0x001E2FDC
	protected override void Start()
	{
		base.Start();
		if (this.killable)
		{
			base._damageReceiver.enabled = false;
		}
		base.animator.SetBool("IsA", Rand.Bool());
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06003439 RID: 13369 RVA: 0x001E4C28 File Offset: 0x001E3028
	protected override void OnStart()
	{
		base.OnStart();
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x0600343A RID: 13370 RVA: 0x001E4C40 File Offset: 0x001E3040
	private IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x001E4C5C File Offset: 0x001E305C
	private IEnumerator shoot_cr()
	{
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.cannonShotDelay);
			base.animator.SetBool("isShooting", true);
			while (!this.justShot)
			{
				yield return null;
			}
			this.justShot = false;
			yield return CupheadTime.WaitForSeconds(this, 0.7f);
			base.animator.SetBool("isShooting", false);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x001E4C78 File Offset: 0x001E3078
	private void ShootStraight()
	{
		this.justShot = true;
		AudioManager.Play("funhouse_starcannon_shoot");
		this.emitAudioFromObject.Add("funhouse_starcannon_shoot");
		this.StraightFX();
		for (int i = 0; i < this.straightRootPositions.Length; i++)
		{
			FunhousePlatformingLevelCannonProjectile funhousePlatformingLevelCannonProjectile = this.projectile.Create(this.straightRootPositions[i].transform.position, 0f, base.Properties.cannonSpeed) as FunhousePlatformingLevelCannonProjectile;
			funhousePlatformingLevelCannonProjectile.direction = this.straightRootPositions[i].transform.rotation * Vector3.right;
			funhousePlatformingLevelCannonProjectile.Properties = base.Properties;
			funhousePlatformingLevelCannonProjectile.Init();
		}
	}

	// Token: 0x0600343D RID: 13373 RVA: 0x001E4D34 File Offset: 0x001E3134
	private void ShootDiag()
	{
		this.justShot = true;
		AudioManager.Play("funhouse_starcannon_shoot");
		this.emitAudioFromObject.Add("funhouse_starcannon_shoot");
		this.DiagFX();
		for (int i = 0; i < this.diagRootPositions.Length; i++)
		{
			FunhousePlatformingLevelCannonProjectile funhousePlatformingLevelCannonProjectile = this.projectile.Create(this.diagRootPositions[i].transform.position, 0f, base.Properties.cannonSpeed) as FunhousePlatformingLevelCannonProjectile;
			funhousePlatformingLevelCannonProjectile.direction = this.diagRootPositions[i].transform.rotation * Vector3.right;
			funhousePlatformingLevelCannonProjectile.Properties = base.Properties;
			funhousePlatformingLevelCannonProjectile.Init();
		}
	}

	// Token: 0x0600343E RID: 13374 RVA: 0x001E4DED File Offset: 0x001E31ED
	private void DiagFX()
	{
		this.diagFX.Create(base.transform.position);
	}

	// Token: 0x0600343F RID: 13375 RVA: 0x001E4E06 File Offset: 0x001E3206
	private void StraightFX()
	{
		this.straightFX.Create(base.transform.position);
	}

	// Token: 0x06003440 RID: 13376 RVA: 0x001E4E1F File Offset: 0x001E321F
	private void SoundCannonRotate()
	{
		AudioManager.Play("funhouse_starcannon_rotation");
		this.emitAudioFromObject.Add("funhouse_starcannon_rotation");
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x001E4E3B File Offset: 0x001E323B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
		this.diagFX = null;
		this.straightFX = null;
	}

	// Token: 0x04003C75 RID: 15477
	[SerializeField]
	private Effect diagFX;

	// Token: 0x04003C76 RID: 15478
	[SerializeField]
	private Effect straightFX;

	// Token: 0x04003C77 RID: 15479
	[SerializeField]
	private bool killable;

	// Token: 0x04003C78 RID: 15480
	[SerializeField]
	private Transform[] diagRootPositions;

	// Token: 0x04003C79 RID: 15481
	[SerializeField]
	private Transform[] straightRootPositions;

	// Token: 0x04003C7A RID: 15482
	[SerializeField]
	private FunhousePlatformingLevelCannonProjectile projectile;

	// Token: 0x04003C7B RID: 15483
	private float offset = 50f;

	// Token: 0x04003C7C RID: 15484
	private bool justShot;
}
