using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000370 RID: 880
public static class TransformExtensions
{
	// Token: 0x06000A11 RID: 2577 RVA: 0x0007DE72 File Offset: 0x0007C272
	public static void ResetScale(this Transform transform)
	{
		transform.localScale = Vector3.one;
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0007DE7F File Offset: 0x0007C27F
	public static void ResetPosition(this Transform transform)
	{
		transform.position = Vector3.zero;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0007DE8C File Offset: 0x0007C28C
	public static void ResetLocalPosition(this Transform transform)
	{
		transform.localPosition = Vector3.zero;
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0007DE99 File Offset: 0x0007C299
	public static void ResetRotation(this Transform transform)
	{
		transform.eulerAngles = Vector3.zero;
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x0007DEA6 File Offset: 0x0007C2A6
	public static void ResetLocalRotation(this Transform transform)
	{
		transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0007DEB3 File Offset: 0x0007C2B3
	public static void ResetTransforms(this Transform transform)
	{
		transform.ResetPosition();
		transform.ResetRotation();
		transform.ResetScale();
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0007DEC7 File Offset: 0x0007C2C7
	public static void ResetLocalTransforms(this Transform transform)
	{
		transform.ResetLocalPosition();
		transform.ResetLocalRotation();
		transform.ResetScale();
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0007DEDC File Offset: 0x0007C2DC
	public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 position = transform.position;
		if (x != null)
		{
			position.x = x.Value;
		}
		if (y != null)
		{
			position.y = y.Value;
		}
		if (z != null)
		{
			position.z = z.Value;
		}
		transform.position = position;
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0007DF48 File Offset: 0x0007C348
	public static void SetLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 localPosition = transform.localPosition;
		if (x != null)
		{
			localPosition.x = x.Value;
		}
		if (y != null)
		{
			localPosition.y = y.Value;
		}
		if (z != null)
		{
			localPosition.z = z.Value;
		}
		transform.localPosition = localPosition;
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0007DFB4 File Offset: 0x0007C3B4
	public static void SetEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 eulerAngles = transform.eulerAngles;
		if (x != null)
		{
			eulerAngles.x = x.Value;
		}
		if (y != null)
		{
			eulerAngles.y = y.Value;
		}
		if (z != null)
		{
			eulerAngles.z = z.Value;
		}
		transform.eulerAngles = eulerAngles;
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0007E020 File Offset: 0x0007C420
	public static void SetLocalEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		if (x != null)
		{
			localEulerAngles.x = x.Value;
		}
		if (y != null)
		{
			localEulerAngles.y = y.Value;
		}
		if (z != null)
		{
			localEulerAngles.z = z.Value;
		}
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0007E08C File Offset: 0x0007C48C
	public static void SetScale(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 localScale = transform.localScale;
		if (x != null)
		{
			localScale.x = x.Value;
		}
		if (y != null)
		{
			localScale.y = y.Value;
		}
		if (z != null)
		{
			localScale.z = z.Value;
		}
		transform.localScale = localScale;
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0007E0F8 File Offset: 0x0007C4F8
	public static void AddPosition(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 position = transform.position;
		position.x += x;
		position.y += y;
		position.z += z;
		transform.position = position;
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0007E140 File Offset: 0x0007C540
	public static void AddLocalPosition(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.x += x;
		localPosition.y += y;
		localPosition.z += z;
		transform.localPosition = localPosition;
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0007E188 File Offset: 0x0007C588
	public static void AddPositionForward2D(this Transform transform, float forward)
	{
		transform.position += transform.right * forward;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0007E1A8 File Offset: 0x0007C5A8
	public static void AddEulerAngles(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 eulerAngles = transform.eulerAngles;
		eulerAngles.x += x;
		eulerAngles.y += y;
		eulerAngles.z += z;
		transform.eulerAngles = eulerAngles;
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0007E1F0 File Offset: 0x0007C5F0
	public static void AddLocalEulerAngles(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.x += x;
		localEulerAngles.y += y;
		localEulerAngles.z += z;
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0007E238 File Offset: 0x0007C638
	public static void AddScale(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 localScale = transform.localScale;
		localScale.x += x;
		localScale.y += y;
		localScale.z += z;
		transform.localScale = localScale;
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0007E280 File Offset: 0x0007C680
	public static void MoveForward(this Transform transform, float amount)
	{
		transform.position += transform.forward * amount;
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0007E29F File Offset: 0x0007C69F
	public static void MoveForward2D(this Transform transform, float amount)
	{
		transform.position += transform.right * amount;
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0007E2BE File Offset: 0x0007C6BE
	public static void LookAt2D(this Transform transform, Transform target)
	{
		transform.LookAt2D(target.position);
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0007E2CC File Offset: 0x0007C6CC
	public static void LookAt2D(this Transform transform, Vector3 target)
	{
		Vector3 vector = target - transform.position;
		vector.Normalize();
		transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f);
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0007E31C File Offset: 0x0007C71C
	public static Transform[] GetChildTransforms(this Transform transform)
	{
		List<Transform> list = new List<Transform>();
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform item = (Transform)obj;
				list.Add(item);
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
		list.Remove(transform);
		return list.ToArray();
	}
}
