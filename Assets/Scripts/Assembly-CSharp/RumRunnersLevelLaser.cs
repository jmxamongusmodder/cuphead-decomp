using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000790 RID: 1936
public class RumRunnersLevelLaser : AbstractCollidableObject
{
	// Token: 0x06002ADE RID: 10974 RVA: 0x00190028 File Offset: 0x0018E428
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		foreach (CollisionChild collisionChild in this.childColliders)
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		}
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x00190078 File Offset: 0x0018E478
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x00190090 File Offset: 0x0018E490
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		float num = this.damageDealer.DealDamage(hit);
		if (num > 0f)
		{
			this.SFX_RUMRUN_Grammobeam_DamagePlayer();
		}
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x001900C4 File Offset: 0x0018E4C4
	private IEnumerator moveMask_cr(GameObject laserMask, float startX, float endX, float duration, bool destroyMask)
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			laserMask.transform.SetLocalPosition(new float?(EaseUtils.Linear(startX, endX, elapsedTime / duration)), null, null);
		}
		if (destroyMask)
		{
			UnityEngine.Object.Destroy(laserMask);
		}
		yield break;
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x001900FD File Offset: 0x0018E4FD
	public void Begin()
	{
		base.StartCoroutine(this.begin_cr(this.mainRenderers, 1f));
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x00190118 File Offset: 0x0018E518
	private IEnumerator begin_cr(SpriteRenderer[] renderers, float durationMultiplier = 1f)
	{
		MinMax DurationRange = new MinMax(1.2f, 1.5f);
		foreach (SpriteRenderer renderer in renderers)
		{
			renderer.enabled = true;
			GameObject laserMask = UnityEngine.Object.Instantiate<GameObject>(this.laserMaskPrefab);
			laserMask.transform.parent = renderer.transform;
			laserMask.transform.ResetLocalRotation();
			laserMask.transform.localPosition = new Vector3(-400f, 0f);
			laserMask.GetComponent<RumRunnersLevelLaserMask>().Setup(renderer.sortingLayerID, renderer.sortingOrder);
			base.StartCoroutine(this.moveMask_cr(laserMask, -400f, 800f, DurationRange.RandomFloat() * durationMultiplier, true));
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x00190141 File Offset: 0x0018E541
	public void End()
	{
		base.StartCoroutine(this.end_cr());
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x00190150 File Offset: 0x0018E550
	private IEnumerator end_cr()
	{
		MinMax DurationRange = new MinMax(1.2f, 1.5f);
		Coroutine[] coroutines = new Coroutine[this.mainRenderers.Length];
		for (int i = 0; i < this.mainRenderers.Length; i++)
		{
			SpriteRenderer renderer = this.mainRenderers[i];
			GameObject laserMask = UnityEngine.Object.Instantiate<GameObject>(this.laserMaskPrefab);
			laserMask.transform.parent = renderer.transform;
			laserMask.transform.ResetLocalRotation();
			laserMask.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
			laserMask.transform.localPosition = new Vector3(220f, 0f);
			laserMask.GetComponent<RumRunnersLevelLaserMask>().Setup(renderer.sortingLayerID, renderer.sortingOrder);
			coroutines[i] = base.StartCoroutine(this.moveMask_cr(laserMask, 220f, 1600f, DurationRange.RandomFloat(), false));
			base.StartCoroutine(this.endSparkles_cr(laserMask.transform));
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		foreach (Coroutine coroutine in coroutines)
		{
			yield return coroutine;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x0019016C File Offset: 0x0018E56C
	private IEnumerator endSparkles_cr(Transform maskTransform)
	{
		MinMax SpawnRandomizationRange = new MinMax(-15f, 15f);
		float elapsedTime = 0f;
		for (;;)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			if (elapsedTime >= 0.02f)
			{
				elapsedTime -= 0.02f;
				for (int i = 0; i < 1; i++)
				{
					this.sparklesEffect.Create(maskTransform.position + maskTransform.right * 280f + new Vector3(SpawnRandomizationRange.RandomFloat(), SpawnRandomizationRange.RandomFloat()));
				}
			}
		}
		yield break;
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x0019018E File Offset: 0x0018E58E
	public void Warning()
	{
		base.StartCoroutine(this.warning_cr());
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x001901A0 File Offset: 0x0018E5A0
	private IEnumerator warning_cr()
	{
		float elapsedTime = 0f;
		while (elapsedTime < 0.3f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 0.3f);
			for (int i = 0; i < this.mainRenderers.Length; i++)
			{
				Color color = this.mainRenderers[i].color;
				color.a = alpha;
				this.mainRenderers[i].color = color;
				color = this.warningRenderers[i].color;
				color.a = 1f - alpha;
				this.warningRenderers[i].color = color;
			}
		}
		yield break;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x001901BC File Offset: 0x0018E5BC
	public void CancelWarning()
	{
		this.StopAllCoroutines();
		foreach (SpriteRenderer spriteRenderer in this.mainRenderers)
		{
			Color color = spriteRenderer.color;
			color.a = 1f;
			spriteRenderer.color = color;
		}
		foreach (SpriteRenderer spriteRenderer2 in this.warningRenderers)
		{
			Color color2 = spriteRenderer2.color;
			color2.a = 0f;
			spriteRenderer2.color = color2;
		}
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x00190250 File Offset: 0x0018E650
	public void Attack()
	{
		base.animator.SetBool("On", true);
		foreach (SpriteRenderer spriteRenderer in this.notesRenderers)
		{
			spriteRenderer.enabled = false;
		}
		base.StartCoroutine(this.begin_cr(this.notesRenderers, 0.5f));
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x001902AC File Offset: 0x0018E6AC
	public void EndAttack()
	{
		base.animator.SetBool("On", false);
	}

	// Token: 0x06002AEC RID: 10988 RVA: 0x001902C0 File Offset: 0x0018E6C0
	private void animationEvent_WarningToOnStarted()
	{
		foreach (SpriteRenderer spriteRenderer in this.mainRenderers)
		{
			Color color = spriteRenderer.color;
			color.a = 1f;
			spriteRenderer.color = color;
		}
		foreach (SpriteRenderer spriteRenderer2 in this.warningRenderers)
		{
			Color color2 = spriteRenderer2.color;
			color2.a = 0f;
			spriteRenderer2.color = color2;
		}
	}

	// Token: 0x06002AED RID: 10989 RVA: 0x0019034C File Offset: 0x0018E74C
	private void SFX_RUMRUN_Grammobeam_DamagePlayer()
	{
		AudioManager.Play("sfx_dlc_rumrun_p2_grammobeam_damageplayer");
	}

	// Token: 0x0400339D RID: 13213
	[SerializeField]
	private SpriteRenderer[] mainRenderers;

	// Token: 0x0400339E RID: 13214
	[SerializeField]
	private SpriteRenderer[] warningRenderers;

	// Token: 0x0400339F RID: 13215
	[SerializeField]
	private SpriteRenderer[] notesRenderers;

	// Token: 0x040033A0 RID: 13216
	[SerializeField]
	private CollisionChild[] childColliders;

	// Token: 0x040033A1 RID: 13217
	[SerializeField]
	private GameObject laserMaskPrefab;

	// Token: 0x040033A2 RID: 13218
	[SerializeField]
	private Effect sparklesEffect;

	// Token: 0x040033A3 RID: 13219
	private DamageDealer damageDealer;
}
