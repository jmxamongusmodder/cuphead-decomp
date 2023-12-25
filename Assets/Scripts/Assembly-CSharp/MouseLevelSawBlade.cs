using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006F4 RID: 1780
public class MouseLevelSawBlade : AbstractCollidableObject
{
	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06002621 RID: 9761 RVA: 0x0016458E File Offset: 0x0016298E
	// (set) Token: 0x06002622 RID: 9762 RVA: 0x00164596 File Offset: 0x00162996
	public MouseLevelSawBlade.State state { get; private set; }

	// Token: 0x06002623 RID: 9763 RVA: 0x0016459F File Offset: 0x0016299F
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002624 RID: 9764 RVA: 0x001645BE File Offset: 0x001629BE
	private void Start()
	{
		base.animator.SetFloat("SawID", (float)this.sawId / 5f);
		base.animator.SetFloat("StickID", (float)this.stickId / 7f);
	}

	// Token: 0x06002625 RID: 9765 RVA: 0x001645FA File Offset: 0x001629FA
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002626 RID: 9766 RVA: 0x00164612 File Offset: 0x00162A12
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002627 RID: 9767 RVA: 0x0016463B File Offset: 0x00162A3B
	public void Begin(LevelProperties.Mouse properties)
	{
		this.properties = properties;
		base.StartCoroutine(this.intro_cr());
		this.attackX = this.attackMinX;
		this.fullAttackX = this.attackMinX;
	}

	// Token: 0x06002628 RID: 9768 RVA: 0x00164669 File Offset: 0x00162A69
	public void Attack()
	{
		AudioManager.Play("level_mouse_buzzsaw_small");
		if (this.state == MouseLevelSawBlade.State.Idle)
		{
			this.state = MouseLevelSawBlade.State.Warning;
			base.StartCoroutine(this.attack_cr(false));
		}
	}

	// Token: 0x06002629 RID: 9769 RVA: 0x00164698 File Offset: 0x00162A98
	public void FullAttack()
	{
		if (this.state == MouseLevelSawBlade.State.Warning)
		{
			this.StopAllCoroutines();
		}
		else if (this.state == MouseLevelSawBlade.State.Idle)
		{
			this.state = MouseLevelSawBlade.State.Warning;
		}
		this.fullAttacking = true;
		base.StartCoroutine(this.attack_cr(true));
	}

