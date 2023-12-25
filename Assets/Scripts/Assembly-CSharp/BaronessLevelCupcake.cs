using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public class BaronessLevelCupcake : BaronessLevelMiniBossBase
{
	// Token: 0x17000320 RID: 800
	// (get) Token: 0x060015E1 RID: 5601 RVA: 0x000C4529 File Offset: 0x000C2929
	// (set) Token: 0x060015E2 RID: 5602 RVA: 0x000C4531 File Offset: 0x000C2931
	public BaronessLevelCupcake.State state { get; private set; }

	// Token: 0x060015E3 RID: 5603 RVA: 0x000C453C File Offset: 0x000C293C
	protected override void Awake()
	{
		base.Awake();
		this.isGoingDown = false;
		this.isGoingRight = false;
		this.xSpeed = this.changeXSpeed;
		this.patternIndex = 0;
		this.fadeTime = 0.3f;
		this.damageDealer = DamageDealer.NewEnemy();
		this.collisionChild.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.collisionChild.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x000C45C0 File Offset: 0x000C29C0
	protected void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x000C45D8 File Offset: 0x000C29D8
	public void Init(LevelProperties.Baroness.Cupcake properties, Vector2 pos, float health)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.health = health;
		this.state = BaronessLevelCupcake.State.Moving;
		base.StartCoroutine(this.select_x_speed_cr());
		base.StartCoroutine(this.moving_cr());
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x000C4628 File Offset: 0x000C2A28
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health < 0f && this.state != BaronessLevelCupcake.State.Dying)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			this.state = BaronessLevelCupcake.State.Dying;
			this.StartDeath();
		}
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x000C46AD File Offset: 0x000C2AAD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x000C46CB File Offset: 0x000C2ACB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.splashPrefab = null;
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x000C46DA File Offset: 0x000C2ADA
	protected override float hitPauseCoefficient()
	{
		return (!this.collisionChild.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x000C4700 File Offset: 0x000C2B00
	private void SetLaunchOffset()
	{
		base.transform.position = this.launchOffset.transform.position;
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x000C4720 File Offset: 0x000C2B20
	private IEnumerator moving_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		bool curlAni = false;
		bool flatAni = false;
		for (;;)
		{
			if (!this.isGoingDown)
			{
				if (flatAni)
				{
					base.StartCoroutine(this.select_x_speed_cr());
					yield return CupheadTime.WaitForSeconds(this, this.properties.hold);
					base.animator.SetTrigger("Continue");
					yield return base.animator.WaitForAnimationToEnd(this, "Slam_Start", false, true);
					flatAni = false;
				}
				this.GoingUp();
				curlAni = true;
			}
			else
			{
				if (curlAni)
				{
					yield return base.animator.WaitForAnimationToEnd(this, "Slam_Curl", false, true);
					curlAni = false;
				}
				this.GoingDown();
				flatAni = true;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x000C473C File Offset: 0x000C2B3C
	private void GoingUp()
	{
		if (base.transform.position.y < 360f - this.offset)
		{
			Vector3 position = base.transform.position;
			if (!this.isGoingRight)
			{
				position.x -= this.changeXSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			}
			else
			{
				position.x += this.changeXSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			}
			position.y += this.ySpeedUp * CupheadTime.FixedDelta;
			base.transform.position = position;
			this.BoundaryCheck();
		}
		else
		{
			this.isGoingDown = true;
		}
	}

	// Token: 0x060015ED RID: 5613 RVA: 0x000C4804 File Offset: 0x000C2C04
	private void GoingDown()
	{
		if (base.transform.position.y > (float)Level.Current.Ground + 120f)
		{
			if (this.xSpeed == 0f)
			{
				this.xSpeed = this.changeXSpeed;
			}
			Vector3 position = base.transform.position;
			position.y -= this.ySpeedDown * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			base.transform.position = position;
		}
		else
		{
			Vector3 position2 = base.transform.position;
			position2.y = (float)Level.Current.Ground + 120f;
			base.transform.position = position2;
			this.isGoingDown = false;
		}
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x000C48CC File Offset: 0x000C2CCC
	private void BoundaryCheck()
	{
		if (base.transform.position.x < -540f && !this.isGoingRight)
		{
			this.xSpeed = 0f;
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
			this.isGoingRight = true;
		}
		else if (base.transform.position.x > 540f && this.isGoingRight)
		{
			this.xSpeed = 0f;
			base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
			this.isGoingRight = false;
		}
		else
		{
			this.xSpeed = this.changeXSpeed;
		}
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x000C49B8 File Offset: 0x000C2DB8
	private IEnumerator select_x_speed_cr()
	{
		string[] pattern = this.properties.XSpeedString[0].Split(new char[]
		{
			','
		});
		Parser.FloatTryParse(pattern[this.patternIndex], out this.changeXSpeed);
		if (this.patternIndex < pattern.Length - 1)
		{
			this.patternIndex++;
		}
		else
		{
			this.patternIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x000C49D3 File Offset: 0x000C2DD3
	private void FireBullets()
	{
		if (this.properties.projectileOn)
		{
			this.StartSplashes();
		}
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x000C49EC File Offset: 0x000C2DEC
	private void StartSplashes()
	{
		base.StartCoroutine(this.splash_cr(true, this.deathRoot.transform.position.x));
		base.StartCoroutine(this.splash_cr(false, this.deathRoot.transform.position.x));
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x000C4A48 File Offset: 0x000C2E48
	private IEnumerator splash_cr(bool onLeft, float posX)
	{
		float originalOffset = (!onLeft) ? (-this.properties.splashOriginalOffset) : this.properties.splashOriginalOffset;
		float offset = (!onLeft) ? (-this.properties.splashOffset) : this.properties.splashOffset;
		float delay = 0.4f;
		int value = 0;
		for (int i = 0; i < 3; i++)
		{
			if (onLeft)
			{
				value = i;
			}
			else if (i == 0)
			{
				value = 2;
			}
			else if (i == 1)
			{
				value = 0;
			}
			else
			{
				value = 1;
			}
			Effect splash = this.splashPrefab.Create(new Vector2(posX + originalOffset + offset * (float)i, (float)Level.Current.Ground));
			float scale = (!onLeft) ? splash.transform.localScale.x : (-splash.transform.localScale.x);
			splash.animator.SetInteger("SplashType", value);
			splash.transform.SetScale(new float?(scale), null, null);
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		yield break;
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x000C4A71 File Offset: 0x000C2E71
	private void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x000C4A88 File Offset: 0x000C2E88
	private IEnumerator death_cr()
	{
		this.StartExplosions();
		this.collisionChild.GetComponent<Collider2D>().enabled = false;
		base.transform.position = this.deathRoot.transform.position;
		this.isDying = true;
		base.animator.SetTrigger("Death");
		yield return base.animator.WaitForAnimationToEnd(this, "Cupcake_Death", false, true);
		this.EndExplosions();
		this.Die();
		yield break;
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x000C4AA3 File Offset: 0x000C2EA3
	private void SoundCupcakeJump()
	{
		AudioManager.Play("level_baroness_cupcake_jump");
		this.emitAudioFromObject.Add("level_baroness_cupcake_jump");
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x000C4ABF File Offset: 0x000C2EBF
	private void SoundCupcakeLand()
	{
		AudioManager.Play("level_baroness_cupcake_land");
		this.emitAudioFromObject.Add("level_baroness_cupcake_land");
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x000C4ADB File Offset: 0x000C2EDB
	private void SoundCupcakeSpin()
	{
		AudioManager.Play("level_baroness_cupcake_spin");
		this.emitAudioFromObject.Add("level_baroness_cupcake_spin");
	}

	// Token: 0x04001F33 RID: 7987
	private LevelProperties.Baroness.Cupcake properties;

	// Token: 0x04001F34 RID: 7988
	private DamageDealer damageDealer;

	// Token: 0x04001F35 RID: 7989
	[SerializeField]
	private Effect splashPrefab;

	// Token: 0x04001F36 RID: 7990
	[SerializeField]
	private Transform launchOffset;

	// Token: 0x04001F37 RID: 7991
	[SerializeField]
	private BasicProjectile cupcakeProjectile;

	// Token: 0x04001F38 RID: 7992
	[SerializeField]
	private Transform collisionChild;

	// Token: 0x04001F39 RID: 7993
	[SerializeField]
	private Transform deathRoot;

	// Token: 0x04001F3A RID: 7994
	private float health;

	// Token: 0x04001F3B RID: 7995
	private float ySpeedUp = 1800f;

	// Token: 0x04001F3C RID: 7996
	private float ySpeedDown = 2500f;

	// Token: 0x04001F3D RID: 7997
	private float xSpeed;

	// Token: 0x04001F3E RID: 7998
	private float changeXSpeed;

	// Token: 0x04001F3F RID: 7999
	private float offset = 250f;

	// Token: 0x04001F40 RID: 8000
	private bool isGoingDown;

	// Token: 0x04001F41 RID: 8001
	private bool isGoingRight;

	// Token: 0x04001F42 RID: 8002
	private int patternIndex;

	// Token: 0x04001F43 RID: 8003
	private int mainPatternIndex;

	// Token: 0x020004EC RID: 1260
	public enum State
	{
		// Token: 0x04001F45 RID: 8005
		Moving,
		// Token: 0x04001F46 RID: 8006
		Dying
	}
}
