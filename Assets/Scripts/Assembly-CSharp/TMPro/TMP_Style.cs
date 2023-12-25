using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C7A RID: 3194
	[Serializable]
	public class TMP_Style
	{
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06005007 RID: 20487 RVA: 0x0029547B File Offset: 0x0029387B
		// (set) Token: 0x06005008 RID: 20488 RVA: 0x00295483 File Offset: 0x00293883
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				if (value != this.m_Name)
				{
					this.m_Name = value;
				}
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06005009 RID: 20489 RVA: 0x0029549D File Offset: 0x0029389D
		// (set) Token: 0x0600500A RID: 20490 RVA: 0x002954A5 File Offset: 0x002938A5
		public int hashCode
		{
			get
			{
				return this.m_HashCode;
			}
			set
			{
				if (value != this.m_HashCode)
				{
					this.m_HashCode = value;
				}
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x0600500B RID: 20491 RVA: 0x002954BA File Offset: 0x002938BA
		public string styleOpeningDefinition
		{
			get
			{
				return this.m_OpeningDefinition;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x0600500C RID: 20492 RVA: 0x002954C2 File Offset: 0x002938C2
		public string styleClosingDefinition
		{
			get
			{
				return this.m_ClosingDefinition;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x0600500D RID: 20493 RVA: 0x002954CA File Offset: 0x002938CA
		public int[] styleOpeningTagArray
		{
			get
			{
				return this.m_OpeningTagArray;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x0600500E RID: 20494 RVA: 0x002954D2 File Offset: 0x002938D2
		public int[] styleClosingTagArray
		{
			get
			{
				return this.m_ClosingTagArray;
			}
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x002954DC File Offset: 0x002938DC
		public void RefreshStyle()
		{
			this.m_HashCode = TMP_TextUtilities.GetSimpleHashCode(this.m_Name);
			this.m_OpeningTagArray = new int[this.m_OpeningDefinition.Length];
			for (int i = 0; i < this.m_OpeningDefinition.Length; i++)
			{
				this.m_OpeningTagArray[i] = (int)this.m_OpeningDefinition[i];
			}
			this.m_ClosingTagArray = new int[this.m_ClosingDefinition.Length];
			for (int j = 0; j < this.m_ClosingDefinition.Length; j++)
			{
				this.m_ClosingTagArray[j] = (int)this.m_ClosingDefinition[j];
			}
			TMPro_EventManager.ON_TEXT_STYLE_PROPERTY_CHANGED(true);
		}

		// Token: 0x040052B4 RID: 21172
		[SerializeField]
		private string m_Name;

		// Token: 0x040052B5 RID: 21173
		[SerializeField]
		private int m_HashCode;

		// Token: 0x040052B6 RID: 21174
		[SerializeField]
		private string m_OpeningDefinition;

		// Token: 0x040052B7 RID: 21175
		[SerializeField]
		private string m_ClosingDefinition;

		// Token: 0x040052B8 RID: 21176
		[SerializeField]
		private int[] m_OpeningTagArray;

		// Token: 0x040052B9 RID: 21177
		[SerializeField]
		private int[] m_ClosingTagArray;
	}
}
