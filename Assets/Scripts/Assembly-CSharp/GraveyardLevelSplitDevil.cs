using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006C7 RID: 1735
public class GraveyardLevelSplitDevil : LevelProperties.Graveyard.Entity
{
	// Token: 0x060024DB RID: 9435 RVA: 0x00159592 File Offset: 0x00157992
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x001595AA File Offset: 0x001579AA
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x001595D4 File Offset: 0x001579D4
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += base.GetComponentInParent<GraveyardLevelSplitDevil>().OnDamageTaken;
		if (base.transform.localScale.x > 0f)
		{
			this.SetIsAngel(true);
			this.id = 0;
		}
		this.sideString = ((base.transform.localScale.x >= 0f) ? "left" : "right");
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x00159678 File Offset: 0x00157A78
	public override void LevelInit(LevelProperties.Graveyard properties)
	{
		base.LevelInit(properties);
		this.numProjectiles = new PatternString(properties.CurrentState.splitDevilProjectiles.numProjectiles[this.id], true);
		this.projectileAngleOffset = new PatternString(properties.CurrentState.splitDevilProjectiles.angleOffsetString, true);
		this.projectilePinkString = new PatternString(properties.CurrentState.splitDevilProjectiles.pinkString, true);
		this.level = (Level.Current as GraveyardLevel);
		base.StartCoroutine(this.fade_in_cr());
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x00159704 File Offset: 0x00157B04
	private IEnumerator fade_in_cr()
	{
		this.mainRend.color = new Color(0f, 0f, 0f, 0f);
		this.haloRend.color = new Color(0f, 0f, 0f, 0f);
		float t = 0f;
		while (t < 4f)
		{
			this.mainRend.color = new Color(0f, 0f, 0f, t / 4f);
			this.haloRend.color = new Color(0f, 0f, 0f, t / 4f);
			t += CupheadTime.Delta;
			yield return new WaitForFixedUpdate();
		}
		this.mainRend.color = new Color(0f, 0f, 0f, 1f);
		this.haloRend.color = new Color(0f, 0f, 0f, 1f);
		yield break;
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x00159720 File Offset: 0x00157B20
	private void SetIsAngel(bool value)
	{
		if (this.isAngel != value && !value)
		{
			AudioManager.Play("sfx_dlc_graveyard_changedirectionbad");
			this.emitAudioFromObject.Add("sfx_dlc_graveyard_changedirectionbad");
		}
		base.animator.SetBool("isAngel", value);
		this.coll.enabled = !value;
		this.isAngel = value;
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_angelsing_" + this.sideString, (!this.isAngel) ? 1E-05f : 0.4f, 0.4f);
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_devilangryrage_" + this.sideString, this.isAngel ? 1E-05f : 0.4f, 0.4f);
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x001597E8 File Offset: 0x00157BE8
	private void LateUpdate()
	{
		if (this.dead)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null && !levelPlayerController.IsDead)
			{
				num2++;
				num += (int)Mathf.Sign(levelPlayerController.transform.localScale.x);
			}
		}
		if (Mathf.Abs(num) == num2)
		{
			if (this.isAngel != (Mathf.Sign((float)num) == Mathf.Sign(base.transform.localScale.x)) && this.headLooping)
			{
				this.ResyncHead();
			}
			this.SetIsAngel(Mathf.Sign((float)num) == Mathf.Sign(base.transform.localScale.x));
		}
		this.headlessRend.enabled = (this.headRend.sprite != null);
		this.mainRend.enabled = !this.headlessRend.enabled;
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x00159930 File Offset: 0x00157D30
	public void NextPattern()
	{
		base.StartCoroutine((!this.level.CheckForBeamAttack()) ? this.projectile_cr() : this.roar_cr());
	}

	// Token: 0x060024E3 RID: 9443 RVA: 0x0015995C File Offset: 0x00157D5C
	private IEnumerator roar_cr()
	{
		LevelProperties.Graveyard.SplitDevilBeam p = base.properties.CurrentState.splitDevilBeam;
		base.animator.SetBool("isSinging", true);
		int targetA = Animator.StringToHash(base.animator.GetLayerName(0) + ".SingStartAngel");
		int targetB = Animator.StringToHash(base.animator.GetLayerName(0) + ".SingStartDevil");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != targetA && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != targetB)
		{
			yield return null;
		}
		this.beamPrefab.Create(new Vector3(Mathf.Sign(base.transform.position.x) * (float)(Level.Current.Right - 50), 80f), p.speed.RandomFloat() * -Mathf.Sign(base.transform.position.x), p.warning, this);
		yield return new WaitForSeconds(1f);
		base.animator.SetBool("isSinging", false);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack.RandomFloat());
		this.NextPattern();
		yield break;
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x00159978 File Offset: 0x00157D78
	private void ResyncHead()
	{
		float num = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 0.25f;
		float num2 = base.animator.GetCurrentAnimatorStateInfo(1).normalizedTime - base.animator.GetCurrentAnimatorStateInfo(1).normalizedTime % 0.25f;
		if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".IdleAngel"))
		{
			base.animator.Play("ShootLoopAngel", 1, num2 + num);
			base.animator.Update(0f);
		}
		else if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".IdleDevil"))
		{
			base.animator.Play("ShootLoopDevil", 1, num2 + num);
			base.animator.Update(0f);
		}
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x00159A88 File Offset: 0x00157E88
	private IEnumerator projectile_cr()
	{
		this.triggerShoot = true;
		while (this.triggerShoot)
		{
			yield return null;
		}
		base.animator.Play("Charge", 2, 0f);
		yield return CupheadTime.WaitForSeconds(this, 0.16666667f);
		this.headLooping = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Charge", 2, false, false);
		LevelProperties.Graveyard.SplitDevilProjectiles p = base.properties.CurrentState.splitDevilProjectiles;
		AbstractPlayerController player = PlayerManager.GetNext();
		float delayBetweenProjectiles = p.delayBetweenProjectiles.RandomFloat();
		LevelPlayerController p2 = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
		int projectileCount = this.numProjectiles.PopInt();
		for (int i = 0; i < projectileCount; i++)
		{
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			float rotation = MathUtils.DirectionToAngle(player.center - this.projectileRoot.transform.position);
			rotation += this.projectileAngleOffset.PopFloat();
			bool isPink = this.projectilePinkString.PopLetter() == 'P';
			if (isPink)
			{
				this.projectilePinkPrefab.Create(this.projectileRoot.transform.position, rotation, p.projectileSpeed, this);
			}
			else
			{
				this.projectilePrefab.Create(this.projectileRoot.transform.position, rotation, p.projectileSpeed, this);
			}
			base.animator.Play((!this.isAngel) ? ((!Rand.Bool()) ? "FireB" : "FireA") : "Light", (!isPink) ? 3 : 4, 0f);
			this.shootFXRend[0].flipX = (this.isAngel && Rand.Bool());
			this.shootFXRend[1].flipX = (this.isAngel && Rand.Bool());
			this.shootFXRend[0].flipY = (this.isAngel && Rand.Bool());
			this.shootFXRend[1].flipY = (this.isAngel && Rand.Bool());
			this.SFX_SplitDevil_Shoot();
			if (i < projectileCount - 1)
			{
				yield return CupheadTime.WaitForSeconds(this, Mathf.Clamp(delayBetweenProjectiles - 0.45833334f, 0f, float.MaxValue));
				base.animator.Play("Charge", 2, 0f);
				yield return base.animator.WaitForAnimationToEnd(this, "Charge", 2, false, true);
				yield return CupheadTime.WaitForSeconds(this, 0.125f);
			}
		}
		this.headLooping = false;
		base.animator.SetBool("isShooting", false);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack.RandomFloat());
		this.NextPattern();
		yield break;
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x00159AA4 File Offset: 0x00157EA4
	public void AniEvent_CanStartShoot()
	{
		if (this.triggerShoot)
		{
			bool flag = base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(base.animator.GetLayerName(0) + ".IdleAngel");
			if (flag != this.isAngel)
			{
				base.animator.Play((!flag) ? "ShootStartDevilToAngel" : "ShootStartAngelToDevil", 1, 0f);
			}
			else
			{
				base.animator.Play((!this.isAngel) ? "ShootStartDevil" : "ShootStartAngel", 1, 0f);
			}
			base.animator.SetBool("isShooting", true);
			this.triggerShoot = false;
		}
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x00159B68 File Offset: 0x00157F68
	private void OnDisable()
	{
		if (!base.animator.GetBool("isShooting"))
		{
			this.headlessRend.enabled = false;
			this.mainRend.enabled = true;
		}
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x00159B97 File Offset: 0x00157F97
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x00159BAC File Offset: 0x00157FAC
	public void Die()
	{
		this.dead = true;
		this.headRend.enabled = false;
		this.headlessRend.enabled = false;
		this.mainRend.enabled = true;
		this.triggerShoot = false;
		this.StopAllCoroutines();
		base.animator.Play((!this.isAngel) ? "DeathDevilLoop" : "DeathAngelLoop");
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x060024EA RID: 9450 RVA: 0x00159C24 File Offset: 0x00158024
	private IEnumerator death_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2.5f);
		base.animator.SetTrigger("DeathContinue");
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		yield break;
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x00159C3F File Offset: 0x0015803F
	private void ActivateBGSkellyMask()
	{
		this.bgSkellyMask.SetActive(true);
	}

	// Token: 0x060024EC RID: 9452 RVA: 0x00159C50 File Offset: 0x00158050
	private void SFXSingRoar()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_angelsing_" + this.sideString, (!this.isAngel) ? 1E-05f : 0.4f, 0.01f);
		AudioManager.FadeSFXVolume("sfx_dlc_graveyard_devilangryrage_" + this.sideString, this.isAngel ? 1E-05f : 0.4f, 0.01f);
		AudioManager.Play("sfx_dlc_graveyard_angelsing_" + this.sideString);
		this.emitAudioFromObject.Add("sfx_dlc_graveyard_angelsing_" + this.sideString);
		AudioManager.Play("sfx_dlc_graveyard_devilangryrage_" + this.sideString);
		this.emitAudioFromObject.Add("sfx_dlc_graveyard_devilangryrage_" + this.sideString);
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x00159D25 File Offset: 0x00158125
	private void AnimationEvent_SFX_SplitDevil_AngelSing()
	{
		this.SFXSingRoar();
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x00159D2D File Offset: 0x0015812D
	private void AnimationEvent_SFX_SplitDevil_DevilRage()
	{
		this.SFXSingRoar();
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x00159D38 File Offset: 0x00158138
	private void SFX_SplitDevil_Shoot()
	{
		AudioManager.Play((!this.isAngel) ? "sfx_dlc_graveyard_devil_shoot" : "sfx_DLC_Graveyard_Angel_Shoot");
		this.emitAudioFromObject.Add((!this.isAngel) ? "sfx_dlc_graveyard_devil_shoot" : "sfx_DLC_Graveyard_Angel_Shoot");
	}

	// Token: 0x04002D7B RID: 11643
	private const float SING_ROAR_MAX_VOLUME = 0.4f;

	// Token: 0x04002D7C RID: 11644
	[SerializeField]
	private GraveyardLevelSplitDevilProjectile projectilePrefab;

	// Token: 0x04002D7D RID: 11645
	[SerializeField]
	private GraveyardLevelSplitDevilProjectile projectilePinkPrefab;

	// Token: 0x04002D7E RID: 11646
	[SerializeField]
	private GraveyardLevelSplitDevilBeam beamPrefab;

	// Token: 0x04002D7F RID: 11647
	private DamageDealer damageDealer;

	// Token: 0x04002D80 RID: 11648
	private DamageReceiver damageReceiver;

	// Token: 0x04002D81 RID: 11649
	[SerializeField]
	private GameObject bgSkellyMask;

	// Token: 0x04002D82 RID: 11650
	[SerializeField]
	private Collider2D coll;

	// Token: 0x04002D83 RID: 11651
	[SerializeField]
	private SpriteRenderer headRend;

	// Token: 0x04002D84 RID: 11652
	[SerializeField]
	private SpriteRenderer mainRend;

	// Token: 0x04002D85 RID: 11653
	[SerializeField]
	private SpriteRenderer headlessRend;

	// Token: 0x04002D86 RID: 11654
	[SerializeField]
	private SpriteRenderer haloRend;

	// Token: 0x04002D87 RID: 11655
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04002D88 RID: 11656
	private PatternString numProjectiles;

	// Token: 0x04002D89 RID: 11657
	private PatternString projectileAngleOffset;

	// Token: 0x04002D8A RID: 11658
	private bool triggerShoot;

	// Token: 0x04002D8B RID: 11659
	public bool isAngel;

	// Token: 0x04002D8C RID: 11660
	public bool dead;

	// Token: 0x04002D8D RID: 11661
	private bool headLooping;

	// Token: 0x04002D8E RID: 11662
	[SerializeField]
	private SpriteRenderer[] shootFXRend;

	// Token: 0x04002D8F RID: 11663
	private int id = 1;

	// Token: 0x04002D90 RID: 11664
	private GraveyardLevel level;

	// Token: 0x04002D91 RID: 11665
	private PatternString projectilePinkString;

	// Token: 0x04002D92 RID: 11666
	private string sideString;
}
