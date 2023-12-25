using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000574 RID: 1396
public class DevilLevelGiantHead : LevelProperties.Devil.Entity
{
	// Token: 0x06001A75 RID: 6773 RVA: 0x000F2144 File Offset: 0x000F0544
	protected override void Awake()
	{
		base.Awake();
		base.animator.Play("Idle");
		base.animator.Play("Idle_Body", 1);
		this.state = DevilLevelGiantHead.State.Intro;
		this.child.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnWinEvent += this.Death;
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x000F21CF File Offset: 0x000F05CF
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x000F21E2 File Offset: 0x000F05E2
	public void StartIntroTransform()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x000F21F4 File Offset: 0x000F05F4
	private IEnumerator intro_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.child.GetComponent<Collider2D>().enabled = false;
		this.platformCr = base.StartCoroutine(this.platforms_cr());
		base.StartCoroutine(this.fireballs_cr());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.state = DevilLevelGiantHead.State.Idle;
		this.waitingForTransform = false;
		yield break;
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x000F220F File Offset: 0x000F060F
	private void OnNeck()
	{
		base.animator.Play("Idle_Body");
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x000F2221 File Offset: 0x000F0621
	private void NoNeck()
	{
		base.animator.Play("Off_Body");
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x000F2233 File Offset: 0x000F0633
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.fireballPrefab = null;
		this.bombPrefab = null;
		this.skullPrefab = null;
		this.swooperPrefab = null;
		this.tearPrefab = null;
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x000F2260 File Offset: 0x000F0660
	private IEnumerator platforms_cr()
	{
		LevelProperties.Devil.GiantHeadPlatforms p = base.properties.CurrentState.giantHeadPlatforms;
		string[] pattern = p.riseString.Split(new char[]
		{
			','
		});
		int patternIndex = UnityEngine.Random.Range(0, pattern.Length);
		for (;;)
		{
			while (this.waitingForTransform)
			{
				yield return null;
			}
			p = base.properties.CurrentState.giantHeadPlatforms;
			patternIndex = (patternIndex + 1) % pattern.Length;
			int platformIndex;
			Parser.IntTryParse(pattern[patternIndex], out platformIndex);
			DevilLevelPlatform platform = this.raisablePlatforms[platformIndex - 1];
			if (platform.state != DevilLevelPlatform.State.Idle)
			{
				bool noIdlePlatforms = true;
				while (noIdlePlatforms)
				{
					foreach (DevilLevelPlatform devilLevelPlatform in this.raisablePlatforms)
					{
						if (devilLevelPlatform.state == DevilLevelPlatform.State.Idle)
						{
							noIdlePlatforms = false;
						}
					}
					if (noIdlePlatforms)
					{
						yield return CupheadTime.WaitForSeconds(this, p.riseDelayRange.RandomFloat());
					}
				}
			}
			else
			{
				platform.Raise(p.riseSpeed, p.maxHeight, p.holdDelay);
				yield return CupheadTime.WaitForSeconds(this, p.riseDelayRange.RandomFloat());
			}
		}
		yield break;
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x000F227C File Offset: 0x000F067C
	private IEnumerator fireballs_cr()
	{
		bool fromRight = Rand.Bool();
		int index = (!fromRight) ? 0 : (this.raisablePlatforms.Length - 1);
		LevelProperties.Devil.Fireballs p = base.properties.CurrentState.fireballs;
		yield return CupheadTime.WaitForSeconds(this, p.initialDelay);
		for (;;)
		{
			p = base.properties.CurrentState.fireballs;
			DevilLevelPlatform platform = this.raisablePlatforms[index];
			index = (((!fromRight) ? (index + 1) : (index - 1)) + this.raisablePlatforms.Length) % this.raisablePlatforms.Length;
			if (platform.state == DevilLevelPlatform.State.Dead)
			{
				yield return null;
			}
			else
			{
				this.fireballPrefab.Create(platform.transform.position.x, p.fallSpeed, p.fallAcceleration, p.size / 200f);
				yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			}
		}
		yield break;
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000F2297 File Offset: 0x000F0697
	public void StartBombEye()
	{
		this.state = DevilLevelGiantHead.State.BombEye;
		base.StartCoroutine(this.eye_cr(base.properties.CurrentState.bombEye.hesitate.RandomFloat()));
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000F22C7 File Offset: 0x000F06C7
	public void StartSkullEye()
	{
		this.state = DevilLevelGiantHead.State.SkullEye;
		base.StartCoroutine(this.eye_cr(base.properties.CurrentState.skullEye.hesitate.RandomFloat()));
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x000F22F8 File Offset: 0x000F06F8
	private IEnumerator eye_cr(float hesitateTime)
	{
		if (this.state == DevilLevelGiantHead.State.BombEye)
		{
			this.bombOnLeft = Rand.Bool();
			this.spawnPos = ((!this.bombOnLeft) ? this.rightEyeRoot.position : this.leftEyeRoot.position);
			base.animator.SetTrigger("OnBomb");
			base.animator.SetBool("BombLeft", this.bombOnLeft);
		}
		else
		{
			this.spawnPos = this.middleRoot.transform.position;
			base.animator.SetTrigger("OnSpiral");
		}
		yield return CupheadTime.WaitForSeconds(this, hesitateTime);
		this.state = DevilLevelGiantHead.State.Idle;
		yield break;
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x000F231A File Offset: 0x000F071A
	private void SpawnBomb()
	{
		this.bombPrefab.Create(this.spawnPos, base.properties.CurrentState.bombEye, this.bombOnLeft);
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x000F2344 File Offset: 0x000F0744
	private void Offset()
	{
		if (base.GetComponent<SpriteRenderer>().flipX)
		{
			base.transform.AddPosition(-60f, 0f, 0f);
		}
		else
		{
			base.transform.AddPosition(60f, 0f, 0f);
		}
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x000F239A File Offset: 0x000F079A
	private void SpawnSpiral()
	{
		this.skullPrefab.Create(this.spawnPos, base.properties.CurrentState.skullEye);
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x000F23BE File Offset: 0x000F07BE
	public void StartHands()
	{
		base.animator.SetTrigger("OnTransA");
		this.handsCr = base.StartCoroutine(this.hands_cr());
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x000F23E4 File Offset: 0x000F07E4
	private IEnumerator hands_cr()
	{
		this.waitingForTransform = true;
		while (this.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		bool platformsDown = false;
		while (!platformsDown)
		{
			platformsDown = true;
			foreach (DevilLevelPlatform devilLevelPlatform in this.raisablePlatforms)
			{
				if (devilLevelPlatform.state == DevilLevelPlatform.State.Raising)
				{
					platformsDown = false;
				}
			}
			yield return null;
		}
		this.waitingForTransform = false;
		foreach (DevilLevelPlatform devilLevelPlatform2 in this.HandsPhaseExit)
		{
			devilLevelPlatform2.Lower(base.properties.CurrentState.giantHeadPlatforms.exitSpeed);
		}
		this.StartSwoopers();
		bool leftHandShoot = Rand.Bool();
		this.hands[0].StartPattern(base.properties.CurrentState.hands);
		this.hands[1].StartPattern(base.properties.CurrentState.hands);
		this.handsSpawnCr = base.StartCoroutine(this.spawn_hand_cr());
		for (;;)
		{
			int handIndex = (!leftHandShoot) ? 1 : 0;
			if (this.hands[handIndex] != null)
			{
				this.hands[handIndex].animator.SetTrigger("OnAttack");
			}
			leftHandShoot = !leftHandShoot;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.hands.shotDelay.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x000F2400 File Offset: 0x000F0800
	private IEnumerator spawn_hand_cr()
	{
		LevelProperties.Devil.Hands p = base.properties.CurrentState.hands;
		yield return CupheadTime.WaitForSeconds(this, p.initialSpawnDelay.RandomFloat());
		this.hands[0].SpawnIn();
		yield return CupheadTime.WaitForSeconds(this, p.initialSpawnDelay.RandomFloat());
		this.hands[1].SpawnIn();
		while (!this.hands[0].isDead)
		{
			while (!this.hands[0].despawned && !this.hands[1].despawned)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelayRange.RandomFloat());
			if (this.hands[0].despawned)
			{
				this.hands[0].SpawnIn();
			}
			else if (this.hands[1].despawned)
			{
				this.hands[1].SpawnIn();
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x000F241B File Offset: 0x000F081B
	public void StartSwoopers()
	{
		this.swooperSpawnCr = base.StartCoroutine(this.swooper_spawn_cr());
		this.swooperSwoopCr = base.StartCoroutine(this.swooper_swoop_cr());
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x000F2444 File Offset: 0x000F0844
	private IEnumerator swooper_spawn_cr()
	{
		LevelProperties.Devil.Swoopers p = base.properties.CurrentState.swoopers;
		string[] swooperSlotPositions = p.positions.Split(new char[]
		{
			','
		});
		this.swoopers = new List<DevilLevelSwooper>();
		this.swooperSlots = new DevilLevelGiantHead.SwooperSlot[swooperSlotPositions.Length];
		for (int i = 0; i < this.swooperSlots.Length; i++)
		{
			float num = 0f;
			Parser.FloatTryParse(swooperSlotPositions[i], out num);
			this.swooperSlots[i] = new DevilLevelGiantHead.SwooperSlot(num - 600f);
		}
		int swooperSlotIndex = UnityEngine.Random.Range(0, this.swooperSlots.Length);
		float delay = p.initialSpawnDelay.RandomFloat();
		int spawnPoint = UnityEngine.Random.Range(0, this.spawnPoints.Length);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			delay = p.spawnDelay.RandomFloat();
			if (this.swoopers.Count < p.maxCount)
			{
				int numToSpawn = p.spawnCount.RandomInt();
				int numSpawned = 0;
				base.animator.SetBool("IsWhincing", true);
				while (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Whince"))
				{
					yield return null;
				}
				while (numSpawned < numToSpawn && this.swoopers.Count < p.maxCount)
				{
					swooperSlotIndex = (swooperSlotIndex + 1) % this.swooperSlots.Length;
					DevilLevelGiantHead.SwooperSlot slot = this.swooperSlots[swooperSlotIndex];
					if (slot.swooper == null)
					{
						DevilLevelSwooper devilLevelSwooper = this.swooperPrefab.Create(this, p, this.spawnPoints[spawnPoint].position, slot.xPos);
						slot.swooper = devilLevelSwooper;
						this.swoopers.Add(devilLevelSwooper);
						numSpawned++;
					}
					spawnPoint = (spawnPoint + 1) % this.spawnPoints.Length;
					yield return CupheadTime.WaitForSeconds(this, 0.4f);
				}
			}
			yield return CupheadTime.WaitForSeconds(this, 1.5f);
			base.animator.SetBool("IsWhincing", false);
		}
		yield break;
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x000F2460 File Offset: 0x000F0860
	private IEnumerator swooper_swoop_cr()
	{
		LevelProperties.Devil.Swoopers p = base.properties.CurrentState.swoopers;
		for (;;)
		{
			while (this.swoopers.Count == 0)
			{
				yield return null;
			}
			List<DevilLevelSwooper> attackSwoopers = new List<DevilLevelSwooper>(this.swoopers);
			attackSwoopers.Shuffle<DevilLevelSwooper>();
			yield return CupheadTime.WaitForSeconds(this, p.attackDelay.RandomFloat());
			foreach (DevilLevelSwooper swooper in attackSwoopers)
			{
				if (swooper != null && swooper.state == DevilLevelSwooper.State.Idle)
				{
					swooper.Swoop();
					if (swooper == attackSwoopers[attackSwoopers.Count - 1])
					{
						swooper.finalSwooping = true;
					}
					this.RemoveSwooperFromSlot(swooper);
					if (swooper != attackSwoopers[attackSwoopers.Count - 1])
					{
						yield return CupheadTime.WaitForSeconds(this, p.attackDelay.RandomFloat());
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000F247B File Offset: 0x000F087B
	public void OnSwooperDeath(DevilLevelSwooper swooper)
	{
		this.swoopers.Remove(swooper);
		this.RemoveSwooperFromSlot(swooper);
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x000F2494 File Offset: 0x000F0894
	private void RemoveSwooperFromSlot(DevilLevelSwooper swooper)
	{
		for (int i = 0; i < this.swooperSlots.Length; i++)
		{
			if (this.swooperSlots[i].swooper == swooper)
			{
				this.swooperSlots[i].swooper = null;
			}
		}
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x000F24E8 File Offset: 0x000F08E8
	public float PutSwooperInSlot(DevilLevelSwooper swooper)
	{
		float num = float.MaxValue;
		int num2 = 0;
		for (int i = 0; i < this.swooperSlots.Length; i++)
		{
			if (!(this.swooperSlots[i].swooper != null))
			{
				float num3 = Mathf.Abs(this.swooperSlots[i].xPos - swooper.transform.position.x);
				if (num3 < num)
				{
					num = num3;
					num2 = i;
				}
			}
		}
		this.swooperSlots[num2].swooper = swooper;
		return this.swooperSlots[num2].xPos;
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x000F2591 File Offset: 0x000F0991
	public void StartTears()
	{
		base.StartCoroutine(this.tears_cr());
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x000F25A0 File Offset: 0x000F09A0
	private IEnumerator tears_cr()
	{
		base.animator.SetTrigger("OnTransB");
		this.waitingForTransform = true;
		while (this.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		bool platformsDown = false;
		while (!platformsDown)
		{
			platformsDown = true;
			foreach (DevilLevelPlatform devilLevelPlatform in this.raisablePlatforms)
			{
				if (devilLevelPlatform.state == DevilLevelPlatform.State.Raising)
				{
					platformsDown = false;
				}
			}
			yield return null;
		}
		this.waitingForTransform = false;
		foreach (DevilLevelPlatform devilLevelPlatform2 in this.TearsPhaseExit)
		{
			devilLevelPlatform2.Lower(base.properties.CurrentState.giantHeadPlatforms.exitSpeed);
		}
		if (!base.properties.CurrentState.giantHeadPlatforms.riseDuringTearPhase)
		{
			base.StopCoroutine(this.platformCr);
		}
		base.StopCoroutine(this.handsCr);
		base.StopCoroutine(this.handsSpawnCr);
		base.StopCoroutine(this.swooperSpawnCr);
		base.StopCoroutine(this.swooperSwoopCr);
		while (this.swoopers.Count > 0)
		{
			this.swoopers[0].Die();
		}
		foreach (DevilLevelHand devilLevelHand in this.hands)
		{
			devilLevelHand.isDead = true;
			devilLevelHand.Die();
		}
		bool spawnLeft = true;
		yield return CupheadTime.WaitForSeconds(this, 2f);
		for (;;)
		{
			this.tearPrefab.CreateTear((!spawnLeft) ? this.rightTearRoot.transform.position : this.leftTearRoot.transform.position, base.properties.CurrentState.tears.speed);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.tears.delay);
			spawnLeft = !spawnLeft;
		}
		yield break;
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x000F25BB File Offset: 0x000F09BB
	private void Death()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDead");
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000F25DF File Offset: 0x000F09DF
	private void sfx_p3_bomb_appear()
	{
		AudioManager.Play("p3_bomb_appear");
		this.emitAudioFromObject.Add("p3_bomb_appear");
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000F25FB File Offset: 0x000F09FB
	private void sfx_p3_bomb_attack()
	{
		AudioManager.Play("p3_bomb_attack");
		this.emitAudioFromObject.Add("p3_bomb_attack");
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x000F2617 File Offset: 0x000F0A17
	private void sfx_p3_cry_idle()
	{
		AudioManager.Play("p3_cry_idle");
		this.emitAudioFromObject.Add("p3_cry_idle");
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000F2633 File Offset: 0x000F0A33
	private void sfx_p3_dead_loop()
	{
		if (!this.DeadLoopSFXActive)
		{
			AudioManager.PlayLoop("p3_dead_loop");
			this.emitAudioFromObject.Add("p3_dead_loop");
			this.DeadLoopSFXActive = true;
		}
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x000F2661 File Offset: 0x000F0A61
	private void sfx_p3_dead_loop_stop()
	{
		AudioManager.Stop("p3_dead_loop");
		this.DeadLoopSFXActive = false;
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x000F2674 File Offset: 0x000F0A74
	private void sfx_p3_hand_release_start()
	{
		AudioManager.Play("p3_hand_release_start");
		this.emitAudioFromObject.Add("p3_hand_release_start");
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x000F2690 File Offset: 0x000F0A90
	private void sfx_p3_hurt_trans_a()
	{
		AudioManager.Play("p3_hurt_trans_a");
		this.emitAudioFromObject.Add("p3_hurt_trans_a");
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x000F26AC File Offset: 0x000F0AAC
	private void sfx_p3_spiral_attack()
	{
		AudioManager.Play("p3_spiral_attack");
		this.emitAudioFromObject.Add("p3_spiral_attack");
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x000F26C8 File Offset: 0x000F0AC8
	private void sfx_p3_intro_end()
	{
		AudioManager.Play("p3_intro_end");
		this.emitAudioFromObject.Add("p3_intro_end");
	}

	// Token: 0x0400239F RID: 9119
	public DevilLevelGiantHead.State state;

	// Token: 0x040023A0 RID: 9120
	[SerializeField]
	private GameObject[] groundPieces;

	// Token: 0x040023A1 RID: 9121
	[SerializeField]
	private DevilLevelPlatform[] HandsPhaseExit;

	// Token: 0x040023A2 RID: 9122
	[SerializeField]
	private DevilLevelPlatform[] TearsPhaseExit;

	// Token: 0x040023A3 RID: 9123
	[SerializeField]
	private DevilLevelPlatform[] raisablePlatforms;

	// Token: 0x040023A4 RID: 9124
	[SerializeField]
	private Transform stage3Platforms;

	// Token: 0x040023A5 RID: 9125
	[SerializeField]
	private DevilLevelFireball fireballPrefab;

	// Token: 0x040023A6 RID: 9126
	[SerializeField]
	private DevilLevelBomb bombPrefab;

	// Token: 0x040023A7 RID: 9127
	[SerializeField]
	private DevilLevelSkull skullPrefab;

	// Token: 0x040023A8 RID: 9128
	[SerializeField]
	private Transform leftEyeRoot;

	// Token: 0x040023A9 RID: 9129
	[SerializeField]
	private Transform rightEyeRoot;

	// Token: 0x040023AA RID: 9130
	[SerializeField]
	private Transform middleRoot;

	// Token: 0x040023AB RID: 9131
	[SerializeField]
	private Transform leftTearRoot;

	// Token: 0x040023AC RID: 9132
	[SerializeField]
	private Transform rightTearRoot;

	// Token: 0x040023AD RID: 9133
	[SerializeField]
	private DevilLevelHand[] hands;

	// Token: 0x040023AE RID: 9134
	[SerializeField]
	private DevilLevelSwooper swooperPrefab;

	// Token: 0x040023AF RID: 9135
	[SerializeField]
	private DevilLevelTear tearPrefab;

	// Token: 0x040023B0 RID: 9136
	[SerializeField]
	private SpriteRenderer bottomSprite;

	// Token: 0x040023B1 RID: 9137
	[SerializeField]
	private DamageReceiver child;

	// Token: 0x040023B2 RID: 9138
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x040023B3 RID: 9139
	private bool waitingForTransform;

	// Token: 0x040023B4 RID: 9140
	private bool bombOnLeft;

	// Token: 0x040023B5 RID: 9141
	private bool DeadLoopSFXActive;

	// Token: 0x040023B6 RID: 9142
	private DamageReceiver damageReceiver;

	// Token: 0x040023B7 RID: 9143
	private Coroutine platformCr;

	// Token: 0x040023B8 RID: 9144
	private Coroutine handsCr;

	// Token: 0x040023B9 RID: 9145
	private Coroutine handsSpawnCr;

	// Token: 0x040023BA RID: 9146
	private Coroutine swooperSpawnCr;

	// Token: 0x040023BB RID: 9147
	private Coroutine swooperSwoopCr;

	// Token: 0x040023BC RID: 9148
	private Vector2 spawnPos;

	// Token: 0x040023BD RID: 9149
	private Color color;

	// Token: 0x040023BE RID: 9150
	private DevilLevelGiantHead.SwooperSlot[] swooperSlots;

	// Token: 0x040023BF RID: 9151
	private List<DevilLevelSwooper> swoopers;

	// Token: 0x02000575 RID: 1397
	public enum State
	{
		// Token: 0x040023C1 RID: 9153
		Intro,
		// Token: 0x040023C2 RID: 9154
		Idle,
		// Token: 0x040023C3 RID: 9155
		BombEye,
		// Token: 0x040023C4 RID: 9156
		SkullEye
	}

	// Token: 0x02000576 RID: 1398
	private struct SwooperSlot
	{
		// Token: 0x06001A99 RID: 6809 RVA: 0x000F26E4 File Offset: 0x000F0AE4
		public SwooperSlot(float xPos)
		{
			this.xPos = xPos;
			this.swooper = null;
		}

		// Token: 0x040023C5 RID: 9157
		public float xPos;

		// Token: 0x040023C6 RID: 9158
		public DevilLevelSwooper swooper;
	}
}
