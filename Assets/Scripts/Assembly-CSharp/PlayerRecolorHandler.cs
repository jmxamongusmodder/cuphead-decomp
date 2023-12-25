using System;
using UnityEngine;

// Token: 0x02000AD2 RID: 2770
public class PlayerRecolorHandler : MonoBehaviour
{
	// Token: 0x06004288 RID: 17032 RVA: 0x0023DD4C File Offset: 0x0023C14C
	private void OnEnable()
	{
		EventManager.Instance.AddListener<ChaliceRecolorEvent>(new EventManager.EventDelegate<ChaliceRecolorEvent>(this.chaliceRecolorEventHandler));
	}

	// Token: 0x06004289 RID: 17033 RVA: 0x0023DD64 File Offset: 0x0023C164
	private void OnDisable()
	{
		EventManager.Instance.RemoveListener<ChaliceRecolorEvent>(new EventManager.EventDelegate<ChaliceRecolorEvent>(this.chaliceRecolorEventHandler));
	}

	// Token: 0x0600428A RID: 17034 RVA: 0x0023DD7C File Offset: 0x0023C17C
	private void chaliceRecolorEventHandler(ChaliceRecolorEvent e)
	{
		PlayerRecolorHandler.SetChaliceRecolorEnabled(this.chaliceRenderer.sharedMaterial, e.enabled);
	}

	// Token: 0x0600428B RID: 17035 RVA: 0x0023DD94 File Offset: 0x0023C194
	public static void SetChaliceRecolorEnabled(Material sharedMaterial, bool enabled)
	{
		sharedMaterial.SetFloat("_RecolorFactor", (float)((!enabled) ? 0 : 1));
	}

	// Token: 0x040048F3 RID: 18675
	[SerializeField]
	private SpriteRenderer chaliceRenderer;
}
