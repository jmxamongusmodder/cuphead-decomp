using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000842 RID: 2114
public class VeggiesLevelCarrot : LevelProperties.Veggies.Entity
{
	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x060030ED RID: 12525 RVA: 0x001CBFE2 File Offset: 0x001CA3E2
	// (set) Token: 0x060030EE RID: 12526 RVA: 0x001CBFEA File Offset: 0x001CA3EA
	public VeggiesLevelCarrot.State state { get; private set; }

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x060030EF RID: 12527 RVA: 0x001CBFF4 File Offset: 0x001CA3F4
	// (remove) Token: 0x060030F0 RID: 12528 RVA: 0x001CC02C File Offset: 0x001CA42C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x060030F1 RID: 12529 RVA: 0x001CC064 File Offset: 0x001CA464
	// (remove) Token: 0x060030F2 RID: 12530 RVA: 0x001CC09C File Offset: 0x001CA49C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VeggiesLevelCarrot.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x060030F3 RID: 12531 RVA: 0x001CC0D4 File Offset: 0x001CA4D4
	private void Start()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.mindLoop = UnityEngine.Object.Instantiate<AudioSource>(this.mindLoopPrefab);
		List<Transform> list = new List<Transform>(this.homingRoot.GetComponentsInChildren<Transform>());
		list.Remove(this.homingRoot);
		this.homingRoots = list.ToArray();
		this.SfxGround();
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x001CC12E File Offset: 0x001CA52E
	private void Update()
	{
		if (PauseManager.state == PauseManager.State.Paused)
		{
			this.mindLoop.Pause();
		}
		else
		{
			this.mindLoop.UnPause();
		}
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x001CC156 File Offset: 0x001CA556
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.mindLoop.Stop();
	}

