using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C4A RID: 3146
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class UIImageHelper : MonoBehaviour
	{
		// Token: 0x06004D57 RID: 19799 RVA: 0x00275BFC File Offset: 0x00273FFC
		public void SetEnabledState(bool newState)
		{
			this.currentState = newState;
			UIImageHelper.State state = (!newState) ? this.disabledState : this.enabledState;
			if (state == null)
			{
				return;
			}
			Image component = base.gameObject.GetComponent<Image>();
			if (component == null)
			{
				UnityEngine.Debug.LogError("Image is missing!");
				return;
			}
			state.Set(component);
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x00275C59 File Offset: 0x00274059
		public void SetEnabledStateColor(Color color)
		{
			this.enabledState.color = color;
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x00275C67 File Offset: 0x00274067
		public void SetDisabledStateColor(Color color)
		{
			this.disabledState.color = color;
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x00275C78 File Offset: 0x00274078
		public void Refresh()
		{
			UIImageHelper.State state = (!this.currentState) ? this.disabledState : this.enabledState;
			Image component = base.gameObject.GetComponent<Image>();
			if (component == null)
			{
				return;
			}
			state.Set(component);
		}

		// Token: 0x04005191 RID: 20881
		[SerializeField]
		private UIImageHelper.State enabledState;

		// Token: 0x04005192 RID: 20882
		[SerializeField]
		private UIImageHelper.State disabledState;

		// Token: 0x04005193 RID: 20883
		private bool currentState;

		// Token: 0x02000C4B RID: 3147
		[Serializable]
		private class State
		{
			// Token: 0x06004D5C RID: 19804 RVA: 0x00275CCA File Offset: 0x002740CA
			public void Set(Image image)
			{
				if (image == null)
				{
					return;
				}
				image.color = this.color;
			}

			// Token: 0x04005194 RID: 20884
			[SerializeField]
			public Color color;
		}
	}
}
