using System;
using UnityEngine;

// Token: 0x020008FF RID: 2303
public class PlatformingLevelEditorPlatform : AbstractMonoBehaviour
{
	// Token: 0x0600360B RID: 13835 RVA: 0x001F6514 File Offset: 0x001F4914
	protected override void Awake()
	{
		base.Awake();
		PlatformingLevelEditorPlatform.Type type = this._type;
		if (type != PlatformingLevelEditorPlatform.Type.Platform)
		{
			if (type == PlatformingLevelEditorPlatform.Type.Solid)
			{
				GameObject gameObject = new GameObject("ground");
				GameObject gameObject2 = new GameObject("walls");
				GameObject gameObject3 = new GameObject("ceiling");
				gameObject.layer = 20;
				gameObject.tag = "Ground";
				gameObject2.layer = 18;
				gameObject2.tag = "Wall";
				gameObject3.layer = 19;
				gameObject3.tag = "Ceiling";
				gameObject.transform.SetParent(base.transform);
				gameObject2.transform.SetParent(base.transform);
				gameObject3.transform.SetParent(base.transform);
				gameObject.transform.ResetLocalTransforms();
				gameObject2.transform.ResetLocalTransforms();
				gameObject3.transform.ResetLocalTransforms();
				this._topCollider = gameObject.AddComponent<BoxCollider2D>();
				this._middleCollider = gameObject2.AddComponent<BoxCollider2D>();
				this._bottomCollider = gameObject3.AddComponent<BoxCollider2D>();
				this._topCollider.isTrigger = true;
				this._middleCollider.isTrigger = true;
				this._bottomCollider.isTrigger = true;
				this._topCollider.size = new Vector2(this._size.x, 20f);
				this._middleCollider.size = this._size - new Vector2(0f, 40f);
				this._bottomCollider.size = new Vector2(this._size.x, 20f);
				this._topCollider.offset = new Vector2(0f, this._size.y / 2f - this._topCollider.size.y / 2f) + this._offset;
				this._middleCollider.offset = Vector2.zero + this._offset;
				this._bottomCollider.offset = new Vector2(0f, -(this._size.y / 2f - this._bottomCollider.size.y / 2f)) + this._offset;
			}
		}
		else
		{
			this._collider = base.gameObject.AddComponent<BoxCollider2D>();
			this._collider.size = this._size;
			this._collider.offset = this._offset;
			this._collider.isTrigger = true;
			this._platform = base.gameObject.AddComponent<LevelPlatform>();
			this._platform.canFallThrough = this._canFallThrough;
		}
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x001F67BA File Offset: 0x001F4BBA
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.5f);
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x001F67CD File Offset: 0x001F4BCD
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x001F67E0 File Offset: 0x001F4BE0
	private void DrawGizmos(float a)
	{
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Vector2 v = Vector2.zero + this._offset;
		Gizmos.color = new Color(0f, 0f, 0f, 0.4f * a);
		Gizmos.DrawCube(v, this._size);
		Gizmos.color = Color.cyan * new Color(1f, 1f, 1f, a);
		PlatformingLevelEditorPlatform.Type type = this._type;
		if (type != PlatformingLevelEditorPlatform.Type.Platform)
		{
			if (type == PlatformingLevelEditorPlatform.Type.Solid)
			{
				Gizmos.DrawWireCube(v, this._size);
			}
		}
		else
		{
			float num = v.y + this._size.y / 2f;
			float num2 = v.y - this._size.y / 2f;
			float y = num - 10f;
			float x = v.x - this._size.x / 2f;
			float x2 = v.x + this._size.x / 2f;
			Gizmos.DrawLine(new Vector2(x, num), new Vector2(x2, num));
			if (!this._canFallThrough)
			{
				Gizmos.DrawLine(new Vector2(x, y), new Vector2(x2, y));
			}
			else
			{
				Gizmos.DrawLine(new Vector2(v.x, num2 + 50f), new Vector2(v.x, num2));
				Gizmos.DrawLine(new Vector2(v.x - 20f, num2 + 20f), new Vector2(v.x, num2));
				Gizmos.DrawLine(new Vector2(v.x + 20f, num2 + 20f), new Vector2(v.x, num2));
			}
		}
		Gizmos.matrix = matrix;
	}

	// Token: 0x04003E0F RID: 15887
	private const int THICKNESS = 20;

	// Token: 0x04003E10 RID: 15888
	[SerializeField]
	private PlatformingLevelEditorPlatform.Type _type;

	// Token: 0x04003E11 RID: 15889
	[SerializeField]
	private bool _canFallThrough;

	// Token: 0x04003E12 RID: 15890
	[SerializeField]
	private Vector2 _size = new Vector2(100f, 10f);

	// Token: 0x04003E13 RID: 15891
	[SerializeField]
	private Vector2 _offset = new Vector2(0f, 0f);

	// Token: 0x04003E14 RID: 15892
	private LevelPlatform _platform;

	// Token: 0x04003E15 RID: 15893
	private BoxCollider2D _collider;

	// Token: 0x04003E16 RID: 15894
	private BoxCollider2D _topCollider;

	// Token: 0x04003E17 RID: 15895
	private BoxCollider2D _middleCollider;

	// Token: 0x04003E18 RID: 15896
	private BoxCollider2D _bottomCollider;

	// Token: 0x02000900 RID: 2304
	public enum Type
	{
		// Token: 0x04003E1A RID: 15898
		Platform,
		// Token: 0x04003E1B RID: 15899
		Solid
	}
}
