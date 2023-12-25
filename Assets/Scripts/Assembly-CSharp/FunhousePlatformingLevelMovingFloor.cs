using System;
using UnityEngine;

// Token: 0x020008BB RID: 2235
public class FunhousePlatformingLevelMovingFloor : AbstractCollidableObject
{
	// Token: 0x06003425 RID: 13349 RVA: 0x001E44A0 File Offset: 0x001E28A0
	private void Start()
	{
		if (base.transform.localScale.x == 1f)
		{
			this.speed = -this.velocity;
		}
		else
		{
			this.speed = this.velocity;
		}
		this.scrollForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, this.speed);
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x001E44FC File Offset: 0x001E28FC
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Stay)
		{
			if (hit.GetComponent<LevelPlayerMotor>())
			{
				this.ScrollOn(hit.GetComponent<LevelPlayerMotor>());
			}
			else
			{
				this.ScrollOff(hit.GetComponent<LevelPlayerMotor>());
			}
		}
		else
		{
			this.ScrollOff(hit.GetComponent<LevelPlayerMotor>());
		}
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x001E4556 File Offset: 0x001E2956
	private void ScrollOn(LevelPlayerMotor player)
	{
		player.AddForce(this.scrollForce);
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x001E4564 File Offset: 0x001E2964
	private void ScrollOff(LevelPlayerMotor player)
	{
		player.RemoveForce(this.scrollForce);
	}

	// Token: 0x04003C64 RID: 15460
	[SerializeField]
	private float velocity;

	// Token: 0x04003C65 RID: 15461
	private float speed;

	// Token: 0x04003C66 RID: 15462
	private LevelPlayerMotor.VelocityManager.Force scrollForce;
}
