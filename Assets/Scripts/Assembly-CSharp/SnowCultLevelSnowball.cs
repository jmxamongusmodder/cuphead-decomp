using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
public class SnowCultLevelSnowball : AbstractProjectile
{
	// Token: 0x06002EC7 RID: 11975 RVA: 0x001B98FC File Offset: 0x001B7CFC
	public virtual SnowCultLevelSnowball Init(Vector3 pos, float gravity, float verticalVelocity, float horizontalVelocity, LevelProperties.SnowCult.Snowball properties, SnowCultLevelYeti main, bool makeSound)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.properties = properties;
		this.gravity = gravity;
		this.velocity.x = -horizontalVelocity;
		this.velocity.y = verticalVelocity;
		base.transform.localScale = new Vector3(Mathf.Sign(horizontalVelocity), 1f);
		this.hitGround = false;
		this.main = main;
		this.makeSound = makeSound;
		base.animator.Play("Spin", 0, main.GetIceCubeStartFrame() * 0.0625f);
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x001B99AC File Offset: 0x001B7DAC
	public virtual SnowCultLevelSnowball InitOriginal(Vector3 pos, float gravity, float speed, float angle, LevelProperties.SnowCult.Snowball properties, SnowCultLevelYeti main)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.speed = speed;
		base.transform.localScale = new Vector3(Mathf.Sign(angle - 90f), 1f);
		this.gravity = gravity;
		this.angle = MathUtils.AngleToDirection(angle);
		this.properties = properties;
		this.hitGround = false;
		this.main = main;
		this.makeSound = true;
		base.animator.Play("Spin", 0, main.GetIceCubeStartFrame() * 0.0625f);
		base.StartCoroutine(this.move_from_yeti_cr());
		return this;
	}

	// Token: 0x06002EC9 RID: 11977 RVA: 0x001B9A5D File Offset: 0x001B7E5D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002ECA RID: 11978 RVA: 0x001B9A7B File Offset: 0x001B7E7B
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		this.SFX_SNOWCULT_IceCubeImpact();
		this.hitGround = true;
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x001B9A94 File Offset: 0x001B7E94
	private void TriggerGlare()
	{
		if (this.glareCounter == 0)
		{
			this.glares[0].enabled = false;
			this.glares[1].enabled = false;
		}
		this.glareCounter++;
		if (this.glareCounter == 3)
		{
			this.glares[0].enabled = true;
			this.glares[1].enabled = true;
			this.glareCounter = 0;
		}
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x001B9B08 File Offset: 0x001B7F08
	private IEnumerator move_from_yeti_cr()
	{
		float accumulativeGravity = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!this.hitGround)
		{
			base.transform.position += this.angle * this.speed * CupheadTime.FixedDelta - new Vector3(0f, accumulativeGravity * CupheadTime.FixedDelta, 0f);
			accumulativeGravity += this.gravity * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.newProjectiles();
		this.Recycle<SnowCultLevelSnowball>();
		yield return null;
		yield break;
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x001B9B24 File Offset: 0x001B7F24
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!this.hitGround)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.newProjectiles();
		this.Recycle<SnowCultLevelSnowball>();
		yield return null;
		yield break;
	}

	// Token: 0x06002ECE RID: 11982 RVA: 0x001B9B40 File Offset: 0x001B7F40
	private void newProjectiles()
	{
		SnowCultLevelSnowballExplosion snowCultLevelSnowballExplosion = this.snowballExplosion.Spawn<SnowCultLevelSnowballExplosion>();
		snowCultLevelSnowballExplosion.Init(base.transform.position, this.size, this.main);
		float d = Time.realtimeSinceStartup % 0.0001f;
		if (this.size == SnowCultLevelSnowball.Size.Large)
		{
			SnowCultLevelSnowball snowCultLevelSnowball = this.mediumSnowballPrefab.Spawn<SnowCultLevelSnowball>();
			snowCultLevelSnowball.Init(base.transform.position + Vector3.back * d, this.properties.mediumGravity, this.properties.mediumVelocityY, this.properties.mediumVelocityX, this.properties, this.main, false);
			SnowCultLevelSnowball snowCultLevelSnowball2 = this.mediumSnowballPrefab.Spawn<SnowCultLevelSnowball>();
			snowCultLevelSnowball2.Init(base.transform.position + Vector3.forward * d, this.properties.mediumGravity, this.properties.mediumVelocityY, -this.properties.mediumVelocityX, this.properties, this.main, true);
		}
		else if (this.size == SnowCultLevelSnowball.Size.Medium)
		{
			SnowCultLevelSnowball snowCultLevelSnowball3 = this.smallSnowballPrefab.Spawn<SnowCultLevelSnowball>();
			snowCultLevelSnowball3.Init(base.transform.position + Vector3.back * d, this.properties.smallGravity, this.properties.smallVelocityY, this.properties.smallVelocityX, this.properties, this.main, true);
			SnowCultLevelSnowball snowCultLevelSnowball4 = this.smallSnowballPrefab.Spawn<SnowCultLevelSnowball>();
			snowCultLevelSnowball4.Init(base.transform.position + Vector3.forward * d, this.properties.smallGravity, this.properties.smallVelocityY, -this.properties.smallVelocityX, this.properties, this.main, false);
		}
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x001B9D18 File Offset: 0x001B8118
	private void SFX_SNOWCULT_IceCubeImpact()
	{
		if (!this.makeSound)
		{
			return;
		}
		string str = "_large";
		if (this.size == SnowCultLevelSnowball.Size.Medium)
		{
			str = "_medium";
		}
		if (this.size == SnowCultLevelSnowball.Size.Small)
		{
			str = "_small";
		}
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_impact" + str);
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_impact" + str);
	}

	// Token: 0x04003771 RID: 14193
	[SerializeField]
	private SnowCultLevelSnowball smallSnowballPrefab;

	// Token: 0x04003772 RID: 14194
	[SerializeField]
	private SnowCultLevelSnowball mediumSnowballPrefab;

	// Token: 0x04003773 RID: 14195
	[SerializeField]
	private SnowCultLevelSnowballExplosion snowballExplosion;

	// Token: 0x04003774 RID: 14196
	public SnowCultLevelSnowball.Size size;

	// Token: 0x04003775 RID: 14197
	private LevelProperties.SnowCult.Snowball properties;

	// Token: 0x04003776 RID: 14198
	private Vector3 velocity;

	// Token: 0x04003777 RID: 14199
	private float gravity;

	// Token: 0x04003778 RID: 14200
	private float speed;

	// Token: 0x04003779 RID: 14201
	private bool hitGround;

	// Token: 0x0400377A RID: 14202
	private bool makeSound;

	// Token: 0x0400377B RID: 14203
	private Vector3 angle;

	// Token: 0x0400377C RID: 14204
	[SerializeField]
	private SpriteRenderer[] glares;

	// Token: 0x0400377D RID: 14205
	private int glareCounter = 1;

	// Token: 0x0400377E RID: 14206
	private SnowCultLevelYeti main;

	// Token: 0x020007F5 RID: 2037
	public enum Size
	{
		// Token: 0x04003780 RID: 14208
		Small,
		// Token: 0x04003781 RID: 14209
		Medium,
		// Token: 0x04003782 RID: 14210
		Large
	}
}
