using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class AirplaneLevelBoomerang : AbstractProjectile
{
	// Token: 0x0600139C RID: 5020 RVA: 0x000ADA08 File Offset: 0x000ABE08
	public AirplaneLevelBoomerang Create(Vector2 pos, float speedF, float easeDF, float speedR, float easeDR, float delay, bool onLeft, int id)
	{
		AirplaneLevelBoomerang airplaneLevelBoomerang = base.Create() as AirplaneLevelBoomerang;
		airplaneLevelBoomerang.transform.position = pos;
		airplaneLevelBoomerang.DamagesType.OnlyPlayer();
		airplaneLevelBoomerang.delay = delay;
		airplaneLevelBoomerang.onLeft = onLeft;
		airplaneLevelBoomerang.speedForward = speedF;
		airplaneLevelBoomerang.easeDistanceForward = easeDF;
		airplaneLevelBoomerang.speedReturn = speedR;
		airplaneLevelBoomerang.easeDistanceReturn = easeDR;
		airplaneLevelBoomerang.id = id;
		return airplaneLevelBoomerang;
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x000ADA75 File Offset: 0x000ABE75
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x000ADA92 File Offset: 0x000ABE92
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x000ADAB0 File Offset: 0x000ABEB0
	protected override void Start()
	{
		base.Start();
		if (!base.CanParry)
		{
			base.animator.Play((!Rand.Bool()) ? "B" : "A");
		}
		base.StartCoroutine(this.move_cr());
		this.SFX_DOGFIGHT_BoneShot_Loop();
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x000ADB08 File Offset: 0x000ABF08
	private IEnumerator move_cr()
	{
		this.rend.enabled = true;
		float end = (!this.onLeft) ? (-725f + this.easeDistanceForward) : (725f - this.easeDistanceForward);
		bool flipSprite = !this.onLeft;
		YieldInstruction wait = new WaitForFixedUpdate();
		base.GetComponent<SpriteRenderer>().flipX = flipSprite;
		while ((this.onLeft && Mathf.Sign(base.transform.position.x - end) == -1f) || (!this.onLeft && Mathf.Sign(base.transform.position.x - end) == 1f))
		{
			base.transform.position += Vector3.right * this.speedForward * CupheadTime.FixedDelta * (float)((!this.onLeft) ? -1 : 1);
			yield return wait;
		}
		float t = 0f;
		float tMax = this.easeDistanceForward / this.speedForward * 2f;
		float start = end;
		end = ((!this.onLeft) ? -725f : 725f);
		while (t < tMax)
		{
			t += CupheadTime.FixedDelta;
			yield return wait;
			base.transform.position = new Vector3(Mathf.Lerp(start, end, EaseUtils.EaseOutSine(0f, 1f, Mathf.InverseLerp(0f, tMax, t))), base.transform.position.y);
		}
		base.transform.position = new Vector3(end, base.transform.position.y);
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		base.GetComponent<SpriteRenderer>().flipX = !flipSprite;
		t = 0f;
		tMax = this.easeDistanceReturn / this.speedReturn * 2f;
		start = base.transform.position.x;
		end = ((!this.onLeft) ? (-725f + this.easeDistanceReturn) : (725f - this.easeDistanceReturn));
		while (t < tMax)
		{
			t += CupheadTime.FixedDelta;
			yield return wait;
			base.transform.position = new Vector3(Mathf.Lerp(start, end, EaseUtils.EaseInSine(0f, 1f, Mathf.InverseLerp(0f, tMax, t))), base.transform.position.y);
		}
		base.transform.position = new Vector3(end, base.transform.position.y);
		end = ((!this.onLeft) ? 1025f : -1025f);
		while ((this.onLeft && Mathf.Sign(base.transform.position.x - end) == 1f) || (!this.onLeft && Mathf.Sign(base.transform.position.x - end) == -1f))
		{
			base.transform.position += Vector3.right * this.speedReturn * CupheadTime.FixedDelta * (float)((!this.onLeft) ? 1 : -1);
			yield return wait;
		}
		this.SFX_DOGFIGHT_BoneShot_StopLoop();
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x000ADB23 File Offset: 0x000ABF23
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this.SFX_DOGFIGHT_BoneShot_StopLoop();
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x000ADB34 File Offset: 0x000ABF34
	private void SFX_DOGFIGHT_BoneShot_Loop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (this.id + 1), 0.12f, 0.1f);
		AudioManager.PlayLoop("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (this.id + 1));
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (this.id + 1));
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x000ADBA5 File Offset: 0x000ABFA5
	private void SFX_DOGFIGHT_BoneShot_StopLoop()
	{
		AudioManager.Stop("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (this.id + 1));
	}

	// Token: 0x04001CBE RID: 7358
	private const float xMax = 725f;

	// Token: 0x04001CBF RID: 7359
	private float delay;

	// Token: 0x04001CC0 RID: 7360
	private bool onLeft;

	// Token: 0x04001CC1 RID: 7361
	private float speedForward;

	// Token: 0x04001CC2 RID: 7362
	private float easeDistanceForward;

	// Token: 0x04001CC3 RID: 7363
	private float speedReturn;

	// Token: 0x04001CC4 RID: 7364
	private float easeDistanceReturn;

	// Token: 0x04001CC5 RID: 7365
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04001CC6 RID: 7366
	private int id;
}
