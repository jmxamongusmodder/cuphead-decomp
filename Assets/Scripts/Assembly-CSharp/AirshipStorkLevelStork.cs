using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004DB RID: 1243
public class AirshipStorkLevelStork : LevelProperties.AirshipStork.Entity
{
	// Token: 0x0600153D RID: 5437 RVA: 0x000BE57F File Offset: 0x000BC97F
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x000BE5B5 File Offset: 0x000BC9B5
	private void Start()
	{
		CupheadLevelCamera.Current.StartFloat(25f, 3f);
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x000BE5CB File Offset: 0x000BC9CB
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x000BE5E3 File Offset: 0x000BC9E3
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x000BE5F8 File Offset: 0x000BC9F8
	public override void LevelInit(LevelProperties.AirshipStork properties)
	{
		base.LevelInit(properties);
		this.knobSwitch = AirshipLevelKnob.Create(this.knobSprite.transform);
		this.knobSwitch.OnActivate += this.OnKnobParry;
		LevelProperties.AirshipStork.Main main = properties.CurrentState.main;
		Vector3 position = base.transform.position;
		position.y = main.headHeight;
		base.transform.position = position;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x000BE677 File Offset: 0x000BCA77
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x000BE68E File Offset: 0x000BCA8E
	private void OnKnobParry()
	{
		base.properties.DealDamage(base.properties.CurrentState.main.parryDamage);
		base.StartCoroutine(this.hurt_cr());
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x000BE6C0 File Offset: 0x000BCAC0
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 5f);
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.spiral_shot_cr());
		base.StartCoroutine(this.babies_cr());
		yield break;
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x000BE6DC File Offset: 0x000BCADC
	private IEnumerator hurt_cr()
	{
		this.knobSwitch.enabled = false;
		this.knobSprite.GetComponent<SpriteRenderer>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.main.pinkDurationOff);
		this.knobSwitch.enabled = true;
		this.knobSprite.GetComponent<SpriteRenderer>().enabled = true;
		yield break;
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x000BE6F8 File Offset: 0x000BCAF8
	private IEnumerator move_cr()
	{
		float offset = 220f;
		float moveTime = 0f;
		LevelProperties.AirshipStork.Main p = base.properties.CurrentState.main;
		string[] leftMovementPattern = p.leftMovementTime.GetRandom<string>().Split(new char[]
		{
			','
		});
		Vector3 pos = base.transform.position;
		Parser.FloatTryParse(leftMovementPattern[this.index], out moveTime);
		float t = 0f;
		for (;;)
		{
			if (this.farRight)
			{
				while (t < moveTime)
				{
					base.transform.position -= base.transform.right * (p.movementSpeed * CupheadTime.Delta);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				this.farRight = !this.farRight;
			}
			else
			{
				while (base.transform.position.x < 640f - offset)
				{
					pos.x = Mathf.MoveTowards(base.transform.position.x, 640f - offset, p.movementSpeed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
				this.farRight = !this.farRight;
			}
			moveTime = (moveTime + 1f) % (float)leftMovementPattern.Length;
		}
		yield break;
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x000BE714 File Offset: 0x000BCB14
	private IEnumerator spiral_shot_cr()
	{
		LevelProperties.AirshipStork.SpiralShot p = base.properties.CurrentState.spiralShot;
		string[] pinkPattern = p.pinkString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] delayPattern = p.shotDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] directionPattern = p.spiralDirection.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, delayPattern.Length);
		int pinkIndex = UnityEngine.Random.Range(0, pinkPattern.Length);
		int directionIndex = UnityEngine.Random.Range(0, directionPattern.Length);
		float seconds = 0f;
		int direction = 0;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, seconds);
			Parser.FloatTryParse(delayPattern[delayIndex], out seconds);
			Parser.IntTryParse(directionPattern[directionIndex], out direction);
			if (pinkPattern[pinkIndex][0] == 'R')
			{
				this.projectile.Create(this.projectileRoot.transform.position, 0f, p.movementSpeed, p.spiralRate, direction);
			}
			else if (pinkPattern[pinkIndex][0] == 'P')
			{
				this.projectilePink.Create(this.projectileRoot.transform.position, 0f, p.movementSpeed, p.spiralRate, direction);
			}
			pinkIndex = (pinkIndex + 1) % pinkPattern.Length;
			delayIndex = (delayIndex + 1) % delayPattern.Length;
			directionIndex = (directionIndex + 1) % directionPattern.Length;
		}
		yield break;
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x000BE730 File Offset: 0x000BCB30
	private IEnumerator babies_cr()
	{
		LevelProperties.AirshipStork.Babies p = base.properties.CurrentState.babies;
		string[] delayPattern = p.babyDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int index = UnityEngine.Random.Range(0, delayPattern.Length);
		float delay = 0f;
		for (;;)
		{
			Parser.FloatTryParse(delayPattern[index], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			Vector2 pos = base.transform.position;
			pos.y = (float)Level.Current.Ground;
			pos.x = (float)Level.Current.Right;
			AirshipStorkLevelBaby baby = UnityEngine.Object.Instantiate<AirshipStorkLevelBaby>(this.babyPrefab);
			baby.Init(p, pos, p.HP);
			yield return null;
			index = (index + 1) % delayPattern.Length;
		}
		yield break;
	}

	// Token: 0x04001E9A RID: 7834
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04001E9B RID: 7835
	[SerializeField]
	private Transform knobSprite;

	// Token: 0x04001E9C RID: 7836
	[SerializeField]
	private AirshipStorkLevelProjectile projectile;

	// Token: 0x04001E9D RID: 7837
	[SerializeField]
	private AirshipStorkLevelProjectile projectilePink;

	// Token: 0x04001E9E RID: 7838
	[SerializeField]
	private AirshipStorkLevelBaby babyPrefab;

	// Token: 0x04001E9F RID: 7839
	private bool farRight = true;

	// Token: 0x04001EA0 RID: 7840
	private int index;

	// Token: 0x04001EA1 RID: 7841
	private DamageDealer damageDealer;

	// Token: 0x04001EA2 RID: 7842
	private DamageReceiver damageReceiver;

	// Token: 0x04001EA3 RID: 7843
	private AirshipLevelKnob knobSwitch;
}
