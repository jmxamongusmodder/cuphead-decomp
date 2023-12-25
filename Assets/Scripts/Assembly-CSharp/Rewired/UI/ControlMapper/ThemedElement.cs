using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C38 RID: 3128
	[AddComponentMenu("")]
	public class ThemedElement : MonoBehaviour
	{
		// Token: 0x06004CE7 RID: 19687 RVA: 0x00274B78 File Offset: 0x00272F78
		private void Start()
		{
			this.ApplyTheme();
			ControlMapper.OnPlayerChange += this.ApplyTheme;
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x00274B91 File Offset: 0x00272F91
		private void OnDestroy()
		{
			ControlMapper.OnPlayerChange -= this.ApplyTheme;
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x00274BA4 File Offset: 0x00272FA4
		private void OnEnable()
		{
			ControlMapper.ApplyTheme(this._elements);
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x00274BB1 File Offset: 0x00272FB1
		private void ApplyTheme()
		{
			ControlMapper.ApplyTheme(this._elements);
		}

		// Token: 0x04005137 RID: 20791
		[SerializeField]
		private ThemedElement.ElementInfo[] _elements;

		// Token: 0x02000C39 RID: 3129
		[Serializable]
		public class ElementInfo
		{
			// Token: 0x17000795 RID: 1941
			// (get) Token: 0x06004CEC RID: 19692 RVA: 0x00274BC6 File Offset: 0x00272FC6
			public string themeClass
			{
				get
				{
					return this._themeClass;
				}
			}

			// Token: 0x17000796 RID: 1942
			// (get) Token: 0x06004CED RID: 19693 RVA: 0x00274BCE File Offset: 0x00272FCE
			public Component component
			{
				get
				{
					return this._component;
				}
			}

			// Token: 0x04005138 RID: 20792
			[SerializeField]
			private string _themeClass;

			// Token: 0x04005139 RID: 20793
			[SerializeField]
			private Component _component;
		}
	}
}
