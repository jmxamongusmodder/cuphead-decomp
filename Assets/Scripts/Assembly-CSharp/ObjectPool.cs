using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public sealed class ObjectPool : MonoBehaviour
{
	// Token: 0x06001006 RID: 4102 RVA: 0x0009E6F0 File Offset: 0x0009CAF0
	private void Awake()
	{
		ObjectPool._instance = this;
		if (this.startupPoolMode == ObjectPool.StartupPoolMode.Awake)
		{
			ObjectPool.CreateStartupPools();
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x0009E708 File Offset: 0x0009CB08
	private void Start()
	{
		if (this.startupPoolMode == ObjectPool.StartupPoolMode.Start)
		{
			ObjectPool.CreateStartupPools();
		}
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0009E71C File Offset: 0x0009CB1C
	private void OnDestroy()
	{
		foreach (KeyValuePair<GameObject, List<GameObject>> keyValuePair in this.pooledObjects)
		{
			ObjectPool.DestroyAll(keyValuePair.Key);
		}
		this.spawnedObjects.Clear();
		this.pooledObjects.Clear();
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x0009E794 File Offset: 0x0009CB94
	public static void CreateStartupPools()
	{
		if (!ObjectPool.instance.startupPoolsCreated)
		{
			ObjectPool.instance.startupPoolsCreated = true;
			ObjectPool.StartupPool[] array = ObjectPool.instance.startupPools;
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					ObjectPool.CreatePool(array[i].prefab, array[i].size);
				}
			}
		}
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0009E7FE File Offset: 0x0009CBFE
	public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
	{
		ObjectPool.CreatePool(prefab.gameObject, initialPoolSize);
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0009E814 File Offset: 0x0009CC14
	public static void CreatePool(GameObject prefab, int initialPoolSize)
	{
		if (prefab != null && !ObjectPool.instance.pooledObjects.ContainsKey(prefab))
		{
			List<GameObject> list = new List<GameObject>();
			ObjectPool.instance.pooledObjects.Add(prefab, list);
			if (initialPoolSize > 0)
			{
				bool activeSelf = prefab.activeSelf;
				prefab.SetActive(true);
				Transform transform = ObjectPool.instance.transform;
				while (list.Count < initialPoolSize)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
					gameObject.SetActive(false);
					gameObject.transform.parent = transform;
					list.Add(gameObject);
				}
				prefab.SetActive(activeSelf);
			}
		}
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0009E8B2 File Offset: 0x0009CCB2
	public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0009E8CE File Offset: 0x0009CCCE
	public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0009E8EA File Offset: 0x0009CCEA
	public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0009E90A File Offset: 0x0009CD0A
	public static T Spawn<T>(T prefab, Vector3 position) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0009E92A File Offset: 0x0009CD2A
	public static T Spawn<T>(T prefab, Transform parent) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0009E94E File Offset: 0x0009CD4E
	public static T Spawn<T>(T prefab) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x0009E974 File Offset: 0x0009CD74
	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		List<GameObject> list;
		GameObject gameObject;
		Transform transform;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			gameObject = null;
			if (list.Count > 0)
			{
				while (gameObject == null && list.Count > 0)
				{
					gameObject = list[0];
					list.RemoveAt(0);
				}
				if (gameObject != null)
				{
					transform = gameObject.transform;
					transform.parent = parent;
					transform.localPosition = position;
					transform.localRotation = rotation;
					gameObject.SetActive(true);
					ObjectPool.instance.spawnedObjects.Add(gameObject, prefab);
					return gameObject;
				}
			}
			gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			transform = gameObject.transform;
			transform.parent = parent;
			transform.localPosition = position;
			transform.localRotation = rotation;
			ObjectPool.instance.spawnedObjects.Add(gameObject, prefab);
			return gameObject;
		}
		gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		transform = gameObject.GetComponent<Transform>();
		transform.parent = parent;
		transform.localPosition = position;
		transform.localRotation = rotation;
		return gameObject;
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x0009EA6E File Offset: 0x0009CE6E
	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
	{
		return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x0009EA7D File Offset: 0x0009CE7D
	public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return ObjectPool.Spawn(prefab, null, position, rotation);
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0009EA88 File Offset: 0x0009CE88
	public static GameObject Spawn(GameObject prefab, Transform parent)
	{
		return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0009EA9B File Offset: 0x0009CE9B
	public static GameObject Spawn(GameObject prefab, Vector3 position)
	{
		return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0009EAAA File Offset: 0x0009CEAA
	public static GameObject Spawn(GameObject prefab)
	{
		return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x0009EABD File Offset: 0x0009CEBD
	public static void Recycle<T>(T obj) where T : Component
	{
		ObjectPool.Recycle(obj.gameObject);
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x0009EAD4 File Offset: 0x0009CED4
	public static void Recycle(GameObject obj)
	{
		GameObject prefab;
		if (ObjectPool.instance.spawnedObjects.TryGetValue(obj, out prefab))
		{
			ObjectPool.Recycle(obj, prefab);
		}
		else
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x0009EB0C File Offset: 0x0009CF0C
	private static void Recycle(GameObject obj, GameObject prefab)
	{
		ObjectPool.instance.pooledObjects[prefab].Add(obj);
		ObjectPool.instance.spawnedObjects.Remove(obj);
		if (obj != null)
		{
			obj.transform.parent = ObjectPool.instance.transform;
			obj.SetActive(false);
		}
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0009EB68 File Offset: 0x0009CF68
	public static void RecycleAll<T>(T prefab) where T : Component
	{
		ObjectPool.RecycleAll(prefab.gameObject);
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0009EB7C File Offset: 0x0009CF7C
	public static void RecycleAll(GameObject prefab)
	{
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectPool.instance.spawnedObjects)
		{
			if (keyValuePair.Value == prefab)
			{
				ObjectPool.tempList.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < ObjectPool.tempList.Count; i++)
		{
			ObjectPool.Recycle(ObjectPool.tempList[i]);
		}
		ObjectPool.tempList.Clear();
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0009EC30 File Offset: 0x0009D030
	public static void RecycleAll()
	{
		ObjectPool.tempList.AddRange(ObjectPool.instance.spawnedObjects.Keys);
		for (int i = 0; i < ObjectPool.tempList.Count; i++)
		{
			ObjectPool.Recycle(ObjectPool.tempList[i]);
		}
		ObjectPool.tempList.Clear();
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0009EC8B File Offset: 0x0009D08B
	public static bool IsSpawned(GameObject obj)
	{
		return ObjectPool.instance.spawnedObjects.ContainsKey(obj);
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0009EC9D File Offset: 0x0009D09D
	public static int CountPooled<T>(T prefab) where T : Component
	{
		return ObjectPool.CountPooled(prefab.gameObject);
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0009ECB4 File Offset: 0x0009D0B4
	public static int CountPooled(GameObject prefab)
	{
		List<GameObject> list;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			return list.Count;
		}
		return 0;
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0009ECE0 File Offset: 0x0009D0E0
	public static int CountSpawned<T>(T prefab) where T : Component
	{
		return ObjectPool.CountSpawned(prefab.gameObject);
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x0009ECF4 File Offset: 0x0009D0F4
	public static int CountSpawned(GameObject prefab)
	{
		int num = 0;
		foreach (GameObject y in ObjectPool.instance.spawnedObjects.Values)
		{
			if (prefab == y)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0009ED68 File Offset: 0x0009D168
	public static int CountAllPooled()
	{
		int num = 0;
		foreach (List<GameObject> list in ObjectPool.instance.pooledObjects.Values)
		{
			num += list.Count;
		}
		return num;
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0009EDD4 File Offset: 0x0009D1D4
	public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		List<GameObject> collection;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out collection))
		{
			list.AddRange(collection);
		}
		return list;
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x0009EE1C File Offset: 0x0009D21C
	public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		List<GameObject> list2;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab.gameObject, out list2))
		{
			for (int i = 0; i < list2.Count; i++)
			{
				list.Add(list2[i].GetComponent<T>());
			}
		}
		return list;
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x0009EE90 File Offset: 0x0009D290
	public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectPool.instance.spawnedObjects)
		{
			if (keyValuePair.Value == prefab)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0009EF24 File Offset: 0x0009D324
	public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		GameObject gameObject = prefab.gameObject;
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectPool.instance.spawnedObjects)
		{
			if (keyValuePair.Value == gameObject)
			{
				list.Add(keyValuePair.Key.GetComponent<T>());
			}
		}
		return list;
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0009EFCC File Offset: 0x0009D3CC
	public static void DestroyPooled(GameObject prefab)
	{
		List<GameObject> list;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				UnityEngine.Object.Destroy(list[i]);
			}
			list.Clear();
		}
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0009F019 File Offset: 0x0009D419
	public static void DestroyPooled<T>(T prefab) where T : Component
	{
		ObjectPool.DestroyPooled(prefab.gameObject);
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x0009F02D File Offset: 0x0009D42D
	public static void DestroyAll(GameObject prefab)
	{
		ObjectPool.RecycleAll(prefab);
		ObjectPool.DestroyPooled(prefab);
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0009F03B File Offset: 0x0009D43B
	public static void DestroyAll<T>(T prefab) where T : Component
	{
		ObjectPool.DestroyAll(prefab.gameObject);
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x0600102C RID: 4140 RVA: 0x0009F050 File Offset: 0x0009D450
	public static ObjectPool instance
	{
		get
		{
			if (ObjectPool._instance != null)
			{
				return ObjectPool._instance;
			}
			ObjectPool._instance = UnityEngine.Object.FindObjectOfType<ObjectPool>();
			if (ObjectPool._instance != null)
			{
				return ObjectPool._instance;
			}
			ObjectPool._instance = new GameObject("ObjectPool")
			{
				transform = 
				{
					localPosition = Vector3.zero,
					localRotation = Quaternion.identity,
					localScale = Vector3.one
				}
			}.AddComponent<ObjectPool>();
			return ObjectPool._instance;
		}
	}

	// Token: 0x0400199E RID: 6558
	private static ObjectPool _instance;

	// Token: 0x0400199F RID: 6559
	private static List<GameObject> tempList = new List<GameObject>();

	// Token: 0x040019A0 RID: 6560
	private Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x040019A1 RID: 6561
	private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

	// Token: 0x040019A2 RID: 6562
	public ObjectPool.StartupPoolMode startupPoolMode;

	// Token: 0x040019A3 RID: 6563
	public ObjectPool.StartupPool[] startupPools;

	// Token: 0x040019A4 RID: 6564
	private bool startupPoolsCreated;

	// Token: 0x02000444 RID: 1092
	public enum StartupPoolMode
	{
		// Token: 0x040019A6 RID: 6566
		Awake,
		// Token: 0x040019A7 RID: 6567
		Start,
		// Token: 0x040019A8 RID: 6568
		CallManually
	}

	// Token: 0x02000445 RID: 1093
	[Serializable]
	public class StartupPool
	{
		// Token: 0x040019A9 RID: 6569
		public int size;

		// Token: 0x040019AA RID: 6570
		public GameObject prefab;
	}
}
