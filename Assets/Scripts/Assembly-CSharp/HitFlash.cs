using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class HitFlash : AbstractMonoBehaviour
{
	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000FCF RID: 4047 RVA: 0x0009CE99 File Offset: 0x0009B299
	// (set) Token: 0x06000FD0 RID: 4048 RVA: 0x0009CEA1 File Offset: 0x0009B2A1
	public bool flashing { get; private set; }

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0009CEAA File Offset: 0x0009B2AA
	// (set) Token: 0x06000FD2 RID: 4050 RVA: 0x0009CEB2 File Offset: 0x0009B2B2
	public bool disabled { get; set; }

	// Token: 0x06000FD3 RID: 4051 RVA: 0x0009CEBC File Offset: 0x0009B2BC
	protected override void Awake()
	{
		base.Awake();
		if (this.includeSelf)
		{
			SpriteRenderer component = base.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.self = new HitFlash.RendererProperties(component);
			}
		}
		this.renderers = new List<HitFlash.RendererProperties>();
		for (int i = 0; i < this.otherRenderers.Length; i++)
		{
			this.renderers.Add(new HitFlash.RendererProperties(this.otherRenderers[i]));
		}
		if (this.damageReceiver == null)
		{
			this.damageReceiver = base.GetComponent<DamageReceiver>();
		}
		if (this.damageReceiver)
		{
			this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x0009CF79 File Offset: 0x0009B379
	private void Update()
	{
		this.time -= CupheadTime.Delta;
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x0009CF92 File Offset: 0x0009B392
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Flash(0.1f);
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x0009CF9F File Offset: 0x0009B39F
	public override void StopAllCoroutines()
	{
		base.StopAllCoroutines();
		this.SetColor(1f);
		this.SetScale(Vector3.one, 1f);
		this.time = 0f;
		this.flashing = false;
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x0009CFD4 File Offset: 0x0009B3D4
	public void StopAllCoroutinesWithoutSettingScale()
	{
		base.StopAllCoroutines();
		this.SetColor(1f);
		this.time = 0f;
		this.flashing = false;
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x0009CFFC File Offset: 0x0009B3FC
	public void Flash(float t = 0.1f)
	{
		if (this.disabled)
		{
			return;
		}
		this.time = t;
		if (this.flashing)
		{
			return;
		}
		if (base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.flash_cr());
		}
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0009D058 File Offset: 0x0009B458
	public void SetColor(float t)
	{
		if (this.self != null)
		{
			Color color = Color.Lerp(this.self.normalColor, this.damageColor, t);
			this.self.renderer.color = color;
		}
		foreach (HitFlash.RendererProperties rendererProperties in this.renderers)
		{
			Color color2 = Color.Lerp(rendererProperties.normalColor, this.damageColor, t);
			rendererProperties.renderer.color = color2;
		}
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0009D100 File Offset: 0x0009B500
	private void SetScale(Vector3 original, float s)
	{
		if (this.self != null)
		{
			this.self.transform.localScale = original * s;
		}
		foreach (HitFlash.RendererProperties rendererProperties in this.renderers)
		{
			rendererProperties.transform.localScale = rendererProperties.scale * s;
		}
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x0009D190 File Offset: 0x0009B590
	private IEnumerator flash_cr()
	{
		this.flashing = true;
		while (this.time > 0f)
		{
			this.SetColor(1f);
			yield return CupheadTime.WaitForSeconds(this, 0.0416f);
			this.SetColor(0f);
			yield return CupheadTime.WaitForSeconds(this, 0.0832f);
		}
		this.flashing = false;
		yield break;
	}

	// Token: 0x04001967 RID: 6503
	[SerializeField]
	private Color damageColor = new Color(1f, 0f, 0f, 1f);

	// Token: 0x04001968 RID: 6504
	[SerializeField]
	private DamageReceiver damageReceiver;

	// Token: 0x04001969 RID: 6505
	[SerializeField]
	private bool includeSelf = true;

	// Token: 0x0400196A RID: 6506
	public SpriteRenderer[] otherRenderers;

	// Token: 0x0400196B RID: 6507
	private float time;

	// Token: 0x0400196E RID: 6510
	private Coroutine coroutine;

	// Token: 0x0400196F RID: 6511
	private HitFlash.RendererProperties self;

	// Token: 0x04001970 RID: 6512
	private List<HitFlash.RendererProperties> renderers;

	// Token: 0x02000437 RID: 1079
	public class RendererProperties
	{
		// Token: 0x06000FDC RID: 4060 RVA: 0x0009D1AB File Offset: 0x0009B5AB
		public RendererProperties(SpriteRenderer r)
		{
			this.renderer = r;
			this.normalColor = r.color;
			this.transform = r.transform;
			this.scale = r.transform.localScale;
		}

		// Token: 0x04001971 RID: 6513
		public readonly SpriteRenderer renderer;

		// Token: 0x04001972 RID: 6514
		public readonly Color normalColor;

		// Token: 0x04001973 RID: 6515
		public readonly Transform transform;

		// Token: 0x04001974 RID: 6516
		public readonly Vector3 scale;
	}
}
