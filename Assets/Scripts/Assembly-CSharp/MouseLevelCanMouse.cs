using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006E2 RID: 1762
public class MouseLevelCanMouse : LevelProperties.Mouse.Entity
{
	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x0600258E RID: 9614 RVA: 0x0015F6B4 File Offset: 0x0015DAB4
	// (set) Token: 0x0600258F RID: 9615 RVA: 0x0015F6BC File Offset: 0x0015DABC
	public MouseLevelCanMouse.State state { get; private set; }

	// Token: 0x06002590 RID: 9616 RVA: 0x0015F6C5 File Offset: 0x0015DAC5
	protected override void Awake()
	{
		base.Awake();
		this.SetWheels(false);
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002591 RID: 9617 RVA: 0x0015F704 File Offset: 0x0015DB04
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (!this.peeking && base.properties.CurrentHealth < base.properties.TotalHealth * this.catPeeking.Peek1Threshold)
		{
			this.catPeeking.StartPeeking();
			this.peeking = true;
		}
	}

	// Token: 0x06002592 RID: 9618 RVA: 0x0015F76B File Offset: 0x0015DB6B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002593 RID: 9619 RVA: 0x0015F794 File Offset: 0x0015DB94
	public override void LevelInit(LevelProperties.Mouse properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.OnLevelStart;
	}

