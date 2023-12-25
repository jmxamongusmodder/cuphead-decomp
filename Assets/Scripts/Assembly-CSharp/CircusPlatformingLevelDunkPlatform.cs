using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A3 RID: 2211
public class CircusPlatformingLevelDunkPlatform : AbstractCollidableObject
{
	// Token: 0x0600336D RID: 13165 RVA: 0x001DEC98 File Offset: 0x001DD098
	private void Start()
	{
		this.collider2d = base.GetComponent<Collider2D>();
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x001DECA8 File Offset: 0x001DD0A8
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<CircusPlatformingLevelCannonProjectile>())
		{
			this.collider2d.enabled = false;
			base.animator.SetTrigger("Hit");
			base.StartCoroutine(this.waitSpin_cr());
		}
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x001DECF6 File Offset: 0x001DD0F6
	public void Drop()
	{
		base.StartCoroutine(this.deactivate_cr());
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x001DED08 File Offset: 0x001DD108
	private IEnumerator deactivate_cr()
	{
		base.animator.SetTrigger("Drop");
		this.platform.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.platformDown);
		base.animator.SetTrigger("Raise");
		yield break;
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x001DED23 File Offset: 0x001DD123
	public void ActivatePlatform()
	{
		this.collider2d.enabled = true;
		this.platform.enabled = true;
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x001DED40 File Offset: 0x001DD140
	private IEnumerator waitSpin_cr()
	{
		AudioManager.Play("circus_platform_plank_target");
		this.emitAudioFromObject.Add("circus_platform_plank_target");
		yield return CupheadTime.WaitForSeconds(this, this.targetSpin);
		base.animator.SetTrigger("SpinStop");
		yield break;
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x001DED5B File Offset: 0x001DD15B
	private void DropSFX()
	{
		AudioManager.Play("circus_platform_plank_drop");
		this.emitAudioFromObject.Add("circus_platform_plank_drop");
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x001DED77 File Offset: 0x001DD177
	private void RaiseSFX()
	{
		AudioManager.Play("circus_platform_plank_raise");
		this.emitAudioFromObject.Add("circus_platform_plank_raise");
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x001DED93 File Offset: 0x001DD193
	private void PlankSFX()
	{
		AudioManager.Play("circus_platform_plank_target");
		this.emitAudioFromObject.Add("circus_platform_plank_target");
	}

	// Token: 0x04003BBC RID: 15292
	private const string HitParameterName = "Hit";

	// Token: 0x04003BBD RID: 15293
	private const string DropParameterName = "Drop";

	// Token: 0x04003BBE RID: 15294
	private const string RaiseParameterName = "Raise";

	// Token: 0x04003BBF RID: 15295
	private const string StopSpinParameterName = "SpinStop";

	// Token: 0x04003BC0 RID: 15296
	[SerializeField]
	private Collider2D platform;

	// Token: 0x04003BC1 RID: 15297
	[SerializeField]
	private float platformDown;

	// Token: 0x04003BC2 RID: 15298
	[SerializeField]
	private float targetSpin;

	// Token: 0x04003BC3 RID: 15299
	private Collider2D collider2d;
}
