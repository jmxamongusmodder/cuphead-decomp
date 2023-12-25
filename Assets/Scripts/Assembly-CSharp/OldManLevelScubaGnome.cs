using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200070D RID: 1805
public class OldManLevelScubaGnome : AbstractProjectile
{
	// Token: 0x060026F8 RID: 9976 RVA: 0x0016D2DC File Offset: 0x0016B6DC
	public virtual OldManLevelScubaGnome Init(Vector3 pos, AbstractPlayerController player, bool isTypeA, bool onLeft, bool dartParryable, LevelProperties.OldMan.ScubaGnomes properties, OldManLevelGnomeLeader leader)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		base.transform.SetScale(new float?((float)((!onLeft) ? 1 : -1)), null, null);
		this.properties = properties;
		this.player = player;
		this.hp = properties.hp;
		this.isTypeA = isTypeA;
		this.onLeft = onLeft;
		this.leader = leader;
		this.dartParryable = dartParryable;
		base.animator.SetBool("IsGreen", Rand.Bool());
		base.animator.Play("Start");
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x0016D3A0 File Offset: 0x0016B7A0
	private void OnEnable()
	{
		Level.Current.OnLevelEndEvent += this.Dead;
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x0016D3B8 File Offset: 0x0016B7B8
	private void OnDisable()
	{
		if (Level.Current != null)
		{
			Level.Current.OnLevelEndEvent -= this.Dead;
		}
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x0016D3E0 File Offset: 0x0016B7E0
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x0016D40B File Offset: 0x0016B80B
	protected override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x0016D430 File Offset: 0x0016B830
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060026FE RID: 9982 RVA: 0x0016D44E File Offset: 0x0016B84E
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Dead();
		}
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x0016D484 File Offset: 0x0016B884
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		float start = base.transform.position.y;
		bool shotBullet = false;
		bool toTop = false;
		bool splashed = false;
		while (t < this.properties.scubaMoveTime)
		{
			float val = t / this.properties.scubaMoveTime;
			base.transform.SetPosition(null, new float?(start + Mathf.Sin(val * 3.1415927f / 2f) * this.properties.jumpHeight), null);
			t += CupheadTime.FixedDelta;
			if (!splashed && base.transform.position.y > this.leader.splashHandler.transform.position.y)
			{
				this.leader.splashHandler.SplashIn(base.transform.position.x + 35f * base.transform.localScale.x);
				splashed = true;
				this.SFX_JumpOut();
				this.SFX_Vocal();
			}
			this.underwaterSprite.color = new Color(1f, 1f, 1f, (1f - Mathf.InverseLerp(this.leader.splashHandler.transform.position.y + -50f, this.leader.splashHandler.transform.position.y + -50f - 140f, base.transform.position.y)) * 0.5f);
			if (!toTop && t >= 0.8f)
			{
				base.animator.SetTrigger("ToTop");
				toTop = true;
			}
			yield return wait;
		}
		splashed = false;
		t = 0f;
		while (t < this.properties.scubaMoveTime)
		{
			float val = t / this.properties.scubaMoveTime;
			base.transform.SetPosition(null, new float?(start + Mathf.Sin((val + 1f) * 3.1415927f / 2f) * this.properties.jumpHeight), null);
			t += CupheadTime.Delta;
			if (!splashed && base.transform.position.y < this.leader.splashHandler.transform.position.y - 75f)
			{
				this.leader.splashHandler.SplashIn(base.transform.position.x + 35f * base.transform.localScale.x);
				splashed = true;
				this.SFX_DiveDown();
			}
			this.underwaterSprite.color = new Color(1f, 1f, 1f, (1f - Mathf.InverseLerp(this.leader.splashHandler.transform.position.y + -50f, this.leader.splashHandler.transform.position.y + -50f - 140f, base.transform.position.y)) * 0.5f);
			if (!toTop && t >= 0.8f)
			{
				base.animator.SetTrigger("ToTop");
				toTop = true;
			}
			float dist = this.shootRoot.position.y - (this.player.center.y + this.properties.shootDistOffset);
			if (!shotBullet && (dist < 10f || this.shootRoot.position.y < 0f))
			{
				base.animator.SetTrigger("Shoot");
				shotBullet = true;
			}
			yield return wait;
		}
		this.Recycle<OldManLevelScubaGnome>();
		yield return null;
		yield break;
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x0016D4A0 File Offset: 0x0016B8A0
	private void Shoot()
	{
		float rotation = (base.transform.position.x >= 0f) ? 180f : 0f;
		float speed = (!this.isTypeA) ? this.properties.shotSpeedB : this.properties.shotSpeedA;
		BasicProjectile basicProjectile = this.projectile.Create(this.shootRoot.position, rotation, speed);
		basicProjectile.SetParryable(this.dartParryable);
		if (this.dartParryable)
		{
			basicProjectile.GetComponent<Animator>().Play("Pink");
		}
		basicProjectile.GetComponent<SpriteRenderer>().flipY = (base.transform.position.x > 0f);
		this.SFX_ShootDart();
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x0016D574 File Offset: 0x0016B974
	private void Dead()
	{
		this.deathPuff.Create(base.transform.position);
		for (int i = 0; i < this.deathParts.Length; i++)
		{
			if (i != 0 || UnityEngine.Random.Range(0, 10) == 0)
			{
				SpriteDeathParts spriteDeathParts = this.deathParts[i].CreatePart(base.transform.position);
				if (i != 0)
				{
					spriteDeathParts.animator.Play((!base.animator.GetBool("IsGreen")) ? "_Blue" : "_Teal");
				}
			}
		}
		AudioManager.Play("sfx_dlc_omm_gnome_death");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death");
		AudioManager.Stop("sfx_dlc_omm_p3_gnomediver_vocal");
		this.Recycle<OldManLevelScubaGnome>();
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x0016D63C File Offset: 0x0016BA3C
	private void SFX_DiveDown()
	{
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_divedown");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_divedown");
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x0016D658 File Offset: 0x0016BA58
	private void SFX_JumpOut()
	{
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_jumpout");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_jumpout");
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x0016D674 File Offset: 0x0016BA74
	private void SFX_ShootDart()
	{
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_shootdart");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_shootdart");
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x0016D690 File Offset: 0x0016BA90
	private void SFX_Vocal()
	{
		AudioManager.Play("sfx_dlc_omm_p3_gnomediver_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_gnomediver_vocal");
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x0016D6AC File Offset: 0x0016BAAC
	private void WORKAROUND_NullifyFields()
	{
		this.deathPuff = null;
		this.deathParts = null;
		this.projectile = null;
		this.shootRoot = null;
		this.player = null;
		this.leader = null;
		this.underwaterSprite = null;
	}

	// Token: 0x04002FAB RID: 12203
	private const float SPLASH_IN_TRIGGER_OFFSET = 75f;

	// Token: 0x04002FAC RID: 12204
	private const float SPLASH_X_POSITION_OFFSET = 35f;

	// Token: 0x04002FAD RID: 12205
	private const float UNDERWATER_FADE_OFFSET = -50f;

	// Token: 0x04002FAE RID: 12206
	private const float LOWEST_SHOOT_POS = 0f;

	// Token: 0x04002FAF RID: 12207
	[Header("Death FX")]
	[SerializeField]
	private Effect deathPuff;

	// Token: 0x04002FB0 RID: 12208
	[SerializeField]
	private SpriteDeathParts[] deathParts;

	// Token: 0x04002FB1 RID: 12209
	[Header("Prefabs")]
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04002FB2 RID: 12210
	[SerializeField]
	private Transform shootRoot;

	// Token: 0x04002FB3 RID: 12211
	private const float Y_DIST_TO_SHOOT = 10f;

	// Token: 0x04002FB4 RID: 12212
	private float hp;

	// Token: 0x04002FB5 RID: 12213
	private bool isTypeA;

	// Token: 0x04002FB6 RID: 12214
	private bool onLeft;

	// Token: 0x04002FB7 RID: 12215
	private LevelProperties.OldMan.ScubaGnomes properties;

	// Token: 0x04002FB8 RID: 12216
	private AbstractPlayerController player;

	// Token: 0x04002FB9 RID: 12217
	private OldManLevelGnomeLeader leader;

	// Token: 0x04002FBA RID: 12218
	private DamageReceiver damageReceiver;

	// Token: 0x04002FBB RID: 12219
	private bool dartParryable;

	// Token: 0x04002FBC RID: 12220
	[SerializeField]
	private SpriteRenderer underwaterSprite;
}
