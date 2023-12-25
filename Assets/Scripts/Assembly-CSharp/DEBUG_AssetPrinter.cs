using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x02000B37 RID: 2871
public class DEBUG_AssetPrinter : MonoBehaviour
{
	// Token: 0x0600459C RID: 17820 RVA: 0x0024B902 File Offset: 0x00249D02
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600459D RID: 17821 RVA: 0x0024B910 File Offset: 0x00249D10
	private void OnGUI()
	{
		GUIStyle guistyle = new GUIStyle(GUI.skin.GetStyle("Box"));
		guistyle.alignment = TextAnchor.UpperLeft;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Load Operations: " + AssetBundleLoader.loadCounter);
		IList list = AssetBundleLoader.DEBUG_LoadedAssetBundles();
		stringBuilder.AppendFormat("=== AssetBundles ({0}) ===\n", list.Count);
		IEnumerator enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string value = (string)obj;
				stringBuilder.AppendLine(value);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		IEnumerable<AssetBundle> allLoadedAssetBundles = AssetBundle.GetAllLoadedAssetBundles();
		int num = 0;
		foreach (AssetBundle assetBundle in AssetBundle.GetAllLoadedAssetBundles())
		{
			num++;
		}
		stringBuilder.AppendFormat("=== System AssetBundles ({0}) ===\n", num);
		foreach (AssetBundle assetBundle2 in AssetBundle.GetAllLoadedAssetBundles())
		{
			stringBuilder.AppendLine(assetBundle2.name);
		}
		GUI.Box(new Rect(0f, 0f, 400f, (float)Screen.height), stringBuilder.ToString());
		stringBuilder.Length = 0;
		list = AssetLoader<SpriteAtlas>.DEBUG_GetLoadedAssets();
		stringBuilder.AppendFormat("=== Cached SpriteAtlases ({0}) ===\n", list.Count);
		IEnumerator enumerator4 = list.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				object obj2 = enumerator4.Current;
				string value2 = (string)obj2;
				stringBuilder.AppendLine(value2);
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator4 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		list = AssetLoader<AudioClip>.DEBUG_GetLoadedAssets();
		stringBuilder.AppendFormat("=== Cached Music ({0}) ===\n", list.Count);
		IEnumerator enumerator5 = list.GetEnumerator();
		try
		{
			while (enumerator5.MoveNext())
			{
				object obj3 = enumerator5.Current;
				string value3 = (string)obj3;
				stringBuilder.AppendLine(value3);
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator5 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		list = AssetLoader<Texture2D[]>.DEBUG_GetLoadedAssets();
		stringBuilder.AppendFormat("=== Cached Textures ({0}) ===\n", list.Count);
		IEnumerator enumerator6 = list.GetEnumerator();
		try
		{
			while (enumerator6.MoveNext())
			{
				object obj4 = enumerator6.Current;
				string value4 = (string)obj4;
				stringBuilder.AppendLine(value4);
			}
		}
		finally
		{
			IDisposable disposable4;
			if ((disposable4 = (enumerator6 as IDisposable)) != null)
			{
				disposable4.Dispose();
			}
		}
		GUI.Box(new Rect(400f, 0f, 400f, (float)Screen.height), stringBuilder.ToString());
		stringBuilder.Length = 0;
		list = Resources.FindObjectsOfTypeAll<SpriteAtlas>();
		stringBuilder.AppendFormat("=== System SpriteAtlases ({0}) ===\n", list.Count);
		IEnumerator enumerator7 = list.GetEnumerator();
		try
		{
			while (enumerator7.MoveNext())
			{
				object obj5 = enumerator7.Current;
				stringBuilder.AppendLine(((SpriteAtlas)obj5).name);
			}
		}
		finally
		{
			IDisposable disposable5;
			if ((disposable5 = (enumerator7 as IDisposable)) != null)
			{
				disposable5.Dispose();
			}
		}
		GUI.Box(new Rect(800f, 0f, 400f, (float)Screen.height), stringBuilder.ToString());
	}
}
