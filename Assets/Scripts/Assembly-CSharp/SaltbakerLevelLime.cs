using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007CE RID: 1998
public class SaltbakerLevelLime : SaltbakerLevelPhaseOneProjectile
{
	// Token: 0x06002D5C RID: 11612 RVA: 0x001AC0D4 File Offset: 0x001AA4D4
	public virtual SaltbakerLevelLime Init(Vector3 position, bool onLeft, bool isHigh, LevelProperties.Saltbaker.Limes properties, int id, int anim)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.properties = properties;
		this.onLeft = onLeft;
		this.isHigh = isHigh;
		this.Move();
		base.animator.Play(anim.ToString());
		this.sfxID = id;
		this.SFX_SALTBAKER_P1_LimeProjectileLoop();
		return this;
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x001AC13C File Offset: 0x001AA53C
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x001AC15A File Offset: 0x001AA55A
	private new void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x001AC169 File Offset: 0x001AA569
	protected override void OnDestroy()
	{
		AudioManager.Stop("sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (this.sfxID + 1));
		base.OnDestroy();
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x001AC190 File Offset: 0x001AA590
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float startPosX = (float)((!this.onLeft) ? Level.Current.Right : Level.Current.Left);
		float curveStartY = (!this.isHigh) ? this.properties.lowStartY : this.properties.highStartY;
		float curveEndY = (!this.isHigh) ? this.properties.lowEndY : this.properties.highEndY;
		float boomerangSpeed = this.properties.straightSpeed;
		float distToTurn = this.properties.distToTurn;
		float loopSizeX = 100f;
		float gravity = this.properties.straightGravity;
		float speed = boomerangSpeed;
		float pivotY = Mathf.Lerp(curveStartY, curveEndY, 0.5f);
		float pivotX = (!this.onLeft) ? (-distToTurn) : distToTurn;
		float loopSizeY = Mathf.Abs(pivotY - curveStartY);
		this.pivot = new Vector3(pivotX, pivotY);
		float offset = (!this.isHigh) ? (-loopSizeY) : loopSizeY;
		base.transform.SetPosition(new float?(startPosX), new float?(this.pivot.y + offset), null);
		if (this.onLeft)
		{
			while (base.transform.position.x < distToTurn)
			{
				base.transform.position += Vector3.right * speed * CupheadTime.FixedDelta;
				base.HandleShadow(0f, 40f);
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > -distToTurn)
			{
				base.transform.position += Vector3.left * speed * CupheadTime.FixedDelta;
				base.HandleShadow(0f, 40f);
				yield return wait;
			}
		}
		float angleToStopAt = 3.1415927f;
		float angle = 0f;
		float angleStartSpeed = this.properties.angleSpeedToLerp.min;
		float angleEndSpeed = this.properties.angleSpeedToLerp.max;
		float timeTolerp = this.properties.angleLerpTime;
		float t = 0f;
		angle *= 0.017453292f;
		while (angle < angleToStopAt)
		{
			t += CupheadTime.FixedDelta;
			float s = Mathf.Lerp(angleStartSpeed, angleEndSpeed, t / timeTolerp);
			angle += s * CupheadTime.FixedDelta;
			Vector3 handleRotationX;
			if (this.onLeft)
			{
				handleRotationX = new Vector3(Mathf.Sin(angle) * loopSizeX, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(-Mathf.Sin(angle) * loopSizeX, 0f, 0f);
			}
			Vector3 handleRotationY;
			if (this.isHigh)
			{
				handleRotationY = new Vector3(0f, Mathf.Cos(angle) * loopSizeY, 0f);
			}
			else
			{
				handleRotationY = new Vector3(0f, -Mathf.Cos(angle) * loopSizeY, 0f);
			}
			base.transform.position = this.pivot;
			base.transform.position += handleRotationX + handleRotationY;
			base.HandleShadow(0f, 40f);
			yield return new WaitForFixedUpdate();
		}
		speed = boomerangSpeed;
		if (this.onLeft)
		{
			while (base.transform.position.x > (float)Level.Current.Left - 300f)
			{
				speed += gravity * CupheadTime.FixedDelta;
				base.transform.position += Vector3.left * speed * CupheadTime.FixedDelta;
				base.HandleShadow(0f, 40f);
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x < (float)Level.Current.Right + 300f)
			{
				speed += gravity * CupheadTime.FixedDelta;
				base.transform.position += Vector3.right * speed * CupheadTime.FixedDelta;
				base.HandleShadow(0f, 40f);
				yield return wait;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		AudioManager.Stop("sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (this.sfxID + 1));
		yield break;
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x001AC1AB File Offset: 0x001AA5AB
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(this.pivot, 10f);
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x001AC1C4 File Offset: 0x001AA5C4
	private void SFX_SALTBAKER_P1_LimeProjectileLoop()
	{
		string key = "sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (this.sfxID + 1);
		AudioManager.PlayLoop(key);
		this.emitAudioFromObject.Add(key);
	}

	// Token: 0x040035E6 RID: 13798
	private LevelProperties.Saltbaker.Limes properties;

	// Token: 0x040035E7 RID: 13799
	private bool isDead;

	// Token: 0x040035E8 RID: 13800
	private bool onLeft;

	// Token: 0x040035E9 RID: 13801
	private bool isHigh;

	// Token: 0x040035EA RID: 13802
	private int sfxID;

	// Token: 0x040035EB RID: 13803
	private Vector3 pivot;
}
