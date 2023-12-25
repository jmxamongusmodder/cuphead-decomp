using System;

namespace GCFreeUtils
{
	// Token: 0x02000B27 RID: 2855
	public class GCFreePredicateList<T>
	{
		// Token: 0x0600452F RID: 17711 RVA: 0x00247838 File Offset: 0x00245C38
		public GCFreePredicateList(int size) : this(size, true)
		{
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x00247842 File Offset: 0x00245C42
		public GCFreePredicateList(int size, bool autoResizeable)
		{
			this.actionList = new Predicate<T>[size];
			this.autoResizeable = autoResizeable;
			this.Count = 0;
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06004531 RID: 17713 RVA: 0x00247864 File Offset: 0x00245C64
		// (set) Token: 0x06004532 RID: 17714 RVA: 0x0024786C File Offset: 0x00245C6C
		public int Count { get; private set; }

		// Token: 0x06004533 RID: 17715 RVA: 0x00247878 File Offset: 0x00245C78
		public void Add(Predicate<T> action)
		{
			if (this.Count == this.actionList.Length)
			{
				if (!this.autoResizeable)
				{
					Debug.LogError("[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.", null);
					return;
				}
				Predicate<T>[] destinationArray = new Predicate<T>[this.actionList.Length * 2];
				Array.Copy(this.actionList, destinationArray, this.actionList.Length);
				this.actionList = destinationArray;
			}
			this.actionList[this.Count] = action;
			this.Count++;
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x002478FC File Offset: 0x00245CFC
		public void Remove(Predicate<T> action)
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

		// Token: 0x06004535 RID: 17717 RVA: 0x00247984 File Offset: 0x00245D84
		public bool CallAnyTrue(T parameter)
		{
			for (int i = 0; i < this.Count; i++)
			{
				try
				{
					if (this.actionList[i] != null)
					{
						bool flag = this.actionList[i](parameter);
						if (flag)
						{
							return true;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogError(message, null);
				}
			}
			return false;
		}

		// Token: 0x04004AE3 RID: 19171
		private const string ERR_BUFFER_TOO_SMALL = "[GCFreeActionList] Current buffer too small. Consider increasing the initial size or set as auto resizeable.";

		// Token: 0x04004AE4 RID: 19172
		private const string LOG_RESIZING = "[GCFreeActionList] Resizing buffer. Maybe you want to increase the initial size.";

		// Token: 0x04004AE6 RID: 19174
		private Predicate<T>[] actionList;

		// Token: 0x04004AE7 RID: 19175
		private bool autoResizeable;
	}
}