	// Token: 0x0600262A RID: 9770 RVA: 0x001646E4 File Offset: 0x00162AE4
	private IEnumerator intro_cr()
	{
		float t = 0f;
		float introTime = Mathf.Abs(this.idleX - this.initX) / this.properties.CurrentState.brokenCanSawBlades.entrySpeed;
		while (t < introTime)
		{
			if (t > introTime * 0.75f)
			{
				base.GetComponent<Collider2D>().enabled = true;
			}
			base.transform.SetLocalPosition(new float?(Mathf.Lerp(this.initX, this.idleX, t / introTime)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(this.idleX), null, null);
		this.state = MouseLevelSawBlade.State.Idle;
		yield break;
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x00164700 File Offset: 0x00162B00
	private IEnumerator attack_cr(bool fullAttack)
	{
		LevelProperties.Mouse.BrokenCanSawBlades p = this.properties.CurrentState.brokenCanSawBlades;
		float t = 0f;
		while (t < p.delayBeforeAttack)
		{
			this.progress = t / p.delayBeforeAttack;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, this.idleX, this.attackMinX, this.progress);
			this.setX(x, fullAttack);
			t += CupheadTime.Delta;
			this.blade.transform.Rotate(Vector3.forward, this.rotateSpeed / 2f * CupheadTime.Delta);
			yield return null;
		}
		base.animator.SetBool("Attacking", true);
		this.state = MouseLevelSawBlade.State.Attack;
		float attackTime = 2f * Mathf.Abs(this.attackMaxX - this.attackMinX) / p.speed;
		t = 0f;
		while (t < attackTime)
		{
			float start = this.attackMinX;
			this.progress = t / attackTime * 2f;
			if (this.progress > 1f)
			{
				start = this.idleX;
				this.progress = 2f - this.progress;
				this.blade.transform.Rotate(Vector3.forward, EaseUtils.EaseInOutSine(0f, this.rotateSpeed, this.progress) * CupheadTime.Delta);
			}
			else
			{
				this.blade.transform.Rotate(Vector3.forward, this.rotateSpeed * CupheadTime.Delta);
			}
			float x2 = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, this.attackMaxX, this.progress);
			this.setX(x2, fullAttack);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.setX(this.idleX, fullAttack);
		if (fullAttack)
		{
			this.fullAttacking = false;
		}
		if (!this.fullAttacking)
		{
			base.animator.SetBool("Attacking", false);
			this.attackX = this.attackMinX;
			this.fullAttackX = this.attackMinX;
			this.state = MouseLevelSawBlade.State.Idle;
		}
		yield break;
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x00164722 File Offset: 0x00162B22
	public void Leave()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.leave_cr());
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x00164738 File Offset: 0x00162B38
	private IEnumerator leave_cr()
	{
		float leaveTime = 0f;
		if (this.state == MouseLevelSawBlade.State.Warning)
		{
			this.state = MouseLevelSawBlade.State.Idle;
			base.animator.SetBool("Attacking", false);
		}
		if (this.state == MouseLevelSawBlade.State.Attack)
		{
			leaveTime = Mathf.Abs(base.transform.localPosition.x - this.initX) / this.properties.CurrentState.brokenCanSawBlades.speed;
		}
		else
		{
			leaveTime = 2f;
		}
		float t = 0f;
		float startingX = base.transform.localPosition.x;
		while (t < leaveTime)
		{
			if (t > leaveTime * 0.25f)
			{
				base.GetComponent<Collider2D>().enabled = false;
			}
			base.transform.SetLocalPosition(new float?(Mathf.Lerp(startingX, this.initX, t / leaveTime)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(this.initX), null, null);
		yield break;
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x00164754 File Offset: 0x00162B54
	private void setX(float x, bool fullAttack)
	{
		if (fullAttack)
		{
			this.fullAttackX = x;
		}
		else
		{
			this.attackX = x;
		}
		if (this.idleX > 0f)
		{
			base.transform.SetLocalPosition(new float?(Mathf.Min(this.attackX, this.fullAttackX)), null, null);
		}
		else
		{
			base.transform.SetLocalPosition(new float?(Mathf.Max(this.attackX, this.fullAttackX)), null, null);
		}
	}

	// Token: 0x04002EA0 RID: 11936
	private const string SawParameterName = "SawID";

	// Token: 0x04002EA1 RID: 11937
	private const string StickParameterName = "StickID";

	// Token: 0x04002EA2 RID: 11938
	private const string AttackParameterName = "Attacking";

	// Token: 0x04002EA3 RID: 11939
	private const int SawIdMax = 6;

	// Token: 0x04002EA4 RID: 11940
	private const int StickIdMax = 8;

	// Token: 0x04002EA6 RID: 11942
	[SerializeField]
	private float initX;

	// Token: 0x04002EA7 RID: 11943
	[SerializeField]
	private float idleX;

	// Token: 0x04002EA8 RID: 11944
	[SerializeField]
	private float attackMinX;

	// Token: 0x04002EA9 RID: 11945
	[SerializeField]
	private float attackMaxX;

	// Token: 0x04002EAA RID: 11946
	[SerializeField]
	private Transform blade;

	// Token: 0x04002EAB RID: 11947
	[SerializeField]
	private float rotateSpeed;

	// Token: 0x04002EAC RID: 11948
	[Range(0f, 5f)]
	[SerializeField]
	private int sawId;

	// Token: 0x04002EAD RID: 11949
	[Range(0f, 7f)]
	[SerializeField]
	private int stickId;

	// Token: 0x04002EAE RID: 11950
	private LevelProperties.Mouse properties;

	// Token: 0x04002EAF RID: 11951
	private bool fullAttacking;

	// Token: 0x04002EB0 RID: 11952
	private bool goBackwards;

	// Token: 0x04002EB1 RID: 11953
	private float attackX;

	// Token: 0x04002EB2 RID: 11954
	private float fullAttackX;

	// Token: 0x04002EB3 RID: 11955
	private float progress;

	// Token: 0x04002EB4 RID: 11956
	private DamageDealer damageDealer;

	// Token: 0x020006F5 RID: 1781
	public enum State
	{
		// Token: 0x04002EB6 RID: 11958
		Init,
		// Token: 0x04002EB7 RID: 11959
		Idle,
		// Token: 0x04002EB8 RID: 11960
		Warning,
		// Token: 0x04002EB9 RID: 11961
		Attack
	}
}
