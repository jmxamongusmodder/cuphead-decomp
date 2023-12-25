using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class BeeLevelAirplane : LevelProperties.Bee.Entity
{
	// Token: 0x17000326 RID: 806
	// (get) Token: 0x060016F1 RID: 5873 RVA: 0x000CE3A2 File Offset: 0x000CC7A2
	// (set) Token: 0x060016F2 RID: 5874 RVA: 0x000CE3AA File Offset: 0x000CC7AA
	public BeeLevelAirplane.State state { get; private set; }

	// Token: 0x060016F3 RID: 5875 RVA: 0x000CE3B4 File Offset: 0x000CC7B4
	protected override void Awake()
	{
		base.Awake();
		this.state = BeeLevelAirplane.State.Unspawned;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.midLayer.GetComponentInChildren<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.topLayer.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x060016F4 RID: 5876 RVA: 0x000CE438 File Offset: 0x000CC838
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != BeeLevelAirplane.State.Dead)
		{
			this.state = BeeLevelAirplane.State.Dead;
			this.Dead();
		}
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x000CE484 File Offset: 0x000CC884
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060016F6 RID: 5878 RVA: 0x000CE4A2 File Offset: 0x000CC8A2
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016F7 RID: 5879 RVA: 0x000CE4BA File Offset: 0x000CC8BA
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bullet = null;
	}

	// Token: 0x060016F8 RID: 5880 RVA: 0x000CE4C9 File Offset: 0x000CC8C9
	public void StartIntro()
	{
		this.state = BeeLevelAirplane.State.Intro;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060016F9 RID: 5881 RVA: 0x000CE4E0 File Offset: 0x000CC8E0
	private IEnumerator intro_cr()
	{
		float speed = 400f;
		this.countPattern = base.properties.CurrentState.wingSwipe.attackCount.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.countIndex = UnityEngine.Random.Range(0, this.countPattern.Length);
		while (base.transform.position.y < -60f)
		{
			base.transform.AddPosition(0f, speed * CupheadTime.Delta, 0f);
			yield return null;
		}
		this.state = BeeLevelAirplane.State.Idle;
		base.StartCoroutine(this.move_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060016FA RID: 5882 RVA: 0x000CE4FB File Offset: 0x000CC8FB
	private void IdleCount()
	{
		this.blinkCount++;
		if (this.blinkCount >= this.blinkCountMax)
		{
			this.blinkOne = Rand.Bool();
		}
	}

	// Token: 0x060016FB RID: 5883 RVA: 0x000CE528 File Offset: 0x000CC928
	private void Blink_One()
	{
		if (this.blinkCount >= this.blinkCountMax && this.blinkOne)
		{
			this.topLayer.enabled = true;
			this.blinkCount = 0;
			this.blinkCountMax = UnityEngine.Random.Range(3, 7);
		}
		else
		{
			this.topLayer.enabled = false;
		}
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x000CE584 File Offset: 0x000CC984
	private void Blink_Two()
	{
		if (this.blinkCount >= this.blinkCountMax && !this.blinkOne)
		{
			this.topLayer.enabled = true;
			this.blinkCount = 0;
			this.blinkCountMax = UnityEngine.Random.Range(3, 7);
		}
		else
		{
			this.topLayer.enabled = false;
		}
	}

	// Token: 0x060016FD RID: 5885 RVA: 0x000CE5E0 File Offset: 0x000CC9E0
	private IEnumerator move_cr()
	{
		bool isLooping = false;
		LevelProperties.Bee.General p = base.properties.CurrentState.general;
		this.movingRight = false;
		this.isMoving = true;
		this.offset = p.movementOffset;
		this.speed = p.movementSpeed;
		for (;;)
		{
			if (this.isMoving)
			{
				if (this.movingRight)
				{
					while (base.transform.position.x < 640f - this.offset && this.movingRight)
					{
						base.transform.AddPosition(this.speed * CupheadTime.Delta * this.hitPauseCoefficient(), 0f, 0f);
						yield return null;
					}
					if (this.state != BeeLevelAirplane.State.Wing)
					{
						this.movingRight = !this.movingRight;
					}
				}
				else
				{
					while (base.transform.position.x > -640f + this.offset && !this.movingRight)
					{
						base.transform.AddPosition(-this.speed * CupheadTime.Delta * this.hitPauseCoefficient(), 0f, 0f);
						yield return null;
					}
					if (this.state != BeeLevelAirplane.State.Wing)
					{
						this.movingRight = !this.movingRight;
					}
				}
				if (!isLooping)
				{
					AudioManager.PlayLoop("bee_airplane_idle_loop");
					this.emitAudioFromObject.Add("bee_airplane_idle_loop");
					isLooping = true;
				}
			}
			else if (isLooping)
			{
				AudioManager.Stop("bee_airplane_idle_loop");
				isLooping = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x000CE5FB File Offset: 0x000CC9FB
	private float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x000CE61C File Offset: 0x000CCA1C
	public void StartTurbine()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.turbine_cr());
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x000CE648 File Offset: 0x000CCA48
	private IEnumerator turbine_cr()
	{
		this.state = BeeLevelAirplane.State.Turbine;
		LevelProperties.Bee.TurbineBlasters p = base.properties.CurrentState.turbineBlasters;
		string[] bulletPattern = p.attackDirectionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		for (int i = 0; i < bulletPattern.Length; i++)
		{
			if (bulletPattern[i][0] == 'R')
			{
				base.animator.Play("Right_Pylon");
			}
			else if (bulletPattern[i][0] == 'L')
			{
				base.animator.Play("Left_Pylon");
			}
			else if (bulletPattern[i][0] == 'B')
			{
				base.animator.Play("Right_Pylon");
				base.animator.Play("Left_Pylon");
			}
			yield return CupheadTime.WaitForSeconds(this, p.repeatDealy);
		}
		yield return CupheadTime.WaitForSeconds(this, p.hesitateRange.RandomFloat());
		this.state = BeeLevelAirplane.State.Idle;
		yield break;
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x000CE664 File Offset: 0x000CCA64
	private void ShootBulletRight()
	{
		AudioManager.Play("bee_airplane_pylon");
		this.emitAudioFromObject.Add("bee_airplane_pylon");
		Vector3 v = new Vector3(0f, 360f, 0f) - new Vector3(0f, base.transform.position.y, 0f);
		float rotation = MathUtils.DirectionToAngle(v);
		this.bullet.Create(this.rightShootRoot.transform.position, rotation, true, base.properties.CurrentState.turbineBlasters);
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x000CE708 File Offset: 0x000CCB08
	private void ShootBulletLeft()
	{
		AudioManager.Play("bee_airplane_pylon");
		this.emitAudioFromObject.Add("bee_airplane_pylon");
		Vector3 v = new Vector3(0f, 360f, 0f) - new Vector3(0f, base.transform.position.y, 0f);
		float rotation = MathUtils.DirectionToAngle(v);
		this.bullet.Create(this.leftShootRoot.transform.position, rotation, false, base.properties.CurrentState.turbineBlasters);
	}

	// Token: 0x06001703 RID: 5891 RVA: 0x000CE7AA File Offset: 0x000CCBAA
	public void StartWing()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.wing_cr());
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x000CE7D8 File Offset: 0x000CCBD8
	private IEnumerator wing_cr()
	{
		this.state = BeeLevelAirplane.State.Wing;
		LevelProperties.Bee.WingSwipe p = base.properties.CurrentState.wingSwipe;
		AbstractPlayerController player = PlayerManager.GetNext();
		this.attackOnRight = (player.transform.position.x >= 0f);
		Vector3 startPos = Vector3.zero;
		int count = 0;
		Parser.IntTryParse(this.countPattern[this.countIndex], out count);
		for (int i = 0; i < count; i++)
		{
			base.animator.SetTrigger("OnSaw");
			yield return base.animator.WaitForAnimationToEnd(this, "Saw_Start", false, true);
			this.movingRight = this.attackOnRight;
			this.offset = p.warningMaxDistance;
			this.speed = p.warningMovementSpeed;
			if (this.attackOnRight)
			{
				while (base.transform.position.x < 640f - p.warningMaxDistance)
				{
					yield return null;
				}
			}
			else
			{
				while (base.transform.position.x > -640f + p.warningMaxDistance)
				{
					yield return null;
				}
			}
			this.isMoving = false;
			yield return CupheadTime.WaitForSeconds(this, p.warningDuration);
			base.animator.SetTrigger("Continue");
			this.isMoving = true;
			this.offset = p.maxDistance;
			this.movingRight = !this.movingRight;
			this.speed = p.movementSpeed;
			if (!this.attackOnRight)
			{
				while (base.transform.position.x < 640f - p.maxDistance)
				{
					yield return null;
				}
			}
			else
			{
				while (base.transform.position.x > -640f + p.maxDistance)
				{
					yield return null;
				}
			}
			this.isMoving = false;
			yield return CupheadTime.WaitForSeconds(this, p.attackDuration);
			base.animator.SetTrigger("End");
			this.isMoving = true;
			this.offset = base.properties.CurrentState.general.movementOffset;
			this.attackOnRight = !this.attackOnRight;
			this.speed = base.properties.CurrentState.general.movementSpeed;
		}
		this.state = BeeLevelAirplane.State.EndWing;
		this.countIndex = (this.countIndex + 1) % this.countPattern.Length;
		yield return CupheadTime.WaitForSeconds(this, p.hesitateRange.RandomFloat());
		this.state = BeeLevelAirplane.State.Idle;
		yield break;
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x000CE7F3 File Offset: 0x000CCBF3
	private void SawLoopSFX()
	{
		this.SawStartSFX();
		AudioManager.PlayLoop("bee_airplane_saw_loop");
		this.emitAudioFromObject.Add("bee_airplane_saw_loop");
	}

	// Token: 0x06001706 RID: 5894 RVA: 0x000CE815 File Offset: 0x000CCC15
	private void SawStartSFX()
	{
		AudioManager.Play("bee_airplane_saw_start");
		this.emitAudioFromObject.Add("bee_airplane_saw_start");
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x000CE831 File Offset: 0x000CCC31
	private void SawEndSFX()
	{
		AudioManager.Stop("bee_airplane_saw_loop");
		AudioManager.Play("bee_airplane_saw_end");
		this.emitAudioFromObject.Add("bee_airplane_saw_end");
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x000CE857 File Offset: 0x000CCC57
	private void DeathHeadSFX()
	{
		AudioManager.Play("bee_airplane_death_head");
		this.emitAudioFromObject.Add("bee_airplane_death_head");
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x000CE873 File Offset: 0x000CCC73
	private void SawContinueSFX()
	{
		if (!this.isPreSFXPlaying)
		{
			AudioManager.Play("bee_airplane_saw_continue");
			this.emitAudioFromObject.Add("bee_airplane_saw_continue");
			this.isPreSFXPlaying = true;
		}
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x000CE8A1 File Offset: 0x000CCCA1
	private void SawContinueSFXEnd()
	{
		this.isPreSFXPlaying = false;
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x000CE8AC File Offset: 0x000CCCAC
	private void Flip()
	{
		if (this.attackOnRight)
		{
			base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		}
		else
		{
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
		}
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x000CE91B File Offset: 0x000CCD1B
	private void Dead()
	{
		this.StopAllCoroutines();
		AudioManager.Stop("bee_airplane_saw_loop");
		AudioManager.Play("bee_airplane_death");
		this.emitAudioFromObject.Add("bee_airplane_death");
		base.animator.SetTrigger("Death");
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x000CE957 File Offset: 0x000CCD57
	private void DeadHead()
	{
		base.animator.Play("Death_Head");
	}

	// Token: 0x0400203A RID: 8250
	[SerializeField]
	private Transform rightShootRoot;

	// Token: 0x0400203B RID: 8251
	[SerializeField]
	private Transform leftShootRoot;

	// Token: 0x0400203C RID: 8252
	[SerializeField]
	private BeeLevelTurbineBullet bullet;

	// Token: 0x0400203D RID: 8253
	[SerializeField]
	private SpriteRenderer topLayer;

	// Token: 0x0400203E RID: 8254
	[SerializeField]
	private SpriteRenderer midLayer;

	// Token: 0x04002040 RID: 8256
	private string[] countPattern;

	// Token: 0x04002041 RID: 8257
	private int countIndex;

	// Token: 0x04002042 RID: 8258
	private int blinkCount;

	// Token: 0x04002043 RID: 8259
	private int blinkCountMax;

	// Token: 0x04002044 RID: 8260
	private bool blinkOne;

	// Token: 0x04002045 RID: 8261
	private bool attackOnRight;

	// Token: 0x04002046 RID: 8262
	private bool movingRight;

	// Token: 0x04002047 RID: 8263
	private bool isMoving;

	// Token: 0x04002048 RID: 8264
	private bool isPreSFXPlaying;

	// Token: 0x04002049 RID: 8265
	private float offset;

	// Token: 0x0400204A RID: 8266
	private float speed;

	// Token: 0x0400204B RID: 8267
	private DamageDealer damageDealer;

	// Token: 0x0400204C RID: 8268
	private DamageReceiver damageReceiver;

	// Token: 0x0400204D RID: 8269
	private Coroutine patternCoroutine;

	// Token: 0x0200050F RID: 1295
	public enum State
	{
		// Token: 0x0400204F RID: 8271
		Unspawned,
		// Token: 0x04002050 RID: 8272
		Intro,
		// Token: 0x04002051 RID: 8273
		Idle,
		// Token: 0x04002052 RID: 8274
		Wing,
		// Token: 0x04002053 RID: 8275
		EndWing,
		// Token: 0x04002054 RID: 8276
		Turbine,
		// Token: 0x04002055 RID: 8277
		Dead
	}
}
