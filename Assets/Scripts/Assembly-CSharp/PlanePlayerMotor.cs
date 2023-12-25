using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A9C RID: 2716
public class PlanePlayerMotor : AbstractPlanePlayerComponent
{
	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06004118 RID: 16664 RVA: 0x00235D28 File Offset: 0x00234128
	// (set) Token: 0x06004119 RID: 16665 RVA: 0x00235D30 File Offset: 0x00234130
	public Trilean2 MoveDirection { get; private set; }

	// Token: 0x0600411A RID: 16666 RVA: 0x00235D3C File Offset: 0x0023413C
	protected override void OnAwake()
	{
		base.OnAwake();
		this.MoveDirection = default(Trilean2);
		this.properties = new PlanePlayerMotor.Properties();
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.OnReviveEvent += this.OnRevive;
		this.pos = base.transform.position;
	}

	// Token: 0x0600411B RID: 16667 RVA: 0x00235DB2 File Offset: 0x002341B2
	private void Start()
	{
		this.pos = base.transform.position;
	}

	// Token: 0x0600411C RID: 16668 RVA: 0x00235DCA File Offset: 0x002341CA
	private void FixedUpdate()
	{
		if (base.player.stats.StoneTime > 0f)
		{
			return;
		}
		this.HandleInput();
		this.Move();
		this.HandleRaycasts();
		this.ClampPosition();
	}

	// Token: 0x0600411D RID: 16669 RVA: 0x00235DFF File Offset: 0x002341FF
	private void LateUpdate()
	{
		this.ClampPosition();
	}

