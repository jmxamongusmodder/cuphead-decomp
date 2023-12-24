using System;
using UnityEngine;

public class AudioWarble : AbstractPausableComponent
{
	[Serializable]
	public class WarbleAttributes
	{
		public float minVal;
		public float maxVal;
		public float warbleTime;
		public float playTime;
	}

	[SerializeField]
	private WarbleAttributes[] warbles;
}
