using System;
using UnityEngine;

// Token: 0x020004DC RID: 1244
public class BaronessLevelBaroness : AbstractCollidableObject
{
	// Token: 0x0600154A RID: 5450 RVA: 0x000BF04D File Offset: 0x000BD44D
	protected override void Awake()
	{
		base.Awake();
		this.isEasyFinal = false;
		this.damageReceiver = this.shootPoint.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x000BF084 File Offset: 0x000BD484
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.isEasyFinal)
		{
			this.properties.DealDamage(info.damage);
			if (this.properties.CurrentHealth <= 0f && this.isEasyFinal)
			{
				this.isEasyFinal = false;
			}
		}
		else if (this.health < 0f && !this.shotEnough)
		{
			this.shotEnough = true;
		}
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x000BF10E File Offset: 0x000BD50E
	private void Update()
	{
		if (this.shotEnough)
		{
			this.health = this.maxHealth;
		}
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x000BF127 File Offset: 0x000BD527
	public void getProperties(LevelProperties.Baroness properties, float health, BaronessLevelCastle parent)
	{
		this.properties = properties;
		this.maxHealth = health;
		this.parent = parent;
		health = this.maxHealth;
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x000BF146 File Offset: 0x000BD546
	public void ShootCounter()
	{
		this.FireProjectileBunch();
		this.shootCounter++;
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x000BF15C File Offset: 0x000BD55C
	public void PopUpCounter()
	{
		this.popUpCounter++;
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x000BF16C File Offset: 0x000BD56C
	public void TransformCounter()
	{
		this.transformCounter++;
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x000BF17C File Offset: 0x000BD57C
	private void FireProjectileBunch()
	{
		AudioManager.Play("level_baroness_gun_fire");
		AbstractPlayerController next = PlayerManager.GetNext();
		float x = next.transform.position.x - base.transform.position.x;
		float y = next.transform.position.y - base.transform.position.y;
		float pointAt = Mathf.Atan2(y, x) * 57.29578f;
		BaronessLevelBaronessProjectileBunch baronessLevelBaronessProjectileBunch = UnityEngine.Object.Instantiate<BaronessLevelBaronessProjectileBunch>(this.baronessProjectileBunch);
		baronessLevelBaronessProjectileBunch.Init(this.baronessProjectileShootPoint.position, (float)this.properties.CurrentState.baronessVonBonbon.projectileSpeed, pointAt, this.properties.CurrentState.baronessVonBonbon, this.parent);
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x000BF250 File Offset: 0x000BD650
	public void FireFinalProjectile()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		BaronessLevelFollowingProjectile baronessLevelFollowingProjectile = UnityEngine.Object.Instantiate<BaronessLevelFollowingProjectile>(this.baronessFollowProjectile);
		baronessLevelFollowingProjectile.Init(this.baronessTossPoint.position, next.transform.position, this.properties.CurrentState.baronessVonBonbon, next, this.parent);
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x000BF2A7 File Offset: 0x000BD6A7
	private void SoundVoiceAngry()
	{
		AudioManager.Play("level_baroness_voice_angry");
		this.emitAudioFromObject.Add("level_baroness_voice_angry");
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x000BF2C3 File Offset: 0x000BD6C3
	private void SoundVoiceEffort()
	{
		AudioManager.Play("level_baroness_voice_effort");
		this.emitAudioFromObject.Add("level_baroness_voice_effort");
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x000BF2DF File Offset: 0x000BD6DF
	private void SoundVoiceCastleyank()
	{
		AudioManager.Play("level_baroness_voice_castleyank");
		this.emitAudioFromObject.Add("level_baroness_voice_castleyank");
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x000BF2FB File Offset: 0x000BD6FB
	private void SoundVoiceIntroA()
	{
		AudioManager.Play("level_baroness_voice_intro_a");
		this.emitAudioFromObject.Add("level_baroness_voice_intro_a");
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x000BF317 File Offset: 0x000BD717
	private void SoundVoiceIntroB()
	{
		AudioManager.Play("level_baroness_voice_intro_b");
		this.emitAudioFromObject.Add("level_baroness_voice_intro_b");
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x000BF333 File Offset: 0x000BD733
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.baronessProjectileBunch = null;
		this.baronessFollowProjectile = null;
	}

	// Token: 0x04001EA4 RID: 7844
	[SerializeField]
	private Transform baronessTossPoint;

	// Token: 0x04001EA5 RID: 7845
	[SerializeField]
	private Transform baronessProjectileShootPoint;

	// Token: 0x04001EA6 RID: 7846
	[SerializeField]
	private BaronessLevelBaronessProjectileBunch baronessProjectileBunch;

	// Token: 0x04001EA7 RID: 7847
	[SerializeField]
	private BaronessLevelFollowingProjectile baronessFollowProjectile;

	// Token: 0x04001EA8 RID: 7848
	[SerializeField]
	public Transform shootPoint;

	// Token: 0x04001EA9 RID: 7849
	private LevelProperties.Baroness properties;

	// Token: 0x04001EAA RID: 7850
	private BaronessLevelCastle parent;

	// Token: 0x04001EAB RID: 7851
	public bool isEasyFinal;

	// Token: 0x04001EAC RID: 7852
	public int shootCounter;

	// Token: 0x04001EAD RID: 7853
	public int popUpCounter;

	// Token: 0x04001EAE RID: 7854
	public int transformCounter;

	// Token: 0x04001EAF RID: 7855
	public bool shotEnough;

	// Token: 0x04001EB0 RID: 7856
	private float health;

	// Token: 0x04001EB1 RID: 7857
	private float maxHealth;

	// Token: 0x04001EB2 RID: 7858
	private DamageReceiver damageReceiver;
}
