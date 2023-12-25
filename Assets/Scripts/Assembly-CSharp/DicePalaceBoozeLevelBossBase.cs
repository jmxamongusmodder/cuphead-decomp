using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200059E RID: 1438
public class DicePalaceBoozeLevelBossBase : LevelProperties.DicePalaceBooze.Entity
{
	// Token: 0x1700035B RID: 859
	// (get) Token: 0x06001B8F RID: 7055 RVA: 0x000FB6A7 File Offset: 0x000F9AA7
	// (set) Token: 0x06001B90 RID: 7056 RVA: 0x000FB6AF File Offset: 0x000F9AAF
	public bool isDead { get; private set; }

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x06001B91 RID: 7057 RVA: 0x000FB6B8 File Offset: 0x000F9AB8
	// (set) Token: 0x06001B92 RID: 7058 RVA: 0x000FB6BF File Offset: 0x000F9ABF
	public static int DEATH_COUNTER { get; private set; }

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06001B93 RID: 7059 RVA: 0x000FB6C7 File Offset: 0x000F9AC7
	// (set) Token: 0x06001B94 RID: 7060 RVA: 0x000FB6CE File Offset: 0x000F9ACE
	public static float ATTACK_DELAY { get; private set; }

	// Token: 0x06001B95 RID: 7061 RVA: 0x000FB6D6 File Offset: 0x000F9AD6
	private void Start()
	{
		this.isDead = false;
		DicePalaceBoozeLevelBossBase.DEATH_COUNTER = 0;
		DicePalaceBoozeLevelBossBase.ATTACK_DELAY = 0f;
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x000FB6EF File Offset: 0x000F9AEF
	public override void LevelInit(LevelProperties.DicePalaceBooze properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x000FB6F8 File Offset: 0x000F9AF8
	protected virtual void StartDying()
	{
		this.isDead = true;
		DicePalaceBoozeLevelBossBase.DEATH_COUNTER++;
		if (DicePalaceBoozeLevelBossBase.DEATH_COUNTER >= 3)
		{
			this.AllDead();
		}
		else
		{
			DicePalaceBoozeLevelBossBase.ATTACK_DELAY += base.properties.CurrentState.main.delaySubstractAmount;
			this.Dying();
		}
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x000FB754 File Offset: 0x000F9B54
	private void Dying()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.dying_cr());
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x000FB76C File Offset: 0x000F9B6C
	private IEnumerator dying_cr()
	{
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<DamageReceiver>().enabled = false;
		UnityEngine.Object.Destroy(base.GetComponent<Rigidbody2D>());
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		yield return null;
		yield break;
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x000FB787 File Offset: 0x000F9B87
	private void AllDead()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		base.properties.DealDamageToNextNamedState();
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x000FB7AA File Offset: 0x000F9BAA
	protected virtual void HandleDead()
	{
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x040024AD RID: 9389
	protected float health;
}
