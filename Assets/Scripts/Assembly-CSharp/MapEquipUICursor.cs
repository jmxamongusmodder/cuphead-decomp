using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000997 RID: 2455
public class MapEquipUICursor : AbstractMonoBehaviour
{
	// Token: 0x0600396B RID: 14699 RVA: 0x002085B8 File Offset: 0x002069B8
	public virtual void SetPosition(Vector3 position)
	{
		base.transform.position = position;
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x002085C6 File Offset: 0x002069C6
	public virtual void SelectIcon(bool onSame)
	{
		if (onSame)
		{
			base.animator.Play("Select_V2", 1);
		}
		else
		{
			base.animator.Play("Select", 1);
		}
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x002085F5 File Offset: 0x002069F5
	public virtual void OnLocked()
	{
		base.animator.Play("Locked", 1);
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x00208608 File Offset: 0x00206A08
	public virtual void Hide()
	{
		this.image.enabled = false;
	}

	// Token: 0x0600396F RID: 14703 RVA: 0x00208616 File Offset: 0x00206A16
	public virtual void Show()
	{
		this.image.enabled = true;
	}

	// Token: 0x06003970 RID: 14704 RVA: 0x00208624 File Offset: 0x00206A24
	private void HideSelectionCursor()
	{
		this.selectionCursor.enabled = false;
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x00208632 File Offset: 0x00206A32
	private void ShowSelectionCursor()
	{
		this.selectionCursor.enabled = true;
	}

	// Token: 0x04004116 RID: 16662
	[SerializeField]
	private Image selectionCursor;

	// Token: 0x04004117 RID: 16663
	[SerializeField]
	protected Image image;

	// Token: 0x04004118 RID: 16664
	public int index;
}
