using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006AB RID: 1707
public class FrogsLevelMorphed : LevelProperties.Frogs.Entity
{
	// Token: 0x0600242C RID: 9260 RVA: 0x00153780 File Offset: 0x00151B80
	protected override void Awake()
	{
		base.Awake();
		FrogsLevelMorphed.Current = this;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
		this.slots.Init(this);
		this.handle = FrogsLevelMorphedSwitch.Create(this);
		this.handle.enabled = false;
		this.handle.OnActivate += this.OnHandleActivated;
		this.slotsParent.SetActive(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x00153828 File Offset: 0x00151C28
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (FrogsLevelMorphed.Current == this)
		{
			FrogsLevelMorphed.Current = null;
		}
		this.coin = null;
		this.snakeBullet = null;
		this.oniBullet = null;
		this.bisonBullet = null;
		this.tigerBullet = null;
		this.dustEffect = null;
		this.slots.OnDestroy();
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x00153886 File Offset: 0x00151C86
	private void Start()
	{
		this.damageReceiver.enabled = false;
	}

	// Token: 0x0600242F RID: 9263 RVA: 0x00153894 File Offset: 0x00151C94
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06002430 RID: 9264 RVA: 0x001538A1 File Offset: 0x00151CA1
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		AudioManager.Play("level_frogs_short_clap_shock");
		this.emitAudioFromObject.Add("level_frogs_short_clap_shock");
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002431 RID: 9265 RVA: 0x001538D9 File Offset: 0x00151CD9
	public override void LevelInit(LevelProperties.Frogs properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06002432 RID: 9266 RVA: 0x001538E2 File Offset: 0x00151CE2
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002433 RID: 9267 RVA: 0x001538F8 File Offset: 0x00151CF8
	public void Enable(bool demonTriggered)
	{
		this.demonTriggered = demonTriggered;
		base.gameObject.SetActive(true);
		this.dustEffect.gameObject.SetActive(true);
		base.properties.OnBossDeath += this.OnBossDeath;
		base.GetComponent<LevelBossDeathExploder>().enabled = true;
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x00153959 File Offset: 0x00151D59
	private void OnBossDeath()
	{
		AudioManager.PlayLoop("level_frogs_morphed_death_loop");
		this.emitAudioFromObject.Add("level_frogs_morphed_death_loop");
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		this.slotsParent.SetActive(false);
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x00153998 File Offset: 0x00151D98
	private void ShootCoin()
	{
		AudioManager.Play("level_frogs_morphed_mouth");
		this.emitAudioFromObject.Add("level_frogs_morphed_mouth");
		this.coinRoot.LookAt2D(PlayerManager.GetNext().center);
		FrogsLevelMorphedCoin frogsLevelMorphedCoin = this.coin.CreateCoin(this.coinRoot.position, this.coinSpeed, this.coinRoot.eulerAngles.z);
		frogsLevelMorphedCoin.transform.SetPosition(null, null, new float?(-600f));
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x00153A30 File Offset: 0x00151E30
	private void StartShooting()
	{
		this.EndShooting();
		this.shootingCoroutine = this.shootingLoop_cr();
		base.StartCoroutine(this.shootingCoroutine);
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x00153A51 File Offset: 0x00151E51
	private void EndShooting()
	{
		if (this.shootingCoroutine != null)
		{
			base.StopCoroutine(this.shootingCoroutine);
		}
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x00153A6A File Offset: 0x00151E6A
	private void OnHandleActivated()
	{
		this.handleActivated = true;
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x00153A74 File Offset: 0x00151E74
	private IEnumerator loop_cr()
	{
		if (this.demonTriggered)
		{
			this.mainIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.demon.demonString.Length);
			this.index = UnityEngine.Random.Range(0, base.properties.CurrentState.demon.demonString[this.mainIndex].Split(new char[]
			{
				','
			}).Length);
		}
		AudioManager.Play("level_frogs_morphed_open");
		this.emitAudioFromObject.Add("level_frogs_morphed_open");
		this.slotsParent.SetActive(true);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.Play("Open");
		AudioManager.Play("level_frogs_morphed_open");
		this.emitAudioFromObject.Add("level_frogs_morphed_open");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			LevelProperties.Frogs.Morph p = base.properties.CurrentState.morph;
			this.StartShooting();
			yield return CupheadTime.WaitForSeconds(this, p.armDownDelay);
			yield return base.StartCoroutine(this.waitForActivate_cr());
			this.EndShooting();
			base.animator.SetTrigger("OnActivated");
			yield return base.StartCoroutine(this.pattern_cr(p));
			this.slotsParent.SetActive(true);
			base.animator.SetTrigger("OnAttackEnd");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_End", false, true);
		}
		yield break;
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x00153A90 File Offset: 0x00151E90
	private IEnumerator waitForActivate_cr()
	{
		this.handleActivated = false;
		this.handle.enabled = true;
		base.animator.SetTrigger("OnArmDown");
		AudioManager.Play("level_frogs_morphed_arm_down");
		this.emitAudioFromObject.Add("level_frogs_morphed_arm_down");
		while (!this.handleActivated)
		{
			yield return null;
		}
		this.handle.enabled = false;
		yield break;
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x00153AAC File Offset: 0x00151EAC
	private IEnumerator shootingLoop_cr()
	{
		LevelProperties.Frogs.Morph p = base.properties.CurrentState.morph;
		float time = p.coinMinMaxTime;
		float t = 0f;
		float val = 0f;
		float coinDelay = 0f;
		for (;;)
		{
			float delay = p.coinDelay.GetFloatAt(val);
			this.coinSpeed = p.coinSpeed.GetFloatAt(val);
			if (coinDelay >= delay)
			{
				base.animator.SetTrigger("OnShoot");
				coinDelay = 0f;
			}
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			coinDelay += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600243C RID: 9276 RVA: 0x00153AC8 File Offset: 0x00151EC8
	private IEnumerator pattern_cr(LevelProperties.Frogs.Morph p)
	{
		Slots.Mode mode = Slots.Mode.Snake;
		if (!this.demonTriggered)
		{
			int num = UnityEngine.Random.Range(0, 3);
			mode = (Slots.Mode)num;
		}
		else
		{
			mode = Slots.Mode.Oni;
			yield return null;
		}
		this.slots.Spin();
		yield return CupheadTime.WaitForSeconds(this, 3f * p.slotSelectionDurationPercentage);
		this.slots.Stop(mode);
		yield return CupheadTime.WaitForSeconds(this, 1f * p.slotSelectionDurationPercentage);
		this.slots.StartFlash();
		yield return CupheadTime.WaitForSeconds(this, 0.8f * p.slotSelectionDurationPercentage);
		this.slots.StartFlash();
		yield return CupheadTime.WaitForSeconds(this, 0.8f * p.slotSelectionDurationPercentage);
		this.slots.StartFlash();
		yield return CupheadTime.WaitForSeconds(this, 0.8f * p.slotSelectionDurationPercentage);
		this.damageReceiver.enabled = true;
		base.animator.SetTrigger("OnAttack");
		AudioManager.Play("level_frogs_morphed_attack");
		this.emitAudioFromObject.Add("level_frogs_morphed_attack");
		yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
		this.slotsParent.SetActive(false);
		AudioManager.PlayLoop("level_frogs_platform_loop");
		this.emitAudioFromObject.Add("level_frogs_platform_loop");
		switch (mode)
		{
		case Slots.Mode.Snake:
			yield return base.StartCoroutine(this.snake_cr(p));
			break;
		case Slots.Mode.Tiger:
			yield return base.StartCoroutine(this.tiger_cr(p));
			break;
		case Slots.Mode.Bison:
			yield return base.StartCoroutine(this.bison_cr(p));
			break;
		case Slots.Mode.Oni:
			yield return base.StartCoroutine(this.oni_cr());
			break;
		}
		AudioManager.Stop("level_frogs_platform_loop");
		this.damageReceiver.enabled = false;
		yield break;
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x00153AEA File Offset: 0x00151EEA
	private void ShootSnake(float speed)
	{
		this.snakeBullet.Create(this.slotBulletRoot.position, speed);
	}

	// Token: 0x0600243E RID: 9278 RVA: 0x00153B09 File Offset: 0x00151F09
	private void ShootBison(float speed, FrogsLevelBisonBullet.Direction dir, float bigX, float smallX)
	{
		this.bisonBullet.Create(this.slotBulletRoot.position, speed, dir, bigX, smallX);
	}

	// Token: 0x0600243F RID: 9279 RVA: 0x00153B2C File Offset: 0x00151F2C
	private void ShootTiger(float speed)
	{
		this.tigerBullet.Create(this.slotBulletRoot.position, speed);
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x00153B4B File Offset: 0x00151F4B
	private void ShootOni(float speed)
	{
		this.oniBullet.Create(this.slotBulletRoot.position, speed, base.properties.CurrentState.demon);
	}

	// Token: 0x06002441 RID: 9281 RVA: 0x00153B7C File Offset: 0x00151F7C
	private IEnumerator snake_cr(LevelProperties.Frogs.Morph p)
	{
		float t = 0f;
		float time = p.snakeDuration;
		float val = 0f;
		float bulletDelay = 1000f;
		float bulletSpeed = 0f;
		float delay = 0f;
		while (t < time)
		{
			if (bulletDelay >= delay)
			{
				bulletSpeed = p.snakeSpeed.GetFloatAt(val);
				this.ShootSnake(bulletSpeed);
				bulletDelay = 0f;
			}
			delay = p.snakeDelay.GetFloatAt(val);
			bulletDelay += CupheadTime.Delta;
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002442 RID: 9282 RVA: 0x00153BA0 File Offset: 0x00151FA0
	private IEnumerator bison_cr(LevelProperties.Frogs.Morph p)
	{
		float t = 0f;
		float time = (float)p.bisonDuration;
		float val = 0f;
		float bulletDelay = 10000f;
		float bulletSpeed = 0f;
		float delay = 0f;
		int sameDirCount = 0;
		FrogsLevelBisonBullet.Direction lastDir = FrogsLevelBisonBullet.Direction.Down;
		FrogsLevelBisonBullet.Direction dir = FrogsLevelBisonBullet.Direction.Up;
		while (t < time)
		{
			if (bulletDelay >= delay)
			{
				bulletSpeed = p.bisonSpeed.GetFloatAt(val);
				this.ShootBison(bulletSpeed, dir, p.bisonBigX, p.bisonSmallX);
				bulletDelay = 0f;
				lastDir = dir;
				dir = (FrogsLevelBisonBullet.Direction)UnityEngine.Random.Range(0, 2);
				if (lastDir == dir)
				{
					sameDirCount++;
				}
				else
				{
					sameDirCount = 0;
				}
				if (sameDirCount >= 3)
				{
					if (dir == FrogsLevelBisonBullet.Direction.Up)
					{
						dir = FrogsLevelBisonBullet.Direction.Down;
					}
					else
					{
						dir = FrogsLevelBisonBullet.Direction.Up;
					}
					sameDirCount = 0;
				}
			}
			delay = p.bisonDelay.GetFloatAt(val);
			bulletDelay += CupheadTime.Delta;
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x00153BC4 File Offset: 0x00151FC4
	private IEnumerator tiger_cr(LevelProperties.Frogs.Morph p)
	{
		float t = 0f;
		float time = p.tigerDuration;
		float val = 0f;
		float bulletDelay = 1000f;
		float bulletSpeed = 0f;
		float delay = 0f;
		while (t < time)
		{
			if (bulletDelay >= delay)
			{
				bulletSpeed = p.tigerSpeed;
				this.ShootTiger(bulletSpeed);
				bulletDelay = 0f;
			}
			delay = p.tigerDelay.GetFloatAt(val);
			bulletDelay += CupheadTime.Delta;
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x00153BE8 File Offset: 0x00151FE8
	private IEnumerator oni_cr()
	{
		LevelProperties.Frogs.Demon p = base.properties.CurrentState.demon;
		float bulletSpeed = 0f;
		float bulletDelay = 1000f;
		float delay = 0f;
		float val = 0f;
		float time = p.demonMaxTime;
		float t = 0f;
		for (;;)
		{
			FrogsLevelBisonBullet.Direction dir = (FrogsLevelBisonBullet.Direction)UnityEngine.Random.Range(0, 2);
			string[] demonPattern = p.demonString[this.mainIndex].Split(new char[]
			{
				','
			});
			if (bulletDelay >= delay)
			{
				demonPattern = p.demonString[this.mainIndex].Split(new char[]
				{
					','
				});
				bulletSpeed = p.demonSpeed.GetFloatAt(val);
				char c = demonPattern[this.index][0];
				if (c != 'S')
				{
					if (c != 'T')
					{
						if (c != 'B')
						{
							if (c == 'O')
							{
								this.ShootOni(bulletSpeed);
							}
						}
						else
						{
							dir = (FrogsLevelBisonBullet.Direction)UnityEngine.Random.Range(0, 2);
							this.ShootBison(bulletSpeed, dir, base.properties.CurrentState.morph.bisonBigX, base.properties.CurrentState.morph.bisonSmallX);
							if (dir == FrogsLevelBisonBullet.Direction.Up)
							{
								dir = FrogsLevelBisonBullet.Direction.Down;
							}
							else
							{
								dir = FrogsLevelBisonBullet.Direction.Up;
							}
						}
					}
					else
					{
						this.ShootTiger(bulletSpeed);
					}
				}
				else
				{
					this.ShootSnake(bulletSpeed);
				}
				if (this.index < demonPattern.Length - 1)
				{
					this.index++;
				}
				else
				{
					this.mainIndex = (this.mainIndex + 1) % p.demonString.Length;
					this.index = 0;
				}
				bulletDelay = 0f;
			}
			delay = p.demonDelay.GetFloatAt(val);
			bulletDelay += CupheadTime.Delta;
			if (val < 1f)
			{
				val = t / time;
				t += CupheadTime.Delta;
			}
			else
			{
				val = 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002CED RID: 11501
	public static FrogsLevelMorphed Current;

	// Token: 0x04002CEE RID: 11502
	[SerializeField]
	private FrogsLevelMorphedCoin coin;

	// Token: 0x04002CEF RID: 11503
	[SerializeField]
	private Transform coinRoot;

	// Token: 0x04002CF0 RID: 11504
	[Space(10f)]
	public Transform switchRoot;

	// Token: 0x04002CF1 RID: 11505
	[Space(10f)]
	[SerializeField]
	private GameObject slotsParent;

	// Token: 0x04002CF2 RID: 11506
	[SerializeField]
	private Slots slots;

	// Token: 0x04002CF3 RID: 11507
	[Space(10f)]
	[SerializeField]
	private FrogsLevelSnakeBullet snakeBullet;

	// Token: 0x04002CF4 RID: 11508
	[SerializeField]
	private FrogsLevelBisonBullet bisonBullet;

	// Token: 0x04002CF5 RID: 11509
	[SerializeField]
	private FrogsLevelTigerBullet tigerBullet;

	// Token: 0x04002CF6 RID: 11510
	[SerializeField]
	private FrogsLevelOniBullet oniBullet;

	// Token: 0x04002CF7 RID: 11511
	[SerializeField]
	private Transform slotBulletRoot;

	// Token: 0x04002CF8 RID: 11512
	[Space(10f)]
	[SerializeField]
	private Effect dustEffect;

	// Token: 0x04002CF9 RID: 11513
	private DamageReceiver damageReceiver;

	// Token: 0x04002CFA RID: 11514
	private DamageDealer damageDealer;

	// Token: 0x04002CFB RID: 11515
	private FrogsLevelMorphedSwitch handle;

	// Token: 0x04002CFC RID: 11516
	private bool demonTriggered;

	// Token: 0x04002CFD RID: 11517
	private int mainIndex;

	// Token: 0x04002CFE RID: 11518
	private int index;

	// Token: 0x04002CFF RID: 11519
	private bool handleActivated;

	// Token: 0x04002D00 RID: 11520
	private IEnumerator shootingCoroutine;

	// Token: 0x04002D01 RID: 11521
	private float coinSpeed;
}
