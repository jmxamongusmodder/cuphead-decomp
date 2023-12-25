using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020008A5 RID: 2213
public class CircusPlatformingLevelHotdogProjectile : BasicProjectile
{
	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06003384 RID: 13188 RVA: 0x001DF3AC File Offset: 0x001DD7AC
	// (remove) Token: 0x06003385 RID: 13189 RVA: 0x001DF3E4 File Offset: 0x001DD7E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<CircusPlatformingLevelHotdogProjectile> OnDestroyCallback;

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06003386 RID: 13190 RVA: 0x001DF41A File Offset: 0x001DD81A
	protected override float DestroyLifetime
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x001DF421 File Offset: 0x001DD821
	protected override void Awake()
	{
		base.Awake();
		this.collider2d = base.GetComponent<Collider2D>();
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x001DF438 File Offset: 0x001DD838
	protected override void Start()
	{
		base.Start();
		base.transform.localScale = new Vector3(0f, 1f, 1f);
		this.spark.Create(base.transform.position - new Vector3(10f, 0f, 0f));
		base.StartCoroutine(this.scaleOnStart_cr());
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x001DF4A8 File Offset: 0x001DD8A8
	private IEnumerator scaleOnStart_cr()
	{
		while (base.transform.localScale.x < 1f)
		{
			base.transform.AddScale(this.scaleFactor * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		yield break;
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x001DF4C4 File Offset: 0x001DD8C4
	public void Side(bool isRight)
	{
		if (isRight)
		{
			for (int i = 0; i < this.renderers.Length; i++)
			{
				this.renderers[i].sortingOrder += 3;
			}
		}
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x001DF508 File Offset: 0x001DD908
	public void SetCondiment(string type)
	{
		if (type == "K")
		{
			base.animator.Play("Ketchup");
		}
		else if (type == "M")
		{
			base.animator.Play("Mustard");
		}
		else if (type == "R")
		{
			base.animator.Play("Relish");
		}
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x001DF57F File Offset: 0x001DD97F
	public void EnableCollider(bool enable)
	{
		if (this.collider2d != null)
		{
			this.collider2d.enabled = enable;
		}
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x001DF59E File Offset: 0x001DD99E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.OnDestroyCallback != null)
		{
			this.OnDestroyCallback(this);
		}
		this.spark = null;
	}

	// Token: 0x04003BD9 RID: 15321
	private const string KetchupState = "Ketchup";

	// Token: 0x04003BDA RID: 15322
	private const string MustardState = "Mustard";

	// Token: 0x04003BDB RID: 15323
	private const string RelishState = "Relish";

	// Token: 0x04003BDC RID: 15324
	private const string Ketchup = "K";

	// Token: 0x04003BDD RID: 15325
	private const string Mustard = "M";

	// Token: 0x04003BDE RID: 15326
	private const string Relish = "R";

	// Token: 0x04003BDF RID: 15327
	[SerializeField]
	private float scaleFactor;

	// Token: 0x04003BE0 RID: 15328
	[SerializeField]
	private SpriteRenderer[] renderers;

	// Token: 0x04003BE1 RID: 15329
	[SerializeField]
	private Effect spark;

	// Token: 0x04003BE2 RID: 15330
	private Collider2D collider2d;
}
