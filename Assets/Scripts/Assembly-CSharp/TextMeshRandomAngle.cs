using System;
using TMPro;
using UnityEngine;

// Token: 0x0200046D RID: 1133
public class TextMeshRandomAngle : MonoBehaviour
{
	// Token: 0x0600115F RID: 4447 RVA: 0x000A4F68 File Offset: 0x000A3368
	private void Start()
	{
		this.initialAngles = new float[this.m_TextComponent.text.Length];
		this.jitterAngles = new float[this.m_TextComponent.text.Length];
		for (int i = 0; i < this.initialAngles.Length; i++)
		{
			this.initialAngles[i] = UnityEngine.Random.Range(-this.m_AngleAmplitude, this.m_AngleAmplitude);
		}
		this.jitterDelay = 0.083333336f;
		this.ApplyRotation();
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x000A4FF0 File Offset: 0x000A33F0
	private void Update()
	{
		this.currentJitterDelay -= CupheadTime.Delta;
		if (this.currentJitterDelay > 0f)
		{
			return;
		}
		this.currentJitterDelay = this.jitterDelay;
		for (int i = 0; i < this.initialAngles.Length; i++)
		{
			this.jitterAngles[i] = UnityEngine.Random.Range(-this.m_JitterAngleAmplitude, this.m_JitterAngleAmplitude);
		}
		this.ApplyRotation();
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x000A506C File Offset: 0x000A346C
	private void ApplyRotation()
	{
		this.m_TextComponent.havePropertiesChanged = true;
		this.m_ShadowTextComponent.havePropertiesChanged = true;
		this.m_TextComponent.ForceMeshUpdate();
		this.m_ShadowTextComponent.ForceMeshUpdate();
		TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0 || this.m_TextComponent.text.Length == 0)
		{
			return;
		}
		for (int i = 0; i < characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				int vertexIndex = (int)textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, (vertices[vertexIndex].y + vertices[vertexIndex + 2].y) / 2f);
				vertices[vertexIndex] += -vector;
				vertices[vertexIndex + 1] += -vector;
				vertices[vertexIndex + 2] += -vector;
				vertices[vertexIndex + 3] += -vector;
				float z = this.initialAngles[i] + this.jitterAngles[i];
				Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
				vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
				vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
				vector += new Vector3(UnityEngine.Random.Range(-this.m_JitterOffsetAmplitude, this.m_JitterOffsetAmplitude), UnityEngine.Random.Range(-this.m_JitterOffsetAmplitude, this.m_JitterOffsetAmplitude), 0f);
				vertices[vertexIndex] += vector;
				vertices[vertexIndex + 1] += vector;
				vertices[vertexIndex + 2] += vector;
				vertices[vertexIndex + 3] += vector;
				this.m_ShadowTextComponent.textInfo.meshInfo[materialReferenceIndex].vertices = vertices;
			}
		}
		this.m_TextComponent.UpdateVertexData();
		this.m_ShadowTextComponent.UpdateVertexData();
	}

	// Token: 0x04001AD7 RID: 6871
	[SerializeField]
	private TMP_Text m_TextComponent;

	// Token: 0x04001AD8 RID: 6872
	[SerializeField]
	private TMP_Text m_ShadowTextComponent;

	// Token: 0x04001AD9 RID: 6873
	public float m_AngleAmplitude = 5f;

	// Token: 0x04001ADA RID: 6874
	public float m_JitterAngleAmplitude = 0.7f;

	// Token: 0x04001ADB RID: 6875
	public float m_JitterOffsetAmplitude = 0.1f;

	// Token: 0x04001ADC RID: 6876
	private float[] initialAngles;

	// Token: 0x04001ADD RID: 6877
	private float[] jitterAngles;

	// Token: 0x04001ADE RID: 6878
	private float jitterDelay = 0.1f;

	// Token: 0x04001ADF RID: 6879
	private float currentJitterDelay;
}
