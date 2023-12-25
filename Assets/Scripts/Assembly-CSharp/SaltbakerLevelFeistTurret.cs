using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007C7 RID: 1991
public class SaltbakerLevelFeistTurret : AbstractCollidableObject
{
	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06002D18 RID: 11544 RVA: 0x001A90DD File Offset: 0x001A74DD
	// (set) Token: 0x06002D19 RID: 11545 RVA: 0x001A90E5 File Offset: 0x001A74E5
	public bool IsActivated { get; private set; }

	// Token: 0x06002D1A RID: 11546 RVA: 0x001A90F0 File Offset: 0x001A74F0
	private void Start()
	{
		this.SFX_SALTBAKER_P2_Saltshaker_Appear();
		this.basePos = base.transform.position;
		this.fxRend.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)UnityEngine.Random.Range(0, 360)));
		this.coll.enabled = true;
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x001A9158 File Offset: 0x001A7558
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health <= 0f && this.IsActivated)
		{
			return;
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			if (this.IsActivated && this.parent.phaseTwoStarted)
			{
				if (!this.parent.preventAdditionalTurretLaunch)
				{
					if (this.parent.PreDamagePhaseTwoAndReturnWhetherDoomed(this.startHealth))
					{
						this.parent.preventAdditionalTurretLaunch = true;
					}
					base.StartCoroutine(this.fire_and_wait_to_respawn_cr());
				}
				else
				{
					this.health = 1f;
				}
			}
			else
			{
				this.health = 1f;
			}
		}
	}

	// Token: 0x06002D1C RID: 11548 RVA: 0x001A921E File Offset: 0x001A761E
	private void FixedUpdate()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.FixedUpdate();
		}
	}

	// Token: 0x06002D1D RID: 11549 RVA: 0x001A9236 File Offset: 0x001A7636
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x001A9254 File Offset: 0x001A7654
	public void Setup(LevelProperties.Saltbaker.Turrets properties, SaltbakerLevelSaltbaker parent, int index)
	{
		this.properties = properties;
		this.parent = parent;
		this.coll = base.GetComponent<Collider2D>();
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.index = index;
	}

	// Token: 0x06002D1F RID: 11551 RVA: 0x001A92BC File Offset: 0x001A76BC
	private void AniEvent_Activate()
	{
		this.health = this.properties.turretHealth;
		this.startHealth = this.health;
		this.IsActivated = true;
		this.coll.enabled = true;
	}

	// Token: 0x06002D20 RID: 11552 RVA: 0x001A92F0 File Offset: 0x001A76F0
	private IEnumerator fire_and_wait_to_respawn_cr()
	{
		base.animator.Play("Explode", 1, 0f);
		if (this.shootCR != null)
		{
			base.StopCoroutine(this.shootCR);
		}
		base.transform.position = this.basePos;
		base.animator.ResetTrigger("Shoot");
		this.coll.enabled = false;
		this.IsActivated = false;
		base.transform.localScale = new Vector3(1f, 1f);
		base.animator.Play("Attack" + this.index);
		this.SFX_SALTBAKER_P2_Saltshaker_DieLaunch();
		base.animator.Update(0f);
		yield return new WaitForEndOfFrame();
		yield return base.animator.WaitForAnimationToEnd(this, "Attack" + this.index, false, true);
		this.sprite.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.properties.respawnTime - 0.75f);
		base.transform.localScale = new Vector3(-Mathf.Sign(base.transform.position.x), 1f);
		this.fxRend.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)UnityEngine.Random.Range(0, 360)));
		this.sprite.sortingLayerName = "Projectiles";
		base.animator.Play("Intro");
		base.animator.Update(0f);
		this.SFX_SALTBAKER_P2_Saltshaker_Appear();
		this.sprite.enabled = true;
		yield break;
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x001A930B File Offset: 0x001A770B
	private void AniEvent_DamageSaltbaker()
	{
		this.SFX_SALTBAKER_P2_Saltshaker_LaunchImpact();
		this.parent.DamageSaltbaker(this.startHealth, this.index);
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x001A932A File Offset: 0x001A772A
	public void Shoot(bool isPink, float warning)
	{
		this.shootCR = base.StartCoroutine(this.shoot_cr(isPink, warning));
	}

	// Token: 0x06002D23 RID: 11555 RVA: 0x001A9340 File Offset: 0x001A7740
	private IEnumerator shoot_cr(bool isPink, float warning)
	{
		Vector3 upPos = base.transform.position + Vector3.up * (float)((base.transform.position.y >= 0f) ? -24 : 40);
		this.shootPink = isPink;
		base.animator.Play("ShootStart");
		base.animator.Update(0f);
		while (base.animator.GetCurrentAnimatorStateInfo(0).IsName("ShootStart"))
		{
			base.transform.position = Vector3.Lerp(this.basePos, upPos, EaseUtils.EaseOutSine(0f, 1f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
			yield return null;
		}
		base.transform.position = upPos;
		if (warning > 0.4f)
		{
			yield return CupheadTime.WaitForSeconds(this, warning - 0.4f);
			this.SFX_SALTBAKER_P2_Saltshaker_PreSneeze();
			yield return CupheadTime.WaitForSeconds(this, 0.4f);
		}
		else
		{
			this.SFX_SALTBAKER_P2_Saltshaker_PreSneeze();
			yield return CupheadTime.WaitForSeconds(this, warning);
		}
		base.animator.SetTrigger("Shoot");
		yield return base.animator.WaitForAnimationToStart(this, "ShootEnd", false);
		while (base.animator.GetCurrentAnimatorStateInfo(0).IsName("ShootEnd"))
		{
			base.transform.position = Vector3.Lerp(upPos, this.basePos, EaseUtils.EaseInSine(0f, 1f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
			yield return null;
		}
		base.transform.position = this.basePos;
		yield break;
	}

	// Token: 0x06002D24 RID: 11556 RVA: 0x001A936C File Offset: 0x001A776C
	private void AniEvent_SpawnProjectile()
	{
		float num = MathUtils.DirectionToAngle(PlayerManager.GetNext().center - base.transform.position);
		SaltbakerLevelTurretBullet saltbakerLevelTurretBullet = this.shootPink ? this.pinkBulletPrefab : this.bulletPrefab;
		saltbakerLevelTurretBullet = saltbakerLevelTurretBullet.Create(this.sneezeFX.transform.position + MathUtils.AngleToDirection(num) * 150f, num, this.properties.shotSpeed, this.parent);
		saltbakerLevelTurretBullet.transform.localScale = base.transform.localScale;
		this.sneezeFX.transform.localScale = base.transform.localScale;
		this.sneezeFX.transform.eulerAngles = new Vector3(0f, 0f, num - 45f);
		this.SFX_SALTBAKER_P2_Saltshaker_SneezeAttack();
	}

	// Token: 0x06002D25 RID: 11557 RVA: 0x001A945B File Offset: 0x001A785B
	private void AniEvent_BottomLeftTurretSnapForward()
	{
		this.sprite.sortingLayerName = "Foreground";
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x001A9470 File Offset: 0x001A7870
	private void LateUpdate()
	{
		this.pepperText.enabled = false;
		this.pepperTextFlip.enabled = false;
		SpriteRenderer spriteRenderer = (base.transform.localScale.x != 1f) ? this.pepperTextFlip : this.pepperText;
		spriteRenderer.enabled = (this.sprite.enabled && this.sprite.sprite != null && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack0") && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"));
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x001A956C File Offset: 0x001A796C
	public void Die()
	{
		this.coll.enabled = false;
		this.sprite.sortingLayerName = "Projectiles";
		this.StopAllCoroutines();
		base.animator.ResetTrigger("Shoot");
		base.transform.position = this.basePos;
		base.animator.SetBool("Dead", true);
		this.SFX_SALTBAKER_P2_Saltshaker_Disappear();
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x001A95D3 File Offset: 0x001A79D3
	private void SFX_SALTBAKER_P2_Saltshaker_Appear()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_appear");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_appear");
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x001A95EF File Offset: 0x001A79EF
	private void SFX_SALTBAKER_P2_Saltshaker_Disappear()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_disappear");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_disappear");
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x001A960B File Offset: 0x001A7A0B
	private void SFX_SALTBAKER_P2_Saltshaker_SneezeAttack()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_sneezeattack");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_sneezeattack");
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x001A9627 File Offset: 0x001A7A27
	private void SFX_SALTBAKER_P2_Saltshaker_PreSneeze()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_sneezepre");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_sneezepre");
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x001A9643 File Offset: 0x001A7A43
	private void SFX_SALTBAKER_P2_Saltshaker_DieLaunch()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_dielaunch");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_dielaunch");
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x001A965F File Offset: 0x001A7A5F
	private void SFX_SALTBAKER_P2_Saltshaker_LaunchImpact()
	{
		AudioManager.Play("sfx_DLC_Saltbaker_P2_Saltshaker_LaunchImpact");
	}

	// Token: 0x04003595 RID: 13717
	[SerializeField]
	private SaltbakerLevelTurretBullet bulletPrefab;

	// Token: 0x04003596 RID: 13718
	[SerializeField]
	private SaltbakerLevelTurretBullet pinkBulletPrefab;

	// Token: 0x04003597 RID: 13719
	private LevelProperties.Saltbaker.Turrets properties;

	// Token: 0x04003598 RID: 13720
	private SaltbakerLevelSaltbaker parent;

	// Token: 0x04003599 RID: 13721
	private Collider2D coll;

	// Token: 0x0400359A RID: 13722
	private SpriteRenderer sprite;

	// Token: 0x0400359B RID: 13723
	[SerializeField]
	private SpriteRenderer pepperText;

	// Token: 0x0400359C RID: 13724
	[SerializeField]
	private SpriteRenderer pepperTextFlip;

	// Token: 0x0400359D RID: 13725
	[SerializeField]
	private GameObject fxRend;

	// Token: 0x0400359E RID: 13726
	[SerializeField]
	private GameObject sneezeFX;

	// Token: 0x0400359F RID: 13727
	private DamageDealer damageDealer;

	// Token: 0x040035A0 RID: 13728
	private DamageReceiver damageReceiver;

	// Token: 0x040035A1 RID: 13729
	private float health;

	// Token: 0x040035A2 RID: 13730
	private float startHealth;

	// Token: 0x040035A3 RID: 13731
	private int index;

	// Token: 0x040035A4 RID: 13732
	private bool shootPink;

	// Token: 0x040035A5 RID: 13733
	private Coroutine shootCR;

	// Token: 0x040035A6 RID: 13734
	private Vector3 basePos;
}
