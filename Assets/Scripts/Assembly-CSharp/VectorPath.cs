using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000381 RID: 897
[Serializable]
public class VectorPath
{
	// Token: 0x06000A90 RID: 2704 RVA: 0x0007F074 File Offset: 0x0007D474
	public static Vector3 Lerp(VectorPath path, float t)
	{
		if (path == null || path._points.Count < 1)
		{
			return Vector3.zero;
		}
		if (path._points.Count == 1)
		{
			return path._points[0];
		}
		if (path._points.Count == 2)
		{
			return Vector3.Lerp(path._points[0], path._points[1], t);
		}
		Vector3 vector = default(Vector3);
		int num = 0;
		if (path.Distance < 0f)
		{
			path.Calculate();
		}
		for (int i = 0; i < path.infoNodes.Count - 1; i++)
		{
			num = i;
			if (path.infoNodes[i + 1].distance > t)
			{
				break;
			}
		}
		Vector3 vector2 = path.infoNodes[num];
		Vector3 vector3 = path.infoNodes[num + 1];
		float distance = path.infoNodes[num].distance;
		float distance2 = path.infoNodes[num + 1].distance;
		float t2 = (t - distance) / (distance2 - distance);
		return Vector3.Lerp(path.infoNodes[num], path.infoNodes[num + 1], t2);
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0007F1DE File Offset: 0x0007D5DE
	public List<Vector3> Points
	{
		get
		{
			return this._points;
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0007F1E6 File Offset: 0x0007D5E6
	// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0007F1EE File Offset: 0x0007D5EE
	public bool Closed
	{
		get
		{
			return this._closed;
		}
		set
		{
			this._closed = value;
			this.Calculate();
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0007F1FD File Offset: 0x0007D5FD
	public float Distance
	{
		get
		{
			if (this._distance < 0f)
			{
				this.Calculate();
			}
			return this._distance;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0007F21B File Offset: 0x0007D61B
	public List<VectorPath.Node> infoNodes
	{
		get
		{
			if (this.__infoNodes == null)
			{
				this.Calculate();
			}
			return this.__infoNodes;
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0007F234 File Offset: 0x0007D634
	private void Calculate()
	{
		this.__infoNodes = VectorPath.Node.NewList(this._points);
		if (this._closed)
		{
			this.infoNodes.Add(new VectorPath.Node(this._points[0]));
		}
		this._distance = 0f;
		for (int i = 1; i < this.infoNodes.Count; i++)
		{
			this._distance += Vector3.Distance(this.infoNodes[i - 1], this.infoNodes[i]);
		}
		float num = 0f;
		for (int j = 1; j < this.infoNodes.Count; j++)
		{
			num += Vector3.Distance(this.infoNodes[j - 1], this.infoNodes[j]);
			VectorPath.Node value = this.infoNodes[j];
			value.distance = num / this._distance;
			this.infoNodes[j] = value;
		}
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0007F34D File Offset: 0x0007D74D
	public Vector3 Lerp(float t)
	{
		return VectorPath.Lerp(this, t);
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0007F358 File Offset: 0x0007D758
	public Vector2 GetClosestPoint(Vector2 originalPosition, Vector2 positionToCheck, bool moveX, bool moveY)
	{
		Vector2 result = originalPosition;
		float num = float.MaxValue;
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		Vector2 vector3 = Vector2.zero;
		Vector2 vector4 = Vector2.zero;
		Vector2 a = Vector2.zero;
		for (int i = 0; i < this.Points.Count - 1; i++)
		{
			vector2 = this.Points[i];
			vector3 = this.Points[i + 1];
			vector4 = positionToCheck - vector2;
			a = vector3 - vector2;
			if (moveX)
			{
				float num2 = vector4.x / a.x;
				if (num2 < 0f)
				{
					vector = vector2;
				}
				else if (num2 > 1f)
				{
					vector = vector3;
				}
				else
				{
					vector = vector2 + a * num2;
				}
				float num3 = Vector2.Distance(positionToCheck, vector);
				if (num3 <= num)
				{
					num = num3;
					result = vector;
				}
			}
			if (moveY)
			{
				float num2 = vector4.y / a.y;
				if (num2 < 0f)
				{
					vector = vector2;
				}
				else if (num2 > 1f)
				{
					vector = vector3;
				}
				else
				{
					vector = vector2 + a * num2;
				}
				float num3 = Vector2.Distance(positionToCheck, vector);
				if (num3 <= num)
				{
					num = num3;
					result = vector;
				}
			}
		}
		return result;
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0007F4C8 File Offset: 0x0007D8C8
	public float GetClosestNormalizedPoint(Vector2 originalPosition, Vector2 positionToCheck, bool moveX, bool moveY)
	{
		Vector2 b = originalPosition;
		float num = float.MaxValue;
		Vector2 vector = Vector2.zero;
		VectorPath.Node node = Vector2.zero;
		VectorPath.Node node2 = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		Vector2 a = Vector2.zero;
		VectorPath.Node node3 = Vector2.zero;
		VectorPath.Node node4 = Vector2.zero;
		for (int i = 0; i < this.Points.Count - 1; i++)
		{
			node = this.infoNodes[i];
			node2 = this.infoNodes[i + 1];
			vector2 = positionToCheck - node.position;
			a = node2.position - node.position;
			if (moveX)
			{
				float num2 = vector2.x / a.x;
				if (num2 < 0f)
				{
					vector = node;
				}
				else if (num2 > 1f)
				{
					vector = node2;
				}
				else
				{
					vector = node + a * num2;
				}
				float num3 = Vector2.Distance(positionToCheck, vector);
				if (num3 <= num)
				{
					num = num3;
					b = vector;
					node3 = node;
					node4 = node2;
				}
			}
			if (moveY)
			{
				float num2 = vector2.y / a.y;
				if (num2 < 0f)
				{
					vector = node;
				}
				else if (num2 > 1f)
				{
					vector = node2;
				}
				else
				{
					vector = node + a * num2;
				}
				float num3 = Vector2.Distance(positionToCheck, vector);
				if (num3 <= num)
				{
					num = num3;
					b = vector;
					node3 = node;
					node4 = node2;
				}
			}
		}
		float num4 = Vector2.Distance(node3.position, node4.position);
		float num5 = Vector2.Distance(node3.position, b);
		return Mathf.Lerp(node3.distance, node4.distance, num5 / num4);
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0007F6E8 File Offset: 0x0007DAE8
	public void DrawGizmos(Vector3 offset)
	{
		this.DrawGizmos(1f, offset);
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0007F6F8 File Offset: 0x0007DAF8
	public void DrawGizmos(float a, Vector3 offset)
	{
		for (int i = 0; i < this._points.Count; i++)
		{
			Gizmos.color = new Color(0f, 0f, 1f, a);
			Gizmos.DrawWireSphere(this._points[i] + offset, 10f);
			if (i < this._points.Count - 1)
			{
				Gizmos.color = new Color(0f, 0f, 1f, a);
				Vector3 vector = this._points[i] + offset;
				Vector3 vector2 = this._points[i + 1] + offset;
				Gizmos.DrawLine(vector, vector2);
				Vector3 a2 = Vector3.Lerp(vector, vector2, 0.45f);
				Vector3 to = Vector3.Lerp(vector, vector2, 0.55f);
				Vector3 b = Quaternion.Euler(0f, 0f, 90f) * (vector2 - vector).normalized * 10f;
				Gizmos.color = new Color(0f, 1f, 0f, a);
				Gizmos.DrawLine(a2 + b, to);
				Gizmos.DrawLine(a2 - b, to);
			}
		}
		if (this.Closed)
		{
			Gizmos.color = new Color(0f, 1f, 0f, a * 0.5f);
			Gizmos.DrawLine(this._points[this._points.Count - 1] + offset, this._points[0] + offset);
		}
	}

	// Token: 0x04001476 RID: 5238
	[SerializeField]
	private List<Vector3> _points = new List<Vector3>
	{
		new Vector2(-100f, 0f),
		new Vector2(100f, 0f)
	};

	// Token: 0x04001477 RID: 5239
	[SerializeField]
	private bool _closed;

	// Token: 0x04001478 RID: 5240
	private float _distance = -1f;

	// Token: 0x04001479 RID: 5241
	private List<VectorPath.Node> __infoNodes;

	// Token: 0x02000382 RID: 898
	public struct Node
	{
		// Token: 0x06000A9C RID: 2716 RVA: 0x0007F89B File Offset: 0x0007DC9B
		public Node(Vector3 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
			this.distance = 0f;
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0007F8CF File Offset: 0x0007DCCF
		public Vector3 position
		{
			get
			{
				return new Vector3(this.x, this.y, this.z);
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0007F8E8 File Offset: 0x0007DCE8
		public static List<VectorPath.Node> NewList(List<Vector3> oldList)
		{
			List<VectorPath.Node> list = new List<VectorPath.Node>(oldList.Count);
			for (int i = 0; i < oldList.Count; i++)
			{
				list.Add(new VectorPath.Node(oldList[i]));
			}
			return list;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0007F92B File Offset: 0x0007DD2B
		public static implicit operator VectorPath.Node(Vector2 v)
		{
			return new VectorPath.Node(v);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0007F938 File Offset: 0x0007DD38
		public static implicit operator Vector2(VectorPath.Node t)
		{
			return new Vector2(t.x, t.y);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0007F94D File Offset: 0x0007DD4D
		public static implicit operator VectorPath.Node(Vector3 v)
		{
			return new VectorPath.Node(v);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0007F955 File Offset: 0x0007DD55
		public static implicit operator Vector3(VectorPath.Node t)
		{
			return new Vector3(t.x, t.y, t.z);
		}

		// Token: 0x0400147A RID: 5242
		public float x;

		// Token: 0x0400147B RID: 5243
		public float y;

		// Token: 0x0400147C RID: 5244
		public float z;

		// Token: 0x0400147D RID: 5245
		public float distance;
	}
}
