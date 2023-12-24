using UnityEngine;
using TMPro;

public class TextMeshCurveAndJitter : MonoBehaviour
{
	[SerializeField]
	private TMP_Text m_TextComponent;
	public AnimationCurve VertexCurve;
	public AnimationCurve VertexSpacing;
	public float AngleMultiplier;
	public float SpeedMultiplier;
	public float CurveScale;
	public float SpacingScale;
	public float jitterAmplitude;
	public float jitterAngleAmplitude;
}
