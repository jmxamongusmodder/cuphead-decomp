using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000C62 RID: 3170
	public class FastAction<A, B>
	{
		// Token: 0x06004EDC RID: 20188 RVA: 0x0027AD49 File Offset: 0x00279149
		public void Add(Action<A, B> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x0027AD78 File Offset: 0x00279178
		public void Remove(Action<A, B> rhs)
		{
			LinkedListNode<Action<A, B>> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x0027ADB4 File Offset: 0x002791B4
		public void Call(A a, B b)
		{
			for (LinkedListNode<Action<A, B>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a, b);
			}
		}

		// Token: 0x04005202 RID: 20994
		private LinkedList<Action<A, B>> delegates = new LinkedList<Action<A, B>>();

		// Token: 0x04005203 RID: 20995
		private Dictionary<Action<A, B>, LinkedListNode<Action<A, B>>> lookup = new Dictionary<Action<A, B>, LinkedListNode<Action<A, B>>>();
	}
}
