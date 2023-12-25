using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007CD RID: 1997
public class SaltBakerLevelLeaf : AbstractProjectile
{
	// Token: 0x06002D55 RID: 11605 RVA: 0x001ABC1C File Offset: 0x001AA01C
	public virtual SaltBakerLevelLeaf Init(Vector3 pos, float xTime, float xDistance, float yGravity, MinMax ySpeed, SaltbakerLevelSaltbaker parent, int animID)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.xDistance = xDistance;
		this.xTime = xTime;
		this.ySpeedMinMax = ySpeed;
		this.yGravity = yGravity;
		this.Move();
		this.parent = parent;
		this.parent.OnDeathEvent += this.Death;
		this.animID = animID;
		return this;
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x001ABC8D File Offset: 0x001AA08D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x001ABCAB File Offset: 0x001AA0AB
	private void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x001ABCBC File Offset: 0x001AA0BC
	private IEnumerator move_cr()
	{
		float ground = (float)Level.Current.Ground;
		YieldInstruction wait = new WaitForFixedUpdate();
		float val = 0f;
		float yVal = 0f;
		float xVal = 0f;
		float t = 0f;
		float startX = base.transform.position.x;
		float endX = base.transform.position.x + this.xDistance;
		AnimationHelper animationHelper = base.GetComponent<AnimationHelper>();
		animationHelper.Speed = 0f;
		bool dirRight = true;
		while (base.transform.position.y > ground)
		{
			val = t / this.xTime;
			xVal = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, val);
			yVal = ((val >= 0.5f) ? (1f - val) : val);
			float ySpeed = this.ySpeedMinMax.GetFloatAt(yVal);
			float yPos = base.transform.position.y - (ySpeed + this.yGravity) * CupheadTime.FixedDelta;
			string animName = Convert.ToChar(65 + this.animID).ToString();
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(startX, endX, xVal)), new float?(yPos), null);
			base.animator.Play(animName, 0, val / 2f + ((!dirRight) ? 0.5f : 0f));
			if (t >= this.xTime)
			{
				t = 0f;
				endX = startX;
				startX = base.transform.position.x;
				dirRight = !dirRight;
			}
			yield return wait;
		}
		this.boxColl.enabled = false;
		animationHelper.Speed = 1f;
		base.animator.SetTrigger("Die");
		yield return base.animator.WaitForAnimationToStart(this, "None", false);
		this.Recycle<SaltBakerLevelLeaf>();
		yield return null;
		yield break;
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x001ABCD7 File Offset: 0x001AA0D7
	private void Death()
	{
		this.Recycle<SaltBakerLevelLeaf>();
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x001ABCDF File Offset: 0x001AA0DF
	protected override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Death;
		base.OnDestroy();
	}

	// Token: 0x040035DF RID: 13791
	private float xTime;

	// Token: 0x040035E0 RID: 13792
	private float xDistance;

	// Token: 0x040035E1 RID: 13793
	private float yGravity;

	// Token: 0x040035E2 RID: 13794
	private MinMax ySpeedMinMax;

	// Token: 0x040035E3 RID: 13795
	private SaltbakerLevelSaltbaker parent;

	// Token: 0x040035E4 RID: 13796
	private int animID;

	// Token: 0x040035E5 RID: 13797
	[SerializeField]
	private BoxCollider2D boxColl;
}