	// Token: 0x06002594 RID: 9620 RVA: 0x0015F7B3 File Offset: 0x0015DBB3
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x0015F7C6 File Offset: 0x0015DBC6
	private float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06002596 RID: 9622 RVA: 0x0015F7E7 File Offset: 0x0015DBE7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.cherryBombPrefab = null;
		this.catapultProjectilePrefab = null;
		this.romanCandlePrefab = null;
		this.wheelSprites = null;
	}

	// Token: 0x06002597 RID: 9623 RVA: 0x0015F80B File Offset: 0x0015DC0B
	public void OnLevelStart()
	{
		base.StartCoroutine(this.wheels_cr());
		base.StartCoroutine(this.levelStart_cr());
	}

	// Token: 0x06002598 RID: 9624 RVA: 0x0015F828 File Offset: 0x0015DC28
	private IEnumerator levelStart_cr()
	{
		base.animator.Play("Intro", 0);
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 0, false, true);
		base.animator.Play("Intro_Down", 1);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x06002599 RID: 9625 RVA: 0x0015F844 File Offset: 0x0015DC44
	private IEnumerator moveBack_cr()
	{
		this.exitAfterMoveBack = true;
		while (this.exitAfterMoveBack)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600259A RID: 9626 RVA: 0x0015F860 File Offset: 0x0015DC60
	private IEnumerator moveToX_cr(float x)
	{
		this.overrideMove = true;
		this.overrideMoveX = x;
		if (!this.moving)
		{
			base.StartCoroutine(this.move_cr());
		}
		while (this.overrideMove)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600259B RID: 9627 RVA: 0x0015F882 File Offset: 0x0015DC82
	private void SetWheels(bool b)
	{
		this.wheelRenderer.enabled = b;
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x0015F890 File Offset: 0x0015DC90
	private IEnumerator move_cr()
	{
		base.animator.Play("Move", 1);
		this.SetWheels(true);
		this.moving = true;
		bool movingBack = false;
		for (;;)
		{
			LevelProperties.Mouse.CanMove p = base.properties.CurrentState.canMove;
			Vector2 end = new Vector2(500f * base.transform.localScale.x, base.transform.position.y);
			bool overriden = false;
			if (this.overrideMove)
			{
				end.x = this.overrideMoveX;
				overriden = true;
			}
			else if (!movingBack)
			{
				end.x -= p.maxXPositionRange.RandomFloat() * base.transform.localScale.x;
			}
			float time = Mathf.Abs(base.transform.position.x - end.x) / p.speed;
			yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, end, EaseUtils.EaseType.easeInOutSine, time));
			yield return CupheadTime.WaitForSeconds(this, p.stopTime);
			if (this.overrideMove && overriden)
			{
				break;
			}
			if (movingBack && this.exitAfterMoveBack)
			{
				goto Block_6;
			}
			movingBack = !movingBack;
		}
		this.overrideMove = false;
		goto IL_270;
		Block_6:
		this.exitAfterMoveBack = false;
		IL_270:
		this.SetWheels(false);
		base.animator.Play("IdleDown", 1);
		this.moving = false;
		yield break;
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x0015F8AC File Offset: 0x0015DCAC
	private IEnumerator wheels_cr()
	{
		int currentFrame = 0;
		Vector2 lastPos = base.transform.position;
		int direction = 1;
		for (;;)
		{
			float distance = 0f;
			while (distance < 6f)
			{
				float speed = lastPos.x - base.transform.position.x;
				distance += Mathf.Abs(speed);
				if (base.transform.localScale.x > 0f)
				{
					if (speed < 0f)
					{
						direction = -1;
					}
					else
					{
						direction = 1;
					}
				}
				else if (speed < 0f)
				{
					direction = 1;
				}
				else
				{
					direction = -1;
				}
				lastPos = base.transform.position;
				yield return null;
			}
			currentFrame += direction;
			currentFrame = (int)Mathf.Repeat((float)currentFrame, (float)this.wheelSprites.Length);
			this.wheelRenderer.sprite = this.wheelSprites[currentFrame];
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600259E RID: 9630 RVA: 0x0015F8C7 File Offset: 0x0015DCC7
	public void StartDash()
	{
		this.state = MouseLevelCanMouse.State.Dash;
		base.StartCoroutine(this.dash_cr());
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x0015F8DD File Offset: 0x0015DCDD
	private void StartDashMove()
	{
		this.dash = true;
		base.animator.SetTrigger("CanContinue");
		AudioManager.Play("level_mouse_can_dash_start");
		this.emitAudioFromObject.Add("level_mouse_can_dash_start");
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x0015F910 File Offset: 0x0015DD10
	private void DashFlipX()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		if (base.transform.localScale.x < 0f)
		{
			this.direction = MouseLevelCanMouse.Direction.Right;
			base.transform.AddPosition(40f, 0f, 0f);
		}
		else
		{
			this.direction = MouseLevelCanMouse.Direction.Left;
			base.transform.AddPosition(-40f, 0f, 0f);
		}
		base.animator.SetTrigger("CanContinue");
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x0015F9CC File Offset: 0x0015DDCC
	private IEnumerator dash_cr()
	{
		LevelProperties.Mouse.CanDash dashProperties = base.properties.CurrentState.canDash;
		for (int i = 0; i < this.springs.Length; i++)
		{
			Vector2 velocity = new Vector2(dashProperties.springVelocityX[i].RandomFloat(), dashProperties.springVelocityY[i].RandomFloat());
			if (this.direction == MouseLevelCanMouse.Direction.Right)
			{
				velocity.x *= -1f;
			}
			this.springs[i].LaunchSpring(new Vector2(base.transform.position.x, base.transform.position.y + 200f), velocity, dashProperties.springGravity);
			AudioManager.Play("level_mouse_can_springboard_shoot");
			this.emitAudioFromObject.Add("level_mouse_can_springboard_shoot");
			base.StartCoroutine(this.timedAudioMouseSnarky_cr());
		}
		if (this.moving)
		{
			yield return base.StartCoroutine(this.moveBack_cr());
		}
		Vector2 start = base.transform.position;
		Vector2 end = new Vector2(-450f * base.transform.localScale.x, base.transform.position.y);
		base.animator.Play("Dash", 1);
		AudioManager.PlayLoop("level_mouse_can_dash_loop");
		this.dash = false;
		while (!this.dash)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.tween_cr(base.transform, start, end, EaseUtils.EaseType.easeInSine, dashProperties.time));
		base.animator.SetTrigger("CanContinue");
		AudioManager.Stop("level_mouse_can_dash_loop");
		AudioManager.Play("level_mouse_can_dash_stop");
		this.emitAudioFromObject.Add("level_mouse_can_dash_stop");
		yield return base.animator.WaitForAnimationToEnd(this, "Dash_End", 1, false, true);
		yield return CupheadTime.WaitForSeconds(this, dashProperties.hesitate);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x0015F9E7 File Offset: 0x0015DDE7
	public void StartCherryBomb()
	{
		this.state = MouseLevelCanMouse.State.CherryBomb;
		base.StartCoroutine(this.cherryBomb_cr());
		base.StartCoroutine(this.timedAudioMouseSnarky_cr());
		if (!this.moving)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x0015FA24 File Offset: 0x0015DE24
	private void FireCherryBomb()
	{
		base.animator.SetTrigger("Shoot");
		Vector2 velocity = new Vector2(base.properties.CurrentState.canCherryBomb.xVelocity * base.transform.localScale.x, base.properties.CurrentState.canCherryBomb.yVelocity);
		this.cherryBombPrefab.Create(this.cherryBombRoot.position, velocity, base.properties.CurrentState.canCherryBomb.gravity, (float)base.properties.CurrentState.canCherryBomb.childSpeed);
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x0015FAD8 File Offset: 0x0015DED8
	private IEnumerator cherryBomb_cr()
	{
		base.animator.ResetTrigger("Continue");
		base.animator.ResetTrigger("Shoot");
		LevelProperties.Mouse.CanCherryBomb properties = base.properties.CurrentState.canCherryBomb;
		KeyValue[] pattern = KeyValue.ListFromString(properties.patterns[UnityEngine.Random.Range(0, properties.patterns.Length)], new char[]
		{
			'P',
			'D'
		});
		base.animator.Play("Cannon_Start", 0);
		yield return base.animator.WaitForAnimationToEnd(this, "Cannon_Start", 0, false, true);
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "P")
			{
				int p = 0;
				while ((float)p < pattern[i].value)
				{
					yield return CupheadTime.WaitForSeconds(this, properties.delay);
					this.FireCherryBomb();
					p++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Idle_Down", 0, false);
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x0015FAF3 File Offset: 0x0015DEF3
	public void StartCatapult()
	{
		this.state = MouseLevelCanMouse.State.Catapult;
		base.StartCoroutine(this.catapult_cr());
		base.StartCoroutine(this.timedAudioMouseSnarky_cr());
		if (!this.moving)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x0015FB30 File Offset: 0x0015DF30
	private void FireCatapult()
	{
		LevelProperties.Mouse.CanCatapult canCatapult = base.properties.CurrentState.canCatapult;
		char[] array = canCatapult.patterns.GetRandom<string>().ToLower().ToCharArray();
		float num = (float)((this.direction != MouseLevelCanMouse.Direction.Right) ? 165 : -45);
		if (array.Length <= 1)
		{
			this.catapultProjectilePrefab.CreateFromPrefab(this.catapultRoot.position, num + canCatapult.angleOffset, (float)canCatapult.projectileSpeed, array[0]);
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			float rotation = num + canCatapult.spreadAngle / (float)(array.Length - 1) * (float)i;
			this.catapultProjectilePrefab.CreateFromPrefab(this.catapultRoot.position, rotation, (float)canCatapult.projectileSpeed, array[i]);
		}
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x0015FC08 File Offset: 0x0015E008
	private IEnumerator catapult_cr()
	{
		LevelProperties.Mouse.CanCatapult properties = base.properties.CurrentState.canCatapult;
		base.animator.ResetTrigger("Continue");
		base.animator.ResetTrigger("Shoot");
		base.animator.Play("Catapult_Idle", 0);
		yield return base.StartCoroutine(this.tweenCatapultY_cr(-280f, 0f, properties.timeIn, EaseUtils.EaseType.easeOutSine));
		yield return CupheadTime.WaitForSeconds(this, properties.pumpDelay);
		for (int i = 0; i < properties.count; i++)
		{
			base.animator.SetTrigger("Continue");
			this.SoundMouseCatapultGlug();
			yield return base.animator.WaitForAnimationToEnd(this, "Catapult_Pump", 0, false, true);
			yield return CupheadTime.WaitForSeconds(this, properties.pumpDelay);
			base.animator.SetTrigger("Shoot");
			yield return base.animator.WaitForAnimationToEnd(this, "Catapult_Shoot", 0, false, true);
			yield return CupheadTime.WaitForSeconds(this, properties.repeatDelay);
		}
		yield return base.StartCoroutine(this.tweenCatapultY_cr(0f, -280f, properties.timeOut, EaseUtils.EaseType.easeOutSine));
		base.animator.Play("Idle_Down", 0);
		yield return CupheadTime.WaitForSeconds(this, (float)properties.hesitate);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x0015FC24 File Offset: 0x0015E024
	private IEnumerator tweenCatapultY_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		this.catapult.transform.SetLocalPosition(null, new float?(start), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.catapult.transform.SetLocalPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.catapult.transform.SetLocalPosition(null, new float?(end), null);
		yield return null;
		yield break;
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x0015FC5C File Offset: 0x0015E05C
	public void StartRomanCandle()
	{
		this.state = MouseLevelCanMouse.State.RomanCandle;
		base.StartCoroutine(this.romanCandle_cr());
		base.StartCoroutine(this.timedAudioMouseSnarky_cr());
		if (!this.moving)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x0015FC98 File Offset: 0x0015E098
	private void FireRomanCandle()
	{
		this.romanCandlePrefab.Create(this.romanCandleRoot.position, (float)((base.transform.localScale.x <= 0f) ? 0 : 180), base.properties.CurrentState.canRomanCandle.speed, base.properties.CurrentState.canRomanCandle.speed, base.properties.CurrentState.canRomanCandle.rotationSpeed, 100f, base.properties.CurrentState.canRomanCandle.timeBeforeHoming, PlayerManager.GetNext());
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x0015FD48 File Offset: 0x0015E148
	private IEnumerator romanCandle_cr()
	{
		LevelProperties.Mouse.CanRomanCandle properties = base.properties.CurrentState.canRomanCandle;
		base.animator.ResetTrigger("Continue");
		base.animator.ResetTrigger("Shoot");
		for (int i = 0; i < properties.count.RandomInt(); i++)
		{
			yield return CupheadTime.WaitForSeconds(this, properties.repeatDelay);
			base.animator.Play("Roman_Candle", 0);
			yield return base.animator.WaitForAnimationToEnd(this, "Roman_Candle", 0, false, true);
		}
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = MouseLevelCanMouse.State.Idle;
		yield break;
	}

	// Token: 0x060025AC RID: 9644 RVA: 0x0015FD63 File Offset: 0x0015E163
	public void Explode(Action onStartPlatform, Action onTransitionComplete)
	{
		this.onStartPlatform = onStartPlatform;
		this.onTransitionComplete = onTransitionComplete;
		base.StartCoroutine(this.explode_cr());
	}

	// Token: 0x060025AD RID: 9645 RVA: 0x0015FD80 File Offset: 0x0015E180
	private IEnumerator explode_cr()
	{
		while (this.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.sawBlades.Begin(base.properties);
		yield return base.StartCoroutine(this.moveToX_cr(0f));
		base.animator.Play("Explode", 1);
		yield break;
	}

	// Token: 0x060025AE RID: 9646 RVA: 0x0015FD9B File Offset: 0x0015E19B
	private void OnExplodedAnim()
	{
		this.onStartPlatform();
		this.SetWheels(false);
		if (this.brokenCan.state != MouseLevelBrokenCanMouse.State.Dying)
		{
			this.catPeeking.IsPhase2 = true;
		}
	}

	// Token: 0x060025AF RID: 9647 RVA: 0x0015FDCC File Offset: 0x0015E1CC
	private void SpawnBrokenCan()
	{
		this.brokenCan.StartPattern(base.transform);
		this.onTransitionComplete();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x0015FDF8 File Offset: 0x0015E1F8
	private IEnumerator tween_cr(Transform trans, Vector2 start, Vector2 end, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.position = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			trans.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta * this.hitPauseCoefficient();
			yield return null;
		}
		trans.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x060025B1 RID: 9649 RVA: 0x0015FE38 File Offset: 0x0015E238
	private void SoundMouseCanIntro()
	{
		AudioManager.Play("level_mouse_can_intro");
		this.emitAudioFromObject.Add("level_mouse_can_intro");
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x0015FE54 File Offset: 0x0015E254
	private void SoundMouseCannonShoot()
	{
		AudioManager.Play("level_mouse_can_cannon_shoot");
		this.emitAudioFromObject.Add("level_mouse_can_cannon_shoot");
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x0015FE70 File Offset: 0x0015E270
	private void SoundMouseCannonEnd()
	{
		AudioManager.Play("level_mouse_can_cannon_end");
		this.emitAudioFromObject.Add("level_mouse_can_cannon_end");
	}

	// Token: 0x060025B4 RID: 9652 RVA: 0x0015FE8C File Offset: 0x0015E28C
	private void SoundMouseCatapultShoot()
	{
		AudioManager.Play("level_mouse_can_catapult_shoot");
		this.emitAudioFromObject.Add("level_mouse_can_catapult_shoot");
	}

	// Token: 0x060025B5 RID: 9653 RVA: 0x0015FEA8 File Offset: 0x0015E2A8
	private void SoundMouseCatapultGlug()
	{
		AudioManager.Play("level_mouse_can_catapult_glug");
		this.emitAudioFromObject.Add("level_mouse_can_catapult_glug");
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x0015FEC4 File Offset: 0x0015E2C4
	private void SoundMouseCanDashStart()
	{
		AudioManager.Play("level_mouse_can_dash_start");
		this.emitAudioFromObject.Add("level_mouse_can_dash_start");
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x0015FEE0 File Offset: 0x0015E2E0
	private void SoundMouseCanDashLoop()
	{
		AudioManager.PlayLoop("level_mouse_can_dash_loop");
		this.emitAudioFromObject.Add("level_mouse_can_dash_loop");
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x0015FEFC File Offset: 0x0015E2FC
	private void SoundMouseCanDashStop()
	{
		AudioManager.PlayLoop("level_mouse_can_dash_stop");
		this.emitAudioFromObject.Add("level_mouse_can_dash_stop");
	}

	// Token: 0x060025B9 RID: 9657 RVA: 0x0015FF18 File Offset: 0x0015E318
	private void SoundMouseCanDashEndAnim()
	{
		AudioManager.Play("level_mouse_can_dash_end");
		this.emitAudioFromObject.Add("level_mouse_can_dash_end");
	}

	// Token: 0x060025BA RID: 9658 RVA: 0x0015FF34 File Offset: 0x0015E334
	private void SoundMouseCanExplode()
	{
		AudioManager.Play("level_mouse_can_explode");
	}

	// Token: 0x060025BB RID: 9659 RVA: 0x0015FF40 File Offset: 0x0015E340
	private void SoundMouseCanExplodePre()
	{
		AudioManager.Play("level_mouse_can_explode_pre");
	}

	// Token: 0x060025BC RID: 9660 RVA: 0x0015FF4C File Offset: 0x0015E34C
	private void SoundMouseCanRomanCandle()
	{
		AudioManager.Play("level_mouse_can_roman_candle");
		this.emitAudioFromObject.Add("level_mouse_can_roman_candle");
	}

	// Token: 0x060025BD RID: 9661 RVA: 0x0015FF68 File Offset: 0x0015E368
	private void SoundMouseChargeVoice()
	{
		AudioManager.Play("level_mouse_charge_voice");
		this.emitAudioFromObject.Add("level_mouse_charge_voice");
	}

	// Token: 0x060025BE RID: 9662 RVA: 0x0015FF84 File Offset: 0x0015E384
	private IEnumerator timedAudioMouseSnarky_cr()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.Play("level_mouse_snarky_voice");
		yield break;
	}

	// Token: 0x04002E1E RID: 11806
	private const int MOUSE_LAYER = 0;

	// Token: 0x04002E1F RID: 11807
	private const int CAN_LAYER = 1;

	// Token: 0x04002E20 RID: 11808
	private const float MOVE_START_X = 500f;

	// Token: 0x04002E21 RID: 11809
	private const float DASH_END_X = 450f;

	// Token: 0x04002E22 RID: 11810
	[Header("Cannon")]
	[SerializeField]
	private Transform cherryBombRoot;

	// Token: 0x04002E23 RID: 11811
	[SerializeField]
	private MouseLevelCherryBombProjectile cherryBombPrefab;

	// Token: 0x04002E24 RID: 11812
	[Header("Catapult")]
	[SerializeField]
	private Transform catapult;

	// Token: 0x04002E25 RID: 11813
	[SerializeField]
	private MouseLevelCanCatapultProjectile catapultProjectilePrefab;

	// Token: 0x04002E26 RID: 11814
	[SerializeField]
	private Transform catapultRoot;

	// Token: 0x04002E27 RID: 11815
	[Header("Roman Candle")]
	[SerializeField]
	private Transform romanCandleRoot;

	// Token: 0x04002E28 RID: 11816
	[SerializeField]
	private MouseLevelRomanCandleProjectile romanCandlePrefab;

	// Token: 0x04002E29 RID: 11817
	[Header("Wheels")]
	[SerializeField]
	private SpriteRenderer wheelRenderer;

	// Token: 0x04002E2A RID: 11818
	[SerializeField]
	private Sprite[] wheelSprites;

	// Token: 0x04002E2B RID: 11819
	[SerializeField]
	private MouseLevelBrokenCanMouse brokenCan;

	// Token: 0x04002E2C RID: 11820
	[SerializeField]
	private MouseLevelSawBladeManager sawBlades;

	// Token: 0x04002E2D RID: 11821
	[SerializeField]
	private MouseLevelCatPeeking catPeeking;

	// Token: 0x04002E2E RID: 11822
	[Header("Springs")]
	[SerializeField]
	private MouseLevelSpring[] springs;

	// Token: 0x04002E30 RID: 11824
	private MouseLevelCanMouse.Direction direction;

	// Token: 0x04002E31 RID: 11825
	private DamageReceiver damageReceiver;

	// Token: 0x04002E32 RID: 11826
	private DamageDealer damageDealer;

	// Token: 0x04002E33 RID: 11827
	private bool moving;

	// Token: 0x04002E34 RID: 11828
	private bool peeking;

	// Token: 0x04002E35 RID: 11829
	private bool overrideMove;

	// Token: 0x04002E36 RID: 11830
	private bool exitAfterMoveBack;

	// Token: 0x04002E37 RID: 11831
	private float overrideMoveX;

	// Token: 0x04002E38 RID: 11832
	private bool dash;

	// Token: 0x04002E39 RID: 11833
	private const float FLIP_OFFSET = 40f;

	// Token: 0x04002E3A RID: 11834
	private Action onStartPlatform;

	// Token: 0x04002E3B RID: 11835
	private Action onTransitionComplete;

	// Token: 0x020006E3 RID: 1763
	public enum State
	{
		// Token: 0x04002E3D RID: 11837
		Intro,
		// Token: 0x04002E3E RID: 11838
		Idle,
		// Token: 0x04002E3F RID: 11839
		Dash,
		// Token: 0x04002E40 RID: 11840
		CherryBomb,
		// Token: 0x04002E41 RID: 11841
		Catapult,
		// Token: 0x04002E42 RID: 11842
		RomanCandle
	}

	// Token: 0x020006E4 RID: 1764
	public enum Direction
	{
		// Token: 0x04002E44 RID: 11844
		Left,
		// Token: 0x04002E45 RID: 11845
		Right
	}
}
