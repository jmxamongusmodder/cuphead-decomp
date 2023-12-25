using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class CupheadMapCamera : AbstractCupheadGameCamera
{
	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000D2F RID: 3375 RVA: 0x0008C0C0 File Offset: 0x0008A4C0
	// (set) Token: 0x06000D30 RID: 3376 RVA: 0x0008C0C7 File Offset: 0x0008A4C7
	public static CupheadMapCamera Current { get; private set; }

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000D31 RID: 3377 RVA: 0x0008C0CF File Offset: 0x0008A4CF
	public override float OrthographicSize
	{
		get
		{
			return 3.6f;
		}
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x0008C0D6 File Offset: 0x0008A4D6
	protected override void Awake()
	{
		base.Awake();
		CupheadMapCamera.Current = this;
		this.SetupColliders();
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x0008C0EA File Offset: 0x0008A4EA
	private void OnDestroy()
	{
		if (CupheadMapCamera.Current == this)
		{
			CupheadMapCamera.Current = null;
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000D34 RID: 3380 RVA: 0x0008C104 File Offset: 0x0008A504
	private Vector2 playerCenter
	{
		get
		{
			if (PlayerManager.Multiplayer)
			{
				return (Map.Current.players[0].transform.position + Map.Current.players[1].transform.position) / 2f;
			}
			return Map.Current.players[0].transform.position;
		}
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x0008C17C File Offset: 0x0008A57C
	private void Update()
	{
		if (Map.Current.CurrentState == Map.State.Event)
		{
			return;
		}
		Vector3 position = base.transform.position;
		Vector3 vector = this.playerCenter;
		if (this.properties.moveX)
		{
			position.x = vector.x;
		}
		if (this.properties.moveY)
		{
			position.y = vector.y;
		}
		position.x = Mathf.Clamp(position.x, this.properties.bounds.left + this.offset.x, this.properties.bounds.right - this.offset.x);
		position.y = Mathf.Clamp(position.y, this.properties.bounds.bottom + this.offset.y, this.properties.bounds.top - this.offset.y);
		if (this.centerOnPlayer)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, position, Time.deltaTime * 6f);
		}
		this.UpdateColliders();
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x0008C2BC File Offset: 0x0008A6BC
	public void Init(Map.Camera properties)
	{
		base.camera.orthographicSize = 3.6f;
		this.properties = properties;
		this.offset = new Vector2(base.Bounds.width / 2f, base.Bounds.height / 2f);
		base.transform.position = this.playerCenter;
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0008C32C File Offset: 0x0008A72C
	public bool IsCameraFarFromPlayer()
	{
		Vector3 position = base.transform.position;
		Vector3 b = this.playerCenter;
		b.x = Mathf.Clamp(b.x, this.properties.bounds.left + this.offset.x, this.properties.bounds.right - this.offset.x);
		b.y = Mathf.Clamp(b.y, this.properties.bounds.bottom + this.offset.y, this.properties.bounds.top - this.offset.y);
		return (double)(position - b).sqrMagnitude > 0.01;
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x0008C402 File Offset: 0x0008A802
	public Coroutine MoveToPosition(Vector2 position, float time, float zoom)
	{
		base.Zoom(zoom, time, EaseUtils.EaseType.easeInOutSine);
		return base.StartCoroutine(this.moveToPosition_cr(position, time));
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x0008C41C File Offset: 0x0008A81C
	private IEnumerator moveToPosition_cr(Vector2 position, float time)
	{
		Vector2 start = base.transform.position;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, position.x, val);
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.y, position.y, val);
			base.transform.SetPosition(new float?(x), new float?(y), new float?(0f));
			t += base.LocalDeltaTime;
			yield return null;
		}
		base.transform.position = position;
		yield return null;
		yield break;
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0008C448 File Offset: 0x0008A848
	private void SetupColliders()
	{
		this.edgeCollider = base.gameObject.AddComponent<EdgeCollider2D>();
		this.edgeCollider.points = new Vector2[2];
		this.secretPathEdgeCollider = new GameObject
		{
			transform = 
			{
				parent = base.transform
			},
			layer = 25
		}.AddComponent<EdgeCollider2D>();
		this.secretPathEdgeCollider.points = this.edgeCollider.points;
		this.UpdateColliders();
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x0008C4C0 File Offset: 0x0008A8C0
	private void UpdateColliders()
	{
		Vector2[] points = new Vector2[]
		{
			new Vector3(-base.Bounds.width / 2f, -base.Bounds.height / 2f, 0f),
			new Vector3(-base.Bounds.width / 2f, base.Bounds.height / 2f, 0f),
			new Vector3(base.Bounds.width / 2f, base.Bounds.height / 2f, 0f),
			new Vector3(base.Bounds.width / 2f, -base.Bounds.height / 2f, 0f),
			new Vector3(-base.Bounds.width / 2f, -base.Bounds.height / 2f, 0f)
		};
		this.edgeCollider.points = points;
		this.secretPathEdgeCollider.points = points;
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x0008C648 File Offset: 0x0008AA48
	public void SetActiveCollider(bool active)
	{
		this.edgeCollider.enabled = active;
		this.secretPathEdgeCollider.enabled = active;
	}

	// Token: 0x040016A6 RID: 5798
	public bool centerOnPlayer = true;

	// Token: 0x040016A7 RID: 5799
	private const float SPEED = 6f;

	// Token: 0x040016A8 RID: 5800
	private const float ORTHO_SIZE = 3.6f;

	// Token: 0x040016AA RID: 5802
	private Map.Camera properties;

	// Token: 0x040016AB RID: 5803
	private Vector2 offset;

	// Token: 0x040016AC RID: 5804
	private EdgeCollider2D edgeCollider;

	// Token: 0x040016AD RID: 5805
	private EdgeCollider2D secretPathEdgeCollider;
}