	// Token: 0x060030F6 RID: 12534 RVA: 0x001CC16C File Offset: 0x001CA56C
	public override void LevelInit(LevelProperties.Veggies properties)
	{
		base.LevelInit(properties);
		this.carrot = base.properties.CurrentState.carrot;
		this.hp = (float)this.carrot.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x001CC1C0 File Offset: 0x001CA5C0
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x001CC21E File Offset: 0x001CA61E
	private void OnInAnimComplete()
	{
		base.transform.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.rings_cr());
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x001CC23E File Offset: 0x001CA63E
	private void Die()
	{
		this.dead = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x001CC25A File Offset: 0x001CA65A
	private void SfxGround()
	{
		AudioManager.Play("level_veggies_carrot_rise");
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x001CC268 File Offset: 0x001CA668
	private void ShootRegular()
	{
		this.spark.Create(this.straightRoot.position);
		this.straightRoot.LookAt2D(PlayerManager.GetNext().center);
		this.straightPrefab.Create(this, this.straightRoot.position, this.carrot.bulletSpeed, this.straightRoot.eulerAngles.z);
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x001CC2DC File Offset: 0x001CA6DC
	public void ShootHoming()
	{
		this.homingPrefab.Create(PlayerManager.GetNext(), this, this.GetHomingRoot(), this.carrot.homingSpeed, this.carrot.homingRotation, (float)this.carrot.homingHP);
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x001CC318 File Offset: 0x001CA718
	private Vector2 GetHomingRoot()
	{
		Vector3 position = this.homingRoots[UnityEngine.Random.Range(0, this.homingRoots.Length)].position;
		this.homingRoot.SetScale(new float?(this.homingRoot.localScale.x * -1f), null, null);
		return position;
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x001CC384 File Offset: 0x001CA784
	private IEnumerator rings_cr()
	{
		for (;;)
		{
			base.StartCoroutine(this.carrot_cr());
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.carrot.idleRange.RandomFloat());
			int count = 0;
			base.animator.SetTrigger("AttackStart");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Start", false, true);
			while (count < this.carrot.bulletCount)
			{
				yield return CupheadTime.WaitForSeconds(this, this.carrot.bulletDelay * 0.5f);
				this.ringEffectPrefab.Create(this.straightRoot.position);
				yield return CupheadTime.WaitForSeconds(this, this.carrot.bulletDelay * 0.5f);
				this.straightRoot.LookAt2D(PlayerManager.GetNext().center);
				for (int i = 0; i < 5; i++)
				{
					AudioManager.Play("level_veggies_carrot_beam");
					this.ringPrefab.Create(this.straightRoot.position, this.straightRoot.eulerAngles.z, this.carrot.bulletSpeed);
					yield return CupheadTime.WaitForSeconds(this, 0.1f);
				}
				count++;
			}
			base.animator.SetTrigger("AttackEnd");
		}
		yield break;
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x001CC3A0 File Offset: 0x001CA7A0
	private IEnumerator carrot_cr()
	{
		int bgCount = 0;
		LevelProperties.Veggies.Carrot p = base.properties.CurrentState.carrot;
		AudioManager.Play("level_veggies_mindmeld_start");
		this.mindLoop.Play();
		yield return CupheadTime.WaitForSeconds(this, this.carrot.startIdleTime);
		bool side = false;
		bgCount = 0;
		int numOfCarrots = p.homingNumOfCarrots.RandomInt();
		while (bgCount < numOfCarrots)
		{
			this.bgPrefab.Create((!side) ? -1 : 1, this.carrot.homingBgSpeed, this);
			side = !side;
			bgCount++;
			yield return CupheadTime.WaitForSeconds(this, this.carrot.homingDelay);
		}
		yield break;
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x001CC3BC File Offset: 0x001CA7BC
	private IEnumerator die_cr()
	{
		this.mindLoop.Stop();
		AudioManager.Play("level_veggies_carrot_die");
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Dead");
		yield return null;
		base.properties.WinInstantly();
		yield break;
	}

	// Token: 0x04003997 RID: 14743
	[SerializeField]
	private AudioSource mindLoopPrefab;

	// Token: 0x04003998 RID: 14744
	private AudioSource mindLoop;

	// Token: 0x04003999 RID: 14745
	[SerializeField]
	private Transform homingRoot;

	// Token: 0x0400399A RID: 14746
	[SerializeField]
	private Transform straightRoot;

	// Token: 0x0400399B RID: 14747
	[SerializeField]
	private VeggiesLevelCarrotHomingProjectile homingPrefab;

	// Token: 0x0400399C RID: 14748
	[SerializeField]
	private VeggiesLevelCarrotRegularProjectile straightPrefab;

	// Token: 0x0400399D RID: 14749
	[SerializeField]
	private BasicProjectile ringPrefab;

	// Token: 0x0400399E RID: 14750
	[SerializeField]
	private Effect ringEffectPrefab;

	// Token: 0x0400399F RID: 14751
	[SerializeField]
	private VeggiesLevelCarrotBgCarrot bgPrefab;

	// Token: 0x040039A0 RID: 14752
	[SerializeField]
	private Effect spark;

	// Token: 0x040039A1 RID: 14753
	private LevelProperties.Veggies.Carrot carrot;

	// Token: 0x040039A2 RID: 14754
	private Transform[] homingRoots;

	// Token: 0x040039A3 RID: 14755
	private bool dead;

	// Token: 0x040039A4 RID: 14756
	private float hp;

	// Token: 0x040039A5 RID: 14757
	private IEnumerator floatingCoroutine;

	// Token: 0x02000843 RID: 2115
	public enum Direction
	{
		// Token: 0x040039A9 RID: 14761
		Down,
		// Token: 0x040039AA RID: 14762
		DownLeft,
		// Token: 0x040039AB RID: 14763
		DownRight
	}

	// Token: 0x02000844 RID: 2116
	public enum State
	{
		// Token: 0x040039AD RID: 14765
		Start,
		// Token: 0x040039AE RID: 14766
		Complete
	}

	// Token: 0x02000845 RID: 2117
	// (Invoke) Token: 0x06003102 RID: 12546
	public delegate void OnAttackHandler(VeggiesLevelCarrot.Direction direction);

	// Token: 0x02000846 RID: 2118
	// (Invoke) Token: 0x06003106 RID: 12550
	public delegate void OnDamageTakenHandler(float damage);
}
