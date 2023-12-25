using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x0200038A RID: 906
public class UITMPAnimator : AbstractMonoBehaviour
{
	// Token: 0x06000ABF RID: 2751 RVA: 0x000803E0 File Offset: 0x0007E7E0
	protected override void Awake()
	{
		base.Awake();
		this.text = base.GetComponent<TextMeshProUGUI>();
		base.StartCoroutine(this.animateCharacters_cr());
		this.ignoreGlobalTime = true;
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x00080408 File Offset: 0x0007E808
	private IEnumerator animateCharacters_cr()
	{
		this.text.havePropertiesChanged = true;
		for (;;)
		{
			this.text.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.text.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount != 0)
			{
				for (int i = 0; i < characterCount; i++)
				{
					if (textInfo.characterInfo[i].isVisible)
					{
						Vector3 b = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.25f, 0.25f), 0f);
						int vertexIndex = (int)textInfo.characterInfo[i].vertexIndex;
						int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
						Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
						vertices[vertexIndex] += b;
						vertices[vertexIndex + 1] += b;
						vertices[vertexIndex + 2] += b;
						vertices[vertexIndex + 3] += b;
					}
				}
				this.text.UpdateVertexData();
				yield return new WaitForSeconds(0.07f);
			}
		}
		yield break;
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00080424 File Offset: 0x0007E824
	private IEnumerator updateTextLikeAMoron_cr()
	{
		this.text.transform.SetScale(new float?(1f), new float?(1f), new float?(0.99f));
		yield return null;
		this.text.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		yield break;
	}

	// Token: 0x04001491 RID: 5265
	private TextMeshProUGUI text;
}
