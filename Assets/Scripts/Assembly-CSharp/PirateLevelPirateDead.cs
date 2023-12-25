using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000730 RID: 1840
public class PirateLevelPirateDead : AbstractMonoBehaviour
{
	// Token: 0x0600281F RID: 10271 RVA: 0x0017690D File Offset: 0x00174D0D
	protected override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x00176921 File Offset: 0x00174D21
	public void Go(float delay, float speed)
	{
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.go_cr(delay, speed));
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x0017693E File Offset: 0x00174D3E
	private void End()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x00176954 File Offset: 0x00174D54
	private IEnumerator go_cr(float delay, float time)
	{
		float startY = base.transform.position.y;
		bool splash = false;
		yield return CupheadTime.WaitForSeconds(this, delay);
		float t = 0f;
		while (t < time)
		{
			float y = EaseUtils.Ease(EaseUtils.EaseType.linear, startY, -250f, t / time);
			base.transform.SetLocalPosition(null, new float?(y), null);
			if (!splash && base.transform.position.y <= -25f)
			{
				splash = true;
				this.splashPrefab.Create(base.transform.position + new Vector3(0f, 20f, 0f));
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(null, new float?(-250f), null);
		this.End();
		yield break;
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x0017697D File Offset: 0x00174D7D
	private void OnDestroy()
	{
		this.splashPrefab = null;
	}

	// Token: 0x040030DF RID: 12511
	public const float END_Y = -250f;

	// Token: 0x040030E0 RID: 12512
	public const float SPLASH_Y = -25f;

	// Token: 0x040030E1 RID: 12513
	[SerializeField]
	private Effect splashPrefab;
}
