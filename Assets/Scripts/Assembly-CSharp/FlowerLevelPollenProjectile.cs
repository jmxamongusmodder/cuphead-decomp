using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000611 RID: 1553
public class FlowerLevelPollenProjectile : BasicProjectile
{
	// Token: 0x06001F51 RID: 8017 RVA: 0x0011FC30 File Offset: 0x0011E030
	public void InitPollen(float speed, float strength, int type, Transform target)
	{
		this.pct = 0f;
		this.time = 0.7795515f;
		this.manual = true;
		this.speed = -speed;
		this.waveStrength = strength;
		this.target = target;
		this.Speed = 0f;
		this.move = false;
		if (type == 1)
		{
			this.SetParryable(true);
			base.animator.Play("Pink_Idle");
		}
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.spawn_petals_cr(type));
	}

	// Token: 0x06001F52 RID: 8018 RVA: 0x0011FCBC File Offset: 0x0011E0BC
	public void StartMoving()
	{
		this.manual = false;
		this.move = true;
		this.Speed = this.speed;
		this.initPosY = base.transform.position.y;
	}

	// Token: 0x06001F53 RID: 8019 RVA: 0x0011FCFC File Offset: 0x0011E0FC
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x06001F54 RID: 8020 RVA: 0x0011FD14 File Offset: 0x0011E114
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (CupheadTime.GlobalSpeed != 0f)
			{
				if (!this.manual)
				{
					Vector3 position = base.transform.position;
					position.y = this.initPosY + Mathf.Sin(this.time * 6f) * (this.waveStrength * this.pct) * CupheadTime.GlobalSpeed;
					base.transform.position = position;
					if (this.pct < 1f)
					{
						this.pct += CupheadTime.FixedDelta * 2f;
					}
					else
					{
						this.pct = 1f;
					}
				}
				else
				{
					base.transform.position = this.target.position;
					this.Speed = 0f;
				}
				this.time += CupheadTime.FixedDelta;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001F55 RID: 8021 RVA: 0x0011FD30 File Offset: 0x0011E130
	private IEnumerator rotate_cr()
	{
		float val = (!Rand.Bool()) ? 420f : -420f;
		float frameTime = 0f;
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				frameTime -= 0.041666668f;
				this.sprite.transform.Rotate(0f, 0f, val * CupheadTime.Delta);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F56 RID: 8022 RVA: 0x0011FD4C File Offset: 0x0011E14C
	private IEnumerator spawn_petals_cr(int type)
	{
		for (;;)
		{
			if (type == 1)
			{
				this.petalPink.Create(base.transform.position);
			}
			else
			{
				this.petal.Create(base.transform.position);
			}
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.2f, 1f));
		}
		yield break;
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x0011FD70 File Offset: 0x0011E170
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			CupheadRenderer instance = CupheadRenderer.Instance;
			instance.TouchFuzzy(15f, 8f, 1.2f);
			base.GetComponent<AudioWarble>().HandleWarble();
		}
	}

	// Token: 0x06001F58 RID: 8024 RVA: 0x0011FDB2 File Offset: 0x0011E1B2
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001F59 RID: 8025 RVA: 0x0011FDC5 File Offset: 0x0011E1C5
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.petal = null;
		this.petalPink = null;
	}

	// Token: 0x040027EB RID: 10219
	private const float ROTATE_FRAME_TIME = 0.041666668f;

	// Token: 0x040027EC RID: 10220
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x040027ED RID: 10221
	[SerializeField]
	private FlowerLevelPollenPetal petalPink;

	// Token: 0x040027EE RID: 10222
	[SerializeField]
	private FlowerLevelPollenPetal petal;

	// Token: 0x040027EF RID: 10223
	private bool manual;

	// Token: 0x040027F0 RID: 10224
	private float time;

	// Token: 0x040027F1 RID: 10225
	private float speed;

	// Token: 0x040027F2 RID: 10226
	private float waveStrength;

	// Token: 0x040027F3 RID: 10227
	private float initPosY;

	// Token: 0x040027F4 RID: 10228
	private Transform target;

	// Token: 0x040027F5 RID: 10229
	private float pct;
}
