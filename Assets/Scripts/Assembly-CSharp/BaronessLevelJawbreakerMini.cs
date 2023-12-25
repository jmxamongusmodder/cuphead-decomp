using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F3 RID: 1267
public class BaronessLevelJawbreakerMini : BaronessLevelMiniBossBase
{
	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06001632 RID: 5682 RVA: 0x000C741B File Offset: 0x000C581B
	// (set) Token: 0x06001633 RID: 5683 RVA: 0x000C7423 File Offset: 0x000C5823
	public BaronessLevelJawbreakerMini.State state { get; private set; }

	// Token: 0x06001634 RID: 5684 RVA: 0x000C742C File Offset: 0x000C582C
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.aim = base.transform;
		this.rotateDeath = false;
		this.bigPath = new List<Vector3>();
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x000C7460 File Offset: 0x000C5860
	protected override void Start()
	{
		base.Start();
		this.layerSwitch = 3;
		this.fadeTime = 2f;
		this.sprite.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 150;
		base.StartCoroutine(this.check_rotation_cr());
		base.StartCoroutine(this.switch_cr());
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x000C74D3 File Offset: 0x000C58D3
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x000C74F4 File Offset: 0x000C58F4
	public void Init(LevelProperties.Baroness.Jawbreaker properties, Vector2 pos, Transform targetPos, float rotationSpeed)
	{
		this.properties = properties;
		this.rotationSpeed = rotationSpeed;
		base.transform.position = pos;
		this.targetPosition = targetPos;
		this.state = BaronessLevelJawbreakerMini.State.Spawned;
		base.StartCoroutine(this.blink_cr());
		base.StartCoroutine(this.calculate_path_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x000C7556 File Offset: 0x000C5956
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x000C7570 File Offset: 0x000C5970
	private void FixedUpdate()
	{
		if (this.state == BaronessLevelJawbreakerMini.State.Spawned)
		{
			if (this.bigPath.Count != 0)
			{
				float num = this.pathLength / this.properties.jawbreakerMiniSpace;
				base.transform.position -= base.transform.right * (num * this.properties.jawbreakerHomingSpeed) * CupheadTime.FixedDelta;
				this.pathLength = 0f;
				this.aim.LookAt2D(2f * base.transform.position - this.bigPath[0]);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.aim.rotation, this.rotationSpeed * CupheadTime.FixedDelta);
				this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
				float num2 = Vector3.Distance(base.transform.position, this.bigPath[0]);
				if (num2 < this.properties.jawbreakerHomingSpeed / 4f)
				{
					this.bigPath.Remove(this.bigPath[0]);
				}
			}
			if (this.state == BaronessLevelJawbreakerMini.State.Dying && this.rotateDeath)
			{
				this.RotateExplode();
				this.rotateDeath = false;
			}
		}
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x000C76F8 File Offset: 0x000C5AF8
	private IEnumerator switch_cr()
	{
		base.StartCoroutine(this.fade_color_cr());
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.sprite.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 250;
		yield break;
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x000C7714 File Offset: 0x000C5B14
	private void Turn()
	{
		this.sprite.transform.SetScale(new float?(-this.sprite.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x000C7764 File Offset: 0x000C5B64
	private IEnumerator check_rotation_cr()
	{
		for (;;)
		{
			if (((this.targetPosition.transform.position.x < base.transform.position.x && !this.lookingLeft) || (this.targetPosition.transform.position.x > base.transform.position.x && this.lookingLeft)) && !this.isTurning)
			{
				this.isTurning = true;
				base.animator.SetTrigger("Turn");
				yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
				this.lookingLeft = !this.lookingLeft;
				this.isTurning = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x000C7780 File Offset: 0x000C5B80
	private IEnumerator move_cr()
	{
		for (;;)
		{
			this.pathLength = 0f;
			for (int i = 0; i < this.bigPath.Count - 1; i++)
			{
				this.pathLength += Vector3.Distance(this.bigPath[i], this.bigPath[i + 1]);
				if (CupheadTime.Delta == 0f)
				{
					yield return null;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x000C779C File Offset: 0x000C5B9C
	private IEnumerator calculate_path_cr()
	{
		for (;;)
		{
			for (int i = 0; i < this.positionsInList; i++)
			{
				if (!Mathf.Approximately(CupheadTime.Delta, 0f))
				{
					this.bigPath.Add(this.targetPosition.transform.position);
				}
				yield return new WaitForFixedUpdate();
			}
		}
		yield break;
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x000C77B7 File Offset: 0x000C5BB7
	public void Stop()
	{
		this.state = BaronessLevelJawbreakerMini.State.Dying;
		base.StopCoroutine(this.blink_cr());
		base.StopCoroutine(this.calculate_path_cr());
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x000C77D8 File Offset: 0x000C5BD8
	public void StartDying()
	{
		base.StartCoroutine(this.dying_cr());
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x000C77E8 File Offset: 0x000C5BE8
	private void RotateExplode()
	{
		float z = (float)UnityEngine.Random.Range(0, 360);
		base.transform.rotation = Quaternion.Euler(0f, 0f, z);
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x000C7820 File Offset: 0x000C5C20
	private IEnumerator dying_cr()
	{
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.SetTrigger("Dead");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.rotateDeath = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Death_End", false, true);
		this.KillMini();
		yield break;
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x000C783C File Offset: 0x000C5C3C
	private IEnumerator blink_cr()
	{
		while (this.state == BaronessLevelJawbreakerMini.State.Spawned)
		{
			base.animator.SetTrigger("Blink");
			int timeBetweenNext = UnityEngine.Random.Range(2, 4);
			yield return CupheadTime.WaitForSeconds(this, (float)timeBetweenNext);
		}
		yield break;
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x000C7858 File Offset: 0x000C5C58
	private void KillMini()
	{
		this.state = BaronessLevelJawbreakerMini.State.Unspawned;
		Collider2D component = base.GetComponent<Collider2D>();
		component.enabled = false;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001F7E RID: 8062
	private const float POSITION_FRAME_TIME = 0.083333336f;

	// Token: 0x04001F7F RID: 8063
	[SerializeField]
	private Transform sprite;

	// Token: 0x04001F81 RID: 8065
	private float rotationSpeed;

	// Token: 0x04001F82 RID: 8066
	private float pathLength;

	// Token: 0x04001F83 RID: 8067
	private int positionsInList = 12;

	// Token: 0x04001F84 RID: 8068
	private bool rotateDeath;

	// Token: 0x04001F85 RID: 8069
	private bool lookingLeft = true;

	// Token: 0x04001F86 RID: 8070
	private bool isTurning;

	// Token: 0x04001F87 RID: 8071
	private DamageDealer damageDealer;

	// Token: 0x04001F88 RID: 8072
	private LevelProperties.Baroness.Jawbreaker properties;

	// Token: 0x04001F89 RID: 8073
	private Vector3 currentPos;

	// Token: 0x04001F8A RID: 8074
	private Transform targetPosition;

	// Token: 0x04001F8B RID: 8075
	private Transform aim;

	// Token: 0x04001F8C RID: 8076
	private List<Vector3> bigPath;

	// Token: 0x04001F8D RID: 8077
	private Quaternion rotate;

	// Token: 0x020004F4 RID: 1268
	public enum State
	{
		// Token: 0x04001F8F RID: 8079
		Unspawned,
		// Token: 0x04001F90 RID: 8080
		Spawned,
		// Token: 0x04001F91 RID: 8081
		Dying
	}
}
