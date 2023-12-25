using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007EA RID: 2026
public class SnowCultLevelEyeProjectile : AbstractProjectile
{
	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06002E5B RID: 11867 RVA: 0x001B511B File Offset: 0x001B351B
	// (set) Token: 0x06002E5C RID: 11868 RVA: 0x001B5123 File Offset: 0x001B3523
	public bool IsGone { get; private set; }

	// Token: 0x06002E5D RID: 11869 RVA: 0x001B512C File Offset: 0x001B352C
	public virtual SnowCultLevelEyeProjectile Init(Vector3 startPos, Vector3 endPos, bool onRight, bool upsideDown, LevelProperties.SnowCult.EyeAttack properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.startPos = startPos;
		base.transform.position = startPos;
		this.properties = properties;
		this.endPos = endPos;
		this.onRight = onRight;
		base.transform.localScale = new Vector3((float)((!onRight) ? -1 : 1), 1f);
		this.upsideDown = upsideDown;
		this.readyToOpenMouth = false;
		this.angle = 0f;
		this.IsGone = false;
		base.StartCoroutine(this.move_cr());
		this.beamCR = base.StartCoroutine(this.beam_cr());
		return this;
	}

	// Token: 0x06002E5E RID: 11870 RVA: 0x001B51D2 File Offset: 0x001B35D2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002E5F RID: 11871 RVA: 0x001B51F0 File Offset: 0x001B35F0
	private IEnumerator beam_cr()
	{
		this.beamAnimator.Play("AuraStart");
		float delay = this.properties.initialBeamDelay.RandomFloat();
		while (!this.IsGone)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			delay = this.properties.beamDelay;
			this.beamAnimator.SetBool("Attack", true);
			this.SFX_SNOWCULT_JackFrostEyeballZap();
			yield return CupheadTime.WaitForSeconds(this, this.properties.beamDuration);
			this.beamAnimator.SetBool("Attack", false);
		}
		yield break;
	}

	// Token: 0x06002E60 RID: 11872 RVA: 0x001B520C File Offset: 0x001B360C
	private IEnumerator move_in_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		if (!this.onRight)
		{
			while (base.transform.position.x < this.properties.distanceToTurn)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.right * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > -this.properties.distanceToTurn)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.left * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002E61 RID: 11873 RVA: 0x001B5228 File Offset: 0x001B3628
	private IEnumerator move_out_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		if (!this.onRight)
		{
			while (base.transform.position.x < this.endPos.x - this.openMouthDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.right * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > this.endPos.x + this.openMouthDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.left * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		this.readyToOpenMouth = true;
		if (!this.onRight)
		{
			while (base.transform.position.x < this.endPos.x - this.beamEndDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.right * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > this.endPos.x + this.beamEndDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.left * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		base.StopCoroutine(this.beamCR);
		this.beamAnimator.SetBool("Attack", false);
		this.beamAnimator.SetTrigger("End");
		if (!this.onRight)
		{
			while (base.transform.position.x < this.endPos.x - this.animatorTakeoverDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.right * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > this.endPos.x + this.animatorTakeoverDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.left * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		this.readyToCloseMouth = true;
		this.controlledByParent = true;
		yield return null;
		this.shadow.SetActive(true);
		yield break;
	}

	// Token: 0x06002E62 RID: 11874 RVA: 0x001B5244 File Offset: 0x001B3644
	private IEnumerator move_cr()
	{
		this.SFX_SNOWCULT_JackFrostEyeballLoop();
		float loopSpeed = this.properties.eyeCurveSpeed;
		float pivotY = (this.startPos.y + this.endPos.y) / 2f;
		float loopSizeY = Mathf.Abs(this.startPos.y - this.endPos.y) / 2f;
		float loopSizeX = loopSizeY;
		float pivotX = (!this.onRight) ? this.properties.distanceToTurn : (-this.properties.distanceToTurn);
		Vector3 pivot = new Vector3(pivotX, pivotY);
		float angleToStopAt = 3.1415927f;
		if (!this.upsideDown)
		{
			base.transform.SetPosition(null, new float?(pivot.y - loopSizeY), null);
		}
		else
		{
			base.transform.SetPosition(null, new float?(pivot.y + loopSizeY), null);
		}
		this.angle *= 0.017453292f;
		yield return base.StartCoroutine(this.move_in_cr());
		while (this.angle < angleToStopAt)
		{
			this.angle += loopSpeed * CupheadTime.FixedDelta;
			Vector3 handleRotationX;
			if (!this.onRight)
			{
				handleRotationX = new Vector3(Mathf.Sin(this.angle) * loopSizeX, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(-Mathf.Sin(this.angle) * loopSizeX, 0f, 0f);
			}
			Vector3 handleRotationY;
			if (!this.upsideDown)
			{
				handleRotationY = new Vector3(0f, -Mathf.Cos(this.angle) * loopSizeY, 0f);
			}
			else
			{
				handleRotationY = new Vector3(0f, Mathf.Cos(this.angle) * loopSizeY, 0f);
			}
			this.lastPos = base.transform.position;
			base.transform.position = pivot;
			base.transform.position += handleRotationX + handleRotationY;
			yield return new WaitForFixedUpdate();
		}
		this.onRight = !this.onRight;
		base.StartCoroutine(this.move_out_cr());
		yield break;
	}

	// Token: 0x06002E63 RID: 11875 RVA: 0x001B525F File Offset: 0x001B365F
	public void ReturnToSnowflake()
	{
		this.SFX_SNOWCULT_JackFrostEyeballLoopStop();
		this.Recycle<SnowCultLevelEyeProjectile>();
		this.IsGone = true;
	}

	// Token: 0x06002E64 RID: 11876 RVA: 0x001B5274 File Offset: 0x001B3674
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.dead)
		{
			base.transform.position += this.vel * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06002E65 RID: 11877 RVA: 0x001B52B0 File Offset: 0x001B36B0
	private void LateUpdate()
	{
		if (this.main.dead != this.dead)
		{
			this.dead = true;
			this.vel /= CupheadTime.FixedDelta;
			this.SFX_SNOWCULT_JackFrostEyeballLoopStop();
			this.StopAllCoroutines();
		}
		else if (!this.dead)
		{
			this.vel = base.transform.position - this.lastPos;
		}
		if (this.controlledByParent)
		{
			base.transform.position = this.main.eyeProjectileGuide.position;
		}
	}

	// Token: 0x06002E66 RID: 11878 RVA: 0x001B534E File Offset: 0x001B374E
	private void SFX_SNOWCULT_JackFrostEyeballLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop");
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x001B536A File Offset: 0x001B376A
	private void SFX_SNOWCULT_JackFrostEyeballLoopStop()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop");
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x001B5376 File Offset: 0x001B3776
	private void SFX_SNOWCULT_JackFrostEyeballZap()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_zap");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_zap");
	}

	// Token: 0x040036EF RID: 14063
	private LevelProperties.SnowCult.EyeAttack properties;

	// Token: 0x040036F0 RID: 14064
	private Vector3 endPos;

	// Token: 0x040036F1 RID: 14065
	private Vector3 startPos;

	// Token: 0x040036F2 RID: 14066
	private float angle;

	// Token: 0x040036F3 RID: 14067
	private bool onRight;

	// Token: 0x040036F4 RID: 14068
	private bool upsideDown;

	// Token: 0x040036F5 RID: 14069
	public bool readyToOpenMouth;

	// Token: 0x040036F6 RID: 14070
	public bool readyToCloseMouth;

	// Token: 0x040036F8 RID: 14072
	[SerializeField]
	private Animator beamAnimator;

	// Token: 0x040036F9 RID: 14073
	[SerializeField]
	private float openMouthDistance = 400f;

	// Token: 0x040036FA RID: 14074
	[SerializeField]
	private float beamEndDistance = 200f;

	// Token: 0x040036FB RID: 14075
	[SerializeField]
	private float animatorTakeoverDistance = 31f;

	// Token: 0x040036FC RID: 14076
	public SnowCultLevelJackFrost main;

	// Token: 0x040036FD RID: 14077
	private Coroutine beamCR;

	// Token: 0x040036FE RID: 14078
	private bool controlledByParent;

	// Token: 0x040036FF RID: 14079
	[SerializeField]
	private GameObject shadow;

	// Token: 0x04003700 RID: 14080
	private Vector3 vel;

	// Token: 0x04003701 RID: 14081
	private Vector3 lastPos;

	// Token: 0x04003702 RID: 14082
	private new bool dead;
}
