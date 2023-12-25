using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public class AirshipCrabLevelCrab : LevelProperties.AirshipCrab.Entity
{
	// Token: 0x17000316 RID: 790
	// (get) Token: 0x060014F3 RID: 5363 RVA: 0x000BBD09 File Offset: 0x000BA109
	// (set) Token: 0x060014F4 RID: 5364 RVA: 0x000BBD11 File Offset: 0x000BA111
	public AirshipCrabLevelCrab.State state { get; private set; }

	// Token: 0x060014F5 RID: 5365 RVA: 0x000BBD1C File Offset: 0x000BA11C
	protected override void Awake()
	{
		base.Awake();
		this.gems = new List<AirshipCrabLevelGems>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.crabHitBox.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.crabHitBox.enabled = false;
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x000BBD7C File Offset: 0x000BA17C
	public override void LevelInit(LevelProperties.AirshipCrab properties)
	{
		base.LevelInit(properties);
		base.StartCoroutine(this.intro_cr());
		this.closedPos = base.transform.position;
		this.openPos = base.transform.position;
		this.openPos.y = base.transform.position.y + properties.CurrentState.main.openCrabOffsetY;
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x000BBDEE File Offset: 0x000BA1EE
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x000BBE01 File Offset: 0x000BA201
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x000BBE19 File Offset: 0x000BA219
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x000BBE30 File Offset: 0x000BA230
	private IEnumerator intro_cr()
	{
		this.state = AirshipCrabLevelCrab.State.Closed;
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.StartCoroutine(this.barnacle_cr());
		base.StartCoroutine(this.spawn_gems_cr());
		yield break;
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x000BBE4C File Offset: 0x000BA24C
	private void StateHandler()
	{
		if (this.state == AirshipCrabLevelCrab.State.Open)
		{
			base.transform.position = this.openPos;
			this.crabHitBox.enabled = true;
			if (this.stateCoroutine != null)
			{
				base.StopCoroutine(this.stateCoroutine);
			}
			this.stateCoroutine = base.StartCoroutine(this.bubbles_cr());
		}
		else if (this.state == AirshipCrabLevelCrab.State.Closed)
		{
			base.transform.position = this.closedPos;
			this.crabHitBox.enabled = false;
			if (this.stateCoroutine != null)
			{
				base.StopCoroutine(this.stateCoroutine);
			}
			this.stateCoroutine = base.StartCoroutine(this.gems_cr());
		}
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x000BBF04 File Offset: 0x000BA304
	private IEnumerator barnacle_cr()
	{
		LevelProperties.AirshipCrab.Barnicles p = base.properties.CurrentState.barnicles;
		float offsetY = p.barnicleOffsetY;
		float offsetX = p.barnicleOffsetX;
		float rotation = Mathf.Atan2(0f, (float)Level.Current.Left) * 57.29578f;
		for (;;)
		{
			int i = 0;
			while ((float)i < p.barnicleAmount)
			{
				Vector2 pos = this.barncileRoot.position;
				pos.y = this.barncileRoot.position.y + offsetY * (float)i;
				pos.x = this.barncileRoot.position.x + offsetX;
				this.barnicleProjectile.Create(pos, rotation, p.bulletSpeed);
				yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
				i++;
			}
			yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		}
		yield break;
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x000BBF20 File Offset: 0x000BA320
	private IEnumerator spawn_gems_cr()
	{
		this.state = AirshipCrabLevelCrab.State.Closed;
		LevelProperties.AirshipCrab.Gems p = base.properties.CurrentState.gems;
		string[] anglePattern = p.angleString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int angleIndex = 0;
		float angle = 0f;
		float offsetX = p.gemOffsetX;
		float offsetY = p.gemOffsetY;
		int i = 0;
		while ((float)i < p.gemAmount)
		{
			Parser.FloatTryParse(anglePattern[angleIndex], out angle);
			Vector2 pos = this.barncileRoot.position;
			pos.y = this.barncileRoot.position.y + offsetY * (float)i;
			pos.x = this.barncileRoot.position.x + offsetX;
			AirshipCrabLevelGems gem = UnityEngine.Object.Instantiate<AirshipCrabLevelGems>(this.gemProjectile);
			gem.Init(p, pos, angle);
			this.gems.Add(gem);
			angleIndex = (angleIndex + 1) % anglePattern.Length;
			yield return null;
			i++;
		}
		this.releaseAllAtOnce = false;
		base.StartCoroutine(this.gems_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x000BBF3C File Offset: 0x000BA33C
	private IEnumerator gems_cr()
	{
		LevelProperties.AirshipCrab.Gems p = base.properties.CurrentState.gems;
		string[] delayPattern = p.gemReleaseDelay.GetRandom<string>().Split(new char[]
		{
			','
		});
		float waitTime = 0f;
		float t = 0f;
		int delayIndex = 0;
		int counter = 0;
		bool checking = true;
		bool startTimer = false;
		bool resetTimer = false;
		int i = 0;
		while ((float)i < p.gemATKAmount)
		{
			if (!this.gems[i].moving)
			{
				if (!this.releaseAllAtOnce)
				{
					Parser.FloatTryParse(delayPattern[delayIndex], out waitTime);
				}
				this.gems[i].parried = false;
				this.gems[i].lastSideHit = AirshipCrabLevelGems.SideHit.None;
				this.gems[i].PickMovement();
				if (!this.releaseAllAtOnce)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
					delayIndex %= delayPattern.Length;
				}
			}
			i++;
		}
		while (checking)
		{
			int num = 0;
			while ((float)num < p.gemATKAmount)
			{
				if (this.gems[num].parried)
				{
					counter++;
				}
				if (this.gems[num].startTimer)
				{
					this.gems[num].startTimer = false;
					startTimer = true;
					resetTimer = true;
				}
				if ((float)counter == p.gemATKAmount)
				{
					checking = false;
					break;
				}
				num++;
			}
			if (startTimer)
			{
				if (resetTimer)
				{
					t = 0f;
					resetTimer = false;
				}
				if (t < p.gemHoldDuration)
				{
					t += CupheadTime.Delta;
				}
				else
				{
					this.releaseAllAtOnce = true;
					this.state = AirshipCrabLevelCrab.State.Closed;
					this.StateHandler();
					startTimer = false;
				}
			}
			counter = 0;
			yield return null;
		}
		this.releaseAllAtOnce = false;
		this.state = AirshipCrabLevelCrab.State.Open;
		this.StateHandler();
		yield break;
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x000BBF58 File Offset: 0x000BA358
	private IEnumerator bubbles_cr()
	{
		this.state = AirshipCrabLevelCrab.State.Open;
		LevelProperties.AirshipCrab.Bubbles p = base.properties.CurrentState.bubbles;
		string[] bubblePattern = p.bubbleCount.GetRandom<string>().Split(new char[]
		{
			','
		});
		int index = 0;
		base.StartCoroutine(this.bubble_timer_cr());
		while (this.state == AirshipCrabLevelCrab.State.Open)
		{
			float count;
			Parser.FloatTryParse(bubblePattern[index], out count);
			int i = 0;
			while ((float)i < count)
			{
				AirshipCrabLevelBubbles bubbles = UnityEngine.Object.Instantiate<AirshipCrabLevelBubbles>(this.bubbleProjectile);
				bubbles.Init(this.bubbleRoot.transform.position, p, p.bubbleSpeed);
				yield return CupheadTime.WaitForSeconds(this, p.bubbleRepeatDelay);
				i++;
			}
			index = (index + 1) % bubblePattern.Length;
			yield return CupheadTime.WaitForSeconds(this, p.bubbleMainDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x000BBF74 File Offset: 0x000BA374
	private IEnumerator bubble_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bubbles.openTimer);
		this.state = AirshipCrabLevelCrab.State.Closed;
		base.StopCoroutine(this.bubbles_cr());
		this.StateHandler();
		yield break;
	}

	// Token: 0x04001E4F RID: 7759
	[SerializeField]
	private Transform barncileRoot;

	// Token: 0x04001E50 RID: 7760
	[SerializeField]
	private Transform bubbleRoot;

	// Token: 0x04001E51 RID: 7761
	[SerializeField]
	private Collider2D crabHitBox;

	// Token: 0x04001E52 RID: 7762
	[SerializeField]
	private BasicProjectile barnicleProjectile;

	// Token: 0x04001E53 RID: 7763
	[SerializeField]
	private AirshipCrabLevelBubbles bubbleProjectile;

	// Token: 0x04001E54 RID: 7764
	[SerializeField]
	private AirshipCrabLevelGems gemProjectile;

	// Token: 0x04001E56 RID: 7766
	private DamageDealer damageDealer;

	// Token: 0x04001E57 RID: 7767
	private DamageReceiver damageReceiver;

	// Token: 0x04001E58 RID: 7768
	private Vector3 closedPos;

	// Token: 0x04001E59 RID: 7769
	private Vector3 openPos;

	// Token: 0x04001E5A RID: 7770
	private List<AirshipCrabLevelGems> gems;

	// Token: 0x04001E5B RID: 7771
	private Coroutine stateCoroutine;

	// Token: 0x04001E5C RID: 7772
	private bool releaseAllAtOnce;

	// Token: 0x020004D0 RID: 1232
	public enum State
	{
		// Token: 0x04001E5E RID: 7774
		Closed,
		// Token: 0x04001E5F RID: 7775
		Open,
		// Token: 0x04001E60 RID: 7776
		Dead
	}
}
