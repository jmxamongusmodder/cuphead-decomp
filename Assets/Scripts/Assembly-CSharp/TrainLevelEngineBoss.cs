using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200080D RID: 2061
public class TrainLevelEngineBoss : LevelProperties.Train.Entity
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06002FBC RID: 12220 RVA: 0x001C49D0 File Offset: 0x001C2DD0
	// (remove) Token: 0x06002FBD RID: 12221 RVA: 0x001C4A08 File Offset: 0x001C2E08
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TrainLevelEngineBoss.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06002FBE RID: 12222 RVA: 0x001C4A40 File Offset: 0x001C2E40
	// (remove) Token: 0x06002FBF RID: 12223 RVA: 0x001C4A78 File Offset: 0x001C2E78
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x06002FC0 RID: 12224 RVA: 0x001C4AB0 File Offset: 0x001C2EB0
	protected override void Awake()
	{
		base.Awake();
		this.tailSwitch = TrainLevelEngineBossTail.Create(this.tailRoot);
		this.tailSwitch.OnActivate += this.OnTailParried;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.smokeRenderer = this.dropperRoot.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x001C4B1F File Offset: 0x001C2F1F
	private void Start()
	{
		this.UpdateHeartDamageReceiver();
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x001C4B27 File Offset: 0x001C2F27
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.health = properties.CurrentState.engine.health;
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x001C4B48 File Offset: 0x001C2F48
	public void StartBoss()
	{
		AudioManager.Play("train_engine_boss_run_start");
		this.TrainRunStep = true;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.tailTimer_cr());
		base.StartCoroutine(this.fireProjectiles_cr());
		this.StartAttack();
		this.UpdateHeartDamageReceiver();
	}

	// Token: 0x06002FC4 RID: 12228 RVA: 0x001C4B99 File Offset: 0x001C2F99
	private void UpdateHeartDamageReceiver()
	{
		this.heartDamageReceiver.enabled = (this.doorState == TrainLevelEngineBoss.DoorState.Open || this.doorState == TrainLevelEngineBoss.DoorState.Closing);
	}

	// Token: 0x06002FC5 RID: 12229 RVA: 0x001C4BC0 File Offset: 0x001C2FC0
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		base.animator.SetBool("Hit", true);
		base.CancelInvoke("StopHitAnim");
		base.Invoke("StopHitAnim", 0.25f);
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002FC6 RID: 12230 RVA: 0x001C4C4A File Offset: 0x001C304A
	private void Die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.damageReceiver.enabled = false;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002FC7 RID: 12231 RVA: 0x001C4C80 File Offset: 0x001C3080
	private IEnumerator die_cr()
	{
		AudioManager.Play("train_engine_boss_die");
		this.emitAudioFromObject.Add("train_engine_boss_die");
		base.animator.SetTrigger("OnDeath");
		this.door.SetActive(false);
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		yield return base.TweenPositionX(base.transform.position.x, -300f, 2.5f * Mathf.Abs(-300f - base.transform.position.x) / 400f, EaseUtils.EaseType.easeInOutSine);
		for (;;)
		{
			yield return base.TweenPositionX(base.transform.position.x, 100f, 2.5f, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenPositionX(base.transform.position.x, -300f, 2.5f, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x001C4C9B File Offset: 0x001C309B
	private void StopHitAnim()
	{
		base.animator.SetBool("Hit", false);
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x001C4CAE File Offset: 0x001C30AE
	public void SpawnDustOnFeet()
	{
		this.footDustPrefab.Create(this.footDustRoot.position, this.footDustRoot.localScale).Play();
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x001C4CD6 File Offset: 0x001C30D6
	private void StartAttack()
	{
		this.StopAttack();
		this.attackCoroutine = this.attack_cr();
		base.StartCoroutine(this.attackCoroutine);
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x001C4CF7 File Offset: 0x001C30F7
	private void StopAttack()
	{
		if (this.attackCoroutine != null)
		{
			base.StopCoroutine(this.attackCoroutine);
		}
	}

	// Token: 0x06002FCC RID: 12236 RVA: 0x001C4D10 File Offset: 0x001C3110
	private void OnAttackAnimComplete()
	{
		this.dropperPrefab.Create(this.dropperRoot.position, base.properties.CurrentState.engine.projectileUpSpeed, base.properties.CurrentState.engine.projectileXSpeed, base.properties.CurrentState.engine.projectileGravity);
	}

	// Token: 0x06002FCD RID: 12237 RVA: 0x001C4D78 File Offset: 0x001C3178
	public void SmokeFX()
	{
		this.smokeRenderer.flipX = Rand.Bool();
		base.animator.SetTrigger("Smoke");
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x001C4D9C File Offset: 0x001C319C
	private IEnumerator attack_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.engine.projectileDelay);
			base.animator.SetTrigger("OnAttack");
			AudioManager.Play("train_engine_boss_attack");
			this.emitAudioFromObject.Add("train_engine_boss_attack");
			yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
			yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
		}
		yield break;
	}

	// Token: 0x06002FCF RID: 12239 RVA: 0x001C4DB8 File Offset: 0x001C31B8
	private IEnumerator fireProjectiles_cr()
	{
		for (;;)
		{
			if (this.doorState == TrainLevelEngineBoss.DoorState.Open)
			{
				base.animator.SetTrigger("FireAttack");
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.engine.fireDelay);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002FD0 RID: 12240 RVA: 0x001C4DD4 File Offset: 0x001C31D4
	public void SpawnProjectile()
	{
		Vector2 zero = Vector2.zero;
		zero.y = base.properties.CurrentState.engine.fireVelocityY;
		zero.x = base.properties.CurrentState.engine.fireVelocityX;
		this.firePrefab.Create(this.fireRoot.position, zero, (float)base.properties.CurrentState.engine.fireGravity);
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06002FD1 RID: 12241 RVA: 0x001C4E5B File Offset: 0x001C325B
	// (set) Token: 0x06002FD2 RID: 12242 RVA: 0x001C4E63 File Offset: 0x001C3263
	private TrainLevelEngineBoss.DoorState doorState
	{
		get
		{
			return this._ds;
		}
		set
		{
			if (value == this._ds)
			{
				return;
			}
			this._ds = value;
			this.UpdateHeartDamageReceiver();
		}
	}

	// Token: 0x06002FD3 RID: 12243 RVA: 0x001C4E7F File Offset: 0x001C327F
	private void DoorAnimOpenStarted()
	{
		if (this.desiredDoorState == TrainLevelEngineBoss.DoorState.Open && this.doorState == TrainLevelEngineBoss.DoorState.Closed)
		{
			this.doorState = TrainLevelEngineBoss.DoorState.Opening;
			base.animator.SetTrigger("Open");
		}
		this.UpdateDoorSprite();
	}

	// Token: 0x06002FD4 RID: 12244 RVA: 0x001C4EB5 File Offset: 0x001C32B5
	private void DoorAnimCloseStarted()
	{
		if (this.desiredDoorState == TrainLevelEngineBoss.DoorState.Closed && this.doorState == TrainLevelEngineBoss.DoorState.Open)
		{
			this.doorState = TrainLevelEngineBoss.DoorState.Closing;
			base.animator.SetTrigger("Close");
		}
		this.UpdateDoorSprite();
	}

	// Token: 0x06002FD5 RID: 12245 RVA: 0x001C4EEB File Offset: 0x001C32EB
	private void DoorOpenAnimComplete()
	{
		if (this.doorState == TrainLevelEngineBoss.DoorState.Opening)
		{
			AudioManager.Play("train_engine_boss_door");
			this.emitAudioFromObject.Add("train_engine_boss_door");
			this.doorState = TrainLevelEngineBoss.DoorState.Open;
		}
		this.UpdateDoorSprite();
	}

	// Token: 0x06002FD6 RID: 12246 RVA: 0x001C4F20 File Offset: 0x001C3320
	private void DoorCloseAnimComplete()
	{
		if (this.doorState == TrainLevelEngineBoss.DoorState.Closing)
		{
			AudioManager.Play("train_engine_boss_door_shut");
			this.emitAudioFromObject.Add("train_engine_boss_door_shut");
			this.doorState = TrainLevelEngineBoss.DoorState.Closed;
		}
		this.UpdateDoorSprite();
	}

	// Token: 0x06002FD7 RID: 12247 RVA: 0x001C4F55 File Offset: 0x001C3355
	private void IronStepSFX()
	{
		if (this.TrainRunStep)
		{
			AudioManager.Play("train_engine_step");
			this.emitAudioFromObject.Add("train_engine_step");
		}
	}

	// Token: 0x06002FD8 RID: 12248 RVA: 0x001C4F7C File Offset: 0x001C337C
	private void UpdateDoorSprite()
	{
		this.doorSprites.DisableAll();
		this.doorSprites[this.doorState].enabled = true;
	}

	// Token: 0x06002FD9 RID: 12249 RVA: 0x001C4FA0 File Offset: 0x001C33A0
	private IEnumerator doorTimer_cr()
	{
		this.desiredDoorState = TrainLevelEngineBoss.DoorState.Open;
		float time = base.properties.CurrentState.engine.doorTime.GetFloatAt(this.health / base.properties.CurrentState.engine.health);
		while (this.doorState != TrainLevelEngineBoss.DoorState.Open)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, time);
		this.desiredDoorState = TrainLevelEngineBoss.DoorState.Closed;
		while (this.doorState != TrainLevelEngineBoss.DoorState.Closed)
		{
			yield return null;
		}
		base.StartCoroutine(this.tailTimer_cr());
		yield break;
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06002FDA RID: 12250 RVA: 0x001C4FBB File Offset: 0x001C33BB
	// (set) Token: 0x06002FDB RID: 12251 RVA: 0x001C4FC3 File Offset: 0x001C33C3
	private TrainLevelEngineBoss.TailState tailState
	{
		get
		{
			return this._tailState;
		}
		set
		{
			this.ChangeTail(value);
		}
	}

	// Token: 0x06002FDC RID: 12252 RVA: 0x001C4FCC File Offset: 0x001C33CC
	private void ChangeTail(TrainLevelEngineBoss.TailState state)
	{
		if (state == this.tailState)
		{
			return;
		}
		this.tailSwitch.tailEnabled = (state == TrainLevelEngineBoss.TailState.On);
		this._tailState = state;
		this.tailSprites.DisableAll();
		this.tailSprites[state].enabled = true;
	}

	// Token: 0x06002FDD RID: 12253 RVA: 0x001C5019 File Offset: 0x001C3419
	private void OnTailParried()
	{
		this.tailState = TrainLevelEngineBoss.TailState.Off;
		base.StartCoroutine(this.doorTimer_cr());
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x001C5030 File Offset: 0x001C3430
	private IEnumerator tailTimer_cr()
	{
		this.tailState = TrainLevelEngineBoss.TailState.Off;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.engine.tailDelay);
		this.tailState = TrainLevelEngineBoss.TailState.On;
		yield break;
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x001C504C File Offset: 0x001C344C
	private IEnumerator move_cr()
	{
		float max_x = base.properties.CurrentState.engine.maxDist;
		float min_x = base.properties.CurrentState.engine.minDist;
		float forwardTime = base.properties.CurrentState.engine.forwardTime;
		float backTime = base.properties.CurrentState.engine.backTime;
		yield return base.TweenLocalPositionX(base.transform.position.x, min_x, 3f, EaseUtils.EaseType.easeOutSine);
		AudioManager.FadeSFXVolume("train_engine_boss_run_start", 0f, 3f);
		AudioManager.PlayLoop("train_engine_boss_run_loop");
		this.emitAudioFromObject.Add("train_engine_boss_run_loop");
		AudioManager.PlayLoop("train_engine_boss_fire_idle");
		this.emitAudioFromObject.Add("train_engine_boss_fire_idle");
		for (;;)
		{
			yield return base.TweenLocalPositionX(base.transform.position.x, max_x, forwardTime, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionX(base.transform.position.x, min_x, backTime, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06002FE0 RID: 12256 RVA: 0x001C5067 File Offset: 0x001C3467
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.footDustPrefab = null;
		this.dropperPrefab = null;
		this.firePrefab = null;
	}

	// Token: 0x04003898 RID: 14488
	private const string HitParameterName = "Hit";

	// Token: 0x04003899 RID: 14489
	private const string StopHitAnimName = "StopHitAnim";

	// Token: 0x0400389A RID: 14490
	private const float StopHitAnimTime = 0.25f;

	// Token: 0x0400389B RID: 14491
	[SerializeField]
	private DamageReceiverChild heartDamageReceiver;

	// Token: 0x0400389C RID: 14492
	[SerializeField]
	private Transform footDustRoot;

	// Token: 0x0400389D RID: 14493
	[SerializeField]
	private Effect footDustPrefab;

	// Token: 0x0400389E RID: 14494
	private DamageReceiver damageReceiver;

	// Token: 0x0400389F RID: 14495
	private float health;

	// Token: 0x040038A0 RID: 14496
	private bool dead;

	// Token: 0x040038A1 RID: 14497
	private bool TrainRunStep;

	// Token: 0x040038A4 RID: 14500
	[Header("Dropper")]
	[SerializeField]
	private Transform dropperRoot;

	// Token: 0x040038A5 RID: 14501
	[SerializeField]
	private TrainLevelEngineBossDropperProjectile dropperPrefab;

	// Token: 0x040038A6 RID: 14502
	private IEnumerator attackCoroutine;

	// Token: 0x040038A7 RID: 14503
	private SpriteRenderer smokeRenderer;

	// Token: 0x040038A8 RID: 14504
	private const string FireAttackParameterName = "FireAttack";

	// Token: 0x040038A9 RID: 14505
	[Header("Fire")]
	[SerializeField]
	private Transform fireRoot;

	// Token: 0x040038AA RID: 14506
	[SerializeField]
	private TrainLevelEngineBossFireProjectile firePrefab;

	// Token: 0x040038AB RID: 14507
	private const string OpenDoorParameterName = "Open";

	// Token: 0x040038AC RID: 14508
	private const string CloseDoorParameterName = "Close";

	// Token: 0x040038AD RID: 14509
	[Header("Door")]
	[SerializeField]
	private TrainLevelEngineBoss.DoorSprites doorSprites;

	// Token: 0x040038AE RID: 14510
	[SerializeField]
	private GameObject door;

	// Token: 0x040038AF RID: 14511
	private TrainLevelEngineBoss.DoorState _ds = TrainLevelEngineBoss.DoorState.Closed;

	// Token: 0x040038B0 RID: 14512
	private TrainLevelEngineBoss.DoorState desiredDoorState = TrainLevelEngineBoss.DoorState.Closed;

	// Token: 0x040038B1 RID: 14513
	[Header("Tail")]
	[SerializeField]
	private TrainLevelEngineBoss.TailSprites tailSprites;

	// Token: 0x040038B2 RID: 14514
	[SerializeField]
	private Transform tailRoot;

	// Token: 0x040038B3 RID: 14515
	private TrainLevelEngineBoss.TailState _tailState = TrainLevelEngineBoss.TailState.Off;

	// Token: 0x040038B4 RID: 14516
	private TrainLevelEngineBossTail tailSwitch;

	// Token: 0x0200080E RID: 2062
	// (Invoke) Token: 0x06002FE2 RID: 12258
	public delegate void OnDamageTakenHandler(float damage);

	// Token: 0x0200080F RID: 2063
	public enum DoorState
	{
		// Token: 0x040038B6 RID: 14518
		Open,
		// Token: 0x040038B7 RID: 14519
		Closed,
		// Token: 0x040038B8 RID: 14520
		Opening,
		// Token: 0x040038B9 RID: 14521
		Closing
	}

	// Token: 0x02000810 RID: 2064
	[Serializable]
	public class DoorSprites
	{
		// Token: 0x1700041B RID: 1051
		public SpriteRenderer this[TrainLevelEngineBoss.DoorState state]
		{
			get
			{
				switch (state)
				{
				default:
					return this.open;
				case TrainLevelEngineBoss.DoorState.Closed:
					return this.closed;
				case TrainLevelEngineBoss.DoorState.Opening:
					return this.opening;
				case TrainLevelEngineBoss.DoorState.Closing:
					return this.closing;
				}
			}
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x001C50C4 File Offset: 0x001C34C4
		public void DisableAll()
		{
			this.open.enabled = false;
			this.closed.enabled = false;
			this.opening.enabled = false;
			this.closing.enabled = false;
		}

		// Token: 0x040038BA RID: 14522
		public SpriteRenderer open;

		// Token: 0x040038BB RID: 14523
		public SpriteRenderer closed;

		// Token: 0x040038BC RID: 14524
		public SpriteRenderer opening;

		// Token: 0x040038BD RID: 14525
		public SpriteRenderer closing;
	}

	// Token: 0x02000811 RID: 2065
	public enum TailState
	{
		// Token: 0x040038BF RID: 14527
		On,
		// Token: 0x040038C0 RID: 14528
		Off
	}

	// Token: 0x02000812 RID: 2066
	[Serializable]
	public class TailSprites
	{
		// Token: 0x1700041C RID: 1052
		public SpriteRenderer this[TrainLevelEngineBoss.TailState state]
		{
			get
			{
				if (state == TrainLevelEngineBoss.TailState.On || state != TrainLevelEngineBoss.TailState.Off)
				{
					return this.on;
				}
				return this.off;
			}
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x001C511F File Offset: 0x001C351F
		public void DisableAll()
		{
			this.on.enabled = false;
			this.off.enabled = false;
		}

		// Token: 0x040038C1 RID: 14529
		public SpriteRenderer on;

		// Token: 0x040038C2 RID: 14530
		public SpriteRenderer off;
	}
}
