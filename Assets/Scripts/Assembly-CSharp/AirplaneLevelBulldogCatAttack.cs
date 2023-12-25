using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B3 RID: 1203
public class AirplaneLevelBulldogCatAttack : LevelProperties.Airplane.Entity
{
	// Token: 0x1700030D RID: 781
	// (get) Token: 0x060013A5 RID: 5029 RVA: 0x000AE20C File Offset: 0x000AC60C
	// (set) Token: 0x060013A6 RID: 5030 RVA: 0x000AE214 File Offset: 0x000AC614
	public bool isAttacking { get; private set; }

	// Token: 0x060013A7 RID: 5031 RVA: 0x000AE21D File Offset: 0x000AC61D
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060013A8 RID: 5032 RVA: 0x000AE22A File Offset: 0x000AC62A
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060013A9 RID: 5033 RVA: 0x000AE232 File Offset: 0x000AC632
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060013AA RID: 5034 RVA: 0x000AE24A File Offset: 0x000AC64A
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x060013AB RID: 5035 RVA: 0x000AE253 File Offset: 0x000AC653
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x000AE274 File Offset: 0x000AC674
	public void StartCat(Vector2 pos)
	{
		this.count = UnityEngine.Random.Range(0, 3);
		this.isAttacking = true;
		base.transform.localScale = new Vector3(Mathf.Sign(pos.x), 1f);
		base.transform.position = pos;
		base.animator.SetBool("Exit", false);
		base.StartCoroutine(this.cat_cr());
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x000AE2E8 File Offset: 0x000AC6E8
	private IEnumerator cat_cr()
	{
		LevelProperties.Airplane.Triple p = base.properties.CurrentState.triple;
		base.animator.Play("Intro");
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 1E-05f, 1E-05f);
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_whistle_in");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_whistle_in");
		yield return base.animator.WaitForAnimationToStart(this, "IntroLoop", false);
		yield return CupheadTime.WaitForSeconds(this, p.initialDelay);
		base.animator.SetTrigger("Continue");
		AudioManager.PlayLoop("sfx_dlc_dogfight_p1_catattack_hover");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_hover");
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 0.15f, 0.5f);
		AudioManager.Play("sfx_dlc_dogfight_p1_catattack_enter");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_enter");
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		yield return CupheadTime.WaitForSeconds(this, p.shootWarning);
		this.SFX_DOGFIGHT_Cat_Shoot();
		base.animator.Play("ShootA");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "ShootA", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.delayAfterFirst.RandomFloat());
		this.SFX_DOGFIGHT_Cat_Shoot();
		base.animator.Play("ShootB");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "ShootB", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.delayAfterSecond.RandomFloat());
		this.SFX_DOGFIGHT_Cat_Shoot();
		base.animator.Play("ShootA");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "ShootA", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.shootRecovery);
		base.animator.SetBool("Exit", true);
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_catattack_hover", 0f, 0.5f);
		AudioManager.Play("sfx_dlc_dogfight_p1_catattack_leave");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catattack_leave");
		yield return base.animator.WaitForAnimationToEnd(this, "Exit", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.returnDelay);
		this.isAttacking = false;
		yield break;
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x000AE303 File Offset: 0x000AC703
	public void EarlyExit()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.early_exit_cr());
	}

	// Token: 0x060013AF RID: 5039 RVA: 0x000AE318 File Offset: 0x000AC718
	private IEnumerator early_exit_cr()
	{
		base.animator.SetBool("Exit", true);
		yield return base.animator.WaitForAnimationToStart(this, "None", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.triple.returnDelay);
		this.isAttacking = false;
		yield break;
	}

	// Token: 0x060013B0 RID: 5040 RVA: 0x000AE334 File Offset: 0x000AC734
	private void AniEvent_Shoot()
	{
		LevelProperties.Airplane.Triple triple = base.properties.CurrentState.triple;
		float num = triple.attackAngleRange.RandomFloat();
		float num2 = (base.transform.localScale.x >= 0f) ? 180f : 0f;
		BasicProjectile basicProjectile = this.projectile.Create(this.root.position, num2 + num, triple.bulletSpeed);
		basicProjectile.transform.localScale = new Vector3(1f, (float)((num2 <= 0f) ? 1 : -1));
		Animator component = basicProjectile.GetComponent<Animator>();
		component.Play((this.count % 3).ToString(), 0, UnityEngine.Random.Range(0.375f, 0.75f));
		component.Update(0f);
		this.count++;
		base.animator.Play(UnityEngine.Random.Range(0, 4).ToString(), 1, 0f);
		base.animator.Update(0f);
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x000AE462 File Offset: 0x000AC862
	private void OnDisable()
	{
		base.GetComponent<HitFlash>().StopAllCoroutinesWithoutSettingScale();
		base.GetComponent<SpriteRenderer>().color = Color.black;
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x000AE47F File Offset: 0x000AC87F
	private void AniEvent_IntroFX()
	{
		base.animator.Play("IntroFX", 1);
		base.animator.Update(0f);
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x000AE4A2 File Offset: 0x000AC8A2
	private void AniEvent_FlashA()
	{
		base.animator.Play("FlashA", 2);
		base.animator.Update(0f);
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x000AE4C5 File Offset: 0x000AC8C5
	private void AniEvent_FlashB()
	{
		base.animator.Play("FlashB", 2);
		base.animator.Update(0f);
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x000AE4E8 File Offset: 0x000AC8E8
	private void AniEvent_EmberA()
	{
		base.animator.Play("EmberA", 3);
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x000AE4FB File Offset: 0x000AC8FB
	private void AniEvent_EmberB()
	{
		base.animator.Play("EmberB", 3);
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x000AE50E File Offset: 0x000AC90E
	private void SFX_DOGFIGHT_Cat_Shoot()
	{
		AudioManager.Play("sfx_dlc_dogfight_catgun_shoot");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_catgun_shoot");
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x000AE52A File Offset: 0x000AC92A
	private void SFX_DOGFIGHT_Cat_StartMeow()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_catgunmeow");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_catgunmeow");
	}

	// Token: 0x04001CC8 RID: 7368
	[SerializeField]
	private AirplaneLevelBulldogPlane main;

	// Token: 0x04001CC9 RID: 7369
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04001CCA RID: 7370
	[SerializeField]
	private Transform root;

	// Token: 0x04001CCB RID: 7371
	private DamageDealer damageDealer;

	// Token: 0x04001CCC RID: 7372
	private int count;
}
