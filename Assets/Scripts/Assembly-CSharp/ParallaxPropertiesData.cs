using UnityEngine;
using System;
using System.Collections.Generic;

public class ParallaxPropertiesData : ScriptableObject
{
	[Serializable]
	public class ThemeProperties
	{
		[Serializable]
		public class Layer
		{
			public float speed;
			public int sortingOrder;
		}

		[Serializable]
		public class LayerGroup
		{
			public ParallaxPropertiesData.ThemeProperties.Layer[] layers;
		}

		public PlatformingLevel.Theme theme;
		public LayerGroup background;
		public LayerGroup foreground;
	}

	[SerializeField]
	private List<ParallaxPropertiesData.ThemeProperties> _properties;
}