	// Token: 0x0600411E RID: 16670 RVA: 0x00235E07 File Offset: 0x00234207
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (info.damage > 0f)
		{
			base.StartCoroutine(this.onDamageTaken_cr());
		}
	}

	// Token: 0x0600411F RID: 16671 RVA: 0x00235E28 File Offset: 0x00234228
	private void HandleInput()
	{
		this.timeSinceInputBuffered += CupheadTime.FixedDelta;
		if (base.player.WeaponBusy)
		{
			this.BufferInputs();
		}
		if (this.damageStun)
		{
			this.MoveDirection = new Trilean2(-1, 0);
			return;
		}
		Trilean trilean = 0;
		Trilean trilean2 = 0;
		float axis = base.player.input.actions.GetAxis(0);
		float axis2 = base.player.input.actions.GetAxis(1);
		if (axis > 0.35f || axis < -0.35f)
		{
			trilean = axis;
		}
		if (axis2 > 0.35f || axis2 < -0.35f)
		{
			trilean2 = axis2;
		}
		this.MoveDirection = new Trilean2(trilean.Value, trilean2.Value);
	}

	// Token: 0x06004120 RID: 16672 RVA: 0x00235F05 File Offset: 0x00234305
	private void BufferInput(PlanePlayerMotor.BufferedInput input)
	{
		this.bufferedInput = input;
		this.timeSinceInputBuffered = 0f;
	}

	// Token: 0x06004121 RID: 16673 RVA: 0x00235F1C File Offset: 0x0023431C
	public void BufferInputs()
	{
		if (base.player.input.actions.GetButtonDown(2))
		{
			this.BufferInput(PlanePlayerMotor.BufferedInput.Jump);
		}
		else if (base.player.input.actions.GetButtonDown(4))
		{
			this.BufferInput(PlanePlayerMotor.BufferedInput.Super);
		}
	}

	// Token: 0x06004122 RID: 16674 RVA: 0x00235F72 File Offset: 0x00234372
	public void ClearBufferedInput()
	{
		this.timeSinceInputBuffered = 0.134f;
	}

	// Token: 0x06004123 RID: 16675 RVA: 0x00235F7F File Offset: 0x0023437F
	public bool HasBufferedInput(PlanePlayerMotor.BufferedInput input)
	{
		return this.bufferedInput == input && this.timeSinceInputBuffered < 0.134f;
	}

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06004124 RID: 16676 RVA: 0x00235F9D File Offset: 0x0023439D
	public Vector2 Velocity
	{
		get
		{
			return this._velocity;
		}
	}

	// Token: 0x06004125 RID: 16677 RVA: 0x00235FA8 File Offset: 0x002343A8
	private void Move()
	{
		float num = (!base.player.Shrunk) ? this.properties.speed : this.properties.shrunkSpeed;
		if (this.MoveDirection.x != 0 && this.MoveDirection.y != 0)
		{
			num *= 0.75f;
		}
		this.pos.x = this.pos.x + this.MoveDirection.x * num * CupheadTime.FixedDelta;
		this.pos.y = this.pos.y + this.MoveDirection.y * num * CupheadTime.FixedDelta;
		foreach (PlanePlayerMotor.Force force in this.externalForces)
		{
			if (force.enabled)
			{
				this.pos += force.force * CupheadTime.FixedDelta;
			}
		}
		Vector2 b = base.transform.position;
		if (PlanePlayerMotor.USE_FALLOFF)
		{
			float num2 = 15f;
			base.transform.position = Vector2.Lerp(base.transform.position, this.pos, num2 * CupheadTime.FixedDelta);
		}
		else
		{
			base.transform.AddPosition(0f, this.MoveDirection.y * num * CupheadTime.FixedDelta, 0f);
		}
		Vector2 a = base.transform.position;
		this._velocity = (a - b) / CupheadTime.FixedDelta;
	}

	// Token: 0x06004126 RID: 16678 RVA: 0x002361A4 File Offset: 0x002345A4
	private void HandleRaycasts()
	{
		int layerMask = 262144;
		int layerMask2 = 524288;
		int layerMask3 = 1048576;
		Vector2 origin = base.transform.position + new Vector2(-20f, 15f);
		float num = 100f;
		float num2 = 100f;
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(origin, new Vector2(1f, num2), 0f, Vector2.left, num * 0.5f, layerMask);
		RaycastHit2D raycastHit2D2 = Physics2D.BoxCast(origin, new Vector2(1f, num2), 0f, Vector2.right, num * 0.5f, layerMask);
		RaycastHit2D raycastHit2D3 = Physics2D.BoxCast(origin, new Vector2(num, 1f), 0f, Vector2.up, num2 * 0.5f, layerMask2);
		RaycastHit2D raycastHit2D4 = Physics2D.BoxCast(origin, new Vector2(num, 1f), 0f, Vector2.down, num2 * 0.5f, layerMask3);
		if (raycastHit2D.collider != null)
		{
			base.transform.SetPosition(new float?(raycastHit2D.point.x + 70f), null, null);
		}
		if (raycastHit2D2.collider != null)
		{
			base.transform.SetPosition(new float?(raycastHit2D2.point.x - 30f), null, null);
		}
		if (raycastHit2D3.collider != null)
		{
			base.transform.SetPosition(null, new float?(raycastHit2D3.point.y - 65f), null);
		}
		if (raycastHit2D4.collider != null)
		{
			base.transform.SetPosition(null, new float?(raycastHit2D4.point.y + 35f), null);
		}
	}

	// Token: 0x06004127 RID: 16679 RVA: 0x002363C4 File Offset: 0x002347C4
	private void ClampPosition()
	{
		Vector2 vector = this.pos;
		vector.x = Mathf.Clamp(vector.x, (float)Level.Current.Left + 70f, (float)Level.Current.Right - 30f);
		vector.y = Mathf.Clamp(vector.y, (float)Level.Current.Ground + 35f, (float)Level.Current.Ceiling - 65f);
		this.pos = vector;
	}

	// Token: 0x06004128 RID: 16680 RVA: 0x0023644C File Offset: 0x0023484C
	public void OnRevive(Vector3 pos)
	{
		base.transform.position = pos;
		this.pos = pos;
		this.MoveDirection = default(Trilean2);
		this.damageStun = false;
	}

	// Token: 0x06004129 RID: 16681 RVA: 0x00236488 File Offset: 0x00234888
	private IEnumerator onDamageTaken_cr()
	{
		this.damageStun = true;
		yield return CupheadTime.WaitForSeconds(this, 0.15f);
		this.damageStun = false;
		yield break;
	}

	// Token: 0x0600412A RID: 16682 RVA: 0x002364A3 File Offset: 0x002348A3
	public void AddForce(PlanePlayerMotor.Force force)
	{
		this.externalForces.Add(force);
	}

	// Token: 0x0600412B RID: 16683 RVA: 0x002364B1 File Offset: 0x002348B1
	public void RemoveForce(PlanePlayerMotor.Force force)
	{
		this.externalForces.Remove(force);
	}

	// Token: 0x040047B7 RID: 18359
	private const float PADDING_TOP = 65f;

	// Token: 0x040047B8 RID: 18360
	private const float PADDING_BOTTOM = 35f;

	// Token: 0x040047B9 RID: 18361
	private const float PADDING_LEFT = 70f;

	// Token: 0x040047BA RID: 18362
	private const float PADDING_RIGHT = 30f;

	// Token: 0x040047BB RID: 18363
	private const float DIAGONAL_FALLOFF = 0.75f;

	// Token: 0x040047BC RID: 18364
	private const float ANALOG_THRESHOLD = 0.35f;

	// Token: 0x040047BD RID: 18365
	private const float HIT_STUN_TIME = 0.15f;

	// Token: 0x040047BE RID: 18366
	private const float EASING_TIME = 15f;

	// Token: 0x040047BF RID: 18367
	private static bool USE_FALLOFF = true;

	// Token: 0x040047C0 RID: 18368
	[NonSerialized]
	public PlanePlayerMotor.Properties properties;

	// Token: 0x040047C2 RID: 18370
	private bool damageStun;

	// Token: 0x040047C3 RID: 18371
	private Vector2 pos;

	// Token: 0x040047C4 RID: 18372
	private List<PlanePlayerMotor.Force> externalForces = new List<PlanePlayerMotor.Force>();

	// Token: 0x040047C5 RID: 18373
	private PlanePlayerMotor.BufferedInput bufferedInput;

	// Token: 0x040047C6 RID: 18374
	private float timeSinceInputBuffered = 0.134f;

	// Token: 0x040047C7 RID: 18375
	private Vector2 _velocity;

	// Token: 0x02000A9D RID: 2717
	public enum BufferedInput
	{
		// Token: 0x040047C9 RID: 18377
		Jump,
		// Token: 0x040047CA RID: 18378
		Super
	}

	// Token: 0x02000A9E RID: 2718
	public class Force
	{
		// Token: 0x0600412D RID: 16685 RVA: 0x00134CFC File Offset: 0x001330FC
		public Force(Vector2 force, bool enabled)
		{
			this.force = force;
			this.enabled = enabled;
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600412E RID: 16686 RVA: 0x00134D12 File Offset: 0x00133112
		// (set) Token: 0x0600412F RID: 16687 RVA: 0x00134D1A File Offset: 0x0013311A
		public virtual Vector2 force { get; private set; }

		// Token: 0x040047CC RID: 18380
		public bool enabled;
	}

	// Token: 0x02000A9F RID: 2719
	[Serializable]
	public class Properties
	{
		// Token: 0x040047CD RID: 18381
		public float speed = 520f;

		// Token: 0x040047CE RID: 18382
		public float shrunkSpeed = 720f;
	}
}
