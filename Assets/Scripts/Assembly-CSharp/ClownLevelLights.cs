using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056A RID: 1386
public class ClownLevelLights : AbstractPausableComponent
{
	// Token: 0x06001A32 RID: 6706 RVA: 0x000EFAEA File Offset: 0x000EDEEA
	private void Start()
	{
		this.redLight.enabled = false;
		this.greenLight.enabled = false;
		base.StartCoroutine(this.warning_lights_cr());
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x000EFB11 File Offset: 0x000EDF11
	public void StartWarningLights()
	{
		AudioManager.PlayLoop("clown_warning_lights_loop");
		this.emitAudioFromObject.Add("clown_warning_lights_loop");
		this.redLight.enabled = true;
		this.greenLight.enabled = true;
		this.isOn = true;
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x000EFB4C File Offset: 0x000EDF4C
	public void StopWarningLights()
	{
		AudioManager.Stop("clown_warning_lights_loop");
		this.redLight.enabled = false;
		this.greenLight.enabled = false;
		this.isOn = false;
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x000EFB78 File Offset: 0x000EDF78
	private IEnumerator warning_lights_cr()
	{
		float t = 0f;
		for (;;)
		{
			this.redLight.color = new Color(1f, 1f, 1f, 1f);
			this.greenLight.color = new Color(1f, 1f, 1f, 0f);
			if (this.isOn)
			{
				t = 0f;
				while (t < 0.083f)
				{
					this.redLight.color = new Color(1f, 1f, 1f, 1f - t / 0.083f);
					this.greenLight.color = new Color(1f, 1f, 1f, t / 0.083f);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.redLight.color = new Color(1f, 1f, 1f, 0f);
				this.greenLight.color = new Color(1f, 1f, 1f, 1f);
				t = 0f;
				yield return CupheadTime.WaitForSeconds(this, 0.083f);
				yield return null;
				while (t < 0.083f)
				{
					this.redLight.color = new Color(1f, 1f, 1f, t / 0.083f);
					this.greenLight.color = new Color(1f, 1f, 1f, 1f - t / 0.083f);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.redLight.color = new Color(1f, 1f, 1f, 1f);
				this.greenLight.color = new Color(1f, 1f, 1f, 0f);
				yield return CupheadTime.WaitForSeconds(this, 0.083f);
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002350 RID: 9040
	[SerializeField]
	private SpriteRenderer redLight;

	// Token: 0x04002351 RID: 9041
	[SerializeField]
	private SpriteRenderer greenLight;

	// Token: 0x04002352 RID: 9042
	private const float fadeTime = 0.083f;

	// Token: 0x04002353 RID: 9043
	private bool isOn;
}
