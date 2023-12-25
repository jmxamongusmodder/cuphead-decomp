using System;
using UnityEngine;

// Token: 0x020008AA RID: 2218
public class CircusPlatformingLevelPretzel : AbstractPlatformingLevelEnemy
{
	// Token: 0x060033B3 RID: 13235 RVA: 0x001E07AA File Offset: 0x001DEBAA
	protected override void OnStart()
	{
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x001E07AC File Offset: 0x001DEBAC
	public void SetPath(Transform[] path)
	{
		this.path = path;
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x001E07B5 File Offset: 0x001DEBB5
	public void SetStartPosition(int index)
	{
		this.nextPoint = index;
		base.transform.position = this.path[this.nextPoint].position;
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x001E07DC File Offset: 0x001DEBDC
	public void Jump()
	{
		if (this.goingLeft)
		{
			this.nextPoint--;
		}
		else
		{
			this.nextPoint++;
		}
		if (this.nextPoint < 0 || (this.path != null && this.nextPoint >= this.path.Length))
		{
			this.Die();
		}
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x001E0848 File Offset: 0x001DEC48
	public void Land()
	{
		base.animator.SetTrigger("Salt");
		if (this.nextPoint < this.path.Length)
		{
			base.transform.position = this.path[this.nextPoint].position;
		}
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x001E0895 File Offset: 0x001DEC95
	public void JumpSFX()
	{
		AudioManager.Play("circus_pretzel_jump");
		this.emitAudioFromObject.Add("circus_pretzel_jump");
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x001E08B4 File Offset: 0x001DECB4
	protected override void Die()
	{
		AudioManager.Stop("circus_pretzel_jump");
		AudioManager.Play("circus_generic_death_big");
		this.emitAudioFromObject.Add("circus_generic_death_big");
		base.Die();
		UnityEngine.Object.Destroy(base.transform.parent.gameObject);
	}

	// Token: 0x04003C00 RID: 15360
	private const string SaltParameterName = "Salt";

	// Token: 0x04003C01 RID: 15361
	public bool goingLeft;

	// Token: 0x04003C02 RID: 15362
	[SerializeField]
	private float jumpMultiplierX;

	// Token: 0x04003C03 RID: 15363
	[SerializeField]
	private float jumpMultiplierY;

	// Token: 0x04003C04 RID: 15364
	[SerializeField]
	private float inverseJumpMultiplierY;

	// Token: 0x04003C05 RID: 15365
	[SerializeField]
	private Transform transformDustA;

	// Token: 0x04003C06 RID: 15366
	[SerializeField]
	private Transform transformDustB;

	// Token: 0x04003C07 RID: 15367
	private Transform[] path;

	// Token: 0x04003C08 RID: 15368
	private int nextPoint;
}
