using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000678 RID: 1656
public class FlyingGenieLevelRing : BasicProjectile
{
	// Token: 0x060022E3 RID: 8931 RVA: 0x00147830 File Offset: 0x00145C30
	protected override void Start()
	{
		base.Start();
		if (this.isMain)
		{
			base.GetComponent<Collider2D>().enabled = false;
			base.animator.Play("Off");
		}
		else
		{
			base.StartCoroutine(this.fade_cr());
		}
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x0014787C File Offset: 0x00145C7C
	private IEnumerator fade_cr()
	{
		float frameTime = 0f;
		float color = 1f;
		while (color > 0f)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, color);
			frameTime += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				color -= 0.05f;
				frameTime -= 0.041666668f;
			}
			yield return null;
		}
		this.OnComplete();
		yield return null;
		yield break;
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x00147897 File Offset: 0x00145C97
	private void OnComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x001478A4 File Offset: 0x00145CA4
	public void DisableCollision()
	{
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04002B82 RID: 11138
	private const float FRAME_TIME = 0.041666668f;

	// Token: 0x04002B83 RID: 11139
	public bool isMain;
}
