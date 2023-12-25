using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006DC RID: 1756
public class MouseLevelBackgroundHopper : AbstractMonoBehaviour
{
	// Token: 0x06002565 RID: 9573 RVA: 0x0015D9DB File Offset: 0x0015BDDB
	protected override void Awake()
	{
		base.Awake();
		this.startPos = base.transform.localPosition;
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x0015D9F9 File Offset: 0x0015BDF9
	private void Start()
	{
		CupheadLevelCamera.Current.OnShakeEvent += this.OnShake;
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x0015DA11 File Offset: 0x0015BE11
	private void OnDestroy()
	{
		if (CupheadLevelCamera.Current != null)
		{
			this.RemoveShake();
		}
	}

	// Token: 0x06002568 RID: 9576 RVA: 0x0015DA29 File Offset: 0x0015BE29
	private void RemoveShake()
	{
		CupheadLevelCamera.Current.OnShakeEvent -= this.OnShake;
	}

	// Token: 0x06002569 RID: 9577 RVA: 0x0015DA41 File Offset: 0x0015BE41
	private void OnShake(float amount, float time)
	{
		if (this.hopCoroutine != null)
		{
			base.StopCoroutine(this.hopCoroutine);
		}
		this.hopCoroutine = base.StartCoroutine(this.hop_cr(amount));
	}

	// Token: 0x0600256A RID: 9578 RVA: 0x0015DA70 File Offset: 0x0015BE70
	private IEnumerator hop_cr(float amount)
	{
		for (int i = 0; i < this.hops.Length; i++)
		{
			float height = this.hops[i].height;
			float time = this.hops[i].time;
			Vector2 endPos = this.startPos + new Vector2(0f, height);
			float ht = time / 2f;
			yield return base.StartCoroutine(this.tween_cr(this.startPos.y, endPos.y, ht, EaseUtils.EaseType.easeOutSine));
			yield return base.StartCoroutine(this.tween_cr(endPos.y, this.startPos.y, ht, EaseUtils.EaseType.easeInSine));
			time /= 2f;
			height /= 2f;
		}
		yield break;
	}

	// Token: 0x0600256B RID: 9579 RVA: 0x0015DA8C File Offset: 0x0015BE8C
	private IEnumerator tween_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		base.transform.SetLocalPosition(new float?(this.startPos.x), new float?(start), new float?(0f));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetLocalPosition(new float?(this.startPos.x), new float?(EaseUtils.Ease(ease, start, end, val)), new float?(0f));
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(this.startPos.x), new float?(end), new float?(0f));
		yield break;
	}

	// Token: 0x04002DF2 RID: 11762
	public MouseLevelBackgroundHopper.Hop[] hops = new MouseLevelBackgroundHopper.Hop[1];

	// Token: 0x04002DF3 RID: 11763
	private Coroutine hopCoroutine;

	// Token: 0x04002DF4 RID: 11764
	private Vector2 startPos;

	// Token: 0x020006DD RID: 1757
	[Serializable]
	public class Hop
	{
		// Token: 0x04002DF5 RID: 11765
		public float height = 50f;

		// Token: 0x04002DF6 RID: 11766
		public float time = 0.25f;
	}
}
