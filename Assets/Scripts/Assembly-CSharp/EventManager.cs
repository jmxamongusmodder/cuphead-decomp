using System;
using System.Collections.Generic;

// Token: 0x02000363 RID: 867
public class EventManager
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x0600099D RID: 2461 RVA: 0x0007C19F File Offset: 0x0007A59F
	public static EventManager Instance
	{
		get
		{
			if (EventManager._instance == null)
			{
				EventManager._instance = new EventManager();
			}
			return EventManager._instance;
		}
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x0007C1BC File Offset: 0x0007A5BC
	public void AddListener<T>(EventManager.EventDelegate<T> del) where T : GameEvent
	{
		if (this.delegateLookup.ContainsKey(del))
		{
			return;
		}
		EventManager.EventDelegate eventDelegate = delegate(GameEvent e)
		{
			del((T)((object)e));
		};
		this.delegateLookup[del] = eventDelegate;
		EventManager.EventDelegate a;
		if (this.delegates.TryGetValue(typeof(T), out a))
		{
			a = (this.delegates[typeof(T)] = (EventManager.EventDelegate)Delegate.Combine(a, eventDelegate));
		}
		else
		{
			this.delegates[typeof(T)] = eventDelegate;
		}
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x0007C268 File Offset: 0x0007A668
	public void RemoveListener<T>(EventManager.EventDelegate<T> del) where T : GameEvent
	{
		EventManager.EventDelegate value;
		if (this.delegateLookup.TryGetValue(del, out value))
		{
			EventManager.EventDelegate eventDelegate;
			if (this.delegates.TryGetValue(typeof(T), out eventDelegate))
			{
				eventDelegate = (EventManager.EventDelegate)Delegate.Remove(eventDelegate, value);
				if (eventDelegate == null)
				{
					this.delegates.Remove(typeof(T));
				}
				else
				{
					this.delegates[typeof(T)] = eventDelegate;
				}
			}
			this.delegateLookup.Remove(del);
		}
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x0007C2F8 File Offset: 0x0007A6F8
	public void Raise(GameEvent e)
	{
		EventManager.EventDelegate eventDelegate;
		if (this.delegates.TryGetValue(e.GetType(), out eventDelegate))
		{
			eventDelegate(e);
		}
	}

	// Token: 0x04001444 RID: 5188
	private static EventManager _instance;

	// Token: 0x04001445 RID: 5189
	private Dictionary<Type, EventManager.EventDelegate> delegates = new Dictionary<Type, EventManager.EventDelegate>();

	// Token: 0x04001446 RID: 5190
	private Dictionary<Delegate, EventManager.EventDelegate> delegateLookup = new Dictionary<Delegate, EventManager.EventDelegate>();

	// Token: 0x02000364 RID: 868
	// (Invoke) Token: 0x060009A3 RID: 2467
	public delegate void EventDelegate<T>(T e) where T : GameEvent;

	// Token: 0x02000365 RID: 869
	// (Invoke) Token: 0x060009A7 RID: 2471
	private delegate void EventDelegate(GameEvent e);
}
