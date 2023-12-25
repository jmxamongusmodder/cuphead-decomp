using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200068D RID: 1677
public class FlyingMermaidLevelMerdusa : LevelProperties.FlyingMermaid.Entity
{
	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06002359 RID: 9049 RVA: 0x0014BE4C File Offset: 0x0014A24C
	// (set) Token: 0x0600235A RID: 9050 RVA: 0x0014BE54 File Offset: 0x0014A254
	public FlyingMermaidLevelMerdusa.State state { get; private set; }

	// Token: 0x0600235B RID: 9051 RVA: 0x0014BE60 File Offset: 0x0014A260
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		CollisionChild collisionChild = this.blockingColliders.gameObject.AddComponent<CollisionChild>();
		collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x0014BEC5 File Offset: 0x0014A2C5
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x0014BED8 File Offset: 0x0014A2D8
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x0014BEF0 File Offset: 0x0014A2F0
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x0014BF10 File Offset: 0x0014A310
	public override void LevelInit(LevelProperties.FlyingMermaid properties)
	{
		base.LevelInit(properties);
		foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
		{
			flyingMermaidLevelEel.Init(properties.CurrentState.eel);
		}
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x0014BF68 File Offset: 0x0014A368
	public void StartIntro(Vector2 pos)
	{
		AudioManager.Play("level_mermaid_merdusa_cackle");
		base.transform.position = pos;
		base.animator.SetTrigger("Continue");
		base.StartCoroutine(this.intro_cr());
		base.StartCoroutine(this.moveBack_cr());
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x0014BFBC File Offset: 0x0014A3BC
	private IEnumerator intro_cr()
	{
		this.StartEels();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.state = FlyingMermaidLevelMerdusa.State.Idle;
		yield break;
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x0014BFD8 File Offset: 0x0014A3D8
	private IEnumerator moveBack_cr()
	{
		float startX = base.transform.position.x;
		float t = 0f;
		while (t < this.introMoveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(new float?(Mathf.Lerp(startX, startX + this.transformMoveX, t / this.introMoveTime)), null, null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x0014BFF4 File Offset: 0x0014A3F4
	private void BlinkMaybe()
	{
		this.blinks++;
		if (this.blinks >= this.maxBlinks)
		{
			this.blinks = 0;
			this.maxBlinks = UnityEngine.Random.Range(2, 4);
			this.blinkOverlaySprite.enabled = true;
		}
		else
		{
			this.blinkOverlaySprite.enabled = false;
		}
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x0014C054 File Offset: 0x0014A454
	public void StartEels()
	{
		foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
		{
			flyingMermaidLevelEel.StartPattern();
		}
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x0014C088 File Offset: 0x0014A488
	private IEnumerator eels_cr()
	{
		foreach (FlyingMermaidLevelEel prefab in this.eels)
		{
			prefab.Spawn<FlyingMermaidLevelEel>();
		}
		bool allEelsGone = false;
		while (!allEelsGone)
		{
			allEelsGone = true;
			foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
			{
				if (flyingMermaidLevelEel.state == FlyingMermaidLevelEel.State.Spawned)
				{
					allEelsGone = false;
				}
			}
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.eel.hesitateAfterAttack);
		this.state = FlyingMermaidLevelMerdusa.State.Idle;
		yield break;
	}

	// Token: 0x06002366 RID: 9062 RVA: 0x0014C0A3 File Offset: 0x0014A4A3
	public void StartZap()
	{
		this.state = FlyingMermaidLevelMerdusa.State.Zap;
		base.StartCoroutine(this.zap_cr());
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x0014C0BC File Offset: 0x0014A4BC
	private IEnumerator zap_cr()
	{
		AudioManager.Play("level_mermaid_merdusa_zap_loop_start");
		base.animator.SetTrigger("Zap");
		yield return base.animator.WaitForAnimationToEnd(this, "Zap_Start", false, true);
		this.laser.SetStoneTime(base.properties.CurrentState.zap.stoneTime);
		this.laser.animator.SetTrigger("Start");
		this.laser.transform.SetParent(null);
		AudioManager.PlayLoop("level_mermaid_merdusa_zap_loop");
		this.laser.StartLaser();
		yield return this.laser.animator.WaitForAnimationToEnd(this, "Lightning_Start", false, true);
		this.laser.animator.SetTrigger("End");
		AudioManager.Stop("level_mermaid_merdusa_zap_loop");
		AudioManager.Play("level_mermaid_merdusa_zap_loop_end");
		this.laser.StopLaser();
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Zap_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.zap.hesitateAfterAttack.RandomFloat());
		this.state = FlyingMermaidLevelMerdusa.State.Idle;
		yield break;
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x0014C0D8 File Offset: 0x0014A4D8
	public void StartTransform()
	{
		if (this.state == FlyingMermaidLevelMerdusa.State.Zap)
		{
			AudioManager.Play("level_mermaid_merdusa_zap_loop_end");
			this.laser.StopLaser();
		}
		AudioManager.Stop("level_mermaid_merdusa_zap_loop");
		this.head.StartIntro(this.headRoot.position);
		base.properties.OnBossDeath -= this.OnBossDeath;
		this.Die();
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x0014C148 File Offset: 0x0014A548
	private void Die()
	{
		List<FlyingMermaidLevelMerdusaBodyPart> list = new List<FlyingMermaidLevelMerdusaBodyPart>();
		this.StopAllCoroutines();
		AudioManager.Play("level_mermaid_merdusa_fallapart_turnstone");
		list.Add(this.bodyPrefab.Create(this.bodyRoot.position));
		list.Add(this.leftArmPrefab.Create(this.leftArmRoot.position));
		list.Add(this.rightArmPrefab.Create(this.rightArmRoot.position));
		this.head.CheckParts(list.ToArray());
		this.StopAllCoroutines();
		CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
		foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
		{
			flyingMermaidLevelEel.Die(true, true);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x0014C22C File Offset: 0x0014A62C
	private void DieEasyMode()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("Die");
		foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
		{
			flyingMermaidLevelEel.Die(true, true);
		}
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x0014C276 File Offset: 0x0014A676
	private void OnBossDeath()
	{
		if (Level.CurrentMode == Level.Mode.Easy)
		{
			this.DieEasyMode();
		}
		else
		{
			this.Die();
		}
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x0014C293 File Offset: 0x0014A693
	private void RightSplash()
	{
		this.splashRight.Create(this.splashRoot.transform.position);
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x0014C2B1 File Offset: 0x0014A6B1
	private void LeftSplash()
	{
		this.splashLeft.Create(this.splashRoot.transform.position);
	}

	// Token: 0x04002BF7 RID: 11255
	[SerializeField]
	private float introMoveTime;

	// Token: 0x04002BF8 RID: 11256
	[SerializeField]
	private float transformMoveX;

	// Token: 0x04002BF9 RID: 11257
	[SerializeField]
	private SpriteRenderer blinkOverlaySprite;

	// Token: 0x04002BFA RID: 11258
	[SerializeField]
	private Transform blockingColliders;

	// Token: 0x04002BFB RID: 11259
	[SerializeField]
	private FlyingMermaidLevelLaser laser;

	// Token: 0x04002BFC RID: 11260
	[SerializeField]
	private FlyingMermaidLevelEel[] eels;

	// Token: 0x04002BFD RID: 11261
	[SerializeField]
	private FlyingMermaidLevelMerdusaHead head;

	// Token: 0x04002BFE RID: 11262
	[SerializeField]
	private FlyingMermaidLevelMerdusaBodyPart bodyPrefab;

	// Token: 0x04002BFF RID: 11263
	[SerializeField]
	private FlyingMermaidLevelMerdusaBodyPart leftArmPrefab;

	// Token: 0x04002C00 RID: 11264
	[SerializeField]
	private FlyingMermaidLevelMerdusaBodyPart rightArmPrefab;

	// Token: 0x04002C01 RID: 11265
	[SerializeField]
	private Transform headRoot;

	// Token: 0x04002C02 RID: 11266
	[SerializeField]
	private Transform bodyRoot;

	// Token: 0x04002C03 RID: 11267
	[SerializeField]
	private Transform leftArmRoot;

	// Token: 0x04002C04 RID: 11268
	[SerializeField]
	private Transform rightArmRoot;

	// Token: 0x04002C05 RID: 11269
	[SerializeField]
	private Effect splashLeft;

	// Token: 0x04002C06 RID: 11270
	[SerializeField]
	private Effect splashRight;

	// Token: 0x04002C07 RID: 11271
	[SerializeField]
	private Transform splashRoot;

	// Token: 0x04002C08 RID: 11272
	private DamageDealer damageDealer;

	// Token: 0x04002C09 RID: 11273
	private DamageReceiver damageReceiver;

	// Token: 0x04002C0A RID: 11274
	private Vector2 startPos;

	// Token: 0x04002C0B RID: 11275
	private int blinks;

	// Token: 0x04002C0C RID: 11276
	private int maxBlinks = 3;

	// Token: 0x0200068E RID: 1678
	public enum State
	{
		// Token: 0x04002C0E RID: 11278
		Intro,
		// Token: 0x04002C0F RID: 11279
		Idle,
		// Token: 0x04002C10 RID: 11280
		Zap
	}
}
