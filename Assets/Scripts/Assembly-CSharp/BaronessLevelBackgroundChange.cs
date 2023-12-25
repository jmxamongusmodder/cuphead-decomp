using System;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public class BaronessLevelBackgroundChange : AbstractPausableComponent
{
	// Token: 0x06001672 RID: 5746 RVA: 0x000C98F8 File Offset: 0x000C7CF8
	protected override void Awake()
	{
		base.Awake();
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		this.size = ((this.b_axis != BaronessLevelBackgroundChange.B_Axis.X) ? ((int)component.sprite.bounds.size.y) : ((int)component.sprite.bounds.size.x)) - this.b_offset;
		this.getOffset.x = base.transform.position.x;
		for (int i = 0; i < this.b_count; i++)
		{
			this.copy = new GameObject(base.gameObject.name + " Copy");
			this.copy.transform.parent = base.transform;
			this.copy.transform.ResetLocalTransforms();
			SpriteRenderer spriteRenderer = this.copy.AddComponent<SpriteRenderer>();
			spriteRenderer.sortingLayerID = component.sortingLayerID;
			spriteRenderer.sortingOrder = component.sortingOrder;
			spriteRenderer.sprite = component.sprite;
			spriteRenderer.material = component.material;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.copy);
			gameObject.transform.parent = base.transform;
			this.copy.transform.SetLocalPosition(new float?((float)(-(float)(this.size + this.size * i))), new float?(0f), new float?(0f));
			gameObject.transform.SetLocalPosition(new float?((float)(-(float)(this.size * 2 + this.size * i))), new float?(0f), new float?(0f));
		}
	}

	// Token: 0x06001673 RID: 5747 RVA: 0x000C9AC1 File Offset: 0x000C7EC1
	private void OnEnable()
	{
		if (!this.isClouds && this.sprite != null)
		{
			this.sprite.GetComponent<OneTimeScrollingSprite>().OutCondition = (() => this.baroness.state == BaronessLevelCastle.State.Dead);
		}
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x000C9AFB File Offset: 0x000C7EFB
	private void OnDisable()
	{
		if (this.sprite != null)
		{
			this.sprite.GetComponent<OneTimeScrollingSprite>().OutCondition = null;
		}
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x000C9B20 File Offset: 0x000C7F20
	private void Update()
	{
		if (!this.isClouds && this.baroness.state != BaronessLevelCastle.State.Chase)
		{
			return;
		}
		if (!this.baroness.pauseScrolling)
		{
			if (base.GetComponent<ParallaxLayer>() != null)
			{
				base.GetComponent<ParallaxLayer>().enabled = false;
			}
			if (this.sprite != null)
			{
				this.sprite.speed = -this.speed;
			}
			Vector3 localPosition = base.transform.localPosition;
			if (localPosition.x >= -((float)this.size - this.getOffset.x))
			{
				localPosition.x -= (float)this.size;
			}
			if (localPosition.x <= (float)this.size - this.getOffset.x)
			{
				localPosition.x += (float)this.size;
			}
			localPosition.x -= (float)((!this.b_negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.b_playbackSpeed;
			base.transform.localPosition = localPosition;
		}
		else if (this.sprite != null)
		{
			this.sprite.GetComponent<OneTimeScrollingSprite>().speed = 0f;
		}
	}

	// Token: 0x04001FBC RID: 8124
	public BaronessLevelBackgroundChange.B_Axis b_axis;

	// Token: 0x04001FBD RID: 8125
	private int size;

	// Token: 0x04001FBE RID: 8126
	private const float X_OUT = -1280f;

	// Token: 0x04001FBF RID: 8127
	[Range(0f, 2000f)]
	public float speed;

	// Token: 0x04001FC0 RID: 8128
	[SerializeField]
	private bool isClouds;

	// Token: 0x04001FC1 RID: 8129
	[SerializeField]
	protected bool b_negativeDirection;

	// Token: 0x04001FC2 RID: 8130
	[SerializeField]
	protected int b_offset;

	// Token: 0x04001FC3 RID: 8131
	[SerializeField]
	[Range(1f, 10f)]
	protected int b_count = 1;

	// Token: 0x04001FC4 RID: 8132
	[SerializeField]
	private BaronessLevelCastle baroness;

	// Token: 0x04001FC5 RID: 8133
	[SerializeField]
	private OneTimeScrollingSprite sprite;

	// Token: 0x04001FC6 RID: 8134
	[NonSerialized]
	public float b_playbackSpeed = 1f;

	// Token: 0x04001FC7 RID: 8135
	private GameObject copy;

	// Token: 0x04001FC8 RID: 8136
	private Vector3 getOffset;

	// Token: 0x020004FB RID: 1275
	public enum B_Axis
	{
		// Token: 0x04001FCA RID: 8138
		X,
		// Token: 0x04001FCB RID: 8139
		Y
	}
}
