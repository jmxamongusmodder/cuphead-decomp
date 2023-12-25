using System;
using UnityEngine;

// Token: 0x020008E3 RID: 2275
public class MountainPlatformingLevelFanBlow : AbstractCollidableObject
{
	// Token: 0x06003545 RID: 13637 RVA: 0x001F0B23 File Offset: 0x001EEF23
	private void Start()
	{
		this.scrollForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, this.parent.GetSpeed());
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x001F0B3C File Offset: 0x001EEF3C
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			if (hit.GetComponent<LevelPlayerMotor>())
			{
				if (this.parent.fanOn && !this.parent.Dead)
				{
					this.FanOn(hit.GetComponent<LevelPlayerMotor>());
				}
				else
				{
					this.FanOff(hit.GetComponent<LevelPlayerMotor>());
				}
			}
			else
			{
				this.FanOff(hit.GetComponent<LevelPlayerMotor>());
			}
		}
		else
		{
			this.FanOff(hit.GetComponent<LevelPlayerMotor>());
		}
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x001F0BC7 File Offset: 0x001EEFC7
	private void FanOn(LevelPlayerMotor player)
	{
		player.AddForce(this.scrollForce);
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x001F0BD5 File Offset: 0x001EEFD5
	private void FanOff(LevelPlayerMotor player)
	{
		player.RemoveForce(this.scrollForce);
	}

	// Token: 0x04003D6E RID: 15726
	[SerializeField]
	private MountainPlatformingLevelFan parent;

	// Token: 0x04003D6F RID: 15727
	private LevelPlayerMotor.VelocityManager.Force scrollForce;
}
