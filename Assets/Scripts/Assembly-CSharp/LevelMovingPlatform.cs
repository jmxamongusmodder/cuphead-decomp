using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class LevelMovingPlatform : LevelPlatform
{
	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0009DA28 File Offset: 0x0009BE28
	public virtual EaseUtils.EaseType Ease
	{
		get
		{
			return EaseUtils.EaseType.linear;
		}
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x0009DA2C File Offset: 0x0009BE2C
	protected override void Awake()
	{
		base.Awake();
		this.startPos = base.transform.position;
		this.endPos = this.startPos + this.end;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x0009DA7C File Offset: 0x0009BE7C
	private IEnumerator move_cr()
	{
		for (;;)
		{
			yield return base.StartCoroutine(this.goTo_cr(this.startPos, this.endPos));
			yield return new WaitForSeconds(1f);
			yield return base.StartCoroutine(this.goTo_cr(this.endPos, this.startPos));
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x0009DA98 File Offset: 0x0009BE98
	private IEnumerator goTo_cr(Vector3 start, Vector3 end)
	{
		float t = 0f;
		base.transform.position = start;
		while (t < this.time)
		{
			float val = t / this.time;
			Vector3 pos = base.transform.position;
			pos.x = EaseUtils.Ease(this.Ease, start.x, end.x, val);
			pos.y = EaseUtils.Ease(this.Ease, start.y, end.y, val);
			base.transform.position = pos;
			t += Time.deltaTime;
			yield return base.StartCoroutine(base.WaitForPause_CR());
		}
		base.transform.position = end;
		yield break;
	}

	// Token: 0x06000FEB RID: 4075 RVA: 0x0009DAC4 File Offset: 0x0009BEC4
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.magenta;
		if (Application.isPlaying)
		{
			Gizmos.DrawLine(this.startPos, this.endPos);
			Gizmos.DrawWireSphere(base.transform.position, 5f);
		}
		else
		{
			Gizmos.DrawLine(base.baseTransform.position, base.baseTransform.position + this.end);
		}
	}

	// Token: 0x04001980 RID: 6528
	[SerializeField]
	private float time;

	// Token: 0x04001981 RID: 6529
	[SerializeField]
	private Vector2 end;

	// Token: 0x04001982 RID: 6530
	private Vector3 startPos;

	// Token: 0x04001983 RID: 6531
	private Vector3 endPos;
}
