using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000853 RID: 2131
public class VeggiesLevelPeas : LevelProperties.Veggies.Entity
{
	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06003168 RID: 12648 RVA: 0x001CE50A File Offset: 0x001CC90A
	// (set) Token: 0x06003169 RID: 12649 RVA: 0x001CE512 File Offset: 0x001CC912
	public VeggiesLevelPeas.State state { get; private set; }

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x0600316A RID: 12650 RVA: 0x001CE51C File Offset: 0x001CC91C
	// (remove) Token: 0x0600316B RID: 12651 RVA: 0x001CE554 File Offset: 0x001CC954
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VeggiesLevelPeas.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x0600316C RID: 12652 RVA: 0x001CE58A File Offset: 0x001CC98A
	private void Start()
	{
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x001CE598 File Offset: 0x001CC998
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.properties = (propertyGroup as LevelProperties.Veggies.Peas);
		this.hp = (float)this.properties.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x001CE5D8 File Offset: 0x001CC9D8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
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

	// Token: 0x0600316F RID: 12655 RVA: 0x001CE62A File Offset: 0x001CCA2A
	private void OnInAnimComplete()
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.peas_cr());
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x001CE645 File Offset: 0x001CCA45
	private void OnDeathAnimComplete()
	{
		this.state = VeggiesLevelPeas.State.Complete;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x001CE659 File Offset: 0x001CCA59
	private void Die()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x001CE670 File Offset: 0x001CCA70
	private IEnumerator peas_cr()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x001CE684 File Offset: 0x001CCA84
	private IEnumerator die_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Idle");
		yield return base.StartCoroutine(base.dieFlash_cr());
		base.animator.SetTrigger("Dead");
		yield break;
	}

	// Token: 0x040039F0 RID: 14832
	[SerializeField]
	private VeggiesLevelOnionTearProjectile projectilePrefab;

	// Token: 0x040039F1 RID: 14833
	private new LevelProperties.Veggies.Peas properties;

	// Token: 0x040039F2 RID: 14834
	private float hp;

	// Token: 0x02000854 RID: 2132
	public enum State
	{
		// Token: 0x040039F5 RID: 14837
		Start,
		// Token: 0x040039F6 RID: 14838
		Complete
	}

	// Token: 0x02000855 RID: 2133
	// (Invoke) Token: 0x06003175 RID: 12661
	public delegate void OnDamageTakenHandler(float damage);
}
