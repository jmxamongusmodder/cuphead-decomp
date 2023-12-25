using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000690 RID: 1680
public class FlyingMermaidLevelMerdusaHead : LevelProperties.FlyingMermaid.Entity
{
	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06002378 RID: 9080 RVA: 0x0014CC71 File Offset: 0x0014B071
	// (set) Token: 0x06002379 RID: 9081 RVA: 0x0014CC79 File Offset: 0x0014B079
	public FlyingMermaidLevelMerdusaHead.State state { get; private set; }

	// Token: 0x0600237A RID: 9082 RVA: 0x0014CC82 File Offset: 0x0014B082
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600237B RID: 9083 RVA: 0x0014CCB8 File Offset: 0x0014B0B8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x0014CCCB File Offset: 0x0014B0CB
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x0014CCE3 File Offset: 0x0014B0E3
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600237E RID: 9086 RVA: 0x0014CD01 File Offset: 0x0014B101
	public void StartIntro(Vector2 pos)
	{
		base.transform.position = pos;
		base.properties.OnBossDeath += this.OnBossDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x0014CD38 File Offset: 0x0014B138
	public void CheckParts(FlyingMermaidLevelMerdusaBodyPart[] parts)
	{
		base.StartCoroutine(this.check_parts_cr(parts));
	}

	// Token: 0x06002380 RID: 9088 RVA: 0x0014CD48 File Offset: 0x0014B148
	private IEnumerator check_parts_cr(FlyingMermaidLevelMerdusaBodyPart[] parts)
	{
		foreach (FlyingMermaidLevelMerdusaBodyPart part in parts)
		{
			while (!part.IsSinking)
			{
				yield return null;
			}
		}
		this.coral.speed = base.properties.CurrentState.coral.coralMoveSpeed;
		foreach (SpriteRenderer spriteRenderer in this.wave1.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.sortingLayerName = SpriteLayer.Background.ToString();
			spriteRenderer.sortingOrder = 100;
		}
		foreach (SpriteRenderer spriteRenderer2 in this.wave2.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer2.sortingLayerName = SpriteLayer.Background.ToString();
			spriteRenderer2.sortingOrder = 101;
		}
		foreach (ScrollingSpriteSpawner scrollingSpriteSpawner in this.scrollingSpritesToEnd)
		{
			scrollingSpriteSpawner.HandlePausing(true);
		}
		foreach (ScrollingSpriteSpawner scrollingSpriteSpawner2 in this.scrollingSprites)
		{
			scrollingSpriteSpawner2.StartLoop(false);
		}
		base.StartCoroutine(this.move_head_cr());
		this.state = FlyingMermaidLevelMerdusaHead.State.Idle;
		base.StartCoroutine(this.spawn_yellow_dots_cr());
		yield break;
	}

	// Token: 0x06002381 RID: 9089 RVA: 0x0014CD6A File Offset: 0x0014B16A
	public override void LevelInit(LevelProperties.FlyingMermaid properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x0014CD73 File Offset: 0x0014B173
	private void OnBossDeath()
	{
		this.StopAllCoroutines();
		base.animator.Play("Death");
		this.state = FlyingMermaidLevelMerdusaHead.State.Dead;
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x0014CD94 File Offset: 0x0014B194
	private IEnumerator intro_cr()
	{
		this.state = FlyingMermaidLevelMerdusaHead.State.Intro;
		Level.Current.SetBounds(null, null, null, new int?(300));
		yield return null;
		yield break;
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x0014CDB0 File Offset: 0x0014B1B0
	private IEnumerator move_head_cr()
	{
		Vector2 pos = base.transform.position;
		YieldInstruction wait = new WaitForFixedUpdate();
		float offset = this.xPosition - pos.x;
		for (;;)
		{
			float targetXDistance = float.MaxValue;
			float targetY = 0f;
			foreach (Transform transform in this.coral.points)
			{
				float num = transform.position.x - pos.x;
				if (num > 0f && num < targetXDistance)
				{
					targetXDistance = num;
					targetY = transform.position.y;
				}
			}
			float t = 0f;
			float time = targetXDistance / this.coral.speed;
			float startY = pos.y;
			while (t < time)
			{
				if (base.transform.position.x < this.xPosition)
				{
					pos.x += offset * (CupheadTime.FixedDelta / this.headBackMoveTime);
				}
				else
				{
					pos.x = this.xPosition;
				}
				t += CupheadTime.FixedDelta;
				pos.y = EaseUtils.EaseInOutSine(startY, targetY, t / time);
				base.transform.position = pos;
				yield return wait;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x0014CDCB File Offset: 0x0014B1CB
	public void StartBubble()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.bubble_cr());
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x0014CDF6 File Offset: 0x0014B1F6
	public void StartHeadBlast()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.head_blast_cr());
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x0014CE21 File Offset: 0x0014B221
	public void StartHeadBubble()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.head_blast_bubble_cr());
	}

	// Token: 0x06002388 RID: 9096 RVA: 0x0014CE4C File Offset: 0x0014B24C
	private IEnumerator bubble_cr()
	{
		this.state = FlyingMermaidLevelMerdusaHead.State.Bubble;
		base.animator.SetTrigger("OnSnakeATK");
		yield return base.animator.WaitForAnimationToEnd(this, "Snake_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bubbles.attackDelayRange.RandomFloat());
		this.state = FlyingMermaidLevelMerdusaHead.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002389 RID: 9097 RVA: 0x0014CE68 File Offset: 0x0014B268
	private IEnumerator head_blast_cr()
	{
		this.state = FlyingMermaidLevelMerdusaHead.State.HeadBlast;
		base.animator.SetTrigger("OnEyewave");
		yield return base.animator.WaitForAnimationToEnd(this, "Eyewave_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.headBlast.attackDelayRange.RandomFloat());
		this.state = FlyingMermaidLevelMerdusaHead.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x0600238A RID: 9098 RVA: 0x0014CE84 File Offset: 0x0014B284
	private IEnumerator head_blast_bubble_cr()
	{
		this.state = FlyingMermaidLevelMerdusaHead.State.Both;
		base.animator.SetTrigger("OnBoth");
		yield return base.animator.WaitForAnimationToEnd(this, "Snake_Eyewave", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.coral.bubbleEyewaveSpawnDelayRange.RandomFloat());
		this.state = FlyingMermaidLevelMerdusaHead.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x0600238B RID: 9099 RVA: 0x0014CEA0 File Offset: 0x0014B2A0
	private void SpawnBubble()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = next.transform.position - base.transform.position;
		LevelProperties.FlyingMermaid.Bubbles bubbles = base.properties.CurrentState.bubbles;
		this.bubblePrefab.CreateBubble(this.snakeRoot.transform.position, bubbles.movementSpeed, bubbles.waveSpeed, bubbles.waveAmount, MathUtils.DirectionToAngle(v));
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x0014CF20 File Offset: 0x0014B320
	private void SpawnHeadBlast()
	{
		BasicProjectile basicProjectile = this.heatBlastPrefab.Create(this.eyebeamRoot.transform.position, 0f, -base.properties.CurrentState.headBlast.movementSpeed);
		basicProjectile.GetComponent<FlyingMermaidLevelLaser>().SetStoneTime(base.properties.CurrentState.zap.stoneTime);
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x0014CF8C File Offset: 0x0014B38C
	private IEnumerator spawn_yellow_dots_cr()
	{
		float xPos = 690f;
		LevelProperties.FlyingMermaid.Coral p = base.properties.CurrentState.coral;
		int mainIndex = UnityEngine.Random.Range(0, p.yellowDotPosString.Length);
		string[] yPosString = p.yellowDotPosString[mainIndex].Split(new char[]
		{
			','
		});
		for (;;)
		{
			yPosString = p.yellowDotPosString[mainIndex].Split(new char[]
			{
				','
			});
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.coral.yellowSpawnDelayRange.RandomFloat());
			float[] yPos = new float[yPosString.Length];
			for (int i = 0; i < yPosString.Length; i++)
			{
				yPos[i] = Parser.FloatParse(yPosString[i]);
			}
			Array.Sort<float>(yPos);
			for (int j = 0; j < yPosString.Length; j++)
			{
				Vector3 v = new Vector3(xPos, base.transform.position.y - 20f + yPos[j]);
				BasicProjectile basicProjectile = this.yellowDot.Create(v, 0f, -p.coralMoveSpeed);
				if (yPosString.Length == 1)
				{
					basicProjectile.animator.SetFloat("PillarType", 1f);
				}
				else if (j == 0)
				{
					basicProjectile.animator.SetFloat("PillarType", 0.5f);
				}
				else if (j == yPosString.Length - 1)
				{
					basicProjectile.animator.SetFloat("PillarType", (!Rand.Bool()) ? 0.25f : 0f);
				}
				else
				{
					basicProjectile.animator.SetFloat("PillarType", 0.75f);
				}
				basicProjectile.animator.Play("Pillar", 0, UnityEngine.Random.value);
				basicProjectile.GetComponent<SpriteRenderer>().sortingOrder = j;
			}
			mainIndex = (mainIndex + 1) % p.yellowDotPosString.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x0014CFA7 File Offset: 0x0014B3A7
	private void SoundMermaidPhase3GhostShoot()
	{
		AudioManager.Play("level_mermaid_phase3_ghostshoot");
		this.emitAudioFromObject.Add("level_mermaid_phase3_ghostshoot");
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x0014CFC3 File Offset: 0x0014B3C3
	private void SoundMermaidPhase3SnakeShoot()
	{
		AudioManager.Play("level_mermaid_phase3_snakeshoot");
		this.emitAudioFromObject.Add("level_mermaid_phase3_snakeshoot");
	}

	// Token: 0x04002C19 RID: 11289
	private const float PillarTopA = 0f;

	// Token: 0x04002C1A RID: 11290
	private const float PillarTopB = 0.25f;

	// Token: 0x04002C1B RID: 11291
	private const float PillarBottom = 0.5f;

	// Token: 0x04002C1C RID: 11292
	private const float PillarPlain = 0.75f;

	// Token: 0x04002C1D RID: 11293
	private const float PillarSingle = 1f;

	// Token: 0x04002C1E RID: 11294
	private const string PillarParameterName = "PillarType";

	// Token: 0x04002C1F RID: 11295
	private const string PillarStateName = "Pillar";

	// Token: 0x04002C20 RID: 11296
	[SerializeField]
	private BasicProjectile yellowDot;

	// Token: 0x04002C21 RID: 11297
	[SerializeField]
	private SpriteRenderer wave1;

	// Token: 0x04002C22 RID: 11298
	[SerializeField]
	private SpriteRenderer wave2;

	// Token: 0x04002C23 RID: 11299
	[SerializeField]
	private ScrollingSpriteSpawner[] scrollingSpritesToEnd;

	// Token: 0x04002C24 RID: 11300
	[SerializeField]
	private ScrollingSpriteSpawner[] scrollingSprites;

	// Token: 0x04002C25 RID: 11301
	[SerializeField]
	private FlyingMermaidLevelBackgroundChange coral;

	// Token: 0x04002C26 RID: 11302
	[SerializeField]
	private Transform snakeRoot;

	// Token: 0x04002C27 RID: 11303
	[SerializeField]
	private Transform eyebeamRoot;

	// Token: 0x04002C28 RID: 11304
	[SerializeField]
	private FlyingMermaidLevelSkullBubble bubblePrefab;

	// Token: 0x04002C29 RID: 11305
	[SerializeField]
	private BasicProjectile heatBlastPrefab;

	// Token: 0x04002C2A RID: 11306
	[SerializeField]
	private float xPosition;

	// Token: 0x04002C2B RID: 11307
	[SerializeField]
	private float headBackMoveTime;

	// Token: 0x04002C2D RID: 11309
	private DamageDealer damageDealer;

	// Token: 0x04002C2E RID: 11310
	private DamageReceiver damageReceiver;

	// Token: 0x04002C2F RID: 11311
	private Coroutine patternCoroutine;

	// Token: 0x02000691 RID: 1681
	public enum State
	{
		// Token: 0x04002C31 RID: 11313
		Intro,
		// Token: 0x04002C32 RID: 11314
		Idle,
		// Token: 0x04002C33 RID: 11315
		HeadBlast,
		// Token: 0x04002C34 RID: 11316
		Bubble,
		// Token: 0x04002C35 RID: 11317
		Both,
		// Token: 0x04002C36 RID: 11318
		Dead
	}
}
