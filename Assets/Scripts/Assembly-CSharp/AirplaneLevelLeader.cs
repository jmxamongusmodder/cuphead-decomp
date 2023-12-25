using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class AirplaneLevelLeader : LevelProperties.Airplane.Entity
{
	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06001418 RID: 5144 RVA: 0x000B2EF1 File Offset: 0x000B12F1
	// (set) Token: 0x06001419 RID: 5145 RVA: 0x000B2EF9 File Offset: 0x000B12F9
	public bool IsAttacking { get; private set; }

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x0600141A RID: 5146 RVA: 0x000B2F02 File Offset: 0x000B1302
	// (set) Token: 0x0600141B RID: 5147 RVA: 0x000B2F0A File Offset: 0x000B130A
	public bool camRotatedHorizontally { get; private set; }

	// Token: 0x0600141C RID: 5148 RVA: 0x000B2F13 File Offset: 0x000B1313
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x000B2F3E File Offset: 0x000B133E
	protected override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x000B2F64 File Offset: 0x000B1364
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.bulletDelayString = new PatternString(properties.CurrentState.dropshot.bulletDelayStrings, true, true);
		this.bulletColorString = new PatternString(properties.CurrentState.dropshot.bulletColorString, true, true);
		this.laserPositionStringsMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.laser.laserPositionStrings.Length);
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x000B2FD0 File Offset: 0x000B13D0
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (((AirplaneLevel)Level.Current).Rotating)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && !this.isDead)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.death_cr());
		}
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x000B3036 File Offset: 0x000B1436
	public void StartLeader()
	{
		base.animator.Play("Intro");
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x000B3048 File Offset: 0x000B1448
	public void RotateCamera()
	{
		this.camRotatedHorizontally = !this.camRotatedHorizontally;
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x000B3059 File Offset: 0x000B1459
	private void AniEvent_PawGrab()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.8f, false);
		((AirplaneLevel)Level.Current).MoveBoundsIn();
		((AirplaneLevel)Level.Current).BlurBGCamera();
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x000B3090 File Offset: 0x000B1490
	private void AniEvent_StartButtonPush()
	{
		if (base.animator.GetCurrentAnimatorStateInfo(3).IsName("Push_Wait"))
		{
			base.animator.Play("Push_Start", 3, 0f);
			base.animator.Update(0f);
			AudioManager.Play("sfx_dlc_dogfight_leadervocal_buttonbashbegin");
			this.emitAudioFromObject.Add("sfx_dlc_dogfight_leadervocal_buttonbashbegin");
		}
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x000B30FC File Offset: 0x000B14FC
	private void LateUpdate()
	{
		if (base.animator.GetCurrentAnimatorStateInfo(3).IsName("Push") && base.animator.GetCurrentAnimatorStateInfo(0).IsName("Sideways_Idle") && base.animator.GetCurrentAnimatorStateInfo(3).normalizedTime != base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		{
			base.animator.Play("Push", 3, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			base.animator.Update(0f);
		}
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x000B31A8 File Offset: 0x000B15A8
	public void StartDropshot()
	{
		base.StartCoroutine(this.drop_shot_cr());
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x000B31B8 File Offset: 0x000B15B8
	private IEnumerator drop_shot_cr()
	{
		this.IsAttacking = true;
		LevelProperties.Airplane.Dropshot p = base.properties.CurrentState.dropshot;
		this.bulletDelayString.SetSubStringIndex(0);
		AirplaneLevelDropBullet[] bullets = new AirplaneLevelDropBullet[this.bulletDelayString.SubStringLength()];
		bool onLeft = true;
		AudioManager.PlayLoop("sfx_dlc_dogfight_p3_leader_buttonpresses_loop");
		for (int i = 0; i < bullets.Length; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, this.bulletDelayString.PopFloat());
			bool isRed = this.bulletColorString.PopLetter() == 'R';
			AirplaneLevelDropBullet bullet = (!isRed) ? this.yellowBullet : this.redBullet;
			Vector3 pos = Vector3.zero;
			pos.x = ((!onLeft) ? 380f : -380f);
			pos.y = ((!isRed) ? this.yellowPosSideways.position.y : this.redPosSideways.position.y);
			Transform startPos = (!onLeft) ? this.rightDogBowlSpawn : this.leftDogBowlSpawn;
			AirplaneLevelDropBullet b = bullet.Spawn<AirplaneLevelDropBullet>();
			b.Init(pos, startPos.position, p.bulletDropSpeed, p.bulletShootSpeed, onLeft, this.camRotatedHorizontally);
			bullets[i] = b;
			AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_dogbowl_fire");
			Transform flashPos = (!onLeft) ? this.flashRootRight : this.flashRootLeft;
			Effect flash = this.flashEffect.Create(flashPos.position, flashPos.localScale);
			flash.transform.rotation = flashPos.rotation;
			onLeft = !onLeft;
		}
		base.animator.SetTrigger("EndButtonPush");
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p3_leader_buttonpresses_loop", 0f, 0.25f);
		bool stillAttacking = true;
		while (stillAttacking)
		{
			bool bulletsAlive = false;
			foreach (AirplaneLevelDropBullet airplaneLevelDropBullet in bullets)
			{
				if (airplaneLevelDropBullet.isMoving)
				{
					bulletsAlive = true;
				}
			}
			if (!bulletsAlive)
			{
				stillAttacking = false;
				break;
			}
			yield return null;
		}
		this.IsAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x000B31D4 File Offset: 0x000B15D4
	public void OpenPawHoles()
	{
		for (int i = 0; i < this.laserAnimator.Length; i++)
		{
			this.laserAnimator[i].Play("SecretOpen");
		}
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x000B320C File Offset: 0x000B160C
	public void StartLaser()
	{
		base.StartCoroutine(this.laser_main_cr());
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x000B321C File Offset: 0x000B161C
	private List<int> GetLasersToShoot(string[] lasers)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < lasers.Length; i++)
		{
			list.Add((int)(lasers[i][0] - 'A'));
		}
		return list;
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x000B3258 File Offset: 0x000B1658
	private IEnumerator laser_main_cr()
	{
		this.IsAttacking = true;
		LevelProperties.Airplane.Laser p = base.properties.CurrentState.laser;
		string[] laserPositionStrings = p.laserPositionStrings[this.laserPositionStringsMainIndex].Split(new char[]
		{
			','
		});
		for (int i = 0; i < laserPositionStrings.Length; i++)
		{
			string[] lasers = laserPositionStrings[i].Split(new char[]
			{
				':'
			});
			this.lasersToShoot = this.GetLasersToShoot(lasers);
			if (i + 1 < laserPositionStrings.Length)
			{
				string[] lasers2 = laserPositionStrings[i + 1].Split(new char[]
				{
					':'
				});
				this.lasersNextToShoot = this.GetLasersToShoot(lasers2);
			}
			else
			{
				this.lasersNextToShoot = new List<int>();
			}
			base.StartCoroutine(this.fire_lasers_cr(this.lasersToShoot, this.lasersNextToShoot, i));
			yield return CupheadTime.WaitForSeconds(this, this.buildLaserAni.length + p.laserHesitation + p.warningTime + p.laserDuration + p.laserDelay);
		}
		yield return CupheadTime.WaitForSeconds(this, this.buildLaserAni.length);
		this.laserPositionStringsMainIndex = (this.laserPositionStringsMainIndex + 1) % p.laserPositionStrings.Length;
		this.IsAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x000B3274 File Offset: 0x000B1674
	private IEnumerator fire_lasers_cr(List<int> lasers, List<int> lasersNext, int round)
	{
		LevelProperties.Airplane.Laser p = base.properties.CurrentState.laser;
		for (int i = 0; i < lasers.Count; i++)
		{
			if (!this.laserOut[lasers[i]])
			{
				this.laserAnimator[lasers[i]].Play("In");
			}
			this.laserOut[lasers[i]] = true;
		}
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_buildout");
		yield return CupheadTime.WaitForSeconds(this, this.buildLaserAni.length);
		yield return CupheadTime.WaitForSeconds(this, p.laserHesitation);
		for (int j = 0; j < lasers.Count; j++)
		{
			this.laserAnimator[lasers[j]].Play("WarningStart");
		}
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_prefire_warning");
		yield return CupheadTime.WaitForSeconds(this, p.warningTime);
		for (int k = 0; k < lasers.Count; k++)
		{
			this.laserAnimator[lasers[k]].Play("FireStart");
		}
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_fire");
		yield return CupheadTime.WaitForSeconds(this, p.laserDuration);
		bool puttingAtLeastOneLaserAway = false;
		for (int l = 0; l < lasers.Count; l++)
		{
			this.laserOut[lasers[l]] = (lasersNext.Contains(lasers[l]) && !p.forceHide);
			this.laserAnimator[lasers[l]].SetBool("StayOut", this.laserOut[lasers[l]]);
			this.laserAnimator[lasers[l]].Play("End");
			if (!this.laserOut[lasers[l]])
			{
				puttingAtLeastOneLaserAway = true;
			}
		}
		if (puttingAtLeastOneLaserAway)
		{
			AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_laser_unbuild");
		}
		yield break;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x000B32A0 File Offset: 0x000B16A0
	private IEnumerator rocket_cr()
	{
		LevelProperties.Airplane.Rocket p = base.properties.CurrentState.rocket;
		int delayMainIndex = UnityEngine.Random.Range(0, p.attackDelayString.Length);
		string[] delayString = p.attackDelayString[delayMainIndex].Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, delayString.Length);
		int dirMainIndex = UnityEngine.Random.Range(0, p.attackOrderString.Length);
		string[] dirString = p.attackOrderString[dirMainIndex].Split(new char[]
		{
			','
		});
		int dirIndex = UnityEngine.Random.Range(0, dirString.Length);
		for (;;)
		{
			delayString = p.attackDelayString[delayMainIndex].Split(new char[]
			{
				','
			});
			dirString = p.attackOrderString[dirMainIndex].Split(new char[]
			{
				','
			});
			int delay = 0;
			Parser.IntTryParse(delayString[delayIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, (float)delay);
			Vector3 position;
			if (dirString[dirIndex][0] == 'R')
			{
				position = this.rocketSpawnRight.position;
			}
			else
			{
				position = this.rocketSpawnLeft.position;
			}
			this.rocketPrefab.Create(PlayerManager.GetNext(), position, p.homingSpeed, p.homingRotation, p.homingHP, p.homingTime);
			if (dirIndex < dirString.Length - 1)
			{
				dirIndex++;
			}
			else
			{
				dirMainIndex = (dirMainIndex + 1) % p.attackOrderString.Length;
			}
			if (delayIndex < delayString.Length - 1)
			{
				delayIndex++;
			}
			else
			{
				delayMainIndex = (delayMainIndex + 1) % p.attackDelayString.Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x000B32BC File Offset: 0x000B16BC
	private IEnumerator death_cr()
	{
		this.isDead = true;
		base.GetComponent<BoxCollider2D>().enabled = false;
		this.rotatedExploder.enabled = this.camRotatedHorizontally;
		this.pawRightExploder.enabled = !this.camRotatedHorizontally;
		this.pawLeftExploder.enabled = !this.camRotatedHorizontally;
		base.animator.Play(this.camRotatedHorizontally ? "Copter_Death_Closeup" : "Copter_Death", base.animator.GetLayerIndex("Death"));
		if (!this.camRotatedHorizontally)
		{
			base.animator.Play("Off");
			base.animator.Play("Blades", base.animator.GetLayerIndex("DeathBlades"));
		}
		else
		{
			base.animator.Play("Death_Closeup", 3);
			base.animator.Play("SidewaysTears", 4);
		}
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_death");
		base.animator.Update(0f);
		base.StartCoroutine(this.activate_death_puffs_cr());
		if (!this.camRotatedHorizontally)
		{
			for (int i = 0; i < this.laserAnimator.Length; i++)
			{
				if (!this.laserAnimator[i].GetCurrentAnimatorStateInfo(0).IsName("Off"))
				{
					this.laserAnimator[i].Play((i != 2) ? "Out" : "Dead", 0, this.laserDeathTime[i]);
				}
				else if (i == 2)
				{
					this.laserAnimator[i].Play("SecretOpen");
				}
			}
			while (PauseManager.state == PauseManager.State.Paused)
			{
				yield return null;
			}
			for (int j = 0; j < this.laserAnimator.Length; j++)
			{
				this.laserAnimator[j].GetComponent<AnimationHelper>().Speed = 1.25f;
			}
			while (!this.laserAnimator[2].GetCurrentAnimatorStateInfo(0).IsName("HoldOpen"))
			{
				yield return null;
			}
			((AirplaneLevel)Level.Current).LeaderDeath();
		}
		yield break;
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x000B32D8 File Offset: 0x000B16D8
	private IEnumerator activate_death_puffs_cr()
	{
		while (this.deathPuffs.Count > 0)
		{
			int i = UnityEngine.Random.Range(0, this.deathPuffs.Count);
			this.deathPuffs[i].gameObject.SetActive(true);
			if (this.camRotatedHorizontally)
			{
				this.deathPuffs[i].Play("Sideways");
				this.deathPuffs[i].Update(0f);
			}
			this.deathPuffs.RemoveAt(i);
			yield return CupheadTime.WaitForSeconds(this, 0.16666667f);
		}
		yield break;
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x000B32F3 File Offset: 0x000B16F3
	private void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_ScreenRotateChomp()
	{
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_screenrotate_chomp");
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x000B32FF File Offset: 0x000B16FF
	private void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_ScreenRotate()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P3_DogCopter_ScreenRotate");
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x000B330B File Offset: 0x000B170B
	private void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_GrabScreen()
	{
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_settle_grabscreen");
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x000B3317 File Offset: 0x000B1717
	private void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_Intro()
	{
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_intro");
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x000B3323 File Offset: 0x000B1723
	private void AnimationEvent_SFX_DOGFIGHT_P3_Dogcopter_Intro2()
	{
		AudioManager.Play("sfx_dlc_dogfight_p3_dogcopter_intro2");
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x000B332F File Offset: 0x000B172F
	private void AnimationEvent_SFX_DOGFIGHT_P3_LeaderVocalEnd()
	{
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_command");
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x000B333C File Offset: 0x000B173C
	private void WORKAROUND_NullifyFields()
	{
		this.laserPositions = null;
		this.rocketSpawnLeft = null;
		this.rocketSpawnRight = null;
		this.yellowPosSideways = null;
		this.redPosSideways = null;
		this.flashRootLeft = null;
		this.flashRootRight = null;
		this.leftDogBowlSpawn = null;
		this.rightDogBowlSpawn = null;
		this.buildLaserAni = null;
		this.rocketPrefab = null;
		this.yellowBullet = null;
		this.redBullet = null;
		this.laserAnimator = null;
		this.flashEffect = null;
		this.lasersToShoot = null;
		this.lasersNextToShoot = null;
		this.laserOut = null;
		this.laserDeathTime = null;
		this.bulletDelayString = null;
		this.bulletColorString = null;
		this.rotatedExploder = null;
		this.pawRightExploder = null;
		this.pawLeftExploder = null;
		this.deathPuffs = null;
	}

	// Token: 0x04001D41 RID: 7489
	private const float PAW_MOVE_X = 750f;

	// Token: 0x04001D42 RID: 7490
	private const float BULLET_SPAWN_X = 380f;

	// Token: 0x04001D43 RID: 7491
	[Header("Spawn Positions")]
	[SerializeField]
	private Transform[] laserPositions;

	// Token: 0x04001D44 RID: 7492
	[SerializeField]
	private Transform rocketSpawnLeft;

	// Token: 0x04001D45 RID: 7493
	[SerializeField]
	private Transform rocketSpawnRight;

	// Token: 0x04001D46 RID: 7494
	[SerializeField]
	private Transform yellowPosSideways;

	// Token: 0x04001D47 RID: 7495
	[SerializeField]
	private Transform redPosSideways;

	// Token: 0x04001D48 RID: 7496
	[SerializeField]
	private Transform flashRootLeft;

	// Token: 0x04001D49 RID: 7497
	[SerializeField]
	private Transform flashRootRight;

	// Token: 0x04001D4A RID: 7498
	[SerializeField]
	private Transform leftDogBowlSpawn;

	// Token: 0x04001D4B RID: 7499
	[SerializeField]
	private Transform rightDogBowlSpawn;

	// Token: 0x04001D4C RID: 7500
	[SerializeField]
	private AnimationClip buildLaserAni;

	// Token: 0x04001D4D RID: 7501
	[Header("Prefabs")]
	[SerializeField]
	private AirplaneLevelRocket rocketPrefab;

	// Token: 0x04001D4E RID: 7502
	[SerializeField]
	private AirplaneLevelDropBullet yellowBullet;

	// Token: 0x04001D4F RID: 7503
	[SerializeField]
	private AirplaneLevelDropBullet redBullet;

	// Token: 0x04001D50 RID: 7504
	[SerializeField]
	private Animator[] laserAnimator;

	// Token: 0x04001D51 RID: 7505
	[SerializeField]
	private Effect flashEffect;

	// Token: 0x04001D54 RID: 7508
	private List<int> lasersToShoot;

	// Token: 0x04001D55 RID: 7509
	private List<int> lasersNextToShoot;

	// Token: 0x04001D56 RID: 7510
	private bool[] laserOut = new bool[5];

	// Token: 0x04001D57 RID: 7511
	private float[] laserDeathTime = new float[]
	{
		0.2f,
		0.6f,
		0.8f,
		0.26666668f,
		0.4f
	};

	// Token: 0x04001D58 RID: 7512
	private PatternString bulletDelayString;

	// Token: 0x04001D59 RID: 7513
	private PatternString bulletColorString;

	// Token: 0x04001D5A RID: 7514
	private int laserPositionStringsMainIndex;

	// Token: 0x04001D5B RID: 7515
	private bool isDead;

	// Token: 0x04001D5C RID: 7516
	private DamageReceiver damageReceiver;

	// Token: 0x04001D5D RID: 7517
	[SerializeField]
	private LevelBossDeathExploder rotatedExploder;

	// Token: 0x04001D5E RID: 7518
	[SerializeField]
	private LevelBossDeathExploder pawRightExploder;

	// Token: 0x04001D5F RID: 7519
	[SerializeField]
	private LevelBossDeathExploder pawLeftExploder;

	// Token: 0x04001D60 RID: 7520
	[SerializeField]
	private List<Animator> deathPuffs;
}
