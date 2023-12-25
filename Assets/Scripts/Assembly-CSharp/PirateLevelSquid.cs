using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000727 RID: 1831
public class PirateLevelSquid : LevelProperties.Pirate.Entity
{
	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x060027E3 RID: 10211 RVA: 0x001753D3 File Offset: 0x001737D3
	// (set) Token: 0x060027E4 RID: 10212 RVA: 0x001753DB File Offset: 0x001737DB
	public PirateLevelSquid.State state { get; private set; }

	// Token: 0x060027E5 RID: 10213 RVA: 0x001753E4 File Offset: 0x001737E4
	protected override void Awake()
	{
		base.Awake();
		base.transform.position = PirateLevelSquid.START_POS;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.onDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x00175424 File Offset: 0x00173824
	private void Update()
	{
		if (this.state == PirateLevelSquid.State.Attack)
		{
			if (this.attackTime > this.squid.maxTime)
			{
				this.Exit();
			}
			else
			{
				this.attackTime += CupheadTime.Delta;
			}
		}
		float t = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, Mathf.PingPong(this.bobTime, 1f));
		base.transform.SetPosition(null, new float?(Mathf.Lerp(this.startY, this.endY, t)), null);
		this.bobTime += CupheadTime.Delta;
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x001754E4 File Offset: 0x001738E4
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
		this.squid = properties.CurrentState.squid;
		float value = this.squid.xPos.RandomFloat();
		base.transform.SetPosition(new float?(value), null, null);
		this.splashPrefab.Create(base.transform.position + new Vector3(0f, -40f, 0f));
		AudioManager.Play("level_pirate_squid_splash");
		this.hp = this.squid.hp;
		this.startY = base.transform.position.y;
		this.endY = this.startY + -20f;
		this.state = PirateLevelSquid.State.Enter;
		AudioManager.Play("level_pirate_squid_enter");
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x001755DB File Offset: 0x001739DB
	private void PlayPopSFX()
	{
		AudioManager.Play("level_pirate_squid_attack_pop");
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x001755E7 File Offset: 0x001739E7
	private void onDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x00175614 File Offset: 0x00173A14
	private void Exit()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.state = PirateLevelSquid.State.Exit;
		AudioManager.Play("level_pirate_squid_exit");
		base.animator.SetTrigger("OnExit");
		base.properties.OnBossDeath -= this.OnBossDeath;
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x00175668 File Offset: 0x00173A68
	public void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.state = PirateLevelSquid.State.Die;
		AudioManager.Play("level_pirate_squid_death");
		base.animator.SetTrigger("OnDeath");
		base.properties.OnBossDeath -= this.OnBossDeath;
	}

	// Token: 0x060027EC RID: 10220 RVA: 0x001756B9 File Offset: 0x00173AB9
	private void OnBossDeath()
	{
		this.Die();
	}

	// Token: 0x060027ED RID: 10221 RVA: 0x001756C4 File Offset: 0x00173AC4
	private IEnumerator attack_cr()
	{
		if (!this.InkAttackSoundActive)
		{
			AudioManager.PlayLoop("level_pirate_squid_attack_loop");
			this.InkAttackSoundActive = true;
		}
		while (this.state == PirateLevelSquid.State.Attack)
		{
			Vector2 v = Vector2.zero;
			v.y = this.squid.blobVelY;
			v.x = this.squid.blobVelX;
			this.inkBlob.Create(this.inkOrigin.position, v, this.squid.blobGravity);
			yield return CupheadTime.WaitForSeconds(this, this.squid.blobDelay);
		}
		AudioManager.Stop("level_pirate_squid_attack_loop");
		this.InkAttackSoundActive = false;
		yield break;
	}

	// Token: 0x060027EE RID: 10222 RVA: 0x001756DF File Offset: 0x00173ADF
	private void OnEnterAnimationComplete()
	{
		base.GetComponent<Collider2D>().enabled = true;
		this.state = PirateLevelSquid.State.Attack;
		this.attackTime = 0f;
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x0017570C File Offset: 0x00173B0C
	private void OnExitAnimationComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x00175719 File Offset: 0x00173B19
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.inkBlob = null;
		this.splashPrefab = null;
	}

	// Token: 0x040030A2 RID: 12450
	private static readonly Vector2 START_POS = new Vector2(-200f, -220f);

	// Token: 0x040030A3 RID: 12451
	private const float BOB_OFFSET = -20f;

	// Token: 0x040030A4 RID: 12452
	[SerializeField]
	private Transform inkOrigin;

	// Token: 0x040030A5 RID: 12453
	[SerializeField]
	private PirateLevelSquidProjectile inkBlob;

	// Token: 0x040030A6 RID: 12454
	[SerializeField]
	private Effect splashPrefab;

	// Token: 0x040030A8 RID: 12456
	private float hp;

	// Token: 0x040030A9 RID: 12457
	private float startY;

	// Token: 0x040030AA RID: 12458
	private float endY;

	// Token: 0x040030AB RID: 12459
	private float bobTime;

	// Token: 0x040030AC RID: 12460
	private float attackTime;

	// Token: 0x040030AD RID: 12461
	private LevelProperties.Pirate.Squid squid;

	// Token: 0x040030AE RID: 12462
	private bool InkAttackSoundActive;

	// Token: 0x02000728 RID: 1832
	public enum State
	{
		// Token: 0x040030B0 RID: 12464
		Init,
		// Token: 0x040030B1 RID: 12465
		Enter,
		// Token: 0x040030B2 RID: 12466
		Attack,
		// Token: 0x040030B3 RID: 12467
		Exit,
		// Token: 0x040030B4 RID: 12468
		Die
	}
}
