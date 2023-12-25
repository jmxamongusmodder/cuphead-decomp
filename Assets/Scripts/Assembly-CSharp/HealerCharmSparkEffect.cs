using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A0D RID: 2573
public class HealerCharmSparkEffect : Effect
{
	// Token: 0x06003CD5 RID: 15573 RVA: 0x0021A798 File Offset: 0x00218B98
	public void Create(Vector3 position, Vector3 scale, AbstractPlayerController target)
	{
		this.startedFlash = 0;
		HealerCharmSparkEffect healerCharmSparkEffect = base.Create(position, scale) as HealerCharmSparkEffect;
		healerCharmSparkEffect.target = target;
		this.particleVectors = new List<Vector2>
		{
			new Vector2(-0.26f, -0.93f),
			new Vector2(0.3f, -0.72f),
			new Vector2(0.77f, -0.28f),
			new Vector2(0.98f, 0.23f),
			new Vector2(0.65f, 0.74f),
			new Vector2(0.02f, 0.6f),
			new Vector2(-0.4f, 0.93f),
			new Vector2(-0.91f, 0.63f),
			new Vector2(-1f, 0.07f),
			new Vector2(-0.67f, -0.47f)
		};
		for (int i = 0; i < 5; i++)
		{
			int index = UnityEngine.Random.Range(0, this.particleVectors.Count);
			UnityEngine.Object.Instantiate<HealerCharmParticleEffect>(this.particle, position, Quaternion.identity).SetVars(new Vector2(this.particleVectors[index].x * base.transform.localScale.x, this.particleVectors[index].y), target, healerCharmSparkEffect);
			this.particleVectors.RemoveAt(index);
		}
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x0021A92C File Offset: 0x00218D2C
	public void StartPlayerFlash()
	{
		if (this.startedFlash >= 0)
		{
			this.startedFlash++;
			if (this.startedFlash > 4)
			{
				base.StartCoroutine(this.player_flash_cr());
				this.startedFlash = -1;
			}
		}
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x0021A968 File Offset: 0x00218D68
	private IEnumerator player_flash_cr()
	{
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.041666668f, false);
		if (!this.target.stats.SuperInvincible)
		{
			LevelPlayerController levelPlayer = this.target as LevelPlayerController;
			PlanePlayerController planePlayer = this.target as PlanePlayerController;
			Material matInstance = null;
			SpriteRenderer playerRend = null;
			if (levelPlayer != null)
			{
				levelPlayer.animationController.SetMaterial(this.flashMaterial);
				matInstance = levelPlayer.animationController.GetMaterial();
				playerRend = levelPlayer.animationController.GetSpriteRenderer();
			}
			else if (planePlayer != null)
			{
				planePlayer.animationController.SetMaterial(this.flashMaterial);
				matInstance = planePlayer.animationController.GetMaterial();
				playerRend = planePlayer.animationController.GetSpriteRenderer();
			}
			Color lightColor = new Color(1f, 0.4509804f, 0.7882353f);
			Color darkColor = new Color(1f, 0.21960784f, 0.7019608f);
			matInstance.SetFloat("_Amount", 1f);
			playerRend.color = lightColor;
			yield return wait;
			playerRend.color = darkColor;
			yield return wait;
			playerRend.color = Color.white;
			yield return wait;
			matInstance.SetFloat("_Amount", 0f);
			yield return wait;
			matInstance.SetFloat("_Amount", 1f);
			playerRend.color = darkColor;
			yield return wait;
			playerRend.color = Color.white;
			yield return wait;
			playerRend.color = lightColor;
			yield return wait;
			playerRend.color = Color.white;
			if (levelPlayer != null)
			{
				levelPlayer.animationController.SetOldMaterial();
			}
			else if (planePlayer != null)
			{
				planePlayer.animationController.SetOldMaterial();
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04004428 RID: 17448
	[SerializeField]
	private Material flashMaterial;

	// Token: 0x04004429 RID: 17449
	[SerializeField]
	private HealerCharmParticleEffect particle;

	// Token: 0x0400442A RID: 17450
	private List<Vector2> particleVectors;

	// Token: 0x0400442B RID: 17451
	private int startedFlash;

	// Token: 0x0400442C RID: 17452
	private AbstractPlayerController target;
}
