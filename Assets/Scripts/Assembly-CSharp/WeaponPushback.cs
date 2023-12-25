using System;
using System.Collections;

// Token: 0x02000A81 RID: 2689
public class WeaponPushback : AbstractLevelWeapon
{
	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x0600404A RID: 16458 RVA: 0x00230C8E File Offset: 0x0022F08E
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x0600404B RID: 16459 RVA: 0x00230C91 File Offset: 0x0022F091
	protected override float rapidFireRate
	{
		get
		{
			return this.bulletFireRate;
		}
	}

	// Token: 0x0600404C RID: 16460 RVA: 0x00230C9C File Offset: 0x0022F09C
	private void Start()
	{
		this.speedTime = WeaponProperties.LevelWeaponPushback.Basic.speedTime;
		base.StartCoroutine(this.determine_speed_cr());
		this.forceAmount = WeaponProperties.LevelWeaponPushback.Basic.pushbackSpeed;
		this.forceLeft = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, this.forceAmount);
		this.forceRight = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, -this.forceAmount);
	}

	// Token: 0x0600404D RID: 16461 RVA: 0x00230CF4 File Offset: 0x0022F0F4
	protected override AbstractProjectile fireBasic()
	{
		BasicProjectile basicProjectile = base.fireBasic() as BasicProjectile;
		basicProjectile.Speed = this.bulletSpeed;
		basicProjectile.Damage = WeaponProperties.LevelWeaponPushback.Basic.damage;
		basicProjectile.PlayerId = this.player.id;
		float y = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		basicProjectile.transform.AddPosition(0f, y, 0f);
		bool flag = this.player.transform.localScale.x < 0f;
		if (!this.hasForce)
		{
			this.AddHorizontalForce(flag);
		}
		return basicProjectile;
	}

	// Token: 0x0600404E RID: 16462 RVA: 0x00230DB4 File Offset: 0x0022F1B4
	private void Update()
	{
		this.facingLeft = (this.player.transform.localScale.x < 0f);
		if ((this.hasForce && !this.holdingShoot) || this.forceIsLeft != this.facingLeft)
		{
			this.player.motor.RemoveForce(this.forceLeft);
			this.player.motor.RemoveForce(this.forceRight);
			this.hasForce = false;
		}
	}

	// Token: 0x0600404F RID: 16463 RVA: 0x00230E40 File Offset: 0x0022F240
	private void AddHorizontalForce(bool facingLeft)
	{
		this.hasForce = true;
		this.forceIsLeft = facingLeft;
		if (facingLeft)
		{
			this.player.motor.AddForce(this.forceLeft);
		}
		else
		{
			this.player.motor.AddForce(this.forceRight);
		}
	}

	// Token: 0x06004050 RID: 16464 RVA: 0x00230E94 File Offset: 0x0022F294
	private IEnumerator determine_speed_cr()
	{
		float t = 0f;
		float speedVal = 0f;
		float fireVal = 0f;
		for (;;)
		{
			if (this.holdingShoot)
			{
				if (speedVal < 1f)
				{
					speedVal = t / this.speedTime;
					fireVal = 1f - t / this.speedTime;
					t += CupheadTime.Delta;
				}
				else
				{
					speedVal = 1f;
					t = 1f;
				}
			}
			else if (speedVal > 0f)
			{
				speedVal = t / this.speedTime;
				fireVal = 1f - t / this.speedTime;
				t -= CupheadTime.Delta;
			}
			else
			{
				speedVal = 0f;
				t = 0f;
			}
			this.holdingShoot = this.player.input.actions.GetButton(3);
			this.bulletSpeed = WeaponProperties.LevelWeaponPushback.Basic.speed.GetFloatAt(speedVal);
			this.bulletFireRate = WeaponProperties.LevelWeaponPushback.Basic.fireRate.GetFloatAt(fireVal);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400471D RID: 18205
	private const float ONE = 1f;

	// Token: 0x0400471E RID: 18206
	private const float Y_POS = 20f;

	// Token: 0x0400471F RID: 18207
	private const float ROTATION_OFFSET = 3f;

	// Token: 0x04004720 RID: 18208
	private int currentY;

	// Token: 0x04004721 RID: 18209
	private float[] yPositions = new float[]
	{
		0f,
		20f,
		40f,
		20f
	};

	// Token: 0x04004722 RID: 18210
	private float bulletSpeed;

	// Token: 0x04004723 RID: 18211
	private float bulletFireRate;

	// Token: 0x04004724 RID: 18212
	private float speedTime;

	// Token: 0x04004725 RID: 18213
	private float forceAmount;

	// Token: 0x04004726 RID: 18214
	private bool holdingShoot;

	// Token: 0x04004727 RID: 18215
	private bool hasForce;

	// Token: 0x04004728 RID: 18216
	private bool facingLeft;

	// Token: 0x04004729 RID: 18217
	private bool forceIsLeft;

	// Token: 0x0400472A RID: 18218
	private LevelPlayerMotor.VelocityManager.Force forceLeft;

	// Token: 0x0400472B RID: 18219
	private LevelPlayerMotor.VelocityManager.Force forceRight;
}
