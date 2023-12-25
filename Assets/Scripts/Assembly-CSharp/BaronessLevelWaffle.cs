using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004F7 RID: 1271
public class BaronessLevelWaffle : BaronessLevelMiniBossBase
{
	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06001655 RID: 5717 RVA: 0x000C7F94 File Offset: 0x000C6394
	// (set) Token: 0x06001656 RID: 5718 RVA: 0x000C7F9C File Offset: 0x000C639C
	public BaronessLevelWaffle.State state { get; private set; }

	// Token: 0x06001657 RID: 5719 RVA: 0x000C7FA8 File Offset: 0x000C63A8
	protected override void Awake()
	{
		base.Awake();
		float num = (float)UnityEngine.Random.Range(0, 2);
		this.pathA = (num == 0f);
		this.check = true;
		this.isDead = false;
		this.isDying = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.mouth.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = true;
		for (int i = 0; i < this.diagonalPieces.Length; i++)
		{
			this.diagonalPieces[i].wafflepiece.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
			this.diagonalPieces[i].wafflepiece.GetComponent<Collider2D>().enabled = false;
			this.diagonalPieces[i].wafflepiece.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		}
		for (int j = 0; j < this.straightPieces.Length; j++)
		{
			this.straightPieces[j].wafflepiece.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
			this.straightPieces[j].wafflepiece.GetComponent<Collider2D>().enabled = false;
			this.straightPieces[j].wafflepiece.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		}
	}

