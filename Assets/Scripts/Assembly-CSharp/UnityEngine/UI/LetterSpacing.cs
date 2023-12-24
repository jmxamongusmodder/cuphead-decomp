using UnityEngine;

namespace UnityEngine.UI
{
	public class LetterSpacing : BaseMeshEffect
	{
		public override void ModifyMesh(VertexHelper vh)
		{
		}

		[SerializeField]
		private bool useRichText;
		[SerializeField]
		private float m_spacing;
	}
}
