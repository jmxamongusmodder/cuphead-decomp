using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005A0 RID: 1440
public class DicePalaceBoozeLevelDecanter : DicePalaceBoozeLevelBossBase
{
	// Token: 0x06001BA0 RID: 7072 RVA: 0x000FBC87 File Offset: 0x000FA087
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x000FBCBD File Offset: 0x000FA0BD
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x000FBCCC File Offset: 0x000FA0CC
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float health = this.health;
		this.health -= info.damage;
		if (health > 0f)
		{
			Level.Current.timeline.DealDamage(Mathf.Clamp(health - this.health, 0f, health));
		}
		if (this.health < 0f && !base.isDead)
		{
			this.StartDying();
		}
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x000FBD44 File Offset: 0x000FA144
	public override void LevelInit(LevelProperties.DicePalaceBooze properties)
	{
		this.dropPosition.z = 0f;
		this.dropPosition.y = this.sprayYRoot.position.y;
		this.attackDelayIndex = UnityEngine.Random.Range(0, properties.CurrentState.decanter.attackDelayString.Split(new char[]
		{
			','
		}).Length);
		this.attacking = false;
		this.nextPlayerTarget = PlayerId.PlayerOne;
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.HandleDead;
		this.health = properties.CurrentState.decanter.decanterHP;
		AudioManager.Play("booze_decanter_intro");
		this.emitAudioFromObject.Add("booze_decanter_intro");
		base.LevelInit(properties);
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x000FBE1F File Offset: 0x000FA21F
	private void OnIntroEnd()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x000FBE30 File Offset: 0x000FA230
	private IEnumerator attack_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.decanter.attackDelayString.Split(new char[]
			{
				','
			})[this.attackDelayIndex]) - DicePalaceBoozeLevelBossBase.ATTACK_DELAY);
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
			AudioManager.Play("booze_decanter_attack");
			this.emitAudioFromObject.Add("booze_decanter_attack");
			base.StartCoroutine(this.spray_cr());
			this.attackDelayIndex++;
			if (this.attackDelayIndex >= base.properties.CurrentState.decanter.attackDelayString.Split(new char[]
			{
				','
			}).Length)
			{
				this.attackDelayIndex = 0;
			}
			if (this.nextPlayerTarget == PlayerId.PlayerOne)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					this.nextPlayerTarget = PlayerId.PlayerTwo;
				}
			}
			else
			{
				this.nextPlayerTarget = PlayerId.PlayerOne;
			}
			while (this.attacking)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x000FBE4C File Offset: 0x000FA24C
	private IEnumerator spray_cr()
	{
		this.attacking = true;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.decanter.beamAppearDelayRange.RandomFloat());
		AudioManager.Play("booze_decanter_spray_down");
		this.emitAudioFromObject.Add("booze_decanter_spray_down");
		GameObject spray = UnityEngine.Object.Instantiate<GameObject>(this.sprayPrefab, this.dropPosition, Quaternion.identity);
		this.attacking = false;
		this.dropPosition.x = PlayerManager.GetPlayer(this.nextPlayerTarget).center.x;
		Vector3 pos = spray.transform.position;
		pos.x = this.dropPosition.x;
		spray.transform.position = pos;
		yield return spray.GetComponent<Animator>().WaitForAnimationToEnd(this, "Spray", false, true);
		UnityEngine.Object.Destroy(spray);
		yield break;
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x000FBE67 File Offset: 0x000FA267
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001BA8 RID: 7080 RVA: 0x000FBE85 File Offset: 0x000FA285
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.sprayPrefab = null;
	}

	// Token: 0x040024B1 RID: 9393
	[SerializeField]
	private Transform sprayYRoot;

	// Token: 0x040024B2 RID: 9394
	[SerializeField]
	private GameObject sprayPrefab;

	// Token: 0x040024B3 RID: 9395
	private bool attacking;

	// Token: 0x040024B4 RID: 9396
	private int attackDelayIndex;

	// Token: 0x040024B5 RID: 9397
	private PlayerId nextPlayerTarget;

	// Token: 0x040024B6 RID: 9398
	private Vector3 dropPosition;

	// Token: 0x040024B7 RID: 9399
	private DamageDealer damageDealer;

	// Token: 0x040024B8 RID: 9400
	private DamageReceiver damageReceiver;
}
