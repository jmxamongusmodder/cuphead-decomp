using System;

namespace TMPro
{
	// Token: 0x02000C94 RID: 3220
	public struct TMP_XmlTagStack<T>
	{
		// Token: 0x06005159 RID: 20825 RVA: 0x00298ED3 File Offset: 0x002972D3
		public TMP_XmlTagStack(T[] tagStack)
		{
			this.itemStack = tagStack;
			this.index = 0;
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x00298EE3 File Offset: 0x002972E3
		public void Clear()
		{
			this.index = 0;
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x00298EEC File Offset: 0x002972EC
		public void SetDefault(T item)
		{
			this.itemStack[0] = item;
			this.index = 1;
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x00298F02 File Offset: 0x00297302
		public void Add(T item)
		{
			if (this.index < this.itemStack.Length)
			{
				this.itemStack[this.index] = item;
				this.index++;
			}
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x00298F38 File Offset: 0x00297338
		public T Remove()
		{
			this.index--;
			if (this.index <= 0)
			{
				this.index = 0;
				return this.itemStack[0];
			}
			return this.itemStack[this.index - 1];
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x00298F86 File Offset: 0x00297386
		public T CurrentItem()
		{
			return this.itemStack[this.index - 1];
		}

		// Token: 0x040053F8 RID: 21496
		public T[] itemStack;

		// Token: 0x040053F9 RID: 21497
		public int index;
	}
}
