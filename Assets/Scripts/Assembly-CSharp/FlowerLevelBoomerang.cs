using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000604 RID: 1540
public class FlowerLevelBoomerang : BasicProjectile
{
	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06001E99 RID: 7833 RVA: 0x00119CAE File Offset: 0x001180AE
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x00119CB4 File Offset: 0x001180B4
	public void OnBoomerangStart(float delay)
	{
		this.BoomerangNumberSFX++;
		if (this.BoomerangNumberSFX == 1)
		{
			AudioManager.FadeSFXVolume("flower_boomerang_1", 1f, 1f);
			AudioManager.PlayLoop("flower_boomerang_1");
			this.emitAudioFromObject.Add("flower_boomerang_1");
		}
		else if (this.BoomerangNumberSFX != 1)
		{
			AudioManager.FadeSFXVolume("flower_boomerang_2", 1f, 1f);
			AudioManager.PlayLoop("flower_boomerang_2");
			this.emitAudioFromObject.Add("flower_boomerang_2");
		}
		this.offScreenDelay = delay;
		this.returnXPosition = (float)(Level.Current.Left - 100);
		this.endXPosition = (float)(Level.Current.Right + 500);
		base.StartCoroutine(this.boomerangStart_cr());
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x00119D88 File Offset: 0x00118188
	private IEnumerator boomerangStart_cr()
	{
		base.transform.GetChild(0).transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		while (base.transform.position.x > this.returnXPosition)
		{
			yield return null;
		}
		this.move = false;
		yield return CupheadTime.WaitForSeconds(this, this.offScreenDelay);
		this.OnBoomerangReturn();
		yield break;
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x00119DA4 File Offset: 0x001181A4
	private void OnBoomerangReturn()
	{
		this.Speed = -this.Speed;
		this.move = true;
		base.transform.position = new Vector3(base.transform.position.x, (float)(Level.Current.Ground + Level.Current.Height / 6), 0f);
		base.StartCoroutine(this.boomerangReturn_cr());
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x00119E14 File Offset: 0x00118214
	private IEnumerator boomerangReturn_cr()
	{
		base.transform.GetChild(0).transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		while (base.transform.position.x < this.endXPosition)
		{
			yield return null;
		}
		if (this.BoomerangNumberSFX == 1)
		{
			AudioManager.FadeSFXVolume("flower_boomerang_1", 0f, 3f);
			AudioManager.FadeSFXVolume("flower_boomerang_2", 0f, 3f);
		}
		else if (this.BoomerangNumberSFX != 1)
		{
			AudioManager.FadeSFXVolume("flower_boomerang_1", 0f, 3f);
			AudioManager.FadeSFXVolume("flower_boomerang_2", 0f, 3f);
		}
		this.BoomerangNumberSFX--;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x00119E2F File Offset: 0x0011822F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x00119E58 File Offset: 0x00118258
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x00119E60 File Offset: 0x00118260
	public override void OnLevelEnd()
	{
		AudioManager.Stop("flower_boomerang_1");
		AudioManager.Stop("flower_boomerang_2");
		base.OnLevelEnd();
	}

	// Token: 0x04002770 RID: 10096
	private float returnXPosition;

	// Token: 0x04002771 RID: 10097
	private float endXPosition;

	// Token: 0x04002772 RID: 10098
	private float offScreenDelay;

	// Token: 0x04002773 RID: 10099
	private int BoomerangNumberSFX;
}
