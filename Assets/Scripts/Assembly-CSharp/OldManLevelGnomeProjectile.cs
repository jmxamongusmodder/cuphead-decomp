using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000703 RID: 1795
public class OldManLevelGnomeProjectile : AbstractProjectile
{
	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06002692 RID: 9874 RVA: 0x0016928B File Offset: 0x0016768B
	// (set) Token: 0x06002693 RID: 9875 RVA: 0x00169293 File Offset: 0x00167693
	public bool IsFlying { get; private set; }

	// Token: 0x06002694 RID: 9876 RVA: 0x0016929C File Offset: 0x0016769C
	public virtual OldManLevelGnomeProjectile Init(Vector3 position, Vector3 speed, float gravity, bool spawnParryable, bool parryable, OldManLevelStomachPlatform target)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		base.transform.localScale = new Vector3((float)((!MathUtils.RandomBool()) ? 1 : -1), 1f);
		this.speed = speed;
		this.gravity = gravity;
		this.spawnParryable = spawnParryable;
		this.IsFlying = true;
		this.SetParryable(parryable);
		this.target = target;
		this.animHelper = base.GetComponent<AnimationHelper>();
		this.animHelper.Speed = 1f;
		base.animator.Play((!spawnParryable) ? ((!parryable) ? "Chicken" : "ChickenPink") : "Bone");
		base.animator.Update(0f);
		base.GetComponent<Collider2D>().enabled = true;
		this.bouncingOffscreen = false;
		this.triedHit = false;
		this.underwaterSprite.color = Color.white;
		this.playedAnticipationSound = false;
		return this;
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x001693A5 File Offset: 0x001677A5
	public override void OnParry(AbstractPlayerController player)
	{
		this.target.CancelAnticipation();
		base.OnParry(player);
	}

	// Token: 0x06002696 RID: 9878 RVA: 0x001693B9 File Offset: 0x001677B9
	public override void OnLevelEnd()
	{
	}

	// Token: 0x06002697 RID: 9879 RVA: 0x001693BC File Offset: 0x001677BC
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
		if (this.bouncingOffscreen)
		{
			this.speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
		}
		base.transform.Translate(this.speed * CupheadTime.FixedDelta);
		if (this.bouncingOffscreen)
		{
			if (!this.splashed && base.transform.position.y < this.target.main.splashHandler.transform.position.y)
			{
				this.target.main.splashHandler.SplashIn(base.transform.position.x);
				this.speed *= 0.5f;
				this.gravity *= 0.5f;
				this.animHelper.Speed = 0.5f;
				this.splashed = true;
			}
			if (base.transform.position.y < -560f)
			{
				this.Recycle<OldManLevelGnomeProjectile>();
			}
			if (this.splashed)
			{
				this.underwaterSprite.color = new Color(1f, 1f, 1f, (1f - Mathf.InverseLerp(this.target.main.splashHandler.transform.position.y, this.target.main.splashHandler.transform.position.y - 140f, base.transform.position.y)) * 0.5f);
			}
		}
		if (this.spawnParryable && base.transform.position.y < this.target.transform.position.y + 200f && !this.playedAnticipationSound)
		{
			this.SFX_PreBoneHit();
			this.playedAnticipationSound = true;
		}
		if (base.transform.position.y < this.target.transform.position.y + ((!this.spawnParryable) ? 200f : 50f) && !this.triedHit)
		{
			this.HitTarget();
		}
	}

	// Token: 0x06002698 RID: 9880 RVA: 0x00169671 File Offset: 0x00167A71
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002699 RID: 9881 RVA: 0x00169690 File Offset: 0x00167A90
	private void HitTarget()
	{
		this.triedHit = true;
		if (this.target.isActivated)
		{
			foreach (AbstractPlayerController abstractPlayerController in this.target.GetComponentsInChildren<AbstractPlayerController>())
			{
				if (!(abstractPlayerController == null))
				{
					abstractPlayerController.transform.parent = null;
				}
			}
			this.target.DeactivatePlatform(this.spawnParryable);
			this.IsFlying = false;
			if (this.spawnParryable)
			{
				this.bouncingOffscreen = true;
				this.speed.y = -this.speed.y * this.bounceModifier;
				this.speed.x = this.speed.x * 2f;
				base.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
				base.GetComponent<Collider2D>().enabled = false;
			}
			else
			{
				base.StartCoroutine(this.wait_for_eat());
			}
		}
		else
		{
			this.bouncingOffscreen = true;
		}
	}

	// Token: 0x0600269A RID: 9882 RVA: 0x001697A8 File Offset: 0x00167BA8
	private IEnumerator wait_for_eat()
	{
		Animator anim = this.target.GetComponent<Animator>();
		yield return anim.WaitForAnimationToStart(this, "Eat", false);
		while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.18965517f)
		{
			yield return null;
		}
		base.GetComponent<Collider2D>().enabled = false;
		this.Recycle<OldManLevelGnomeProjectile>();
		yield break;
	}

	// Token: 0x0600269B RID: 9883 RVA: 0x001697C3 File Offset: 0x00167BC3
	private void SFX_PreBoneHit()
	{
		AudioManager.Play("sfx_dlc_omm_p3_dinobells_prebonehit");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_prebonehit");
	}

	// Token: 0x04002F47 RID: 12103
	private const float OFFSET_TO_HIT_BONE = 50f;

	// Token: 0x04002F48 RID: 12104
	private const float OFFSET_TO_PLAY_ANTICIPATION_SOUND = 200f;

	// Token: 0x04002F49 RID: 12105
	private const float OFFSET_TO_HIT_LEG = 200f;

	// Token: 0x04002F4B RID: 12107
	private Vector3 speed;

	// Token: 0x04002F4C RID: 12108
	private float gravity;

	// Token: 0x04002F4D RID: 12109
	private bool spawnParryable;

	// Token: 0x04002F4E RID: 12110
	private OldManLevelStomachPlatform target;

	// Token: 0x04002F4F RID: 12111
	private bool bouncingOffscreen;

	// Token: 0x04002F50 RID: 12112
	[SerializeField]
	private float bounceModifier = 0.5f;

	// Token: 0x04002F51 RID: 12113
	[SerializeField]
	private SpriteRenderer underwaterSprite;

	// Token: 0x04002F52 RID: 12114
	private bool triedHit;

	// Token: 0x04002F53 RID: 12115
	private bool splashed;

	// Token: 0x04002F54 RID: 12116
	private AnimationHelper animHelper;

	// Token: 0x04002F55 RID: 12117
	private bool playedAnticipationSound;
}
