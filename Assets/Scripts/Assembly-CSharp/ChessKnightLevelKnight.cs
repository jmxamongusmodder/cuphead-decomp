using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200053F RID: 1343
public class ChessKnightLevelKnight : LevelProperties.ChessKnight.Entity
{
	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001885 RID: 6277 RVA: 0x000DDEB1 File Offset: 0x000DC2B1
	// (set) Token: 0x06001886 RID: 6278 RVA: 0x000DDEB9 File Offset: 0x000DC2B9
	public ChessKnightLevelKnight.State state { get; private set; }

	// Token: 0x06001887 RID: 6279 RVA: 0x000DDEC4 File Offset: 0x000DC2C4
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		float x = -640f + this.positionBoundaryInset.minimum;
		Gizmos.DrawLine(new Vector3(x, -500f), new Vector3(x, 500f));
		Gizmos.color = Color.green;
		x = -640f + this.positionBoundaryInset.maximum;
		Gizmos.DrawLine(new Vector3(x, -500f), new Vector3(x, 500f));
		Gizmos.color = Color.red;
		x = 640f - this.positionBoundaryInset.minimum;
		Gizmos.DrawLine(new Vector3(x, -500f), new Vector3(x, 500f));
		Gizmos.color = Color.green;
		x = 640f - this.positionBoundaryInset.maximum;
		Gizmos.DrawLine(new Vector3(x, -500f), new Vector3(x, 500f));
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x000DDFB4 File Offset: 0x000DC3B4
	public override void LevelInit(LevelProperties.ChessKnight properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.onIntroEventHandler;
		LevelProperties.ChessKnight.Knight knight = properties.CurrentState.knight;
		this.attackIntervalPattern = new PatternString(knight.attackIntervalString, true, true);
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null && !player2.IsDead)
		{
			this.targetPlayer = ((!Rand.Bool()) ? player2 : player);
		}
		this.battleStartPosition = base.transform.position.x;
		this.movementPattern = new PatternString(properties.CurrentState.movement.movementString, true, true);
		this.numberTauntString = new PatternString(properties.CurrentState.tauntAttack.numberTauntString, true);
		this.numberTauntString.SetSubStringIndex(-1);
		this.tauntAttackCounter = this.numberTauntString.PopInt();
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x000DE0A8 File Offset: 0x000DC4A8
	protected override void Awake()
	{
		base.Awake();
		Vector3 position = base.transform.position;
		position.x = 640f - (this.positionBoundaryInset.maximum + this.positionBoundaryInset.minimum) * 0.5f;
		base.transform.position = position;
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x000DE100 File Offset: 0x000DC500
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.pink.OnActivate += this.GotParried;
		this.swordHitbox.OnPlayerCollision += this.OnCollisionPlayer;
		this.upHitbox.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x000DE15F File Offset: 0x000DC55F
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x000DE177 File Offset: 0x000DC577
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x000DE198 File Offset: 0x000DC598
	private void GotParried()
	{
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
		this.hitFlash.Flash(0.7f);
		base.StartCoroutine(this.parry_timer_cr());
		if (base.properties.CurrentHealth <= 0f && this.state != ChessKnightLevelKnight.State.Death)
		{
			this.state = ChessKnightLevelKnight.State.Death;
			this.death();
		}
	}

	// Token: 0x0600188E RID: 6286 RVA: 0x000DE214 File Offset: 0x000DC614
	private IEnumerator parry_timer_cr()
	{
		this.pink.gameObject.SetActive(false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.knight.parryCooldown);
		this.pink.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x000DE22F File Offset: 0x000DC62F
	private void onIntroEventHandler()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x000DE240 File Offset: 0x000DC640
	private IEnumerator intro_cr()
	{
		base.animator.SetTrigger("Intro");
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Intro", 0, false, false, true);
		this.EndAttack();
		yield break;
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x000DE25B File Offset: 0x000DC65B
	private void EndAttack()
	{
		this.isTauntAttack = false;
		this.SFX_KOG_KNIGHT_RecoverFoley();
		this.state = ChessKnightLevelKnight.State.Move;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x000DE280 File Offset: 0x000DC680
	private IEnumerator move_cr()
	{
		if (this.tauntAttackCounter > 0)
		{
			LevelProperties.ChessKnight.Movement p = base.properties.CurrentState.movement;
			float idleTime = this.attackIntervalPattern.PopFloat();
			float idleT = 0f;
			float moveT = 0f;
			YieldInstruction wait = new WaitForFixedUpdate();
			float startPosition = base.transform.position.x;
			float movementAmount = this.movementPattern.PopFloat();
			this.goingLeft = this.chooseGoingLeft(movementAmount);
			float endPosition = this.getWalkingEndPosition(movementAmount);
			float moveTime = Mathf.Abs(endPosition - startPosition) / p.movementSpeed;
			base.animator.SetBool("FacingLeft", this.facingLeft);
			base.animator.SetBool("Walking", true);
			for (;;)
			{
				idleT += CupheadTime.FixedDelta;
				Vector3 previousPosition = base.transform.position;
				if (moveT < moveTime)
				{
					moveT += CupheadTime.FixedDelta;
					if (p.hasEasing)
					{
						float t = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, moveT / moveTime);
						base.transform.SetPosition(new float?(Mathf.Lerp(startPosition, endPosition, t)), null, null);
					}
					else if (this.goingLeft && base.transform.position.x > endPosition)
					{
						base.transform.position += Vector3.left * p.movementSpeed * CupheadTime.FixedDelta;
					}
					else if (!this.goingLeft && base.transform.position.x < endPosition)
					{
						base.transform.position += Vector3.right * p.movementSpeed * CupheadTime.FixedDelta;
					}
				}
				else
				{
					if (idleT > idleTime)
					{
						break;
					}
					this.goingLeft = !this.goingLeft;
					base.transform.SetPosition(new float?(endPosition), null, null);
					startPosition = endPosition;
					endPosition = this.getWalkingEndPosition(this.movementPattern.PopFloat());
					moveTime = Mathf.Abs(endPosition - startPosition) / p.movementSpeed;
					moveT = 0f;
				}
				this.updateAnimatorSpeed(base.transform.position, previousPosition);
				yield return wait;
			}
			this.goingLeft = !this.goingLeft;
		}
		this.CheckTaunt();
		yield break;
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000DE29C File Offset: 0x000DC69C
	private bool chooseGoingLeft(float movementAmount)
	{
		bool flag = Rand.Bool();
		float num;
		if (this.facingLeft)
		{
			num = ((!flag) ? (640f - this.positionBoundaryInset.minimum) : (640f - this.positionBoundaryInset.maximum));
		}
		else
		{
			num = ((!flag) ? (-640f + this.positionBoundaryInset.maximum) : (-640f + this.positionBoundaryInset.minimum));
		}
		float num2 = num - base.transform.position.x;
		if (Mathf.Abs(num2 / movementAmount) < 0.5f)
		{
			flag = !flag;
		}
		return flag;
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x000DE34C File Offset: 0x000DC74C
	private float getWalkingEndPosition(float movementAmount)
	{
		float endPosition = base.transform.position.x + ((!this.goingLeft) ? movementAmount : (-movementAmount)) * 2f;
		return this.clampEndPosition(endPosition, this.facingLeft);
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x000DE398 File Offset: 0x000DC798
	private void CheckTaunt()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		float tauntDistance = base.properties.CurrentState.taunt.tauntDistance;
		float num = Mathf.Abs(player.transform.position.x - base.transform.position.x);
		float num2 = (!(player2 != null) || player2.IsDead) ? num : Mathf.Abs(player2.transform.position.x - base.transform.position.x);
		if (num > tauntDistance && num2 > tauntDistance)
		{
			if (this.tauntAttackCounter <= 0)
			{
				this.isTauntAttack = true;
				base.StartCoroutine(this.long_cr());
			}
			else
			{
				base.StartCoroutine(this.taunt_cr());
			}
		}
		else
		{
			this.state = ChessKnightLevelKnight.State.Idle;
		}
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x000DE494 File Offset: 0x000DC894
	private IEnumerator taunt_cr()
	{
		this.state = ChessKnightLevelKnight.State.Taunt;
		base.animator.SetBool("Taunting", true);
		base.animator.SetBool("Walking", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.taunt.tauntDuration);
		base.animator.SetBool("Taunting", false);
		if (this.shouldBackDash())
		{
			base.animator.SetTrigger("BackDash");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Taunt.Exit", 0, false, false, true);
			yield return base.StartCoroutine(this.backDash_cr());
		}
		else if (this.shouldTurn())
		{
			base.animator.SetTrigger("Turn");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Taunt.Exit", 0, false, false, true);
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Turn", 0, false, false, true);
			this.turn();
		}
		else
		{
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Taunt.Exit", 0, false, false, true);
		}
		this.tauntAttackCounter--;
		this.EndAttack();
		yield break;
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x000DE4AF File Offset: 0x000DC8AF
	public void Short()
	{
		this.state = ChessKnightLevelKnight.State.Short;
		base.StartCoroutine(this.short_cr());
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x000DE4C8 File Offset: 0x000DC8C8
	private IEnumerator short_cr()
	{
		this.tauntAttackCounter = this.numberTauntString.PopInt();
		LevelProperties.ChessKnight.ShortAttack p = base.properties.CurrentState.shortAttack;
		base.animator.SetTrigger("RegularAttack");
		base.animator.SetBool("Walking", false);
		yield return CupheadTime.WaitForSeconds(this, p.shortAntiDuration);
		base.animator.SetTrigger("OnAttack");
		yield return CupheadTime.WaitForSeconds(this, p.shortAttackDuration);
		base.animator.SetTrigger("OnAttackEnd");
		yield return base.animator.WaitForAnimationToStart(this, "RegularAttack.RecoveryHold", false);
		yield return CupheadTime.WaitForSeconds(this, p.shortRecoveryDuration);
		if (this.shouldBackDash())
		{
			base.animator.SetTrigger("BackDash");
			yield return base.StartCoroutine(this.backDash_cr());
		}
		else
		{
			base.animator.SetTrigger("ExitRecovery");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "RegularAttack.RecoveryExit", 0, false, false, true);
		}
		this.EndAttack();
		yield break;
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x000DE4E3 File Offset: 0x000DC8E3
	public void Long()
	{
		this.isTauntAttack = false;
		this.state = ChessKnightLevelKnight.State.Long;
		base.StartCoroutine(this.long_cr());
	}

	// Token: 0x0600189A RID: 6298 RVA: 0x000DE500 File Offset: 0x000DC900
	private IEnumerator long_cr()
	{
		this.tauntAttackCounter = this.numberTauntString.PopInt();
		float antiDuration = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longAntiDuration : base.properties.CurrentState.tauntAttack.tauntAttackAntiDuration;
		float attackTime = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longAttackTime : base.properties.CurrentState.tauntAttack.tauntAttackTime;
		float attackDist = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longAttackDist : base.properties.CurrentState.tauntAttack.tauntAttackDist;
		float attackRecovery = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longRecoveryDuration : base.properties.CurrentState.tauntAttack.tauntAttackRecoveryDuration;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		base.animator.SetBool("Walking", false);
		if (this.isTauntAttack)
		{
			base.animator.Play("DashAttack.Anticipation");
			base.animator.SetBool("Taunting", false);
		}
		else
		{
			base.animator.SetTrigger("DashAttack");
		}
		if (antiDuration > 0.7f)
		{
			yield return CupheadTime.WaitForSeconds(this, antiDuration);
			base.animator.SetTrigger("OnAttack");
		}
		else
		{
			if (!this.isTauntAttack)
			{
				yield return base.animator.WaitForAnimationToStart(this, "DashAttack.Anticipation", false);
			}
			yield return CupheadTime.WaitForSeconds(this, Mathf.Max(antiDuration, 0.20833333f));
			base.animator.Play("DashAttack.Attack");
			base.animator.Update(0f);
		}
		float t = 0f;
		float time = attackTime;
		float startPosition = base.transform.position.x;
		float endPosition = (!this.facingLeft) ? (base.transform.position.x + attackDist) : (base.transform.position.x - attackDist);
		endPosition = this.clampEndPosition(endPosition, !this.facingLeft);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(startPosition, endPosition, t / time)), null, null);
			yield return wait;
		}
		base.animator.SetTrigger("OnAttackEnd");
		this.recovery(attackRecovery);
		this.SFX_KOG_KNIGHT_RecoverFoley();
		yield break;
	}

	// Token: 0x0600189B RID: 6299 RVA: 0x000DE51B File Offset: 0x000DC91B
	public void Up()
	{
		this.state = ChessKnightLevelKnight.State.Up;
		base.StartCoroutine(this.up_cr());
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x000DE534 File Offset: 0x000DC934
	private IEnumerator up_cr()
	{
		this.tauntAttackCounter = this.numberTauntString.PopInt();
		LevelProperties.ChessKnight.UpAttack p = base.properties.CurrentState.upAttack;
		base.animator.SetTrigger("MoonAttack");
		base.animator.SetBool("Walking", false);
		yield return CupheadTime.WaitForSeconds(this, p.upAntiDuration);
		base.animator.SetTrigger("OnAttack");
		yield return base.animator.WaitForAnimationToStart(this, "Recovery", false);
		this.recovery(p.upRecoveryDuration);
		this.SFX_KOG_KNIGHT_RecoverFoley();
		yield break;
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x000DE54F File Offset: 0x000DC94F
	private void recovery(float duration)
	{
		base.StartCoroutine(this.recovery_cr(duration));
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x000DE560 File Offset: 0x000DC960
	private IEnumerator recovery_cr(float duration)
	{
		this.SFX_KOG_KNIGHT_MoonSlash_Panting();
		yield return CupheadTime.WaitForSeconds(this, duration);
		if (this.shouldBackDash())
		{
			base.animator.SetTrigger("BackDash");
			yield return base.StartCoroutine(this.backDash_cr());
		}
		else if (this.shouldTurn())
		{
			base.animator.SetTrigger("Turn");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Turn", 0, false, false, true);
			this.turn();
		}
		else
		{
			base.animator.SetTrigger("ExitRecovery");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "RecoveryExit", 0, false, false, true);
		}
		this.SFX_KOG_KNIGHT_MoonSlash_PantingStop();
		this.EndAttack();
		yield break;
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x000DE584 File Offset: 0x000DC984
	private bool shouldFaceLeft()
	{
		if (this.targetPlayer == null || this.targetPlayer.IsDead)
		{
			this.targetPlayer = PlayerManager.GetNext();
		}
		return this.targetPlayer.transform.position.x < base.transform.position.x;
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x000DE5EA File Offset: 0x000DC9EA
	private bool shouldTurn()
	{
		return this.shouldFaceLeft() != this.facingLeft;
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x000DE600 File Offset: 0x000DCA00
	private void turn()
	{
		this.facingLeft = !this.facingLeft;
		base.transform.SetScale(new float?((float)((!this.facingLeft) ? -1 : 1)), null, null);
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000DE654 File Offset: 0x000DCA54
	private bool shouldBackDash()
	{
		bool flag = this.shouldFaceLeft();
		float num = (float)((!flag) ? 640 : -640);
		float num2 = 640f;
		float num3 = Mathf.Abs(num - base.transform.position.x);
		bool flag2 = false;
		if (PlayerManager.BothPlayersActive())
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (Mathf.Sign(base.transform.position.x - player.transform.position.x) != Mathf.Sign(base.transform.position.x - player2.transform.position.x))
			{
				flag2 = true;
			}
		}
		return num3 < num2 && !flag2;
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000DE734 File Offset: 0x000DCB34
	private IEnumerator backDash_cr()
	{
		float returnSpeed = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longReturnSpeed : base.properties.CurrentState.tauntAttack.tauntAttackReturnSpeed;
		this.facingLeft = this.shouldFaceLeft();
		base.transform.SetScale(new float?((float)((!this.facingLeft) ? -1 : 1)), null, null);
		float startPosition = base.transform.position.x;
		float endPosition = (!this.facingLeft) ? (-640f + this.positionBoundaryInset.minimum) : (640f - this.positionBoundaryInset.minimum);
		float time = Mathf.Abs(endPosition - base.transform.position.x) / returnSpeed;
		float t = 0f;
		base.StartCoroutine(this.backDashAnimation_cr());
		Effect smoke = this.backDashSmoke.Spawn(this.smokeSpawnPoint.position);
		smoke.transform.SetScale(new float?((float)((!this.facingLeft) ? -1 : 1)), null, null);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			Vector3 previousPosition = base.transform.position;
			t += CupheadTime.FixedDelta;
			if (base.properties.CurrentState.movement.hasEasing)
			{
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
				base.transform.SetPosition(new float?(Mathf.Lerp(startPosition, endPosition, t2)), null, null);
			}
			else if (this.goingLeft && base.transform.position.x > endPosition)
			{
				base.transform.position += Vector3.left * returnSpeed * CupheadTime.FixedDelta;
			}
			else if (!this.goingLeft && base.transform.position.x < endPosition)
			{
				base.transform.position += Vector3.right * returnSpeed * CupheadTime.FixedDelta;
			}
			this.updateAnimatorSpeed(base.transform.position, previousPosition);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x000DE750 File Offset: 0x000DCB50
	private IEnumerator backDashAnimation_cr()
	{
		yield return base.animator.WaitForNormalizedTime(this, 1f, "BackDash.Dash", 0, false, false, true);
		this.SFX_KOG_KNIGHT_RecoverFoley();
		base.animator.SetBool("FacingLeft", this.facingLeft);
		base.animator.SetBool("Walking", true);
		yield break;
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x000DE76C File Offset: 0x000DCB6C
	private void death()
	{
		this.StopAllCoroutines();
		this.SFX_KOG_KNIGHT_Die();
		base.animator.SetBool("Walking", false);
		base.animator.SetTrigger("Death");
		for (int i = 0; i < this.deathArmor.Length; i++)
		{
			SpriteDeathPartsDLC spriteDeathPartsDLC = UnityEngine.Object.Instantiate<SpriteDeathPartsDLC>(this.deathArmor[i], this.deathArmorSpawns[i].position, Quaternion.identity);
			spriteDeathPartsDLC.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
			spriteDeathPartsDLC.transform.parent = base.transform;
			spriteDeathPartsDLC.SetVelocity(new Vector3(spriteDeathPartsDLC.transform.localPosition.x * 3f * base.transform.localScale.x, spriteDeathPartsDLC.transform.localPosition.y * 6f + 800f));
		}
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x000DE874 File Offset: 0x000DCC74
	private void updateAnimatorSpeed(Vector3 currentPosition, Vector3 previousPosition)
	{
		if (CupheadTime.IsPaused())
		{
			return;
		}
		Vector3 vector = (currentPosition - previousPosition) / CupheadTime.FixedDelta;
		base.animator.SetFloat("XSpeed", vector.x);
		float value = MathUtilities.LerpMapping(Mathf.Abs(vector.x), 0f, this.maximumLegVelocity, this.legSpeedMultiplierRange.minimum, this.legSpeedMultiplierRange.maximum, true);
		base.animator.SetFloat("LegSpeed", value);
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x000DE8FC File Offset: 0x000DCCFC
	private float clampEndPosition(float endPosition, bool isRightSide)
	{
		if (isRightSide)
		{
			float min = 640f - this.positionBoundaryInset.maximum;
			float max = 640f - this.positionBoundaryInset.minimum;
			endPosition = Mathf.Clamp(endPosition, min, max);
		}
		else
		{
			float min2 = -640f + this.positionBoundaryInset.minimum;
			float max2 = -640f + this.positionBoundaryInset.maximum;
			endPosition = Mathf.Clamp(endPosition, min2, max2);
		}
		return endPosition;
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000DE971 File Offset: 0x000DCD71
	private void AnimationEvent_SFX_KOG_KNIGHT_AttackUpwards_Stab()
	{
		AudioManager.Play("sfx_dlc_kog_knight_attackupwards_stab");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_attackupwards_stab");
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x000DE98D File Offset: 0x000DCD8D
	private void AnimationEvent_SFX_KOG_KNIGHT_AttackUpwards_Start()
	{
		AudioManager.Play("sfx_dlc_kog_knight_attackupwards_start");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_attackupwards_start");
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x000DE9A9 File Offset: 0x000DCDA9
	private void SFX_KOG_KNIGHT_Die()
	{
		AudioManager.Play("sfx_dlc_kog_knight_die");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_die");
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x000DE9C5 File Offset: 0x000DCDC5
	private void AnimationEvent_SFX_KOG_KNIGHT_Foley_Walk()
	{
		AudioManager.Play("sfx_dlc_kog_knight_foley_walk");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_foley_walk");
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x000DE9E1 File Offset: 0x000DCDE1
	private void AnimationEvent_SFX_KOG_KNIGHT_Intro_ShieldBash()
	{
		AudioManager.Play("sfx_dlc_kog_knight_intro_shieldbash");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_intro_shieldbash");
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x000DE9FD File Offset: 0x000DCDFD
	private void AnimationEvent_SFX_KOG_KNIGHT_Intro_Visor()
	{
		AudioManager.Play("sfx_dlc_kog_knight_intro_visor");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_intro_visor");
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x000DEA19 File Offset: 0x000DCE19
	private void AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_End()
	{
		AudioManager.Stop("sfx_dlc_kog_knight_moonslash_panting");
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_end");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_end");
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x000DEA3F File Offset: 0x000DCE3F
	private void SFX_KOG_KNIGHT_MoonSlash_Panting()
	{
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_panting");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_panting");
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x000DEA5B File Offset: 0x000DCE5B
	private void SFX_KOG_KNIGHT_MoonSlash_PantingStop()
	{
		AudioManager.Stop("sfx_dlc_kog_knight_moonslash_panting");
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x000DEA67 File Offset: 0x000DCE67
	private void AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_Start()
	{
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_start");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_start");
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x000DEA83 File Offset: 0x000DCE83
	private void AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_Swing()
	{
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_swing");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_swing");
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x000DEA9F File Offset: 0x000DCE9F
	private void AnimationEvent_SFX_KOG_KNIGHT_TauntHand()
	{
		AudioManager.Play("sfx_dlc_kog_knight_taunthand");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_taunthand");
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x000DEABB File Offset: 0x000DCEBB
	private void AnimationEvent_SFX_KOG_KNIGHT_Dash_End()
	{
		AudioManager.Play("sfx_dlc_kog_knight_dash_end");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_end");
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x000DEAD7 File Offset: 0x000DCED7
	private void SFX_KOG_KNIGHT_RecoverFoley()
	{
		AudioManager.Play("sfx_dlc_kog_knight_recoverfoley");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_recoverfoley");
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x000DEAF3 File Offset: 0x000DCEF3
	private void AnimationEvent_SFX_KOG_KNIGHT_Dash_Start()
	{
		AudioManager.Play("sfx_dlc_kog_knight_dash_start");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_start");
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x000DEB0F File Offset: 0x000DCF0F
	private void AnimationEvent_SFX_KOG_KNIGHT_Dash_Attack()
	{
		AudioManager.Play("sfx_dlc_kog_knight_dash_attack");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_attack");
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000DEB2B File Offset: 0x000DCF2B
	private void AnimationEvent_SFX_KOG_KNIGHT_Vocal_Attack()
	{
		AudioManager.Play("sfx_dlc_kog_knight_vocal_attack");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_vocal_attack");
	}

	// Token: 0x040021A6 RID: 8614
	[SerializeField]
	private ParrySwitch pink;

	// Token: 0x040021A7 RID: 8615
	[SerializeField]
	private CollisionChild swordHitbox;

	// Token: 0x040021A8 RID: 8616
	[SerializeField]
	private CollisionChild upHitbox;

	// Token: 0x040021A9 RID: 8617
	[SerializeField]
	private Rangef positionBoundaryInset;

	// Token: 0x040021AA RID: 8618
	[SerializeField]
	private Transform smokeSpawnPoint;

	// Token: 0x040021AB RID: 8619
	[SerializeField]
	private Effect backDashSmoke;

	// Token: 0x040021AC RID: 8620
	[SerializeField]
	private Rangef legSpeedMultiplierRange;

	// Token: 0x040021AD RID: 8621
	[SerializeField]
	private float maximumLegVelocity;

	// Token: 0x040021AE RID: 8622
	[SerializeField]
	private SpriteDeathPartsDLC[] deathArmor;

	// Token: 0x040021AF RID: 8623
	[SerializeField]
	private Transform[] deathArmorSpawns;

	// Token: 0x040021B0 RID: 8624
	[SerializeField]
	private HitFlash hitFlash;

	// Token: 0x040021B1 RID: 8625
	private DamageDealer damageDealer;

	// Token: 0x040021B2 RID: 8626
	private float battleStartPosition;

	// Token: 0x040021B3 RID: 8627
	private bool goingLeft = true;

	// Token: 0x040021B4 RID: 8628
	private bool facingLeft = true;

	// Token: 0x040021B5 RID: 8629
	private AbstractPlayerController targetPlayer;

	// Token: 0x040021B6 RID: 8630
	private PatternString attackIntervalPattern;

	// Token: 0x040021B7 RID: 8631
	private PatternString movementPattern;

	// Token: 0x040021B8 RID: 8632
	private PatternString numberTauntString;

	// Token: 0x040021B9 RID: 8633
	public int tauntAttackCounter;

	// Token: 0x040021BA RID: 8634
	private bool isTauntAttack;

	// Token: 0x02000540 RID: 1344
	public enum State
	{
		// Token: 0x040021BD RID: 8637
		Intro,
		// Token: 0x040021BE RID: 8638
		Move,
		// Token: 0x040021BF RID: 8639
		Idle,
		// Token: 0x040021C0 RID: 8640
		Short,
		// Token: 0x040021C1 RID: 8641
		Long,
		// Token: 0x040021C2 RID: 8642
		Up,
		// Token: 0x040021C3 RID: 8643
		Taunt,
		// Token: 0x040021C4 RID: 8644
		Death
	}
}
