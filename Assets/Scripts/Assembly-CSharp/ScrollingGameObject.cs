using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043D RID: 1085
public class ScrollingGameObject : AbstractMonoBehaviour
{
	// Token: 0x06000FF6 RID: 4086 RVA: 0x0009DE60 File Offset: 0x0009C260
	protected override void Awake()
	{
		base.Awake();
		GameObject gameObject = new GameObject("Container");
		gameObject.transform.SetParent(base.transform);
		if (this.resetTransforms)
		{
			base.transform.ResetLocalTransforms();
		}
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.SetParent(gameObject.transform);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject.gameObject);
		gameObject2.transform.SetParent(base.transform);
		gameObject2.transform.ResetLocalTransforms();
		gameObject2.transform.SetLocalPosition(new float?((float)this.size), new float?(0f), new float?(0f));
		GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
		gameObject3.transform.SetParent(base.transform);
		gameObject3.transform.SetLocalPosition(new float?((float)(-(float)this.size)), new float?(0f), new float?(0f));
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0009DFA4 File Offset: 0x0009C3A4
	private void Update()
	{
		Vector3 localPosition = base.transform.localPosition;
		if (localPosition.x <= (float)(-(float)this.size))
		{
			localPosition.x += (float)this.size;
		}
		if (localPosition.x >= 1280f)
		{
			localPosition.x -= (float)this.size;
		}
		localPosition.x -= (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x04001987 RID: 6535
	public ScrollingGameObject.Axis axis;

	// Token: 0x04001988 RID: 6536
	[SerializeField]
	private bool negativeDirection;

	// Token: 0x04001989 RID: 6537
	[Range(0f, 500f)]
	[SerializeField]
	private float speed;

	// Token: 0x0400198A RID: 6538
	[SerializeField]
	private int size = 1280;

	// Token: 0x0400198B RID: 6539
	[SerializeField]
	private bool resetTransforms = true;

	// Token: 0x0200043E RID: 1086
	public enum Axis
	{
		// Token: 0x0400198D RID: 6541
		X,
		// Token: 0x0400198E RID: 6542
		Y
	}
}
