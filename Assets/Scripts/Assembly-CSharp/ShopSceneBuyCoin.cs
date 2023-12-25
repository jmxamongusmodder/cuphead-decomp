using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B05 RID: 2821
public class ShopSceneBuyCoin : MonoBehaviour
{
	// Token: 0x0600446C RID: 17516 RVA: 0x002434D8 File Offset: 0x002418D8
	private void Start()
	{
		this.velocity = new Vector2(UnityEngine.Random.Range(this.VelocityXMin, this.VelocityXMax), UnityEngine.Random.Range(this.VelocityYMin, this.VelocityYMax));
		this.randomRotation = new Vector2((float)UnityEngine.Random.Range(-500, 500), (float)UnityEngine.Random.Range(-500, 500));
		base.StartCoroutine(this.scaledown_cr());
	}

	// Token: 0x0600446D RID: 17517 RVA: 0x0024354C File Offset: 0x0024194C
	private void Update()
	{
		base.transform.position += (this.velocity + new Vector2(-300f, this.accumulatedGravity)) * Time.fixedDeltaTime;
		this.accumulatedGravity += -100f;
		base.transform.Rotate(this.randomRotation * Time.deltaTime);
	}

	// Token: 0x0600446E RID: 17518 RVA: 0x002435CC File Offset: 0x002419CC
	private IEnumerator scaledown_cr()
	{
		Vector2 startScale = base.transform.localScale;
		float t = 0f;
		float TIME = 1f;
		while (t < TIME)
		{
			float val = t / TIME;
			float newAlpha = Mathf.Lerp(1f, 0f, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, val));
			Color newColor = this.spriteRenderer.color;
			newColor.a = newAlpha;
			this.spriteRenderer.color = newColor;
			t += Time.deltaTime;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600446F RID: 17519 RVA: 0x002435E7 File Offset: 0x002419E7
	private void OnDestroy()
	{
		this.spriteRenderer = null;
	}

	// Token: 0x04004A06 RID: 18950
	public float VelocityXMin = -500f;

	// Token: 0x04004A07 RID: 18951
	public float VelocityXMax = 500f;

	// Token: 0x04004A08 RID: 18952
	public float VelocityYMin = 500f;

	// Token: 0x04004A09 RID: 18953
	public float VelocityYMax = 1000f;

	// Token: 0x04004A0A RID: 18954
	private const float GRAVITY = -100f;

	// Token: 0x04004A0B RID: 18955
	private Vector2 velocity;

	// Token: 0x04004A0C RID: 18956
	private Vector2 randomRotation;

	// Token: 0x04004A0D RID: 18957
	private float accumulatedGravity;

	// Token: 0x04004A0E RID: 18958
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x04004A0F RID: 18959
	[SerializeField]
	private bool isCoinA;
}
