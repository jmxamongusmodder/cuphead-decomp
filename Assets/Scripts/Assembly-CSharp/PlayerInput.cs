using System;
using Rewired;
using UnityEngine;

// Token: 0x02000AC9 RID: 2761
public class PlayerInput : AbstractMonoBehaviour
{
	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06004242 RID: 16962 RVA: 0x0023C36F File Offset: 0x0023A76F
	// (set) Token: 0x06004243 RID: 16963 RVA: 0x0023C377 File Offset: 0x0023A777
	public PlayerId playerId { get; private set; }

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06004244 RID: 16964 RVA: 0x0023C380 File Offset: 0x0023A780
	public bool IsDead
	{
		get
		{
			return this.player != null && this.player.IsDead;
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06004245 RID: 16965 RVA: 0x0023C3A0 File Offset: 0x0023A7A0
	// (set) Token: 0x06004246 RID: 16966 RVA: 0x0023C3A8 File Offset: 0x0023A7A8
	public Player actions { get; private set; }

	// Token: 0x06004247 RID: 16967 RVA: 0x0023C3B1 File Offset: 0x0023A7B1
	protected override void Awake()
	{
		base.Awake();
		this.player = base.GetComponent<AbstractPlayerController>();
	}

	// Token: 0x06004248 RID: 16968 RVA: 0x0023C3C5 File Offset: 0x0023A7C5
	private void Start()
	{
		if (Level.Current != null && Level.Current.CameraRotates)
		{
			this.canRotateInput = true;
			this.cameraTransform = Camera.main.transform;
		}
	}

	// Token: 0x06004249 RID: 16969 RVA: 0x0023C3FD File Offset: 0x0023A7FD
	public void Init(PlayerId playerId)
	{
		this.playerId = playerId;
		this.actions = PlayerManager.GetPlayerInput(playerId);
	}

	// Token: 0x0600424A RID: 16970 RVA: 0x0023C412 File Offset: 0x0023A812
	public override void StopAllCoroutines()
	{
	}

	// Token: 0x0600424B RID: 16971 RVA: 0x0023C414 File Offset: 0x0023A814
	public int GetAxisInt(PlayerInput.Axis axis, bool crampedDiagonal = false, bool duckMod = false)
	{
		Vector2 v = new Vector2(this.actions.GetAxis(0), this.actions.GetAxis(1));
		if (this.canRotateInput)
		{
			if (SettingsData.Data.rotateControlsWithCamera)
			{
				v = this.cameraTransform.rotation * v;
			}
			else if (Mathf.Abs(this.cameraTransform.rotation.eulerAngles.z - 180f) <= 1f)
			{
				v = this.cameraTransform.rotation * v;
			}
		}
		float magnitude = v.magnitude;
		float num = (!crampedDiagonal) ? 0.38268f : 0.5f;
		if (magnitude < 0.375f)
		{
			return 0;
		}
		float num2 = ((axis != PlayerInput.Axis.X) ? v.y : v.x) / magnitude;
		if (num2 > num)
		{
			return 1;
		}
		if (num2 < ((!duckMod) ? (-num) : -0.705f))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x0023C538 File Offset: 0x0023A938
	public float GetAxis(PlayerInput.Axis axis)
	{
		if (axis == PlayerInput.Axis.X)
		{
			return this.actions.GetAxis(0);
		}
		return this.actions.GetAxis(1);
	}

	// Token: 0x0600424D RID: 16973 RVA: 0x0023C559 File Offset: 0x0023A959
	public bool GetButton(CupheadButton button)
	{
		return this.actions.GetButton((int)button);
	}

	// Token: 0x040048B7 RID: 18615
	private AbstractPlayerController player;

	// Token: 0x040048BA RID: 18618
	private bool canRotateInput;

	// Token: 0x040048BB RID: 18619
	private Transform cameraTransform;

	// Token: 0x02000ACA RID: 2762
	public enum Axis
	{
		// Token: 0x040048BD RID: 18621
		X,
		// Token: 0x040048BE RID: 18622
		Y
	}
}
