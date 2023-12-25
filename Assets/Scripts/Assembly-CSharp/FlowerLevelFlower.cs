using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000608 RID: 1544
public class FlowerLevelFlower : LevelProperties.Flower.Entity
{
	// Token: 0x14000042 RID: 66
	// (add) Token: 0x06001ED0 RID: 7888 RVA: 0x0011B540 File Offset: 0x00119940
	// (remove) Token: 0x06001ED1 RID: 7889 RVA: 0x0011B578 File Offset: 0x00119978
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x06001ED2 RID: 7890 RVA: 0x0011B5B0 File Offset: 0x001199B0
	// (remove) Token: 0x06001ED3 RID: 7891 RVA: 0x0011B5E8 File Offset: 0x001199E8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnStateChanged;

	// Token: 0x06001ED4 RID: 7892 RVA: 0x0011B61E File Offset: 0x00119A1E
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x0011B631 File Offset: 0x00119A31
	public void AdditionDamageTaken(DamageDealer.DamageInfo info)
	{
		this.OnDamageTaken(info);
	}

	// Token: 0x06001ED6 RID: 7894 RVA: 0x0011B63A File Offset: 0x00119A3A
	public void PhaseTwoTrigger()
	{
		base.animator.SetTrigger("PhaseTwoTransition");
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x0011B64C File Offset: 0x00119A4C
	private void Die()
	{
		this.isDead = true;
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x0011B662 File Offset: 0x00119A62
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.boomerangPrefab = null;
		this.bulletSeedPrefab = null;
		this.cloudBombPrefab = null;
		this.enemySeedPrefab = null;
		this.pollenProjectile = null;
		this.gattlingFX = null;
		this.vineHandPrefab = null;
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x0011B69B File Offset: 0x00119A9B
	private void SpawnMainVine()
	{
		if (this.OnStateChanged != null)
		{
			this.OnStateChanged();
		}
		this.mainVine.SetActive(true);
		base.animator.SetTrigger("SpawnMainVine");
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x0011B6CF File Offset: 0x00119ACF
	private void MainVineSpawned()
	{
		base.StartCoroutine(this.vineHands_cr());
		this.projectileSpawned = false;
		base.StartCoroutine(this.pollenAttack_cr());
		this.attackCount = 1;
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x0011B6FC File Offset: 0x00119AFC
	private IEnumerator die_cr()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.Play("Phase One Death");
		}
		else
		{
			base.animator.Play("Phase Two Death");
		}
		yield return null;
		base.animator.enabled = false;
		base.animator.enabled = true;
		base.properties.WinInstantly();
		yield break;
	}

	// Token: 0x06001EDC RID: 7900 RVA: 0x0011B718 File Offset: 0x00119B18
	public override void LevelInit(LevelProperties.Flower properties)
	{
		properties.OnBossDeath += this.Phase2DeathAudio;
		properties.OnBossDeath += this.Die;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.LevelInit(properties);
		this.attackCount = 0;
		this.miniFlowerSpawned = false;
		Level.Current.OnIntroEvent += this.OnIntro;
		int num = UnityEngine.Random.Range(0, properties.CurrentState.laser.attackType.Split(new char[]
		{
			','
		}).Length);
		this.currentLaserAttack = num;
		this.currentGattlingGunAttackPattern = new List<string>();
		this.currentGattlingGunAttackString = UnityEngine.Random.Range(0, properties.CurrentState.gattlingGun.seedSpawnString.Length);
		string[] array = properties.CurrentState.vineHands.handAttackString.Split(new char[]
		{
			','
		});
		this.currentVineHandsAttack = UnityEngine.Random.Range(0, array.Length);
		this.pollenAttackCount = UnityEngine.Random.Range(0, properties.CurrentState.pollenSpit.pollenAttackCount.Split(new char[]
		{
			','
		}).Length);
		this.currentPollenType = UnityEngine.Random.Range(0, properties.CurrentState.pollenSpit.pollenType.Split(new char[]
		{
			','
		}).Length);
		base.StartCoroutine(this.find_s_cr());
	}

	// Token: 0x06001EDD RID: 7901 RVA: 0x0011B894 File Offset: 0x00119C94
	private IEnumerator find_s_cr()
	{
		while (base.properties.CurrentState.podHands.attacktype.Split(new char[]
		{
			','
		})[this.currentPodHandsAttack][0] != 'S')
		{
			this.currentPodHandsAttack = UnityEngine.Random.Range(0, base.properties.CurrentState.podHands.attacktype.Split(new char[]
			{
				','
			}).Length);
			yield return null;
		}
		this.podHandsAttackCountTarget = UnityEngine.Random.Range(0, base.properties.CurrentState.podHands.attackAmount.Split(new char[]
		{
			','
		}).Length);
		yield return null;
		yield break;
	}

	// Token: 0x06001EDE RID: 7902 RVA: 0x0011B8AF File Offset: 0x00119CAF
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x0011B8C7 File Offset: 0x00119CC7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x0011B8E8 File Offset: 0x00119CE8
	public void StartLaser(Action callback)
	{
		this.attackCallback = callback;
		this.attackType = base.properties.CurrentState.laser.attackType.Split(new char[]
		{
			','
		})[this.currentLaserAttack][0];
		this.attackCharge = base.properties.CurrentState.laser.anticHold;
		this.OnLaserStarted();
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x0011B958 File Offset: 0x00119D58
	public void OnLaserStarted()
	{
		if (this.attackType.Equals('T'))
		{
			this.topLaserAttack = true;
		}
		else
		{
			this.topLaserAttack = false;
		}
		if (this.topLaserAttack)
		{
			base.animator.SetBool("TopLaser", true);
		}
		else
		{
			base.animator.SetBool("BottomLaser", true);
		}
		base.StartCoroutine(this.laserCharge_cr());
	}

	// Token: 0x06001EE2 RID: 7906 RVA: 0x0011B9CC File Offset: 0x00119DCC
	private IEnumerator laserCharge_cr()
	{
		if (this.topLaserAttack)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "TopLaserAttackStart", true, true);
		}
		else
		{
			yield return base.animator.WaitForAnimationToEnd(this, "BottomLaserAttackStart", true, true);
		}
		yield return CupheadTime.WaitForSeconds(this, this.attackCharge);
		base.animator.SetTrigger("OnAttackChargeComplete");
		yield break;
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x0011B9E7 File Offset: 0x00119DE7
	private void OnHoldComplete()
	{
		base.StartCoroutine(this.onLaser_cr());
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x0011B9F8 File Offset: 0x00119DF8
	private IEnumerator onLaser_cr()
	{
		this.attackCharge = base.properties.CurrentState.laser.attackHold;
		yield return CupheadTime.WaitForSeconds(this, this.attackCharge);
		if (this.topLaserAttack)
		{
			base.animator.SetBool("TopLaser", false);
		}
		else
		{
			base.animator.SetBool("BottomLaser", false);
		}
		if (this.topLaserAttack)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "TopLaserAttackEnd", true, true);
		}
		else
		{
			yield return base.animator.WaitForAnimationToEnd(this, "BottomLaserAttackEnd", true, true);
		}
		yield break;
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x0011BA14 File Offset: 0x00119E14
	public void OnLaserComplete()
	{
		this.currentLaserAttack++;
		if (this.currentLaserAttack >= base.properties.CurrentState.laser.attackType.Split(new char[]
		{
			','
		}).Length)
		{
			this.currentLaserAttack = 0;
		}
		this.topLaserAttack = false;
		if (this.attackCallback != null)
		{
			this.attackCallback();
		}
		this.attackCallback = null;
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x0011BA8C File Offset: 0x00119E8C
	public void StartPotHands(Action callback)
	{
		this.attackCount = 0;
		if (this.podHandsAttackCountTarget >= base.properties.CurrentState.podHands.attackAmount.Split(new char[]
		{
			','
		}).Length)
		{
			this.podHandsAttackCountTarget = 0;
		}
		this.attackCountTarget = Parser.IntParse(base.properties.CurrentState.podHands.attackAmount.Split(new char[]
		{
			','
		})[this.podHandsAttackCountTarget].ToString());
		this.attackType = base.properties.CurrentState.podHands.attacktype.Split(new char[]
		{
			','
		})[this.currentPodHandsAttack][0];
		this.attackCallback = callback;
		this.attackCharge = base.properties.CurrentState.podHands.attackHold;
		this.OnPotHandsStarted();
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x0011BB75 File Offset: 0x00119F75
	public void OnPotHandsStarted()
	{
		base.animator.SetBool("PotHandsAttack", true);
		base.StartCoroutine(this.potHandsHold_cr());
		this.attackCount++;
	}

	// Token: 0x06001EE8 RID: 7912 RVA: 0x0011BBA4 File Offset: 0x00119FA4
	private IEnumerator potHandsHold_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.attackCharge);
		base.animator.SetTrigger("OnAttackChargeComplete");
		this.OpenPotHands();
		yield break;
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x0011BBBF File Offset: 0x00119FBF
	private void OpenPotHands()
	{
		this.attackCharge = base.properties.CurrentState.podHands.attackDelay;
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x0011BBDC File Offset: 0x00119FDC
	public void OnPotHandsComplete()
	{
		this.projectileSpawned = false;
		if (this.attackCount >= this.attackCountTarget)
		{
			this.attackCount = 0;
			this.podHandsAttackCountTarget++;
			if (this.podHandsAttackCountTarget >= base.properties.CurrentState.podHands.attackAmount.Split(new char[]
			{
				','
			}).Length)
			{
				this.podHandsAttackCountTarget = 0;
			}
			base.animator.SetTrigger("OnAttackComplete");
			if (this.attackCallback != null)
			{
				this.attackCallback();
				this.attackCallback = null;
			}
		}
		base.animator.SetBool("PotHandsAttack", false);
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x0011BC8D File Offset: 0x0011A08D
	public void StartGattlingGun(Action callback)
	{
		this.attackCallback = callback;
		this.attackCharge = base.properties.CurrentState.gattlingGun.loopDuration;
		base.animator.SetBool("GattlingGunAttack", true);
		this.attackType = 'G';
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x0011BCCC File Offset: 0x0011A0CC
	private IEnumerator startGattlingFX_cr()
	{
		string animAttrib = "GattlingGunAttack";
		int target = Animator.StringToHash(base.animator.GetLayerName(0) + ".GattlingGunStart");
		if (target == base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "GattlingGunStart", true, true);
		}
		while (base.animator.GetBool(animAttrib))
		{
			GameObject fxObject = UnityEngine.Object.Instantiate<GameObject>(this.gattlingFX, this.topProjectileSpawnPoint.position, Quaternion.identity);
			Animator fx = fxObject.GetComponent<Animator>();
			yield return base.StartCoroutine(this.killGattlingFX_cr(fx));
		}
		yield break;
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x0011BCE8 File Offset: 0x0011A0E8
	private IEnumerator killGattlingFX_cr(Animator fx)
	{
		yield return fx.WaitForAnimationToEnd(this, true);
		UnityEngine.Object.Destroy(fx.gameObject);
		yield break;
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x0011BD0A File Offset: 0x0011A10A
	public void OnGattlingGunEnded()
	{
		base.animator.SetBool("GattlingGunAttack", false);
		this.OnGattlingGunComplete();
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x0011BD23 File Offset: 0x0011A123
	public void OnGattlingGunComplete()
	{
		if (this.attackCallback != null)
		{
			this.attackCallback();
		}
		this.attackCallback = null;
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x0011BD44 File Offset: 0x0011A144
	private void AddAttackTypes(string[] s)
	{
		for (int i = 0; i < s.Length; i++)
		{
			this.currentGattlingGunAttackPattern.Add(s[i]);
		}
	}

	// Token: 0x06001EF1 RID: 7921 RVA: 0x0011BD73 File Offset: 0x0011A173
	private void StartVineHandsAttack()
	{
		base.StartCoroutine(this.vineHands_cr());
	}

	// Token: 0x06001EF2 RID: 7922 RVA: 0x0011BD84 File Offset: 0x0011A184
	private IEnumerator vineHands_cr()
	{
		while (!this.isDead)
		{
			string[] attackPositions = base.properties.CurrentState.vineHands.handAttackString.Split(new char[]
			{
				','
			});
			string[] currentWave = attackPositions[this.currentVineHandsAttack].Split(new char[]
			{
				'-'
			});
			if (attackPositions[this.currentVineHandsAttack][0] != 'D')
			{
				if (currentWave.Length > 1)
				{
					this.currentVineHandsAttack += 2;
					this.vineHandPrefab.GetComponent<FlowerLevelFlowerVineHand>().OnVineHandSpawn(base.properties.CurrentState.vineHands.firstPositionHold, base.properties.CurrentState.vineHands.secondPositionHold, Parser.IntParse(currentWave[0]), Parser.IntParse(currentWave[1]));
				}
				else
				{
					this.currentVineHandsAttack++;
					this.vineHandPrefab.GetComponent<FlowerLevelFlowerVineHand>().OnVineHandSpawn(base.properties.CurrentState.vineHands.firstPositionHold, base.properties.CurrentState.vineHands.secondPositionHold, Parser.IntParse(currentWave[0]), 0);
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(attackPositions[this.currentVineHandsAttack].Substring(1)));
			}
			if (this.currentVineHandsAttack >= attackPositions.Length)
			{
				this.currentVineHandsAttack = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.vineHands.attackDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x0011BDA0 File Offset: 0x0011A1A0
	private IEnumerator pollenAttack_cr()
	{
		while (!this.isDead)
		{
			if (!this.projectileSpawned)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.pollenSpit.pollenCommaDelay);
				string delay = base.properties.CurrentState.pollenSpit.pollenAttackCount.Split(new char[]
				{
					','
				})[this.pollenAttackCount].ToString();
				if (delay[0].Equals('D'))
				{
					yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(delay.Substring(1).ToString()));
				}
				else
				{
					this.attackCountTarget = Parser.IntParse(delay);
				}
				this.pollenAttackCount++;
				if (this.pollenAttackCount >= base.properties.CurrentState.pollenSpit.pollenAttackCount.Split(new char[]
				{
					','
				}).Length)
				{
					this.pollenAttackCount = 0;
				}
				this.projectileSpawned = true;
				base.animator.SetBool("OnPollenAttack", true);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.pollenSpit.consecutiveAttackHold);
				base.animator.SetTrigger("OnAttackChargeComplete");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x0011BDBC File Offset: 0x0011A1BC
	private void launchPollen()
	{
		string text = base.properties.CurrentState.pollenSpit.pollenType.Split(new char[]
		{
			','
		})[this.currentPollenType];
		int type;
		if (text[0].Equals('R'))
		{
			type = 0;
		}
		else
		{
			type = 1;
		}
		this.currentPollenType++;
		if (this.currentPollenType >= base.properties.CurrentState.pollenSpit.pollenType.Split(new char[]
		{
			','
		}).Length)
		{
			this.currentPollenType = 0;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pollenProjectile, this.topProjectileSpawnPoint.position, Quaternion.identity);
		this.currentPollenShot = gameObject.GetComponent<FlowerLevelPollenProjectile>();
		this.currentPollenShot.InitPollen((float)base.properties.CurrentState.pollenSpit.pollenSpeed, base.properties.CurrentState.pollenSpit.pollenUpDownStrength, type, this.topProjectileSpawnPoint);
		AudioManager.Play("flower_phase2_spit_projectile");
		this.attackCount++;
		if (this.attackCount > this.attackCountTarget)
		{
			base.animator.SetBool("OnPollenAttack", false);
			this.attackCount = 1;
			this.projectileSpawned = false;
		}
		else
		{
			this.projectileSpawned = true;
		}
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x0011BF17 File Offset: 0x0011A317
	private void PollenShotEnd()
	{
		this.currentPollenShot.StartMoving();
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x0011BF24 File Offset: 0x0011A324
	private void OnIntro()
	{
		base.animator.SetTrigger("OnIntroEnded");
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x0011BF38 File Offset: 0x0011A338
	private void SpawnProjectile()
	{
		char c = this.attackType;
		if (c != 'R')
		{
			if (c != 'S')
			{
				if (c != 'B')
				{
					if (c == 'G')
					{
						base.StartCoroutine(this.spawnGattlingGunSeeds_cr());
					}
				}
				else
				{
					this.SpawnBoomerang();
				}
			}
			else
			{
				this.SpawnBullets();
			}
		}
		else
		{
			this.SpawnCloudShot();
		}
		this.currentPodHandsAttack++;
		if (this.currentPodHandsAttack >= base.properties.CurrentState.podHands.attacktype.Split(new char[]
		{
			','
		}).Length)
		{
			this.currentPodHandsAttack = 0;
		}
		this.attackType = base.properties.CurrentState.podHands.attacktype.Split(new char[]
		{
			','
		})[this.currentPodHandsAttack][0];
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x0011C028 File Offset: 0x0011A428
	private IEnumerator spawnGattlingGunSeeds_cr()
	{
		base.StartCoroutine(this.startGattlingFX_cr());
		this.currentGattlingGunAttackPattern.Clear();
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gattlingGun.initialSeedDelay);
		string[] projectileAttributes = base.properties.CurrentState.gattlingGun.seedSpawnString[this.currentGattlingGunAttackString].Split(new char[]
		{
			','
		});
		float delayNextProjectileWave = base.properties.CurrentState.gattlingGun.fallingSeedDelay;
		for (int j = 0; j < projectileAttributes.Length; j++)
		{
			string[] array = projectileAttributes[j].Split(new char[]
			{
				'-'
			});
			if (array.Length > 1)
			{
				this.AddAttackTypes(array);
			}
			else
			{
				this.currentGattlingGunAttackPattern.Add(projectileAttributes[j]);
			}
			this.currentGattlingGunAttackPattern.Add("D" + base.properties.CurrentState.gattlingGun.fallingSeedDelay.ToStringInvariant());
		}
		for (int i = 0; i < this.currentGattlingGunAttackPattern.Count; i++)
		{
			char t = this.currentGattlingGunAttackPattern[i][0];
			if (t == 'D')
			{
				yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(this.currentGattlingGunAttackPattern[i].Substring(1)));
			}
			else
			{
				if (this.miniFlowerSpawned)
				{
					if (t != 'C')
					{
						this.SpawnEnemySeed(Parser.IntParse(this.currentGattlingGunAttackPattern[i].Substring(1)), t, true);
					}
					else
					{
						this.SpawnEnemySeed(Parser.IntParse(this.currentGattlingGunAttackPattern[i].Substring(1)), t, false);
					}
				}
				else
				{
					this.SpawnEnemySeed(Parser.IntParse(this.currentGattlingGunAttackPattern[i].Substring(1)), t, true);
				}
				if (t == 'C')
				{
					this.miniFlowerSpawned = true;
				}
			}
		}
		yield return CupheadTime.WaitForSeconds(this, delayNextProjectileWave);
		this.currentGattlingGunAttackString++;
		if (this.currentGattlingGunAttackString >= base.properties.CurrentState.gattlingGun.seedSpawnString.Length)
		{
			this.currentGattlingGunAttackString = 0;
		}
		AudioManager.Stop("flower_gattling_gun_loop");
		this.OnGattlingGunEnded();
		yield break;
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x0011C043 File Offset: 0x0011A443
	public void OnMiniFlowerDeath()
	{
		this.miniFlowerSpawned = false;
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x0011C04C File Offset: 0x0011A44C
	private void SpawnEnemySeed(int xPos, char t, bool a = true)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.enemySeedPrefab);
		gameObject.transform.position = new Vector3((float)(-600 + xPos), (float)Level.Current.Height, 0f);
		gameObject.GetComponent<FlowerLevelEnemySeed>().OnSeedSpawn(base.properties, this, t, a);
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x0011C0A4 File Offset: 0x0011A4A4
	private void SpawnBoomerang()
	{
		BasicProjectile proj = this.boomerangPrefab.GetComponent<FlowerLevelBoomerang>().Create(this.bottomProjectileSpawnPoint.position + (this.topProjectileSpawnPoint.position - this.bottomProjectileSpawnPoint.position) / 2f, 0f, 0f);
		base.StartCoroutine(this.spawnBoomerang_cr(proj));
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x0011C114 File Offset: 0x0011A514
	private IEnumerator spawnBoomerang_cr(BasicProjectile proj)
	{
		proj.GetComponent<FlowerLevelBoomerang>().OnBoomerangStart(base.properties.CurrentState.boomerang.offScreenDelay);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boomerang.initialMovementDelay);
		proj.GetComponent<BasicProjectile>().Speed = (float)(-(float)base.properties.CurrentState.boomerang.speed);
		this.OnPotHandsComplete();
		yield break;
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x0011C138 File Offset: 0x0011A538
	private void SpawnBullets()
	{
		this.bulletSpawns.Clear();
		for (int i = 0; i < base.properties.CurrentState.bullets.numberOfProjectiles; i++)
		{
			this.bulletSpawns.Add(this.bulletSeedPrefab.GetComponent<FlowerLevelSeedBullet>().Create(Vector2.zero));
			Vector3 position = this.bottomProjectileSpawnPoint.position + (this.topProjectileSpawnPoint.position - this.bottomProjectileSpawnPoint.position) / (float)(base.properties.CurrentState.bullets.numberOfProjectiles - 1) * (float)i;
			this.bulletSpawns[this.bulletSpawns.Count - 1].transform.position = position;
		}
		base.StartCoroutine(this.spawnBullets_cr());
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x0011C218 File Offset: 0x0011A618
	private IEnumerator spawnBullets_cr()
	{
		List<AbstractProjectile> bullets = new List<AbstractProjectile>();
		List<AbstractProjectile> activeBullets = this.bulletSpawns;
		for (int i = 0; i < base.properties.CurrentState.bullets.numberOfProjectiles; i++)
		{
			float delay = (float)base.properties.CurrentState.bullets.holdDelay / (float)base.properties.CurrentState.bullets.numberOfProjectiles;
			int rand = UnityEngine.Random.Range(0, activeBullets.Count);
			bullets.Add(activeBullets[rand]);
			bullets[i].GetComponent<FlowerLevelSeedBullet>().OnBulletSeedStart(this, PlayerManager.GetNext(), base.properties.CurrentState.bullets.acceleration, base.properties.CurrentState.bullets.speedMinMax.min, base.properties.CurrentState.bullets.speedMinMax.max);
			activeBullets.RemoveAt(rand);
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		yield return null;
		for (int j = 0; j < bullets.Count; j++)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bullets.delayNextShot);
			yield return null;
			if (bullets[j] != null)
			{
				bullets[j].GetComponent<FlowerLevelSeedBullet>().LaunchBullet();
			}
		}
		this.OnPotHandsComplete();
		yield return null;
		yield break;
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x0011C234 File Offset: 0x0011A634
	private void SpawnCloudShot()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.cloudBombPrefab);
		Vector3 position = this.bottomProjectileSpawnPoint.position + (this.topProjectileSpawnPoint.position - this.bottomProjectileSpawnPoint.position) / 2f;
		gameObject.transform.position = position;
		gameObject.GetComponent<FlowerLevelCloudBomb>().OnCloudBombStart(PlayerManager.GetNext().center, (float)base.properties.CurrentState.puffUp.speed, base.properties.CurrentState.puffUp.delayExplosion);
		this.OnPotHandsComplete();
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x0011C2D5 File Offset: 0x0011A6D5
	private void PodHandsFX()
	{
		base.animator.Play("Twinkle", 2);
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x0011C2E8 File Offset: 0x0011A6E8
	private void GattlingEndAudio()
	{
		AudioManager.Play("flower_gattling_gun_end");
		this.emitAudioFromObject.Add("flower_gattling_gun_end");
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x0011C304 File Offset: 0x0011A704
	private void GattlingLoopAudio()
	{
		base.StartCoroutine(this.gattlingLoopEnd_cr());
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x0011C314 File Offset: 0x0011A714
	private IEnumerator gattlingLoopEnd_cr()
	{
		yield return new WaitForEndOfFrame();
		AudioManager.PlayLoop("flower_gattling_gun_loop");
		this.emitAudioFromObject.Add("flower_gattling_gun_loop");
		yield break;
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x0011C32F File Offset: 0x0011A72F
	private void StopGattlingLoopAudio()
	{
		AudioManager.Stop("flower_gattling_gun_loop");
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x0011C33B File Offset: 0x0011A73B
	private void GattlingStartAudio()
	{
		AudioManager.Play("flower_gattling_gun_start");
		this.emitAudioFromObject.Add("flower_gattling_gun_start");
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x0011C357 File Offset: 0x0011A757
	private void Phase1IntroAudio()
	{
		AudioManager.Play("flower_intro_yell");
		this.emitAudioFromObject.Add("flower_intro_yell");
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x0011C373 File Offset: 0x0011A773
	private void Phase1_2TransitionAudio()
	{
		AudioManager.Play("flower_phase1_2_transition");
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x0011C37F File Offset: 0x0011A77F
	private void Phase2DeathAudio()
	{
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x0011C381 File Offset: 0x0011A781
	private void PodHandsStartAudio()
	{
		AudioManager.Play("flower_pod_hands_start");
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x0011C38D File Offset: 0x0011A78D
	private void PodHandsOpenAudio()
	{
		AudioManager.Play("flower_pod_hands_open");
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x0011C399 File Offset: 0x0011A799
	private void PodHandsCloseAudio()
	{
		AudioManager.Play("flower_pod_hands_end");
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x0011C3A5 File Offset: 0x0011A7A5
	private void SpitStartAudio()
	{
		AudioManager.Play("flower_spit_start");
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x0011C3B1 File Offset: 0x0011A7B1
	private void TopLaserAttackStartAudio()
	{
		AudioManager.Play("flower_top_laser_attack_start");
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x0011C3BD File Offset: 0x0011A7BD
	private void TopLaserAttackHoldAudio()
	{
		AudioManager.PlayLoop("flower_top_laser_attack_hold");
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x0011C3C9 File Offset: 0x0011A7C9
	private void TopLaserAttackEndAudio()
	{
		AudioManager.Play("flower_top_laser_attack_end");
		AudioManager.Stop("flower_top_laser_attack_hold");
	}

	// Token: 0x0400278F RID: 10127
	private Action attackCallback;

	// Token: 0x04002792 RID: 10130
	public GameObject attackPoint;

	// Token: 0x04002793 RID: 10131
	private bool topLaserAttack;

	// Token: 0x04002794 RID: 10132
	private bool projectileSpawned;

	// Token: 0x04002795 RID: 10133
	private bool isDead;

	// Token: 0x04002796 RID: 10134
	private float attackCharge;

	// Token: 0x04002797 RID: 10135
	private int attackCount;

	// Token: 0x04002798 RID: 10136
	private int attackCountTarget;

	// Token: 0x04002799 RID: 10137
	private char attackType;

	// Token: 0x0400279A RID: 10138
	private int currentLaserAttack;

	// Token: 0x0400279B RID: 10139
	private int currentPodHandsAttack;

	// Token: 0x0400279C RID: 10140
	private int podHandsAttackCountTarget;

	// Token: 0x0400279D RID: 10141
	private int currentGattlingGunAttackString;

	// Token: 0x0400279E RID: 10142
	private List<string> currentGattlingGunAttackPattern;

	// Token: 0x0400279F RID: 10143
	private int currentVineHandsAttack;

	// Token: 0x040027A0 RID: 10144
	private int pollenAttackCount;

	// Token: 0x040027A1 RID: 10145
	private int currentPollenType;

	// Token: 0x040027A2 RID: 10146
	private FlowerLevelPollenProjectile currentPollenShot;

	// Token: 0x040027A3 RID: 10147
	[Header("Vines")]
	[SerializeField]
	private GameObject vineHandPrefab;

	// Token: 0x040027A4 RID: 10148
	[Space(10f)]
	[Header("Prefabs")]
	[SerializeField]
	private GameObject boomerangPrefab;

	// Token: 0x040027A5 RID: 10149
	[SerializeField]
	private GameObject bulletSeedPrefab;

	// Token: 0x040027A6 RID: 10150
	[SerializeField]
	private GameObject cloudBombPrefab;

	// Token: 0x040027A7 RID: 10151
	[SerializeField]
	private GameObject enemySeedPrefab;

	// Token: 0x040027A8 RID: 10152
	private bool miniFlowerSpawned;

	// Token: 0x040027A9 RID: 10153
	[SerializeField]
	private GameObject pollenProjectile;

	// Token: 0x040027AA RID: 10154
	[Space(10f)]
	[SerializeField]
	private Transform topProjectileSpawnPoint;

	// Token: 0x040027AB RID: 10155
	[SerializeField]
	private Transform bottomProjectileSpawnPoint;

	// Token: 0x040027AC RID: 10156
	[SerializeField]
	private GameObject mainVine;

	// Token: 0x040027AD RID: 10157
	[SerializeField]
	private GameObject gattlingFX;

	// Token: 0x040027AE RID: 10158
	private DamageDealer damageDealer;

	// Token: 0x040027AF RID: 10159
	private DamageReceiver damageReceiver;

	// Token: 0x040027B0 RID: 10160
	private List<AbstractProjectile> bulletSpawns = new List<AbstractProjectile>();
}
