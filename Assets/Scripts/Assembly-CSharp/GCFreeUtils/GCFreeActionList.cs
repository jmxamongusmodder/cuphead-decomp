using System;

namespace GCFreeUtils
{
	// Token: 0x02000B26 RID: 2854
	public class GCFreeActionList
	{
		// Token: 0x06004528 RID: 17704 RVA: 0x0024768F File Offset: 0x00245A8F
		public GCFreeActionList(int size) : this(size, true)
		{
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x00247699 File Offset: 0x00245A99
		public GCFreeActionList(int size, bool autoResizeable)
		{
			this.actionList = new Action[size];
			this.autoResizeable = autoResizeable;
			this.Count = 0;
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600452A RID: 17706 RVA: 0x002476BB File Offset: 0x00245ABB
		// (set) Token: 0x0600452B RID: 17707 RVA: 0x002476C3 File Offset: 0x00245AC3
		public int Count { get; private set; }

		// Token: 0x0600452C RID: 17708 RVA: 0x002476CC File Offset: 0x00245ACC
		public void Add(Action action)
		{
			if (this.Count == this.actionList.Length)
			{
				if (!this.autoResizeable)
				{
					Debug.LogError("[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.", null);
					return;
				}
				Action[] destinationArray = new Action[this.actionList.Length * 2];
				Array.Copy(this.actionList, destinationArray, this.actionList.Length);
				this.actionList = destinationArray;
			}
			this.actionList[this.Count] = action;
			this.Count++;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x00247750 File Offset: 0x00245B50
		public void Remove(Action action)
		{
			if (this.Count > 0)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.actionList[i] == action)
					{
						if (this.Count > 1)
						{
							this.actionList[i] = this.actionList[this.Count - 1];
						}
						else
						{
							this.actionList[i] = null;
						}
						this.Count--;
						break;
					}
				}
			}
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x002477D8 File Offset: 0x00245BD8
		public void Call()
		{
			for (int i = 0; i < this.Count; i++)
			{
				try
				{
					if (this.actionList[i] != null)
					{
						this.actionList[i]();
					}
				}
				catch (Exception message)
				{
					Debug.LogError(message, null);
				}
			}
		}

		// Token: 0x04004ADE RID: 19166
		private const string ERR_BUFFER_TOO_SMALL = "[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.";

		// Token: 0x04004ADF RID: 19167
		private const string LOG_RESIZING = "[GCFreeActionList] Resizing buffer. Maybe you want to increase the initial size.";

		// Token: 0x04004AE1 RID: 19169
		private Action[] actionList;

		// Token: 0x04004AE2 RID: 19170
		private bool autoResizeable;
	}
}
