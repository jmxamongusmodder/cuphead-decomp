using System;
using System.Collections;
using System.Collections.Generic;
using GCFreeUtils;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
public static class CupheadTime
{
	// Token: 0x06000D9F RID: 3487 RVA: 0x0008EC0C File Offset: 0x0008D00C
	static CupheadTime()
	{
		CupheadTime.delta = new CupheadTime.DeltaObject();
		CupheadTime.layers = new Dictionary<int, float>();
		CupheadTime.Layer[] values = EnumUtils.GetValues<CupheadTime.Layer>();
		foreach (CupheadTime.Layer key in values)
		{
			CupheadTime.layers.Add((int)key, 1f);
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0008EC77 File Offset: 0x0008D077
	public static CupheadTime.DeltaObject Delta
	{
		get
		{
			return CupheadTime.delta;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0008EC7E File Offset: 0x0008D07E
	public static float GlobalDelta
	{
		get
		{
			return Time.deltaTime;
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0008EC85 File Offset: 0x0008D085
	public static float FixedDelta
	{
		get
		{
			return Time.fixedDeltaTime * CupheadTime.GlobalSpeed;
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0008EC92 File Offset: 0x0008D092
	// (set) Token: 0x06000DA4 RID: 3492 RVA: 0x0008EC99 File Offset: 0x0008D099
	public static float GlobalSpeed
	{
		get
		{
			return CupheadTime.globalSpeed;
		}
		set
		{
			CupheadTime.globalSpeed = Mathf.Clamp(value, 0f, 1f);
			CupheadTime.OnChanged();
		}
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x0008ECB5 File Offset: 0x0008D0B5
	public static float GetLayerSpeed(CupheadTime.Layer layer)
	{
		return CupheadTime.layers[(int)layer];
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x0008ECC2 File Offset: 0x0008D0C2
	public static void SetLayerSpeed(CupheadTime.Layer layer, float value)
	{
		CupheadTime.layers[(int)layer] = value;
		CupheadTime.OnChanged();
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x0008ECD5 File Offset: 0x0008D0D5
	public static void Reset()
	{
		CupheadTime.SetAll(1f);
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x0008ECE4 File Offset: 0x0008D0E4
	public static void SetAll(float value)
	{
		CupheadTime.GlobalSpeed = value;
		foreach (CupheadTime.Layer key in EnumUtils.GetValues<CupheadTime.Layer>())
		{
			CupheadTime.layers[(int)key] = value;
		}
		CupheadTime.OnChanged();
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x0008ED26 File Offset: 0x0008D126
	private static void OnChanged()
	{
		if (CupheadTime.OnChangedEvent != null)
		{
			CupheadTime.OnChangedEvent.Call();
		}
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x0008ED3C File Offset: 0x0008D13C
	public static bool IsPaused()
	{
		return CupheadTime.GlobalSpeed <= 1E-05f || PauseManager.state == PauseManager.State.Paused;
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x0008ED58 File Offset: 0x0008D158
	public static Coroutine WaitForSeconds(MonoBehaviour m, float time)
	{
		return m.StartCoroutine(CupheadTime.waitForSeconds_cr(time, CupheadTime.Layer.Default));
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x0008ED67 File Offset: 0x0008D167
	public static Coroutine WaitForSeconds(MonoBehaviour m, float time, CupheadTime.Layer layer)
	{
		return m.StartCoroutine(CupheadTime.waitForSeconds_cr(time, layer));
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x0008ED78 File Offset: 0x0008D178
	private static IEnumerator waitForSeconds_cr(float time, CupheadTime.Layer layer)
	{
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta[layer];
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x0008ED9A File Offset: 0x0008D19A
	public static Coroutine WaitForUnpause(MonoBehaviour m)
	{
		return m.StartCoroutine(CupheadTime.waitForUnpause_cr());
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x0008EDA8 File Offset: 0x0008D1A8
	private static IEnumerator waitForUnpause_cr()
	{
		while (CupheadTime.GlobalSpeed == 0f)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400171B RID: 5915
	private static readonly CupheadTime.DeltaObject delta;

	// Token: 0x0400171C RID: 5916
	private static float globalSpeed = 1f;

	// Token: 0x0400171D RID: 5917
	private static Dictionary<int, float> layers;

	// Token: 0x0400171E RID: 5918
	public static GCFreeActionList OnChangedEvent = new GCFreeActionList(200, true);

	// Token: 0x020003F1 RID: 1009
	public enum Layer
	{
		// Token: 0x04001720 RID: 5920
		Default,
		// Token: 0x04001721 RID: 5921
		Player,
		// Token: 0x04001722 RID: 5922
		Enemy,
		// Token: 0x04001723 RID: 5923
		UI
	}

	// Token: 0x020003F2 RID: 1010
	public class DeltaObject
	{
		// Token: 0x1700024B RID: 587
		public float this[CupheadTime.Layer layer]
		{
			get
			{
				return Time.deltaTime * CupheadTime.GetLayerSpeed(layer) * CupheadTime.GlobalSpeed;
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0008EDD8 File Offset: 0x0008D1D8
		public static implicit operator float(CupheadTime.DeltaObject d)
		{
			return d[CupheadTime.Layer.Default] * CupheadTime.GlobalSpeed;
		}
	}
}
