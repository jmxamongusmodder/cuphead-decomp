using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000729 RID: 1833
public class PirateLevelSquidInkOverlay : LevelProperties.Pirate.Entity
{
	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x060027F3 RID: 10227 RVA: 0x001758C4 File Offset: 0x00173CC4
	// (set) Token: 0x060027F4 RID: 10228 RVA: 0x001758CB File Offset: 0x00173CCB
	public static PirateLevelSquidInkOverlay Current { get; private set; }

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x060027F5 RID: 10229 RVA: 0x001758D4 File Offset: 0x00173CD4
	// (set) Token: 0x060027F6 RID: 10230 RVA: 0x001758F4 File Offset: 0x00173CF4
	private float alpha
	{
		get
		{
			return this.spriteRenderer.color.a;
		}
		set
		{
			this.color.a = Mathf.Clamp(value, 0f, 1f);
			this.spriteRenderer.color = this.color;
		}
	}

	// Token: 0x060027F7 RID: 10231 RVA: 0x00175924 File Offset: 0x00173D24
	protected override void Awake()
	{
		base.Awake();
		PirateLevelSquidInkOverlay.Current = this;
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.spriteRenderer.enabled = false;
		this.alpha = 0f;
		this.color = this.spriteRenderer.color;
		this.splatGroups = new List<PirateLevelSquidInkOverlay.SplatGroup>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.name.ToLower().Contains("group"))
				{
					this.splatGroups.Add(new PirateLevelSquidInkOverlay.SplatGroup(transform));
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x060027F8 RID: 10232 RVA: 0x001759F8 File Offset: 0x00173DF8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		PirateLevelSquidInkOverlay.Current = null;
		this.smallSplat = null;
		this.largeSplat = null;
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x00175A14 File Offset: 0x00173E14
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		IEnumerator enumerator = base.baseTransform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeInHierarchy)
				{
					IEnumerator enumerator2 = transform.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							Transform transform2 = (Transform)obj2;
							if (transform2.name.ToLower().Contains("small"))
							{
								Gizmos.DrawWireSphere(transform2.position, 20f);
							}
							else if (transform2.name.ToLower().Contains("large"))
							{
								Gizmos.DrawWireSphere(transform2.position, 40f);
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x00175B30 File Offset: 0x00173F30
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x00175B3C File Offset: 0x00173F3C
	public void Hit()
	{
		if (!this.SFXSplatScreenActive)
		{
			AudioManager.Play("level_pirate_squid_blackout_screen");
			this.SFXSplatScreenActive = true;
		}
		LevelProperties.Pirate.Squid squid = base.properties.CurrentState.squid;
		this.spriteRenderer.enabled = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.splats_cr());
		base.StartCoroutine(this.hit_cr(squid));
	}

	// Token: 0x060027FC RID: 10236 RVA: 0x00175BA4 File Offset: 0x00173FA4
	private IEnumerator splats_cr()
	{
		PirateLevelSquidInkOverlay.SplatGroup group = this.splatGroups[UnityEngine.Random.Range(0, this.splatGroups.Count)];
		group.RandomizeDelay(10);
		for (int i = 0; i < 10; i++)
		{
			foreach (PirateLevelSquidInkOverlay.SplatGroup.Splat splat in group.splats)
			{
				if (splat.delay == i)
				{
					Vector3 position = splat.position;
					if (splat.type == PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Large)
					{
						this.largeSplat.Create(position);
					}
					else
					{
						this.smallSplat.Create(position);
					}
				}
			}
			yield return CupheadTime.WaitForSeconds(this, 0.025f);
		}
		yield break;
	}

	// Token: 0x060027FD RID: 10237 RVA: 0x00175BC0 File Offset: 0x00173FC0
	private IEnumerator hit_cr(LevelProperties.Pirate.Squid p)
	{
		if (!this.SFXSplatScreenActive)
		{
			AudioManager.Play("level_pirate_squid_blackout_screen");
			this.SFXSplatScreenActive = true;
		}
		this.targetAlpha = Mathf.Clamp(this.targetAlpha + p.opacityAdd, 0f, 1f);
		float t = 0f;
		while (t < p.opacityAddTime)
		{
			float val = t / p.opacityAddTime;
			this.alpha = Mathf.Lerp(this.alpha, this.targetAlpha, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, p.darkHoldTime);
		yield return base.StartCoroutine(this.fade_cr(p));
		yield break;
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x00175BE4 File Offset: 0x00173FE4
	private IEnumerator fade_cr(LevelProperties.Pirate.Squid p)
	{
		float t = 0f;
		while (t < p.darkFadeTime)
		{
			float val = t / p.darkFadeTime;
			this.alpha = Mathf.Lerp(this.alpha, 0f, val);
			this.targetAlpha = this.alpha;
			t += CupheadTime.Delta;
			yield return null;
		}
		this.alpha = 0f;
		this.targetAlpha = this.alpha;
		this.spriteRenderer.enabled = false;
		this.SFXSplatScreenActive = false;
		yield break;
	}

	// Token: 0x040030B5 RID: 12469
	private const int DELAY_MAX = 10;

	// Token: 0x040030B6 RID: 12470
	private const float DELAY_WAIT = 0.025f;

	// Token: 0x040030B8 RID: 12472
	[SerializeField]
	private Effect largeSplat;

	// Token: 0x040030B9 RID: 12473
	[SerializeField]
	private Effect smallSplat;

	// Token: 0x040030BA RID: 12474
	private SpriteRenderer spriteRenderer;

	// Token: 0x040030BB RID: 12475
	private List<PirateLevelSquidInkOverlay.SplatGroup> splatGroups;

	// Token: 0x040030BC RID: 12476
	private bool SFXSplatScreenActive;

	// Token: 0x040030BD RID: 12477
	private Color color;

	// Token: 0x040030BE RID: 12478
	private float targetAlpha;

	// Token: 0x0200072A RID: 1834
	public class SplatGroup
	{
		// Token: 0x060027FF RID: 10239 RVA: 0x00175C08 File Offset: 0x00174008
		public SplatGroup(Transform parent)
		{
			this.splats = new List<PirateLevelSquidInkOverlay.SplatGroup.Splat>();
			IEnumerator enumerator = parent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					this.splats.Add(new PirateLevelSquidInkOverlay.SplatGroup.Splat(transform));
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x00175C84 File Offset: 0x00174084
		public void RandomizeDelay(int max)
		{
			foreach (PirateLevelSquidInkOverlay.SplatGroup.Splat splat in this.splats)
			{
				splat.delay = UnityEngine.Random.Range(0, max);
			}
		}

		// Token: 0x040030BF RID: 12479
		public List<PirateLevelSquidInkOverlay.SplatGroup.Splat> splats;

		// Token: 0x0200072B RID: 1835
		public class Splat
		{
			// Token: 0x06002801 RID: 10241 RVA: 0x00175CE8 File Offset: 0x001740E8
			public Splat(Transform transform)
			{
				this.position = transform.position;
				if (transform.name.ToLower().Contains("small"))
				{
					this.type = PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Small;
				}
				else
				{
					this.type = PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Large;
				}
			}

			// Token: 0x040030C0 RID: 12480
			public readonly PirateLevelSquidInkOverlay.SplatGroup.Splat.Type type;

			// Token: 0x040030C1 RID: 12481
			public readonly Vector2 position;

			// Token: 0x040030C2 RID: 12482
			public int delay;

			// Token: 0x0200072C RID: 1836
			public enum Type
			{
				// Token: 0x040030C4 RID: 12484
				Small,
				// Token: 0x040030C5 RID: 12485
				Large
			}
		}
	}
}
