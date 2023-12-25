using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200097F RID: 2431
public class MapPlayerMotor : AbstractMapPlayerComponent
{
	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x060038B8 RID: 14520 RVA: 0x002049D0 File Offset: 0x00202DD0
	// (set) Token: 0x060038B9 RID: 14521 RVA: 0x002049D8 File Offset: 0x00202DD8
	public Vector2 velocity { get; private set; }

	// Token: 0x060038BA RID: 14522 RVA: 0x002049E4 File Offset: 0x00202DE4
	private void Update()
	{
		if (!MapPlayerController.CanMove() || base.player.state == MapPlayerController.State.Stationary)
		{
			this.velocity = Vector2.zero;
			this.axis = Vector2.zero;
			base.rigidbody2D.velocity = Vector2.zero;
			return;
		}
		if (PauseManager.state == PauseManager.State.Paused)
		{
			return;
		}
		this.HandleInput();
		MapPlayerController.State state = base.player.state;
		if (state != MapPlayerController.State.Walking)
		{
			if (state == MapPlayerController.State.Ladder)
			{
				this.MoveLadder();
			}
		}
		else
		{
			this.MoveWalking();
		}
	}

	// Token: 0x060038BB RID: 14523 RVA: 0x00204A7C File Offset: 0x00202E7C
	private void LateUpdate()
	{
		MapPlayerController.State state = base.player.state;
		if (state == MapPlayerController.State.Ladder)
		{
			this.ClampPositionLadder();
		}
	}

	// Token: 0x060038BC RID: 14524 RVA: 0x00204AAC File Offset: 0x00202EAC
	public override void OnPause()
	{
		base.OnPause();
		base.rigidbody2D.velocity = Vector2.zero;
	}

	// Token: 0x060038BD RID: 14525 RVA: 0x00204AC4 File Offset: 0x00202EC4
	private void HandleInput()
	{
		if (base.player.EquipMenuOpen)
		{
			return;
		}
		this.axis = new Vector2((float)base.input.GetAxisInt(PlayerInput.Axis.X, false, false), (float)base.input.GetAxisInt(PlayerInput.Axis.Y, false, false));
		float magnitude = this.axis.magnitude;
		if (magnitude < 0.0001f)
		{
			this.axis = Vector2.zero;
		}
		else
		{
			this.axis /= magnitude;
		}
	}

	// Token: 0x060038BE RID: 14526 RVA: 0x00204B44 File Offset: 0x00202F44
	private void MoveWalking()
	{
		this.velocity = Vector2.Lerp(this.velocity, new Vector2(this.axis.x * 2.5f, this.axis.y * 2.5f), CupheadTime.Delta * 100f);
		base.rigidbody2D.velocity = this.velocity;
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x00204BAA File Offset: 0x00202FAA
	private void MoveLadder()
	{
		this.velocity = new Vector2(0f, this.axis.y * 2.5f);
		base.rigidbody2D.velocity = this.velocity;
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x00204BE0 File Offset: 0x00202FE0
	private void ClampPositionLadder()
	{
		MapPlayerLadderObject mapPlayerLadderObject = base.player.ladderManager.Current;
		MapPlayerController.State state = base.player.state;
		if (state == MapPlayerController.State.Ladder)
		{
			base.transform.SetPosition(null, new float?(Mathf.Clamp(base.transform.position.y, mapPlayerLadderObject.bottom.y, mapPlayerLadderObject.top.y)), null);
		}
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x00204C73 File Offset: 0x00203073
	protected override void OnLadderEnter(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location)
	{
		base.OnLadderEnter(point, ladder, location);
		base.StartCoroutine(this.onLadderStart_cr(point, location));
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x00204C8D File Offset: 0x0020308D
	protected override void OnLadderExit(Vector2 point, Vector2 exit, MapLadder.Location location)
	{
		base.OnLadderExit(point, exit, location);
		base.StartCoroutine(this.onLadderEnd_cr(exit, location));
	}

	// Token: 0x060038C3 RID: 14531 RVA: 0x00204CA8 File Offset: 0x002030A8
	private IEnumerator onLadderStart_cr(Vector2 endPos, MapLadder.Location location)
	{
		location = ((location != MapLadder.Location.Top) ? MapLadder.Location.Top : MapLadder.Location.Bottom);
		yield return base.StartCoroutine(this.ladder_cr(base.transform.position, endPos, location));
		base.player.LadderEnterComplete();
		yield break;
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x00204CD4 File Offset: 0x002030D4
	private IEnumerator onLadderEnd_cr(Vector2 endPos, MapLadder.Location location)
	{
		yield return base.StartCoroutine(this.ladder_cr(base.transform.position, endPos, location));
		base.player.LadderExitComplete();
		yield break;
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x00204D00 File Offset: 0x00203100
	private IEnumerator ladder_cr(Vector2 startPos, Vector2 endPos, MapLadder.Location location)
	{
		Vector2 centerPos = new Vector2(Mathf.Lerp(startPos.x, endPos.x, 0.5f), (location != MapLadder.Location.Top) ? (startPos.y + 0.2f) : (endPos.y + 0.2f));
		float t = 0f;
		float time = 0.15f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / time);
			Vector2 newPos = Vector2.Lerp(startPos, centerPos, val);
			base.transform.SetPosition(new float?(newPos.x), new float?(newPos.y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			Vector2 newPos2 = Vector2.Lerp(centerPos, endPos, val2);
			base.transform.SetPosition(new float?(newPos2.x), new float?(newPos2.y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400406D RID: 16493
	private const float SPEED = 2.5f;

	// Token: 0x0400406E RID: 16494
	private const float DIAGONAL_FALLOFF = 0.75f;

	// Token: 0x0400406F RID: 16495
	private const float FALLOFF_SPEED = 100f;

	// Token: 0x04004070 RID: 16496
	public const float INPUT_THRESHOLD = 0.3f;

	// Token: 0x04004072 RID: 16498
	private Vector2 axis;

	// Token: 0x04004073 RID: 16499
	public const float LADDER_ENTER_TIME = 0.3f;

	// Token: 0x04004074 RID: 16500
	public const float LADDER_EXIT_TIME = 0.3f;

	// Token: 0x04004075 RID: 16501
	public const float LADDER_EXIT_JUMP = 0.2f;
}