	// Token: 0x06001658 RID: 5720 RVA: 0x000C813B File Offset: 0x000C653B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x000C815C File Offset: 0x000C655C
	public void Init(LevelProperties.Baroness.Waffle properties, Vector2 pos, Transform pivot, float speed, float health)
	{
		this.properties = properties;
		this.speed = speed;
		this.health = health;
		base.transform.position = pos;
		this.pivotPoint = pivot;
		this.state = BaronessLevelWaffle.State.Enter;
		base.StartCoroutine(this.enter_cr());
		base.StartCoroutine(this.switchLayer_cr());
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x000C81B9 File Offset: 0x000C65B9
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x000C81D4 File Offset: 0x000C65D4
	private IEnumerator switchLayer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
		yield break;
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x000C81F0 File Offset: 0x000C65F0
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health < 0f && this.state == BaronessLevelWaffle.State.Move)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			this.isDead = true;
			base.StartCoroutine(this.death_cr());
		}
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x000C827C File Offset: 0x000C667C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.explosion = null;
		this.explosionReverse = null;
		this.straightPieces = null;
		this.diagonalPieces = null;
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x000C82A0 File Offset: 0x000C66A0
	private IEnumerator enter_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.originalPivotPos = this.pivotPoint.transform.position;
		if (this.pathA)
		{
			this.startPos = this.pivotPoint.position + Vector3.right * this.loopSize;
			this.angle = -1.5707964f;
		}
		else
		{
			this.startPos = this.pivotPoint.position + Vector3.down * this.loopSize;
			this.angle = 3.1415927f;
			this.speed = -this.speed;
		}
		while (base.transform.position != this.startPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.startPos, this.properties.movementSpeed * 300f * CupheadTime.FixedDelta);
			yield return wait;
		}
		this.StartCircle();
		yield return null;
		yield break;
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x000C82BB File Offset: 0x000C66BB
	private void StartCircle()
	{
		this.state = BaronessLevelWaffle.State.Move;
		base.StartCoroutine(this.circle_cr());
		base.StartCoroutine(this.check_attack_cr());
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x000C82E0 File Offset: 0x000C66E0
	private IEnumerator circle_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!this.isDead)
		{
			if (this.state == BaronessLevelWaffle.State.Move)
			{
				this.MovePivot();
				this.PathMovement();
				this.CheckIfTurn();
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x000C82FC File Offset: 0x000C66FC
	private void MovePivot()
	{
		Vector3 position = this.pivotPoint.transform.position;
		float pivotPointMoveAmount = this.properties.pivotPointMoveAmount;
		if (this.pivotMovingLeft)
		{
			position.x = Mathf.MoveTowards(this.pivotPoint.transform.position.x, this.originalPivotPos.x - pivotPointMoveAmount, this.properties.XAxisSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
			this.pivotMovingLeft = (this.pivotPoint.transform.position.x != this.originalPivotPos.x - pivotPointMoveAmount);
		}
		else
		{
			position.x = Mathf.MoveTowards(this.pivotPoint.transform.position.x, this.originalPivotPos.x + pivotPointMoveAmount, this.properties.XAxisSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
			this.pivotMovingLeft = (this.pivotPoint.transform.position.x == this.originalPivotPos.x + pivotPointMoveAmount);
		}
		this.pivotPoint.transform.position = position;
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x000C844C File Offset: 0x000C684C
	private void PathMovement()
	{
		this.angle += this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
		Vector3 a = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		Vector3 b = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += a + b;
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x000C84F4 File Offset: 0x000C68F4
	private void CheckIfTurn()
	{
		if (this.check)
		{
			if (base.transform.position.y < this.pivotPoint.position.y)
			{
				if (!this.onBottom)
				{
					base.StartCoroutine(this.turn_cr());
					this.check = false;
				}
				this.onBottom = true;
			}
			else
			{
				if (this.onBottom)
				{
					base.StartCoroutine(this.turn_cr());
					this.check = false;
				}
				this.onBottom = false;
			}
		}
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x000C8588 File Offset: 0x000C6988
	private IEnumerator turn_cr()
	{
		base.animator.SetBool("Turn", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Waffle_Turn", false, true);
		base.animator.SetBool("Turn", false);
		this.check = true;
		yield return null;
		yield break;
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x000C85A4 File Offset: 0x000C69A4
	private void Turn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x000C85EC File Offset: 0x000C69EC
	private IEnumerator check_attack_cr()
	{
		if (!this.isDead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.attackDelayRange.RandomFloat());
			base.StartCoroutine(this.attack_cr());
			this.state = BaronessLevelWaffle.State.Attack;
			while (this.state == BaronessLevelWaffle.State.Attack)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x000C8608 File Offset: 0x000C6A08
	private IEnumerator attack_cr()
	{
		if (!this.isDead)
		{
			base.animator.Play("Waffle_Tuck_Start");
			base.GetComponent<Collider2D>().enabled = false;
			float randomValue = (float)UnityEngine.Random.Range(0, 2);
			this.diagFirst = (randomValue == 0f);
			yield return CupheadTime.WaitForSeconds(this, this.properties.anticipation);
			base.animator.SetTrigger("Continue");
			base.StartCoroutine(this.waffle_pieces((!this.diagFirst) ? this.straightPieces : this.diagonalPieces, true));
			yield return CupheadTime.WaitForSeconds(this, this.properties.explodeTwoDuration);
			base.StartCoroutine(this.waffle_pieces((!this.diagFirst) ? this.diagonalPieces : this.straightPieces, false));
		}
		yield break;
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x000C8624 File Offset: 0x000C6A24
	private void hitPause(int i)
	{
		if (this.diagonalPieces[i].wafflepiece.GetComponent<DamageReceiver>().IsHitPaused || this.straightPieces[i].wafflepiece.GetComponent<DamageReceiver>().IsHitPaused)
		{
			BaronessLevelWaffle.pauseValue = 0f;
		}
		else
		{
			BaronessLevelWaffle.pauseValue = 1f;
		}
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x000C8684 File Offset: 0x000C6A84
	private IEnumerator waffle_pieces(BaronessLevelWaffle.WafflePieces[] pieces, bool isFirst)
	{
		float t = 0f;
		float explodeTime = this.properties.explodeSpeed;
		float returnTime = this.properties.explodeReturnSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!this.switchedOn)
		{
			yield return null;
		}
		foreach (BaronessLevelWaffle.WafflePieces wafflePieces in pieces)
		{
			wafflePieces.wafflepiece.GetComponent<Collider2D>().enabled = true;
			wafflePieces.waffleFX.Play("Trail", 0, UnityEngine.Random.Range(0f, 0.6f));
		}
		if (isFirst)
		{
			this.explosion.Create(new Vector2(this.mouth.position.x, this.mouth.position.y - 20f));
		}
		while (t < explodeTime)
		{
			for (int j = 0; j < pieces.Length; j++)
			{
				t += CupheadTime.FixedDelta;
				float num = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / explodeTime);
				pieces[j].wafflepiece.transform.localPosition = Vector3.Lerp(pieces[j].wafflepiece.transform.localPosition.normalized, pieces[j].direction * this.properties.explodeDistance, num * BaronessLevelWaffle.pauseValue);
				this.hitPause(j);
			}
			yield return wait;
		}
		t = 0f;
		foreach (BaronessLevelWaffle.WafflePieces wafflePieces2 in pieces)
		{
			wafflePieces2.wafflepiece.GetComponent<Collider2D>().enabled = true;
			wafflePieces2.waffleFX.SetTrigger("Death");
		}
		if (isFirst)
		{
			this.explosionReverse.Create(new Vector2(this.mouth.position.x, this.mouth.position.y - 20f));
		}
		while (t < returnTime / 2f)
		{
			for (int l = 0; l < pieces.Length; l++)
			{
				t += CupheadTime.FixedDelta;
				float num2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / returnTime);
				pieces[l].wafflepiece.transform.localPosition = Vector3.Lerp(pieces[l].wafflepiece.transform.localPosition, Vector3.zero, num2 * BaronessLevelWaffle.pauseValue);
				this.hitPause(l);
			}
			yield return wait;
		}
		for (int m = 0; m < pieces.Length; m++)
		{
			pieces[m].wafflepiece.GetComponent<Collider2D>().enabled = false;
			pieces[m].wafflepiece.localPosition = Vector3.zero;
		}
		yield return null;
		if (!isFirst)
		{
			base.animator.SetBool("Split", false);
			base.GetComponent<Collider2D>().enabled = true;
			yield return base.animator.WaitForAnimationToEnd(this, "Waffle_Return", false, true);
			this.state = BaronessLevelWaffle.State.Move;
			this.switchedOn = false;
			yield return base.StartCoroutine(this.check_attack_cr());
		}
		yield break;
	}

	// Token: 0x0600166A RID: 5738 RVA: 0x000C86AD File Offset: 0x000C6AAD
	private void switchAnimation()
	{
		this.switchedOn = true;
		base.animator.SetBool("Split", true);
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x000C86C8 File Offset: 0x000C6AC8
	private IEnumerator destroyMouth_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Waffle_Explode_Death", false, true);
		this.mouth.GetComponent<Collider2D>().enabled = false;
		yield break;
	}

	// Token: 0x0600166C RID: 5740 RVA: 0x000C86E4 File Offset: 0x000C6AE4
	private IEnumerator death_cr()
	{
		this.pivotPoint.transform.position = this.originalPivotPos;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.StartExplosions();
		Collider2D collider = base.GetComponent<Collider2D>();
		this.isDead = true;
		this.state = BaronessLevelWaffle.State.Dying;
		this.isDying = true;
		base.animator.SetTrigger("Death");
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.SetBool("DeathExplode", true);
		bool explodeDeath = true;
		float untilDestroy = 1500f;
		base.StartCoroutine(this.destroyMouth_cr());
		while (explodeDeath)
		{
			collider.enabled = false;
			for (int i = 0; i < this.diagonalPieces.Length; i++)
			{
				this.diagonalPieces[i].distanceFromCenter = Vector3.Distance(this.diagonalPieces[i].wafflepiece.transform.localPosition, this.mouth.transform.localPosition);
				this.diagonalPieces[i].wafflepiece.GetComponent<Collider2D>().enabled = false;
				this.diagonalPieces[i].wafflepiece.transform.localPosition += this.diagonalPieces[i].direction * 700f * CupheadTime.FixedDelta;
				if (this.diagonalPieces[i].distanceFromCenter >= untilDestroy)
				{
					explodeDeath = false;
					break;
				}
			}
			for (int j = 0; j < this.straightPieces.Length; j++)
			{
				this.straightPieces[j].distanceFromCenter = Vector3.Distance(this.straightPieces[j].wafflepiece.transform.localPosition, this.mouth.transform.localPosition);
				this.straightPieces[j].wafflepiece.GetComponent<Collider2D>().enabled = false;
				this.straightPieces[j].wafflepiece.transform.localPosition += this.straightPieces[j].direction * 700f * CupheadTime.FixedDelta;
				if (this.straightPieces[j].distanceFromCenter >= untilDestroy)
				{
					explodeDeath = false;
					break;
				}
			}
			yield return wait;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x000C86FF File Offset: 0x000C6AFF
	private void SoundWaffleExplode()
	{
		AudioManager.Play("level_baroness_waffle_explode");
		this.emitAudioFromObject.Add("level_baroness_waffle_explode");
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x000C871B File Offset: 0x000C6B1B
	private void SoundWaffleWingflap()
	{
		AudioManager.Play("level_baroness_waffle_wingflap");
		this.emitAudioFromObject.Add("level_baroness_waffle_wingflap");
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x000C8737 File Offset: 0x000C6B37
	private void SoundWaffleReform()
	{
		AudioManager.Play("level_baroness_waffle_reform");
		this.emitAudioFromObject.Add("level_baroness_waffle_reform");
	}

	// Token: 0x04001F99 RID: 8089
	private static float pauseValue;

	// Token: 0x04001F9B RID: 8091
	[SerializeField]
	private Effect explosion;

	// Token: 0x04001F9C RID: 8092
	[SerializeField]
	private Effect explosionReverse;

	// Token: 0x04001F9D RID: 8093
	[SerializeField]
	private BaronessLevelWaffle.WafflePieces[] diagonalPieces;

	// Token: 0x04001F9E RID: 8094
	[SerializeField]
	private BaronessLevelWaffle.WafflePieces[] straightPieces;

	// Token: 0x04001F9F RID: 8095
	[SerializeField]
	private Transform mouth;

	// Token: 0x04001FA0 RID: 8096
	private LevelProperties.Baroness.Waffle properties;

	// Token: 0x04001FA1 RID: 8097
	private Transform pivotPoint;

	// Token: 0x04001FA2 RID: 8098
	private DamageDealer damageDealer;

	// Token: 0x04001FA3 RID: 8099
	private DamageReceiver damageReceiver;

	// Token: 0x04001FA4 RID: 8100
	private float health;

	// Token: 0x04001FA5 RID: 8101
	private float speed;

	// Token: 0x04001FA6 RID: 8102
	private float angle;

	// Token: 0x04001FA7 RID: 8103
	private float loopSize = 200f;

	// Token: 0x04001FA8 RID: 8104
	private bool switchedOn;

	// Token: 0x04001FA9 RID: 8105
	private bool pathA;

	// Token: 0x04001FAA RID: 8106
	private bool check;

	// Token: 0x04001FAB RID: 8107
	private bool onBottom;

	// Token: 0x04001FAC RID: 8108
	private bool diagFirst;

	// Token: 0x04001FAD RID: 8109
	private bool isDead;

	// Token: 0x04001FAE RID: 8110
	private bool pivotMovingLeft;

	// Token: 0x04001FAF RID: 8111
	private bool isHitPaused;

	// Token: 0x04001FB0 RID: 8112
	private Vector3 startPos;

	// Token: 0x04001FB1 RID: 8113
	private Vector3 originalPivotPos;

	// Token: 0x020004F8 RID: 1272
	public enum State
	{
		// Token: 0x04001FB3 RID: 8115
		Enter,
		// Token: 0x04001FB4 RID: 8116
		Move,
		// Token: 0x04001FB5 RID: 8117
		Attack,
		// Token: 0x04001FB6 RID: 8118
		Dying
	}

	// Token: 0x020004F9 RID: 1273
	[Serializable]
	public class WafflePieces
	{
		// Token: 0x04001FB7 RID: 8119
		public Transform wafflepiece;

		// Token: 0x04001FB8 RID: 8120
		public Animator waffleFX;

		// Token: 0x04001FB9 RID: 8121
		public Vector3 direction;

		// Token: 0x04001FBA RID: 8122
		public float distanceFromCenter;

		// Token: 0x04001FBB RID: 8123
		public DamageDealer damageDealer;
	}
}
