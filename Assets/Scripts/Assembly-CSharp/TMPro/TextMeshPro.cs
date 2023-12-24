using UnityEngine;

namespace TMPro
{
	public class TextMeshPro : TMP_Text
	{
		[SerializeField]
		private float m_lineLength;
		[SerializeField]
		private TMP_Compatibility.AnchorPositions m_anchor;
		[SerializeField]
		private Vector2 m_uvOffset;
		[SerializeField]
		private float m_uvLineOffset;
		[SerializeField]
		private bool m_hasFontAssetChanged;
		[SerializeField]
		private Renderer m_renderer;
		[SerializeField]
		protected TMP_SubMesh[] m_subTextObjects;
		[SerializeField]
		private MaskingTypes m_maskType;
	}
}
