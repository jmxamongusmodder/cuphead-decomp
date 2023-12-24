using System;
using UnityEngine;

public class SpriteErrorManager : AbstractMonoBehaviour
{
	[Serializable]
	public class Pair
	{
		public Sprite sprite;
		public int chance;
		public string name;
	}

	[SerializeField]
	private Pair[] errors;
}
