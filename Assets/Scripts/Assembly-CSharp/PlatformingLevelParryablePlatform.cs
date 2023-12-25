using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000907 RID: 2311
public class PlatformingLevelParryablePlatform : ParrySwitch
{
	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x0600363C RID: 13884 RVA: 0x001F7963 File Offset: 0x001F5D63
	// (set) Token: 0x0600363D RID: 13885 RVA: 0x001F7970 File Offset: 0x001F5D70
	public new bool enabled
	{
		get
		{
			return base.GetComponent<Collider2D>().enabled;
		}
		set
		{
			base.GetComponent<Collider2D>().enabled = value;
		}
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x001F7980 File Offset: 0x001F5D80
	private void Start()
	{
		this.platform.SetActive(false);
		this.platform.transform.SetScale(new float?(this.platformWidth), new float?(5f), new float?(5f));
		this.pink = base.GetComponent<SpriteRenderer>().color;
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x001F79DC File Offset: 0x001F5DDC
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		this.platform.SetActive(true);
		this.enabled = false;
		base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x001F7A30 File Offset: 0x001F5E30
	private IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.openDuration);
		this.platform.SetActive(false);
		this.enabled = true;
		base.GetComponent<SpriteRenderer>().color = this.pink;
		yield break;
	}

	// Token: 0x04003E36 RID: 15926
	[SerializeField]
	private GameObject platform;

	// Token: 0x04003E37 RID: 15927
	[SerializeField]
	private float openDuration = 3f;

	// Token: 0x04003E38 RID: 15928
	[SerializeField]
	private float platformWidth = 36f;

	// Token: 0x04003E39 RID: 15929
	private Color pink;
}
