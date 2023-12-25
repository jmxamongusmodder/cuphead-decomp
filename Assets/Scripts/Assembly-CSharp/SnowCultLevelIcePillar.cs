using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007EB RID: 2027
public class SnowCultLevelIcePillar : AbstractProjectile
{
	// Token: 0x06002E6A RID: 11882 RVA: 0x001B609C File Offset: 0x001B449C
	public virtual SnowCultLevelIcePillar Init(Vector3 pos, LevelProperties.SnowCult.IcePillar properties, bool typeToPlay, float timeToDelay)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.typeString = ((!typeToPlay) ? "B" : "A");
		base.animator.Play("IceBlade_Start" + this.typeString);
		this.timeToDelay = timeToDelay;
		this.outTime = properties.outTime;
		this.Attack();
		return this;
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x001B6112 File Offset: 0x001B4512
	private void Attack()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06002E6C RID: 11884 RVA: 0x001B6124 File Offset: 0x001B4524
	private IEnumerator attack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.timeToDelay);
		base.animator.SetTrigger("popUp");
		this.SFX_SNOWCULT_BladeStabfromGround();
		yield break;
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x001B613F File Offset: 0x001B453F
	private void WaitAndRetract()
	{
		base.StartCoroutine(this.waitandretract_cr());
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x001B6150 File Offset: 0x001B4550
	private IEnumerator waitandretract_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.outTime);
		base.animator.SetTrigger("popDown");
		yield break;
	}

	// Token: 0x06002E6F RID: 11887 RVA: 0x001B616B File Offset: 0x001B456B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002E70 RID: 11888 RVA: 0x001B6189 File Offset: 0x001B4589
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06002E71 RID: 11889 RVA: 0x001B6191 File Offset: 0x001B4591
	private void WarningSmokeFX()
	{
		this.warningSmoke.Create(base.transform.position);
	}

	// Token: 0x06002E72 RID: 11890 RVA: 0x001B61AA File Offset: 0x001B45AA
	private void SFX_SNOWCULT_BladeStabfromGround()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_blade_stabfromground");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_blade_stabfromground");
	}

	// Token: 0x04003703 RID: 14083
	private const float Y_POS_START = -430f;

	// Token: 0x04003704 RID: 14084
	private const float Y_POS_END = -200f;

	// Token: 0x04003705 RID: 14085
	private string typeString;

	// Token: 0x04003706 RID: 14086
	private float timeToDelay;

	// Token: 0x04003707 RID: 14087
	private float outTime;

	// Token: 0x04003708 RID: 14088
	[SerializeField]
	private Effect warningSmoke;
}
