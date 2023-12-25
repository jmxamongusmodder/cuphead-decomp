using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A6C RID: 2668
public class WeaponBoomerangProjectile : AbstractProjectile
{
	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06003FA9 RID: 16297 RVA: 0x0022BC3F File Offset: 0x0022A03F
	protected override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06003FAA RID: 16298 RVA: 0x0022BC48 File Offset: 0x0022A048
	protected override void Start()
	{
		base.Start();
		this.forwardDir = MathUtils.AngleToDirection(base.transform.rotation.eulerAngles.z);
		this.lateralDir = new Vector2(-this.forwardDir.y, this.forwardDir.x);
		this.lateralDir *= this.player.motor.TrueLookDirection.x;
		this.DestroyDistance = 0f;
		if (this.isEx)
		{
			this.trailPositions = new Vector2[6];
			for (int i = 0; i < this.trailPositions.Length; i++)
			{
				this.trailPositions[i] = base.transform.position;
			}
			base.StartCoroutine(this.ex_cr());
		}
		else
		{
			base.StartCoroutine(this.basic_cr());
		}
	}

	// Token: 0x06003FAB RID: 16299 RVA: 0x0022BD4C File Offset: 0x0022A14C
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		if (this.wasCaught)
		{
			this.Die();
		}
		if (this.isEx && this.hasTurned && (base.transform.position - this.player.center).magnitude < this.player.colliderManager.Width / 2f + base.GetComponent<CircleCollider2D>().radius)
		{
			this.wasCaught = true;
		}
		bool flag = CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 150f));
		base.GetComponent<Collider2D>().enabled = flag;
		if (!flag && this.headedOffscreen)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this.isEx)
		{
			this.updateTrails();
		}
	}

	// Token: 0x06003FAC RID: 16300 RVA: 0x0022BE4C File Offset: 0x0022A24C
	private void updateTrails()
	{
		int num = this.currentPositionIndex - 2;
		if (num < 0)
		{
			num += this.trailPositions.Length;
		}
		int num2 = this.currentPositionIndex - 5;
		if (num2 < 0)
		{
			num2 += this.trailPositions.Length;
		}
		this.trail1.position = this.trailPositions[num];
		this.trail2.position = this.trailPositions[num2];
		this.currentPositionIndex = (this.currentPositionIndex + 1) % this.trailPositions.Length;
		this.trailPositions[this.currentPositionIndex] = base.transform.position;
	}

	// Token: 0x06003FAD RID: 16301 RVA: 0x0022BF10 File Offset: 0x0022A310
	protected override void Die()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		base.Die();
		this.StopAllCoroutines();
		this.SetInt(AbstractProjectile.Variant, this.variant);
	}

	// Token: 0x06003FAE RID: 16302 RVA: 0x0022BF60 File Offset: 0x0022A360
	private IEnumerator basic_cr()
	{
		Vector2 startPos = base.transform.position;
		Vector2 turnPos = startPos + this.forwardDir * this.forwardDistance + this.lateralDir * this.lateralDistance * 0.5f;
		Vector2 returnPos = startPos + this.lateralDir * this.lateralDistance;
		float moveTime = this.forwardDistance / this.Speed * 1.5707964f;
		yield return base.StartCoroutine(this.move_cr(turnPos, EaseUtils.EaseType.easeOutSine, EaseUtils.EaseType.easeInSine, moveTime));
		this.hasTurned = true;
		yield return base.StartCoroutine(this.move_cr(returnPos, EaseUtils.EaseType.easeInSine, EaseUtils.EaseType.easeOutSine, moveTime));
		Vector2 velocity = this.Speed * -this.forwardDir;
		this.headedOffscreen = true;
		for (;;)
		{
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06003FAF RID: 16303 RVA: 0x0022BF7C File Offset: 0x0022A37C
	private IEnumerator ex_cr()
	{
		Vector2 startPos = base.transform.position;
		Vector2 turnPos = startPos + this.forwardDir * this.forwardDistance + this.lateralDir * this.lateralDistance * 0.5f;
		for (;;)
		{
			EaseUtils.EaseType ease = (!this.hasTurned) ? EaseUtils.EaseType.easeOutSine : EaseUtils.EaseType.easeInOutSine;
			float moveTime = ((!this.hasTurned) ? this.forwardDistance : (this.forwardDistance * 2f)) / this.Speed;
			yield return base.StartCoroutine(this.move_cr(turnPos, ease, ease, moveTime));
			this.hasTurned = true;
			startPos = base.transform.position;
			Vector2 playerPos = this.player.transform.position;
			turnPos = playerPos + (playerPos - startPos).normalized * this.forwardDistance;
		}
		yield break;
	}

	// Token: 0x06003FB0 RID: 16304 RVA: 0x0022BF98 File Offset: 0x0022A398
	private IEnumerator move_cr(Vector2 endPos, EaseUtils.EaseType forwardEaseType, EaseUtils.EaseType lateralEaseType, float time)
	{
		float t = 0f;
		Vector2 startPos = base.transform.localPosition;
		Vector2 relativeEndPos = endPos - startPos;
		float forwardMovement = Vector2.Dot(this.forwardDir, relativeEndPos);
		float lateralMovement = Vector2.Dot(this.lateralDir, relativeEndPos);
		while (t < time)
		{
			while (this.timeUntilUnfreeze > 0f)
			{
				this.timeUntilUnfreeze -= CupheadTime.FixedDelta;
				yield return new WaitForFixedUpdate();
			}
			base.transform.position = startPos + this.forwardDir * EaseUtils.Ease(forwardEaseType, 0f, forwardMovement, t / time) + this.lateralDir * EaseUtils.Ease(lateralEaseType, 0f, lateralMovement, t / time);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.position = endPos;
		yield break;
	}

	// Token: 0x06003FB1 RID: 16305 RVA: 0x0022BFD0 File Offset: 0x0022A3D0
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		float num = this.damageDealer.DealDamage(hit);
		if (this.isEx)
		{
			this.totalDamage += num;
			if (this.totalDamage > this.maxDamage)
			{
				this.Die();
			}
			if (num > 0f)
			{
				this.hitFXPrefab.Create(base.transform.position);
				this.timeUntilUnfreeze = this.hitFreezeTime;
				AudioManager.Play("player_ex_impact_hit");
				this.emitAudioFromObject.Add("player_ex_impact_hit");
			}
		}
	}

	// Token: 0x06003FB2 RID: 16306 RVA: 0x0022C06A File Offset: 0x0022A46A
	public void SetPink(bool pink)
	{
		if (pink)
		{
			this.SetParryable(true);
			this.variant = 2;
		}
		else
		{
			this.SetParryable(false);
			this.variant = UnityEngine.Random.Range(0, 2);
		}
		this.SetInt(AbstractProjectile.Variant, this.variant);
	}

	// Token: 0x06003FB3 RID: 16307 RVA: 0x0022C0AC File Offset: 0x0022A4AC
	protected override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionDie(hit, phase);
		if (base.tag == "PlayerProjectile" && phase == CollisionPhase.Enter)
		{
			if (hit.GetComponent<DamageReceiver>() && hit.GetComponent<DamageReceiver>().enabled)
			{
				AudioManager.Play("player_shoot_hit_cuphead");
			}
			else
			{
				AudioManager.Play("player_weapon_peashot_miss");
			}
		}
	}

	// Token: 0x04004685 RID: 18053
	private const float TurnTimeRatio = 0.4f;

	// Token: 0x04004686 RID: 18054
	public float Speed;

	// Token: 0x04004687 RID: 18055
	public float forwardDistance;

	// Token: 0x04004688 RID: 18056
	public float lateralDistance;

	// Token: 0x04004689 RID: 18057
	public float maxDamage;

	// Token: 0x0400468A RID: 18058
	public float hitFreezeTime;

	// Token: 0x0400468B RID: 18059
	public LevelPlayerController player;

	// Token: 0x0400468C RID: 18060
	[SerializeField]
	private bool isEx;

	// Token: 0x0400468D RID: 18061
	[SerializeField]
	private Transform trail1;

	// Token: 0x0400468E RID: 18062
	[SerializeField]
	private Transform trail2;

	// Token: 0x0400468F RID: 18063
	[SerializeField]
	private Effect hitFXPrefab;

	// Token: 0x04004690 RID: 18064
	private Vector2[] trailPositions;

	// Token: 0x04004691 RID: 18065
	private int currentPositionIndex;

	// Token: 0x04004692 RID: 18066
	private const int trailFrameDelay = 3;

	// Token: 0x04004693 RID: 18067
	private Vector2 forwardDir;

	// Token: 0x04004694 RID: 18068
	private Vector2 lateralDir;

	// Token: 0x04004695 RID: 18069
	private bool hasTurned;

	// Token: 0x04004696 RID: 18070
	private bool wasCaught;

	// Token: 0x04004697 RID: 18071
	private bool headedOffscreen;

	// Token: 0x04004698 RID: 18072
	private float totalDamage;

	// Token: 0x04004699 RID: 18073
	private int variant;

	// Token: 0x0400469A RID: 18074
	private float timeUntilUnfreeze;
}
