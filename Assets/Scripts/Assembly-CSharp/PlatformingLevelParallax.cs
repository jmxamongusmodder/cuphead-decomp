using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F8 RID: 2296
[ExecuteInEditMode]
public class PlatformingLevelParallax : AbstractPausableComponent
{
	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x060035D5 RID: 13781 RVA: 0x001F6047 File Offset: 0x001F4447
	public PlatformingLevel.Theme Theme
	{
		get
		{
			return this._theme;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x060035D6 RID: 13782 RVA: 0x001F604F File Offset: 0x001F444F
	public Color Color
	{
		get
		{
			return this._color;
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x060035D7 RID: 13783 RVA: 0x001F6057 File Offset: 0x001F4457
	public PlatformingLevelParallax.Sides Side
	{
		get
		{
			return this._side;
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x060035D8 RID: 13784 RVA: 0x001F605F File Offset: 0x001F445F
	public int Layer
	{
		get
		{
			return this._layer;
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x060035D9 RID: 13785 RVA: 0x001F6067 File Offset: 0x001F4467
	public int SortingOrderOffset
	{
		get
		{
			return this._sortingOrderOffset;
		}
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x060035DA RID: 13786 RVA: 0x001F6070 File Offset: 0x001F4470
	private SpriteRenderer[] _spriteRenderers
	{
		get
		{
			if (this._s == null)
			{
				List<SpriteRenderer> list = new List<SpriteRenderer>(base.GetComponentsInChildren<SpriteRenderer>());
				this._s = list.ToArray();
			}
			return this._s;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x060035DB RID: 13787 RVA: 0x001F60A6 File Offset: 0x001F44A6
	protected new Transform transform
	{
		get
		{
			if (!this.transformCached)
			{
				this._cachedTransform = base.transform;
				this.transformCached = true;
			}
			return this._cachedTransform;
		}
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x060035DC RID: 13788 RVA: 0x001F60CC File Offset: 0x001F44CC
	private ParallaxPropertiesData.ThemeProperties.Layer LayerProperties
	{
		get
		{
			if (!this.layerPropertiesCached)
			{
				this._layerProperties = ParallaxPropertiesData.Instance.GetProperty(this._theme, this._layer, this._side);
				this.layerPropertiesCached = true;
			}
			return this._layerProperties;
		}
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x001F6108 File Offset: 0x001F4508
	private void Start()
	{
		base.FrameDelayedCallback(new Action(this.DelayedStart), 1);
		this.UpdatePosition();
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x001F6124 File Offset: 0x001F4524
	private void DelayedStart()
	{
		this.SetSpriteProperties();
		this.UpdatePosition();
	}

	// Token: 0x060035DF RID: 13791 RVA: 0x001F6134 File Offset: 0x001F4534
	public void UpdateBasePosition()
	{
		if (this.levelCameraTransform == null)
		{
			CupheadLevelCamera cupheadLevelCamera = UnityEngine.Object.FindObjectOfType<CupheadLevelCamera>();
			if (cupheadLevelCamera == null)
			{
				return;
			}
			this.levelCameraTransform = cupheadLevelCamera.transform;
			if (this.levelCameraTransform == null)
			{
				return;
			}
		}
		if (this.overrideLayerYSpeed)
		{
			this.basePos.x = this.transform.position.x - this.levelCameraTransform.position.x * this.LayerProperties.speed;
			this.basePos.y = this.transform.position.y - this.levelCameraTransform.position.y * this.overrideYSpeed;
		}
		else
		{
			this.basePos = this.transform.position - this.levelCameraTransform.position * this.LayerProperties.speed;
		}
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x001F623C File Offset: 0x001F463C
	public void SetSpriteProperties()
	{
		foreach (SpriteRenderer spriteRenderer in this._spriteRenderers)
		{
			spriteRenderer.sortingLayerName = ((this._side != PlatformingLevelParallax.Sides.Background) ? SpriteLayer.Foreground.ToString() : SpriteLayer.Background.ToString());
			spriteRenderer.sortingOrder = this.LayerProperties.sortingOrder + this._sortingOrderOffset;
			PlatformingLevelParallaxChild component = spriteRenderer.gameObject.GetComponent<PlatformingLevelParallaxChild>();
			if (component != null)
			{
				spriteRenderer.sortingOrder += component.SortingOrderOffset;
			}
			spriteRenderer.color = this._color;
		}
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x001F62EF File Offset: 0x001F46EF
	private void LateUpdate()
	{
		this.UpdatePosition();
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x001F62F8 File Offset: 0x001F46F8
	private void UpdatePosition()
	{
		if (this.levelCameraTransform == null)
		{
			CupheadLevelCamera cupheadLevelCamera = UnityEngine.Object.FindObjectOfType<CupheadLevelCamera>();
			if (cupheadLevelCamera == null)
			{
				return;
			}
			this.levelCameraTransform = cupheadLevelCamera.transform;
			if (this.levelCameraTransform == null)
			{
				return;
			}
		}
		if (this.overrideLayerYSpeed)
		{
			this.transform.SetPosition(new float?(this.basePos.x + this.levelCameraTransform.position.x * this.LayerProperties.speed), new float?(this.basePos.y + this.levelCameraTransform.position.y * this.overrideYSpeed), null);
		}
		else
		{
			this.transform.position = this.basePos + this.levelCameraTransform.position * this.LayerProperties.speed;
		}
	}

	// Token: 0x060035E3 RID: 13795 RVA: 0x001F63F8 File Offset: 0x001F47F8
	private void OnValidate()
	{
		foreach (SpriteRenderer spriteRenderer in this._spriteRenderers)
		{
			if (!(spriteRenderer == null))
			{
				spriteRenderer.hideFlags = HideFlags.None;
			}
		}
	}

	// Token: 0x04003DE6 RID: 15846
	[SerializeField]
	private PlatformingLevel.Theme _theme;

	// Token: 0x04003DE7 RID: 15847
	[SerializeField]
	private Color _color = Color.white;

	// Token: 0x04003DE8 RID: 15848
	[SerializeField]
	private PlatformingLevelParallax.Sides _side;

	// Token: 0x04003DE9 RID: 15849
	[SerializeField]
	[Range(0f, 19f)]
	private int _layer;

	// Token: 0x04003DEA RID: 15850
	[SerializeField]
	[Range(-2000f, 2000f)]
	private int _sortingOrderOffset;

	// Token: 0x04003DEB RID: 15851
	[HideInInspector]
	public Vector3 basePos;

	// Token: 0x04003DEC RID: 15852
	[HideInInspector]
	public Vector3 lastPos;

	// Token: 0x04003DED RID: 15853
	public bool overrideLayerYSpeed;

	// Token: 0x04003DEE RID: 15854
	public float overrideYSpeed;

	// Token: 0x04003DEF RID: 15855
	private Transform levelCameraTransform;

	// Token: 0x04003DF0 RID: 15856
	private ParallaxLayer _parallaxLayer;

	// Token: 0x04003DF1 RID: 15857
	private SpriteRenderer[] _s;

	// Token: 0x04003DF2 RID: 15858
	private bool transformCached;

	// Token: 0x04003DF3 RID: 15859
	private Transform _cachedTransform;

	// Token: 0x04003DF4 RID: 15860
	private bool layerPropertiesCached;

	// Token: 0x04003DF5 RID: 15861
	private ParallaxPropertiesData.ThemeProperties.Layer _layerProperties;

	// Token: 0x020008F9 RID: 2297
	public enum Sides
	{
		// Token: 0x04003DF7 RID: 15863
		Background,
		// Token: 0x04003DF8 RID: 15864
		Foreground
	}
}
