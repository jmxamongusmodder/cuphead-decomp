using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000C61 RID: 3169
	public class FastAction<A>
	{
		// Token: 0x06004ED8 RID: 20184 RVA: 0x0027AC8C File Offset: 0x0027908C
		public void Add(Action<A> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x0027ACB8 File Offset: 0x002790B8
		public void Remove(Action<A> rhs)
		{
			LinkedListNode<Action<A>> node;
			if (this.lookup.TryGetValue(rhs, out node))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(node);
			}
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x0027ACF4 File Offset: 0x002790F4
		public void Call(A a)
		{
			for (LinkedListNode<Action<A>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a);
			}
		}

		// Token: 0x04005200 RID: 20992
		private LinkedList<Action<A>> delegates = new LinkedList<Action<A>>();

		// Token: 0x04005201 RID: 20993
		private Dictionary<Action<A>, LinkedListNode<Action<A>>> lookup = new Dictionary<Action<A>, LinkedListNode<Action<A>>>();
	}
}
