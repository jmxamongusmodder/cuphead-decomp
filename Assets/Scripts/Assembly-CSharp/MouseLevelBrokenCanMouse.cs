using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x020006DE RID: 1758
public class MouseLevelBrokenCanMouse : LevelProperties.Mouse.Entity
{
	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x0600256E RID: 9582 RVA: 0x0015DE61 File Offset: 0x0015C261
	// (set) Token: 0x0600256F RID: 9583 RVA: 0x0015DE69 File Offset: 0x0015C269
	public MouseLevelBrokenCanMouse.State state { get; private set; }

	// Token: 0x06002570 RID: 9584 RVA: 0x0015DE74 File Offset: 0x0015C274
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
		this.platformLocalPos = this.platform.localPosition;
		this.platform.transform.parent = null;
		this.setFlameCollidersEnabled(false);
		this.colliders = this.mouse.GetComponents<Collider2D>();
		for (int i = 0; i < this.colliders.Length; i++)
		{
			this.colliders[i].enabled = false;
		}
	}

	// Token: 0x06002571 RID: 9585 RVA: 0x0015DF1C File Offset: 0x0015C31C
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.platform.position = new Vector2(base.transform.position.x, base.transform.position.y) + this.platformLocalPos;
		if (this.peeking && base.properties.CurrentHealth < base.properties.TotalHealth * this.catPeeking.Peek2Threshold)
		{
			this.catPeeking.StopPeeking();
			this.peeking = false;
		}
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x0015DFC9 File Offset: 0x0015C3C9
	private void LateUpdate()
	{
		this.leftFlame.UpdateParentTransform(base.transform);
		this.rightFlame.UpdateParentTransform(base.transform);
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x0015DFED File Offset: 0x0015C3ED
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002574 RID: 9588 RVA: 0x0015E016 File Offset: 0x0015C416
	public override void LevelInit(LevelProperties.Mouse properties)
	{
		base.LevelInit(properties);
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x06002575 RID: 9589 RVA: 0x0015E031 File Offset: 0x0015C431
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002576 RID: 9590 RVA: 0x0015E044 File Offset: 0x0015C444
	private float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06002577 RID: 9591 RVA: 0x0015E065 File Offset: 0x0015C465
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bulletPrefab = null;
	}

	// Token: 0x06002578 RID: 9592 RVA: 0x0015E074 File Offset: 0x0015C474
	public void StartPattern(Transform transform)
	{
		for (int i = 0; i < this.colliders.Length; i++)
		{
			this.colliders[i].enabled = true;
		}
		base.transform.position = transform.position;
		base.transform.localScale = transform.localScale;
		base.animator.SetTrigger("Continue");
		if (this.state != MouseLevelBrokenCanMouse.State.Dying)
		{
			this.state = MouseLevelBrokenCanMouse.State.Down;
			base.StartCoroutine("main_cr");
		}
		base.StartCoroutine(this.move_cr());
		this.direction = ((transform.localScale.x <= 0f) ? MouseLevelBrokenCanMouse.Direction.Right : MouseLevelBrokenCanMouse.Direction.Left);
	}

	// Token: 0x06002579 RID: 9593 RVA: 0x0015E12C File Offset: 0x0015C52C
	private IEnumerator main_cr()
	{
		LevelProperties.Mouse.State patternState = base.properties.CurrentState;
		string[] pattern = patternState.brokenCanFlame.attackString.RandomChoice<string>().Split(new char[]
		{
			','
		});
		while (!this.dead)
		{
			if (patternState != base.properties.CurrentState)
			{
				if (!patternState.brokenCanFlame.attackString.SequenceEqual(base.properties.CurrentState.brokenCanFlame.attackString))
				{
					pattern = base.properties.CurrentState.brokenCanFlame.attackString.RandomChoice<string>().Split(new char[]
					{
						','
					});
				}
				patternState = base.properties.CurrentState;
			}
			LevelProperties.Mouse.BrokenCanFlame p = base.properties.CurrentState.brokenCanFlame;
			foreach (string instruction in pattern)
			{
				this.state = ((this.state != MouseLevelBrokenCanMouse.State.Down) ? MouseLevelBrokenCanMouse.State.Down : MouseLevelBrokenCanMouse.State.Up);
				base.animator.SetTrigger((this.state != MouseLevelBrokenCanMouse.State.Down) ? "Up" : "Down");
				yield return base.animator.WaitForAnimationToEnd(this, (this.state != MouseLevelBrokenCanMouse.State.Down) ? "Going_Up" : "Going_Down", 1, false, true);
				base.animator.SetTrigger("Continue");
				yield return CupheadTime.WaitForSeconds(this, p.delayBeforeShot);
				if (instruction == "BF")
				{
					base.animator.SetTrigger("Shoot");
					BasicProjectile leftBullet = this.bulletPrefab.Create(this.leftBulletRoot.position, (float)((base.transform.localScale.x <= 0f) ? 0 : 180), p.shotSpeed);
					leftBullet.SetParryable(true);
					BasicProjectile rightBullet = this.bulletPrefab.Create(this.rightBulletRoot.position, (float)((base.transform.localScale.x <= 0f) ? 180 : 0), p.shotSpeed);
					rightBullet.SetParryable(true);
					yield return base.animator.WaitForAnimationToEnd(this, "Fire", 2, false, true);
					yield return CupheadTime.WaitForSeconds(this, p.delayAfterShot);
				}
				base.StartCoroutine(this.scale_flames_cr(true));
				base.animator.SetTrigger("Flame");
				yield return CupheadTime.WaitForSeconds(this, p.chargeTime);
				base.animator.SetTrigger("FlameContinue");
				this.setFlameCollidersEnabled(true);
				this.flameOn = true;
				yield return CupheadTime.WaitForSeconds(this, p.loopTime);
				this.setFlameCollidersEnabled(false);
				base.StartCoroutine(this.scale_flames_cr(false));
				this.flameOn = false;
				base.animator.SetTrigger("FlameContinue");
				yield return base.animator.WaitForAnimationToEnd(this, "End", 4, false, true);
			}
		}
		yield break;
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x0015E148 File Offset: 0x0015C548
	private IEnumerator scale_flames_cr(bool turningOn)
	{
		float t = 0f;
		float time = 1f;
		if (turningOn)
		{
			this.leftFlame.transform.SetScale(new float?(1f), new float?(1f), new float?(0f));
			this.rightFlame.transform.SetScale(new float?(1f), new float?(1f), new float?(0f));
			this.leftFlameSprite.transform.SetScale(new float?(this.finalFlameScale.x), new float?(this.finalFlameScale.y), new float?(0f));
			this.rightFlameSprite.transform.SetScale(new float?(this.finalFlameScale.x), new float?(this.finalFlameScale.y), new float?(0f));
			this.leftFlameSprite.enabled = true;
			this.rightFlameSprite.enabled = true;
		}
		while (t < time)
		{
			t += CupheadTime.Delta;
			if (turningOn)
			{
				this.leftFlame.transform.SetScale(new float?(-(t / time)), new float?(t / time), new float?(0f));
				this.rightFlame.transform.SetScale(new float?(t / time), new float?(t / time), new float?(0f));
				this.leftFlameSprite.transform.SetScale(new float?(t / time * this.finalFlameScale.x), new float?(t / time * this.finalFlameScale.y), new float?(0f));
				this.rightFlameSprite.transform.SetScale(new float?(t / time * this.finalFlameScale.x), new float?(t / time * this.finalFlameScale.y), new float?(0f));
			}
			else
			{
				this.leftFlame.transform.SetScale(new float?(-1f + t / time), new float?(1f - t / time), new float?(0f));
				this.rightFlame.transform.SetScale(new float?(1f - t / time), new float?(1f - t / time), new float?(0f));
				this.leftFlameSprite.transform.SetScale(new float?(this.finalFlameScale.x - t / time * this.finalFlameScale.x), new float?(this.finalFlameScale.y - t / time * this.finalFlameScale.y), new float?(0f));
				this.rightFlameSprite.transform.SetScale(new float?(this.finalFlameScale.x - t / time * this.finalFlameScale.x), new float?(this.finalFlameScale.y - t / time * this.finalFlameScale.y), new float?(0f));
			}
			yield return null;
		}
		if (!turningOn)
		{
			this.leftFlame.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
			this.rightFlame.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
			this.leftFlameSprite.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
			this.rightFlameSprite.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
			this.leftFlameSprite.enabled = false;
			this.rightFlameSprite.enabled = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x0015E16A File Offset: 0x0015C56A
	private void setFlameCollidersEnabled(bool enabled)
	{
		this.leftFlame.SetColliderEnabled(enabled);
		this.rightFlame.SetColliderEnabled(enabled);
		if (enabled)
		{
			this.SoundMouseFlameThrower();
		}
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x0015E190 File Offset: 0x0015C590
	private IEnumerator moveToX_cr(float x)
	{
		this.overrideMove = true;
		this.overrideMoveX = x;
		while (this.overrideMove)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x0015E1B4 File Offset: 0x0015C5B4
	private IEnumerator move_cr()
	{
		Vector2 startPos = base.transform.position;
		bool overridden;
		do
		{
			LevelProperties.Mouse.BrokenCanMove p = base.properties.CurrentState.brokenCanMove;
			Vector2 targetPos = startPos;
			overridden = false;
			if (this.overrideMove)
			{
				targetPos.x = this.overrideMoveX;
				overridden = true;
			}
			else if (this.direction == MouseLevelBrokenCanMouse.Direction.Left)
			{
				targetPos.x -= p.maxXPositionRange.RandomFloat();
			}
			else
			{
				targetPos.x += p.maxXPositionRange.RandomFloat();
			}
			float time = Mathf.Abs(targetPos.x - base.transform.position.x) / p.speed;
			yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, targetPos, EaseUtils.EaseType.easeInOutSine, time));
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
			this.direction = ((this.direction != MouseLevelBrokenCanMouse.Direction.Left) ? MouseLevelBrokenCanMouse.Direction.Left : MouseLevelBrokenCanMouse.Direction.Right);
		}
		while (!this.overrideMove || !overridden);
		this.overrideMove = false;
		base.animator.Play("Idle", 0);
		yield break;
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x0015E1D0 File Offset: 0x0015C5D0
	private IEnumerator tween_cr(Transform trans, Vector2 start, Vector2 end, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.position = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			trans.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta * this.hitPauseCoefficient();
			float wheelAnimProgress = -base.transform.localScale.x * base.transform.position.x / 100f;
			wheelAnimProgress %= 1f;
			if (wheelAnimProgress < 0f)
			{
				wheelAnimProgress += 1f;
			}
			base.animator.Play("Move", 0, wheelAnimProgress);
			yield return null;
		}
		trans.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x0015E210 File Offset: 0x0015C610
	public void OnBossDeath()
	{
		foreach (Collider2D collider2D in this.mouse.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		this.StopAllCoroutines();
		this.state = MouseLevelBrokenCanMouse.State.Dying;
		base.StartCoroutine(this.death_cr(false));
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x0015E263 File Offset: 0x0015C663
	public void Transform()
	{
		this.state = MouseLevelBrokenCanMouse.State.Dying;
		this.SoundMouseScreamVoice();
		base.StartCoroutine(this.death_cr(true));
		base.properties.OnBossDeath -= this.OnBossDeath;
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x0015E297 File Offset: 0x0015C697
	public void BeEaten()
	{
		UnityEngine.Object.Destroy(this.leftFlame.gameObject);
		UnityEngine.Object.Destroy(this.rightFlame.gameObject);
		UnityEngine.Object.Destroy(this.platform.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x0015E2D4 File Offset: 0x0015C6D4
	private IEnumerator death_cr(bool transform)
	{
		base.animator.SetTrigger("Die");
		this.sawBlades.Leave();
		if (transform)
		{
			base.animator.SetTrigger("Down");
		}
		else
		{
			base.animator.SetBool("CrazyScissor", true);
		}
		if (this.flameOn)
		{
			base.animator.SetTrigger("FlameContinue");
		}
		else
		{
			base.animator.ResetTrigger("Flame");
			AudioManager.Play("level_mouse_scream_death_voice");
		}
		this.leftFlame.SetColliderEnabled(false);
		this.rightFlame.SetColliderEnabled(false);
		base.StopCoroutine("main_cr");
		if (transform)
		{
			yield return base.StartCoroutine(this.moveToX_cr(0f));
			yield return base.animator.WaitForAnimationToStart(this, "Idle_Down", 1, false);
			this.cat.StartIntro();
		}
		yield break;
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x0015E2F6 File Offset: 0x0015C6F6
	private void SoundMouseBrkCanScissorUp()
	{
		AudioManager.Play("level_mouse_broken_can_scissor_going_up");
		this.emitAudioFromObject.Add("level_mouse_broken_can_scissor_going_up");
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x0015E312 File Offset: 0x0015C712
	private void SoundMouseBrkCanScissorDown()
	{
		AudioManager.Play("level_mouse_broken_can_scissor_going_down");
		this.emitAudioFromObject.Add("level_mouse_broken_can_scissor_going_down");
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x0015E32E File Offset: 0x0015C72E
	private void SoundMouseBrkCanStartUp()
	{
		AudioManager.Play("level_mouse_broken_can_start_up");
		this.emitAudioFromObject.Add("level_mouse_broken_can_start_up");
	}

	// Token: 0x06002586 RID: 9606 RVA: 0x0015E34A File Offset: 0x0015C74A
	private void SoundMouseFlameThrower()
	{
		AudioManager.Play("level_mouse_flamethrower");
		this.emitAudioFromObject.Add("level_mouse_flamethrower");
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x0015E366 File Offset: 0x0015C766
	private void SoundMouseSnarkyVoice()
	{
		AudioManager.Play("level_mouse_snarky_voice");
		this.emitAudioFromObject.Add("level_mouse_snarky_voice");
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x0015E382 File Offset: 0x0015C782
	private void SoundMouseScreamVoice()
	{
		AudioManager.Play("level_mouse_scream_voice");
		this.emitAudioFromObject.Add("level_mouse_scream_voice");
	}

	// Token: 0x04002DF7 RID: 11767
	private const int CART_LAYER = 0;

	// Token: 0x04002DF8 RID: 11768
	private const int SCISSOR_LAYER = 1;

	// Token: 0x04002DF9 RID: 11769
	private const int CANNON_LAYER = 2;

	// Token: 0x04002DFA RID: 11770
	private const int MOUSE_LAYER = 3;

	// Token: 0x04002DFB RID: 11771
	private const int FLAME_LAYER = 4;

	// Token: 0x04002DFC RID: 11772
	private const int CAN_LAYER = 5;

	// Token: 0x04002DFD RID: 11773
	[SerializeField]
	private MouseLevelFlame leftFlame;

	// Token: 0x04002DFE RID: 11774
	[SerializeField]
	private MouseLevelFlame rightFlame;

	// Token: 0x04002DFF RID: 11775
	[SerializeField]
	private SpriteRenderer leftFlameSprite;

	// Token: 0x04002E00 RID: 11776
	[SerializeField]
	private SpriteRenderer rightFlameSprite;

	// Token: 0x04002E01 RID: 11777
	[SerializeField]
	private Vector2 finalFlameScale;

	// Token: 0x04002E02 RID: 11778
	[SerializeField]
	private Transform leftBulletRoot;

	// Token: 0x04002E03 RID: 11779
	[SerializeField]
	private Transform rightBulletRoot;

	// Token: 0x04002E04 RID: 11780
	[SerializeField]
	private Transform mouse;

	// Token: 0x04002E05 RID: 11781
	[SerializeField]
	private Transform platform;

	// Token: 0x04002E06 RID: 11782
	[SerializeField]
	private BasicProjectile bulletPrefab;

	// Token: 0x04002E07 RID: 11783
	[SerializeField]
	private MouseLevelSawBladeManager sawBlades;

	// Token: 0x04002E08 RID: 11784
	[SerializeField]
	private MouseLevelCat cat;

	// Token: 0x04002E09 RID: 11785
	[SerializeField]
	private MouseLevelCatPeeking catPeeking;

	// Token: 0x04002E0B RID: 11787
	private MouseLevelBrokenCanMouse.Direction direction;

	// Token: 0x04002E0C RID: 11788
	private DamageReceiver damageReceiver;

	// Token: 0x04002E0D RID: 11789
	private DamageDealer damageDealer;

	// Token: 0x04002E0E RID: 11790
	private bool flameOn;

	// Token: 0x04002E0F RID: 11791
	private Vector2 platformLocalPos;

	// Token: 0x04002E10 RID: 11792
	private bool dead;

	// Token: 0x04002E11 RID: 11793
	private bool peeking = true;

	// Token: 0x04002E12 RID: 11794
	private Collider2D[] colliders;

	// Token: 0x04002E13 RID: 11795
	private bool overrideMove;

	// Token: 0x04002E14 RID: 11796
	private float overrideMoveX;

	// Token: 0x04002E15 RID: 11797
	private const float WHEEL_MOVE_FACTOR = 100f;

	// Token: 0x020006DF RID: 1759
	public enum State
	{
		// Token: 0x04002E17 RID: 11799
		Init,
		// Token: 0x04002E18 RID: 11800
		Down,
		// Token: 0x04002E19 RID: 11801
		Up,
		// Token: 0x04002E1A RID: 11802
		Dying
	}

	// Token: 0x020006E0 RID: 1760
	public enum Direction
	{
		// Token: 0x04002E1C RID: 11804
		Left,
		// Token: 0x04002E1D RID: 11805
		Right
	}
}
