using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007CC RID: 1996
public class SaltbakerLevelJumper : AbstractProjectile
{
	// Token: 0x06002D4A RID: 11594 RVA: 0x001AAB40 File Offset: 0x001A8F40
	public SaltbakerLevelJumper Create(Vector3 position, SaltbakerLevel parent, LevelProperties.Saltbaker.Swooper swooperProperties, LevelProperties.Saltbaker.Jumper jumperProperties, float firstDelay, bool isSwooper)
	{
		SaltbakerLevelJumper saltbakerLevelJumper = this.InstantiatePrefab<SaltbakerLevelJumper>();
		saltbakerLevelJumper.transform.position = position;
		if (isSwooper)
		{
			saltbakerLevelJumper.transform.position += Vector3.up * -94f;
		}
		saltbakerLevelJumper.count = ((!isSwooper) ? jumperProperties.numberFireJumpers : swooperProperties.numberFireSwoopers);
		saltbakerLevelJumper.apexHeight = ((!isSwooper) ? (jumperProperties.apexHeight - 68f) : (swooperProperties.apexHeight + -94f));
		saltbakerLevelJumper.apexTime = ((!isSwooper) ? jumperProperties.apexTime : swooperProperties.apexTime);
		saltbakerLevelJumper.initialFallDelay = ((!isSwooper) ? jumperProperties.initialFallDelay : swooperProperties.initialFallDelay);
		saltbakerLevelJumper.jumpDelay = ((!isSwooper) ? jumperProperties.jumpDelay : swooperProperties.jumpDelay);
		saltbakerLevelJumper.levelEdgeOffset = ((!isSwooper) ? 260f : 75f);
		saltbakerLevelJumper.parent = parent;
		saltbakerLevelJumper.firstDelay = firstDelay;
		saltbakerLevelJumper.isSwooper = isSwooper;
		saltbakerLevelJumper.coll = saltbakerLevelJumper.GetComponent<CircleCollider2D>();
		saltbakerLevelJumper.FXbottom.transform.parent = null;
		saltbakerLevelJumper.aimPosition = saltbakerLevelJumper.transform.position;
		if (saltbakerLevelJumper.isSwooper)
		{
			saltbakerLevelJumper.StartCoroutine(saltbakerLevelJumper.arc_cr());
		}
		else
		{
			saltbakerLevelJumper.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
			saltbakerLevelJumper.StartCoroutine(saltbakerLevelJumper.fall_cr());
		}
		return saltbakerLevelJumper;
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x001AACCF File Offset: 0x001A90CF
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x001AACD1 File Offset: 0x001A90D1
	public Vector3 GetAimPos()
	{
		return this.aimPosition;
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x001AACD9 File Offset: 0x001A90D9
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x001AACF8 File Offset: 0x001A90F8
	private IEnumerator arc_cr()
	{
		AnimationHelper animHelper = base.GetComponent<AnimationHelper>();
		if (this.isSwooper)
		{
			base.animator.Play("SwooperIntro");
			yield return base.animator.WaitForAnimationToEnd(this, "SwooperIntro", false, true);
		}
		else
		{
			this.FXbottom.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
			this.FXbottom.GetComponent<SpriteRenderer>().sortingOrder = -2;
		}
		float root = (float)(Level.Current.Left + Level.Current.Right) / 2f;
		yield return CupheadTime.WaitForSeconds(this, this.firstDelay);
		while (!this.dead)
		{
			if (this.isSwooper)
			{
				base.animator.Play("SwooperAntic");
				while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.888f)
				{
					yield return null;
				}
			}
			float t = 0f;
			float endPosY = (!this.isSwooper) ? ((float)Level.Current.Ground + 68f) : (CupheadLevelCamera.Current.Bounds.yMax + -94f);
			this.aimPosition = new Vector3(Mathf.Clamp(PlayerManager.GetNext().center.x, (float)Level.Current.Left + this.levelEdgeOffset, (float)Level.Current.Right - this.levelEdgeOffset), endPosY);
			float offset = Mathf.Sign(root - this.aimPosition.x) * this.coll.bounds.size.x;
			bool foundPos = false;
			while (!foundPos)
			{
				if (this.aimPosition.x < (float)Level.Current.Left + this.levelEdgeOffset || this.aimPosition.x > (float)Level.Current.Right - this.levelEdgeOffset)
				{
					this.aimPosition = base.transform.position;
					foundPos = true;
				}
				else if (this.parent.IsPositionAvailable(this.aimPosition, this))
				{
					foundPos = true;
				}
				else
				{
					this.aimPosition.x = this.aimPosition.x + offset;
				}
			}
			float x = this.aimPosition.x - base.transform.position.x;
			float y = this.aimPosition.y - base.transform.position.y;
			float apexTime2 = this.apexTime * this.apexTime;
			float g = -2f * this.apexHeight / apexTime2;
			float viY = 2f * this.apexHeight / this.apexTime;
			float viX2 = viY * viY;
			float sqrtRooted = viX2 + 2f * g * y;
			float tEnd = (-viY + Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd2 = (-viY - Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd3 = Mathf.Max(tEnd, tEnd2);
			float velocityX = x / tEnd3;
			if (this.isSwooper)
			{
				viY = -viY;
			}
			Vector3 vel = new Vector3(velocityX, viY);
			base.animator.SetInteger("ArcWidth", Mathf.Clamp((int)(Mathf.Abs(velocityX) / 250f), 0, 2));
			int jumpLoopHash = Animator.StringToHash(base.animator.GetLayerName(0) + "." + ((!this.isSwooper) ? "Jumper" : "Swooper") + "JumpLoop");
			float animTimeOnLand = -1f;
			base.animator.SetInteger("Variant", UnityEngine.Random.Range(0, 2));
			if (this.isSwooper)
			{
				base.transform.localScale = new Vector3(Mathf.Sign(velocityX), 1f);
				yield return base.animator.WaitForAnimationToEnd(this, "SwooperAntic", false, true);
				tEnd3 -= 0.375f;
			}
			else
			{
				base.animator.SetTrigger("StartJumperAntic");
				yield return base.animator.WaitForAnimationToStart(this, "JumperAntic", false);
				base.transform.localScale = new Vector3(Mathf.Sign(-velocityX), 1f);
				yield return base.animator.WaitForAnimationToEnd(this, "JumperAntic", false, true);
			}
			this.FXbottom.transform.position = base.transform.position + Vector3.up * ((!this.isSwooper) ? -20f : 27f);
			this.FXbottom.transform.localScale = new Vector3(base.transform.localScale.x, (float)((!this.isSwooper) ? 1 : -1));
			this.FXbottom.Play(this.FXanimNames[base.animator.GetInteger("ArcWidth")], 0, 0f);
			bool stillMoving = true;
			YieldInstruction wait = new WaitForFixedUpdate();
			while (stillMoving)
			{
				if (animTimeOnLand < 0f && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == jumpLoopHash)
				{
					float num = tEnd3 / 0.41666666f;
					animTimeOnLand = (num + base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1f;
					float num2 = animTimeOnLand - ((!this.isSwooper) ? 0.75f : 0.65f);
					float num3 = num - num2;
					animHelper.Speed = num3 / num;
				}
				if (this.isSwooper)
				{
					vel.y -= g * CupheadTime.FixedDelta;
				}
				else
				{
					vel.y += g * CupheadTime.FixedDelta;
				}
				base.transform.Translate(vel * CupheadTime.FixedDelta);
				tEnd3 -= CupheadTime.FixedDelta;
				yield return wait;
				t += CupheadTime.FixedDelta;
				if (t > this.apexTime)
				{
					if (this.isSwooper)
					{
						if (base.transform.position.y >= endPosY)
						{
							stillMoving = false;
						}
						if (tEnd3 <= 0f)
						{
							base.animator.SetBool("EndJump", true);
							animHelper.Speed = 1f;
						}
					}
					else if (base.transform.position.y <= endPosY)
					{
						stillMoving = false;
						base.animator.SetBool("EndJump", true);
						animHelper.Speed = 1f;
					}
				}
			}
			base.transform.SetPosition(null, new float?(endPosY), null);
			if (!this.isSwooper)
			{
				yield return base.animator.WaitForAnimationToEnd(this, "JumperJumpLoop", false, true);
			}
			base.animator.SetBool("EndJump", false);
			if (!this.isSwooper)
			{
				yield return base.animator.WaitForAnimationToStart(this, "JumperIdle", false);
			}
			if (!this.dead)
			{
				yield return CupheadTime.WaitForSeconds(this, this.jumpDelay);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002D4F RID: 11599 RVA: 0x001AAD14 File Offset: 0x001A9114
	private void AniEvent_FlipX()
	{
		base.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
	}

	// Token: 0x06002D50 RID: 11600 RVA: 0x001AAD4C File Offset: 0x001A914C
	private IEnumerator fall_cr()
	{
		base.animator.Play("JumperFallLoop");
		float endPosY = (float)Level.Current.Ground + 68f;
		float apexTime2 = this.apexTime * this.apexTime;
		float g = -2f * this.apexHeight / apexTime2;
		Vector3 vel = Vector3.zero;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y > 200f)
		{
			vel.y += g * CupheadTime.FixedDelta;
			base.transform.Translate(vel * CupheadTime.FixedDelta);
			yield return wait;
		}
		base.animator.SetTrigger("StartJumperIntro");
		while (base.transform.position.y > endPosY)
		{
			yield return wait;
			vel.y += g * CupheadTime.FixedDelta;
			base.transform.Translate(vel * CupheadTime.FixedDelta);
		}
		base.transform.SetPosition(null, new float?(endPosY), null);
		yield return base.animator.WaitForAnimationToStart(this, "JumperIdle", false);
		base.StartCoroutine(this.arc_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x001AAD67 File Offset: 0x001A9167
	public new void Die()
	{
		this.dead = true;
		base.animator.SetTrigger("Die");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x001AAD8C File Offset: 0x001A918C
	public void AniEvent_DeathComplete()
	{
		UnityEngine.Object.Destroy(this.FXbottom.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x001AADA9 File Offset: 0x001A91A9
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (this.coll != null)
		{
			Gizmos.DrawWireSphere(this.aimPosition, this.coll.radius);
		}
	}

	// Token: 0x040035C6 RID: 13766
	private const float SWOOPER_DIVE_LENGTH = 0.375f;

	// Token: 0x040035C7 RID: 13767
	private const float JUMPER_INTRO_LENGTH = 0.41666666f;

	// Token: 0x040035C8 RID: 13768
	private const float JUMPER_GROUND_OFFSET = 68f;

	// Token: 0x040035C9 RID: 13769
	private const float SWOOPER_CEILING_OFFSET = -94f;

	// Token: 0x040035CA RID: 13770
	private const float SWOOPER_FX_OFFSET = 27f;

	// Token: 0x040035CB RID: 13771
	private const float JUMPER_ENTRANCE_Y_POS = 200f;

	// Token: 0x040035CC RID: 13772
	private const float JUMP_LOOP_LENGTH = 0.41666666f;

	// Token: 0x040035CD RID: 13773
	private const float SWOOPER_LOOP_EXIT_TIME = 0.65f;

	// Token: 0x040035CE RID: 13774
	private const float JUMPER_LOOP_EXIT_TIME = 0.75f;

	// Token: 0x040035CF RID: 13775
	private const float LEVEL_EDGE_OFFSET_SWOOPER = 75f;

	// Token: 0x040035D0 RID: 13776
	private const float LEVEL_EDGE_OFFSET_JUMPER = 260f;

	// Token: 0x040035D1 RID: 13777
	[SerializeField]
	private Animator FXbottom;

	// Token: 0x040035D2 RID: 13778
	private string[] FXanimNames = new string[]
	{
		"Thin",
		"Medium",
		"Wide"
	};

	// Token: 0x040035D3 RID: 13779
	protected Vector3 aimPosition;

	// Token: 0x040035D4 RID: 13780
	private float levelEdgeOffset = 75f;

	// Token: 0x040035D5 RID: 13781
	protected SaltbakerLevel parent;

	// Token: 0x040035D6 RID: 13782
	protected float firstDelay;

	// Token: 0x040035D7 RID: 13783
	protected bool isSwooper;

	// Token: 0x040035D8 RID: 13784
	private CircleCollider2D coll;

	// Token: 0x040035D9 RID: 13785
	private int count;

	// Token: 0x040035DA RID: 13786
	private float apexHeight;

	// Token: 0x040035DB RID: 13787
	private float apexTime;

	// Token: 0x040035DC RID: 13788
	private float initialFallDelay;

	// Token: 0x040035DD RID: 13789
	private float jumpDelay;

	// Token: 0x040035DE RID: 13790
	private new bool dead;
}
