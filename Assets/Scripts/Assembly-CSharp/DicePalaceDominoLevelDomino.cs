using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
public class DicePalaceDominoLevelDomino : LevelProperties.DicePalaceDomino.Entity
{
	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06001C54 RID: 7252 RVA: 0x0010387E File Offset: 0x00101C7E
	// (set) Token: 0x06001C55 RID: 7253 RVA: 0x00103886 File Offset: 0x00101C86
	public DicePalaceDominoLevelDomino.State state { get; private set; }

	// Token: 0x06001C56 RID: 7254 RVA: 0x0010388F File Offset: 0x00101C8F
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x001038C5 File Offset: 0x00101CC5
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x001038DD File Offset: 0x00101CDD
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x001038F0 File Offset: 0x00101CF0
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x00103910 File Offset: 0x00101D10
	public override void LevelInit(LevelProperties.DicePalaceDomino properties)
	{
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.OnDeath;
		base.transform.parent.GetComponent<DicePalaceDominoLevelDominoSwing>().InitSwing(properties);
		this.happyAttackAngleIndex = UnityEngine.Random.Range(0, properties.CurrentState.bouncyBall.angleString.Split(new char[]
		{
			','
		}).Length);
		this.happyAttackDirectionIndex = UnityEngine.Random.Range(0, properties.CurrentState.bouncyBall.upDownString.Split(new char[]
		{
			','
		}).Length);
		this.happyAttackBallTypePattern = properties.CurrentState.bouncyBall.projectileTypeString.Split(new char[]
		{
			','
		});
		this.happyAttackBallTypeIndex = UnityEngine.Random.Range(0, this.happyAttackBallTypePattern.Length);
		this.happyAttackDelay = properties.CurrentState.bouncyBall.attackDelayRange.RandomFloat();
		this.sadAttackBoomerangTypeIndex = UnityEngine.Random.Range(0, properties.CurrentState.boomerang.boomerangTypeString.Split(new char[]
		{
			','
		}).Length);
		this.sadAttackDelay = properties.CurrentState.boomerang.attackDelayRange.RandomFloat();
		this.floor.InitFloor(properties);
		base.LevelInit(properties);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x00103A79 File Offset: 0x00101E79
	private void OnIntroEnd()
	{
		base.animator.enabled = true;
		this.floor.StartSpawningTiles();
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x00103A94 File Offset: 0x00101E94
	private IEnumerator intro_cr()
	{
		AudioManager.PlayLoop("dice_palace_domino_intro_start_loop");
		this.emitAudioFromObject.Add("dice_palace_domino_intro_start_loop");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Intro", false);
		AudioManager.Stop("dice_palace_domino_intro_start_loop");
		AudioManager.Play("dice_palace_domino_intro");
		this.emitAudioFromObject.Add("dice_palace_domino_intro");
		this.state = DicePalaceDominoLevelDomino.State.Idle;
		yield break;
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x00103AAF File Offset: 0x00101EAF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bouncyBallPrefab = null;
		this.boomerangPrefab = null;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x00103AC5 File Offset: 0x00101EC5
	public void OnBouncyBall()
	{
		base.StartCoroutine(this.bouncyBall_cr());
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x00103AD4 File Offset: 0x00101ED4
	private IEnumerator bouncyBall_cr()
	{
		this.state = DicePalaceDominoLevelDomino.State.BouncyBall;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bouncyBall.initialAttackDelay);
		base.animator.SetTrigger("OnProjectile");
		yield return base.animator.WaitForAnimationToStart(this, "Projectile_Attack", false);
		AudioManager.Play("dice_palace_domino_projectile_attack");
		this.emitAudioFromObject.Add("dice_palace_domino_projectile_attack");
		yield return base.animator.WaitForAnimationToEnd(this, "Projectile_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.happyAttackDelay);
		this.state = DicePalaceDominoLevelDomino.State.Idle;
		yield break;
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x00103AEF File Offset: 0x00101EEF
	public void SpawnBall()
	{
		base.StartCoroutine(this.spawn_ball_cr());
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x00103B00 File Offset: 0x00101F00
	private IEnumerator spawn_ball_cr()
	{
		float angle = (float)Parser.IntParse(base.properties.CurrentState.bouncyBall.angleString.Split(new char[]
		{
			','
		})[this.happyAttackAngleIndex]);
		if (base.properties.CurrentState.bouncyBall.upDownString.Split(new char[]
		{
			','
		})[this.happyAttackDirectionIndex][0] == 'U')
		{
			angle = -angle;
		}
		Vector3 direction = Vector3.left;
		direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
		AbstractProjectile proj = this.bouncyBallPrefab.Create(this.bouncySpawnpoint.position);
		proj.SetParryable(this.happyAttackBallTypePattern[this.happyAttackBallTypeIndex][0] == 'P');
		proj.GetComponent<DicePalaceDominoLevelBouncyBall>().InitBouncyBall(base.properties.CurrentState.bouncyBall.bulletSpeed, direction);
		this.happyAttackAngleIndex++;
		if (this.happyAttackAngleIndex >= base.properties.CurrentState.bouncyBall.angleString.Split(new char[]
		{
			','
		}).Length)
		{
			this.happyAttackAngleIndex = 0;
		}
		this.happyAttackDirectionIndex++;
		if (this.happyAttackDirectionIndex >= base.properties.CurrentState.bouncyBall.upDownString.Split(new char[]
		{
			','
		}).Length)
		{
			this.happyAttackDirectionIndex = 0;
		}
		this.happyAttackBallTypeIndex++;
		if (this.happyAttackBallTypeIndex >= this.happyAttackBallTypePattern.Length)
		{
			this.happyAttackBallTypeIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x00103B1B File Offset: 0x00101F1B
	public void OnBoomerang()
	{
		base.StartCoroutine(this.boomerang_cr());
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x00103B2C File Offset: 0x00101F2C
	private IEnumerator boomerang_cr()
	{
		this.state = DicePalaceDominoLevelDomino.State.Boomerang;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boomerang.initialAttackDelay);
		base.animator.SetTrigger("OnBird");
		yield return base.animator.WaitForAnimationToEnd(this, "Bird_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.sadAttackDelay);
		this.state = DicePalaceDominoLevelDomino.State.Idle;
		yield break;
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x00103B47 File Offset: 0x00101F47
	public void SpawnBoomerang()
	{
		base.StartCoroutine(this.spawn_boomerang_cr());
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x00103B58 File Offset: 0x00101F58
	private IEnumerator spawn_boomerang_cr()
	{
		LevelProperties.DicePalaceDomino.Boomerang p = base.properties.CurrentState.boomerang;
		if (base.properties.CurrentState.boomerang.boomerangTypeString.Split(new char[]
		{
			','
		})[this.sadAttackBoomerangTypeIndex][0] == 'R')
		{
			DicePalaceDominoLevelBoomerang proj = this.boomerangPrefab.Create(this.birdSpawnpoint.position, p.boomerangSpeed, p.health);
		}
		else
		{
			DicePalaceDominoLevelBoomerang proj = this.boomerangPrefab.Create(this.birdSpawnpoint.position, p.boomerangSpeed, p.health);
			proj.GetComponent<SpriteRenderer>().color = Color.magenta;
			proj.SetParryable(true);
		}
		this.sadAttackBoomerangTypeIndex++;
		if (this.sadAttackBoomerangTypeIndex >= base.properties.CurrentState.boomerang.boomerangTypeString.Split(new char[]
		{
			','
		}).Length)
		{
			this.sadAttackBoomerangTypeIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x00103B73 File Offset: 0x00101F73
	private void OnDeath()
	{
		AudioManager.PlayLoop("dice_palace_domino_death_start_loop");
		this.emitAudioFromObject.Add("dice_palace_domino_death_start_loop");
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x00103B9F File Offset: 0x00101F9F
	private void EndDeathLoop()
	{
		AudioManager.Stop("dice_palace_domino_death_start_loop");
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x00103BAB File Offset: 0x00101FAB
	private void DeathSFX()
	{
		AudioManager.Play("dice_palace_domino_death");
		this.emitAudioFromObject.Add("dice_palace_domino_death");
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x00103BC7 File Offset: 0x00101FC7
	private void BirdAttackSFX()
	{
		AudioManager.Play("dice_palace_domino_bird_attack");
		this.emitAudioFromObject.Add("dice_palace_domino_bird_attack");
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x00103BE3 File Offset: 0x00101FE3
	private void SwingForwardSFX()
	{
		AudioManager.Play("swing_forward");
		this.emitAudioFromObject.Add("swing_forward");
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x00103BFF File Offset: 0x00101FFF
	private void SwingBackSFX()
	{
		AudioManager.Play("swing_back");
		this.emitAudioFromObject.Add("swing_back");
	}

	// Token: 0x04002555 RID: 9557
	[SerializeField]
	private Transform bouncySpawnpoint;

	// Token: 0x04002556 RID: 9558
	[SerializeField]
	private Transform birdSpawnpoint;

	// Token: 0x04002557 RID: 9559
	[SerializeField]
	private DicePalaceDominoLevelBouncyBall bouncyBallPrefab;

	// Token: 0x04002558 RID: 9560
	[SerializeField]
	private DicePalaceDominoLevelBoomerang boomerangPrefab;

	// Token: 0x04002559 RID: 9561
	[SerializeField]
	private DicePalaceDominoLevelFloor floor;

	// Token: 0x0400255A RID: 9562
	private int happyAttackAngleIndex;

	// Token: 0x0400255B RID: 9563
	private int happyAttackDirectionIndex;

	// Token: 0x0400255C RID: 9564
	private string[] happyAttackBallTypePattern;

	// Token: 0x0400255D RID: 9565
	private int happyAttackBallTypeIndex;

	// Token: 0x0400255E RID: 9566
	private float happyAttackDelay;

	// Token: 0x0400255F RID: 9567
	private int sadAttackBoomerangTypeIndex;

	// Token: 0x04002560 RID: 9568
	private float sadAttackDelay;

	// Token: 0x04002561 RID: 9569
	private DamageDealer damageDealer;

	// Token: 0x04002562 RID: 9570
	private DamageReceiver damageReceiver;

	// Token: 0x020005B6 RID: 1462
	public enum State
	{
		// Token: 0x04002565 RID: 9573
		Idle,
		// Token: 0x04002566 RID: 9574
		Boomerang,
		// Token: 0x04002567 RID: 9575
		BouncyBall
	}
}
