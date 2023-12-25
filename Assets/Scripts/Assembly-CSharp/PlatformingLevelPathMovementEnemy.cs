using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200086A RID: 2154
public class PlatformingLevelPathMovementEnemy : AbstractPlatformingLevelEnemy
{
	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x0600320B RID: 12811 RVA: 0x001D34D9 File Offset: 0x001D18D9
	// (set) Token: 0x0600320C RID: 12812 RVA: 0x001D34E1 File Offset: 0x001D18E1
	private protected float[] allValues { protected get; private set; }

	// Token: 0x0600320D RID: 12813 RVA: 0x001D34EC File Offset: 0x001D18EC
	public PlatformingLevelPathMovementEnemy Spawn(Vector3 position, VectorPath path, float startPosition, bool destroyEnemyAfterLeavingScreen)
	{
		PlatformingLevelPathMovementEnemy platformingLevelPathMovementEnemy = this.InstantiatePrefab<PlatformingLevelPathMovementEnemy>();
		platformingLevelPathMovementEnemy.transform.position = position;
		platformingLevelPathMovementEnemy.startPosition = startPosition;
		platformingLevelPathMovementEnemy.path = path;
		platformingLevelPathMovementEnemy._destroyEnemyAfterLeavingScreen = destroyEnemyAfterLeavingScreen;
		platformingLevelPathMovementEnemy._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant;
		return platformingLevelPathMovementEnemy;
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x0600320E RID: 12814 RVA: 0x001D352A File Offset: 0x001D192A
	protected virtual SpriteRenderer spriteRenderer
	{
		get
		{
			return this._spriteRenderer;
		}
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x0600320F RID: 12815 RVA: 0x001D3532 File Offset: 0x001D1932
	protected virtual Collider2D collider
	{
		get
		{
			return this._collider;
		}
	}

	// Token: 0x06003210 RID: 12816 RVA: 0x001D353C File Offset: 0x001D193C
	protected override void Start()
	{
		base.Start();
		this._offset = base.transform.position;
		this.MoveCallback(this.startPosition);
		this._collider = base.GetComponent<Collider2D>();
		this._spriteRenderer = base.GetComponent<SpriteRenderer>();
		if (base.Properties.MoveLoopMode == EnemyProperties.LoopMode.DelayAtPoint)
		{
			this.SetUp();
		}
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x001D359C File Offset: 0x001D199C
	protected override void OnStart()
	{
		this.hasStarted = true;
		switch (base.Properties.MoveLoopMode)
		{
		case EnemyProperties.LoopMode.PingPong:
			base.StartCoroutine(this.pingpong_cr());
			break;
		case EnemyProperties.LoopMode.Repeat:
			base.StartCoroutine(this.repeat_cr());
			break;
		case EnemyProperties.LoopMode.Once:
			base.StartCoroutine(this.once_cr());
			break;
		case EnemyProperties.LoopMode.DelayAtPoint:
			base.StartCoroutine(this.delay_at_point_cr());
			break;
		}
	}

	// Token: 0x06003212 RID: 12818 RVA: 0x001D3624 File Offset: 0x001D1A24
	protected override void Update()
	{
		base.Update();
		this.CalculateCollider();
		this.CalculateDirection();
		this.CalculateRender();
	}

	// Token: 0x06003213 RID: 12819 RVA: 0x001D3640 File Offset: 0x001D1A40
	private void CalculateRender()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position) && !this._enteredScreen)
		{
			this._enteredScreen = true;
		}
		if (this._enteredScreen && this._destroyEnemyAfterLeavingScreen && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 100f)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003214 RID: 12820 RVA: 0x001D36CD File Offset: 0x001D1ACD
	private void LateUpdate()
	{
		this.CalculateCollider();
		this.CalculateDirection();
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x001D36DC File Offset: 0x001D1ADC
	protected virtual void CalculateCollider()
	{
		if (this.collider == null || this.spriteRenderer == null || base.Dead)
		{
			return;
		}
		if (this.spriteRenderer.isVisible)
		{
			this.collider.enabled = true;
		}
		else
		{
			this.collider.enabled = false;
		}
	}

	// Token: 0x06003216 RID: 12822 RVA: 0x001D3744 File Offset: 0x001D1B44
	private void CalculateDirection()
	{
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Forward && this._hasFacingDirection)
		{
			this.spriteRenderer.flipX = true;
		}
		else
		{
			this.spriteRenderer.flipX = false;
		}
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x001D377A File Offset: 0x001D1B7A
	private void MoveCallback(float value)
	{
		base.transform.position = this._offset + this.path.Lerp(value);
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x001D37A0 File Offset: 0x001D1BA0
	private float CalculateRemainingTime(float t, PlatformingLevelPathMovementEnemy.Direction d)
	{
		float num = this.CalculateTime();
		return (d != PlatformingLevelPathMovementEnemy.Direction.Forward) ? (t * num) : ((1f - t) * num);
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x001D37CC File Offset: 0x001D1BCC
	private float CalculateTime()
	{
		return this.path.Distance / base.Properties.MoveSpeed;
	}

	// Token: 0x0600321A RID: 12826 RVA: 0x001D37F4 File Offset: 0x001D1BF4
	private float CalculatePartTime(int current, int next)
	{
		return Vector3.Distance(this.path.Points[current], this.path.Points[next]) / base.Properties.MoveSpeed;
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x001D3836 File Offset: 0x001D1C36
	private Coroutine Turn()
	{
		return base.StartCoroutine(this.turn_cr());
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x001D3844 File Offset: 0x001D1C44
	private IEnumerator turn_cr()
	{
		if (this._hasTurnAnimation && base.animator != null)
		{
			base.animator.Play("Turn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		}
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Forward)
		{
			this._direction = PlatformingLevelPathMovementEnemy.Direction.Back;
		}
		else
		{
			this._direction = PlatformingLevelPathMovementEnemy.Direction.Forward;
		}
		yield break;
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x001D3860 File Offset: 0x001D1C60
	private IEnumerator pingpong_cr()
	{
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Back)
		{
			yield return base.TweenValue(this.startPosition, 0f, this.CalculateRemainingTime(this.startPosition, PlatformingLevelPathMovementEnemy.Direction.Back), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
		}
		else
		{
			yield return base.TweenValue(this.startPosition, 1f, this.CalculateRemainingTime(this.startPosition, PlatformingLevelPathMovementEnemy.Direction.Forward), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
			yield return base.TweenValue(1f, 0f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
		}
		for (;;)
		{
			yield return base.TweenValue(0f, 1f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
			yield return base.TweenValue(1f, 0f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
		}
		yield break;
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x001D387C File Offset: 0x001D1C7C
	private IEnumerator repeat_cr()
	{
		float start = 0f;
		float end = 1f;
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Back)
		{
			start = 1f;
			end = 0f;
		}
		yield return base.TweenValue(this.startPosition, end, this.CalculateRemainingTime(this.startPosition, this._direction), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
		for (;;)
		{
			yield return base.TweenValue(start, end, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
		}
		yield break;
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x001D3898 File Offset: 0x001D1C98
	private void SetUp()
	{
		this._easeType = EaseUtils.EaseType.linear;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		this.allValues = new float[this.path.Points.Count];
		for (int i = 0; i < this.path.Points.Count; i++)
		{
			int index = (i != 0) ? (i - 1) : 0;
			float f = this.path.Points[i].y - this.path.Points[index].y;
			float f2 = this.path.Points[i].x - this.path.Points[index].x;
			float num4 = Mathf.Pow(f, 2f);
			float num5 = Mathf.Pow(f2, 2f);
			float f3 = num4 + num5;
			num3 += Mathf.Sqrt(f3);
		}
		for (int j = 0; j < this.path.Points.Count; j++)
		{
			num2 += num;
			int index2 = (j != 0) ? (j - 1) : 0;
			float f4 = this.path.Points[j].y - this.path.Points[index2].y;
			float f5 = this.path.Points[j].x - this.path.Points[index2].x;
			float num6 = Mathf.Pow(f4, 2f);
			float num7 = Mathf.Pow(f5, 2f);
			float f6 = num6 + num7;
			num = Mathf.Sqrt(f6);
			this.allValues[j] = (num2 + num) / num3;
		}
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x001D3A90 File Offset: 0x001D1E90
	private IEnumerator delay_at_point_cr()
	{
		float prevVal = this.startPosition;
		while (this.hasStarted)
		{
			yield return base.TweenValue(prevVal, this.allValues[this.pathIndex], this.CalculatePartTime(this.pathIndex - 1, this.pathIndex), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return null;
			if (this._hasTurnAnimation)
			{
				base.animator.SetTrigger("Turn");
				yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			}
			yield return null;
			if (this.pathIndex == this.path.Points.Count - 1)
			{
				break;
			}
			prevVal = this.allValues[this.pathIndex];
			this.pathIndex++;
			yield return null;
		}
		this.EndPath();
		yield return null;
		yield break;
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x001D3AAB File Offset: 0x001D1EAB
	protected virtual void EndPath()
	{
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x001D3AB0 File Offset: 0x001D1EB0
	private IEnumerator once_cr()
	{
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Back)
		{
			yield return base.TweenValue(this.startPosition, 0f, this.CalculateRemainingTime(this.startPosition, PlatformingLevelPathMovementEnemy.Direction.Back), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			this.Die();
		}
		else
		{
			yield return base.TweenValue(this.startPosition, 1f, this.CalculateRemainingTime(this.startPosition, PlatformingLevelPathMovementEnemy.Direction.Forward), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			this.Die();
		}
		yield break;
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x001D3ACB File Offset: 0x001D1ECB
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x001D3ADE File Offset: 0x001D1EDE
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x001D3AF4 File Offset: 0x001D1EF4
	private void DrawGizmos(float a)
	{
		if (Application.isPlaying)
		{
			this.path.DrawGizmos(a, this._offset);
			return;
		}
		this.path.DrawGizmos(a, base.baseTransform.position);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawSphere(this.path.Lerp(this.startPosition) + base.baseTransform.position, 10f);
		Gizmos.DrawWireSphere(this.path.Lerp(this.startPosition) + base.baseTransform.position, 11f);
	}

	// Token: 0x04003A6F RID: 14959
	protected int pathIndex;

	// Token: 0x04003A70 RID: 14960
	private const float SCREEN_PADDING = 100f;

	// Token: 0x04003A71 RID: 14961
	public float loopRepeatDelay;

	// Token: 0x04003A72 RID: 14962
	public float startPosition = 0.5f;

	// Token: 0x04003A73 RID: 14963
	public VectorPath path;

	// Token: 0x04003A74 RID: 14964
	[SerializeField]
	protected PlatformingLevelPathMovementEnemy.Direction _direction = PlatformingLevelPathMovementEnemy.Direction.Forward;

	// Token: 0x04003A75 RID: 14965
	[SerializeField]
	private bool _hasTurnAnimation;

	// Token: 0x04003A76 RID: 14966
	[SerializeField]
	private bool _hasFacingDirection;

	// Token: 0x04003A77 RID: 14967
	[SerializeField]
	private EaseUtils.EaseType _easeType = EaseUtils.EaseType.linear;

	// Token: 0x04003A78 RID: 14968
	protected Vector3 _offset;

	// Token: 0x04003A79 RID: 14969
	protected bool hasStarted;

	// Token: 0x04003A7A RID: 14970
	private SpriteRenderer _spriteRenderer;

	// Token: 0x04003A7B RID: 14971
	private Collider2D _collider;

	// Token: 0x04003A7C RID: 14972
	private bool _destroyEnemyAfterLeavingScreen;

	// Token: 0x04003A7D RID: 14973
	private bool _enteredScreen;

	// Token: 0x0200086B RID: 2155
	public enum Direction
	{
		// Token: 0x04003A7F RID: 14975
		Forward = 1,
		// Token: 0x04003A80 RID: 14976
		Back = -1
	}
}
