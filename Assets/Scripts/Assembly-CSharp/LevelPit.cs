using System;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class LevelPit : AbstractCollidableObject
{
	// Token: 0x1700030B RID: 779
	// (get) Token: 0x0600136F RID: 4975 RVA: 0x000AB643 File Offset: 0x000A9A43
	// (set) Token: 0x06001370 RID: 4976 RVA: 0x000AB64A File Offset: 0x000A9A4A
	public static LevelPit Instance { get; private set; }

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06001371 RID: 4977 RVA: 0x000AB652 File Offset: 0x000A9A52
	// (set) Token: 0x06001372 RID: 4978 RVA: 0x000AB65A File Offset: 0x000A9A5A
	public float ExtraOffset
	{
		get
		{
			return this.extraOffset;
		}
		set
		{
			this.extraOffset = value;
		}
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x000AB664 File Offset: 0x000A9A64
	private void Start()
	{
		if (Level.Current.LevelType == Level.Type.Platforming)
		{
			LevelPit.Instance = this;
			base.transform.SetParent(CupheadLevelCamera.Current.transform);
			base.transform.ResetLocalTransforms();
			base.transform.SetLocalPosition(new float?(0f), new float?(-500f), new float?(0f));
		}
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x000AB6D0 File Offset: 0x000A9AD0
	private void FixedUpdate()
	{
		this.CheckPlayer(PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController);
		this.CheckPlayer(PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController);
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x000AB6F4 File Offset: 0x000A9AF4
	private void CheckPlayer(LevelPlayerController player)
	{
		if (player == null || player.IsDead)
		{
			return;
		}
		float num = 1f;
		if (Level.Current.LevelType == Level.Type.Platforming)
		{
			num *= 1.3f;
		}
		num *= this.forceMultiplier;
		if (player.motor.GravityReversed && Level.Current.LevelType == Level.Type.Platforming)
		{
			float num2 = base.transform.parent.position.y - base.transform.localPosition.y;
			if (player.transform.position.y >= num2 - this.extraOffset)
			{
				player.OnPitKnockUp(num2 - this.extraOffset, num);
			}
		}
		else if (player.transform.position.y <= base.transform.position.y + this.extraOffset)
		{
			player.OnPitKnockUp(base.transform.position.y + this.extraOffset, num);
		}
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x000AB818 File Offset: 0x000A9C18
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.3f);
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x000AB82B File Offset: 0x000A9C2B
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x000AB840 File Offset: 0x000A9C40
	private void DrawGizmos(float a)
	{
		Rect rect = new Rect(base.baseTransform.position.x + -1000f, base.baseTransform.position.y, 2000f, 0f);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawLine(new Vector2(rect.xMin, rect.y), new Vector2(rect.xMax, rect.y));
		for (int i = 0; i < 20; i++)
		{
			float num = 100f;
			Rect rect2 = new Rect(rect.xMin + num * (float)i, rect.y, num, -20f);
			Gizmos.DrawLine(new Vector2(rect2.xMin, rect2.y), new Vector2(rect2.center.x, rect2.yMax));
			Gizmos.DrawLine(new Vector2(rect2.xMax, rect2.y), new Vector2(rect2.center.x, rect2.yMax));
		}
	}

	// Token: 0x04001C84 RID: 7300
	private const float PLATFORMING_LEVEL_CAMERA_OFFSET_Y = -500f;

	// Token: 0x04001C85 RID: 7301
	[SerializeField]
	private float extraOffset;

	// Token: 0x04001C86 RID: 7302
	[SerializeField]
	private float forceMultiplier = 1f;
}
