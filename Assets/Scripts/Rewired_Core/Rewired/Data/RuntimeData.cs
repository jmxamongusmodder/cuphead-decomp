using UnityEngine;
using Rewired.Platforms;
using System.Collections.Generic;

namespace Rewired.Data
{
	public class RuntimeData : ScriptableObject
	{
		public Platform platform;
		public WebplayerPlatform webplayerPlatform;
		public EditorPlatform editorPlatform;
		public List<TextAsset> libraries;
	}
}
