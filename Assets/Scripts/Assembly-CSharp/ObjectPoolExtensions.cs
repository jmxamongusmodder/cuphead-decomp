using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public static class ObjectPoolExtensions
{
	// Token: 0x0600102F RID: 4143 RVA: 0x0009F0F2 File Offset: 0x0009D4F2
	public static void CreatePool<T>(this T prefab) where T : Component
	{
		ObjectPool.CreatePool<T>(prefab, 0);
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0009F0FB File Offset: 0x0009D4FB
	public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
	{
		ObjectPool.CreatePool<T>(prefab, initialPoolSize);
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0009F104 File Offset: 0x0009D504
	public static void CreatePool(this GameObject prefab)
	{
		ObjectPool.CreatePool(prefab, 0);
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0009F10D File Offset: 0x0009D50D
	public static void CreatePool(this GameObject prefab, int initialPoolSize)
	{
		ObjectPool.CreatePool(prefab, initialPoolSize);
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0009F116 File Offset: 0x0009D516
	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, parent, position, rotation);
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0009F121 File Offset: 0x0009D521
	public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, null, position, rotation);
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0009F12C File Offset: 0x0009D52C
	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, parent, position, Quaternion.identity);
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0009F13B File Offset: 0x0009D53B
	public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, null, position, Quaternion.identity);
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0009F14A File Offset: 0x0009D54A
	public static T Spawn<T>(this T prefab, Transform parent) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0009F15D File Offset: 0x0009D55D
	public static T Spawn<T>(this T prefab) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, null, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0009F170 File Offset: 0x0009D570
	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		return ObjectPool.Spawn(prefab, parent, position, rotation);
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x0009F17B File Offset: 0x0009D57B
	public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return ObjectPool.Spawn(prefab, null, position, rotation);
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0009F186 File Offset: 0x0009D586
	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
	{
		return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x0009F195 File Offset: 0x0009D595
	public static GameObject Spawn(this GameObject prefab, Vector3 position)
	{
		return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x0009F1A4 File Offset: 0x0009D5A4
	public static GameObject Spawn(this GameObject prefab, Transform parent)
	{
		return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x0009F1B7 File Offset: 0x0009D5B7
	public static GameObject Spawn(this GameObject prefab)
	{
		return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x0009F1CA File Offset: 0x0009D5CA
	public static void Recycle<T>(this T obj) where T : Component
	{
		ObjectPool.Recycle<T>(obj);
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x0009F1D2 File Offset: 0x0009D5D2
	public static void Recycle(this GameObject obj)
	{
		ObjectPool.Recycle(obj);
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x0009F1DA File Offset: 0x0009D5DA
	public static void RecycleAll<T>(this T prefab) where T : Component
	{
		ObjectPool.RecycleAll<T>(prefab);
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0009F1E2 File Offset: 0x0009D5E2
	public static void RecycleAll(this GameObject prefab)
	{
		ObjectPool.RecycleAll(prefab);
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x0009F1EA File Offset: 0x0009D5EA
	public static int CountPooled<T>(this T prefab) where T : Component
	{
		return ObjectPool.CountPooled<T>(prefab);
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0009F1F2 File Offset: 0x0009D5F2
	public static int CountPooled(this GameObject prefab)
	{
		return ObjectPool.CountPooled(prefab);
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x0009F1FA File Offset: 0x0009D5FA
	public static int CountSpawned<T>(this T prefab) where T : Component
	{
		return ObjectPool.CountSpawned<T>(prefab);
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x0009F202 File Offset: 0x0009D602
	public static int CountSpawned(this GameObject prefab)
	{
		return ObjectPool.CountSpawned(prefab);
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x0009F20A File Offset: 0x0009D60A
	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return ObjectPool.GetSpawned(prefab, list, appendList);
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x0009F214 File Offset: 0x0009D614
	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
	{
		return ObjectPool.GetSpawned(prefab, list, false);
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x0009F21E File Offset: 0x0009D61E
	public static List<GameObject> GetSpawned(this GameObject prefab)
	{
		return ObjectPool.GetSpawned(prefab, null, false);
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x0009F228 File Offset: 0x0009D628
	public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return ObjectPool.GetSpawned<T>(prefab, list, appendList);
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x0009F232 File Offset: 0x0009D632
	public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
	{
		return ObjectPool.GetSpawned<T>(prefab, list, false);
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x0009F23C File Offset: 0x0009D63C
	public static List<T> GetSpawned<T>(this T prefab) where T : Component
	{
		return ObjectPool.GetSpawned<T>(prefab, null, false);
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x0009F246 File Offset: 0x0009D646
	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return ObjectPool.GetPooled(prefab, list, appendList);
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0009F250 File Offset: 0x0009D650
	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
	{
		return ObjectPool.GetPooled(prefab, list, false);
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0009F25A File Offset: 0x0009D65A
	public static List<GameObject> GetPooled(this GameObject prefab)
	{
		return ObjectPool.GetPooled(prefab, null, false);
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0009F264 File Offset: 0x0009D664
	public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return ObjectPool.GetPooled<T>(prefab, list, appendList);
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0009F26E File Offset: 0x0009D66E
	public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
	{
		return ObjectPool.GetPooled<T>(prefab, list, false);
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x0009F278 File Offset: 0x0009D678
	public static List<T> GetPooled<T>(this T prefab) where T : Component
	{
		return ObjectPool.GetPooled<T>(prefab, null, false);
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x0009F282 File Offset: 0x0009D682
	public static void DestroyPooled(this GameObject prefab)
	{
		ObjectPool.DestroyPooled(prefab);
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x0009F28A File Offset: 0x0009D68A
	public static void DestroyPooled<T>(this T prefab) where T : Component
	{
		ObjectPool.DestroyPooled(prefab.gameObject);
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x0009F29E File Offset: 0x0009D69E
	public static void DestroyAll(this GameObject prefab)
	{
		ObjectPool.DestroyAll(prefab);
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x0009F2A6 File Offset: 0x0009D6A6
	public static void DestroyAll<T>(this T prefab) where T : Component
	{
		ObjectPool.DestroyAll(prefab.gameObject);
	}
}
