using System;
using UnityEngine;

// Token: 0x02000788 RID: 1928
public class RumRunnersLevelBugGirlIntro : MonoBehaviour
{
	// Token: 0x06002A8B RID: 10891 RVA: 0x0018DD47 File Offset: 0x0018C147
	private void OnEnable()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002A8C RID: 10892 RVA: 0x0018DD60 File Offset: 0x0018C160
	private void OnDisable()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken -= this.OnDamageTaken;
	}

	// Token: 0x06002A8D RID: 10893 RVA: 0x0018DD79 File Offset: 0x0018C179
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.introAnimation.bugGirlDamage += info.damage;
	}

	// Token: 0x06002A8E RID: 10894 RVA: 0x0018DD93 File Offset: 0x0018C193
	private void animationEvent_BugWalkBegin()
	{
		this.introAnimation.StartBugWalk();
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x0018DDA0 File Offset: 0x0018C1A0
	private void animationEvent_BugTauntBegin()
	{
		this.introAnimation.StopBugWalk();
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x0018DDAD File Offset: 0x0018C1AD
	private void animationEvent_TauntBump()
	{
		this.introAnimation.BarrelExit();
	}

	// Token: 0x04003357 RID: 13143
	[SerializeField]
	private RumRunnersLevelMobIntroAnimation introAnimation;
}
