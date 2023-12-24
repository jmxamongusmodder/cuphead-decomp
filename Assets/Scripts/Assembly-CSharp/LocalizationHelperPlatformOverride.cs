using UnityEngine;
using System;

public class LocalizationHelperPlatformOverride : MonoBehaviour
{
	[Serializable]
	public class OverrideInfo
	{
		public RuntimePlatform platform;
		public int id;
	}

	public OverrideInfo[] overrides;
}
