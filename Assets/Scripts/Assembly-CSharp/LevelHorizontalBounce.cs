using System;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public class LevelHorizontalBounce : AbstractCollidableObject
{
	// Token: 0x0600135E RID: 4958 RVA: 0x000AB3DC File Offset: 0x000A97DC
	private void Start()
	{
		this.scrollForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, (!this.onLeft) ? (-this.fanForce) : this.fanForce);
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x000AB408 File Offset: 0x000A9808
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		LevelPlayerMotor component = hit.GetComponent<LevelPlayerMotor>();
		if (phase != CollisionPhase.Exit)
		{
			this.FanOn(component);
		}
		else
		{
			this.FanOff(hit.GetComponent<LevelPlayerMotor>());
		}
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x000AB443 File Offset: 0x000A9843
	private void FanOn(LevelPlayerMotor player)
	{
		player.AddForce(this.scrollForce);
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x000AB451 File Offset: 0x000A9851
	private void FanOff(LevelPlayerMotor player)
	{
		player.RemoveForce(this.scrollForce);
	}

	// Token: 0x04001C79 RID: 7289
	[SerializeField]
	private bool onLeft;

	// Token: 0x04001C7A RID: 7290
	[SerializeField]
	private float fanForce = 1f;

	// Token: 0x04001C7B RID: 7291
	private LevelPlayerMotor.VelocityManager.Force scrollForce;
}
