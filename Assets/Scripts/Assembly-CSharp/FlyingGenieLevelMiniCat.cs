using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000670 RID: 1648
public class FlyingGenieLevelMiniCat : HomingProjectile
{
	// Token: 0x17000396 RID: 918
	// (get) Token: 0x060022A8 RID: 8872 RVA: 0x00145A51 File Offset: 0x00143E51
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x00145A54 File Offset: 0x00143E54
	public FlyingGenieLevelMiniCat Create(Vector3 pos, float rotation, AbstractPlayerController player, LevelProperties.FlyingGenie.Sphinx properties)
	{
		FlyingGenieLevelMiniCat flyingGenieLevelMiniCat = base.Create(pos, rotation, properties.homingSpeed, properties.homingSpeed, properties.homingRotation, 20f, 0f, player) as FlyingGenieLevelMiniCat;
		flyingGenieLevelMiniCat.properties = properties;
		flyingGenieLevelMiniCat.transform.position = pos;
		return flyingGenieLevelMiniCat;
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x00145AA9 File Offset: 0x00143EA9
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x00145ABE File Offset: 0x00143EBE
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.properties.dieOnCollisionPlayer)
		{
			this.Die();
		}
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x00145ADE File Offset: 0x00143EDE
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x00145AFC File Offset: 0x00143EFC
	private IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.miniHomingDurationRange.RandomFloat());
		base.HomingEnabled = false;
		for (;;)
		{
			base.transform.position += base.transform.right * this.properties.homingSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x00145B17 File Offset: 0x00143F17
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetFloat("Pink", (float)((!parryable) ? 0 : 1));
	}

	// Token: 0x04002B4C RID: 11084
	private const string PinkParameterName = "Pink";

	// Token: 0x04002B4D RID: 11085
	private LevelProperties.FlyingGenie.Sphinx properties;
}
