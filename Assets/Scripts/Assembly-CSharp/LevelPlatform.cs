using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class LevelPlatform : AbstractCollidableObject
{
	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000FEE RID: 4078 RVA: 0x0009D8BB File Offset: 0x0009BCBB
	// (set) Token: 0x06000FEF RID: 4079 RVA: 0x0009D8C3 File Offset: 0x0009BCC3
	private protected List<Transform> players { protected get; private set; }

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0009D8CC File Offset: 0x0009BCCC
	public bool AllowShadows
	{
		get
		{
			return this.allowShadows;
		}
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x0009D8D4 File Offset: 0x0009BCD4
	protected override void Awake()
	{
		base.Awake();
		this.players = new List<Transform>();
		base.gameObject.layer = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString());
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x0009D914 File Offset: 0x0009BD14
	public virtual void AddChild(Transform player)
	{
		if (!this.players.Contains(player))
		{
			this.players.Add(player);
		}
		player.parent = base.transform;
		Vector3 localScale = player.localScale;
		localScale.y = 1f;
		LevelPlayerMotor component = player.GetComponent<LevelPlayerMotor>();
		if (component != null)
		{
			localScale.y *= component.GravityReversalMultiplier;
		}
		player.localScale = localScale;
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0009D98B File Offset: 0x0009BD8B
	public virtual void OnPlayerExit(Transform player)
	{
		if (this.players.Contains(player))
		{
			this.players.Remove(player);
		}
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0009D9AC File Offset: 0x0009BDAC
	protected override void OnDestroy()
	{
		foreach (Transform transform in this.players)
		{
			if (!(transform == null))
			{
				transform.parent = null;
			}
		}
		base.OnDestroy();
	}

	// Token: 0x04001984 RID: 6532
	public bool canFallThrough = true;

	// Token: 0x04001986 RID: 6534
	[SerializeField]
	private bool allowShadows = true;
}
