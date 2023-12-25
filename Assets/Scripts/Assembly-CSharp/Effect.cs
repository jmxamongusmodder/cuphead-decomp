using System;
using UnityEngine;

// Token: 0x02000B0E RID: 2830
[Serializable]
public class Effect : AbstractCollidableObject
{
	// Token: 0x060044AC RID: 17580 RVA: 0x000B9EAB File Offset: 0x000B82AB
	public virtual Effect Create(Vector3 position)
	{
		return this.Create(position, Vector3.one);
	}

	// Token: 0x060044AD RID: 17581 RVA: 0x000B9EBC File Offset: 0x000B82BC
	public virtual Effect Create(Vector3 position, Vector3 scale)
	{
		Effect component = UnityEngine.Object.Instantiate<GameObject>(base.gameObject).GetComponent<Effect>();
		component.name = component.name.Replace("(Clone)", string.Empty);
		if (this.randomMirrorX)
		{
			scale.x = ((!Rand.Bool()) ? (-scale.x) : scale.x);
		}
		if (this.randomMirrorY)
		{
			scale.y = ((!Rand.Bool()) ? (-scale.y) : scale.y);
		}
		component.Initialize(position, scale, this.randomRotation);
		return component;
	}

	// Token: 0x060044AE RID: 17582 RVA: 0x000B9F64 File Offset: 0x000B8364
	public virtual void Initialize(Vector3 position)
	{
		Vector3 scale = new Vector3(1f, 1f);
		if (this.randomMirrorX)
		{
			scale.x = ((!Rand.Bool()) ? (-scale.x) : scale.x);
		}
		if (this.randomMirrorY)
		{
			scale.y = ((!Rand.Bool()) ? (-scale.y) : scale.y);
		}
		this.Initialize(position, scale, this.randomRotation);
	}

	// Token: 0x060044AF RID: 17583 RVA: 0x000B9FF0 File Offset: 0x000B83F0
	public virtual void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		int value = UnityEngine.Random.Range(0, base.animator.GetInteger("Count"));
		base.animator.SetInteger("Effect", value);
		Transform transform = base.transform;
		transform.position = position;
		transform.localScale = scale;
		if (randomR)
		{
			transform.eulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0f, 360f));
		}
	}

	// Token: 0x060044B0 RID: 17584 RVA: 0x000BA064 File Offset: 0x000B8464
	protected virtual void OnEffectCompletePool()
	{
		if (this.removeOnEnd)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.inUse = false;
		}
	}

	// Token: 0x060044B1 RID: 17585 RVA: 0x000BA088 File Offset: 0x000B8488
	protected virtual void OnEffectComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060044B2 RID: 17586 RVA: 0x000BA095 File Offset: 0x000B8495
	public void Play()
	{
		base.animator.Play("A");
	}

	// Token: 0x04004A67 RID: 19047
	[SerializeField]
	protected bool randomRotation;

	// Token: 0x04004A68 RID: 19048
	[Space(10f)]
	[SerializeField]
	protected bool randomMirrorX;

	// Token: 0x04004A69 RID: 19049
	[SerializeField]
	protected bool randomMirrorY;

	// Token: 0x04004A6A RID: 19050
	public bool inUse;

	// Token: 0x04004A6B RID: 19051
	public bool removeOnEnd;
}
