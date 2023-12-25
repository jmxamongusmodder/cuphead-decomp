using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007C3 RID: 1987
public class SaltbakerLevelBouncer : LevelProperties.Saltbaker.Entity
{
	// Token: 0x06002CEA RID: 11498 RVA: 0x001A71CC File Offset: 0x001A55CC
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.idleHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle");
		foreach (CollisionChild collisionChild in this.collisionKids)
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
			collisionChild.OnPlayerProjectileCollision += this.OnCollisionPlayerProjectile;
		}
	}

	// Token: 0x06002CEB RID: 11499 RVA: 0x001A726D File Offset: 0x001A566D
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x001A7285 File Offset: 0x001A5685
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002CED RID: 11501 RVA: 0x001A7298 File Offset: 0x001A5698
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002CEE RID: 11502 RVA: 0x001A72B6 File Offset: 0x001A56B6
	public void StartBouncer(Vector3 startPos)
	{
		this.bouncerStartPos = startPos;
		base.transform.position = startPos;
		this.saltHands.gameObject.SetActive(true);
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x06002CEF RID: 11503 RVA: 0x001A72EC File Offset: 0x001A56EC
	private float TimeToGround(float curYVel, float groundY, float gravity)
	{
		float num = base.transform.position.y - groundY;
		return (curYVel + Mathf.Sqrt(curYVel * curYVel + 2f * gravity * num)) / gravity;
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x001A7328 File Offset: 0x001A5728
	private IEnumerator jump_cr()
	{
		LevelProperties.Saltbaker.Bouncer p = base.properties.CurrentState.bouncer;
		AnimationHelper animHelper = base.GetComponent<AnimationHelper>();
		while (!this.isDead)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "Explode", false, false);
			base.transform.position = this.bouncerStartPos;
			base.animator.Play("Idle");
			this.saltHands.Play();
			foreach (Collider2D collider2D in this.colliders)
			{
				collider2D.enabled = false;
			}
			yield return CupheadTime.WaitForSeconds(this, 3.5f);
			this.SFX_SALTB_Bouncer_Twirl();
			YieldInstruction wait = new WaitForFixedUpdate();
			foreach (Collider2D collider2D2 in this.colliders)
			{
				collider2D2.enabled = true;
			}
			bool goingRight = Rand.Bool();
			float velocityY = 0f;
			float velocityX = 0f;
			float gravity = p.initDropYGravity;
			this.onGroundY = (float)Level.Current.Ground + base.GetComponent<Collider2D>().bounds.size.y / 2f + 13f;
			float maxX = (float)Level.Current.Right - base.GetComponent<Collider2D>().bounds.size.x / 2f;
			this.minShadowHeight = this.onGroundY + 75f - p.jumpGravity * 0.027777778f + p.jumpYSpeed * 0.16666667f;
			this.maxShadowHeight = this.onGroundY + p.jumpYSpeed * p.jumpYSpeed / (p.jumpGravity * 2f);
			AbstractPlayerController player = PlayerManager.GetNext();
			base.transform.SetPosition(new float?(player.transform.position.x), null, null);
			float timeToGround = this.TimeToGround(velocityY, this.onGroundY + 75f, gravity);
			float animTimeOnLand = -1f;
			bool useLandB = false;
			for (int i = 0; i < p.numBounces + 1; i++)
			{
				while (base.transform.position.y > this.onGroundY + 75f)
				{
					timeToGround -= CupheadTime.FixedDelta;
					if (animTimeOnLand < 0f && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == this.idleHash)
					{
						float num = (timeToGround - 0.1f) / 0.6666667f;
						animTimeOnLand = (num + base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1f;
						float num2 = Mathf.Min(Mathf.Abs(animTimeOnLand - 0.84375f), Mathf.Abs(0.84375f - animTimeOnLand));
						float num3 = Mathf.Min(Mathf.Abs(animTimeOnLand - 0.40625f), Mathf.Abs(0.40625f - animTimeOnLand));
						useLandB = (num2 > num3 && i < p.numBounces);
						float num4 = animTimeOnLand - ((!useLandB) ? 0.84375f : 0.40625f);
						float num5 = num - num4;
						animHelper.Speed = num5 / num;
					}
					if (timeToGround < 0.1f)
					{
						animHelper.Speed = 1f;
						if (i == p.numBounces || this.isDead)
						{
							base.animator.Play("Explode");
						}
						else
						{
							base.animator.Play((!useLandB) ? "Land_A" : "Land_B");
						}
					}
					velocityY -= gravity * CupheadTime.FixedDelta;
					if (i > 0)
					{
						velocityX = ((!goingRight) ? (-p.jumpXSpeed) : p.jumpXSpeed);
					}
					base.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0f);
					if ((!goingRight && base.transform.position.x < -maxX) || (goingRight && base.transform.position.x > maxX))
					{
						base.transform.SetPosition(new float?((!goingRight) ? (-maxX) : maxX), null, null);
						goingRight = !goingRight;
						if (velocityY < 0f)
						{
							velocityX = 0f;
						}
					}
					yield return wait;
				}
				CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
				base.transform.SetPosition(new float?(base.transform.position.x + velocityX / Mathf.Abs(velocityY) * 75f), new float?(this.onGroundY), null);
				this.landFXAnimator.transform.position = base.transform.position;
				this.landFXAnimator.Play("LandFX");
				while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.55f && !this.isDead)
				{
					yield return null;
				}
				if (i < p.numBounces && !this.isDead)
				{
					velocityY = p.jumpYSpeed;
					velocityX = ((!goingRight) ? (-p.jumpXSpeed) : p.jumpXSpeed);
					gravity = p.jumpGravity;
					base.transform.position += Vector3.up * 76f;
					base.transform.position += Vector3.right * (velocityX / Mathf.Abs(velocityY) * 75f);
					timeToGround = this.TimeToGround(velocityY, this.onGroundY + 75f, gravity);
					animTimeOnLand = -1f;
				}
				if (this.isDead)
				{
					break;
				}
				yield return wait;
			}
			foreach (Collider2D collider2D3 in this.colliders)
			{
				collider2D3.enabled = false;
			}
			if (this.isDead && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Explode"))
			{
				base.animator.Play("Explode", 0, 0f);
				base.animator.Update(0f);
			}
		}
		yield break;
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x001A7343 File Offset: 0x001A5743
	public void EndBouncer()
	{
		base.StartCoroutine(this.end_bouncer_cr());
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x001A7354 File Offset: 0x001A5754
	private IEnumerator end_bouncer_cr()
	{
		this.isDead = true;
		yield return base.animator.WaitForAnimationToStart(this, "Off", false);
		foreach (Collider2D collider2D in this.colliders)
		{
			collider2D.enabled = false;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		foreach (CollisionChild collisionChild in this.collisionKids)
		{
			collisionChild.OnPlayerCollision -= this.OnCollisionPlayer;
			collisionChild.OnPlayerProjectileCollision -= this.OnCollisionPlayerProjectile;
		}
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002CF3 RID: 11507 RVA: 0x001A7370 File Offset: 0x001A5770
	private void LateUpdate()
	{
		if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == this.idleHash)
		{
			this.shadow.sprite = this.shadowSprites[(int)(Mathf.InverseLerp(this.maxShadowHeight, this.minShadowHeight, base.transform.position.y) * (float)(this.shadowSprites.Length - 1))];
		}
		this.shadow.transform.position = new Vector3(base.transform.position.x, this.onGroundY);
	}

	// Token: 0x06002CF4 RID: 11508 RVA: 0x001A7410 File Offset: 0x001A5810
	public override void OnPause()
	{
		base.OnPause();
		this.pauseShadow.sprite = this.shadow.sprite;
		this.pauseShadow.transform.position = new Vector3(this.shadow.transform.position.x, this.onGroundY);
		this.shadow.enabled = false;
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x001A7478 File Offset: 0x001A5878
	public override void OnUnpause()
	{
		base.OnUnpause();
		this.pauseShadow.sprite = null;
		this.shadow.enabled = true;
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x001A7498 File Offset: 0x001A5898
	private void AnimationEvent_SFX_SALTB_Bouncer_Bounce()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3_bouncer_bounce");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_bounce");
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x001A74B4 File Offset: 0x001A58B4
	private void AnimationEvent_SFX_SALTB_Bouncer_Death()
	{
		AudioManager.Stop("sfx_dlc_saltbaker_p3_bouncer_twirl");
		AudioManager.Play("sfx_dlc_saltbaker_p3_bouncer_death");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_death");
	}

	// Token: 0x06002CF8 RID: 11512 RVA: 0x001A74DA File Offset: 0x001A58DA
	private void SFX_SALTB_Bouncer_Twirl()
	{
		AudioManager.PlayLoop("sfx_dlc_saltbaker_p3_bouncer_twirl");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_bouncer_twirl");
	}

	// Token: 0x04003568 RID: 13672
	private const float IDLE_ANIM_LENGTH = 0.6666667f;

	// Token: 0x04003569 RID: 13673
	private const float ANIM_TIME_PRE_LAND = 0.1f;

	// Token: 0x0400356A RID: 13674
	private const float NORMALIZED_ANIM_TIME_TO_RELAUNCH = 0.55f;

	// Token: 0x0400356B RID: 13675
	private const float TARGET_TIME_LAND_A = 0.84375f;

	// Token: 0x0400356C RID: 13676
	private const float TARGET_TIME_LAND_B = 0.40625f;

	// Token: 0x0400356D RID: 13677
	private const float GROUND_TRIGGER_OFFSET = 75f;

	// Token: 0x0400356E RID: 13678
	private const float GROUND_POS_OFFSET = 13f;

	// Token: 0x0400356F RID: 13679
	[SerializeField]
	private SaltbakerLevelBGSaltHands saltHands;

	// Token: 0x04003570 RID: 13680
	[SerializeField]
	private SpriteRenderer shadow;

	// Token: 0x04003571 RID: 13681
	[SerializeField]
	private SpriteRenderer pauseShadow;

	// Token: 0x04003572 RID: 13682
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x04003573 RID: 13683
	[SerializeField]
	private CollisionChild[] collisionKids;

	// Token: 0x04003574 RID: 13684
	[SerializeField]
	private Animator landFXAnimator;

	// Token: 0x04003575 RID: 13685
	private DamageDealer damageDealer;

	// Token: 0x04003576 RID: 13686
	private DamageReceiver damageReceiver;

	// Token: 0x04003577 RID: 13687
	private Vector3 bouncerStartPos;

	// Token: 0x04003578 RID: 13688
	private float onGroundY;

	// Token: 0x04003579 RID: 13689
	[SerializeField]
	private Collider2D[] colliders;

	// Token: 0x0400357A RID: 13690
	private bool isDead;

	// Token: 0x0400357B RID: 13691
	private int shadowSprite;

	// Token: 0x0400357C RID: 13692
	private int idleHash;

	// Token: 0x0400357D RID: 13693
	private float minShadowHeight;

	// Token: 0x0400357E RID: 13694
	private float maxShadowHeight;
}
