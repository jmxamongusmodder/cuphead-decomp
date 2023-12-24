using UnityEngine;
using TMPro;

public class TextMeshRandomAngle : MonoBehaviour
{
	[SerializeField]
	private TMP_Text m_TextComponent;
	[SerializeField]
	private TMP_Text m_ShadowTextComponent;
	public float m_AngleAmplitude;
	public float m_JitterAngleAmplitude;
	public float m_JitterOffsetAmplitude;
}
