using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlowText : MonoBehaviour
{
	[SerializeField]
	private RenderTexture renderTextureGlow;
	[SerializeField]
	private GameObject cameraGlow;
	[SerializeField]
	private RawImage rawImageGlow;
	[SerializeField]
	private TextMeshProUGUI[] tmpTextsToGlow;
	[SerializeField]
	private Image[] imagesToGlow;
}
