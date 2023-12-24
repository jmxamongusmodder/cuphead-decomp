using UnityEngine;
using System;

namespace Rewired.Data
{
	public class EditorPlatformData : ScriptableObject
	{
		[Serializable]
		public class Platform
		{
			public TextAsset[] libraries;
		}

		public Platform windowsStandalone;
		public Platform windowsStore;
		public Platform osxStandalone;
		public Platform linuxStandalone;
		public Platform webplayer;
		public Platform fallback;
	}
}
