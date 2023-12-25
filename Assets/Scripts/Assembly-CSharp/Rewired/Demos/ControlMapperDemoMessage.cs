using System;
using System.Collections;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000C05 RID: 3077
	[AddComponentMenu("")]
	public class ControlMapperDemoMessage : MonoBehaviour
	{
		// Token: 0x0600496A RID: 18794 RVA: 0x00265B64 File Offset: 0x00263F64
		private void Awake()
		{
			if (this.controlMapper != null)
			{
				this.controlMapper.ScreenClosedEvent += this.OnControlMapperClosed;
				this.controlMapper.ScreenOpenedEvent += this.OnControlMapperOpened;
			}
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x00265BB0 File Offset: 0x00263FB0
		private void Start()
		{
			this.SelectDefault();
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x00265BB8 File Offset: 0x00263FB8
		private void OnControlMapperClosed()
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.SelectDefaultDeferred());
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x00265BD3 File Offset: 0x00263FD3
		private void OnControlMapperOpened()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x00265BE1 File Offset: 0x00263FE1
		private void SelectDefault()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (this.defaultSelectable != null)
			{
				EventSystem.current.SetSelectedGameObject(this.defaultSelectable.gameObject);
			}
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x00265C1C File Offset: 0x0026401C
		private IEnumerator SelectDefaultDeferred()
		{
			yield return null;
			this.SelectDefault();
			yield break;
		}

		// Token: 0x04004F81 RID: 20353
		public ControlMapper controlMapper;

		// Token: 0x04004F82 RID: 20354
		public Selectable defaultSelectable;
	}
}
