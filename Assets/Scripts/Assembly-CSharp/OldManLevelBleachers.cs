using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006FE RID: 1790
public class OldManLevelBleachers : AbstractPausableComponent
{
	// Token: 0x06002653 RID: 9811 RVA: 0x00165FEA File Offset: 0x001643EA
	private void Start()
	{
		base.StartCoroutine(this.move_bleachers_cr());
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x00165FFC File Offset: 0x001643FC
	private IEnumerator move_bleachers_cr()
	{
		Vector3 rightStartPos = this.gnomeBleacherRight.transform.localPosition;
		Vector3 leftStartPos = this.gnomeBleacherLeft.transform.localPosition;
		Vector3 rightStepStartPos = rightStartPos;
		Vector3 leftStepStartPos = leftStartPos;
		this.SFX_OMM_P2_PuppetBleachersRaiseUp();
		this.SFX_OMM_BleachersCrowdLoop();
		yield return null;
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0.15f, 0.5f);
		for (int i = 0; i < 3; i++)
		{
			float t = 0f;
			float time = this.enterStepTime;
			Vector3 rightEndPos = Vector3.Lerp(rightStartPos, this.gnomeBleacherRightEnd.position, 0.5f + (float)i * 0.25f);
			Vector3 leftEndPos = Vector3.Lerp(leftStartPos, this.gnomeBleacherLeftEnd.position, 0.5f + (float)i * 0.25f);
			while (t < time + this.offset)
			{
				t += CupheadTime.Delta;
				this.gnomeBleacherRight.transform.localPosition = Vector3.Lerp(rightStepStartPos, rightEndPos, EaseUtils.EaseOutBounce(0f, 1f, Mathf.Clamp((t - this.offset) / time, 0f, 1f)));
				this.gnomeBleacherLeft.transform.localPosition = Vector3.Lerp(leftStepStartPos, leftEndPos, EaseUtils.EaseOutBounce(0f, 1f, Mathf.Clamp(t / time, 0f, 1f)));
				yield return null;
			}
			rightStepStartPos = this.gnomeBleacherRight.transform.localPosition;
			leftStepStartPos = this.gnomeBleacherLeft.transform.localPosition;
			yield return CupheadTime.WaitForSeconds(this, this.enterStepPause);
		}
		while (this.level.InPhase2())
		{
			yield return null;
		}
		this.SFX_OMM_P2_End_BleacherPuppetsLower();
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0f, 1.5f);
		for (int j = 0; j < 3; j++)
		{
			float t = 0f;
			float time = this.exitStepTime;
			Vector3 rightEndPos2 = Vector3.Lerp(this.gnomeBleacherRightEnd.position, rightStartPos, (float)(j + 1) * 0.333f);
			Vector3 leftEndPos2 = Vector3.Lerp(this.gnomeBleacherLeftEnd.position, leftStartPos, (float)(j + 1) * 0.333f);
			while (t < time + this.offset)
			{
				t += CupheadTime.Delta;
				this.gnomeBleacherRight.transform.localPosition = Vector3.Lerp(rightStepStartPos, rightEndPos2, EaseUtils.EaseOutElastic(0f, 1f, Mathf.Clamp((t - this.offset) / time, 0f, 1f)));
				this.gnomeBleacherLeft.transform.localPosition = Vector3.Lerp(leftStepStartPos, leftEndPos2, EaseUtils.EaseOutElastic(0f, 1f, Mathf.Clamp(t / time, 0f, 1f)));
				yield return null;
			}
			rightStepStartPos = this.gnomeBleacherRight.transform.localPosition;
			leftStepStartPos = this.gnomeBleacherLeft.transform.localPosition;
			yield return CupheadTime.WaitForSeconds(this, this.exitStepPause);
		}
		base.gameObject.SetActive(false);
		yield return null;
		yield break;
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x00166017 File Offset: 0x00164417
	private void SFX_OMM_P2_PuppetBleachersRaiseUp()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_bleachersraiseup");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_bleachersraiseup");
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x00166033 File Offset: 0x00164433
	private void SFX_OMM_BleachersCrowdLoop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p2_bleacherscrowd_loop", 0.001f, 0.001f);
		AudioManager.PlayLoop("sfx_dlc_omm_p2_bleacherscrowd_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_bleacherscrowd_loop");
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x00166063 File Offset: 0x00164463
	private void SFX_OMM_P2_End_BleacherPuppetsLower()
	{
		AudioManager.Stop("sfx_dlc_omm_p2_bleacherscrowd_loop");
		AudioManager.Play("sfx_dlc_omm_p2_end_bleacherpuppetslower");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_bleacherpuppetslower");
	}

	// Token: 0x04002EDC RID: 11996
	[SerializeField]
	private GameObject gnomeBleacherRight;

	// Token: 0x04002EDD RID: 11997
	[SerializeField]
	private Transform gnomeBleacherRightEnd;

	// Token: 0x04002EDE RID: 11998
	[SerializeField]
	private GameObject gnomeBleacherLeft;

	// Token: 0x04002EDF RID: 11999
	[SerializeField]
	private Transform gnomeBleacherLeftEnd;

	// Token: 0x04002EE0 RID: 12000
	[SerializeField]
	private OldManLevel level;

	// Token: 0x04002EE1 RID: 12001
	[SerializeField]
	private float enterStepTime = 0.6f;

	// Token: 0x04002EE2 RID: 12002
	[SerializeField]
	private float enterStepPause = 0.1f;

	// Token: 0x04002EE3 RID: 12003
	[SerializeField]
	private float exitStepTime = 0.3f;

	// Token: 0x04002EE4 RID: 12004
	[SerializeField]
	private float exitStepPause = 0.05f;

	// Token: 0x04002EE5 RID: 12005
	[SerializeField]
	private float offset = 0.1f;
}
