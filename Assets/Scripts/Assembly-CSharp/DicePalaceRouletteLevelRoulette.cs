using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
public class DicePalaceRouletteLevelRoulette : LevelProperties.DicePalaceRoulette.Entity
{
	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06001DEA RID: 7658 RVA: 0x001130A0 File Offset: 0x001114A0
	// (set) Token: 0x06001DEB RID: 7659 RVA: 0x001130A8 File Offset: 0x001114A8
	public DicePalaceRouletteLevelRoulette.State state { get; private set; }

	// Token: 0x06001DEC RID: 7660 RVA: 0x001130B1 File Offset: 0x001114B1
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x001130E7 File Offset: 0x001114E7
	private void Start()
	{
		this.state = DicePalaceRouletteLevelRoulette.State.Intro;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x001130FD File Offset: 0x001114FD
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x00113115 File Offset: 0x00111515
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x00113134 File Offset: 0x00111534
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != DicePalaceRouletteLevelRoulette.State.Death)
		{
			this.state = DicePalaceRouletteLevelRoulette.State.Death;
			this.StartDeath();
		}
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x00113180 File Offset: 0x00111580
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		AudioManager.Play("dice_palace_roulette_intro");
		this.emitAudioFromObject.Add("dice_palace_roulette_intro");
		base.animator.Play("Roulette_Intro");
		this.state = DicePalaceRouletteLevelRoulette.State.Idle;
		yield break;
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x0011319B File Offset: 0x0011159B
	protected virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x001131BC File Offset: 0x001115BC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.marble = null;
		this.marbleLaunch = null;
	}

	// Token: 0x06001DF4 RID: 7668 RVA: 0x001131D2 File Offset: 0x001115D2
	public void StartTwirl()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.twirl_cr());
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x00113200 File Offset: 0x00111600
	private IEnumerator twirl_cr()
	{
		this.state = DicePalaceRouletteLevelRoulette.State.Twirl;
		base.animator.Play("Roulette_Travel");
		LevelProperties.DicePalaceRoulette.Twirl p = base.properties.CurrentState.twirl;
		string[] amountPattern = p.twirlAmount.GetRandom<string>().Split(new char[]
		{
			','
		});
		base.StartCoroutine(this.twirl_vary_speed_cr());
		float twirlAmount = 0f;
		float stopDist = 200f;
		Parser.FloatTryParse(amountPattern[this.index], out twirlAmount);
		Vector3 pos = base.transform.position;
		int i = 0;
		while ((float)i < twirlAmount)
		{
			if (this.onRight)
			{
				this.slowDown = false;
				float maxPoint = -630f;
				while (base.transform.position.x > maxPoint)
				{
					if (!this.stopTwirl)
					{
						float num = maxPoint - base.transform.position.x;
						num = Mathf.Abs(num);
						pos.x = Mathf.MoveTowards(base.transform.position.x, maxPoint, this.speed * CupheadTime.Delta * this.hitPauseCoefficient());
						if (num < stopDist)
						{
							this.slowDown = true;
						}
						base.transform.position = pos;
					}
					yield return null;
				}
				this.onRight = !this.onRight;
			}
			else
			{
				this.slowDown = false;
				float maxPoint2 = 490f;
				while (base.transform.position.x < maxPoint2)
				{
					if (!this.stopTwirl)
					{
						float num2 = maxPoint2 - base.transform.position.x;
						num2 = Mathf.Abs(num2);
						pos.x = Mathf.MoveTowards(base.transform.position.x, maxPoint2, this.speed * CupheadTime.Delta * this.hitPauseCoefficient());
						if (num2 < stopDist)
						{
							this.slowDown = true;
						}
						base.transform.position = pos;
					}
					yield return null;
				}
				this.onRight = !this.onRight;
			}
			i++;
		}
		twirlAmount = (twirlAmount + 1f) % (float)amountPattern.Length;
		base.StopCoroutine(this.twirl_vary_speed_cr());
		this.state = DicePalaceRouletteLevelRoulette.State.Idle;
		yield break;
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x0011321B File Offset: 0x0011161B
	private void TwirlStop()
	{
		this.stopTwirl = true;
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x00113224 File Offset: 0x00111624
	private void TwirlStart()
	{
		this.stopTwirl = false;
	}

	// Token: 0x06001DF8 RID: 7672 RVA: 0x00113230 File Offset: 0x00111630
	private IEnumerator twirl_vary_speed_cr()
	{
		LevelProperties.DicePalaceRoulette.Twirl p = base.properties.CurrentState.twirl;
		float incrementspeed = p.movementSpeed / 50f;
		for (;;)
		{
			if (this.slowDown)
			{
				if (this.speed <= 50f)
				{
					this.slowDown = false;
				}
				else
				{
					this.speed -= incrementspeed;
				}
			}
			else if (this.speed < p.movementSpeed)
			{
				this.speed += incrementspeed;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001DF9 RID: 7673 RVA: 0x0011324B File Offset: 0x0011164B
	public void StartMarbleDrop()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.marble_drop_cr());
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x00113278 File Offset: 0x00111678
	private void SpawnMarble(float xOffset)
	{
		LevelProperties.DicePalaceRoulette.MarbleDrop marbleDrop = base.properties.CurrentState.marbleDrop;
		float rotation = Mathf.Atan2((float)Level.Current.Ground, 0f) * 57.29578f;
		Vector2 position = base.transform.position;
		position.y = 360f;
		position.x = ((!this.onRight) ? (640f - xOffset) : (-640f + xOffset));
		this.marble.Create(position, rotation, marbleDrop.marbleSpeed);
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x00113308 File Offset: 0x00111708
	private IEnumerator marble_drop_cr()
	{
		LevelProperties.DicePalaceRoulette.MarbleDrop p = base.properties.CurrentState.marbleDrop;
		string[] spawnPattern = p.marblePositionStrings.GetRandom<string>().Split(new char[]
		{
			','
		});
		float waitTime = 0f;
		this.state = DicePalaceRouletteLevelRoulette.State.Marble;
		this.firstLaunch = true;
		this.stopMarbles = false;
		base.animator.Play("Roulette_Attack_Start");
		AudioManager.Play("dice_palace_roulette_attack_start");
		this.emitAudioFromObject.Add("dice_palace_roulette_attack_start");
		yield return base.animator.WaitForAnimationToStart(this, "Roulette_Attack_Loop", false);
		AudioManager.PlayLoop("dice_palace_roulette_attack_loop");
		this.emitAudioFromObject.Add("dice_palace_roulette_attack_loop");
		base.StartCoroutine(this.marble_sound_cr());
		yield return CupheadTime.WaitForSeconds(this, p.marbleInitalDelay);
		for (int i = 0; i < spawnPattern.Length; i++)
		{
			if (spawnPattern[i][0] == 'D')
			{
				Parser.FloatTryParse(spawnPattern[i].Substring(1), out waitTime);
				yield return CupheadTime.WaitForSeconds(this, waitTime);
			}
			else
			{
				string[] array = spawnPattern[i].Split(new char[]
				{
					'-'
				});
				foreach (string s in array)
				{
					float xOffset = 0f;
					Parser.FloatTryParse(s, out xOffset);
					this.SpawnMarble(xOffset);
				}
			}
			i %= spawnPattern.Length;
			yield return CupheadTime.WaitForSeconds(this, p.marbleDelay);
		}
		this.stopMarbles = true;
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		base.animator.SetTrigger("Continue");
		AudioManager.Stop("dice_palace_roulette_attack_loop");
		AudioManager.Play("dice_palace_roulette_attack_end");
		this.emitAudioFromObject.Add("dice_palace_roulette_attack_end");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.state = DicePalaceRouletteLevelRoulette.State.Idle;
		yield break;
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x00113324 File Offset: 0x00111724
	public void SpawnMarbleAnimation()
	{
		if (this.stopMarbles)
		{
			return;
		}
		DicePalaceRouletteLevelMarblesLaunch dicePalaceRouletteLevelMarblesLaunch = UnityEngine.Object.Instantiate<DicePalaceRouletteLevelMarblesLaunch>(this.marbleLaunch, this.marbleRoot, false);
		dicePalaceRouletteLevelMarblesLaunch.IsFirstTime = this.firstLaunch;
		this.firstLaunch = false;
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x00113364 File Offset: 0x00111764
	private IEnumerator marble_sound_cr()
	{
		AudioManager.Play("dice_palace_roulette_balls_start");
		this.emitAudioFromObject.Add("dice_palace_roulette_balls_start");
		AudioManager.PlayLoop("dice_palace_roulette_balls_shoot_loop");
		this.emitAudioFromObject.Add("dice_palace_roulette_balls_shoot_loop");
		while (!this.stopMarbles)
		{
			yield return null;
		}
		AudioManager.Stop("dice_palace_roulette_balls_shoot_loop");
		AudioManager.Play("dice_palace_roulette_balls_end");
		this.emitAudioFromObject.Add("dice_palace_roulette_balls_end");
		yield break;
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x0011337F File Offset: 0x0011177F
	private void StartDeath()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Roulette_Death");
		AudioManager.Play("dice_palace_roulette_death");
		this.emitAudioFromObject.Add("dice_palace_roulette_death");
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x001133BD File Offset: 0x001117BD
	private void TravelSFX()
	{
		AudioManager.Play("dice_palace_roulette_travel");
		this.emitAudioFromObject.Add("dice_palace_roulette_travel");
	}

	// Token: 0x040026B6 RID: 9910
	[SerializeField]
	private BasicProjectile marble;

	// Token: 0x040026B7 RID: 9911
	[SerializeField]
	private DicePalaceRouletteLevelMarblesLaunch marbleLaunch;

	// Token: 0x040026B8 RID: 9912
	[SerializeField]
	private Transform marbleRoot;

	// Token: 0x040026B9 RID: 9913
	private bool onRight = true;

	// Token: 0x040026BA RID: 9914
	private bool slowDown;

	// Token: 0x040026BB RID: 9915
	private bool stopTwirl;

	// Token: 0x040026BC RID: 9916
	private bool firstLaunch = true;

	// Token: 0x040026BD RID: 9917
	private bool stopMarbles;

	// Token: 0x040026BE RID: 9918
	private int index;

	// Token: 0x040026BF RID: 9919
	private float speed;

	// Token: 0x040026C0 RID: 9920
	private DamageDealer damageDealer;

	// Token: 0x040026C1 RID: 9921
	private DamageReceiver damageReceiver;

	// Token: 0x040026C2 RID: 9922
	private Coroutine patternCoroutine;

	// Token: 0x020005E6 RID: 1510
	public enum State
	{
		// Token: 0x040026C4 RID: 9924
		Intro,
		// Token: 0x040026C5 RID: 9925
		Idle,
		// Token: 0x040026C6 RID: 9926
		Twirl,
		// Token: 0x040026C7 RID: 9927
		Marble,
		// Token: 0x040026C8 RID: 9928
		Death
	}
}
