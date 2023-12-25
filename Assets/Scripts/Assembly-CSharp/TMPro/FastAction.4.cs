using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000C63 RID: 3171
	public class FastAction<A, B, C>
	{
		// Token: 0x06004EE0 RID: 20192 RVA: 0x0027AE0A File Offset: 0x0027920A
		public void Add(Action<A, B, C> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x0027AE38 File Offset: 0x00279238
		public void Remove(Action<A, B, C> rhs)
		{
			LinkedListNode<Action<A, B, C>> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x0027AE74 File Offset: 0x00279274
		public void Call(A a, B b, C c)
		{
			for (LinkedListNode<Action<A, B, C>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a, b, c);
			}
		}

		// Token: 0x04005204 RID: 20996
		private LinkedList<Action<A, B, C>> delegates = new LinkedList<Action<A, B, C>>();

		// Token: 0x04005205 RID: 20997
		private Dictionary<Action<A, B, C>, LinkedListNode<Action<A, B, C>>> lookup = new Dictionary<Action<A, B, C>, LinkedListNode<Action<A, B, C>>>();
	}
}
