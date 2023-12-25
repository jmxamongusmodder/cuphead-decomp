using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace TMPro
{
	// Token: 0x02000C76 RID: 3190
	internal class TMP_ObjectPool<T> where T : new()
	{
		// Token: 0x06004FE9 RID: 20457 RVA: 0x0029513F File Offset: 0x0029353F
		public TMP_ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
		{
			this.m_ActionOnGet = actionOnGet;
			this.m_ActionOnRelease = actionOnRelease;
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06004FEA RID: 20458 RVA: 0x00295160 File Offset: 0x00293560
		// (set) Token: 0x06004FEB RID: 20459 RVA: 0x00295168 File Offset: 0x00293568
		public int countAll { get; private set; }

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x00295171 File Offset: 0x00293571
		public int countActive
		{
			get
			{
				return this.countAll - this.countInactive;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06004FED RID: 20461 RVA: 0x00295180 File Offset: 0x00293580
		public int countInactive
		{
			get
			{
				return this.m_Stack.Count;
			}
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x00295190 File Offset: 0x00293590
		public T Get()
		{
			T t;
			if (this.m_Stack.Count == 0)
			{
				t = Activator.CreateInstance<T>();
				this.countAll++;
			}
			else
			{
				t = this.m_Stack.Pop();
			}
			if (this.m_ActionOnGet != null)
			{
				this.m_ActionOnGet(t);
			}
			return t;
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x002951EC File Offset: 0x002935EC
		public void Release(T element)
		{
			if (this.m_Stack.Count > 0 && object.ReferenceEquals(this.m_Stack.Peek(), element))
			{
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.", null);
			}
			if (this.m_ActionOnRelease != null)
			{
				this.m_ActionOnRelease(element);
			}
			this.m_Stack.Push(element);
		}

		// Token: 0x0400529D RID: 21149
		private readonly Stack<T> m_Stack = new Stack<T>();

		// Token: 0x0400529E RID: 21150
		private readonly UnityAction<T> m_ActionOnGet;

		// Token: 0x0400529F RID: 21151
		private readonly UnityAction<T> m_ActionOnRelease;
	}
}
