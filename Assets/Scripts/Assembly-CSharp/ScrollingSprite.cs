using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B1A RID: 2842
public class ScrollingSprite : AbstractPausableComponent
{
	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x060044D6 RID: 17622 RVA: 0x00114278 File Offset: 0x00112678
	// (set) Token: 0x060044D7 RID: 17623 RVA: 0x00114280 File Offset: 0x00112680
	public List<SpriteRenderer> copyRenderers { get; private set; }

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x060044D8 RID: 17624 RVA: 0x00114289 File Offset: 0x00112689
	// (set) Token: 0x060044D9 RID: 17625 RVA: 0x00114291 File Offset: 0x00112691
	public bool looping { get; set; }

	// Token: 0x060044DA RID: 17626 RVA: 0x0011429C File Offset: 0x0011269C
	protected virtual void Start()
	{
		this.looping = true;
		this.copyRenderers = new List<SpriteRenderer>();
		this.direction = ((!this.negativeDirection) ? 1 : -1);
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		this.copyRenderers.Add(component);
		this.size = ((this.axis != ScrollingSprite.Axis.X) ? component.sprite.bounds.size.y : component.sprite.bounds.size.x) - this.offset;
		for (int i = 0; i < this.count; i++)
		{
			GameObject gameObject = new GameObject(base.gameObject.name + " Copy");
			gameObject.transform.parent = base.transform;
			gameObject.transform.ResetLocalTransforms();
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sortingLayerID = component.sortingLayerID;
			spriteRenderer.sortingOrder = component.sortingOrder;
			spriteRenderer.sprite = component.sprite;
			spriteRenderer.material = component.material;
			this.copyRenderers.Add(spriteRenderer);
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.ResetLocalTransforms();
			this.copyRenderers.Add(gameObject2.GetComponent<SpriteRenderer>());
			if (this.axis == ScrollingSprite.Axis.X)
			{
				gameObject.transform.SetLocalPosition(new float?((float)this.direction * (this.size + this.size * (float)i)), new float?(0f), new float?(0f));
				gameObject2.transform.SetLocalPosition(new float?((float)this.direction * -(this.size + this.size * (float)i)), new float?(0f), new float?(0f));
			}
			else
			{
				gameObject.transform.SetLocalPosition(new float?(0f), new float?(this.size + this.size * (float)i), new float?(0f));
				gameObject2.transform.SetLocalPosition(new float?(0f), new float?(-(this.size + this.size * (float)i)), new float?(0f));
			}
		}
		this.startY = base.transform.localPosition.y;
	}

	// Token: 0x060044DB RID: 17627 RVA: 0x0011452C File Offset: 0x0011292C
	protected virtual void Update()
	{
		this.pos = base.transform.localPosition;
		if (this.axis == ScrollingSprite.Axis.X)
		{
			if (this.pos.x <= -this.size && this.looping)
			{
				this.pos.x = this.pos.x + this.size;
				if (this.isRotated)
				{
					this.pos.y = this.startY;
				}
				this.onLoop();
			}
			if (this.pos.x >= this.size && this.looping)
			{
				this.pos.x = this.pos.x - this.size;
				this.onLoop();
			}
			if (!this.isRotated)
			{
				this.pos.x = this.pos.x - (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.playbackSpeed;
			}
		}
		else
		{
			if (this.pos.y <= -this.size && this.looping)
			{
				this.pos.y = this.pos.y + this.size;
				this.onLoop();
			}
			if (this.pos.y >= this.size && this.looping)
			{
				this.pos.y = this.pos.y - this.size;
				this.onLoop();
			}
			if (!this.isRotated)
			{
				this.pos.y = this.pos.y - (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.playbackSpeed;
			}
		}
		if (this.isRotated)
		{
			this.pos -= base.transform.right * this.speed * CupheadTime.Delta;
		}
		base.transform.localPosition = this.pos;
	}

	// Token: 0x060044DC RID: 17628 RVA: 0x00114758 File Offset: 0x00112B58
	protected virtual void onLoop()
	{
	}

	// Token: 0x060044DD RID: 17629 RVA: 0x0011475A File Offset: 0x00112B5A
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.copyRenderers = null;
	}

	// Token: 0x04004A9A RID: 19098
	public ScrollingSprite.Axis axis;

	// Token: 0x04004A9B RID: 19099
	[SerializeField]
	protected bool negativeDirection;

	// Token: 0x04004A9C RID: 19100
	[SerializeField]
	private bool onLeft;

	// Token: 0x04004A9D RID: 19101
	[SerializeField]
	private bool isRotated;

	// Token: 0x04004A9E RID: 19102
	[Range(0f, 4000f)]
	public float speed;

	// Token: 0x04004A9F RID: 19103
	[SerializeField]
	public float offset;

	// Token: 0x04004AA0 RID: 19104
	[SerializeField]
	[Range(1f, 10f)]
	private int count = 1;

	// Token: 0x04004AA1 RID: 19105
	[NonSerialized]
	public float playbackSpeed = 1f;

	// Token: 0x04004AA2 RID: 19106
	protected float size;

	// Token: 0x04004AA3 RID: 19107
	protected Vector3 pos;

	// Token: 0x04004AA4 RID: 19108
	private float startY;

	// Token: 0x04004AA5 RID: 19109
	protected int direction;

	// Token: 0x02000B1B RID: 2843
	public enum Axis
	{
		// Token: 0x04004AA9 RID: 19113
		X,
		// Token: 0x04004AAA RID: 19114
		Y
	}
}
