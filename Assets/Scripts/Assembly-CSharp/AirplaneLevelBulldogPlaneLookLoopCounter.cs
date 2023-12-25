using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B7 RID: 1207
public class AirplaneLevelBulldogPlaneLookLoopCounter : MonoBehaviour
{
	// Token: 0x060013E6 RID: 5094 RVA: 0x000B13E6 File Offset: 0x000AF7E6
	private void OnDestroy()
	{
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x000B13EE File Offset: 0x000AF7EE
	private void aniEvent_IncreaseIdleLookLoopCount()
	{
		this.bullDogPlane.SetInteger("IdleLoopCount", this.bullDogPlane.GetInteger("IdleLoopCount") + 1);
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x000B1412 File Offset: 0x000AF812
	private void AniEvent_RecedeIntoDistance()
	{
		base.StartCoroutine(this.recede_cr());
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x000B1424 File Offset: 0x000AF824
	private IEnumerator recede_cr()
	{
		float startTime = this.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime;
		Vector3 startPos = base.transform.position;
		Vector3 endPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 100f, base.transform.position.z);
		endPos = Vector3.Lerp(startPos, endPos, 0.8f);
		while (this.bullDogPlane.GetCurrentAnimatorStateInfo(0).IsName("Death"))
		{
			float t = Mathf.InverseLerp(startTime, 1f, this.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime);
			base.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0f, 1f, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x000B143F File Offset: 0x000AF83F
	private void AnimationEvent_SFX_DOGFIGHT_Intro_BulldogPlaneFlyby()
	{
		AudioManager.Play("sfx_dlc_dogfight_bulldogplane_introflyby");
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x000B144B File Offset: 0x000AF84B
	private void AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectDown()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectdown");
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x000B1457 File Offset: 0x000AF857
	private void AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectUp()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectUp");
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x000B1463 File Offset: 0x000AF863
	private void AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectLeverPull()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectleverpull");
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x000B146F File Offset: 0x000AF86F
	private void AnimationEvent_SFX_DOGFIGHT_Bulldog_LandsCockpit()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_landscockpit");
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x000B147B File Offset: 0x000AF87B
	private void SFX_DOGFIGHT_Bulldog_WingExtend_WhimperOut()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_whimperout");
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x000B1487 File Offset: 0x000AF887
	private void SFX_DOGFIGHT_Bulldog_WingExtend_WhistleOut()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_Whistle_Out");
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x000B1493 File Offset: 0x000AF893
	private void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_WingStretchOut()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_WingStretch_Out");
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x000B149F File Offset: 0x000AF89F
	private void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_WingStretchIn()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_WingStretch_In");
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x000B14AB File Offset: 0x000AF8AB
	private void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_DiePlaneExplodes()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_planeexplodes");
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x000B14B7 File Offset: 0x000AF8B7
	private void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_DiePlaneExplodes_VO()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_PlaneExplodes_VO");
		CupheadLevelCamera.Current.Shake(30f, 0.29166666f, false);
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x000B14D8 File Offset: 0x000AF8D8
	private void WORKAROUND_NullifyFields()
	{
		this.bullDogPlane = null;
	}

	// Token: 0x04001D15 RID: 7445
	[SerializeField]
	private Animator bullDogPlane;
}
