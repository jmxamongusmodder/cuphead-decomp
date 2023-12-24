using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour
{
	[Serializable]
	public class StartupPool
	{
		public int size;
		public GameObject prefab;
	}

	public enum StartupPoolMode
	{
		Awake = 0,
		Start = 1,
		CallManually = 2,
	}

	public StartupPoolMode startupPoolMode;
	public StartupPool[] startupPools;
}
