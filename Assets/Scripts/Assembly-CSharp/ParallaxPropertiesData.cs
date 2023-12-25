using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F4 RID: 2292
public class ParallaxPropertiesData : ScriptableObject
{
	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x060035C9 RID: 13769 RVA: 0x001F5DF2 File Offset: 0x001F41F2
	public static ParallaxPropertiesData Instance
	{
		get
		{
			if (ParallaxPropertiesData._instance == null)
			{
				ParallaxPropertiesData._instance = Resources.Load<ParallaxPropertiesData>("Parallax/data");
			}
			return ParallaxPropertiesData._instance;
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x060035CA RID: 13770 RVA: 0x001F5E18 File Offset: 0x001F4218
	public List<ParallaxPropertiesData.ThemeProperties> Properties
	{
		get
		{
			return this._properties;
		}
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x001F5E20 File Offset: 0x001F4220
	public ParallaxPropertiesData.ThemeProperties.Layer GetProperty(PlatformingLevel.Theme theme, int layer, PlatformingLevelParallax.Sides side)
	{
		if (side == PlatformingLevelParallax.Sides.Background || side != PlatformingLevelParallax.Sides.Foreground)
		{
			return this.GetTheme(theme).background.GetLayer(layer);
		}
		return this.GetTheme(theme).foreground.GetLayer(layer);
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x001F5E5C File Offset: 0x001F425C
	private ParallaxPropertiesData.ThemeProperties GetTheme(PlatformingLevel.Theme theme)
	{
		foreach (ParallaxPropertiesData.ThemeProperties themeProperties in this._properties)
		{
			if (themeProperties.theme == theme)
			{
				return themeProperties;
			}
		}
		return null;
	}

	// Token: 0x04003DDA RID: 15834
	private const string PATH = "Parallax/data";

	// Token: 0x04003DDB RID: 15835
	public const int LAYER_COUNT = 20;

	// Token: 0x04003DDC RID: 15836
	private static ParallaxPropertiesData _instance;

	// Token: 0x04003DDD RID: 15837
	[SerializeField]
	private List<ParallaxPropertiesData.ThemeProperties> _properties;

	// Token: 0x020008F5 RID: 2293
	[Serializable]
	public class ThemeProperties
	{
		// Token: 0x060035CD RID: 13773 RVA: 0x001F5EC8 File Offset: 0x001F42C8
		public ThemeProperties()
		{
			this.background = new ParallaxPropertiesData.ThemeProperties.LayerGroup();
			this.background.InvertSortingOrder();
			this.foreground = new ParallaxPropertiesData.ThemeProperties.LayerGroup();
			this.foreground.InvertSpeed();
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x001F5EFC File Offset: 0x001F42FC
		public ThemeProperties(PlatformingLevel.Theme theme)
		{
			this.theme = theme;
			this.background = new ParallaxPropertiesData.ThemeProperties.LayerGroup();
			this.background.InvertSortingOrder();
			this.foreground = new ParallaxPropertiesData.ThemeProperties.LayerGroup();
			this.foreground.InvertSpeed();
		}

		// Token: 0x04003DDE RID: 15838
		public PlatformingLevel.Theme theme;

		// Token: 0x04003DDF RID: 15839
		public ParallaxPropertiesData.ThemeProperties.LayerGroup background;

		// Token: 0x04003DE0 RID: 15840
		public ParallaxPropertiesData.ThemeProperties.LayerGroup foreground;

		// Token: 0x04003DE1 RID: 15841
		[NonSerialized]
		public bool zEditor_expanded;

		// Token: 0x020008F6 RID: 2294
		[Serializable]
		public class LayerGroup
		{
			// Token: 0x060035CF RID: 13775 RVA: 0x001F5F38 File Offset: 0x001F4338
			public LayerGroup()
			{
				this.layers = new ParallaxPropertiesData.ThemeProperties.Layer[20];
				for (int i = 0; i < this.layers.Length; i++)
				{
					this.layers[i] = new ParallaxPropertiesData.ThemeProperties.Layer();
					this.layers[i].speed = 0.05f * (float)(i + 1);
					this.layers[i].sortingOrder = 100 * (i + 1);
				}
			}

			// Token: 0x060035D0 RID: 13776 RVA: 0x001F5FA8 File Offset: 0x001F43A8
			public void InvertSpeed()
			{
				foreach (ParallaxPropertiesData.ThemeProperties.Layer layer in this.layers)
				{
					layer.speed *= -1f;
				}
			}

			// Token: 0x060035D1 RID: 13777 RVA: 0x001F5FE8 File Offset: 0x001F43E8
			public void InvertSortingOrder()
			{
				foreach (ParallaxPropertiesData.ThemeProperties.Layer layer in this.layers)
				{
					layer.sortingOrder *= -1;
				}
			}

			// Token: 0x060035D2 RID: 13778 RVA: 0x001F6022 File Offset: 0x001F4422
			public ParallaxPropertiesData.ThemeProperties.Layer GetLayer(int layer)
			{
				return this.layers[layer];
			}

			// Token: 0x04003DE2 RID: 15842
			public ParallaxPropertiesData.ThemeProperties.Layer[] layers;

			// Token: 0x04003DE3 RID: 15843
			[NonSerialized]
			public bool zEditor_expanded;
		}

		// Token: 0x020008F7 RID: 2295
		[Serializable]
		public class Layer
		{
			// Token: 0x04003DE4 RID: 15844
			public float speed;

			// Token: 0x04003DE5 RID: 15845
			public int sortingOrder;
		}
	}
}
