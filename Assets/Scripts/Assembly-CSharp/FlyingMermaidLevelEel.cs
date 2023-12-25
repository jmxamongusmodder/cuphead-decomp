using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000683 RID: 1667
public class FlyingMermaidLevelEel : AbstractCollidableObject
{
	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06002329 RID: 9001 RVA: 0x0014A37F File Offset: 0x0014877F
	// (set) Token: 0x0600232A RID: 9002 RVA: 0x0014A387 File Offset: 0x00148787
	public FlyingMermaidLevelEel.State state { get; private set; }

	// Token: 0x0600232B RID: 9003 RVA: 0x0014A390 File Offset: 0x00148790
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x0014A3C6 File Offset: 0x001487C6
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x0014A3DE File Offset: 0x001487DE
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state == FlyingMermaidLevelEel.State.Spawned)
		{
			this.Die(true, false);
		}
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x0014A418 File Offset: 0x00148818
	public void Die(bool explode, bool permanent)
	{
		Collider2D component = base.GetComponent<Collider2D>();
		if (!component.enabled)
		{
			return;
		}
		this.StopAllCoroutines();
		component.enabled = false;
		base.animator.SetTrigger("Despawn");
		base.animator.ResetTrigger("Attack");
		base.animator.ResetTrigger("Continue");
		base.animator.ResetTrigger("Leave");
		float num = (float)((!(base.GetComponent<SpriteRenderer>().sortingLayerName == "Foreground")) ? -270 : -380);
		if (explode && this.state == FlyingMermaidLevelEel.State.Spawned)
		{
			SpriteRenderer component2 = base.GetComponent<SpriteRenderer>();
			for (int i = 0; i < this.numSegments; i++)
			{
				float floatAt = this.segmentY.GetFloatAt((float)i / ((float)this.numSegments - 1f));
				Vector2 position = base.transform.position;
				position.y += floatAt;
				if (position.y >= num + 30f)
				{
					FlyingMermaidLevelEelSegment flyingMermaidLevelEelSegment;
					if (i == this.numSegments - 1)
					{
						flyingMermaidLevelEelSegment = this.headSegmentPrefab;
					}
					else
					{
						flyingMermaidLevelEelSegment = this.bodySegmentPrefabs.RandomChoice<FlyingMermaidLevelEelSegment>();
					}
					string text = component2.sortingLayerName;
					int sortingOrder = component2.sortingOrder;
					int num2 = UnityEngine.Random.Range(-1, 2);
					if (text == "Foreground")
					{
						if (num2 == -1)
						{
							text = "Enemies";
							sortingOrder = 1000;
						}
						else if (num2 == 1)
						{
							sortingOrder = 21;
						}
					}
					else if (num2 == -1)
					{
						text = "Background";
						sortingOrder = 75;
					}
					else if (num2 == 1)
					{
						text = "Foreground";
						sortingOrder = 1;
					}
					flyingMermaidLevelEelSegment.Create(position, text, sortingOrder);
				}
			}
		}
		if (!permanent)
		{
			base.StartCoroutine(this.main_cr());
		}
		this.state = FlyingMermaidLevelEel.State.Unspawned;
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x0014A60A File Offset: 0x00148A0A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x0014A628 File Offset: 0x00148A28
	public void Init(LevelProperties.FlyingMermaid.Eel properties)
	{
		this.properties = properties;
		this.initialY = base.transform.localPosition.y;
		Collider2D component = base.GetComponent<Collider2D>();
		component.enabled = false;
		this.bulletPinkPattern = properties.bulletPinkString.Split(new char[]
		{
			','
		});
		this.bulletPinkIndex = UnityEngine.Random.Range(0, this.bulletPinkPattern.Length);
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x0014A693 File Offset: 0x00148A93
	public void StartPattern()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x0014A6A4 File Offset: 0x00148AA4
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.appearDelay.RandomFloat());
		this.state = FlyingMermaidLevelEel.State.Spawned;
		base.animator.SetTrigger("Spawn");
		AudioManager.Play("level_mermaid_eel_intro");
		float t = 0f;
		this.hp = this.properties.hp;
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = true;
		while (t < this.riseTime - 0.25f)
		{
			t += CupheadTime.Delta;
			base.transform.SetLocalPosition(null, new float?(Mathf.Lerp(this.initialY - this.riseDistance, this.initialY, t / this.riseTime)), null);
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		while (t < this.riseTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetLocalPosition(null, new float?(Mathf.Lerp(this.initialY - this.riseDistance, this.initialY, t / this.riseTime)), null);
			yield return null;
		}
		base.transform.SetLocalPosition(null, new float?(this.initialY), null);
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		for (int numAttacks = this.properties.attackAmount.RandomInt(); numAttacks >= 0; numAttacks--)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.idleTime.RandomFloat());
			AudioManager.Play("level_mermaid_eel_attack_start");
			base.animator.SetTrigger("Attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Start", false, true);
			this.FireProjectiles();
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_End", false, true);
			AudioManager.Play("level_mermaid_eel_attack_end");
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.idleTime.RandomFloat());
		base.animator.SetTrigger("Leave");
		yield return base.animator.WaitForAnimationToEnd(this, "Leave_Start", false, true);
		AudioManager.Play("level_mermaid_eel_attack_leave");
		t = 0f;
		bool spawnedSplash = false;
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		float waterY = (float)((!(sprite.sortingLayerName == "Foreground")) ? -270 : -380);
		while (t < this.leaveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetLocalPosition(null, new float?(Mathf.Lerp(this.initialY, this.initialY - this.riseDistance, t / this.leaveTime)), null);
			if (!spawnedSplash && base.transform.position.y < waterY - 80f)
			{
				FlyingMermaidLevelSplashManager.Instance.SpawnSplashMedium(base.gameObject, 35f, true, waterY + 80f);
				spawnedSplash = true;
			}
			yield return null;
		}
		this.Die(false, false);
		yield break;
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x0014A6C0 File Offset: 0x00148AC0
	private void FireProjectiles()
	{
		int num = 0;
		while ((float)num < this.properties.numBullets)
		{
			float floatAt = this.properties.spreadAngle.GetFloatAt((float)num / (this.properties.numBullets - 1f));
			BasicProjectile basicProjectile = this.projectilePrefab.Create(this.projectileRoot.position, floatAt, this.properties.bulletSpeed);
			basicProjectile.SetParryable(this.bulletPinkPattern[this.bulletPinkIndex][0] == 'P');
			this.bulletPinkIndex = (this.bulletPinkIndex + 1) % this.bulletPinkPattern.Length;
			num++;
		}
	}

	// Token: 0x04002BCE RID: 11214
	[SerializeField]
	private float riseTime;

	// Token: 0x04002BCF RID: 11215
	[SerializeField]
	private float riseDistance;

	// Token: 0x04002BD0 RID: 11216
	[SerializeField]
	private float leaveTime;

	// Token: 0x04002BD1 RID: 11217
	[SerializeField]
	private MinMax segmentY;

	// Token: 0x04002BD2 RID: 11218
	[SerializeField]
	private int numSegments;

	// Token: 0x04002BD3 RID: 11219
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x04002BD4 RID: 11220
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04002BD5 RID: 11221
	[SerializeField]
	private FlyingMermaidLevelEelSegment headSegmentPrefab;

	// Token: 0x04002BD6 RID: 11222
	[SerializeField]
	private FlyingMermaidLevelEelSegment[] bodySegmentPrefabs;

	// Token: 0x04002BD7 RID: 11223
	private LevelProperties.FlyingMermaid.Eel properties;

	// Token: 0x04002BD8 RID: 11224
	private DamageDealer damageDealer;

	// Token: 0x04002BD9 RID: 11225
	private DamageReceiver damageReceiver;

	// Token: 0x04002BDA RID: 11226
	private string[] bulletPinkPattern;

	// Token: 0x04002BDB RID: 11227
	private int bulletPinkIndex;

	// Token: 0x04002BDC RID: 11228
	private float initialY;

	// Token: 0x04002BDD RID: 11229
	private float hp;

	// Token: 0x02000684 RID: 1668
	public enum State
	{
		// Token: 0x04002BDF RID: 11231
		Unspawned,
		// Token: 0x04002BE0 RID: 11232
		Spawned
	}
}
