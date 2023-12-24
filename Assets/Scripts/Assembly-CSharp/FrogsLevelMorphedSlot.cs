using System;
using UnityEngine;

public class FrogsLevelMorphedSlot : AbstractPausableComponent
{
	[Serializable]
	public class Textures
	{
		public Texture2D[] normal;
		public Texture2D[] flashing;
	}

	[SerializeField]
	private Textures textures;
}
