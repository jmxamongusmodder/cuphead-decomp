using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000C60 RID: 3168
	public class FastAction
	{
		// Token: 0x06004ED4 RID: 20180 RVA: 0x0027ABCE File Offset: 0x00278FCE
		public void Add(Action rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x0027ABFC File Offset: 0x00278FFC
		public void Remove(Action rhs)
		{
			LinkedListNode<Action> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x0027AC38 File Offset: 0x00279038
		public void Call()
		{
			for (LinkedListNode<Action> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value();
			}
		}

		// Token: 0x040051FE RID: 20990
		private LinkedList<Action> delegates = new LinkedList<Action>();

		// Token: 0x040051FF RID: 20991
		private Dictionary<Action, LinkedListNode<Action>> lookup = new Dictionary<Action, LinkedListNode<Action>>();
	}
}
