using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x0200046C RID: 1132
public class TextMeshCurveAndJitter : MonoBehaviour
{
	// Token: 0x170002B9 RID: 697
	// (set) Token: 0x06001156 RID: 4438 RVA: 0x000A47F2 File Offset: 0x000A2BF2
	public byte AlphaValue
	{
		set
		{
			this.applyAlpha = true;
			this.alphaValue = value;
		}
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x000A4802 File Offset: 0x000A2C02
	private void Awake()
	{
		this.jitterDelay = 0.083333336f;
		this.AlphaValue = byte.MaxValue;
		this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x000A482B File Offset: 0x000A2C2B
	private void Start()
	{
		base.StartCoroutine(this.WarpText());
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x000A483C File Offset: 0x000A2C3C
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
	{
		return new AnimationCurve
		{
			keys = curve.keys
		};
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x000A485C File Offset: 0x000A2C5C
	private IEnumerator WarpText()
	{
		this.VertexCurve.preWrapMode = WrapMode.Once;
		this.VertexCurve.postWrapMode = WrapMode.Once;
		this.m_TextComponent.havePropertiesChanged = true;
		for (;;)
		{
			this.currentJitterDelay -= CupheadTime.Delta;
			if (this.currentJitterDelay <= 0f)
			{
				this.currentJitterDelay = this.jitterDelay;
			}
			this.ApplyChanges(true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x000A4878 File Offset: 0x000A2C78
	public void ApplyChanges(bool jitter)
	{
		this.m_TextComponent.ForceMeshUpdate();
		TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0 || this.m_TextComponent.text.Length == 0)
		{
			return;
		}
		float x = this.m_TextComponent.bounds.min.x;
		float x2 = this.m_TextComponent.bounds.max.x;
		for (int i = 0; i < characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				int vertexIndex = (int)textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
				if (jitter)
				{
					this.ApplyCurveAndJitter(jitter, vertices, vertexIndex, i, textInfo, x, x2);
				}
				if (this.applyAlpha)
				{
					this.ApplyAlpha(colors, vertexIndex);
				}
			}
		}
		jitter = false;
		this.m_TextComponent.UpdateVertexData();
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x000A49B8 File Offset: 0x000A2DB8
	private void ApplyCurveAndJitter(bool jitter, Vector3[] vertices, int vertexIndex, int i, TMP_TextInfo textInfo, float boundsMinX, float boundsMaxX)
	{
		Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
		float time = (vector.x - Mathf.Min(boundsMinX, -229.3845f)) / (Mathf.Max(boundsMaxX, 226.2879f) - Mathf.Min(boundsMinX, -229.3845f));
		float num = this.VertexSpacing.Evaluate(time) * this.SpacingScale;
		vertices[vertexIndex].x = vertices[vertexIndex].x + num;
		int num2 = vertexIndex + 1;
		vertices[num2].x = vertices[num2].x + num;
		int num3 = vertexIndex + 2;
		vertices[num3].x = vertices[num3].x + num;
		int num4 = vertexIndex + 3;
		vertices[num4].x = vertices[num4].x + num;
		vertices[vertexIndex] += -vector;
		vertices[vertexIndex + 1] += -vector;
		vertices[vertexIndex + 2] += -vector;
		vertices[vertexIndex + 3] += -vector;
		float num5 = (vector.x - boundsMinX) / (boundsMaxX - boundsMinX);
		float num6 = num5 + 0.0001f;
		float y = this.VertexCurve.Evaluate(num5) * this.CurveScale;
		float y2 = this.VertexCurve.Evaluate(num6) * this.CurveScale;
		Vector3 lhs = new Vector3(1f, 0f, 0f);
		Vector3 rhs = new Vector3(num6 * (boundsMaxX - boundsMinX) + boundsMinX, y2) - new Vector3(vector.x, y);
		float num7 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
		float num8 = (Vector3.Cross(lhs, rhs).z <= 0f) ? (360f - num7) : num7;
		float num9 = 0f;
		if (jitter)
		{
			num9 = UnityEngine.Random.Range(-this.jitterAngleAmplitude, this.jitterAngleAmplitude);
		}
		Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, num8 + num9), Vector3.one);
		vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
		vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
		vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
		vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
		vertices[vertexIndex] += vector;
		vertices[vertexIndex + 1] += vector;
		vertices[vertexIndex + 2] += vector;
		vertices[vertexIndex + 3] += vector;
		Vector3 zero = Vector3.zero;
		if (jitter)
		{
			zero = new Vector3(UnityEngine.Random.Range(-this.jitterAmplitude, this.jitterAmplitude), UnityEngine.Random.Range(-this.jitterAmplitude, this.jitterAmplitude), 0f);
		}
		vertices[vertexIndex] += zero;
		vertices[vertexIndex + 1] += zero;
		vertices[vertexIndex + 2] += zero;
		vertices[vertexIndex + 3] += zero;
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x000A4DC8 File Offset: 0x000A31C8
	private void ApplyAlpha(Color32[] vertices, int vertexIndex)
	{
		vertices[vertexIndex].a = this.alphaValue;
		vertices[vertexIndex + 1].a = this.alphaValue;
		vertices[vertexIndex + 2].a = this.alphaValue;
		vertices[vertexIndex + 3].a = this.alphaValue;
	}

	// Token: 0x04001AC8 RID: 6856
	private const float MAX_BOUNDS_TEXT_COMPONENT = 226.2879f;

	// Token: 0x04001AC9 RID: 6857
	private const float MIN_BOUNDS_TEXT_COMPONENT = -229.3845f;

	// Token: 0x04001ACA RID: 6858
	[SerializeField]
	private TMP_Text m_TextComponent;

	// Token: 0x04001ACB RID: 6859
	public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 2f),
		new Keyframe(0.5f, 0f),
		new Keyframe(0.75f, 2f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04001ACC RID: 6860
	public AnimationCurve VertexSpacing = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 1.5f),
		new Keyframe(0.5f, 0f),
		new Keyframe(1f, -1.5f)
	});

	// Token: 0x04001ACD RID: 6861
	public float AngleMultiplier = 1f;

	// Token: 0x04001ACE RID: 6862
	public float SpeedMultiplier = 1f;

	// Token: 0x04001ACF RID: 6863
	public float CurveScale = 1f;

	// Token: 0x04001AD0 RID: 6864
	public float SpacingScale = 1f;

	// Token: 0x04001AD1 RID: 6865
	public float jitterAmplitude = 0.1f;

	// Token: 0x04001AD2 RID: 6866
	public float jitterAngleAmplitude = 0.1f;

	// Token: 0x04001AD3 RID: 6867
	private float jitterDelay = 0.1f;

	// Token: 0x04001AD4 RID: 6868
	private float currentJitterDelay;

	// Token: 0x04001AD5 RID: 6869
	private bool applyAlpha;

	// Token: 0x04001AD6 RID: 6870
	private byte alphaValue;
}
