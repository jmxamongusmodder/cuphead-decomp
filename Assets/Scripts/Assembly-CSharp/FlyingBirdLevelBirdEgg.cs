using System;
using UnityEngine;

// Token: 0x02000618 RID: 1560
public class FlyingBirdLevelBirdEgg : AbstractProjectile
{
	// Token: 0x06001FB5 RID: 8117 RVA: 0x00123870 File Offset: 0x00121C70
	public virtual AbstractProjectile Create(float speed, Vector2 pos)
	{
		FlyingBirdLevelBirdEgg flyingBirdLevelBirdEgg = this.Create(pos, 0f) as FlyingBirdLevelBirdEgg;
		flyingBirdLevelBirdEgg.speed = -speed;
		flyingBirdLevelBirdEgg.CollisionDeath.OnlyPlayer();
		flyingBirdLevelBirdEgg.DamagesType.OnlyPlayer();
		return flyingBirdLevelBirdEgg;
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x001238B0 File Offset: 0x00121CB0
	protected override void Start()
	{
		base.Start();
		Level.Mode mode = Level.Current.mode;
		if (mode != Level.Mode.Easy)
		{
			if (mode != Level.Mode.Normal)
			{
				if (mode == Level.Mode.Hard)
				{
					this.maxProjectiles = 5;
				}
			}
			else
			{
				this.maxProjectiles = 3;
			}
		}
		else
		{
			this.maxProjectiles = 2;
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x0012390C File Offset: 0x00121D0C
	protected override void Update()
	{
		base.Update();
		base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
		if (this.state == FlyingBirdLevelBirdEgg.State.Idle && base.transform.position.x < -640f)
		{
			this.Explode();
			this.Die();
		}
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x0012398E File Offset: 0x00121D8E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x001239B4 File Offset: 0x00121DB4
	private void Explode()
	{
		AudioManager.Play("level_flying_bird_egg_explode");
		this.emitAudioFromObject.Add("level_flying_bird_egg_explode");
		AudioManager.Play("level_flying_bird_egg_break");
		this.emitAudioFromObject.Add("level_flying_bird_egg_break");
		if (this.state != FlyingBirdLevelBirdEgg.State.Idle)
		{
			return;
		}
		this.state = FlyingBirdLevelBirdEgg.State.Exploded;
		this.effectPrefab.Create(base.transform.position);
		if (this.maxProjectiles == 0)
		{
			return;
		}
		Vector3 position = base.transform.position;
		position.x += 42f;
		if (this.maxProjectiles == 2)
		{
			this.childPrefab.Create(position, 90f, Vector2.one, -this.speed);
			this.childPrefab.Create(position, -90f, Vector2.one, -this.speed);
		}
		else
		{
			for (int i = 0; i < this.maxProjectiles; i++)
			{
				float rotation;
				switch (i)
				{
				default:
					rotation = 0f;
					break;
				case 1:
					rotation = -45f;
					break;
				case 2:
					rotation = 45f;
					break;
				case 3:
					rotation = 90f;
					break;
				case 4:
					rotation = -90f;
					break;
				}
				this.childPrefab.Create(position, rotation, Vector2.one, -this.speed);
			}
		}
	}

	// Token: 0x0400283E RID: 10302
	private const float ANGLE = 45f;

	// Token: 0x0400283F RID: 10303
	[SerializeField]
	private BasicProjectile childPrefab;

	// Token: 0x04002840 RID: 10304
	[SerializeField]
	private Effect effectPrefab;

	// Token: 0x04002841 RID: 10305
	private float speed;

	// Token: 0x04002842 RID: 10306
	private FlyingBirdLevelBirdEgg.State state;

	// Token: 0x04002843 RID: 10307
	private int maxProjectiles;

	// Token: 0x02000619 RID: 1561
	private enum State
	{
		// Token: 0x04002845 RID: 10309
		Idle,
		// Token: 0x04002846 RID: 10310
		Exploded
	}
}
