using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000701 RID: 1793
public class OldManLevelGnomeClimber : AbstractProjectile
{
	// Token: 0x0600266D RID: 9837 RVA: 0x001674F8 File Offset: 0x001658F8
	public virtual OldManLevelGnomeClimber Init(float startXPosition, float facing, Transform smashPos, LevelProperties.OldMan.ClimberGnomes properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = new Vector3(startXPosition, -165f);
		this.smashPos = smashPos;
		this.properties = properties;
		base.transform.SetScale(new float?(facing), null, null);
		this.smashFXA = Rand.Bool();
		if (!properties.canDestroy)
		{
			this.rigidbody.simulated = false;
		}
		this.hp = properties.health;
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.StartMoving();
		return this;
	}

	// Token: 0x0600266E RID: 9838 RVA: 0x001675A7 File Offset: 0x001659A7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600266F RID: 9839 RVA: 0x001675B5 File Offset: 0x001659B5
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x001675EC File Offset: 0x001659EC
	protected override void Die()
	{
		this.deathPuff.Create(base.transform.position);
		this.deathParts[0].Create(base.transform.position);
		this.deathParts[1].Create(base.transform.position);
		SpriteDeathParts spriteDeathParts = this.hat.CreatePart(base.transform.position);
		spriteDeathParts.animator.Play("_Teal");
		AudioManager.Play("sfx_dlc_omm_gnome_death");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death");
		this.Recycle<OldManLevelGnomeClimber>();
	}

	// Token: 0x06002671 RID: 9841 RVA: 0x00167689 File Offset: 0x00165A89
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002672 RID: 9842 RVA: 0x001676A7 File Offset: 0x00165AA7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002673 RID: 9843 RVA: 0x001676C5 File Offset: 0x00165AC5
	private void StartMoving()
	{
		base.StartCoroutine(this.move_up_cr());
	}

	// Token: 0x06002674 RID: 9844 RVA: 0x001676D4 File Offset: 0x00165AD4
	private IEnumerator move_up_cr()
	{
		base.animator.SetBool("DualSmash", this.properties.dualSmash);
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = this.properties.climbSpeed;
		yield return base.animator.WaitForAnimationToEnd(this, "Appear", false, true);
		while (this.smashPos != null && base.transform.position.y < this.smashPos.position.y + 60f)
		{
			base.transform.AddPosition(0f, speed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		base.transform.parent = this.smashPos;
		base.animator.SetTrigger("ReachedTop");
		yield return base.animator.WaitForAnimationToEnd(this, "ReachedTop", false, true);
		if (this.smashPos != null)
		{
			base.transform.SetPosition(new float?(this.smashPos.position.x), new float?(this.smashPos.position.y + 100f), null);
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.preAttackDelay);
		base.animator.Play("Anticipation");
		yield return base.animator.WaitForAnimationToEnd(this, "Anticipation", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.attackDelay);
		base.animator.SetTrigger("Attack");
		string vanishAnimation = (!this.properties.dualSmash) ? "Vanish" : "Vanish_Flipped";
		yield return base.animator.WaitForAnimationToEnd(this, vanishAnimation, false, true);
		this.Recycle<OldManLevelGnomeClimber>();
		yield return null;
		yield break;
	}

	// Token: 0x06002675 RID: 9845 RVA: 0x001676F0 File Offset: 0x00165AF0
	private void AniEvent_SpawnEffect(AnimationEvent ev)
	{
		Effect effect = this.smashEffect.Create(base.transform.position + new Vector3(this.smashRoot.localPosition.x * base.transform.localScale.x * ev.floatParameter, this.smashRoot.localPosition.y));
		effect.transform.SetScale(new float?(base.transform.localScale.x * ev.floatParameter), null, null);
		effect.GetComponent<Animator>().Play((!this.smashFXA) ? "B" : "A");
		this.smashFXA = !this.smashFXA;
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x001677D2 File Offset: 0x00165BD2
	private void AnimationEvent_SFX_OMM_Gnome_ClimberHammer()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_climber_attack");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_climber_attack");
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x001677EE File Offset: 0x00165BEE
	private void AnimationEvent_SFX_OMM_Gnome_ClimberHammerVocal()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_climber_attackvocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_climber_attackvocal");
	}

	// Token: 0x06002678 RID: 9848 RVA: 0x0016780A File Offset: 0x00165C0A
	private void WORKAROUND_NullifyFields()
	{
		this.deathPuff = null;
		this.deathParts = null;
		this.hat = null;
		this.smashEffect = null;
		this.smashRoot = null;
		this.smashPos = null;
		this.rigidbody = null;
	}

	// Token: 0x04002F0F RID: 12047
	private const float START_Y = -165f;

	// Token: 0x04002F10 RID: 12048
	private const float CLIMB_X_OFFSET = 120f;

	// Token: 0x04002F11 RID: 12049
	private const float TOP_Y_OFFSET = 60f;

	// Token: 0x04002F12 RID: 12050
	private const float SMASH_Y_OFFSET = 100f;

	// Token: 0x04002F13 RID: 12051
	[SerializeField]
	private Effect deathPuff;

	// Token: 0x04002F14 RID: 12052
	[SerializeField]
	private Effect[] deathParts;

	// Token: 0x04002F15 RID: 12053
	[SerializeField]
	private SpriteDeathPartsDLC hat;

	// Token: 0x04002F16 RID: 12054
	[SerializeField]
	private Effect smashEffect;

	// Token: 0x04002F17 RID: 12055
	[SerializeField]
	private Transform smashRoot;

	// Token: 0x04002F18 RID: 12056
	private LevelProperties.OldMan.ClimberGnomes properties;

	// Token: 0x04002F19 RID: 12057
	private Transform smashPos;

	// Token: 0x04002F1A RID: 12058
	[SerializeField]
	private DamageReceiver damageReceiver;

	// Token: 0x04002F1B RID: 12059
	[SerializeField]
	private new Rigidbody2D rigidbody;

	// Token: 0x04002F1C RID: 12060
	private bool smashFXA;

	// Token: 0x04002F1D RID: 12061
	private float hp;
}
