using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class ComponentReferenceManager
{
	// Token: 0x06000A5C RID: 2652 RVA: 0x0007E730 File Offset: 0x0007CB30
	public void AddRef(Component c)
	{
		string key = string.Format("Index:<color=red>{0}</color> ComponentType:<color=red>{1}</color> GameObject:<color=red>{2}</color>", this.count, c.GetType().ToString(), this.GetGameObjectPath(c));
		this.refs[key] = new WeakReference(c);
		this.count++;
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0007E788 File Offset: 0x0007CB88
	private string GetGameObjectPath(Component c)
	{
		GameObject gameObject = c.gameObject;
		string text = "/" + gameObject.name;
		while (gameObject.transform.parent != null)
		{
			gameObject = gameObject.transform.parent.gameObject;
			text = "/" + gameObject.name + text;
		}
		return text;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0007E7EC File Offset: 0x0007CBEC
	public void PrintLog()
	{
		GC.Collect();
		global::Debug.LogError("Objects that are destroyed but still referenced:", null);
		foreach (KeyValuePair<string, WeakReference> keyValuePair in this.refs)
		{
			if (keyValuePair.Value.IsAlive)
			{
				if (keyValuePair.Value.Target == null)
				{
					global::Debug.LogErrorFormat("Target is null {0}", new object[]
					{
						keyValuePair.Key
					});
				}
				else
				{
					Component component = keyValuePair.Value.Target as Component;
					if (component == null)
					{
						global::Debug.LogErrorFormat("Component is null {0}", new object[]
						{
							keyValuePair.Key
						});
					}
					else if (component.gameObject == null)
					{
						global::Debug.LogErrorFormat("Component attached game object is null {0}", new object[]
						{
							keyValuePair.Key
						});
					}
				}
			}
		}
	}

	// Token: 0x04001464 RID: 5220
	public static ComponentReferenceManager Instance = new ComponentReferenceManager();

	// Token: 0x04001465 RID: 5221
	private Dictionary<string, WeakReference> refs = new Dictionary<string, WeakReference>();

	// Token: 0x04001466 RID: 5222
	private int count;
}
