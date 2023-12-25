using System;
using UnityEngine;

// Token: 0x020003A0 RID: 928
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class AnimationHelper : AbstractMonoBehaviour
{
	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00082CE3 File Offset: 0x000810E3
	// (set) Token: 0x06000B4E RID: 2894 RVA: 0x00082CEB File Offset: 0x000810EB
	public CupheadTime.Layer Layer
	{
		get
		{
			return this.layer;
		}
		set
		{
			this.layer = value;
			this.Set();
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x00082CFA File Offset: 0x000810FA
	// (set) Token: 0x06000B50 RID: 2896 RVA: 0x00082D07 File Offset: 0x00081107
	public float LayerSpeed
	{
		get
		{
			return CupheadTime.GetLayerSpeed(this.Layer);
		}
		set
		{
			CupheadTime.SetLayerSpeed(this.Layer, value);
			this.Set();
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00082D1B File Offset: 0x0008111B
	// (set) Token: 0x06000B52 RID: 2898 RVA: 0x00082D23 File Offset: 0x00081123
	public float Speed
	{
		get
		{
			return this.speed;
		}
		set
		{
			this.speed = value;
			this.Set();
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00082D32 File Offset: 0x00081132
	// (set) Token: 0x06000B54 RID: 2900 RVA: 0x00082D3A File Offset: 0x0008113A
	public bool IgnoreGlobal
	{
		get
		{
			return this.ignoreGlobal;
		}
		set
		{
			this.ignoreGlobal = value;
			this.Set();
		}
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00082D4C File Offset: 0x0008114C
	protected override void Awake()
	{
		base.Awake();
		if (base.animator == null)
		{
			global::Debug.LogError("AnimationHelper needs Animator component", null);
			UnityEngine.Object.Destroy(this);
			return;
		}
		CupheadTime.OnChangedEvent.Add(new Action(this.Set));
		this.Set();
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00082D9E File Offset: 0x0008119E
	private void Update()
	{
		if (this.autoUpdate)
		{
			this.Set();
		}
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00082DB1 File Offset: 0x000811B1
	private void OnDestroy()
	{
		CupheadTime.OnChangedEvent.Remove(new Action(this.Set));
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00082DCC File Offset: 0x000811CC
	protected void Set()
	{
		if (this.IgnoreGlobal)
		{
			base.animator.speed = this.Speed * this.LayerSpeed;
		}
		else
		{
			base.animator.speed = this.Speed * this.LayerSpeed * CupheadTime.GlobalSpeed;
		}
	}

	// Token: 0x040014FC RID: 5372
	[SerializeField]
	private CupheadTime.Layer layer;

	// Token: 0x040014FD RID: 5373
	[SerializeField]
	private float speed = 1f;

	// Token: 0x040014FE RID: 5374
	[SerializeField]
	private bool ignoreGlobal;

	// Token: 0x040014FF RID: 5375
	[SerializeField]
	private bool autoUpdate;
}
