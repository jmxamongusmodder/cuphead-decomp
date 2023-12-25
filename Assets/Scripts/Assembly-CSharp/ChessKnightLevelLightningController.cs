using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000543 RID: 1347
public class ChessKnightLevelLightningController : AbstractMonoBehaviour
{
	// Token: 0x060018BD RID: 6333 RVA: 0x000E0537 File Offset: 0x000DE937
	private void Start()
	{
		base.StartCoroutine(this.lightning_cr());
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x000E0548 File Offset: 0x000DE948
	private IEnumerator lightning_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.lightningDelayRange.RandomFloat());
			base.animator.Play((UnityEngine.Random.Range(0f, 3f) >= 1f) ? "Short" : "Long");
			base.animator.Update(0f);
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Long"))
			{
				this.SFX_KOG_Thunder();
			}
			yield return base.animator.WaitForAnimationToStart(this, "None", false);
		}
		yield break;
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x000E0564 File Offset: 0x000DE964
	private void LateUpdate()
	{
		int num = (int)(this.rend.sprite.name[this.rend.sprite.name.Length - 1] - '1');
		if (num == 51)
		{
			num = 3;
		}
		this.glowTexture.enabled = (num < 3);
		if (this.glowTexture.enabled)
		{
			this.glowTexture.material.SetColor("_OutlineColor", new Color(1f, 1f, 1f, this.glowIntensity[num]));
			this.glowTexture.material.SetFloat("_DimFactor", this.glowIntensity[num] * 0.6f);
		}
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x000E061E File Offset: 0x000DEA1E
	private void SFX_KOG_Thunder()
	{
		AudioManager.Play("sfx_dlc_kog_knight_castlethunder");
	}

	// Token: 0x040021CB RID: 8651
	[SerializeField]
	private MinMax lightningDelayRange = new MinMax(3f, 8f);

	// Token: 0x040021CC RID: 8652
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x040021CD RID: 8653
	[SerializeField]
	private Renderer glowTexture;

	// Token: 0x040021CE RID: 8654
	[SerializeField]
	private float[] glowIntensity = new float[]
	{
		0.8f,
		0.4f,
		0.1f
	};
}
